﻿// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DarumaInfoImpressoraResolver.cs" company="OpenAC .Net">
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

using System.Text;
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Daruma;

/// <summary>
/// Resolver para obter informações da impressora Daruma.
/// </summary>
public sealed class DarumaInfoImpressoraResolver : InfoResolver<InformacoesImpressora>
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="DarumaInfoImpressoraResolver"/>.
    /// </summary>
    /// <param name="encoding">Codificação utilizada para decodificar os dados retornados pela impressora.</param>
    public DarumaInfoImpressoraResolver(Encoding encoding) :
        base([[CmdConst.ESC, 195], [CmdConst.ESC, 199], [CmdConst.ESC, 229]],
            (dados) =>
            {
                if (dados.IsNullOrEmpty()) return InformacoesImpressora.Empty;
                if (dados.Length < 3) return InformacoesImpressora.Empty;

                var modelo = dados[0].IsNullOrEmpty() ? "" : encoding.GetString(dados[0]).Trim().TrimStart('_').Replace("\0", string.Empty);
                var firmware = dados[1].IsNullOrEmpty() ? "" : encoding.GetString(dados[1]).Trim().TrimStart('_').Replace("\0", string.Empty);
                var guilhotina = dados[2].IsNullOrEmpty() && encoding.GetString(dados[2]).Substring(8, 1) == "1";

                return new InformacoesImpressora("Daruma", modelo, firmware, "", guilhotina);
            })
    { }
}