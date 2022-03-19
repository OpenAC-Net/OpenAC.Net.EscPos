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

using OpenAC.Net.EscPos.Command;

namespace OpenAC.Net.EscPos.Interpreter
{
    public class BematechInterpreter : EscPosInterpreter
    {
        /// <inheritdoc />
        protected override void InicializarComandos()
        {
            Commandos.Add(EscPosCommand.Zera, new[] { Command.ESC, (byte)'@' });
            Commandos.Add(EscPosCommand.Beep, new byte[] { Command.ESC, (byte)'(', (byte)'A', 4, 0, 1, 2, 1, 0 });
            Commandos.Add(EscPosCommand.EspacoEntreLinhasPadrao, new byte[] { Command.ESC, 2 });
            Commandos.Add(EscPosCommand.EspacoEntreLinhas, new byte[] { Command.ESC, 3 });
            Commandos.Add(EscPosCommand.PaginaDeCodigo, new[] { Command.ESC, (byte)'t' });
            Commandos.Add(EscPosCommand.FonteNormal, new byte[] { Command.ESC, (byte)'!', 0, Command.ESC, (byte)'H', Command.ESC, 5 });
            Commandos.Add(EscPosCommand.FonteA, new[] { Command.ESC, (byte)'H' });
            Commandos.Add(EscPosCommand.FonteB, new[] { Command.ESC, Command.SI });
            Commandos.Add(EscPosCommand.AlinhadoEsquerda, new byte[] { Command.ESC, (byte)'a', 0 });
            Commandos.Add(EscPosCommand.AlinhadoCentro, new byte[] { Command.ESC, (byte)'a', 1 });
            Commandos.Add(EscPosCommand.AlinhadoDireita, new byte[] { Command.ESC, (byte)'a', 2 });
            Commandos.Add(EscPosCommand.LigaExpandido, new byte[] { Command.ESC, (byte)'W', 1 });
            Commandos.Add(EscPosCommand.DesligaExpandido, new byte[] { Command.ESC, (byte)'W', 0 });
            Commandos.Add(EscPosCommand.LigaCondensado, new[] { Command.ESC, Command.SI });
            Commandos.Add(EscPosCommand.DesligaCondensado, new[] { Command.ESC, (byte)'H' });
            Commandos.Add(EscPosCommand.LigaNegrito, new[] { Command.ESC, (byte)'E' });
            Commandos.Add(EscPosCommand.DesligaNegrito, new[] { Command.ESC, (byte)'F' });
            Commandos.Add(EscPosCommand.LigaSublinhado, new byte[] { Command.ESC, (byte)'-', 1 });
            Commandos.Add(EscPosCommand.DesligaSublinhado, new byte[] { Command.ESC, (byte)'-', 0 });
            Commandos.Add(EscPosCommand.LigaAlturaDupla, new byte[] { Command.ESC, (byte)'d', 1 });
            Commandos.Add(EscPosCommand.DesligaAlturaDupla, new byte[] { Command.ESC, (byte)'d', 0 });
            Commandos.Add(EscPosCommand.CorteTotal, new[] { Command.ESC, (byte)'w' });
            Commandos.Add(EscPosCommand.CorteParcial, new[] { Command.ESC, (byte)'m' });
            Commandos.Add(EscPosCommand.PuloDeLinha, new[] { Command.LF });
        }
    }
}