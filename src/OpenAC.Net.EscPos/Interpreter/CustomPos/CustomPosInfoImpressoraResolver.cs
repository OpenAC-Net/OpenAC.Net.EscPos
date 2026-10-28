// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="CustomPosInfoImpressoraResolver.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Interpreter.CustomPos;

/// <summary>
/// Resolver para obter informações da impressora CustomPos.
/// </summary>
public sealed class CustomPosInfoImpressoraResolver : InfoResolver<InformacoesImpressora>
{
    /// <summary>
    /// Inicializa uma nova instância de <see cref="CustomPosInfoImpressoraResolver"/>.
    /// </summary>
    /// <param name="encoding">Codificação utilizada para decodificar os dados retornados pela impressora.</param>
    public CustomPosInfoImpressoraResolver(Encoding encoding) :
        base([
                [CmdConst.GS, (byte)'I', 3],
                [CmdConst.GS, (byte)'I', 1],
                [CmdConst.GS, (byte)'I', 255],
                [CmdConst.GS, (byte)'I', 5],
                [CmdConst.GS, (byte)'I', 2]
            ],
            dados =>
            {
                if (dados.IsNullOrEmpty() || dados.Length < 5) return InformacoesImpressora.Empty;

                var firmware = dados[0].IsNullOrEmpty() ? "" : encoding.GetString(dados[0]).Trim().Replace("\0", string.Empty);

                var modelo = "";
                if (!dados[1].IsNullOrEmpty())
                {
                    var id = dados[1][0];
                    if ((id == 142 || id == 255) && !dados[2].IsNullOrEmpty() && dados[2].Length >= 2 && dados[2][0] == 2)
                    {
                        modelo = dados[2][1] switch
                        {
                            10 => "K3",
                            41 => "Q3X",
                            79 => "Q3X ETH",
                            _ => ""
                        };
                    }
                    else if ((id == 95 || id == 101) && !dados[3].IsNullOrEmpty())
                    {
                        modelo = dados[3][0] switch
                        {
                            188 => "KUBE II",
                            166 => "KUBE II ETH",
                            _ => ""
                        };
                    }

                    if (modelo.IsEmpty())
                        modelo = encoding.GetString(dados[1]).Trim().Replace("\0", string.Empty);
                }

                var guilhotina = !dados[4].IsNullOrEmpty() && dados[4][0].IsBitOn(1);

                return new InformacoesImpressora("CUSTOM", modelo, firmware, "", guilhotina);
            })
    {
    }
}
