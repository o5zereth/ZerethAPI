namespace ZerethAPI.Utils.Transpiler;

using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

/// <summary>
/// A utility class to create common <see cref="CodeInstruction"/> snippets.
/// </summary>
public static class InstructionHelper
{
    /// <summary>
    /// Creates a branch instruction with the specified label and form.
    /// </summary>
    /// <param name="type">The branch type of the instruction to be created.</param>
    /// <param name="label">The label that will be branched to in the instruction.</param>
    /// <param name="shortForm">Specifies whether the branch instruction should use short form (8-bit) instead of (32-bit).</param>
    /// <returns>A branch instruction that branches to the specified label.</returns>
    /// <exception cref="ArgumentException">Thrown when using <see cref="BranchType.Switch"/> or an unnamed enum value for the first argument.</exception>
    public static CodeInstruction Branch(BranchType type, Label label, bool shortForm = true)
    {
        OpCode opCode = type switch
        {
            BranchType.Br => shortForm ? OpCodes.Br_S : OpCodes.Br,
            BranchType.Leave => shortForm ? OpCodes.Leave_S : OpCodes.Leave,
            BranchType.Brfalse => shortForm ? OpCodes.Brfalse_S : OpCodes.Brfalse,
            BranchType.Brtrue => shortForm ? OpCodes.Brtrue_S : OpCodes.Brtrue,
            BranchType.Switch => throw new ArgumentException("Creating an instruction of type 'Switch' is not allowed in this method. Use InstructionHelper::Switch method instead.", nameof(type)),
            BranchType.Beq => shortForm ? OpCodes.Beq_S : OpCodes.Beq,
            BranchType.Bge => shortForm ? OpCodes.Bge_S : OpCodes.Bge,
            BranchType.Bgt => shortForm ? OpCodes.Bgt_S : OpCodes.Bgt,
            BranchType.Ble => shortForm ? OpCodes.Ble_S : OpCodes.Ble,
            BranchType.Blt => shortForm ? OpCodes.Blt_S : OpCodes.Blt,
            BranchType.Bne_Un => shortForm ? OpCodes.Bne_Un_S : OpCodes.Bne_Un,
            BranchType.Bge_Un => shortForm ? OpCodes.Bge_Un_S : OpCodes.Bge_Un,
            BranchType.Bgt_Un => shortForm ? OpCodes.Bgt_Un_S : OpCodes.Bgt_Un,
            BranchType.Ble_Un => shortForm ? OpCodes.Ble_Un_S : OpCodes.Ble_Un,
            BranchType.Blt_Un => shortForm ? OpCodes.Blt_Un_S : OpCodes.Blt_Un,

            _ => throw new ArgumentException("Enum value must be named.", nameof(type)),
        };

        return new CodeInstruction(opCode, label);
    }

    /// <summary>
    /// Creates a switch instruction with the specified labels.
    /// </summary>
    /// <param name="labels">The labels this switch instruction will have.</param>
    /// <returns>A switch code instruction branching to the specified labels, in order.</returns>
    public static CodeInstruction Switch(params Label[] labels)
	{
		// Note: Switch instructions with zero labels is valid in CIL,
		//       so theres no need to worry about arguments.
		return new CodeInstruction(OpCodes.Switch, labels);
	}

	/// <summary>
	/// Creates a switch instruction with the specified labels.
	/// </summary>
	/// <param name="labels">The labels this switch instruction will have.</param>
	/// <returns>A switch code instruction branching to the specified labels, in order.</returns>
	public static CodeInstruction Switch(ICollection<Label> labels)
	{
		Label[] arr = new Label[labels.Count];
		labels.CopyTo(arr, 0);

		// Note: Switch instructions with zero labels is valid in CIL,
		//       so theres no need to worry about arguments.
		return new CodeInstruction(OpCodes.Switch, arr);
	}

