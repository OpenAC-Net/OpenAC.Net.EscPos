// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DefaultCommands.cs" company="OpenAC .Net">
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
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Interpreter;

public static class DefaultCommands
{
    static DefaultCommands()
    {
        EscPos = new Dictionary<CmdEscPos, byte[]>
        {
            // Diversos
            {CmdEscPos.Zera, new[] {CmdConst.ESC, (byte) '@'}},
            {CmdEscPos.Beep, new byte[] {CmdConst.ESC, (byte) '(', (byte) 'A', 5, 0, 97, 100, 1, 50, 50}},
            {CmdEscPos.EspacoEntreLinhasPadrao, new[] {CmdConst.ESC, (byte)'2'}},
            {CmdEscPos.EspacoEntreLinhas, new[] {CmdConst.ESC, (byte)'3'}},
            {CmdEscPos.PaginaDeCodigo, new[] {CmdConst.ESC, (byte) 't'}},
            {CmdEscPos.PuloDeLinha, new[] {CmdConst.LF}},
            {CmdEscPos.PuloDePagina, new[] {CmdConst.FF}},
            {CmdEscPos.ImprimePagina, new[] {CmdConst.ESC, CmdConst.FF}},

            // Alinhamento
            {CmdEscPos.AlinhadoEsquerda, new byte[] {CmdConst.ESC, (byte) 'a', 0}},
            {CmdEscPos.AlinhadoCentro, new byte[] {CmdConst.ESC, (byte) 'a', 1}},
            {CmdEscPos.AlinhadoDireita, new byte[] {CmdConst.ESC, (byte) 'a', 2}},

            // Fonte
            {CmdEscPos.FonteNormal, new byte[] {CmdConst.ESC, (byte) '!', 0}},
            {CmdEscPos.FonteA, new byte[] {CmdConst.ESC, (byte) 'M', 0}},
            {CmdEscPos.FonteB, new byte[] {CmdConst.ESC, (byte) 'M', 1}},
            {CmdEscPos.LigaExpandido, new byte[] {CmdConst.GS, (byte) '!', 16}},
            {CmdEscPos.DesligaExpandido, new byte[] {CmdConst.GS, (byte) '!', 0}},
            {CmdEscPos.LigaCondensado, new byte[] {CmdConst.ESC, (byte) '!', 1}},
            {CmdEscPos.DesligaCondensado,  new byte[] {CmdConst.ESC, (byte) '!', 0}},
            {CmdEscPos.LigaNegrito, new byte[] {CmdConst.ESC, (byte) 'E', 1}},
            {CmdEscPos.DesligaNegrito, new byte[] {CmdConst.ESC, (byte) 'E', 0}},
            {CmdEscPos.LigaSublinhado, new byte[] {CmdConst.ESC, (byte) '-', 1}},
            {CmdEscPos.DesligaSublinhado, new byte[] {CmdConst.ESC, (byte) '-', 0}},
            //{CmdEscPos.LigaItalico, new byte[] {CmdConst.GS, (byte) 'B', 1}},
            //{CmdEscPos.DesligaItalico, new byte[] {CmdConst.GS, (byte) 'B', 0}},
            {CmdEscPos.LigaInvertido, new byte[] {CmdConst.GS, (byte) 'B', 1}},
            {CmdEscPos.DesligaInvertido, new byte[] {CmdConst.GS, (byte) 'B', 0}},
            {CmdEscPos.LigaAlturaDupla, new byte[] {CmdConst.GS, (byte) '!', 1}},
            {CmdEscPos.DesligaAlturaDupla, new byte[] {CmdConst.GS, (byte) '!', 0}},

            //Corte
            {CmdEscPos.CorteTotal, new byte[] {CmdConst.GS, (byte) 'V', 0}},
            {CmdEscPos.CorteParcial, new byte[] {CmdConst.GS, (byte) 'V', 1}},

            // ModoPagina
            {CmdEscPos.LigaModoPagina, new[] {CmdConst.ESC, (byte) 'L'}},
            {CmdEscPos.DesligaModoPagina, new[] {CmdConst.ESC, (byte) 'S'}},

            // Gaveta
            {CmdEscPos.Gaveta, new[] {CmdConst.ESC, (byte) 'p'}},

            // Barcodes
            {CmdEscPos.IniciarBarcode, new byte[] {CmdConst.GS, 107}},
            {CmdEscPos.BarcodeWidth, new byte[] {CmdConst.GS, 119}},
            {CmdEscPos.BarcodeHeight, new byte[] {CmdConst.GS, 104}},
            {CmdEscPos.BarcodeNoText, new byte[] {CmdConst.GS, 72, 0}},
            {CmdEscPos.BarcodeTextAbove, new byte[] {CmdConst.GS, 72, 1}},
            {CmdEscPos.BarcodeTextBelow, new byte[] {CmdConst.GS, 72, 2}},
            {CmdEscPos.BarcodeTextBoth, new byte[] {CmdConst.GS, 72, 3}},
            {CmdEscPos.BarcodeUPCA, new byte[] {65}},
            {CmdEscPos.BarcodeUPCE, new byte[] {66}},
            {CmdEscPos.BarcodeEAN13, new byte[] {67}},
            {CmdEscPos.BarcodeEAN8, new byte[] {68}},
            {CmdEscPos.BarcodeCODE39, new byte[] {69}},
            {CmdEscPos.BarcodeInter2of5, new byte[] {70}},
            {CmdEscPos.BarcodeCodaBar, new byte[] {71}},
            {CmdEscPos.BarcodeCODE93, new byte[] {72}},
            {CmdEscPos.BarcodeCODE128, new byte[] {73}},

            // Logo
            {CmdEscPos.LogoNew, new byte[] {CmdConst.GS, 40, 76, 6, 0, 48, 69}},
            {CmdEscPos.LogoOld, new byte[] {CmdConst.FS, 112}},

            // QrCode
            {CmdEscPos.QrCodeInitial, new byte[] {CmdConst.GS, 40, 107}},
            {CmdEscPos.QrCodeModel, new byte[] {4, 0, 49, 65}},
            {CmdEscPos.QrCodeSize, new byte[] {3, 0, 49, 67}},
            {CmdEscPos.QrCodeError, new byte[] {3, 0, 49, 69}},
            {CmdEscPos.QrCodeStore, new byte[] {49, 80, 48}},
            {CmdEscPos.QrCodePrint, new byte[] {3, 0, 49, 81, 48}}
        };

        EscBema = new Dictionary<CmdEscPos, byte[]>
        {
            // Diversos
            {CmdEscPos.Zera, new[] {CmdConst.ESC, (byte) '@'}},
            {CmdEscPos.Beep, new byte[] {CmdConst.ESC, (byte) '(', (byte) 'A', 4, 0, 1, 2, 1, 0}},
            {CmdEscPos.EspacoEntreLinhasPadrao, new byte[] { CmdConst.ESC, 2 }},
            {CmdEscPos.EspacoEntreLinhas, new byte[] { CmdConst.ESC, 3 }},
            {CmdEscPos.PaginaDeCodigo, new[] { CmdConst.ESC, (byte)'t' }},
            {CmdEscPos.PuloDeLinha, new[] { CmdConst.LF }},

            // Alinhamento
            {CmdEscPos.AlinhadoEsquerda, new byte[] { CmdConst.ESC, (byte)'a', 0 }},
            {CmdEscPos.AlinhadoCentro, new byte[] { CmdConst.ESC, (byte)'a', 1 }},
            {CmdEscPos.AlinhadoDireita, new byte[] { CmdConst.ESC, (byte)'a', 2 }},

            // Fonte
            {CmdEscPos.FonteNormal, new byte[] { CmdConst.ESC, (byte)'!', 0, CmdConst.ESC, (byte)'H', CmdConst.ESC, 5 }},
            {CmdEscPos.FonteA, new[] { CmdConst.ESC, (byte)'H' }},
            {CmdEscPos.FonteB, new[] { CmdConst.ESC, CmdConst.SI }},
            {CmdEscPos.LigaExpandido, new byte[] { CmdConst.ESC, (byte)'W', 1 }},
            {CmdEscPos.DesligaExpandido, new byte[] { CmdConst.ESC, (byte)'W', 0 }},
            {CmdEscPos.LigaCondensado, new[] { CmdConst.ESC, CmdConst.SI }},
            {CmdEscPos.DesligaCondensado, new[] { CmdConst.ESC, (byte)'H' }},

            {CmdEscPos.LigaNegrito, new[] { CmdConst.ESC, (byte)'E' }},
            {CmdEscPos.DesligaNegrito, new[] { CmdConst.ESC, (byte)'F' }},
            {CmdEscPos.LigaSublinhado, new byte[] { CmdConst.ESC, (byte)'-', 1 }},
            {CmdEscPos.DesligaSublinhado, new byte[] { CmdConst.ESC, (byte)'-', 0 }},
            {CmdEscPos.LigaAlturaDupla, new byte[] { CmdConst.ESC, (byte)'d', 1 }},
            {CmdEscPos.DesligaAlturaDupla, new byte[] { CmdConst.ESC, (byte)'d', 0 }},

            //Corte
            {CmdEscPos.CorteTotal, new[] { CmdConst.ESC, (byte)'w' }},
            {CmdEscPos.CorteParcial, new[] { CmdConst.ESC, (byte)'m' }},

            //// Barcodes
            {CmdEscPos.IniciarBarcode, new byte[] {CmdConst.GS, 107}},
            {CmdEscPos.BarcodeWidth, new byte[] {CmdConst.GS, 119}},
            {CmdEscPos.BarcodeHeight, new byte[] {CmdConst.GS, 104}},
            {CmdEscPos.BarcodeNoText, new byte[] {CmdConst.GS, 72, 0}},
            {CmdEscPos.BarcodeTextAbove, new byte[] {CmdConst.GS, 72, 1}},
            {CmdEscPos.BarcodeTextBelow, new byte[] {CmdConst.GS, 72, 2}},
            {CmdEscPos.BarcodeTextBoth, new byte[] {CmdConst.GS, 72, 3}},
            {CmdEscPos.BarcodeUPCA, new byte[] {65}},
            {CmdEscPos.BarcodeUPCE, new byte[] {66}},
            {CmdEscPos.BarcodeEAN13, new byte[] {67}},
            {CmdEscPos.BarcodeEAN8, new byte[] {68}},
            {CmdEscPos.BarcodeCODE39, new byte[] {69}},
            {CmdEscPos.BarcodeInter2of5, new byte[] {70}},
            {CmdEscPos.BarcodeCodaBar, new byte[] {71}},
            {CmdEscPos.BarcodeCODE93, new byte[] {72}},
            {CmdEscPos.BarcodeCODE128, new byte[] {73}}
        };
    }

    public static IReadOnlyDictionary<CmdEscPos, byte[]> EscPos { get; }

    public static IReadOnlyDictionary<CmdEscPos, byte[]> EscBema { get; }
}