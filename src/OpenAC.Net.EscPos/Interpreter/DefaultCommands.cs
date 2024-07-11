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
            {CmdEscPos.Zera, [CmdConst.ESC, (byte) '@'] },
            {CmdEscPos.Beep, [CmdConst.ESC, (byte) '(', (byte) 'A', 5, 0, 97, 100, 1, 50, 50] },
            {CmdEscPos.EspacoEntreLinhasPadrao, [CmdConst.ESC, (byte)'2'] },
            {CmdEscPos.EspacoEntreLinhas, [CmdConst.ESC, (byte)'3'] },
            {CmdEscPos.PaginaDeCodigo, [CmdConst.ESC, (byte) 't'] },
            {CmdEscPos.PuloDeLinha, [CmdConst.LF] },
            {CmdEscPos.PuloDePagina, [CmdConst.FF] },
            {CmdEscPos.ImprimePagina, [CmdConst.ESC, CmdConst.FF] },

            // Alinhamento
            {CmdEscPos.AlinhadoEsquerda, [CmdConst.ESC, (byte) 'a', 0] },
            {CmdEscPos.AlinhadoCentro, [CmdConst.ESC, (byte) 'a', 1] },
            {CmdEscPos.AlinhadoDireita, [CmdConst.ESC, (byte) 'a', 2] },

            // Fonte
            {CmdEscPos.FonteNormal, [CmdConst.ESC, (byte) '!', 0] },
            {CmdEscPos.FonteA, [CmdConst.ESC, (byte) 'M', 0] },
            {CmdEscPos.FonteB, [CmdConst.ESC, (byte) 'M', 1] },
            {CmdEscPos.LigaExpandido, [CmdConst.GS, (byte) '!', 16] },
            {CmdEscPos.DesligaExpandido, [CmdConst.GS, (byte) '!', 0] },
            {CmdEscPos.LigaCondensado, [CmdConst.ESC, (byte) '!', 1] },
            {CmdEscPos.DesligaCondensado, [CmdConst.ESC, (byte) '!', 0] },
            {CmdEscPos.LigaNegrito, [CmdConst.ESC, (byte) 'E', 1] },
            {CmdEscPos.DesligaNegrito, [CmdConst.ESC, (byte) 'E', 0] },
            {CmdEscPos.LigaSublinhado, [CmdConst.ESC, (byte) '-', 1] },
            {CmdEscPos.DesligaSublinhado, [CmdConst.ESC, (byte) '-', 0] },
            //{CmdEscPos.LigaItalico, new byte[] {CmdConst.GS, (byte) 'B', 1}},
            //{CmdEscPos.DesligaItalico, new byte[] {CmdConst.GS, (byte) 'B', 0}},
            {CmdEscPos.LigaInvertido, [CmdConst.GS, (byte) 'B', 1] },
            {CmdEscPos.DesligaInvertido, [CmdConst.GS, (byte) 'B', 0] },
            {CmdEscPos.LigaAlturaDupla, [CmdConst.GS, (byte) '!', 1] },
            {CmdEscPos.DesligaAlturaDupla, [CmdConst.GS, (byte) '!', 0] },

            //Corte
            {CmdEscPos.CorteTotal, [CmdConst.GS, (byte) 'V', 0] },
            {CmdEscPos.CorteParcial, [CmdConst.GS, (byte) 'V', 1] },

            // ModoPagina
            {CmdEscPos.LigaModoPagina, [CmdConst.ESC, (byte) 'L'] },
            {CmdEscPos.DesligaModoPagina, [CmdConst.ESC, (byte) 'S'] },

            // Gaveta
            {CmdEscPos.Gaveta, [CmdConst.ESC, (byte) 'p'] },

            // Barcodes
            {CmdEscPos.IniciarBarcode, [CmdConst.GS, 107] },
            {CmdEscPos.BarcodeWidth, [CmdConst.GS, 119] },
            {CmdEscPos.BarcodeHeight, [CmdConst.GS, 104] },
            {CmdEscPos.BarcodeNoText, [CmdConst.GS, 72, 0] },
            {CmdEscPos.BarcodeTextAbove, [CmdConst.GS, 72, 1] },
            {CmdEscPos.BarcodeTextBelow, [CmdConst.GS, 72, 2] },
            {CmdEscPos.BarcodeTextBoth, [CmdConst.GS, 72, 3] },
            {CmdEscPos.BarcodeUPCA, [65] },
            {CmdEscPos.BarcodeUPCE, [66] },
            {CmdEscPos.BarcodeEAN13, [67] },
            {CmdEscPos.BarcodeEAN8, [68] },
            {CmdEscPos.BarcodeCODE39, [69] },
            {CmdEscPos.BarcodeInter2of5, [70] },
            {CmdEscPos.BarcodeCodaBar, [71] },
            {CmdEscPos.BarcodeCODE93, [72] },
            {CmdEscPos.BarcodeCODE128, [73] },

            // Logo
            {CmdEscPos.LogoNew, [CmdConst.GS, 40, 76, 6, 0, 48, 69] },
            {CmdEscPos.LogoOld, [CmdConst.FS, 112] },

            // QrCode
            {CmdEscPos.QrCodeInitial, [CmdConst.GS, 40, 107] },
            {CmdEscPos.QrCodeModel, [4, 0, 49, 65] },
            {CmdEscPos.QrCodeSize, [3, 0, 49, 67] },
            {CmdEscPos.QrCodeError, [3, 0, 49, 69] },
            {CmdEscPos.QrCodeStore, [49, 80, 48] },
            {CmdEscPos.QrCodePrint, [3, 0, 49, 81, 48] }
        };

        EscBema = new Dictionary<CmdEscPos, byte[]>
        {
            // Diversos
            {CmdEscPos.Zera, [CmdConst.ESC, (byte) '@'] },
            {CmdEscPos.Beep, [CmdConst.ESC, (byte) '(', (byte) 'A', 4, 0, 1, 2, 1, 0] },
            {CmdEscPos.EspacoEntreLinhasPadrao, [CmdConst.ESC, 2] },
            {CmdEscPos.EspacoEntreLinhas, [CmdConst.ESC, 3] },
            {CmdEscPos.PaginaDeCodigo, [CmdConst.ESC, (byte)'t'] },
            {CmdEscPos.PuloDeLinha, [CmdConst.LF] },

            // Alinhamento
            {CmdEscPos.AlinhadoEsquerda, [CmdConst.ESC, (byte)'a', 0] },
            {CmdEscPos.AlinhadoCentro, [CmdConst.ESC, (byte)'a', 1] },
            {CmdEscPos.AlinhadoDireita, [CmdConst.ESC, (byte)'a', 2] },

            // Fonte
            {CmdEscPos.FonteNormal, [CmdConst.ESC, (byte)'!', 0, CmdConst.ESC, (byte)'H', CmdConst.ESC, 5] },
            {CmdEscPos.FonteA, [CmdConst.ESC, (byte)'H'] },
            {CmdEscPos.FonteB, [CmdConst.ESC, CmdConst.SI] },
            {CmdEscPos.LigaExpandido, [CmdConst.ESC, (byte)'W', 1] },
            {CmdEscPos.DesligaExpandido, [CmdConst.ESC, (byte)'W', 0] },
            {CmdEscPos.LigaCondensado, [CmdConst.ESC, CmdConst.SI] },
            {CmdEscPos.DesligaCondensado, [CmdConst.ESC, (byte)'H'] },

            {CmdEscPos.LigaNegrito, [CmdConst.ESC, (byte)'E'] },
            {CmdEscPos.DesligaNegrito, [CmdConst.ESC, (byte)'F'] },
            {CmdEscPos.LigaSublinhado, [CmdConst.ESC, (byte)'-', 1] },
            {CmdEscPos.DesligaSublinhado, [CmdConst.ESC, (byte)'-', 0] },
            {CmdEscPos.LigaAlturaDupla, [CmdConst.ESC, (byte)'d', 1] },
            {CmdEscPos.DesligaAlturaDupla, [CmdConst.ESC, (byte)'d', 0] },

            //Corte
            {CmdEscPos.CorteTotal, [CmdConst.ESC, (byte)'w'] },
            {CmdEscPos.CorteParcial, [CmdConst.ESC, (byte)'m'] },

            //// Barcodes
            {CmdEscPos.IniciarBarcode, [CmdConst.GS, 107] },
            {CmdEscPos.BarcodeWidth, [CmdConst.GS, 119] },
            {CmdEscPos.BarcodeHeight, [CmdConst.GS, 104] },
            {CmdEscPos.BarcodeNoText, [CmdConst.GS, 72, 0] },
            {CmdEscPos.BarcodeTextAbove, [CmdConst.GS, 72, 1] },
            {CmdEscPos.BarcodeTextBelow, [CmdConst.GS, 72, 2] },
            {CmdEscPos.BarcodeTextBoth, [CmdConst.GS, 72, 3] },
            {CmdEscPos.BarcodeUPCA, [65] },
            {CmdEscPos.BarcodeUPCE, [66] },
            {CmdEscPos.BarcodeEAN13, [67] },
            {CmdEscPos.BarcodeEAN8, [68] },
            {CmdEscPos.BarcodeCODE39, [69] },
            {CmdEscPos.BarcodeInter2of5, [70] },
            {CmdEscPos.BarcodeCodaBar, [71] },
            {CmdEscPos.BarcodeCODE93, [72] },
            {CmdEscPos.BarcodeCODE128, [73] }
        };
    }

    public static IReadOnlyDictionary<CmdEscPos, byte[]> EscPos { get; }

    public static IReadOnlyDictionary<CmdEscPos, byte[]> EscBema { get; }
}