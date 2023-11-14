namespace ZerethAPI.Utils.Patches;

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using ZerethAPI.Utils.Transpiler;

internal static class SafetyPatch
{
	public static HarmonyMethod TranspilerMethod => new(typeof(SafetyPatch), nameof(Transpiler));

	internal static Action<Exception> HandleException;

	private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions, MethodBase method, ILGenerator generator)
	{
		if (method is not MethodInfo info)
		{
			throw new InvalidCastException($"Could not convert method to MethodInfo. This is required to determine the return type of the method.");
		}

		if (HandleException is null)
		{
			throw new InvalidOperationException($"The exception handler delegate is null.");
		}

		if (!HandleException.Method.IsStatic)
		{
			throw new InvalidOperationException($"The exception handler delegate must target a static method.");
		}

		instructions.BeginTranspiler(out List<CodeInstruction> newInstructions);

		// Behaviour changes depending on whether the method returns void.
		if (info.ReturnType == typeof(void))
		{
			HandleVoidReturn(newInstructions, generator);
		}
		else
		{
			HandleReturn(newInstructions, generator, info.ReturnType);
		}

		return newInstructions.FinishTranspiler();
	}

	private static void HandleVoidReturn(List<CodeInstruction> instructions, ILGenerator generator)
	{
		Label exitExceptionBlock = generator.DefineLabel();

		instructions[0].blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock));

		// Catch the exception, call the delegate, then return.
		instructions.AddRange(new CodeInstruction[]
		{
				new CodeInstruction(OpCodes.Call, HandleException.Method)
					.WithBlocks(new ExceptionBlock(ExceptionBlockType.BeginCatchBlock, typeof(Exception))),
				new CodeInstruction(OpCodes.Leave_S, exitExceptionBlock)
					.WithBlocks(new ExceptionBlock(ExceptionBlockType.EndExceptionBlock)),

				new CodeInstruction(OpCodes.Ret)
					.WithLabels(exitExceptionBlock),
		});

		// Replace ret instructions with leave instructions.
		for (int i = 0; i < instructions.Count - 1; i++)
		{
			CodeInstruction code = instructions[i];

			if (code.opcode != OpCodes.Ret)
				continue;

			code.opcode = OpCodes.Leave_S;
			code.operand = exitExceptionBlock;
		}
	}

	private static void HandleReturn(List<CodeInstruction> instructions, ILGenerator generator, Type returnType)
	{
		Label exitExceptionBlock = generator.DefineLabel();
		LocalBuilder returnLocal = generator.DeclareLocal(returnType);

		instructions[0].blocks.Add(new ExceptionBlock(ExceptionBlockType.BeginExceptionBlock));

		// Catch the exception, call the delegate, assign the result to default, then return.
		instructions.AddRange(new CodeInstruction[]
		{
				new CodeInstruction(OpCodes.Call, HandleException.Method)
					.WithBlocks(new ExceptionBlock(ExceptionBlockType.BeginCatchBlock, typeof(Exception))),

				// returnLocal = default;
				new(OpCodes.Ldloca_S, returnLocal),
				new(OpCodes.Initobj, returnType),

				new CodeInstruction(OpCodes.Leave_S, exitExceptionBlock)
					.WithBlocks(new ExceptionBlock(ExceptionBlockType.EndExceptionBlock)),

				// return returnLocal;
				new CodeInstruction(OpCodes.Ldloc_S, returnLocal)
					.WithLabels(exitExceptionBlock),
				new(OpCodes.Ret),
		});

		// Replace ret instructions with leave 
		// instructions and assign the return local.
		for (int i = 0; i < instructions.Count - 1; i++)
		{
			CodeInstruction code = instructions[i];

			if (code.opcode != OpCodes.Ret)
				continue;

			code.opcode = OpCodes.Stloc_S;
			code.operand = returnLocal;

			instructions.Insert(++i, new CodeInstruction(OpCodes.Leave_S, exitExceptionBlock));
		}

	}
}
