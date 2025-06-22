// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="CmdEscPos.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Commom;

/// <summary>
/// Enumeração dos comandos ESC/POS suportados.
/// </summary>
public enum CmdEscPos
{
    /// <summary>Zera impressora.</summary>
    Zera,
    /// <summary>Define espaço entre linhas.</summary>
    EspacoEntreLinhas,
    /// <summary>Define espaço entre linhas padrão.</summary>
    EspacoEntreLinhasPadrao,
    /// <summary>Ativa negrito.</summary>
    LigaNegrito,
    /// <summary>Desativa negrito.</summary>
    DesligaNegrito,
    /// <summary>Ativa expandido.</summary>
    LigaExpandido,
    /// <summary>Desativa expandido.</summary>
    DesligaExpandido,
    /// <summary>Ativa altura dupla.</summary>
    LigaAlturaDupla,
    /// <summary>Desativa altura dupla.</summary>
    DesligaAlturaDupla,
    /// <summary>Ativa sublinhado.</summary>
    LigaSublinhado,
    /// <summary>Desativa sublinhado.</summary>
    DesligaSublinhado,
    /// <summary>Ativa itálico.</summary>
    LigaItalico,
    /// <summary>Desativa itálico.</summary>
    DesligaItalico,
    /// <summary>Ativa condensado.</summary>
    LigaCondensado,
    /// <summary>Desativa condensado.</summary>
    DesligaCondensado,
    /// <summary>Ativa invertido.</summary>
    LigaInvertido,
    /// <summary>Desativa invertido.</summary>
    DesligaInvertido,
    /// <summary>Fonte normal.</summary>
    FonteNormal,
    /// <summary>Fonte A.</summary>
    FonteA,
    /// <summary>Fonte B.</summary>
    FonteB,
    /// <summary>Alinha à esquerda.</summary>
    AlinhadoEsquerda,
    /// <summary>Alinha à direita.</summary>
    AlinhadoDireita,
    /// <summary>Alinha ao centro.</summary>
    AlinhadoCentro,
    /// <summary>Emite beep.</summary>
    Beep,
    /// <summary>Corte total do papel.</summary>
    CorteTotal,
    /// <summary>Corte parcial do papel.</summary>
    CorteParcial,
    /// <summary>Pula linha.</summary>
    PuloDeLinha,
    /// <summary>Pula página.</summary>
    PuloDePagina,
    /// <summary>Ativa modo página.</summary>
    LigaModoPagina,
    /// <summary>Desativa modo página.</summary>
    DesligaModoPagina,
    /// <summary>Imprime página.</summary>
    ImprimePagina,
    /// <summary>Define página de código.</summary>
    PaginaDeCodigo,
    /// <summary>Aciona gaveta.</summary>
    Gaveta,
    /// <summary>Inicia barcode.</summary>
    IniciarBarcode,
    /// <summary>Largura do barcode.</summary>
    BarcodeWidth,
    /// <summary>Altura do barcode.</summary>
    BarcodeHeight,
    /// <summary>Barcode sem texto.</summary>
    BarcodeNoText,
    /// <summary>Texto acima do barcode.</summary>
    BarcodeTextAbove,
    /// <summary>Texto abaixo do barcode.</summary>
    BarcodeTextBelow,
    /// <summary>Texto acima e abaixo do barcode.</summary>
    BarcodeTextBoth,
    /// <summary>Barcode UPCA.</summary>
    BarcodeUPCA,
    /// <summary>Barcode UPCE.</summary>
    BarcodeUPCE,
    /// <summary>Barcode EAN13.</summary>
    BarcodeEAN13,
    /// <summary>Barcode EAN8.</summary>
    BarcodeEAN8,
    /// <summary>Barcode CODE39.</summary>
    BarcodeCODE39,
    /// <summary>Barcode Interleaved 2 of 5.</summary>
    BarcodeInter2of5,
    /// <summary>Barcode CodaBar.</summary>
    BarcodeCodaBar,
    /// <summary>Barcode CODE93.</summary>
    BarcodeCODE93,
    /// <summary>Barcode CODE128.</summary>
    BarcodeCODE128,
    /// <summary>Logo novo.</summary>
    LogoNew,
    /// <summary>Logo antigo.</summary>
    LogoOld,
    /// <summary>Inicializa QR Code.</summary>
    QrCodeInitial,
    /// <summary>Modelo do QR Code.</summary>
    QrCodeModel,
    /// <summary>Tamanho do QR Code.</summary>
    QrCodeSize,
    /// <summary>Erro do QR Code.</summary>
    QrCodeError,
    /// <summary>Armazena QR Code.</summary>
    QrCodeStore,
    /// <summary>Imprime QR Code.</summary>
    QrCodePrint
}