﻿/*
 * MemorySharp Library
 * http://www.binarysharp.com/
 *
 * Copyright (C) 2012-2014 Jämes Ménétrey (a.k.a. ZenLulz).
 * This library is released under the MIT License.
 * See the file LICENSE for more information.
*/

using System;
using System.Runtime.InteropServices;
using Binarysharp.MemoryManagement.Internals;

namespace Binarysharp.MemoryManagement.Native
{
    #region FlashInfo

    /// <summary>
    ///     Contains the flash status for a window and the number of times the system should flash the window.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FlashInfo
    {
        public int Size;
        public IntPtr Hwnd;
        public FlashWindowFlags Flags;
        public uint Count;
        public int Timeout;
    }

    #endregion FlashInfo

    #region HardwareInput

    /// <summary>
    ///     Contains information about a simulated message generated by an input device other than a keyboard or mouse.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct HardwareInput
    {
        /// <summary>
        ///     The message generated by the input hardware.
        /// </summary>
        public int Message;

        /// <summary>
        ///     The low-order word of the lParam parameter for <see cref="Message" />.
        /// </summary>
        public short WParamL;

        /// <summary>
        ///     The high-order word of the lParam parameter for <see cref="Message" />.
        /// </summary>
        public short WParamH;
    }

    #endregion HardwareInput

    #region Input

    /// <summary>
    ///     Used by <see cref="NativeMethods.SendInput" /> to store information for synthesizing input events such as
    ///     keystrokes, mouse movement, and mouse clicks.
    /// </summary>
    [StructLayout(LayoutKind.Explicit)]
    public struct Input
    {
        /// <summary>
        ///     Constructor that specifies a type.
        /// </summary>
        /// <param name="type">The type if the input event.</param>
        public Input(InputTypes type) : this()
        {
            Type = type;
        }

        /// <summary>
        ///     The type of the input event.
        /// </summary>
        [FieldOffset(0)] public InputTypes Type;

        /// <summary>
        ///     The information about a simulated mouse event.
        /// </summary>
        [FieldOffset(sizeof(int))] public MouseInput Mouse;

        /// <summary>
        ///     The information about a simulated keyboard event.
        /// </summary>
        [FieldOffset(sizeof(int))] public KeyboardInput Keyboard;

        /// <summary>
        ///     The information about a simulated hardware event.
        /// </summary>
        [FieldOffset(sizeof(int))] public HardwareInput Hardware;
    }

    #endregion Input

    #region KeyboardInput

    /// <summary>
    ///     Contains information about a simulated keyboard event.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct KeyboardInput
    {
        /// <summary>
        ///     A virtual-key code. The code must be a value in the range 1 to 254. If the <see cref="Flags" /> member specifies
        ///     KEYEVENTF_UNICODE, wVk must be 0.
        /// </summary>
        public Keys VirtualKey;

        /// <summary>
        ///     A hardware scan code for the key.
        ///     If <see cref="Flags" /> specifies KEYEVENTF_UNICODE, wScan specifies a Unicode character which is to be sent to the
        ///     foreground application.
        /// </summary>
        public short ScanCode;

        /// <summary>
        ///     Specifies various aspects of a keystroke.
        /// </summary>
        public KeyboardFlags Flags;

        /// <summary>
        ///     The time stamp for the event, in milliseconds. If this parameter is zero, the system will provide its own time
        ///     stamp.
        /// </summary>
        public int Time;

        /// <summary>
        ///     An additional value associated with the keystroke. Use the GetMessageExtraInfo function to obtain this information.
        /// </summary>
        public IntPtr ExtraInfo;
    }

    #endregion KeyboardInput

    #region LdtEntry

    /// <summary>
    ///     Describes an entry in the descriptor table. This structure is valid only on x86-based systems.
    /// </summary>
    /// <remarks>This is a simplified version of the original structure.</remarks>
    [StructLayout(LayoutKind.Sequential)]
    public struct LdtEntry
    {
        /// <summary>
        ///     The low-order part of the address of the last byte in the segment.
        /// </summary>
        public ushort LimitLow;

        /// <summary>
        ///     The low-order part of the base address of the segment.
        /// </summary>
        public ushort BaseLow;

