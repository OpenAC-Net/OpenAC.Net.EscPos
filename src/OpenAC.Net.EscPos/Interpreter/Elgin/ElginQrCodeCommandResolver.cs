// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="ElginQrCodeCommandResolver.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Interpreter.Elgin;

/// <summary>
/// Resolve comandos de impressão de QR Code para impressoras Elgin.
/// </summary>
public sealed class ElginQrCodeCommandResolver : CommandResolver<QrCodeCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância de <see cref="ElginQrCodeCommandResolver"/>.
    /// </summary>
    /// <param name="dictionary">Dicionário de comandos ESC/POS.</param>
    public ElginQrCodeCommandResolver(IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
    {
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Resolve o comando de QR Code para o formato de bytes esperado pela impressora Elgin.
    /// </summary>
    /// <param name="command">Comando de QR Code.</param>
    /// <returns>Array de bytes com o comando formatado.</returns>
    /// <exception cref="ArgumentOutOfRangeException">Lançada quando o alinhamento, tipo ou nível de erro é inválido.</exception>
    public override byte[] Resolve(QrCodeCommand command)
    {
        if (!Commandos.ContainsKey(CmdEscPos.QrCodeInitial)) return [];

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

        // Symbol type: 1:Original type 2:Enhanced type(Recommended)
        byte tipo;
        switch (command.Tipo)
        {
            case QrCodeTipo.Model1:
                tipo = 1;
                break;

            case QrCodeTipo.Micro:
            case QrCodeTipo.Model2:
                tipo = 2;
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        byte error;
        switch (command.ErrorLevel)
        {
            case QrCodeErrorLevel.LevelL:
                error = (byte)'L';
                break;

            case QrCodeErrorLevel.LevelM:
                error = (byte)'M';
                break;

            case QrCodeErrorLevel.LevelQ:
                error = (byte)'Q';
                break;

            case QrCodeErrorLevel.LevelH:
                error = (byte)'H';
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        builder.Append([CmdConst.GS, (byte)'o', 0]);  // Define parâmetros do QR Code
        builder.Append((byte)command.LarguraModulo); // Largura do módulo básico
        builder.Append(0); // Modo de idioma: 0:Chinês 1:Japonês
        builder.Append(tipo); // Tipo do símbolo: 1:Original 2:Recomendado
        builder.Append([CmdConst.GS, (byte)'k']); // Código de barras
        builder.Append(11); // Tipo = QRCode. Número de caracteres: 4-928
        builder.Append(error);
        builder.Append('A'); // Modo de entrada de dados: A-automático (Recomendado)
        builder.Append(Encoding.UTF8.GetBytes(command.Code));
        builder.Append(0);

        // Volta alinhamento para Esquerda.
        if (Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda) && command.Alinhamento != CmdAlinhamento.Esquerda)
            builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);

        return builder.ToArray();
    }

    #endregion Methods
}