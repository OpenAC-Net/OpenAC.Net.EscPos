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
using System.Linq;
using System.Reflection;
using System.Threading;
using OpenAC.Net.Core;
using OpenAC.Net.Core.Logging;
using OpenAC.Net.Devices;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter;

namespace OpenAC.Net.EscPos
{
    /// <summary>
    /// Classe para impressão EscPos.
    /// </summary>
    /// <typeparam name="TConfig"></typeparam>
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
            Device = device;
            commands = new List<PrintCommand>();
        }

        ~EscPosPrinter() => Dispose();

        #endregion Constructors

        #region properties

        /// <summary>
        /// Configuações de comunicação com a impressora.
        /// </summary>
        public TConfig Device { get; }

        /// <summary>
        /// Define/Obtém o protocolo de comunicação.
        /// </summary>
        public ProtocoloEscPos Protocolo
        {
            get => protocolo;
            set
            {
                Guard.Against<OpenException>(Conectado, "Não pode mudar o protocolo quando esta conectado.");
                protocolo = value;
            }
        }

        /// <summary>
        /// Define/Obtém o espaço entre as linhas da impressão.
        /// </summary>
        public byte EspacoEntreLinhas { get; set; } = 0;

        /// <summary>
        /// Define/Obtém a quantidade de linhas para pular antes de executar um corte.
        /// </summary>
        public byte LinhasEntreCupons { get; set; } = 5;

        /// <summary>
        /// Define/Obtém as configurações padrão da gaveta.
        /// </summary>
        public GavetaConfig Gaveta { get; } = new();

        /// <summary>
        /// Define/Obtém as configurações padrão do codigo de barras.
        /// </summary>
        public BarcodeConfig Barcode { get; } = new();

        /// <summary>
        /// Define/Obtém as configurações padrão para impressão do logo.
        /// </summary>
        public LogoConfig Logo { get; } = new();

        /// <summary>
        /// Define/Obtém as configurações padrão para impressão do QrCode.
        /// </summary>
        public QrCodeConfig QrCode { get; } = new();

        public bool Conectado => device?.Conectado ?? false;

        #endregion properties

        #region Methods

        /// <summary>
        /// Conecta na impressora.
        /// </summary>
        public void Conectar()
        {
            Guard.Against<OpenException>(Conectado, "A porta já está aberta");
            Guard.Against<NotImplementedException>(!Enum.IsDefined(typeof(ProtocoloEscPos), Protocolo), "Protocolo não implementado");

            device = OpenDeviceFactory.Create(Device);
            device.Open();

            interpreter = EscPosInterpreterFactory.Create(Protocolo);

            Inicializar();
        }

        /// <summary>
        /// Desconecta da impressora.
        /// </summary>
        public void Desconectar()
        {
            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            device?.Close();
            device?.Dispose();
            device = null;
            interpreter = null;

            inicializada = false;
        }

        private void Inicializar()
        {
            this.Log().Debug("Inicializar");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            if (inicializada) return;

            var cmd = new EspacoEntreLinhasCommand(interpreter) { Espaco = EspacoEntreLinhas };
            var dados = cmd.Content;

            device.Write(dados);
            inicializada = true;
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
        public void Reset()
        {
            this.Log().Debug("Reset");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new ZeraCommand(interpreter);
            var dados = cmd.Content;

            device.Write(dados);
            inicializada = false;
            Inicializar();
        }

        /// <summary>
        /// Adicionado o comando de beep no buffer.
        /// </summary>
        public void Beep()
        {
            this.Log().Debug("Beep");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new BeepCommand(interpreter);
            commands.Add(cmd);
        }

        /// <summary>
        /// Retornar o status da impressora.
        /// </summary>
        /// <param name="tentativas"></param>
        /// <returns></returns>
        public EscPosTipoStatus LerStatusImpressora(int tentativas = 1)
        {
            //ToDo: resolver o problema da função não estar funcionando.
            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            try
            {
                var dados = interpreter.GetStatusCommand();
                var ret = new List<byte[]>();
                foreach (var dado in dados)
                {
                    device.Write(dado);
                    Thread.Sleep(500);
                    ret.Add(device.Read());
                }

                var status = interpreter.ProcessarStatus(ret.ToArray());
                if (!Gaveta.SinalInvertido) return status;

                if (status.HasFlag(EscPosTipoStatus.GavetaAberta))
                    status &= ~EscPosTipoStatus.GavetaAberta;
                else
                    status |= EscPosTipoStatus.GavetaAberta;

                return status;
            }
            catch (Exception ex)
            {
                this.Log().Error("Erro ao ler status", ex);
                return EscPosTipoStatus.ErroLeitura;
            }
        }

        /// <summary>
        /// Adiciona o comando de pular linhas ao buffer.
        /// </summary>
        /// <param name="aLinhas"></param>
        public void PularLinhas(int aLinhas)
        {
            this.Log().Debug("PularLinhas");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new JumpLineCommand(interpreter)
            {
                Linhas = aLinhas <= 0 ? LinhasEntreCupons : aLinhas
            };

            commands.Add(cmd);
        }

        /// <summary>
        /// Adiciona o comando de imprimir linha ao buffer.
        /// </summary>
        /// <param name="dupla"></param>
        public void ImprimirLinha(bool dupla = false)
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            //Todo: Implementar o calculo do tamano da colunas de acordo com a fonte.
            var cmd = new PrintLineCommand(interpreter)
            {
                Tamanho = 48
            };

            commands.Add(cmd);
        }

        /// <summary>
        /// Adiciona o commando de corta papel ao buffer.
        /// </summary>
        /// <param name="parcial"></param>
        public void CortarPapel(bool parcial = false)
        {
            this.Log().Debug($"CortarPapel {(parcial ? "Parcial" : "Total")}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            commands.Add(new JumpLineCommand(interpreter)
            {
                Linhas = LinhasEntreCupons
            });

            commands.Add(new CutCommand(interpreter)
            {
                Parcial = parcial
            });
        }

        /// <summary>
        /// Adiciona o comando de abrir gaveta ao buffer.
        /// </summary>
        /// <param name="aGaveta"></param>
        public void AbrirGaveta(CmdGaveta aGaveta = CmdGaveta.GavetaUm)
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name} {(byte)aGaveta}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new CashDrawerCommand(interpreter)
            {
                Gaveta = aGaveta,
                TempoON = Gaveta.TempoON,
                TempoOFF = Gaveta.TempoOFF
            };

            commands.Add(cmd);
        }

        /// <summary>
        /// Adiciona o comando de impressão de logo ao buffer.
        /// </summary>
        /// <param name="kc1"></param>
        /// <param name="kc2"></param>
        /// <param name="fatorX"></param>
        /// <param name="fatorY"></param>
        public void ImprimirLogo(int kc1 = -1, int kc2 = -1, int fatorX = -1, int fatorY = -1)
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new LogoCommand(interpreter)
            {
                KC1 = kc1 < 0 ? Logo.KC1 : (byte)kc1,
                KC2 = kc2 < 0 ? Logo.KC2 : (byte)kc2,
                FatorX = fatorX < 0 ? Logo.FatorX : (byte)fatorX,
                FatorY = fatorY < 0 ? Logo.FatorY : (byte)fatorY,
            };

            commands.Add(cmd);
        }

        /// <summary>
        /// Adicionao o comando de impressão de texto ao buffer.
        /// </summary>
        /// <param name="aTexto"></param>
        public void ImprimirTexto(string aTexto)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, null);
        }

        /// <summary>
        /// Adicionao o comando de impressão de texto ao buffer.
        /// </summary>
        /// <param name="aTexto"></param>
        /// <param name="aAlinhamento"></param>
        public void ImprimirTexto(string aTexto, CmdAlinhamento aAlinhamento)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, aAlinhamento, null);
        }

        /// <summary>
        /// Adicionao o comando de impressão de texto ao buffer.
        /// </summary>
        /// <param name="aTexto"></param>
        /// <param name="tamanho"></param>
        /// <param name="aAlinhamento"></param>
        public void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, tamanho, aAlinhamento, null);
        }

        /// <summary>
        /// Adicionao o comando de impressão de texto ao buffer.
        /// </summary>
        /// <param name="aTexto"></param>
        /// <param name="tamanho"></param>
        public void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, tamanho, CmdAlinhamento.Esquerda, null);
        }

        /// <summary>
        /// Adicionao o comando de impressão de texto ao buffer.
        /// </summary>
        /// <param name="aTexto"></param>
        /// <param name="aEstilo"></param>
        public void ImprimirTexto(string aTexto, CmdEstiloFonte aEstilo)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, aEstilo);
        }

        /// <summary>
        /// Adicionao o comando de impressão de texto ao buffer.
        /// </summary>
        /// <param name="aTexto"></param>
        /// <param name="aAlinhamento"></param>
        /// <param name="aEstilo"></param>
        public void ImprimirTexto(string aTexto, CmdAlinhamento aAlinhamento, CmdEstiloFonte aEstilo)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, aAlinhamento, aEstilo);
        }

        /// <summary>
        /// Adicionao o comando de impressão de texto ao buffer.
        /// </summary>
        /// <param name="aTexto"></param>
        /// <param name="tamanho"></param>
        /// <param name="aAlinhamento"></param>
        /// <param name="aEstilo"></param>
        public void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, CmdEstiloFonte aEstilo)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, tamanho, aAlinhamento, aEstilo);
        }

        /// <summary>
        /// Adicionao o comando de impressão de texto ao buffer.
        /// </summary>
        /// <param name="aTexto"></param>
        /// <param name="fonte"></param>
        /// <param name="tamanho"></param>
        /// <param name="aAlinhamento"></param>
        /// <param name="aEstilo"></param>
        public void ImprimirTexto(string aTexto, CmdFonte fonte, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, CmdEstiloFonte? aEstilo)
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

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

        /// <summary>
        /// Adiciona o comando de impressão de codigo de barras ao buffer.
        /// </summary>
        /// <param name="aTexto"></param>
        /// <param name="barcode"></param>
        /// <param name="aAlinhamento"></param>
        /// <param name="exibir"></param>
        /// <param name="altura"></param>
        /// <param name="largura"></param>
        public void ImprimirBarcode(string aTexto, CmdBarcode barcode, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda,
            CmdBarcodeText exibir = CmdBarcodeText.SemTexto, int altura = 0, int largura = 0)
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new BarcodeCommand(interpreter)
            {
                Tipo = barcode,
                Code = aTexto,
                Alinhamento = aAlinhamento,
                Exibir = exibir,
                Altura = altura,
                Largura = largura
            };

            commands.Add(cmd);
        }

        /// <summary>
        /// Adiciona o comando de impressão de QrCode ao buffer.
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="aAlinhamento"></param>
        public void ImprimirQrCode(string texto, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda)
        {
            ImprimirQrCode(texto, null, 0, null, aAlinhamento);
        }

        /// <summary>
        /// Adiciona o comando de impressão de QrCode ao buffer.
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="tipo"></param>
        /// <param name="tamanho"></param>
        /// <param name="erroLevel"></param>
        /// <param name="aAlinhamento"></param>
        public void ImprimirQrCode(string texto, QrCodeTipo? tipo = null, QrCodeSize? tamanho = null, QrCodeErrorLevel? erroLevel = null, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda)
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new QrCodeCommand(interpreter)
            {
                Code = texto,
                Alinhamento = aAlinhamento,
                Tipo = tipo ?? QrCode.Tipo,
                Tamanho = tamanho ?? QrCode.Tamanho,
                ErrorLevel = erroLevel ?? QrCode.ErrorLevel,
            };

            commands.Add(cmd);
        }

        /// <summary>
        /// Imprime os comandos que estão no buffer de impressão.
        /// </summary>
        /// <param name="copias">Quantidade de copias apra impressão.</param>
        public void Imprimir(int copias = 1)
        {
            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");
            if (!commands.Any()) return;

            copias = Math.Max(1, copias);

            // Sempre precisa iniciar com o comando Zerar ele inicializa a impressora.
            if (commands[0] is not ZeraCommand)
                commands.Insert(0, new ZeraCommand(interpreter));

            var comandos = new List<byte>();

            foreach (var command in commands)
                comandos.AddRange(command.Content);

            var dados = comandos.ToArray();

            for (var i = 0; i < copias; i++)
                device.Write(dados);

            Clear();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            if (Conectado)
                Desconectar();
        }

        #endregion Methods
    }
}