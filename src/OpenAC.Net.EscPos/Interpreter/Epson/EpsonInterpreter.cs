// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="EpsonInterpreter.cs" company="OpenAC .Net">
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

using System.Text;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Epson;

/// <summary>
/// Classe de interpretação com comandos da Epson.
/// </summary>
public class EpsonInterpreter : EscPosInterpreter
{
    #region Constructors

    internal EpsonInterpreter(Encoding enconder) : base(enconder)
    {
    }

    #endregion Constructors

    #region Methods

    /// <inheritdoc />
    protected override void IniciarInterpreter()
    {
        Status = new EpsonStatusResolver();
        InfoImpressora = new EpsonInfoImpressoraResolver(Enconder);

        CommandResolver.AddResolver<CodePageCommand, DefaultCodePageResolver>(new DefaultCodePageResolver(DefaultCommands.EscPos));
        CommandResolver.AddResolver<TextCommand, DefaultTextResolver>(new DefaultTextResolver(Enconder, DefaultCommands.EscPos));
        CommandResolver.AddResolver<ZeraCommand, DefaultZeraResolver>(new DefaultZeraResolver(DefaultCommands.EscPos));
        CommandResolver.AddResolver<EspacoEntreLinhasCommand, DefaultEspacoEntreLinhasResolver>(new DefaultEspacoEntreLinhasResolver(DefaultCommands.EscPos));
        CommandResolver.AddResolver<PrintLineCommand, DefaultPrintLineResolver>(new DefaultPrintLineResolver(Enconder, DefaultCommands.EscPos));
        CommandResolver.AddResolver<JumpLineCommand, DefaultJumpLineResolver>(new DefaultJumpLineResolver(DefaultCommands.EscPos));
        CommandResolver.AddResolver<CutCommand, DefaultCutResolver>(new DefaultCutResolver(DefaultCommands.EscPos));
        CommandResolver.AddResolver<CashDrawerCommand, DefaultCashDrawerResolver>(new DefaultCashDrawerResolver(DefaultCommands.EscPos));
        CommandResolver.AddResolver<BeepCommand, DefaultBeepResolver>(new DefaultBeepResolver(DefaultCommands.EscPos));
        CommandResolver.AddResolver<BarcodeCommand, DefaultBarcodeResolver>(new DefaultBarcodeResolver(Enconder, DefaultCommands.EscPos));
        CommandResolver.AddResolver<LogoCommand, DefaultLogoResolver>(new DefaultLogoResolver(DefaultCommands.EscPos));
        CommandResolver.AddResolver<QrCodeCommand, DefaultQrCodeResolver>(new DefaultQrCodeResolver(DefaultCommands.EscPos));
        CommandResolver.AddResolver<ImageCommand, DefaultImageResolver>(new DefaultImageResolver(DefaultCommands.EscPos));
        CommandResolver.AddResolver<ModoPaginaCommand, DefaultModoPaginaResolver>(new DefaultModoPaginaResolver(DefaultCommands.EscPos));
    }

    #endregion Methods
}