        /// <summary>
        ///     Middle bits (16–23) of the base address of the segment.
        /// </summary>
        public byte BaseMid;

        /// <summary>
        ///     Values of the Type, Dpl, and Pres members in the Bits structure (not implemented).
        /// </summary>
        public byte Flag1;

        /// <summary>
        ///     Values of the LimitHi, Sys, Reserved_0, Default_Big, and Granularity members in the Bits structure (not
        ///     implemented).
        /// </summary>
        public byte Flag2;

        /// <summary>
        ///     High bits (24–31) of the base address of the segment.
        /// </summary>
        public byte BaseHi;
    }

    #endregion LdtEntry

    #region MemoryBasicInformation

    /// <summary>
    ///     Contains information about a range of pages in the virtual address space of a process. The VirtualQuery and
    ///     <see cref="NativeMethods.VirtualQueryEx" /> functions use this structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MemoryBasicInformation
    {
        /// <summary>
        ///     A pointer to the base address of the region of pages.
        /// </summary>
        public IntPtr BaseAddress;

        /// <summary>
        ///     A pointer to the base address of a range of pages allocated by the VirtualAlloc function. The page pointed to by
        ///     the BaseAddress member is contained within this allocation range.
        /// </summary>
        public IntPtr AllocationBase;

        /// <summary>
        ///     The memory protection option when the region was initially allocated. This member can be one of the memory
        ///     protection constants or 0 if the caller does not have access.
        /// </summary>
        public MemoryProtectionFlags AllocationProtect;

        /// <summary>
        ///     The size of the region beginning at the base address in which all pages have identical attributes, in bytes.
        /// </summary>
        public readonly int RegionSize;

        /// <summary>
        ///     The state of the pages in the region.
        /// </summary>
        public MemoryStateFlags State;

        /// <summary>
        ///     The access protection of the pages in the region. This member is one of the values listed for the AllocationProtect
        ///     member.
        /// </summary>
        public MemoryProtectionFlags Protect;

        /// <summary>
        ///     The type of pages in the region.
        /// </summary>
        public MemoryTypeFlags Type;
    }

    #endregion MemoryBasicInformation

    #region MouseInput

    /// <summary>
    ///     Contains information about a simulated mouse event.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct MouseInput
    {
        /// <summary>
        ///     The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on
        ///     the value of the <see cref="Flags" /> member.
        ///     Absolute data is specified as the x coordinate of the mouse; relative data is specified as the number of pixels
        ///     moved.
        /// </summary>
        public int DeltaX;

        /// <summary>
        ///     The absolute position of the mouse, or the amount of motion since the last mouse event was generated, depending on
        ///     the value of the <see cref="Flags" /> member.
        ///     Absolute data is specified as the y coordinate of the mouse; relative data is specified as the number of pixels
        ///     moved.
        /// </summary>
        public int DeltaY;

        /// <summary>
        ///     If <see cref="Flags" /> contains <see cref="MouseFlags.Wheel" />, then mouseData specifies the amount of wheel
        ///     movement.
        ///     A positive value indicates that the wheel was rotated forward, away from the user; a negative value indicates that
        ///     the wheel was rotated backward, toward the user.
        ///     One wheel click is defined as WHEEL_DELTA, which is 120.
        ///     Windows Vista: If dwFlags contains <see cref="MouseFlags.HWheel" />, then dwData specifies the amount of wheel
        ///     movement.
        ///     A positive value indicates that the wheel was rotated to the right; a negative value indicates that the wheel was
        ///     rotated to the left.
        ///     One wheel click is defined as WHEEL_DELTA, which is 120.
        ///     If dwFlags does not contain <see cref="MouseFlags.Wheel" />, <see cref="MouseFlags.XDown" />, or
        ///     <see cref="MouseFlags.XUp" />, then mouseData should be zero.
        ///     If dwFlags contains <see cref="MouseFlags.XDown" /> or <see cref="MouseFlags.XUp" />, then mouseData specifies
        ///     which X buttons were pressed or released.
        ///     This value may be any combination of the following flags.
        ///     XBUTTON1 = 0x1
        ///     XBUTTON2 = 0x2
        /// </summary>
        public int MouseData;

        /// <summary>
        ///     A set of bit flags that specify various aspects of mouse motion and button clicks.
        ///     The bits in this member can be any reasonable combination of the following values.
        ///     The bit flags that specify mouse button status are set to indicate changes in status, not ongoing conditions.
        ///     For example, if the left mouse button is pressed and held down, <see cref="MouseFlags.LeftDown" /> is set when the
        ///     left
        ///     button is first pressed, but not for subsequent motions. Similarly, <see cref="MouseFlags.LeftUp" /> is set only
        ///     when the button is first released.
        ///     You cannot specify both the <see cref="MouseFlags.Wheel" /> flag and either <see cref="MouseFlags.XDown" /> or
        ///     <see cref="MouseFlags.XUp" /> flags
        ///     simultaneously in the dwFlags parameter, because they both require use of the mouseData field.
        /// </summary>
        public MouseFlags Flags;

        /// <summary>
        ///     The time stamp for the event, in milliseconds. If this parameter is 0, the system will provide its own time stamp.
        /// </summary>
        public int Time;

        /// <summary>
        ///     An additional value associated with the mouse event. An application calls GetMessageExtraInfo to obtain this extra
        ///     information.
        /// </summary>
        public IntPtr ExtraInfo;
    }

