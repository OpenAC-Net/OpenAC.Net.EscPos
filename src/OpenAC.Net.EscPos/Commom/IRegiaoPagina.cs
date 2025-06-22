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

/// <summary>
/// Interface que define a região de página para impressão, incluindo propriedades de layout e métodos para adicionar comandos de impressão ao buffer.
/// </summary>
public interface IRegiaoPagina
{
    #region Properties

    /// <summary>
    /// Largura da região de impressão.
    /// </summary>
    int Largura { get; set; }

    /// <summary>
    /// Altura da região de impressão.
    /// </summary>
    int Altura { get; set; }

    /// <summary>
    /// Posição esquerda da região de impressão.
    /// </summary>
    int Esquerda { get; set; }

    /// <summary>
    /// Posição superior da região de impressão.
    /// </summary>
    int Topo { get; set; }

    /// <summary>
    /// Direção de impressão da região.
    /// </summary>
    CmdPosDirecao Direcao { get; set; }

    /// <summary>
    /// Espaço entre linhas na região de impressão.
    /// </summary>
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
    /// <param name="aLinhas">Quantidade de linhas a pular.</param>
    void PularLinhas(int aLinhas);

    /// <summary>
    /// Adiciona o comando de imprimir linha ao buffer de impressão da região.
    /// </summary>
    /// <param name="dupla">Indica se a linha deve ser dupla.</param>
    void ImprimirLinha(bool dupla = false);

