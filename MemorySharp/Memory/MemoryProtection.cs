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
    ///     Class providing tools for manipulating memory protection.
    /// </summary>
    public class MemoryProtection : IDisposable
    {
        #region Fields

        /// <summary>
        ///     The reference of the <see cref="MemorySharp" /> object.
        /// </summary>
        private readonly MemorySharp _memorySharp;

        #endregion Fields

        #region Properties

        #region BaseAddress

        /// <summary>
        ///     The base address of the altered memory.
        /// </summary>
        public IntPtr BaseAddress { get; }

        #endregion BaseAddress

        #region MustBedisposed

        /// <summary>
        ///     States if the <see cref="MemoryProtection" /> object nust be disposed when it is collected.
        /// </summary>
        public bool MustBeDisposed { get; set; }

        #endregion MustBedisposed

        #region NewProtection

        /// <summary>
        ///     Defines the new protection applied to the memory.
        /// </summary>
        public MemoryProtectionFlags NewProtection { get; }

        #endregion NewProtection

        #region OldProtection

        /// <summary>
        ///     References the inital protection of the memory.
        /// </summary>
        public MemoryProtectionFlags OldProtection { get; }

        #endregion OldProtection

        #region Size

        /// <summary>
        ///     The size of the altered memory.
        /// </summary>
        public int Size { get; }

        #endregion Size

        #endregion Properties

        #region Constructor/Destructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="MemoryProtection" /> class.
        /// </summary>
        /// <param name="memorySharp">The reference of the <see cref="MemorySharp" /> object.</param>
        /// <param name="baseAddress">The base address of the memory to change the protection.</param>
        /// <param name="size">The size of the memory to change.</param>
        /// <param name="protection">The new protection to apply.</param>
        /// <param name="mustBeDisposed">The resource will be automatically disposed when the finalizer collects the object.</param>
        public MemoryProtection(MemorySharp memorySharp, IntPtr baseAddress, int size,
            MemoryProtectionFlags protection = MemoryProtectionFlags.ExecuteReadWrite,
            bool mustBeDisposed = true)
        {
            // Save the parameters
            _memorySharp = memorySharp;
            BaseAddress = baseAddress;
            NewProtection = protection;
            Size = size;
            MustBeDisposed = mustBeDisposed;

            // Change the memory protection
            OldProtection = MemoryCore.ChangeProtection(_memorySharp.Handle, baseAddress, size, protection);
        }

        /// <summary>
        ///     Frees resources and perform other cleanup operations before it is reclaimed by garbage collection.
        /// </summary>
        ~MemoryProtection()
        {
            if (MustBeDisposed)
                Dispose();
        }

        #endregion Constructor/Destructor

        #region Methods

        #region Dispose (implementation of IDisposable)

        /// <summary>
        ///     Restores the initial protection of the memory.
        /// </summary>
        public virtual void Dispose()
        {
            // Restore the memory protection
            MemoryCore.ChangeProtection(_memorySharp.Handle, BaseAddress, Size, OldProtection);
            // Avoid the finalizer
            GC.SuppressFinalize(this);
        }

        #endregion Dispose (implementation of IDisposable)

        #region ToString (override)

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return
                $"BaseAddress = 0x{BaseAddress.ToInt64():X} NewProtection = {NewProtection} OldProtection = {OldProtection}";
        }

        #endregion ToString (override)

        #endregion Methods
    }
}