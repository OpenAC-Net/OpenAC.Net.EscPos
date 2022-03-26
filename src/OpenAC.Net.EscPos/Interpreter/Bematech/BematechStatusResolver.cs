﻿// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="BematechInterpreter.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Interpreter.Bematech
{
    public sealed class BematechStatusResolver : StatusResolver
    {
        public BematechStatusResolver() :
            base(new[] { new byte[] { CmdConst.GS, 248, (byte)'1' } },
                dados =>
                {
                    if (dados.IsNullOrEmpty()) return EscPosTipoStatus.ErroLeitura;
                    if (dados.Length < 1) return EscPosTipoStatus.ErroLeitura;
                    if (dados[0].Length < 2) return EscPosTipoStatus.ErroLeitura;

                    var bitTest = new Func<int, byte, bool>((value, index) => ((value >> index) & 1) == 1);

                    EscPosTipoStatus? status = null;
                    var b = dados[0][0];
                    if (bitTest(b, 2))
                        status |= EscPosTipoStatus.Imprimindo;
                    if (bitTest(b, 3))
                        status |= EscPosTipoStatus.OffLine;
                    if (bitTest(b, 4))
                        status |= EscPosTipoStatus.Imprimindo;

                    b = dados[0][1];
                    if (bitTest(b, 1))
                        status |= EscPosTipoStatus.PoucoPapel;
                    if (bitTest(b, 2))
                        status |= EscPosTipoStatus.SemPapel;
                    if (!bitTest(b, 4))
                        status |= EscPosTipoStatus.GavetaAberta;
                    if (bitTest(b, 5))
                        status |= EscPosTipoStatus.SemPapel;
                    if (bitTest(b, 6))
                        status |= EscPosTipoStatus.Erro;
                    if (!bitTest(b, 7))
                        status |= EscPosTipoStatus.TampaAberta;

                    return status ?? EscPosTipoStatus.ErroLeitura;
                })
        {
        }
    }
}