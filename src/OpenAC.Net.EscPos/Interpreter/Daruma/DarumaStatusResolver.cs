// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DarumaStatusResolver.cs" company="OpenAC .Net">
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
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Daruma
{
    public sealed class DarumaStatusResolver : InfoResolver<EscPosTipoStatus>
    {
        public DarumaStatusResolver() :
            base(new[] { new byte[] { CmdConst.ENQ }, new byte[] { CmdConst.GS, CmdConst.ENQ } },
                dados =>
                {
                    if (dados.IsNullOrEmpty()) return EscPosTipoStatus.ErroLeitura;

                    EscPosTipoStatus? status = null;

                    var bitTest = new Func<int, byte, bool>((value, index) => ((value >> index) & 1) == 1);

                    var b = dados[0][0];
                    if (bitTest(b, 0))
                        status = EscPosTipoStatus.Imprimindo;

                    if (bitTest(b, 3))
                        if (status.HasValue)
                            status |= EscPosTipoStatus.Erro;
                        else
                            status = EscPosTipoStatus.Erro;

                    if (!bitTest(b, 4))
                        if (status.HasValue)
                            status |= EscPosTipoStatus.OffLine;
                        else
                            status = EscPosTipoStatus.OffLine;

                    if (bitTest(b, 5))
                        if (status.HasValue)
                            status |= EscPosTipoStatus.SemPapel;
                        else
                            status = EscPosTipoStatus.SemPapel;

                    if (bitTest(b, 7))
                        if (status.HasValue)
                            status |= EscPosTipoStatus.TampaAberta;
                        else
                            status = EscPosTipoStatus.TampaAberta;

                    b = dados[0][1];
                    if (bitTest(b, 0))
                        if (status.HasValue)
                            status |= EscPosTipoStatus.PoucoPapel;
                        else
                            status = EscPosTipoStatus.PoucoPapel;

                    if (bitTest(b, 1))
                        if (status.HasValue)
                            status |= EscPosTipoStatus.SemPapel;
                        else
                            status = EscPosTipoStatus.SemPapel;

                    if (bitTest(b, 3))
                        if (status.HasValue)
                            status |= EscPosTipoStatus.OffLine;
                        else
                            status = EscPosTipoStatus.OffLine;

                    if (!bitTest(b, 4))
                        if (status.HasValue)
                            status |= EscPosTipoStatus.TampaAberta;
                        else
                            status = EscPosTipoStatus.TampaAberta;

                    if (bitTest(b, 6))
                        if (status.HasValue)
                            status |= EscPosTipoStatus.Erro;
                        else
                            status = EscPosTipoStatus.Erro;

                    if (bitTest(b, 7))
                        if (status.HasValue)
                            status |= EscPosTipoStatus.GavetaAberta;
                        else
                            status = EscPosTipoStatus.GavetaAberta;

                    return status ?? EscPosTipoStatus.Nenhum;
                })
        {
        }
    }
}