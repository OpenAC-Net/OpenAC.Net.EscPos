// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="CustomPosQrCodeResolver.cs" company="OpenAC .Net">
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
using OpenAC.Net.EscPos.Extensions;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.CustomPos;

/// <summary>
/// Resolve comandos ESC/POS para impressão de QR Code em impressoras CustomPos.
/// </summary>
public sealed class CustomPosQrCodeResolver : CommandResolver<QrCodeCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância de <see cref="CustomPosQrCodeResolver"/>.
    /// </summary>
    /// <param name="dictionary">Dicionário de comandos ESC/POS.</param>
    public CustomPosQrCodeResolver(IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
    {
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Resolve o comando de QR Code para o formato ESC/POS da CustomPos.
    /// </summary>
    /// <param name="command">Comando de QR Code.</param>
    /// <returns>Array de bytes com o comando ESC/POS.</returns>
    public override byte[] Resolve(QrCodeCommand command)
    {
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

        var codeBytes = Encoding.UTF8.GetBytes(command.Code);
        var num = codeBytes.Length + 3;
        var pL = (byte)(num % 256);
        var pH = (byte)(num / 256);

        var tipo = command.Tipo == QrCodeTipo.Model2 ? (byte)0 : (byte)1;
        var largura = Math.Max((byte)2, (byte)command.LarguraModulo);
        byte errorLevel = command.ErrorLevel switch
        {
            QrCodeErrorLevel.LevelL => 0,
            QrCodeErrorLevel.LevelM => 1,
            QrCodeErrorLevel.LevelQ => 2,
            QrCodeErrorLevel.LevelH => 3,
            _ => 0
        };

        var initial = Commandos[CmdEscPos.QrCodeInitial];
        builder.Append(initial, new byte[] { 3, 0, (byte)'1', (byte)'A', tipo });
        builder.Append(initial, new byte[] { 3, 0, (byte)'1', (byte)'B', largura });
        builder.Append(initial, new byte[] { 3, 0, (byte)'1', (byte)'E', errorLevel });
        builder.Append(initial, pL, pH, new byte[] { (byte)'1', (byte)'P', (byte)'1' });
        builder.Append(codeBytes);
        builder.Append(initial, new byte[] { 3, 0, (byte)'1', (byte)'Q', (byte)'1' });

        if (Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda) && command.Alinhamento != CmdAlinhamento.Esquerda)
            builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);

        return builder.ToArray();
    }

    #endregion Methods
}
