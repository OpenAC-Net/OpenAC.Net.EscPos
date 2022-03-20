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

using System.Collections.Generic;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter;

namespace OpenAC.Net.EscPos.Command
{
    /// <summary>
    /// WIP - Work In Progress
    /// </summary>
    public sealed class ModoPaginaCommand : PrintCommand
    {
        #region Constructors

        public ModoPaginaCommand(EscPosInterpreter interpreter) : base(interpreter)
        {
            Commands = new List<PrintCommand>();
        }

        #endregion Constructors

        #region Properties

        protected List<PrintCommand> Commands { get; }

        #endregion Properties

        #region Methods

        protected override byte[] GetContet()
        {
            var comandos = new List<byte>();

            foreach (var command in Commands)
                comandos.AddRange(command.Content);

            return comandos.ToArray();
        }

        public void Clear() => Commands.Clear();

        public void ImprimirTexto(string aTexto)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, null);
        }

        public void ImprimirTexto(string aTexto, CmdAlinhamento aAlinhamento)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, aAlinhamento, null);
        }

        public void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, tamanho, aAlinhamento, null);
        }

        public void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, tamanho, CmdAlinhamento.Esquerda, null);
        }

        public void ImprimirTexto(string aTexto, CmdEstiloFonte aEstilo)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, aEstilo);
        }

        public void ImprimirTexto(string aTexto, CmdAlinhamento aAlinhamento, CmdEstiloFonte aEstilo)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, aAlinhamento, aEstilo);
        }

        public void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, CmdEstiloFonte aEstilo)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, tamanho, aAlinhamento, aEstilo);
        }

        public void ImprimirTexto(string aTexto, CmdFonte fonte, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, CmdEstiloFonte? aEstilo)
        {
            var cmd = new TextCommand(Interpreter)
            {
                Texto = aTexto,
                Fonte = fonte,
                Tamanho = tamanho,
                Alinhamento = aAlinhamento,
                Estilo = aEstilo
            };

            Commands.Add(cmd);
        }

        #endregion Methods
    }
}