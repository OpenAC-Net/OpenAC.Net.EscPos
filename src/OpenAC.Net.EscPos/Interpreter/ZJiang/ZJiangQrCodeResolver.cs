// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="ZJiangQrCodeResolver.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Interpreter.ZJiang;

public sealed class ZJiangQrCodeResolver : CommandResolver<QrCodeCommand>
{
    #region Constructors

    public ZJiangQrCodeResolver(IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
    {
    }

    #endregion Constructors

    #region Methods

    public override byte[] Resolve(QrCodeCommand command)
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

        byte largura = command.LarguraModulo switch
        {
            QrCodeModSize.Minusculo => 2,
            QrCodeModSize.Pequeno => 3,
            QrCodeModSize.Normal => 4,
            QrCodeModSize.Grande => 5,
            QrCodeModSize.ExtraGrande => 6,
            _ => throw new ArgumentOutOfRangeException()
        };

        byte erro = command.ErrorLevel switch
        {
            QrCodeErrorLevel.LevelL => (byte)'L',
            QrCodeErrorLevel.LevelM => (byte)'M',
            QrCodeErrorLevel.LevelQ => (byte)'Q',
            QrCodeErrorLevel.LevelH => (byte)'H',
            _ => throw new ArgumentOutOfRangeException()
        };

        var num = command.Code.Length;
        var pL = (byte)(num % 256);
        var pH = (byte)(num / 256);

        builder.Append([CmdConst.ESC, (byte)'Z', 0]);
        builder.Append(erro);
        builder.Append(largura);
        builder.Append([pL, pH]);
        builder.Append(Encoding.UTF8.GetBytes(command.Code));

        // Volta alinhamento para Esquerda.
        if (command.Alinhamento != CmdAlinhamento.Esquerda)
            builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);

        return builder.ToArray();
    }

    #endregion Methods
}