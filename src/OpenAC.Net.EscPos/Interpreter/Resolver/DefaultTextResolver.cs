// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="CommandResolver.cs" company="OpenAC .Net">
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
using System.Text;
using OpenAC.Net.Devices.Commom;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Interpreter.Resolver;

public sealed class DefaultTextResolver : CommandResolver<TextCommand>
{
    #region Constructors

    public DefaultTextResolver(Encoding enconder, IReadOnlyDictionary<CmdEscPos, byte[]> dict) : base(dict)
    {
        Enconder = enconder;
    }

    #endregion Constructors

    #region Properties

    public Encoding Enconder { get; }

    #endregion Properties

    #region Methods

    public override byte[] Resolve(TextCommand cmd)
    {
        using var builder = new ByteArrayBuilder();

        switch (cmd.Fonte)
        {
            case CmdFonte.Normal when Commandos.ContainsKey(CmdEscPos.FonteNormal):
                builder.Append(Commandos[CmdEscPos.FonteNormal]);
                break;

            case CmdFonte.FonteA when Commandos.ContainsKey(CmdEscPos.FonteA):
                builder.Append(Commandos[CmdEscPos.FonteA]);
                break;

            case CmdFonte.FonteB when Commandos.ContainsKey(CmdEscPos.FonteB):
                builder.Append(Commandos[CmdEscPos.FonteB]);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (cmd.Alinhamento)
        {
            case CmdAlinhamento.Esquerda when Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda):
                builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);
                break;

            case CmdAlinhamento.Centro when Commandos.ContainsKey(CmdEscPos.AlinhadoCentro):
                builder.Append(Commandos[CmdEscPos.AlinhadoCentro]);
                break;

            case CmdAlinhamento.Direita when Commandos.ContainsKey(CmdEscPos.AlinhadoDireita):
                builder.Append(Commandos[CmdEscPos.AlinhadoDireita]);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (cmd.Tamanho)
        {
            case CmdTamanhoFonte.Expandida when Commandos.ContainsKey(CmdEscPos.LigaExpandido):
                builder.Append(Commandos[CmdEscPos.LigaExpandido]);
                break;

            case CmdTamanhoFonte.Condensada when Commandos.ContainsKey(CmdEscPos.LigaCondensado):
                builder.Append(Commandos[CmdEscPos.LigaCondensado]);
                break;
        }

        if (cmd.Estilo.HasValue)
        {
            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Negrito) && Commandos.ContainsKey(CmdEscPos.LigaNegrito))
                builder.Append(Commandos[CmdEscPos.LigaNegrito]);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Sublinhado) && Commandos.ContainsKey(CmdEscPos.LigaSublinhado))
                builder.Append(Commandos[CmdEscPos.LigaSublinhado]);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Italico) && Commandos.ContainsKey(CmdEscPos.LigaItalico))
                builder.Append(Commandos[CmdEscPos.LigaItalico]);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.AlturaDupla) && Commandos.ContainsKey(CmdEscPos.LigaAlturaDupla))
                builder.Append(Commandos[CmdEscPos.LigaAlturaDupla]);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Invertido) && Commandos.ContainsKey(CmdEscPos.LigaInvertido))
                builder.Append(Commandos[CmdEscPos.LigaInvertido]);
        }

        // Adiciona o texto, fazendo o tratamento para quebra de linha
        builder.Append(Enconder.GetBytes(TratarString(cmd.Texto)));

        if (cmd.Estilo.HasValue)
        {
            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Negrito) && Commandos.ContainsKey(CmdEscPos.DesligaNegrito))
                builder.Append(Commandos[CmdEscPos.DesligaNegrito]);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Sublinhado) && Commandos.ContainsKey(CmdEscPos.DesligaSublinhado))
                builder.Append(Commandos[CmdEscPos.DesligaSublinhado]);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Italico) && Commandos.ContainsKey(CmdEscPos.DesligaItalico))
                builder.Append(Commandos[CmdEscPos.DesligaItalico]);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.AlturaDupla) && Commandos.ContainsKey(CmdEscPos.DesligaAlturaDupla))
                builder.Append(Commandos[CmdEscPos.DesligaAlturaDupla]);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Invertido) && Commandos.ContainsKey(CmdEscPos.DesligaInvertido))
                builder.Append(Commandos[CmdEscPos.DesligaInvertido]);
        }

        switch (cmd.Tamanho)
        {
            case CmdTamanhoFonte.Expandida when Commandos.ContainsKey(CmdEscPos.DesligaExpandido):
                builder.Append(Commandos[CmdEscPos.DesligaExpandido]);
                break;

            case CmdTamanhoFonte.Condensada when Commandos.ContainsKey(CmdEscPos.DesligaCondensado):
                builder.Append(Commandos[CmdEscPos.DesligaCondensado]);
                break;
        }

        // Volta a Fonte para Normal.
        if (Commandos.ContainsKey(CmdEscPos.FonteNormal) && cmd.Fonte != CmdFonte.Normal)
            builder.Append(Commandos[CmdEscPos.FonteNormal]);

        // Volta alinhamento para Esquerda.
        if (Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda) && cmd.Alinhamento != CmdAlinhamento.Esquerda)
            builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);

        // Texto sempre tem que terminar com o pulo de linha
        builder.Append(Commandos[CmdEscPos.PuloDeLinha]);
        return builder.ToArray();
    }

    /// <summary>
    /// Função para tratar a quebra de linha da string para o formato que a impressora aceita.
    /// </summary>
    /// <param name="texto"></param>
    /// <returns></returns>
    private static string TratarString(string texto) => texto.Replace("\r\n", "\n").Replace("\r", "\n");

    #endregion Methods
}