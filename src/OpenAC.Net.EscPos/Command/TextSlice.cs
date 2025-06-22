// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="TextSlice.cs" company="OpenAC .Net">
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
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Command;

/// <summary>
/// Representa um segmento de texto com um estilo de fonte opcional.
/// </summary>
public sealed class TextSlice
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância de <see cref="TextSlice"/>.
    /// </summary>
    /// <param name="aTexto">O texto do segmento.</param>
    /// <param name="estilo">O estilo de fonte opcional.</param>
    public TextSlice(string aTexto, CmdEstiloFonte? estilo = null)
    {
        Texto = aTexto.Replace(Environment.NewLine, string.Empty);
        Estilo = estilo;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtém ou define o texto do segmento.
    /// </summary>
    public string Texto { get; set; }

    /// <summary>
    /// Obtém ou define o estilo de fonte do segmento.
    /// </summary>
    public CmdEstiloFonte? Estilo { get; set; }

    #endregion Properties
}