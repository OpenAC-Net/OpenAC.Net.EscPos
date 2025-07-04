﻿// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="EscPosInterpreterFactory.cs" company="OpenAC .Net">
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

using System;
using System.Text;
using OpenAC.Net.Core;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter;
using OpenAC.Net.EscPos.Interpreter.Bematech;
using OpenAC.Net.EscPos.Interpreter.Daruma;
using OpenAC.Net.EscPos.Interpreter.Datecs;
using OpenAC.Net.EscPos.Interpreter.Diebold;
using OpenAC.Net.EscPos.Interpreter.Elgin;
using OpenAC.Net.EscPos.Interpreter.Epson;
using OpenAC.Net.EscPos.Interpreter.GPrinter;
using OpenAC.Net.EscPos.Interpreter.PosStar;
using OpenAC.Net.EscPos.Interpreter.Sunmi;
using OpenAC.Net.EscPos.Interpreter.ZJiang;

namespace OpenAC.Net.EscPos;

/// <summary>
/// Cria uma instância de <see cref="EscPosInterpreter"/> de acordo com o protocolo e página de código informados.
/// </summary>
/// <param name="protocolo">O protocolo ESC/POS a ser utilizado.</param>
/// <param name="paginaCodigo">A página de código a ser utilizada para codificação de caracteres.</param>
/// <returns>Uma instância de <see cref="EscPosInterpreter"/> correspondente ao protocolo informado.</returns>
/// <exception cref="ArgumentOutOfRangeException">
/// Lançada quando o protocolo ou a página de código informados não são suportados.
/// </exception>
public static class EscPosInterpreterFactory
{
    /// <summary>
    /// Cria uma instância de <see cref="EscPosInterpreter"/> de acordo com o protocolo e página de código informados.
    /// </summary>
    /// <param name="protocolo">O protocolo ESC/POS a ser utilizado.</param>
    /// <param name="paginaCodigo">A página de código a ser utilizada para codificação de caracteres.</param>
    /// <returns>Uma instância de <see cref="EscPosInterpreter"/> correspondente ao protocolo informado.</returns>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Lançada quando o protocolo ou a página de código informados não são suportados.
    /// </exception>
    public static EscPosInterpreter Create(ProtocoloEscPos protocolo, PaginaCodigo paginaCodigo)
    {
        var enconder = paginaCodigo switch
        {
            PaginaCodigo.pc437 => Encoding.GetEncoding(437),
            PaginaCodigo.pc850 => OpenEncoding.IBM850,
            PaginaCodigo.pc852 => Encoding.GetEncoding("IBM852"),
            PaginaCodigo.pc860 => OpenEncoding.IBM860,
            PaginaCodigo.pcUTF8 => Encoding.UTF8,
            PaginaCodigo.pc1252 => OpenEncoding.CP1252,
            _ => throw new ArgumentOutOfRangeException()
        };

        return protocolo switch
        {
            ProtocoloEscPos.EscPos => new EpsonInterpreter(enconder),
            ProtocoloEscPos.EscBema => new BematechInterpreter(enconder),
            ProtocoloEscPos.EscDaruma => new DarumaInterpreter(enconder),
            ProtocoloEscPos.EscElgin => new ElginInterpreter(enconder),
            ProtocoloEscPos.EscDiebold => new DieboldInterpreter(enconder),
            ProtocoloEscPos.EscGPrinter => new GPrinterInterpreter(enconder),
            ProtocoloEscPos.EscDatecs => new DatecsInterpreter(enconder),
            ProtocoloEscPos.EscZJiang => new ZJiangInterpreter(enconder),
            ProtocoloEscPos.EscPosStar => new PosStarInterpreter(enconder),
            ProtocoloEscPos.EscSunmi => new SunmiInterpreter(enconder),
            _ => throw new ArgumentOutOfRangeException(nameof(protocolo), protocolo, null)
        };
    }
}