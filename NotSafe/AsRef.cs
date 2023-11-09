namespace ZerethAPI.NotSafe;

using System.Runtime.CompilerServices;

/// <summary>
/// A struct representing a pointer.
/// </summary>
/// <typeparam name="T">The type to represent at the underlying pointer value.</typeparam>
public unsafe readonly struct AsRef<T>
	where T : unmanaged
{
	private readonly void* ptr;

	/// <summary>
	/// Creates an instance of the <see cref="AsRef{T}"/> struct.
	/// </summary>
	/// <param name="reference">The reference of the underlying pointer.</param>
	public AsRef(ref T reference) => this.ptr = Unsafe.AsPointer(ref reference);

	/// <summary>
	/// Creates an instance of the <see cref="AsRef{T}"/> struct.
	/// </summary>
	/// <param name="pointer">The underlying pointer.</param>
	public AsRef(void* pointer) => this.ptr = pointer;

	/// <summary>
	/// Gets the underlying pointer value of this instance.
	/// </summary>
	public void* Pointer => this.ptr;

	/// <summary>
	/// Gets the reference to the underlying value.
	/// </summary>
	public ref T Reference => ref Unsafe.AsRef<T>(this.ptr);

	/// <summary>
	/// Gets a value indicating whether this instance represents a null reference.
	/// </summary>
	public bool IsNullReference => this.ptr is null;

	/// <summary>
	/// Converts the current instance to the specified generic type.
	/// </summary>
	/// <typeparam name="NewT">The new generic type to convert to.</typeparam>
	/// <returns>A new instance of the specified generic type.</returns>
	public AsRef<NewT> Convert<NewT>()
		where NewT : unmanaged
	{
		return new AsRef<NewT>(this.ptr);
	}

	/// <summary>
	/// Assigns the value at the underlying pointer to the one provided.
	/// </summary>
	/// <param name="value"></param>
	public void AssignValue(T value)
	{
		ref T reference = ref Unsafe.AsRef<T>(this.ptr);
		reference = value;
	}

	/// <summary>
	/// Adds the specified number to the pointer offset.
	/// </summary>
	/// <param name="left">The left argument.</param>
	/// <param name="right">The right argument.</param>
	/// <returns>A new instance with the pointer offset by the specified value.</returns>
	public static AsRef<T> operator +(AsRef<T> left, int right)
	{
		return new((void*)((int)left.ptr + right));
	}

	/// <summary>
	/// Subtracts the specified number from the pointer offset.
	/// </summary>
	/// <param name="left">The left argument.</param>
	/// <param name="right">The right argument.</param>
	/// <returns>A new instance with the pointer offset by the specified value.</returns>
	public static AsRef<T> operator -(AsRef<T> left, int right)
	{
		return new((void*)((int)left.ptr - right));
	}

	/// <summary>
	/// Converts the specified instance to an integer.
	/// </summary>
	/// <param name="reference">The reference to convert to an integer.</param>
	public static explicit operator int(AsRef<T> reference)
	{
		return (int)reference.ptr;
	}
}
