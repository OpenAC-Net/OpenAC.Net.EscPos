﻿// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DefaultCutResolver.cs" company="OpenAC .Net">
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

using System.Collections.Generic;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Interpreter.Resolver;

/// <summary>
/// Resolve comandos de corte para impressoras ESC/POS.
/// </summary>
public sealed class DefaultCutResolver : CommandResolver<CutCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância de <see cref="DefaultCutResolver"/>.
    /// </summary>
    /// <param name="dictionary">Dicionário de comandos ESC/POS.</param>
    public DefaultCutResolver(IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
    {
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Resolve o comando de corte baseado no tipo solicitado.
    /// </summary>
    /// <param name="command">Comando de corte.</param>
    /// <returns>Array de bytes do comando ESC/POS correspondente.</returns>
    public override byte[] Resolve(CutCommand command)
    {
        if (!Commandos.ContainsKey(CmdEscPos.CorteParcial) && command.Parcial || 
            !Commandos.TryGetValue(CmdEscPos.CorteTotal, out var commando)) return [];

        return command.Parcial ? Commandos[CmdEscPos.CorteParcial] : commando;
    }

    #endregion Methods
}