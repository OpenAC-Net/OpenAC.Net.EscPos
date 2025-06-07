// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="ElginStatusResolver.cs" company="OpenAC .Net">
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

using OpenAC.Net.Core.Extensions;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Elgin;

public sealed class ElginStatusResolver : InfoResolver<EscPosTipoStatus>
{
    public ElginStatusResolver() :
        base([[5]],
            dados =>
            {
                if (dados.IsNullOrEmpty()) return EscPosTipoStatus.ErroLeitura;

                EscPosTipoStatus? status = null;

                var b = dados[0][0];
                if (b.IsBitOff(0))
                    status = EscPosTipoStatus.OffLine;

                if (b.IsBitOn(1))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.SemPapel;
                    else
                        status = EscPosTipoStatus.SemPapel;

                if (b.IsBitOn(2))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.GavetaAberta;
                    else
                        status = EscPosTipoStatus.GavetaAberta;

                if (b.IsBitOn(3))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.TampaAberta;
                    else
                        status = EscPosTipoStatus.TampaAberta;

                if (b.IsBitOn(4))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.PoucoPapel;
                    else
                        status = EscPosTipoStatus.PoucoPapel;

                return status ?? EscPosTipoStatus.Nenhum;
            })
    {
    }
}