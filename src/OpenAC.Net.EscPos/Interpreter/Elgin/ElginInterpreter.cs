﻿// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="ElginInterpreter.cs" company="OpenAC .Net">
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
using OpenAC.Net.EscPos.Interpreter.Bematech;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Elgin;

/// <summary>
/// Interpretador ESC/POS específico para impressoras Elgin.
/// </summary>
public sealed class ElginInterpreter : EscPosInterpreter
{
    #region Methods

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ElginInterpreter"/>.
    /// </summary>
    /// <param name="enconder">Codificação de caracteres a ser utilizada.</param>
    internal ElginInterpreter(Encoding enconder) : base(enconder)
    {
    }

    #endregion Methods

    #region Methods

    /// <summary>
    /// Inicializa os resolvers específicos da Elgin.
    /// </summary>
    protected override void IniciarInterpreter()
    {
        Status = new ElginStatusResolver();
        InfoImpressora = new BemaInfoImpressoraResolver(Enconder);

        // Default
        CommandResolver.AddResolver<TextCommand, DefaultTextResolver>(new DefaultTextResolver(Enconder, DefaultCommands.EscBema));
        CommandResolver.AddResolver<TextSliceCommand, DefaultTextSliceResolver>(new DefaultTextSliceResolver(Enconder, DefaultCommands.EscBema));
        CommandResolver.AddResolver<ZeraCommand, DefaultZeraResolver>(new DefaultZeraResolver(DefaultCommands.EscBema));
        CommandResolver.AddResolver<EspacoEntreLinhasCommand, DefaultEspacoEntreLinhasResolver>(new DefaultEspacoEntreLinhasResolver(DefaultCommands.EscBema));
        CommandResolver.AddResolver<PrintLineCommand, DefaultPrintLineResolver>(new DefaultPrintLineResolver(Enconder, DefaultCommands.EscBema));
        CommandResolver.AddResolver<JumpLineCommand, DefaultJumpLineResolver>(new DefaultJumpLineResolver(DefaultCommands.EscBema));
        CommandResolver.AddResolver<CutCommand, DefaultCutResolver>(new DefaultCutResolver(DefaultCommands.EscBema));
        CommandResolver.AddResolver<BeepCommand, DefaultBeepResolver>(new DefaultBeepResolver(DefaultCommands.EscBema));
        CommandResolver.AddResolver<ImageCommand, DefaultImageResolver>(new DefaultImageResolver(DefaultCommands.EscBema));

        // Iguais da Bema
        CommandResolver.AddResolver<CodePageCommand, BemaCodePageResolver>(new BemaCodePageResolver(DefaultCommands.EscBema));
        CommandResolver.AddResolver<BarcodeCommand, BemaBarcodeCommandResolver>(new BemaBarcodeCommandResolver(Enconder, DefaultCommands.EscBema));
        CommandResolver.AddResolver<LogoCommand, BemaLogoCommandResolver>(new BemaLogoCommandResolver(DefaultCommands.EscBema));

        // Custons
        CommandResolver.AddResolver<CashDrawerCommand, ElginCashDrawerResolver>(new ElginCashDrawerResolver(DefaultCommands.EscBema));
        CommandResolver.AddResolver<QrCodeCommand, BemaQrCodeCommandResolver>(new BemaQrCodeCommandResolver(DefaultCommands.EscBema));
    }

    #endregion Methods
}