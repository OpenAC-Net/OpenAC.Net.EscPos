// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="CmdConst.cs" company="OpenAC .Net">
//		        		   The MIT License (MIT)
//	     		    Copyright (c) 2014 - 2021 Projeto OpenAC .Net
//
//	 Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the "Software"),
// to deal in the Software without restriction, including without limitation
// the rights to use, copy, modify, merge, publish, distribute, sublicense,
// and/or sell copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following conditions:
//	 The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//	 THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER
// DEALINGS IN THE SOFTWARE.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace OpenAC.Net.EscPos.Commom;

/// <summary>
/// Define constantes de comandos de controle ESC/POS.
/// </summary>
internal static class CmdConst
{
    /// <summary>Nulo (NUL).</summary>
    public static byte NUL => 0;

    /// <summary>Enquiry (ENQ).</summary>
    public static byte ENQ => 5;

    /// <summary>Escape (ESC).</summary>
    public static byte ESC => 27;

    /// <summary>File Separator (FS).</summary>
    public static byte FS => 28;

    /// <summary>Group Separator (GS).</summary>
    public static byte GS => 29;

    /// <summary>Backspace (BS).</summary>
    public static byte BS => 8;

    /// <summary>Tabulação Horizontal (TAB).</summary>
    public static byte TAB => 9;

    /// <summary>Line Feed (LF).</summary>
    public static byte LF => 10;

    /// <summary>Form Feed (FF).</summary>
    public static byte FF => 12;

    /// <summary>Carriage Return (CR).</summary>
    public static byte CR => 13;

    /// <summary>Shift In (SI).</summary>
    public static byte SI => 15;

    /// <summary>Device Control 2 (DC2).</summary>
    public static byte DC2 => 18;

    /// <summary>Device Control 4 (DC4).</summary>
    public static byte DC4 => 20;

    /// <summary>Synchronous Idle (SYN).</summary>
    public static byte SYN => 22;

    /// <summary>Bell (BELL).</summary>
    public static byte BELL => 7;
}