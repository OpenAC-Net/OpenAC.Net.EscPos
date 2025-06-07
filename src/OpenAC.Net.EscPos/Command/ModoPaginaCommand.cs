// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="ModoPaginaCommand.cs" company="OpenAC .Net">
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
using System.Collections.Generic;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter;

namespace OpenAC.Net.EscPos.Command;

public sealed class ModoPaginaCommand : PrintCommand<ModoPaginaCommand>, IModoPagina
{
    #region Fields

    private readonly List<ModoPaginaRegiao> regioes;

    #endregion Fields

    #region Constructors

    public ModoPaginaCommand(EscPosInterpreter interpreter) : base(interpreter)
    {
        regioes = [];
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Comandos para serem impressos dentro do modo pagina.
    /// </summary>
    public IReadOnlyList<ModoPaginaRegiao> Regioes => regioes;

    IReadOnlyList<IRegiaoPagina> IModoPagina.Regioes => regioes;

    /// <summary>
    /// Define/Obtém o espaço entre as linhas da impressão.
    /// </summary>
    public byte EspacoEntreLinhas { get; set; } = 0;

    /// <summary>
    /// Define/Obtém a quantidade de coluna quando a fonte é normal.
    /// </summary>
    public int Colunas { get; set; } = 48;

    /// <summary>
    /// Retorna a quantidade de colunas quando a fonte é expandida.
    /// </summary>
    public int ColunasExpandida
    {
        get
        {
            if (Interpreter == null) return 0;
            return (int)Math.Truncate(Colunas / Interpreter.RazaoColuna.Expandida);
        }
    }

    /// <summary>
    /// Retorna a quantidade de colunas quando a fonte é concensada.
    /// </summary>
    public int ColunasCondensada
    {
        get
        {
            if (Interpreter == null) return 0;
            return (int)Math.Truncate(Colunas / Interpreter.RazaoColuna.Condensada);
        }
    }

    /// <summary>
    /// Define/Obtém as configurações padrão do codigo de barras.
    /// </summary>
    public BarcodeConfig CodigoBarras { get; set; } = new();

    /// <summary>
    /// Define/Obtém as configurações padrão para impressão do logo.
    /// </summary>
    public LogoConfig Logo { get; set; } = new();

    /// <summary>
    /// Define/Obtém as configurações padrão para impressão do QrCode.
    /// </summary>
    public QrCodeConfig QrCode { get; set; } = new();

    #endregion Properties

    #region Methods

    public IRegiaoPagina NovaRegiao(int esqueda, int topo, int largura, int altura)
    {
        var regiao = new ModoPaginaRegiao(this, Interpreter)
        {
            Largura = largura,
            Altura = altura,
            Esquerda = esqueda,
            Topo = topo
        };

        regioes.Add(regiao);
        return regiao;
    }

    public void RemoverRegiao(IRegiaoPagina regiao) => regioes.Remove(regiao as ModoPaginaRegiao);

    #endregion Methods
}