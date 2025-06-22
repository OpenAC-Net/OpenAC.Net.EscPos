// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="InformacoesImpressora.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Commom;

/// <summary>
/// Representa as informações de uma impressora.
/// </summary>
public class InformacoesImpressora
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InformacoesImpressora"/>.
    /// </summary>
    /// <param name="fabricante">Nome do fabricante da impressora.</param>
    /// <param name="modelo">Modelo da impressora.</param>
    /// <param name="firmware">Versão do firmware da impressora.</param>
    /// <param name="serial">Número de série da impressora.</param>
    /// <param name="guilhotina">Indica se a impressora possui guilhotina.</param>
    public InformacoesImpressora(string fabricante, string modelo, string firmware, string serial, bool guilhotina)
    {
        Fabricante = fabricante;
        Modelo = modelo;
        Firmware = firmware;
        Serial = serial;
        Guilhotina = guilhotina;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtém o nome do fabricante da impressora.
    /// </summary>
    public string Fabricante { get; }

    /// <summary>
    /// Obtém o modelo da impressora.
    /// </summary>
    public string Modelo { get; }

    /// <summary>
    /// Obtém a versão do firmware da impressora.
    /// </summary>
    public string Firmware { get; }

    /// <summary>
    /// Obtém o número de série da impressora.
    /// </summary>
    public string Serial { get; }

    /// <summary>
    /// Indica se a impressora possui guilhotina.
    /// </summary>
    public bool Guilhotina { get; }

    /// <summary>
    /// Obtém uma instância vazia de <see cref="InformacoesImpressora"/>.
    /// </summary>
    public static InformacoesImpressora Empty = new("", "", "", "", false);

    #endregion Properties

    #region Methods

    /// <summary>
    /// Retorna uma representação em string das propriedades da impressora.
    /// </summary>
    /// <returns>String contendo os nomes e valores das propriedades.</returns>
    public override string ToString()
    {
        var props = GetType().GetProperties();
        var sb = new StringBuilder();
        foreach (var p in props)
            sb.AppendLine(p.Name + ": " + p.GetValue(this, null));
        return sb.ToString();
    }

    #endregion Methods
}