﻿// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DarumaInterpreter.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Interpreter.Daruma;

/// <summary>
/// Interpretador ESC/POS específico para impressoras Daruma.
/// </summary>
public sealed class DarumaInterpreter : EscPosInterpreter
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância de <see cref="DarumaInterpreter"/>.
    /// </summary>
    /// <param name="enconder">Codificação de caracteres utilizada.</param>
    internal DarumaInterpreter(Encoding enconder) : base(enconder)
    {
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Inicializa o interpretador com os comandos e resolvers específicos da Daruma.
    /// </summary>
    protected override void IniciarInterpreter()
    {
        Status = new DarumaStatusResolver();
        InfoImpressora = new DarumaInfoImpressoraResolver(Enconder);
        RazaoColuna.Condensada = 0.8421M;  // 48 / 57

        var commandos = new Dictionary<CmdEscPos, byte[]>
        {
            // Diversos
            {CmdEscPos.Zera, [CmdConst.ESC, (byte) '@'] },
            {CmdEscPos.Beep, [CmdConst.BELL] },
            {CmdEscPos.EspacoEntreLinhasPadrao, [CmdConst.ESC, (byte)'2'] },
            {CmdEscPos.EspacoEntreLinhas, [CmdConst.ESC, (byte)'3'] },
            {CmdEscPos.PuloDeLinha, [CmdConst.LF] },

            // Alinhamento
            {CmdEscPos.AlinhadoEsquerda, [CmdConst.ESC, (byte) 'j', 0] },
            {CmdEscPos.AlinhadoCentro, [CmdConst.ESC, (byte) 'j', 1] },
            {CmdEscPos.AlinhadoDireita, [CmdConst.ESC, (byte) 'j', 2] },

            // Fonte
            {CmdEscPos.FonteNormal, [CmdConst.ESC, (byte) '!', 0, CmdConst.DC2] },
            {CmdEscPos.FonteA, [CmdConst.DC4] },
            {CmdEscPos.FonteB, [CmdConst.ESC, CmdConst.SI] },
            {CmdEscPos.LigaExpandido, [CmdConst.ESC, (byte)'W', 1] },
            {CmdEscPos.DesligaExpandido, [CmdConst.ESC, (byte)'W', 0] },
            {CmdEscPos.LigaCondensado, [CmdConst.ESC, CmdConst.SI] },
            {CmdEscPos.DesligaCondensado, [CmdConst.ESC, CmdConst.DC2, CmdConst.DC4] },
            {CmdEscPos.LigaNegrito, [CmdConst.ESC, (byte) 'E'] },
            {CmdEscPos.DesligaNegrito, [CmdConst.ESC, (byte) 'F'] },
            {CmdEscPos.LigaSublinhado, [CmdConst.ESC, (byte) '-', 1] },
            {CmdEscPos.DesligaSublinhado, [CmdConst.ESC, (byte) '-', 0] },
            {CmdEscPos.LigaItalico, [CmdConst.ESC, (byte)'4', 1] },
            {CmdEscPos.DesligaItalico, [CmdConst.ESC, (byte)'4', 0] },
            {CmdEscPos.LigaAlturaDupla, [CmdConst.ESC, (byte)'w', 1] },
            {CmdEscPos.DesligaAlturaDupla, [CmdConst.ESC, (byte)'w', 0] },

            //Corte
            {CmdEscPos.CorteTotal, [CmdConst.ESC, (byte) 'm'] },
            {CmdEscPos.CorteParcial, [CmdConst.ESC, (byte) 'm'] }
        };

        CommandResolver.AddResolver<TextCommand, DefaultTextResolver>(new DefaultTextResolver(Enconder, commandos));
        CommandResolver.AddResolver<TextSliceCommand, DefaultTextSliceResolver>(new DefaultTextSliceResolver(Enconder, commandos));
        CommandResolver.AddResolver<ZeraCommand, DefaultZeraResolver>(new DefaultZeraResolver(commandos));
        CommandResolver.AddResolver<EspacoEntreLinhasCommand, DefaultEspacoEntreLinhasResolver>(new DefaultEspacoEntreLinhasResolver(commandos));
        CommandResolver.AddResolver<PrintLineCommand, DefaultPrintLineResolver>(new DefaultPrintLineResolver(Enconder, commandos));
        CommandResolver.AddResolver<JumpLineCommand, DefaultJumpLineResolver>(new DefaultJumpLineResolver(commandos));
        CommandResolver.AddResolver<CutCommand, DefaultCutResolver>(new DefaultCutResolver(commandos));
        CommandResolver.AddResolver<BeepCommand, DefaultBeepResolver>(new DefaultBeepResolver(commandos));

        CommandResolver.AddResolver<ImageCommand, DarumaImageResolver>(new DarumaImageResolver(commandos));
        CommandResolver.AddResolver<CashDrawerCommand, DarumaCashDrawerResolver>(new DarumaCashDrawerResolver(commandos));
        CommandResolver.AddResolver<BarcodeCommand, DarumaBarcodeResolver>(new DarumaBarcodeResolver(Enconder, commandos));
        CommandResolver.AddResolver<LogoCommand, DarumaLogoResolver>(new DarumaLogoResolver(commandos));
        CommandResolver.AddResolver<QrCodeCommand, DarumaQrCodeResolver>(new DarumaQrCodeResolver(commandos));
    }

    #endregion Methods
}