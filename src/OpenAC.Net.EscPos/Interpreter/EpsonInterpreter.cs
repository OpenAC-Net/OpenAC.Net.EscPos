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
using OpenAC.Net.EscPos.Command;

namespace OpenAC.Net.EscPos.Interpreter
{
    /// <summary>
    /// Classe de interpretação com comandos da Epson.
    /// </summary>
    public class EpsonInterpreter : EscPosInterpreter
    {
        /// <inheritdoc />
        protected override void InicializarComandos()
        {
            Commandos.Add(EscPosCommand.Zera, new[] { Command.ESC, (byte)'@' });
            Commandos.Add(EscPosCommand.Beep, new byte[] { Command.ESC, (byte)'(', (byte)'A', 5, 0, 97, 100, 1, 50, 50 });
            Commandos.Add(EscPosCommand.EspacoEntreLinhasPadrao, new byte[] { Command.ESC, 2 });
            Commandos.Add(EscPosCommand.EspacoEntreLinhas, new byte[] { Command.ESC, 3 });
            Commandos.Add(EscPosCommand.PaginaDeCodigo, new[] { Command.ESC, (byte)'t' });
            Commandos.Add(EscPosCommand.FonteNormal, new byte[] { Command.ESC, (byte)'!', 0 });
            Commandos.Add(EscPosCommand.FonteA, new byte[] { Command.ESC, (byte)'M', 0 });
            Commandos.Add(EscPosCommand.FonteB, new byte[] { Command.ESC, (byte)'M', 1 });
            Commandos.Add(EscPosCommand.AlinhadoEsquerda, new byte[] { Command.ESC, (byte)'a', 0 });
            Commandos.Add(EscPosCommand.AlinhadoCentro, new byte[] { Command.ESC, (byte)'a', 1 });
            Commandos.Add(EscPosCommand.AlinhadoDireita, new byte[] { Command.ESC, (byte)'a', 2 });
            Commandos.Add(EscPosCommand.LigaExpandido, new byte[] { Command.GS, (byte)'!', 16 });
            Commandos.Add(EscPosCommand.DesligaExpandido, new byte[] { Command.GS, (byte)'!', 0 });
            Commandos.Add(EscPosCommand.LigaCondensado, new[] { Command.SI });
            Commandos.Add(EscPosCommand.DesligaCondensado, new[] { Command.DC2 });
            Commandos.Add(EscPosCommand.LigaNegrito, new byte[] { Command.ESC, (byte)'E', 1 });
            Commandos.Add(EscPosCommand.DesligaNegrito, new byte[] { Command.ESC, (byte)'E', 0 });
            Commandos.Add(EscPosCommand.LigaSublinhado, new byte[] { Command.ESC, (byte)'-', 1 });
            Commandos.Add(EscPosCommand.DesligaSublinhado, new byte[] { Command.ESC, (byte)'-', 0 });
            Commandos.Add(EscPosCommand.LigaInvertido, new byte[] { Command.GS, (byte)'B', 1 });
            Commandos.Add(EscPosCommand.DesligaInvertido, new byte[] { Command.GS, (byte)'B', 0 });
            Commandos.Add(EscPosCommand.LigaAlturaDupla, new byte[] { Command.GS, (byte)'!', 1 });
            Commandos.Add(EscPosCommand.DesligaAlturaDupla, new byte[] { Command.GS, (byte)'!', 0 });
            Commandos.Add(EscPosCommand.CorteTotal, new byte[] { Command.GS, (byte)'V', 0 });
            Commandos.Add(EscPosCommand.CorteParcial, new byte[] { Command.GS, (byte)'V', 1 });
            Commandos.Add(EscPosCommand.PuloDeLinha, new[] { Command.LF });
            Commandos.Add(EscPosCommand.PuloDePagina, new[] { Command.FF });
            Commandos.Add(EscPosCommand.LigaModoPagina, new byte[] { Command.ESC, (byte)'L', 0 });
            Commandos.Add(EscPosCommand.DesligaModoPagina, new byte[] { Command.ESC, (byte)'S', 0 });
            Commandos.Add(EscPosCommand.ImprimePagina, new byte[] { Command.ESC, Command.FF });
        }
    }
}