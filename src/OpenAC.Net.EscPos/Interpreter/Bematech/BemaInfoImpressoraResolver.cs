﻿// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="BemaInfoImpressoraResolver.cs" company="OpenAC .Net">
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
using System.Text;
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Bematech;

public sealed class BemaInfoImpressoraResolver : InfoResolver<InformacoesImpressora>
{
    public BemaInfoImpressoraResolver(Encoding encoding) :
        base(new[] { new byte[] { CmdConst.GS, 249, 39, 0 }, new byte[] { CmdConst.GS, 249, 39, 3 }, new byte[] { CmdConst.GS, 249, 1 }, new byte[] { CmdConst.GS, 249, 39, 49 } },
            (dados) =>
            {
                if (dados.IsNullOrEmpty()) return InformacoesImpressora.Empty;
                if (dados.Length < 4) return InformacoesImpressora.Empty;

                var fabricante = string.Empty;
                var modelo = dados[0].IsNullOrEmpty() ? "" : encoding.GetString(dados[0]).Trim().TrimStart('_').Replace("\0", string.Empty);
                var firmware = dados[1].IsNullOrEmpty() ? "" : encoding.GetString(dados[1]).Trim().TrimStart('_').Replace("\0", string.Empty);
                var serial = dados[2].IsNullOrEmpty() ? "" : encoding.GetString(dados[2]).Trim().TrimStart('_').Replace("\0", string.Empty);
                var guilhotina = !dados[3].IsNullOrEmpty() && dados[3].Length >= 3 && dados[3][2].IsBitOn(2);

                return new InformacoesImpressora(fabricante, modelo, firmware, serial, guilhotina);
            })
    { }
}