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
using System.Drawing;
using System.Linq;
using System.Reflection;
using OpenAC.Net.Core;
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.Core.Logging;
using OpenAC.Net.Devices;
using OpenAC.Net.Devices.Commom;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter;

namespace OpenAC.Net.EscPos
{
    public abstract class EscPosPrinter : OpenDisposable, IOpenLog
    {
        #region Fields

        private readonly List<IPrintCommand> commands;
        private OpenDeviceStream device;
        private EscPosInterpreter interpreter;
        private ProtocoloEscPos protocolo;
        private bool inicializada;
        private PaginaCodigo paginaCodigo;

        #endregion Fields

        #region Constructors

        protected EscPosPrinter(IDeviceConfig device)
        {
            Guard.Against<ArgumentNullException>(device == null, "As configurações de device não pode ser nulas.");

            Device = device;

            commands = new List<IPrintCommand>();

            paginaCodigo = PaginaCodigo.pc850;
            protocolo = ProtocoloEscPos.EscPos;
            interpreter = EscPosInterpreterFactory.Create(protocolo, paginaCodigo);
        }

        #endregion Constructors

        #region properties

        /// <summary>
        /// Configurações de comunicação com a impressora.
        /// </summary>
        public IDeviceConfig Device { get; }

        /// <summary>
        /// Define/Obtém o protocolo de comunicação.
        /// </summary>
        public ProtocoloEscPos Protocolo
        {
            get => protocolo;
            set
            {
                Guard.Against<OpenException>(Conectado, "Não pode mudar o protocolo quando esta conectado.");
                if (protocolo == value) return;

                protocolo = value;

                interpreter = null;
                interpreter = EscPosInterpreterFactory.Create(value, PaginaCodigo);
            }
        }

        /// <summary>
        /// Define/Obtém o enconding para comunicação com a impressora.
        /// </summary>
        public PaginaCodigo PaginaCodigo
        {
            get => paginaCodigo;
            set
            {
                Guard.Against<OpenException>(Conectado, "Não pode mudar o protocolo quando esta conectado.");
                if (paginaCodigo == value) return;

                paginaCodigo = value;

                interpreter = null;
                interpreter = EscPosInterpreterFactory.Create(protocolo, value);
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
        public BarcodeConfig CodigoBarras { get; } = new();

        /// <summary>
        /// Define/Obtém as configurações padrão para impressão do logo.
        /// </summary>
        public LogoConfig Logo { get; } = new();

        /// <summary>
        /// Define/Obtém as configurações padrão para impressão do QrCode.
        /// </summary>
        public QrCodeConfig QrCode { get; } = new();

        /// <summary>
        /// Define se esta conectado com a impressora.
        /// </summary>
        public bool Conectado => device?.Conectado ?? false;

        /// <summary>
        /// Retorna se o protocolo tem suporte ao modo pagina.
        /// </summary>
        public bool SuportaModoPagina => interpreter?.CommandResolver?.HasResolver<ModoPaginaCommand>() ?? false;

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
                if (interpreter == null) return 0;
                return (int)Math.Truncate(Colunas / interpreter.RazaoColuna.Expandida);
            }
        }

        /// <summary>
        /// Retorna a quantidade de colunas quando a fonte é concensada.
        /// </summary>
        public int ColunasCondensada
        {
            get
            {
                if (interpreter == null) return 0;
                return (int)Math.Truncate(Colunas / interpreter.RazaoColuna.Condensada);
            }
        }

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

            using var builder = new ByteArrayBuilder();

            IPrintCommand cmd;
            if (interpreter.CommandResolver.HasResolver<EspacoEntreLinhasCommand>())
            {
                cmd = new EspacoEntreLinhasCommand(interpreter) { Espaco = EspacoEntreLinhas };
                builder.Append(cmd.Content);
            }

            if (interpreter.CommandResolver.HasResolver<CodePageCommand>())
            {
                cmd = new CodePageCommand(interpreter) { PaginaCodigo = paginaCodigo };
                builder.Append(cmd.Content);
            }

            if (builder.Length > 0)
                device.Write(builder.ToArray());

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
            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            if (interpreter.Status == null) return EscPosTipoStatus.ErroLeitura;

