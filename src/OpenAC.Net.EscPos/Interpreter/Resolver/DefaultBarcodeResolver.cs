// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DefaultBarcodeResolver.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Interpreter.Resolver;

public sealed class DefaultBarcodeResolver : CommandResolver<BarcodeCommand>
{
    #region Constructors

    public DefaultBarcodeResolver(Encoding enconder, IReadOnlyDictionary<CmdEscPos, byte[]> dict) : base(dict)
    {
        Enconder = enconder;
    }

    #endregion Constructors

    #region Properties

    public Encoding Enconder { get; }

    #endregion Properties

    #region Methods

    public override byte[] Resolve(BarcodeCommand command)
    {
        if (!Commandos.ContainsKey(CmdEscPos.IniciarBarcode)) return new byte[0];

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

        // Formando o codigo de barras
        byte[] barCode;
        switch (command.Tipo)
        {
            case CmdBarcode.UPCA when Commandos.ContainsKey(CmdEscPos.BarcodeUPCA):
                barCode = Commandos[CmdEscPos.BarcodeUPCA];
                break;

            case CmdBarcode.UPCE when Commandos.ContainsKey(CmdEscPos.BarcodeUPCE):
                barCode = Commandos[CmdEscPos.BarcodeUPCE];
                break;

            case CmdBarcode.EAN13 when Commandos.ContainsKey(CmdEscPos.BarcodeEAN13):
                barCode = Commandos[CmdEscPos.BarcodeEAN13];
                break;

            case CmdBarcode.EAN8 when Commandos.ContainsKey(CmdEscPos.BarcodeEAN8):
                barCode = Commandos[CmdEscPos.BarcodeEAN8];
                break;

            case CmdBarcode.CODE39 when Commandos.ContainsKey(CmdEscPos.BarcodeCODE39):
                barCode = Commandos[CmdEscPos.BarcodeCODE39];
                break;

            case CmdBarcode.Inter2of5 when Commandos.ContainsKey(CmdEscPos.BarcodeInter2of5):
                barCode = Commandos[CmdEscPos.BarcodeInter2of5];
                break;

            case CmdBarcode.CodaBar when Commandos.ContainsKey(CmdEscPos.BarcodeCodaBar):
                barCode = Commandos[CmdEscPos.BarcodeCodaBar];
                break;

            case CmdBarcode.CODE93 when Commandos.ContainsKey(CmdEscPos.BarcodeCODE93):
                barCode = Commandos[CmdEscPos.BarcodeCODE93];
                break;

            case CmdBarcode.CODE128b when Commandos.ContainsKey(CmdEscPos.BarcodeCODE128):
            case CmdBarcode.CODE128 when Commandos.ContainsKey(CmdEscPos.BarcodeCODE128):
                barCode = Commandos[CmdEscPos.BarcodeCODE128];
                if (!command.Code.StartsWith("{"))
                    command.Code = "{B" + command.Code;
                break;

            case CmdBarcode.CODE128a when Commandos.ContainsKey(CmdEscPos.BarcodeCODE128):
                barCode = Commandos[CmdEscPos.BarcodeCODE128];
                if (!command.Code.StartsWith("{"))
                    command.Code = "{A" + command.Code;
                break;

            case CmdBarcode.CODE128c when Commandos.ContainsKey(CmdEscPos.BarcodeCODE128):
                barCode = Commandos[CmdEscPos.BarcodeCODE128];
                var code = command.Code.OnlyNumbers();
                var s = code.Length;
                if (s % 2 != 0)
                {
                    code = "0" + code;
                    s++;
                }

                var code128c = "";
                var i = 0;
                while (i < s)
                {
                    code128c += (char)code.Substring(i, 2).ToInt32();
                    i += 2;
                }

                command.Code = "{C" + code128c;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        var largura = command.Largura == 0 ? (byte)2 : Math.Max(Math.Min((byte)command.Largura, (byte)4), (byte)1);
        var altura = command.Altura == 0 ? (byte)50 : Math.Max(Math.Min((byte)command.Altura, (byte)255), (byte)1);

        var showCode = command.Exibir switch
        {
            CmdBarcodeText.SemTexto when Commandos.ContainsKey(CmdEscPos.BarcodeNoText) => Commandos[CmdEscPos.BarcodeNoText],
            CmdBarcodeText.Acima when Commandos.ContainsKey(CmdEscPos.BarcodeTextAbove) => Commandos[CmdEscPos.BarcodeTextAbove],
            CmdBarcodeText.Abaixo when Commandos.ContainsKey(CmdEscPos.BarcodeTextBelow) => Commandos[CmdEscPos.BarcodeTextBelow],
            CmdBarcodeText.Ambos when Commandos.ContainsKey(CmdEscPos.BarcodeTextBoth) => Commandos[CmdEscPos.BarcodeTextBoth],
            _ => new byte[0]
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