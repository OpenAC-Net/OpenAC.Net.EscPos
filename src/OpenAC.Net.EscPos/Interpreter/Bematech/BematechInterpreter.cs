// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="BematechInterpreter.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Interpreter.Bematech
{
    /// <summary>
    /// WIP - Work In Progress
    /// </summary>
    public class BematechInterpreter : EscPosInterpreter
    {
        #region Constructors

        internal BematechInterpreter(Encoding enconder) : base(enconder)
        {
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc />
        protected override void IniciarInterpreter()
        {
            Status = new BematechStatusResolver();
            InfoImpressora = new BemaInfoImpressoraResolver(Enconder);

            CommandResolver.AddResolver<CodePageCommand, BemaCodePageResolver>(new BemaCodePageResolver(DefaultCommands.EscBema));
            CommandResolver.AddResolver<TextCommand, DefaultTextResolver>(new DefaultTextResolver(Enconder, DefaultCommands.EscBema));
            CommandResolver.AddResolver<ZeraCommand, DefaultZeraResolver>(new DefaultZeraResolver(DefaultCommands.EscBema));
            CommandResolver.AddResolver<EspacoEntreLinhasCommand, DefaultEspacoEntreLinhasResolver>(new DefaultEspacoEntreLinhasResolver(DefaultCommands.EscBema));
            CommandResolver.AddResolver<PrintLineCommand, DefaultPrintLineResolver>(new DefaultPrintLineResolver(Enconder, DefaultCommands.EscBema));
            CommandResolver.AddResolver<JumpLineCommand, DefaultJumpLineResolver>(new DefaultJumpLineResolver(DefaultCommands.EscBema));
            CommandResolver.AddResolver<CutCommand, DefaultCutResolver>(new DefaultCutResolver(DefaultCommands.EscBema));
            CommandResolver.AddResolver<BeepCommand, DefaultBeepResolver>(new DefaultBeepResolver(DefaultCommands.EscBema));
            CommandResolver.AddResolver<ImageCommand, DefaultImageResolver>(new DefaultImageResolver(DefaultCommands.EscBema));

            // Custons
            CommandResolver.AddResolver<CashDrawerCommand, BemaCashDrawerCommandResolver>(new BemaCashDrawerCommandResolver(DefaultCommands.EscBema));
            CommandResolver.AddResolver<BarcodeCommand, BemaBarcodeCommandResolver>(new BemaBarcodeCommandResolver(Enconder, DefaultCommands.EscBema));
            CommandResolver.AddResolver<LogoCommand, BemaLogoCommandResolver>(new BemaLogoCommandResolver(DefaultCommands.EscBema));
            CommandResolver.AddResolver<QrCodeCommand, BemaQrCodeCommandResolver>(new BemaQrCodeCommandResolver(DefaultCommands.EscBema));
        }

        #endregion Methods
    }
}