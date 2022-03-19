// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="EpsonEscPosEscPosInterpreter.cs" company="OpenAC .Net">
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
using System.Linq;
using System.Text;
using OpenAC.Net.EscPos.Command;

namespace OpenAC.Net.EscPos.Interpreter
{
    public abstract class EscPosInterpreter
    {
        #region Constructors

        protected EscPosInterpreter() => InicializarComandos();

        #endregion Constructors

        #region Properties

        protected Dictionary<EscPosCommand, byte[]> Commandos { get; } = new Dictionary<EscPosCommand, byte[]>();

        #endregion Properties

        #region Methods

        /// <summary>
        /// Processa o comando e retornar os bytes correspondente.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public byte[] ProcessCommand(PrintCommand command)
        {
            var list = new List<byte>();

            switch (command)
            {
                case TextCommand cmd: list.AddRange(ProcessCommand(cmd)); break;
                case ZeraCommand cmd: list.AddRange(ProcessCommand(cmd)); break;
                case EspacoEntreLinhasCommand cmd: list.AddRange(ProcessCommand(cmd)); break;
            }

            list.AddRange(Commandos[EscPosCommand.PuloDeLinha]);
            return list.ToArray();
        }

        /// <summary>
        /// Processa o comando de texto.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        protected virtual byte[] ProcessCommand(TextCommand cmd)
        {
            var list = new List<byte>();

            switch (cmd.Fonte)
            {
                case CmdFonte.Normal when Commandos.ContainsKey(EscPosCommand.FonteNormal):
                    list.AddRange(Commandos[EscPosCommand.FonteNormal]);
                    break;

                case CmdFonte.FonteA when Commandos.ContainsKey(EscPosCommand.FonteA):
                    list.AddRange(Commandos[EscPosCommand.FonteA]);
                    break;

                case CmdFonte.FonteB when Commandos.ContainsKey(EscPosCommand.FonteB):
                    list.AddRange(Commandos[EscPosCommand.FonteB]);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (cmd.Alinhamento)
            {
                case CmdAlinhamento.Esquerda when Commandos.ContainsKey(EscPosCommand.AlinhadoEsquerda):
                    list.AddRange(Commandos[EscPosCommand.AlinhadoEsquerda]);
                    break;

                case CmdAlinhamento.Centro when Commandos.ContainsKey(EscPosCommand.AlinhadoCentro):
                    list.AddRange(Commandos[EscPosCommand.AlinhadoCentro]);
                    break;

                case CmdAlinhamento.Direita when Commandos.ContainsKey(EscPosCommand.AlinhadoDireita):
                    list.AddRange(Commandos[EscPosCommand.AlinhadoDireita]);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (cmd.Tamanho)
            {
                case CmdTamanhoFonte.Expandida when Commandos.ContainsKey(EscPosCommand.LigaExpandido):
                    list.AddRange(Commandos[EscPosCommand.LigaExpandido]);
                    break;

                case CmdTamanhoFonte.Condensada when Commandos.ContainsKey(EscPosCommand.LigaCondensado):
                    list.AddRange(Commandos[EscPosCommand.LigaCondensado]);
                    break;
            }

            if (cmd.Estilo.HasValue)
            {
                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Negrito) && Commandos.ContainsKey(EscPosCommand.LigaNegrito))
                    list.AddRange(Commandos[EscPosCommand.LigaNegrito]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Sublinhado) && Commandos.ContainsKey(EscPosCommand.LigaSublinhado))
                    list.AddRange(Commandos[EscPosCommand.LigaSublinhado]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Invertido) && Commandos.ContainsKey(EscPosCommand.LigaInvertido))
                    list.AddRange(Commandos[EscPosCommand.LigaSublinhado]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Italico) && Commandos.ContainsKey(EscPosCommand.LigaItalico))
                    list.AddRange(Commandos[EscPosCommand.LigaItalico]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.AlturaDupla) && Commandos.ContainsKey(EscPosCommand.LigaAlturaDupla))
                    list.AddRange(Commandos[EscPosCommand.LigaAlturaDupla]);
            }

            // Adiciona o texto, fazendo o tratamento para quebra de linha
            // ToDo: Checar se precisar fazer o enconding aqui.
            list.AddRange(Encoding.UTF8.GetBytes(TratarString(cmd.Texto)));

            if (cmd.Estilo.HasValue)
            {
                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Negrito) && Commandos.ContainsKey(EscPosCommand.DesligaNegrito))
                    list.AddRange(Commandos[EscPosCommand.DesligaNegrito]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Sublinhado) && Commandos.ContainsKey(EscPosCommand.DesligaSublinhado))
                    list.AddRange(Commandos[EscPosCommand.DesligaSublinhado]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Invertido) && Commandos.ContainsKey(EscPosCommand.DesligaInvertido))
                    list.AddRange(Commandos[EscPosCommand.LigaSublinhado]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Italico) && Commandos.ContainsKey(EscPosCommand.DesligaItalico))
                    list.AddRange(Commandos[EscPosCommand.DesligaItalico]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.AlturaDupla) && Commandos.ContainsKey(EscPosCommand.DesligaAlturaDupla))
                    list.AddRange(Commandos[EscPosCommand.DesligaAlturaDupla]);
            }

            switch (cmd.Tamanho)
            {
                case CmdTamanhoFonte.Expandida when Commandos.ContainsKey(EscPosCommand.DesligaExpandido):
                    list.AddRange(Commandos[EscPosCommand.DesligaExpandido]);
                    break;

                case CmdTamanhoFonte.Condensada when Commandos.ContainsKey(EscPosCommand.DesligaCondensado):
                    list.AddRange(Commandos[EscPosCommand.DesligaCondensado]);
                    break;
            }

            // Volta a Fonte para Normal.
            if (Commandos.ContainsKey(EscPosCommand.FonteNormal))
                list.AddRange(Commandos[EscPosCommand.FonteNormal]);

            // Volta alinhamento para Esquerda.
            if (Commandos.ContainsKey(EscPosCommand.AlinhadoEsquerda))
                list.AddRange(Commandos[EscPosCommand.AlinhadoEsquerda]);

            return list.ToArray();
        }

        /// <summary>
        /// Processa o comando Zera.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected virtual byte[] ProcessCommand(ZeraCommand cmd) => Commandos.ContainsKey(EscPosCommand.Zera) ? Commandos[EscPosCommand.Zera] : new byte[0];

        /// <summary>
        /// Processa o comando de Espaco Entre Linhas.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected virtual byte[] ProcessCommand(EspacoEntreLinhasCommand cmd)
        {
            if (!Commandos.ContainsKey(EscPosCommand.EspacoEntreLinhasPadrao)) return new byte[0];

            var espacos = Math.Max((byte)0, cmd.Espaco);
            if (espacos == 0) return Commandos[EscPosCommand.EspacoEntreLinhasPadrao];

            if (!Commandos.ContainsKey(EscPosCommand.EspacoEntreLinhas)) return new byte[0];

            return Commandos[EscPosCommand.EspacoEntreLinhas].Concat(new[] { espacos }).ToArray();
        }

        /// <summary>
        /// Função para tratar a quebra de linha da string para o formato que a impressora aceita.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        protected virtual string TratarString(string texto) => texto.Replace("\r\n", Encoding.UTF8.GetString(Commandos[EscPosCommand.PuloDeLinha]));

        /// <summary>
        /// Função para inicializar o dicionario de comandos para ser usados no interpreter.
        /// </summary>
        protected abstract void InicializarComandos();

        #endregion Methods
    }
}