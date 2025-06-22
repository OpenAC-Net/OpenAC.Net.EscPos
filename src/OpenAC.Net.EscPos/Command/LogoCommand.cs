// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="LogoCommand.cs" company="OpenAC .Net">
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

using OpenAC.Net.EscPos.Interpreter;

namespace OpenAC.Net.EscPos.Command;

/// <summary>
/// Representa o comando para impressão de logotipo na impressora ESC/POS.
/// </summary>
public sealed class LogoCommand : PrintCommand<LogoCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="LogoCommand"/>.
    /// </summary>
    /// <param name="interpreter">O interpretador ESC/POS utilizado pelo comando.</param>
    public LogoCommand(EscPosInterpreter interpreter) : base(interpreter)
    {
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtém ou define o valor KC1 do logotipo.
    /// </summary>
    public byte KC1 { get; set; }

    /// <summary>
    /// Obtém ou define o valor KC2 do logotipo.
    /// </summary>
    public byte KC2 { get; set; }

    /// <summary>
    /// Obtém ou define o fator de multiplicação horizontal.
    /// </summary>
    public byte FatorX { get; set; }

    /// <summary>
    /// Obtém ou define o fator de multiplicação vertical.
    /// </summary>
    public byte FatorY { get; set; }

    #endregion Properties
}