            try
            {
                byte[][] ret;
                var leituras = 0;
                do
                {
                    var dados = interpreter.Status.Commands;
                    if (dados.IsNullOrEmpty()) return EscPosTipoStatus.Nenhum;

                    ret = dados.Select(dado => device.WriteRead(dado, 10)).ToArray();
                    leituras++;
                } while (leituras < tentativas && !ret.Any());

                if (!ret.Any()) return EscPosTipoStatus.ErroLeitura;

                var status = interpreter.Status.Resolve(ret);
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
        /// Le as informações da impressora.
        /// </summary>
        /// <returns></returns>
        public InformacoesImpressora LerInfoImpressora()
        {
            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            if (interpreter.InfoImpressora == null) return InformacoesImpressora.Empty;

            try
            {
                var dados = interpreter.InfoImpressora.Commands;
                if (dados.IsNullOrEmpty()) return null;

                var ret = dados.Select(dado => device.WriteRead(dado, 500)).ToArray();
                return !ret.Any() ? InformacoesImpressora.Empty : interpreter.InfoImpressora.Resolve(ret);
            }
            catch (Exception ex)
            {
                this.Log().Error("Erro ao ler as informações da impressora", ex);
                return InformacoesImpressora.Empty;
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
        /// <param name="tamanho"></param>
        /// <param name="dupla"></param>
        public void ImprimirLinha(int tamanho, bool dupla = false)
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new PrintLineCommand(interpreter)
            {
                Tamanho = tamanho
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
        public void ImprimirLogo(byte? kc1 = null, byte? kc2 = null, byte? fatorX = null, byte? fatorY = null)
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new LogoCommand(interpreter)
            {
                KC1 = kc1 ?? Logo.KC1,
                KC2 = kc2 ?? Logo.KC2,
                FatorX = fatorX ?? Logo.FatorX,
                FatorY = fatorY ?? Logo.FatorY
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
        /// <param name="tamanho"></param>
        /// <param name="aEstilo"></param>
        public void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdEstiloFonte aEstilo)
        {
            ImprimirTexto(aTexto, CmdFonte.Normal, tamanho, CmdAlinhamento.Esquerda, aEstilo);
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
        public void ImprimirBarcode(string aTexto, CmdBarcode barcode)
        {
            ImprimirBarcode(aTexto, barcode, CmdAlinhamento.Esquerda);
        }

        /// <summary>
        /// Adiciona o comando de impressão de codigo de barras ao buffer.
        /// </summary>
        /// <param name="aTexto"></param>
        /// <param name="barcode"></param>
        /// <param name="exibir"></param>
        public void ImprimirBarcode(string aTexto, CmdBarcode barcode, CmdBarcodeText exibir)
        {
            ImprimirBarcode(aTexto, barcode, CmdAlinhamento.Esquerda, exibir);
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
        public void ImprimirBarcode(string aTexto, CmdBarcode barcode, CmdAlinhamento aAlinhamento, CmdBarcodeText? exibir = null, int? altura = null, int? largura = null)
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new BarcodeCommand(interpreter)
            {
                Tipo = barcode,
                Code = aTexto,
                Alinhamento = aAlinhamento,
                Exibir = exibir ?? CodigoBarras.Exibir,
                Altura = altura ?? CodigoBarras.Altura,
                Largura = largura ?? CodigoBarras.Largura
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
        public void ImprimirQrCode(string texto, QrCodeTipo? tipo = null, QrCodeModSize? tamanho = null, QrCodeErrorLevel? erroLevel = null, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda)
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new QrCodeCommand(interpreter)
            {
                Code = texto,
                Alinhamento = aAlinhamento,
                Tipo = tipo ?? QrCode.Tipo,
                LarguraModulo = tamanho ?? QrCode.Tamanho,
                ErrorLevel = erroLevel ?? QrCode.ErrorLevel,
            };

            commands.Add(cmd);
        }

        /// <summary>
        /// Adicionado o comando para imprimir imagem ao buffer.
        /// </summary>
        /// <param name="imagem"></param>
        /// <param name="aAlinhamento"></param>
        /// <param name="isHdpi"></param>
        public void ImprimirImagem(Image imagem, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda, bool isHdpi = false)
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new ImageCommand(interpreter)
            {
                Imagem = imagem,
                IsHiDPI = isHdpi
            };

            commands.Add(cmd);
        }

        /// <summary>
        /// Inicia o modo pagina em impressoras compativeis.
        /// </summary>
        /// <returns></returns>
        public ModoPaginaCommand IniciarModoPagina()
        {
            this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

            Guard.Against<OpenException>(!Conectado, "A porta não está aberta");

            var cmd = new ModoPaginaCommand(interpreter)
            {
                EspacoEntreLinhas = EspacoEntreLinhas,
                QrCode = QrCode,
                Gaveta = Gaveta,
                CodigoBarras = CodigoBarras,
                Logo = Logo
            };
            commands.Add(cmd);
            return cmd;
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

            using var builder = new ByteArrayBuilder();

            foreach (var command in commands)
                builder.Append(command.Content);

            var dados = builder.ToArray();

            for (var i = 0; i < copias; i++)
                device.Write(dados);

            Clear();
        }

        /// <inheritdoc />
        protected override void DisposeManaged()
        {
            if (Conectado)
                Desconectar();
        }

        #endregion Methods
    }
}