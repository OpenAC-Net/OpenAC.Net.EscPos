// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="BarcodeCommand.cs" company="OpenAC .Net">
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
/// Representa um comando para impressão de código de barras.
/// </summary>
public sealed class BarcodeCommand : PrintCommand<BarcodeCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="BarcodeCommand"/>.
    /// </summary>
    /// <param name="interpreter">O interpretador ESC/POS a ser utilizado.</param>
    public BarcodeCommand(EscPosInterpreter interpreter) : base(interpreter)
    {
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtém ou define o tipo do código de barras.
    /// </summary>
    public CmdBarcode Tipo { get; set; }

    /// <summary>
    /// Obtém ou define o valor do código de barras.
    /// </summary>
    public string Code { get; set; } = "";

    /// <summary>
    /// Obtém ou define se o texto será exibido junto ao código de barras.
    /// </summary>
    public CmdBarcodeText Exibir { get; set; } = CmdBarcodeText.SemTexto;

    /// <summary>
    /// Obtém ou define o alinhamento do código de barras.
    /// </summary>
    public CmdAlinhamento Alinhamento { get; set; } = CmdAlinhamento.Esquerda;

    /// <summary>
    /// Obtém ou define a altura do código de barras.
    /// </summary>
    public int Altura { get; set; } = 0;

    /// <summary>
    /// Obtém ou define a largura do código de barras.
    /// </summary>
    public int Largura { get; set; } = 0;

    #endregion Properties
}