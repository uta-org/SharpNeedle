﻿/*
 * MemorySharp Library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2012-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/

using System;
using Binarysharp.MemoryManagement.Native;

namespace Binarysharp.MemoryManagement.Memory
{
    /// <summary>
    ///     Represents a contiguous block of memory in the remote process.
    /// </summary>
    public class RemoteRegion : RemotePointer, IEquatable<RemoteRegion>
    {
        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoteRegion" /> class.
        /// </summary>
        /// <param name="memorySharp">The reference of the <see cref="MemorySharp" /> object.</param>
        /// <param name="baseAddress">The base address of the memory region.</param>
        internal RemoteRegion(MemorySharp memorySharp, IntPtr baseAddress) : base(memorySharp, baseAddress)
        {
        }

        #endregion Constructor

        #region Properties

        #region Information

        /// <summary>
        ///     Contains information about the memory.
        /// </summary>
        public MemoryBasicInformation Information => MemoryCore.Query(MemorySharp.Handle, BaseAddress);

        #endregion Information

        #region IsValid

        /// <summary>
        ///     Gets if the <see cref="RemoteRegion" /> is valid.
        /// </summary>
        public override bool IsValid => base.IsValid && Information.State != MemoryStateFlags.Free;

        #endregion IsValid

        #endregion Properties

        #region Methods

        #region ChangeProtection

        /// <summary>
        ///     Changes the protection of the n next bytes in remote process.
        /// </summary>
        /// <param name="protection">The new protection to apply.</param>
        /// <param name="mustBeDisposed">The resource will be automatically disposed when the finalizer collects the object.</param>
        /// <returns>A new instance of the <see cref="MemoryProtection" /> class.</returns>
        public MemoryProtection ChangeProtection(
            MemoryProtectionFlags protection = MemoryProtectionFlags.ExecuteReadWrite, bool mustBeDisposed = true)
        {
            return new MemoryProtection(MemorySharp, BaseAddress, Information.RegionSize, protection, mustBeDisposed);
        }

        #endregion ChangeProtection

        #region Equals (override)

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((RemoteRegion)obj);
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(RemoteRegion other)
        {
            if (ReferenceEquals(null, other)) return false;
            return ReferenceEquals(this, other) || BaseAddress.Equals(other.BaseAddress) &&
                   MemorySharp.Equals(other.MemorySharp) &&
                   Information.RegionSize.Equals(other.Information.RegionSize);
        }

        #endregion Equals (override)

        #region GetHashCode (override)

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        public override int GetHashCode()
        {
            return BaseAddress.GetHashCode() ^ MemorySharp.GetHashCode() ^ Information.RegionSize.GetHashCode();
        }

        #endregion GetHashCode (override)

        #region Operator (override)

        public static bool operator ==(RemoteRegion left, RemoteRegion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RemoteRegion left, RemoteRegion right)
        {
            return !Equals(left, right);
        }

        #endregion Operator (override)

        #region Release

        /// <summary>
        ///     Releases the memory used by the region.
        /// </summary>
        public void Release()
        {
            // Release the memory
            MemoryCore.Free(MemorySharp.Handle, BaseAddress);
            // Remove the pointer
            BaseAddress = IntPtr.Zero;
        }

        #endregion Release

        #region ToString (override)

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return
                $"BaseAddress = 0x{BaseAddress.ToInt64():X} Size = 0x{Information.RegionSize:X} Protection = {Information.Protect}";
        }

        #endregion ToString (override)

        #endregion Methods
    }
}