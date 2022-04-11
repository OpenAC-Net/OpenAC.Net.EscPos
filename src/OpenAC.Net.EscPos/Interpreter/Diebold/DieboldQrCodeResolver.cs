// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DieboldQrCodeResolver.cs" company="OpenAC .Net">
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

using System.Collections.Generic;
using System.Text;
using OpenAC.Net.Devices.Commom;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Extensions;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Diebold;

public sealed class DieboldQrCodeResolver : CommandResolver<QrCodeCommand>
{
    #region Constructors

    public DieboldQrCodeResolver(IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
    {
    }

    #endregion Constructors

    #region Methods

    public override byte[] Resolve(QrCodeCommand command)
    {
        var num = command.Code.Length + 3;
        var pL = (byte)(num % 256);
        var pH = (byte)(num / 256);

        var initial = Commandos[CmdEscPos.QrCodeInitial];
        using var builder = new ByteArrayBuilder();
        builder.Append(initial, new byte[] { 3, 0, (byte)'1', (byte)'B' });
        builder.Append(command.Alinhamento == CmdAlinhamento.Esquerda ? (byte)0 : (byte)1); // 0 - A esquerda, 1 - Centralizar
        builder.Append(initial, Commandos[CmdEscPos.QrCodeSize], (byte)command.LarguraModulo); // Error Level
        builder.Append(initial, Commandos[CmdEscPos.QrCodeError], (byte)command.ErrorLevel);
        builder.Append(initial, pL, pH, Commandos[CmdEscPos.QrCodeStore]);
        builder.Append(Encoding.UTF8.GetBytes(command.Code));
        builder.Append(initial, Commandos[CmdEscPos.QrCodePrint]);
        return builder.ToArray();
    }

    #endregion Methods
}