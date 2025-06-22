// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="TextSliceCommand.cs" company="OpenAC .Net">
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
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter;

namespace OpenAC.Net.EscPos.Command;

/// <summary>
/// Representa um comando de impressão que manipula fatias de texto.
/// </summary>
public sealed class TextSliceCommand : PrintCommand<TextSliceCommand>
{
    #region Fields

    /// <summary>
    /// Lista de fatias de texto.
    /// </summary>
    private readonly List<TextSlice> slices;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="TextSliceCommand"/>.
    /// </summary>
    /// <param name="interpreter">O interpretador ESC/POS associado.</param>
    public TextSliceCommand(EscPosInterpreter interpreter) : base(interpreter)
    {
        slices = [];
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtém as fatias de texto adicionadas ao comando.
    /// </summary>
    public IReadOnlyCollection<TextSlice> Slices => slices.AsReadOnly();

    /// <summary>
    /// Obtém ou define a fonte do texto.
    /// </summary>
    public CmdFonte Fonte { get; set; } = CmdFonte.Normal;

    /// <summary>
    /// Obtém ou define o tamanho da fonte.
    /// </summary>
    public CmdTamanhoFonte Tamanho { get; set; } = CmdTamanhoFonte.Normal;

    /// <summary>
    /// Obtém ou define o alinhamento do texto.
    /// </summary>
    public CmdAlinhamento Alinhamento { get; set; } = CmdAlinhamento.Esquerda;

    #endregion Properties

    #region Methods

    /// <summary>
    /// Adiciona uma fatia de texto à lista.
    /// </summary>
    /// <param name="slice">A fatia de texto a ser adicionada.</param>
    public void AddSlice(TextSlice slice) => slices.Add(slice);

    #endregion Methods
}