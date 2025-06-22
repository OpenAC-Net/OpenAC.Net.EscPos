// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DefaultEspacoEntreLinhasResolver.cs" company="OpenAC .Net">
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
using System.Linq;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Interpreter.Resolver;

/// <summary>
/// Resolve comandos para definir o espaçamento entre linhas na impressora.
/// </summary>
public sealed class DefaultEspacoEntreLinhasResolver : CommandResolver<EspacoEntreLinhasCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância de <see cref="DefaultEspacoEntreLinhasResolver"/>.
    /// </summary>
    /// <param name="dictionary">Dicionário de comandos ESC/POS.</param>
    public DefaultEspacoEntreLinhasResolver(IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
    {
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Resolve o comando <see cref="EspacoEntreLinhasCommand"/> para o formato de bytes ESC/POS.
    /// </summary>
    /// <param name="command">O comando de espaçamento entre linhas.</param>
    /// <returns>Array de bytes correspondente ao comando ESC/POS.</returns>
    public override byte[] Resolve(EspacoEntreLinhasCommand command)
    {
        if (!Commandos.ContainsKey(CmdEscPos.EspacoEntreLinhasPadrao)) return [];

        var espacos = Math.Max((byte)0, command.Espaco);
        if (espacos == 0) return Commandos[CmdEscPos.EspacoEntreLinhasPadrao];

        return !Commandos.TryGetValue(CmdEscPos.EspacoEntreLinhas, out var commando) ? [] : commando.Concat([espacos]).ToArray();
    }

    #endregion Methods
}