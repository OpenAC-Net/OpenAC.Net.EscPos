// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="IModoPagina.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Commom;

/// <summary>
/// Interface que define o modo página para impressão, incluindo regiões, espaçamento entre linhas e configurações de códigos de barras e QR Code.
/// </summary>
public interface IModoPagina
{
    #region Properties

    /// <summary>
    /// Obtém a lista de regiões a serem impressas dentro do modo página.
    /// </summary>
    IReadOnlyList<IRegiaoPagina> Regioes { get; }

    /// <summary>
    /// Define ou obtém o espaço entre as linhas da impressão.
    /// </summary>
    byte EspacoEntreLinhas { get; set; }

    /// <summary>
    /// Obtém as configurações padrão do código de barras.
    /// </summary>
    BarcodeConfig CodigoBarras { get; }

    /// <summary>
    /// Obtém as configurações padrão para impressão do QR Code.
    /// </summary>
    QrCodeConfig QrCode { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Cria uma nova região na página com as dimensões especificadas.
    /// </summary>
    /// <param name="esqueda">Posição da margem esquerda da região.</param>
    /// <param name="topo">Posição do topo da região.</param>
    /// <param name="largura">Largura da região.</param>
    /// <param name="altura">Altura da região.</param>
    /// <returns>Instância de <see cref="IRegiaoPagina"/> criada.</returns>
    IRegiaoPagina NovaRegiao(int esqueda, int topo, int largura, int altura);

    /// <summary>
    /// Remove uma região existente da página.
    /// </summary>
    /// <param name="regiao">Região a ser removida.</param>
    void RemoverRegiao(IRegiaoPagina regiao);

    #endregion Methods
}