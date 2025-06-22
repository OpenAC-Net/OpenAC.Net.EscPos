// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="QrCodeCommand.cs" company="OpenAC .Net">
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

using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter;

namespace OpenAC.Net.EscPos.Command;

/// <summary>
/// Representa o comando para impressão de QR Code.
/// </summary>
public sealed class QrCodeCommand : PrintCommand<QrCodeCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="QrCodeCommand"/>.
    /// </summary>
    /// <param name="interpreter">O interpretador ESC/POS.</param>
    public QrCodeCommand(EscPosInterpreter interpreter) : base(interpreter)
    {
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtém ou define o conteúdo do QR Code.
    /// </summary>
    public string Code { get; set; } = "";

    /// <summary>
    /// Obtém ou define o alinhamento do QR Code.
    /// </summary>
    public CmdAlinhamento Alinhamento { get; set; } = CmdAlinhamento.Esquerda;

    /// <summary>
    /// Obtém ou define o tipo do QR Code.
    /// </summary>
    public QrCodeTipo Tipo { get; set; }

    /// <summary>
    /// Obtém ou define a largura do módulo do QR Code.
    /// </summary>
    public QrCodeModSize LarguraModulo { get; set; } = QrCodeModSize.Normal;

    /// <summary>
    /// Obtém ou define o nível de correção de erro do QR Code.
    /// </summary>
    public QrCodeErrorLevel ErrorLevel { get; set; }

    #endregion Properties
}