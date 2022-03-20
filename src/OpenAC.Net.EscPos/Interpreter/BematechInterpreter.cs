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

using System;
using System.Text;
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Interpreter
{
    /// <summary>
    /// WIP - Work In Progress
    /// </summary>
    public class BematechInterpreter : EscPosInterpreter
    {
        #region Constructors

        public BematechInterpreter(Encoding enconder) : base(enconder)
        {
        }

        #endregion Constructors

        #region Methods

        public override byte[][] GetStatusCommand() => throw new NotImplementedException();

        public override EscPosTipoStatus ProcessarStatus(byte[][] dados) => throw new NotImplementedException();

        /// <inheritdoc />
        protected override void InicializarComandos()
        {
            Commandos.Add(CmdEscPos.Zera, new[] { CmdConst.ESC, (byte)'@' });
            Commandos.Add(CmdEscPos.Beep, new byte[] { CmdConst.ESC, (byte)'(', (byte)'A', 4, 0, 1, 2, 1, 0 });
            Commandos.Add(CmdEscPos.EspacoEntreLinhasPadrao, new byte[] { CmdConst.ESC, 2 });
            Commandos.Add(CmdEscPos.EspacoEntreLinhas, new byte[] { CmdConst.ESC, 3 });
            Commandos.Add(CmdEscPos.PaginaDeCodigo, new[] { CmdConst.ESC, (byte)'t' });
            Commandos.Add(CmdEscPos.FonteNormal, new byte[] { CmdConst.ESC, (byte)'!', 0, CmdConst.ESC, (byte)'H', CmdConst.ESC, 5 });
            Commandos.Add(CmdEscPos.FonteA, new[] { CmdConst.ESC, (byte)'H' });
            Commandos.Add(CmdEscPos.FonteB, new[] { CmdConst.ESC, CmdConst.SI });
            Commandos.Add(CmdEscPos.AlinhadoEsquerda, new byte[] { CmdConst.ESC, (byte)'a', 0 });
            Commandos.Add(CmdEscPos.AlinhadoCentro, new byte[] { CmdConst.ESC, (byte)'a', 1 });
            Commandos.Add(CmdEscPos.AlinhadoDireita, new byte[] { CmdConst.ESC, (byte)'a', 2 });
            Commandos.Add(CmdEscPos.LigaExpandido, new byte[] { CmdConst.ESC, (byte)'W', 1 });
            Commandos.Add(CmdEscPos.DesligaExpandido, new byte[] { CmdConst.ESC, (byte)'W', 0 });
            Commandos.Add(CmdEscPos.LigaCondensado, new[] { CmdConst.ESC, CmdConst.SI });
            Commandos.Add(CmdEscPos.DesligaCondensado, new[] { CmdConst.ESC, (byte)'H' });
            Commandos.Add(CmdEscPos.LigaNegrito, new[] { CmdConst.ESC, (byte)'E' });
            Commandos.Add(CmdEscPos.DesligaNegrito, new[] { CmdConst.ESC, (byte)'F' });
            Commandos.Add(CmdEscPos.LigaSublinhado, new byte[] { CmdConst.ESC, (byte)'-', 1 });
            Commandos.Add(CmdEscPos.DesligaSublinhado, new byte[] { CmdConst.ESC, (byte)'-', 0 });
            Commandos.Add(CmdEscPos.LigaAlturaDupla, new byte[] { CmdConst.ESC, (byte)'d', 1 });
            Commandos.Add(CmdEscPos.DesligaAlturaDupla, new byte[] { CmdConst.ESC, (byte)'d', 0 });
            Commandos.Add(CmdEscPos.CorteTotal, new[] { CmdConst.ESC, (byte)'w' });
            Commandos.Add(CmdEscPos.CorteParcial, new[] { CmdConst.ESC, (byte)'m' });
            Commandos.Add(CmdEscPos.PuloDeLinha, new[] { CmdConst.LF });
        }

        #endregion Methods
    }
}