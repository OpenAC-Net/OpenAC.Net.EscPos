// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="IRegiaoPagina.cs" company="OpenAC .Net">
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

using System.Drawing;
using OpenAC.Net.EscPos.Command;

namespace OpenAC.Net.EscPos.Commom;

public interface IRegiaoPagina
{
    #region Properties

    int Largura { get; set; }

    int Altura { get; set; }

    int Esquerda { get; set; }

    int Topo { get; set; }

    CmdPosDirecao Direcao { get; set; }

    byte EspacoEntreLinhas { get; set; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Limpa o buffer de impressão da região.
    /// </summary>
    void Clear();

    /// <summary>
    /// Adiciona o comando de pular linhas ao buffer de impressão da região.
    /// </summary>
    /// <param name="aLinhas"></param>
    void PularLinhas(int aLinhas);

    /// <summary>
    /// Adiciona o comando de imprimir linha ao buffer de impressão da região.
    /// </summary>
    /// <param name="tamanho"></param>
    /// <param name="dupla"></param>
    void ImprimirLinha(bool dupla = false);

    /// <summary>
    /// Adiciona o comando de impressão de logo ao buffer de impressão da região.
    /// </summary>
    /// <param name="kc1"></param>
    /// <param name="kc2"></param>
    /// <param name="fatorX"></param>
    /// <param name="fatorY"></param>
    void ImprimirLogo(byte? kc1 = null, byte? kc2 = null, byte? fatorX = null, byte? fatorY = null);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    void ImprimirTexto(string aTexto);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="aAlinhamento"></param>
    void ImprimirTexto(string aTexto, CmdAlinhamento aAlinhamento);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="tamanho"></param>
    /// <param name="aAlinhamento"></param>
    void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="tamanho"></param>
    void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="tamanho"></param>
    /// <param name="aEstilo"></param>
    void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdEstiloFonte aEstilo);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="aEstilo"></param>
    void ImprimirTexto(string aTexto, CmdEstiloFonte aEstilo);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="aAlinhamento"></param>
    /// <param name="aEstilo"></param>
    void ImprimirTexto(string aTexto, CmdAlinhamento aAlinhamento, CmdEstiloFonte aEstilo);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="tamanho"></param>
    /// <param name="aAlinhamento"></param>
    /// <param name="aEstilo"></param>
    void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, CmdEstiloFonte aEstilo);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="fonte"></param>
    /// <param name="tamanho"></param>
    /// <param name="aAlinhamento"></param>
    /// <param name="aEstilo"></param>
    void ImprimirTexto(string aTexto, CmdFonte fonte, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, CmdEstiloFonte? aEstilo);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer.
    /// </summary>
    /// <param name="slices"></param>
    void ImprimirTexto(params TextSlice[] slices);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer.
    /// </summary>
    /// <param name="aAlinhamento"></param>
    /// <param name="slices"></param>
    void ImprimirTexto(CmdAlinhamento aAlinhamento, params TextSlice[] slices);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer.
    /// </summary>
    /// <param name="fonte"></param>
    /// <param name="tamanho"></param>
    /// <param name="aAlinhamento"></param>
    /// <param name="slices"></param>
    void ImprimirTexto(CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, params TextSlice[] slices);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer.
    /// </summary>
    /// <param name="fonte"></param>
    /// <param name="aAlinhamento"></param>
    /// <param name="slices"></param>
    void ImprimirTexto(CmdFonte fonte, CmdAlinhamento aAlinhamento, params TextSlice[] slices);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer.
    /// </summary>
    /// <param name="fonte"></param>
    /// <param name="tamanho"></param>
    /// <param name="aAlinhamento"></param>
    /// <param name="slices"></param>
    void ImprimirTexto(CmdFonte fonte, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, params TextSlice[] slices);

    /// <summary>
    /// Adiciona o comando de impressão de codigo de barras ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="barcode"></param>
    void ImprimirBarcode(string aTexto, CmdBarcode barcode);

    /// <summary>
    /// Adiciona o comando de impressão de codigo de barras ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="barcode"></param>
    /// <param name="exibir"></param>
    void ImprimirBarcode(string aTexto, CmdBarcode barcode, CmdBarcodeText exibir);

    /// <summary>
    /// Adiciona o comando de impressão de codigo de barras ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="barcode"></param>
    /// <param name="aAlinhamento"></param>
    /// <param name="exibir"></param>
    /// <param name="altura"></param>
    /// <param name="largura"></param>
    void ImprimirBarcode(string aTexto, CmdBarcode barcode, CmdAlinhamento aAlinhamento, CmdBarcodeText? exibir = null, int? altura = null, int? largura = null);

    /// <summary>
    /// Adiciona o comando de impressão de QrCode ao buffer de impressão da região.
    /// </summary>
    /// <param name="texto"></param>
    /// <param name="aAlinhamento"></param>
    void ImprimirQrCode(string texto, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda);

    /// <summary>
    /// Adiciona o comando de impressão de QrCode ao buffer de impressão da região.
    /// </summary>
    /// <param name="texto"></param>
    /// <param name="tipo"></param>
    /// <param name="tamanho"></param>
    /// <param name="erroLevel"></param>
    /// <param name="aAlinhamento"></param>
    void ImprimirQrCode(string texto, QrCodeTipo? tipo = null, QrCodeModSize? tamanho = null, QrCodeErrorLevel? erroLevel = null, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda);

    /// <summary>
    /// Adicionado o comando para imprimir imagem ao buffer.
    /// </summary>
    /// <param name="imagem"></param>
    /// <param name="aAlinhamento"></param>
    /// <param name="isHdpi"></param>
    void ImprimirImagem(Image imagem, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda, bool isHdpi = false);

    #endregion Methods
}