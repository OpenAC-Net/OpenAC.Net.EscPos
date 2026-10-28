// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="EpsonP2Interpreter.cs" company="OpenAC .Net">
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
using System.Text;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.EpsonP2;

/// <summary>
/// Interpretador para impressoras matriciais no padrão Epson ESC/P2.
/// </summary>
public sealed class EpsonP2Interpreter : EscPosInterpreter
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="EpsonP2Interpreter"/>.
    /// </summary>
    /// <param name="enconder">Codificação de caracteres a ser utilizada.</param>
    internal EpsonP2Interpreter(Encoding enconder) : base(enconder)
    {
    }

    #endregion Constructors

    #region Methods

    /// <inheritdoc />
    protected override void IniciarInterpreter()
    {
        var commandos = new Dictionary<CmdEscPos, byte[]>
        {
            { CmdEscPos.Zera, [CmdConst.ESC, (byte)'@', CmdConst.SI] },
            { CmdEscPos.PuloDeLinha, [CmdConst.LF] },
            { CmdEscPos.EspacoEntreLinhasPadrao, [CmdConst.ESC, (byte)'2'] },
            { CmdEscPos.EspacoEntreLinhas, [CmdConst.ESC, (byte)'0'] },
            { CmdEscPos.LigaNegrito, [CmdConst.ESC, (byte)'G'] },
            { CmdEscPos.DesligaNegrito, [CmdConst.ESC, (byte)'H'] },
            { CmdEscPos.LigaExpandido, [CmdConst.ESC, (byte)'W', (byte)'1'] },
            { CmdEscPos.DesligaExpandido, [CmdConst.ESC, (byte)'W', (byte)'0'] },
            { CmdEscPos.LigaSublinhado, [CmdConst.ESC, (byte)'-', 1] },
            { CmdEscPos.DesligaSublinhado, [CmdConst.ESC, (byte)'-', 0] },
            { CmdEscPos.LigaItalico, [CmdConst.ESC, (byte)'4'] },
            { CmdEscPos.DesligaItalico, [CmdConst.ESC, (byte)'5'] },
            { CmdEscPos.LigaCondensado, [CmdConst.SI] },
            { CmdEscPos.DesligaCondensado, [CmdConst.DC2] },
            { CmdEscPos.FonteNormal, [CmdConst.SI] },
            { CmdEscPos.FonteA, [CmdConst.DC2] },
            { CmdEscPos.FonteB, [CmdConst.SI] }
        };

        CommandResolver.AddResolver<TextCommand, DefaultTextResolver>(new DefaultTextResolver(Enconder, commandos));
        CommandResolver.AddResolver<TextSliceCommand, DefaultTextSliceResolver>(new DefaultTextSliceResolver(Enconder, commandos));
        CommandResolver.AddResolver<ZeraCommand, DefaultZeraResolver>(new DefaultZeraResolver(commandos));
        CommandResolver.AddResolver<EspacoEntreLinhasCommand, DefaultEspacoEntreLinhasResolver>(new DefaultEspacoEntreLinhasResolver(commandos));
        CommandResolver.AddResolver<PrintLineCommand, DefaultPrintLineResolver>(new DefaultPrintLineResolver(Enconder, commandos));
        CommandResolver.AddResolver<JumpLineCommand, DefaultJumpLineResolver>(new DefaultJumpLineResolver(commandos));
    }

    #endregion Methods
}