    #endregion MouseInput

    #region Point

    /// <summary>
    ///     The <see cref="Point" /> structure defines the x and y coordinates of a point.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Point
    {
        /// <summary>
        ///     The x-coordinate of the point.
        /// </summary>
        public int X;

        /// <summary>
        ///     The y-coordinate of the point.
        /// </summary>
        public int Y;

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"X = {X} Y = {Y}";
        }
    }

    #endregion Point

    #region ProcessBasicInformation

    /// <summary>
    ///     Structure containing basic information about a process.
    /// </summary>
    public struct ProcessBasicInformation
    {
        /// <summary>
        ///     The exit status.
        /// </summary>
        public uint ExitStatus;

        /// <summary>
        ///     The base address of Process Environment Block.
        /// </summary>
        public IntPtr PebBaseAddress;

        /// <summary>
        ///     The affinity mask.
        /// </summary>
        public uint AffinityMask;

        /// <summary>
        ///     The base priority.
        /// </summary>
        public uint BasePriority;

        /// <summary>
        ///     The process id.
        /// </summary>
        public int ProcessId;

        /// <summary>
        ///     The process id of the parent process.
        /// </summary>
        public int InheritedFromUniqueProcessId;

        /// <summary>
        ///     The size of this structure.
        /// </summary>
        public int Size => MarshalType<ProcessBasicInformation>.Size;
    }

    #endregion ProcessBasicInformation

    #region ThreadBasicInformation

    /// <summary>
    ///     Structure containing basic information about a thread.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ThreadBasicInformation
    {
        /// <summary>
        ///     the exit status.
        /// </summary>
        public uint ExitStatus;

        /// <summary>
        ///     The base address of Thread Environment Block.
        /// </summary>
        public IntPtr TebBaseAdress;

        /// <summary>
        ///     The process id which owns the thread.
        /// </summary>
        public int ProcessId;

        /// <summary>
        ///     The thread id.
        /// </summary>
        public int ThreadId;

        /// <summary>
        ///     The affinity mask.
        /// </summary>
        public uint AffinityMask;

        /// <summary>
        ///     The priority.
        /// </summary>
        public uint Priority;

        /// <summary>
        ///     The base priority.
        /// </summary>
        public uint BasePriority;
    }

    #endregion ThreadBasicInformation

    #region ThreadContext

    /// <summary>
    ///     Represents a thread context.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct ThreadContext
    {
        /// <summary>
        ///     Determines which registers are returned or set when using <see cref="NativeMethods.GetThreadContext" /> or
        ///     <see cref="NativeMethods.SetThreadContext" />.
        ///     If the context record is used as an INPUT parameter, then for each portion of the context record controlled by a
        ///     flag whose value is set, it is assumed that portion of the
        ///     context record contains valid context. If the context record is being used to modify a threads context, then only
        ///     that portion of the threads context will be modified.
        ///     If the context record is used as an INPUT/OUTPUT parameter to capture the context of a thread, then only those
        ///     portions of the thread's context corresponding to set flags will be returned.
        ///     The context record is never used as an OUTPUT only parameter.
        /// </summary>
        public ThreadContextFlags ContextFlags;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr0;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr1;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr2;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr3;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr6;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.DebugRegisters" />.
        /// </summary>
        public uint Dr7;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.FloatingPoint" />.
        /// </summary>
        [MarshalAs(UnmanagedType.Struct)] public FloatingSaveArea FloatingSave;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public uint SegGs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public uint SegFs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public uint SegEs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Segments" />.
        /// </summary>
        public uint SegDs;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Edi;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Esi;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Ebx;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Edx;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Ecx;

        /// <summary>
        ///     This register is specified/returned if the ContextFlags word contains the flag
        ///     <see cref="ThreadContextFlags.Integer" />.
        /// </summary>
        public uint Eax;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint Ebp;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint Eip;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint SegCs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint EFlags;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint Esp;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.Control" />.
        /// </summary>
        public uint SegSs;

        /// <summary>
        ///     This is specified/returned if <see cref="ContextFlags" /> contains the flag
        ///     <see cref="ThreadContextFlags.ExtendedRegisters" />.
        ///     The format and contexts are processor specific.
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 512)]
        public byte[] ExtendedRegisters;
    }

    #endregion ThreadContext

    #region FloatingSaveArea

    /// <summary>
    ///     Returned if <see cref="ThreadContextFlags.FloatingPoint" /> flag is set.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct FloatingSaveArea
    {
        public uint ControlWord;
        public uint StatusWord;
        public uint TagWord;
        public uint ErrorOffset;
        public uint ErrorSelector;
        public uint DataOffset;
        public uint DataSelector;

        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 80)]
        public byte[] RegisterArea;

        public uint Cr0NpxState;
    }

    #endregion FloatingSaveArea

    #region Rectangle

    /// <summary>
    ///     The <see cref="Rectangle" /> structure defines the coordinates of the upper-left and lower-right corners of a
    ///     rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct Rectangle
    {
        /// <summary>
        ///     The x-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Left;

        /// <summary>
        ///     The y-coordinate of the upper-left corner of the rectangle.
        /// </summary>
        public int Top;

        /// <summary>
        ///     The x-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Right;

        /// <summary>
        ///     The y-coordinate of the lower-right corner of the rectangle.
        /// </summary>
        public int Bottom;

        /// <summary>
        ///     Gets or sets the height of the element.
        /// </summary>
        public int Height
        {
            get => Bottom - Top;
            set => Bottom = Top + value;
        }

        /// <summary>
        ///     Gets or sets the width of the element.
        /// </summary>
        public int Width
        {
            get => Right - Left;
            set => Right = Left + value;
        }

        /// <summary>
        ///     Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"Left = {Left} Top = {Top} Height = {Height} Width = {Width}";
        }
    }

    #endregion Rectangle

    #region WindowPlacement

    /// <summary>
    ///     Contains information about the placement of a window on the screen.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct WindowPlacement
    {
        /// <summary>
        ///     The length of the structure, in bytes. Before calling the GetWindowPlacement or SetWindowPlacement functions, set
        ///     this member to sizeof(WINDOWPLACEMENT).
        /// </summary>
        public int Length;

        /// <summary>
        ///     Specifies flags that control the position of the minimized window and the method by which the window is restored.
        /// </summary>
        public int Flags;

        /// <summary>
        ///     The current show state of the window.
        /// </summary>
        public WindowStates ShowCmd;

        /// <summary>
        ///     The coordinates of the window's upper-left corner when the window is minimized.
        /// </summary>
        public Point MinPosition;

        /// <summary>
        ///     The coordinates of the window's upper-left corner when the window is maximized.
        /// </summary>
        public Point MaxPosition;

        /// <summary>
        ///     The window's coordinates when the window is in the restored position.
        /// </summary>
        public Rectangle NormalPosition;
    }

    #endregion WindowPlacement
}