// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DefaultModoPaginaResolver.cs" company="OpenAC .Net">
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
using System.Collections.Generic;
using OpenAC.Net.Devices.Commom;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Extensions;

namespace OpenAC.Net.EscPos.Interpreter.Resolver;

public sealed class DefaultModoPaginaResolver : CommandResolver<ModoPaginaCommand>
{
    #region Constructors

    public DefaultModoPaginaResolver(IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
    {
    }

    #endregion Constructors

    #region Methods

    public override byte[] Resolve(ModoPaginaCommand command)
    {
        using var builder = new ByteArrayBuilder();

        builder.Append(Commandos[CmdEscPos.LigaModoPagina]);

        foreach (var regiao in command.Regioes)
        {
            builder.Append([CmdConst.ESC, (byte)'T', (byte)regiao.Direcao]);

            var espacos = Math.Max((byte)0, regiao.EspacoEntreLinhas);
            if (espacos == 0)
                builder.Append(Commandos[CmdEscPos.EspacoEntreLinhasPadrao]);
            else
                builder.Append(Commandos[CmdEscPos.EspacoEntreLinhas], espacos);

            builder.Append([CmdConst.ESC, (byte)'W']);

            builder.Append((byte)(regiao.Esquerda % 256));
            builder.Append((byte)(regiao.Esquerda / 256));

            builder.Append((byte)(regiao.Topo % 256));
            builder.Append((byte)(regiao.Topo / 256));

            builder.Append((byte)(regiao.Largura % 256));
            builder.Append((byte)(regiao.Largura / 256));

            builder.Append((byte)(regiao.Altura % 256));
            builder.Append((byte)(regiao.Altura / 256));

            foreach (var cmd in regiao.Commands)
                builder.Append(cmd.Content);
        }

        builder.Append(Commandos[CmdEscPos.ImprimePagina]);
        builder.Append(Commandos[CmdEscPos.DesligaModoPagina]);

        return builder.ToArray();
    }

    #endregion Methods
}