	/// <summary>
	/// Creates a switch instruction with the specified labels.
	/// </summary>
	/// <param name="labels">The labels this switch instruction will have.</param>
	/// <returns>A switch code instruction branching to the specified labels, in order.</returns>
	public static CodeInstruction Switch(IEnumerable<Label> labels)
	{
		if (labels is Label[] arr)
		{
			return Switch(arr);
		}

		if (labels is ICollection<Label> coll)
		{
			return Switch(coll);
		}

		// Note: Switch instructions with zero labels is valid in CIL,
		//       so theres no need to worry about arguments.
		return new CodeInstruction(OpCodes.Switch, labels.ToArray());
	}

	/// <summary>
	/// Creates an instruction that loads the argument at the specified index onto the stack.
	/// </summary>
	/// <param name="index">The index of the argument to push onto the stack.</param>
	/// <returns>An instruction that loads the argument at the specified index onto the stack.</returns>
	public static CodeInstruction LoadArg(ushort index)
	{
		return index switch
		{
			0 => new(OpCodes.Ldarg_0),
			1 => new(OpCodes.Ldarg_1),
			2 => new(OpCodes.Ldarg_2),
			3 => new(OpCodes.Ldarg_3),

			_ => index <= byte.MaxValue
				? new(OpCodes.Ldarg_S, (byte)index)
				: new(OpCodes.Ldarg, index),
		};
	}

	/// <summary>
	/// Creates an instruction that loads the argument address at the specified index onto the stack.
	/// </summary>
	/// <param name="index">The index of the argument address to push onto the stack.</param>
	/// <returns>An instruction that loads the argument address at the specified index onto the stack.</returns>
	public static CodeInstruction LoadArgAddr(ushort index)
	{
		return index <= byte.MaxValue
			? new(OpCodes.Ldarga_S, (byte)index)
			: new(OpCodes.Ldarga, index);
	}

	/// <summary>
	/// Creates an instruction that loads the local at the specified index onto the stack.
	/// </summary>
	/// <param name="index">The index of the local to push onto the stack.</param>
	/// <returns>An instruction that loads the local at the specified index onto the stack.</returns>
	public static CodeInstruction LoadLoc(ushort index)
	{
		return index switch
		{
			0 => new(OpCodes.Ldloc_0),
			1 => new(OpCodes.Ldloc_1),
			2 => new(OpCodes.Ldloc_2),
			3 => new(OpCodes.Ldloc_3),

			_ => index <= byte.MaxValue
				? new(OpCodes.Ldloc_S, (byte)index)
				: new(OpCodes.Ldloc, index),
		};
	}

	/// <summary>
	/// Creates an instruction that loads the local address at the specified index onto the stack.
	/// </summary>
	/// <param name="index">The index of the local address to push onto the stack.</param>
	/// <returns>An instruction that loads the local address at the specified index onto the stack.</returns>
	public static CodeInstruction LoadLocAddr(ushort index)
	{
		return index <= byte.MaxValue
			? new(OpCodes.Ldloca_S, (byte)index)
			: new(OpCodes.Ldloca, index);
	}

	/// <summary>
	/// Creates an instruction that loads the specified local onto the stack.
	/// </summary>
	/// <param name="local">The local to push onto the stack.</param>
	/// <returns>An instruction that loads the specified local onto the stack.</returns>
	public static CodeInstruction LoadLoc(LocalBuilder local)
	{
		return LoadLoc((ushort)local.LocalIndex);
	}

	/// <summary>
	/// Creates an instruction that loads the specified local address onto the stack.
	/// </summary>
	/// <param name="local">The local whose address is to be pushed onto the stack.</param>
	/// <returns>An instruction that loads the specified local address onto the stack.</returns>
	public static CodeInstruction LoadLocAddr(LocalBuilder local)
	{
		return LoadLocAddr((ushort)local.LocalIndex);
	}

	/// <summary>
	/// Creates an array with new labels.
	/// </summary>
	/// <param name="generator">The generator to use when defining labels.</param>
	/// <param name="count">The number of labels to create.</param>
	/// <returns>An array containing new labels with the specified amount.</returns>
	public static Label[] CreateLabels(ILGenerator generator, uint count)
	{
		Label[] arr = new Label[count];

		for (uint i = 0; i < count;)
		{
			arr[i++] = generator.DefineLabel();
		}

		return arr;
	}
}
