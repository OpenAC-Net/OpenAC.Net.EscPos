// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="EscPosPrinter.cs" company="OpenAC .Net">
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
using OpenAC.Net.Core;
using OpenAC.Net.Core.Logging;
using OpenAC.Net.Devices;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Interpreter;

namespace OpenAC.Net.EscPos
{
    public sealed class EscPosPrinter<TConfig> : IDisposable, IOpenLog where TConfig : IDeviceConfig
    {
        #region Fields

        private readonly List<PrintCommand> commands;
        private OpenDeviceStream device;
        private EscPosInterpreter interpreter;
        private ProtocoloEscPos protocolo;
        private bool inicializada;

        #endregion Fields

        #region Constructors

        internal EscPosPrinter(ProtocoloEscPos protocolo, TConfig device)
        {
            Protocolo = protocolo;
            Config = device;
            commands = new List<PrintCommand>();
        }

        ~EscPosPrinter() => Dispose();

        #endregion Constructors

        #region properties

        public TConfig Config { get; }

        public ProtocoloEscPos Protocolo
        {
            get => protocolo;
            set
            {
                Guard.Against<OpenException>(Conectado, "Não pode mudar o protocolo quando esta conectado.");
                protocolo = value;
            }
        }

        public byte EspacoEntreLinhas { get; set; } = 0;

        public bool Conectado => device?.Conectado ?? false;

        #endregion properties

        #region Methods

        public void Conectar()
        {
            Guard.Against<OpenException>(Conectado, "A porta já está aberta");
            Guard.Against<NotImplementedException>(!Enum.IsDefined(typeof(ProtocoloEscPos), Protocolo), "Protocolo não implementado");

            device = OpenDeviceFactory.Create(Config);
            device.Open();

            interpreter = EscPosInterpreterFactory.Create(Protocolo);

            Inicializar();
        }

        public void Desconectar()
        {
            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            device?.Close();
            device?.Dispose();
            device = null;
            interpreter = null;
        }

        private void Inicializar()
        {
            this.Log().Debug("Inicializar");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new EspacoEntreLinhasCommand(interpreter) { Espaco = EspacoEntreLinhas };
            var dados = cmd.Content;

            device.Write(dados);
        }

        /// <summary>
        /// Limpa o buffer de impressão.
        /// </summary>
        public void Clear()
        {
            this.Log().Debug("Clear");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");
            commands.Clear();
        }

        /// <summary>
        /// Envia o comando de zerar para impressora.
        /// </summary>
        public void Zerar()
        {
            this.Log().Debug("Zerar");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new ZeraCommand(interpreter);
            var dados = cmd.Content;

            device.Write(dados);
            Inicializar();
        }

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
            var cmd = new TextCommand(interpreter)
            {
                Texto = aTexto,
                Fonte = fonte,
                Tamanho = tamanho,
                Alinhamento = aAlinhamento,
                Estilo = aEstilo
            };

            commands.Add(cmd);
        }

        public void Imprimir(int copias = 1)
        {
            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");
            copias = Math.Max(1, copias);

            var comandos = new List<byte>();

            foreach (var command in commands)
                comandos.AddRange(command.Content);

            var dados = comandos.ToArray();

            for (var i = 0; i < copias; i++)
                device.Write(dados);
        }

        public void Dispose()
        {
            if (Conectado)
                Desconectar();
        }

        #endregion Methods
    }
}