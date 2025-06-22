// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="InfoResolver.cs" company="OpenAC .Net">
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

namespace OpenAC.Net.EscPos.Interpreter.Resolver;

/// <summary>
/// Classe base abstrata para resolver informações a partir de comandos e dados em bytes.
/// </summary>
/// <typeparam name="TInfo">Tipo da informação resolvida.</typeparam>
public abstract class InfoResolver<TInfo>
{
    #region Fields

    /// <summary>
    /// Função responsável por processar os dados e retornar a informação.
    /// </summary>
    private readonly Func<byte[][], TInfo> processStatus;

    #endregion Fields

    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="InfoResolver{TInfo}"/>.
    /// </summary>
    /// <param name="commands">Comandos utilizados para consulta.</param>
    /// <param name="resolver">Função que processa os dados recebidos.</param>
    protected InfoResolver(byte[][] commands, Func<byte[][], TInfo> resolver)
    {
        Commands = commands;
        processStatus = resolver;
    }

    #endregion Constructors

    #region Methods

    /// <summary>
    /// Obtém os comandos utilizados para consulta.
    /// </summary>
    public byte[][] Commands { get; }

    /// <summary>
    /// Processa os dados recebidos e retorna a informação resolvida.
    /// </summary>
    /// <param name="dados">Dados recebidos para processamento.</param>
    /// <returns>Informação resolvida do tipo <typeparamref name="TInfo"/>.</returns>
    public TInfo Resolve(byte[][] dados) => processStatus(dados);

    #endregion Methods
}