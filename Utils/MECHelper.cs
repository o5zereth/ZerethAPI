namespace ZerethAPI.Utils;

using MEC;
using System;
using System.Collections;
using System.Collections.Generic;
using ZerethAPI.Safe;

/// <summary>
/// A utility class to assist in the usage of MEC Coroutines.
/// </summary>
public static class MECHelper
{
	/// <summary>
	/// Converts the specified coroutine enumerator to a safer version with exception handling.
	/// </summary>
	/// <param name="enumerator">The coroutine enumerator to make safe.</param>
	/// <param name="onException">The action to invoke upon an exception being thrown.</param>
	/// <returns>A safe version of the provided coroutine enumerator.</returns>
	public static IEnumerator<float> Safe(this IEnumerator<float> enumerator, Action<Exception> onException = null)
	{
		return onException is null
			? new SafeEnumeratorIgnoreException<float>(enumerator)
			: new SafeEnumerator<float>(enumerator, onException);
	}

	/// <summary>
	/// Kills the coroutine at the specified reference, and assigns it to the default value.
	/// </summary>
	/// <param name="handle">The handle reference to kill.</param>
	public static void Kill(ref this CoroutineHandle handle)
	{
		Timing.KillCoroutines(handle);
		handle = default;
	}

	/// <summary>
	/// Kills the specified coroutine handle.
	/// </summary>
	/// <param name="handle">The handle to kill.</param>
	public static void KillHandle(this CoroutineHandle handle)
	{
		Timing.KillCoroutines(handle);
	}

	/// <summary>
	/// Kills the coroutine at the specified reference, and assigns it to the specified value.
	/// </summary>
	/// <param name="handle">The handle reference to kill.</param>
	/// <param name="newHandle">The new handle to assign to the specified reference.</param>
	/// <returns>The newly assigned coroutine handle instance.</returns>
	public static CoroutineHandle KillAssignNew(ref this CoroutineHandle handle, CoroutineHandle newHandle)
	{
		Timing.KillCoroutines(handle);
		return handle = newHandle;
	}

	/// <summary>
	/// Runs the specified coroutine after a number of frames.
	/// </summary>
	/// <param name="enumerator">The underlying coroutine.</param>
	/// <param name="frameWait">The number of frames to wait.</param>
	/// <returns></returns>
	public static IEnumerator<float> RunAfterFrames(this IEnumerator<float> enumerator, ushort frameWait)
	{
		return new RunCoroutineAfterFramesEnumerator(frameWait, enumerator);
	}

	/// <summary>
	/// An enumerator that allows for running a coroutine after waiting the specified number of frames.
	/// </summary>
	public struct RunCoroutineAfterFramesEnumerator : IEnumerator<float>
	{
		private ushort state;
		private readonly ushort initialState;
		private readonly IEnumerator<float> enumerator;

		/// <inheritdoc/>
		public readonly float Current => this.state == 0u ? this.enumerator.Current : Timing.WaitForOneFrame;

		readonly object IEnumerator.Current => this.Current;

        /// <summary>
        /// Creates an instance of the <see cref="RunCoroutineAfterFramesEnumerator"/> struct.
        /// </summary>
        /// <param name="frameWait">The number of frames to wait.</param>
        /// <param name="enumerator">The underlying enumerator to iterate after waiting.</param>
        public RunCoroutineAfterFramesEnumerator(ushort frameWait, IEnumerator<float> enumerator)
		{
			this.initialState = frameWait;
			this.state = frameWait;
			this.enumerator = enumerator;
		}

		/// <inheritdoc/>
		public readonly void Dispose()
		{
			this.enumerator.Dispose();
		}

		/// <inheritdoc/>
		public bool MoveNext()
		{
            if (this.state == 0u)
			{
				return this.enumerator.MoveNext();
			}

			this.state -= 1;
			return true;
		}

		/// <inheritdoc/>
		public void Reset()
		{
			this.enumerator.Reset();
			this.state = this.initialState;
		}
	}
}
