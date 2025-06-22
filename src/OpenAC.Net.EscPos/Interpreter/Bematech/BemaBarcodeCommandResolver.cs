// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="BemaBarcodeCommandResolver.cs" company="OpenAC .Net">
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
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.Devices.Commom;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Extensions;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Bematech;

/// <summary>
/// Resolve comandos de impressão de código de barras para impressoras Bematech.
/// </summary>
public sealed class BemaBarcodeCommandResolver : CommandResolver<BarcodeCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância de <see cref="BemaBarcodeCommandResolver"/>.
    /// </summary>
    /// <param name="enconder">Codificação a ser utilizada para o código de barras.</param>
    /// <param name="dict">Dicionário de comandos ESC/POS.</param>
    public BemaBarcodeCommandResolver(Encoding enconder, IReadOnlyDictionary<CmdEscPos, byte[]> dict) : base(dict)
    {
        Enconder = enconder;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtém a codificação utilizada para o código de barras.
    /// </summary>
    public Encoding Enconder { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Resolve o comando de impressão de código de barras.
    /// </summary>
    /// <param name="command">Comando de código de barras.</param>
    /// <returns>Array de bytes com o comando ESC/POS.</returns>
    public override byte[] Resolve(BarcodeCommand command)
    {
        if (!Commandos.ContainsKey(CmdEscPos.IniciarBarcode)) return [];

        using var builder = new ByteArrayBuilder();

        switch (command.Alinhamento)
        {
            case CmdAlinhamento.Esquerda when Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda):
                builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);
                break;

            case CmdAlinhamento.Centro when Commandos.ContainsKey(CmdEscPos.AlinhadoCentro):
                builder.Append(Commandos[CmdEscPos.AlinhadoCentro]);
                break;

            case CmdAlinhamento.Direita when Commandos.ContainsKey(CmdEscPos.AlinhadoDireita):
                builder.Append(Commandos[CmdEscPos.AlinhadoDireita]);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        // Formando o código de barras
        byte[] barCode = command.Tipo switch
        {
            CmdBarcode.UPCA => Commandos[CmdEscPos.BarcodeUPCA],
            CmdBarcode.UPCE => Commandos[CmdEscPos.BarcodeUPCE],
            CmdBarcode.EAN13 => Commandos[CmdEscPos.BarcodeEAN13],
            CmdBarcode.EAN8 => Commandos[CmdEscPos.BarcodeEAN8],
            CmdBarcode.CODE39 => Commandos[CmdEscPos.BarcodeCODE39],
            CmdBarcode.Inter2of5 => Commandos[CmdEscPos.BarcodeInter2of5],
            CmdBarcode.CodaBar => Commandos[CmdEscPos.BarcodeCodaBar],
            CmdBarcode.CODE93 => Commandos[CmdEscPos.BarcodeCODE93],
            CmdBarcode.CODE128 or CmdBarcode.CODE128b or CmdBarcode.CODE128c => Commandos[CmdEscPos.BarcodeCODE128],
            _ => throw new ArgumentOutOfRangeException()
        };

        var largura = command.Largura == 0 ? (byte)2 : Math.Max(Math.Min((byte)command.Largura, (byte)4), (byte)1);
        var altura = command.Altura == 0 ? (byte)50 : Math.Max(Math.Min((byte)command.Altura, (byte)255), (byte)1);

        var showCode = command.Exibir switch
        {
            CmdBarcodeText.SemTexto when Commandos.ContainsKey(CmdEscPos.BarcodeNoText) => Commandos[CmdEscPos.BarcodeNoText],
            CmdBarcodeText.Acima when Commandos.ContainsKey(CmdEscPos.BarcodeTextAbove) => Commandos[CmdEscPos.BarcodeTextAbove],
            CmdBarcodeText.Abaixo when Commandos.ContainsKey(CmdEscPos.BarcodeTextBelow) => Commandos[CmdEscPos.BarcodeTextBelow],
            CmdBarcodeText.Ambos when Commandos.ContainsKey(CmdEscPos.BarcodeTextBoth) => Commandos[CmdEscPos.BarcodeTextBoth],
            _ => []
        };

        if (Commandos.ContainsKey(CmdEscPos.BarcodeWidth))
            builder.Append(Commandos[CmdEscPos.BarcodeHeight], largura);

        if (Commandos.TryGetValue(CmdEscPos.BarcodeHeight, out var commando))
            builder.Append(commando, altura);

        if (!showCode.IsNullOrEmpty())
            builder.Append(showCode);

        builder.Append(Commandos[CmdEscPos.IniciarBarcode], barCode);
        builder.Append((byte)command.Code.Length);
        builder.Append(Enconder.GetBytes(command.Code));

        // Volta alinhamento para Esquerda.
        if (Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda) && command.Alinhamento != CmdAlinhamento.Esquerda)
            builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);

        return builder.ToArray();
    }

    #endregion Methods
}