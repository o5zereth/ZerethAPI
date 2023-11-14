namespace ZerethAPI.Utils;

using HarmonyLib;
using System;
using System.Reflection;
using ZerethAPI.Utils.Patches;

/// <summary>
/// A utility class to assist in HarmonyLib patching.
/// </summary>
public static class PatchHelper
{
	/// <summary>
	/// Applies a safety patch onto a method, injecting a try-catch block.
	/// </summary>
	/// <param name="harmony">The harmony instance to apply this patch with.</param>
	/// <param name="type">The type of the method to patch.</param>
	/// <param name="name">The name of the method to patch.</param>
	/// <param name="handler">The handler to invoke upon an exception in the specified method.</param>
	/// <exception cref="NullReferenceException"/>
	public static void ApplySafetyPatch(this Harmony harmony, Type type, string name, Action<Exception> handler)
	{
		MethodInfo method = AccessTools.Method(type, name) ?? throw new NullReferenceException("Method could not be found.");

		harmony.ApplySafetyPatch(method, handler);
	}

	/// <summary>
	/// Applies a safety patch onto a method, injecting a try-catch block.
	/// </summary>
	/// <param name="harmony">The harmony instance to apply this patch with.</param>
	/// <param name="method">The method to patch.</param>
	/// <param name="handler">The handler to invoke upon an exception in the specified method.</param>
	/// <exception cref="ArgumentNullException"/>
	public static void ApplySafetyPatch(this Harmony harmony, MethodInfo method, Action<Exception> handler)
	{
		if (handler is null)
		{
			throw new ArgumentNullException(nameof(handler));
		}

		SafetyPatch.HandleException = handler;

		harmony.Patch(method, null, null, SafetyPatch.TranspilerMethod, null);
	}
}
