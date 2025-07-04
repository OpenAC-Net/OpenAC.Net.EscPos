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

using OpenAC.Net.Core.Extensions;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Bematech;

/// <summary>
/// Resolve o status da impressora Bematech a partir dos dados retornados pelo comando de status.
/// </summary>
public sealed class BematechStatusResolver : InfoResolver<EscPosTipoStatus>
{
    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="BematechStatusResolver"/>.
    /// </summary>
    public BematechStatusResolver() :
        base([[CmdConst.GS, 248, (byte)'1']],
            dados =>
            {
                // Verifica se os dados recebidos são válidos
                if (dados.IsNullOrEmpty() || dados.Length < 1 || dados[0].Length < 2) return EscPosTipoStatus.ErroLeitura;

                EscPosTipoStatus? status = null;
                var b = dados[0][0];

                // Verifica os bits do primeiro byte de status
                if (b.IsBitOn(2))
                    status = EscPosTipoStatus.Imprimindo;
                if (b.IsBitOn(3))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.OffLine;
                    else
                        status = EscPosTipoStatus.OffLine;
                if (b.IsBitOn(4))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.Imprimindo;
                    else
                        status = EscPosTipoStatus.Imprimindo;

                b = dados[0][1];

                // Verifica os bits do segundo byte de status
                if (b.IsBitOn(1))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.PoucoPapel;
                    else
                        status = EscPosTipoStatus.PoucoPapel;
                if (b.IsBitOn(2))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.SemPapel;
                    else
                        status = EscPosTipoStatus.SemPapel;
                if (b.IsBitOff(4))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.GavetaAberta;
                    else
                        status = EscPosTipoStatus.GavetaAberta;
                if (b.IsBitOn(5))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.SemPapel;
                    else
                        status = EscPosTipoStatus.SemPapel;
                if (b.IsBitOn(6))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.Erro;
                    else
                        status = EscPosTipoStatus.Erro;
                if (b.IsBitOff(7))
                    if (status.HasValue)
                        status |= EscPosTipoStatus.TampaAberta;
                    else
                        status = EscPosTipoStatus.TampaAberta;

                return status ?? EscPosTipoStatus.Nenhum;
            })
    {
    }
}