﻿/*
 * MemorySharp Library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2012-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Binarysharp.MemoryManagement.Native;
using Binarysharp.MemoryManagement.Threading;
using Binarysharp.MemoryManagement.Windows.Keyboard;
using Binarysharp.MemoryManagement.Windows.Mouse;

namespace Binarysharp.MemoryManagement.Windows
{
    /// <summary>
    ///     Class repesenting a window in the remote process.
    /// </summary>
    public class RemoteWindow : IEquatable<RemoteWindow>
    {
        #region Fields

        /// <summary>
        ///     The reference of the <see cref="MemoryManagement.MemorySharp" /> object.
        /// </summary>
        protected readonly MemorySharp MemorySharp;

        #endregion Fields

        #region Constructor

        /// <summary>
        ///     Initializes a new instance of the <see cref="RemoteWindow" /> class.
        /// </summary>
        /// <param name="memorySharp">The reference of the <see cref="MemoryManagement.MemorySharp" /> object.</param>
        /// <param name="handle">The handle of a window.</param>
        internal RemoteWindow(MemorySharp memorySharp, IntPtr handle)
        {
            // Save the parameters
            MemorySharp = memorySharp;
            Handle = handle;
            // Create the tools
            Keyboard = new MessageKeyboard(this);
            Mouse = new SendInputMouse(this);
        }

        #endregion Constructor

        #region Properties

        #region Children

        /// <summary>
        ///     Gets all the child windows of this window.
        /// </summary>
        public IEnumerable<RemoteWindow> Children
        {
            get { return ChildrenHandles.Select(handle => new RemoteWindow(MemorySharp, handle)); }
        }

        #endregion Children

        #region ChildHandles (protected)

        /// <summary>
        ///     Gets all the child window handles of this window.
        /// </summary>
        protected IEnumerable<IntPtr> ChildrenHandles => WindowCore.EnumChildWindows(Handle);

        #endregion ChildHandles (protected)

        #region ClassName

        /// <summary>
        ///     Gets the class name of the window.
        /// </summary>
        public string ClassName => WindowCore.GetClassName(Handle);

        #endregion ClassName

        #region Handle

        /// <summary>
        ///     The handle of the window.
        /// </summary>
        /// <remarks>
        ///     The type here is not <see cref="SafeMemoryHandle" /> because a window cannot be closed by calling
        ///     <see cref="NativeMethods.CloseHandle" />.
        ///     For more information, see:
        ///     http://stackoverflow.com/questions/8507307/why-cant-i-close-the-window-handle-in-my-code.
        /// </remarks>
        public IntPtr Handle { get; }

        #endregion Handle

        #region Height

        /// <summary>
        ///     Gets or sets the height of the element.
        /// </summary>
        public int Height
        {
            get => Placement.NormalPosition.Height;
            set
            {
                var p = Placement;
                p.NormalPosition.Height = value;
                Placement = p;
            }
        }

        #endregion Height

        #region IsActivated

        /// <summary>
        ///     Gets if the window is currently activated.
        /// </summary>
        public bool IsActivated => WindowCore.GetForegroundWindow() == Handle;

        #endregion IsActivated

        #region IsMainWindow

        /// <summary>
        ///     Gets if this is the main window.
        /// </summary>
        public bool IsMainWindow => MemorySharp.Windows.MainWindow == this;

        #endregion IsMainWindow

        #region Keyboard

        /// <summary>
        ///     Tools for managing a virtual keyboard in the window.
        /// </summary>
        public BaseKeyboard Keyboard { get; set; }

        #endregion Keyboard

        #region Mouse

        /// <summary>
        ///     Tools for managing a virtual mouse in the window.
        /// </summary>
        public BaseMouse Mouse { get; set; }

        #endregion Mouse

        #region Placement (internal)

        /// <summary>
        ///     Gets or sets the placement of the window.
        /// </summary>
        public WindowPlacement Placement
        {
            get => WindowCore.GetWindowPlacement(Handle);
            set => WindowCore.SetWindowPlacement(Handle, value);
        }

        #endregion Placement (internal)

        #region State

        /// <summary>
        ///     Gets or sets the specified window's show state.
        /// </summary>
        public WindowStates State
        {
            get => Placement.ShowCmd;
            set => WindowCore.ShowWindow(Handle, value);
        }

        #endregion State

        #region Title

        /// <summary>
        ///     Gets or sets the title of the window.
        /// </summary>
        public string Title
        {
            get => WindowCore.GetWindowText(Handle);
            set => WindowCore.SetWindowText(Handle, value);
        }

        #endregion Title

        #region Thread

        /// <summary>
        ///     Gets the thread of the window.
        /// </summary>
        public RemoteThread Thread => MemorySharp.Threads.GetThreadById(WindowCore.GetWindowThreadId(Handle));

        #endregion Thread

        #region Width

        /// <summary>
        ///     Gets or sets the width of the element.
        /// </summary>
        public int Width
        {
            get => Placement.NormalPosition.Width;
            set
            {
                var p = Placement;
                p.NormalPosition.Width = value;
                Placement = p;
            }
        }

        #endregion Width

        #region X

        /// <summary>
        ///     Gets or sets the x-coordinate of the window.
        /// </summary>
        public int X
        {
            get => Placement.NormalPosition.Left;
            set
            {
                var p = Placement;
                p.NormalPosition.Right = value + p.NormalPosition.Width;
                p.NormalPosition.Left = value;
                Placement = p;
            }
        }

        #endregion X

        #region Y

        /// <summary>
        ///     Gets or sets the y-coordinate of the window.
        /// </summary>
        public int Y
        {
            get => Placement.NormalPosition.Top;
            set
            {
                var p = Placement;
                p.NormalPosition.Bottom = value + p.NormalPosition.Height;
                p.NormalPosition.Top = value;
                Placement = p;
            }
        }

        #endregion Y

        #endregion Properties

        #region Methods

        #region Activate

        /// <summary>
        ///     Activates the window.
        /// </summary>
        public void Activate()
        {
            WindowCore.SetForegroundWindow(Handle);
        }

        #endregion Activate

        #region Close

        /// <summary>
        ///     Closes the window.
        /// </summary>
        public void Close()
        {
            PostMessage(WindowsMessages.Close, UIntPtr.Zero, UIntPtr.Zero);
        }

        #endregion Close

        #region Equals (override)

        /// <summary>
        ///     Determines whether the specified object is equal to the current object.
        /// </summary>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((RemoteWindow)obj);
        }

        /// <summary>
        ///     Returns a value indicating whether this instance is equal to a specified object.
        /// </summary>
        public bool Equals(RemoteWindow other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(MemorySharp, other.MemorySharp) && Handle.Equals(other.Handle);
        }

        #endregion Equals (override)

        #region Flash

        /// <summary>
        ///     Flashes the window one time. It does not change the active state of the window.
        /// </summary>
        public void Flash()
        {
            WindowCore.FlashWindow(Handle);
        }

        /// <summary>
        ///     Flashes the window. It does not change the active state of the window.
        /// </summary>
        /// <param name="count">The number of times to flash the window.</param>
        /// <param name="timeout">The rate at which the window is to be flashed.</param>
        /// <param name="flags">The flash status.</param>
        public void Flash(uint count, TimeSpan timeout, FlashWindowFlags flags = FlashWindowFlags.All)
        {
            WindowCore.FlashWindowEx(Handle, flags, count, timeout);
        }

        #endregion Flash

        #region GetHashCode (override)

        /// <summary>
        ///     Serves as a hash function for a particular type.
        /// </summary>
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = MemorySharp != null ? MemorySharp.GetHashCode() : 0;
                hashCode = (hashCode * 397) ^ Handle.GetHashCode();
                return hashCode;
            }
        }

        #endregion GetHashCode (override)

        #region Operator (override)

        public static bool operator ==(RemoteWindow left, RemoteWindow right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(RemoteWindow left, RemoteWindow right)
        {
            return !Equals(left, right);
        }

        #endregion Operator (override)

        #region PostMessage

        /// <summary>
        ///     Places (posts) a message in the message queue associated with the thread that created the window and returns
        ///     without waiting for the thread to process the message.
        /// </summary>
        /// <param name="message">The message to be posted.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        public void PostMessage(WindowsMessages message, UIntPtr wParam, UIntPtr lParam)
        {
            WindowCore.PostMessage(Handle, message, wParam, lParam);
        }

        /// <summary>
        ///     Places (posts) a message in the message queue associated with the thread that created the window and returns
        ///     without waiting for the thread to process the message.
        /// </summary>
        /// <param name="message">The message to be posted.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        public void PostMessage(uint message, UIntPtr wParam, UIntPtr lParam)
        {
            WindowCore.PostMessage(Handle, message, wParam, lParam);
        }

        #endregion PostMessage

        #region SendMessage

        /// <summary>
        ///     Sends the specified message to a window or windows.
        ///     The SendMessage function calls the window procedure for the specified window and does not return until the window
        ///     procedure has processed the message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public IntPtr SendMessage(WindowsMessages message, UIntPtr wParam, IntPtr lParam)
        {
            return WindowCore.SendMessage(Handle, message, wParam, lParam);
        }

        /// <summary>
        ///     Sends the specified message to a window or windows.
        ///     The SendMessage function calls the window procedure for the specified window and does not return until the window
        ///     procedure has processed the message.
        /// </summary>
        /// <param name="message">The message to be sent.</param>
        /// <param name="wParam">Additional message-specific information.</param>
        /// <param name="lParam">Additional message-specific information.</param>
        /// <returns>The return value specifies the result of the message processing; it depends on the message sent.</returns>
        public IntPtr SendMessage(uint message, UIntPtr wParam, IntPtr lParam)
        {
            return WindowCore.SendMessage(Handle, message, wParam, lParam);
        }

        #endregion SendMessage

        #region ToString (override)

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"Title = {Title} ClassName = {ClassName}";
        }

        #endregion ToString (override)

        #endregion Methods
    }
}