    /// <summary>
    /// Adiciona o comando de impressão de logo ao buffer de impressão da região.
    /// </summary>
    /// <param name="kc1">Primeiro identificador do logo.</param>
    /// <param name="kc2">Segundo identificador do logo.</param>
    /// <param name="fatorX">Fator de escala no eixo X.</param>
    /// <param name="fatorY">Fator de escala no eixo Y.</param>
    void ImprimirLogo(byte? kc1 = null, byte? kc2 = null, byte? fatorX = null, byte? fatorY = null);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto a ser impresso.</param>
    void ImprimirTexto(string aTexto);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto a ser impresso.</param>
    /// <param name="aAlinhamento">Alinhamento do texto.</param>
    void ImprimirTexto(string aTexto, CmdAlinhamento aAlinhamento);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto a ser impresso.</param>
    /// <param name="tamanho">Tamanho da fonte.</param>
    /// <param name="aAlinhamento">Alinhamento do texto.</param>
    void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto a ser impresso.</param>
    /// <param name="tamanho">Tamanho da fonte.</param>
    void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto a ser impresso.</param>
    /// <param name="tamanho">Tamanho da fonte.</param>
    /// <param name="aEstilo">Estilo da fonte.</param>
    void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdEstiloFonte aEstilo);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto a ser impresso.</param>
    /// <param name="aEstilo">Estilo da fonte.</param>
    void ImprimirTexto(string aTexto, CmdEstiloFonte aEstilo);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto a ser impresso.</param>
    /// <param name="aAlinhamento">Alinhamento do texto.</param>
    /// <param name="aEstilo">Estilo da fonte.</param>
    void ImprimirTexto(string aTexto, CmdAlinhamento aAlinhamento, CmdEstiloFonte aEstilo);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto a ser impresso.</param>
    /// <param name="tamanho">Tamanho da fonte.</param>
    /// <param name="aAlinhamento">Alinhamento do texto.</param>
    /// <param name="aEstilo">Estilo da fonte.</param>
    void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, CmdEstiloFonte aEstilo);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto a ser impresso.</param>
    /// <param name="fonte">Fonte do texto.</param>
    /// <param name="tamanho">Tamanho da fonte.</param>
    /// <param name="aAlinhamento">Alinhamento do texto.</param>
    /// <param name="aEstilo">Estilo da fonte (opcional).</param>
    void ImprimirTexto(string aTexto, CmdFonte fonte, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento,
        CmdEstiloFonte? aEstilo);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer.
    /// </summary>
    /// <param name="slices">Fragmentos de texto formatados.</param>
    void ImprimirTexto(params TextSlice[] slices);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer.
    /// </summary>
    /// <param name="aAlinhamento">Alinhamento do texto.</param>
    /// <param name="slices">Fragmentos de texto formatados.</param>
    void ImprimirTexto(CmdAlinhamento aAlinhamento, params TextSlice[] slices);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer.
    /// </summary>
    /// <param name="tamanho">Tamanho da fonte.</param>
    /// <param name="aAlinhamento">Alinhamento do texto.</param>
    /// <param name="slices">Fragmentos de texto formatados.</param>
    void ImprimirTexto(CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, params TextSlice[] slices);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer.
    /// </summary>
    /// <param name="fonte">Fonte do texto.</param>
    /// <param name="aAlinhamento">Alinhamento do texto.</param>
    /// <param name="slices">Fragmentos de texto formatados.</param>
    void ImprimirTexto(CmdFonte fonte, CmdAlinhamento aAlinhamento, params TextSlice[] slices);

    /// <summary>
    /// Adiciona o comando de impressão de texto ao buffer.
    /// </summary>
    /// <param name="fonte">Fonte do texto.</param>
    /// <param name="tamanho">Tamanho da fonte.</param>
    /// <param name="aAlinhamento">Alinhamento do texto.</param>
    /// <param name="slices">Fragmentos de texto formatados.</param>
    void ImprimirTexto(CmdFonte fonte, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, params TextSlice[] slices);

    /// <summary>
    /// Adiciona o comando de impressão de código de barras ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto do código de barras.</param>
    /// <param name="barcode">Tipo de código de barras.</param>
    void ImprimirBarcode(string aTexto, CmdBarcode barcode);

    /// <summary>
    /// Adiciona o comando de impressão de código de barras ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto do código de barras.</param>
    /// <param name="barcode">Tipo de código de barras.</param>
    /// <param name="exibir">Exibição do texto no código de barras.</param>
    void ImprimirBarcode(string aTexto, CmdBarcode barcode, CmdBarcodeText exibir);

    /// <summary>
    /// Adiciona o comando de impressão de código de barras ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto">Texto do código de barras.</param>
    /// <param name="barcode">Tipo de código de barras.</param>
    /// <param name="aAlinhamento">Alinhamento do código de barras.</param>
    /// <param name="exibir">Exibição do texto no código de barras (opcional).</param>
    /// <param name="altura">Altura do código de barras (opcional).</param>
    /// <param name="largura">Largura do código de barras (opcional).</param>
    void ImprimirBarcode(string aTexto, CmdBarcode barcode, CmdAlinhamento aAlinhamento, CmdBarcodeText? exibir = null,
        int? altura = null, int? largura = null);

    /// <summary>
    /// Adiciona o comando de impressão de QrCode ao buffer de impressão da região.
    /// </summary>
    /// <param name="texto">Texto do QrCode.</param>
    /// <param name="aAlinhamento">Alinhamento do QrCode.</param>
    void ImprimirQrCode(string texto, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda);

    /// <summary>
    /// Adiciona o comando de impressão de QrCode ao buffer de impressão da região.
    /// </summary>
    /// <param name="texto">Texto do QrCode.</param>
    /// <param name="tipo">Tipo do QrCode (opcional).</param>
    /// <param name="tamanho">Tamanho do QrCode (opcional).</param>
    /// <param name="erroLevel">Nível de correção de erro (opcional).</param>
    /// <param name="aAlinhamento">Alinhamento do QrCode.</param>
    void ImprimirQrCode(string texto, QrCodeTipo? tipo = null, QrCodeModSize? tamanho = null,
        QrCodeErrorLevel? erroLevel = null, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda);

    /// <summary>
    /// Adiciona o comando para imprimir imagem ao buffer.
    /// </summary>
    /// <param name="imagem">Imagem a ser impressa.</param>
    /// <param name="aAlinhamento">Alinhamento da imagem.</param>
    /// <param name="isHdpi">Indica se a imagem é de alta densidade.</param>
    void ImprimirImagem(Image imagem, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda, bool isHdpi = false);

    #endregion Methods
}