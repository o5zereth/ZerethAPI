namespace ZerethAPI.Extensions;

using System.Collections.Generic;

/// <summary>
/// An extension class for collections.
/// </summary>
public static class CollectionExtensions
{
	/// <summary>
	/// Toggles the specified value from being in the hashset.
	/// </summary>
	/// <typeparam name="T">The type of elements in the hashset.</typeparam>
	/// <param name="hashSet">The hashset to modify.</param>
	/// <param name="value">The value to toggle.</param>
	/// <returns>A value indicating whether the value is in the hashset after the operation.</returns>
	public static bool Toggle<T>(this HashSet<T> hashSet, T value)
	{
		bool added;

		if (!(added = hashSet.Add(value)))
		{
			hashSet.Remove(value);
		}

		return added;
	}
}
