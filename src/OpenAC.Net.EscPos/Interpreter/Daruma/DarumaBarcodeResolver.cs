﻿// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DarumaBarcodeResolver.cs" company="OpenAC .Net">
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
using System.Collections.Generic;
using System.Text;
using OpenAC.Net.Devices.Commom;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Daruma;

/// <summary>
/// Resolve comandos de impressão de código de barras para impressoras Daruma.
/// </summary>
public sealed class DarumaBarcodeResolver : CommandResolver<BarcodeCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância de <see cref="DarumaBarcodeResolver"/>.
    /// </summary>
    /// <param name="enconder">Codificação de caracteres a ser utilizada.</param>
    /// <param name="dict">Dicionário de comandos ESC/POS.</param>
    public DarumaBarcodeResolver(Encoding enconder, IReadOnlyDictionary<CmdEscPos, byte[]> dict) : base(dict)
    {
        Enconder = enconder;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtém a codificação de caracteres utilizada.
    /// </summary>
    public Encoding Enconder { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Resolve o comando de código de barras para o formato Daruma.
    /// </summary>
    /// <param name="command">Comando de código de barras.</param>
    /// <returns>Array de bytes representando o comando ESC/POS.</returns>
    public override byte[] Resolve(BarcodeCommand command)
    {
        using var builder = new ByteArrayBuilder();

        switch (command.Alinhamento)
        {
            case CmdAlinhamento.Esquerda:
                builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);
                break;

            case CmdAlinhamento.Centro:
                builder.Append(Commandos[CmdEscPos.AlinhadoCentro]);
                break;

            case CmdAlinhamento.Direita:
                builder.Append(Commandos[CmdEscPos.AlinhadoDireita]);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        // Formando o código de barras
        byte[] barCode;
        switch (command.Tipo)
        {
            case CmdBarcode.UPCA:
                barCode = [8];
                break;

            case CmdBarcode.EAN13:
                barCode = [1];
                break;

            case CmdBarcode.EAN8:
                barCode = [2];
                break;

            case CmdBarcode.CODE39:
                barCode = [6];
                break;

            case CmdBarcode.Inter2of5:
                barCode = [4];
                break;

            case CmdBarcode.CodaBar:
                barCode = [9];
                break;

            case CmdBarcode.CODE93:
                barCode = [7];
                break;

            case CmdBarcode.CODE128b:
            case CmdBarcode.CODE128:
            case CmdBarcode.CODE128a:
            case CmdBarcode.CODE128c:
                barCode = [5];
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        var largura = command.Largura == 0 ? (byte)2 : Math.Max(Math.Min((byte)command.Largura, (byte)5), (byte)2);
        var altura = command.Altura == 0 ? (byte)50 : Math.Max(Math.Min((byte)command.Altura, (byte)200), (byte)50);

        var showCode = command.Exibir switch
        {
            CmdBarcodeText.SemTexto => new byte[] { 0 },
            CmdBarcodeText.Acima => new byte[] { 0 },
            CmdBarcodeText.Abaixo => new byte[] { 1 },
            CmdBarcodeText.Ambos => new byte[] { 0 },
            _ => new byte[0]
        };

        builder.Append([CmdConst.ESC, (byte)'b']);
        builder.Append(barCode);
        builder.Append(largura);
        builder.Append(altura);
        builder.Append(showCode);
        builder.Append(Enconder.GetBytes(command.Code));
        builder.Append(CmdConst.NUL);

        // Volta alinhamento para Esquerda.
        if (command.Alinhamento != CmdAlinhamento.Esquerda)
            builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);

        return builder.ToArray();
    }

    #endregion Methods
}