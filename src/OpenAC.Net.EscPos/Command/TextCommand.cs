﻿// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="TextCommand.cs" company="OpenAC .Net">
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
/// Representa um comando de impressão de texto para impressoras ESC/POS.
/// </summary>
public sealed class TextCommand : PrintCommand<TextCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="TextCommand"/>.
    /// </summary>
    /// <param name="interpreter">O interpretador ESC/POS associado.</param>
    public TextCommand(EscPosInterpreter interpreter) : base(interpreter)
    {
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtém ou define o texto a ser impresso.
    /// </summary>
    public string? Texto { get; set; }

    /// <summary>
    /// Obtém ou define a fonte do texto.
    /// </summary>
    public CmdFonte Fonte { get; set; } = CmdFonte.Normal;

    /// <summary>
    /// Obtém ou define o tamanho da fonte do texto.
    /// </summary>
    public CmdTamanhoFonte Tamanho { get; set; } = CmdTamanhoFonte.Normal;

    /// <summary>
    /// Obtém ou define o alinhamento do texto.
    /// </summary>
    public CmdAlinhamento Alinhamento { get; set; } = CmdAlinhamento.Esquerda;

    /// <summary>
    /// Obtém ou define o estilo da fonte do texto.
    /// </summary>
    public CmdEstiloFonte? Estilo { get; set; } = null;

    #endregion Properties
}