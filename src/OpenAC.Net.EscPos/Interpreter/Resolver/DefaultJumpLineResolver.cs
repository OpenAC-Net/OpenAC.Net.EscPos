﻿// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DefaultJumpLineResolver.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Interpreter.Resolver;

/// <summary>
/// Resolve o comando de pulo de linha (JumpLineCommand) para o formato de bytes ESC/POS.
/// </summary>
public sealed class DefaultJumpLineResolver : CommandResolver<JumpLineCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância de <see cref="DefaultJumpLineResolver"/>.
    /// </summary>
    /// <param name="dictionary">Dicionário de comandos ESC/POS.</param>
    public DefaultJumpLineResolver(IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
    {
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Resolve o comando <see cref="JumpLineCommand"/> para um array de bytes.
    /// </summary>
    /// <param name="command">O comando de pulo de linha.</param>
    /// <returns>Array de bytes correspondente ao comando.</returns>
    public override byte[] Resolve(JumpLineCommand command)
    {
        if (!Commandos.ContainsKey(CmdEscPos.PuloDeLinha)) return [];

        var linhas = Math.Max(1, command.Linhas);
        using var builder = new ByteArrayBuilder();
        for (var i = 0; i < linhas; i++)
            builder.Append(Commandos[CmdEscPos.PuloDeLinha]);

        return builder.ToArray();
    }

    #endregion Methods
}