// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DefaultLogoResolver.cs" company="OpenAC .Net">
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
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Interpreter.Resolver;

public sealed class DefaultLogoResolver : CommandResolver<LogoCommand>
{
    #region Constructors

    public DefaultLogoResolver(IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
    {
    }

    #endregion Constructors

    #region Methods

    public override byte[] Resolve(LogoCommand command)
    {
        if (!Commandos.ContainsKey(CmdEscPos.LogoNew) &&
            !Commandos.ContainsKey(CmdEscPos.LogoOld)) return new byte[0];

        // Verificando se informou o KeyCode compatível com o comando Novo ou Antigo.

        // Nota: O Comando novo da Epson "GS + '(L'", não é compatível em alguns
        // Equipamentos(não Epson), mas que usam EscPosEpson...
        // Nesse caso, vamos usar o comando "FS + 'p'", para tal, informe:
        // KeyCode1:= 1..255; KeyCode2:= 0

        var keyCodeUnico = new Func<byte, byte>(keycode => (keycode is < 32 or > 126) ? (byte)((char)keycode).ToInt32() : keycode);

        if (command.KC2 != 0)
            return Commandos[CmdEscPos.LogoNew].Concat(new[] { command.KC1, command.KC2, command.FatorX, command.FatorY }).ToArray();

        var keyCode = keyCodeUnico(command.KC1);
        byte m = 0;
        if (command.FatorX > 1)
            m += 1;

        if (command.FatorY > 1)
            m += 2;

        return Commandos[CmdEscPos.LogoOld].Concat(new[] { keyCode, m }).ToArray();
    }

    #endregion Methods
}