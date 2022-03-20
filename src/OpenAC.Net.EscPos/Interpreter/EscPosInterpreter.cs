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
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Extensions;

namespace OpenAC.Net.EscPos.Interpreter
{
    public abstract class EscPosInterpreter
    {
        #region Constructors

        protected EscPosInterpreter() => InicializarComandos();

        #endregion Constructors

        #region Properties

        protected Dictionary<CmdEscPos, byte[]> Commandos { get; } = new();

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
            return command switch
            {
                TextCommand cmd => ProcessCommand(cmd),
                ZeraCommand cmd => ProcessCommand(cmd),
                EspacoEntreLinhasCommand cmd => ProcessCommand(cmd),
                JumpLineCommand cmd => ProcessCommand(cmd),
                CutCommand cmd => ProcessCommand(cmd),
                CashDrawerCommand cmd => ProcessCommand(cmd),
                BeepCommand cmd => ProcessCommand(cmd),
                BarcodeCommand cmd => ProcessCommand(cmd),
                LogoCommand cmd => ProcessCommand(cmd),
                QrCodeCommand cmd => ProcessCommand(cmd),
                _ => throw new ArgumentException("Comando custom, não pode ser usado no interpretador.")
            };
        }

        public abstract byte[][] GetStatusCommand();

        public abstract EscPosTipoStatus ProcessarStatus(byte[][] dados);

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
                case CmdFonte.Normal when Commandos.ContainsKey(CmdEscPos.FonteNormal):
                    list.AddRange(Commandos[CmdEscPos.FonteNormal]);
                    break;

                case CmdFonte.FonteA when Commandos.ContainsKey(CmdEscPos.FonteA):
                    list.AddRange(Commandos[CmdEscPos.FonteA]);
                    break;

                case CmdFonte.FonteB when Commandos.ContainsKey(CmdEscPos.FonteB):
                    list.AddRange(Commandos[CmdEscPos.FonteB]);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (cmd.Alinhamento)
            {
                case CmdAlinhamento.Esquerda when Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda):
                    list.AddRange(Commandos[CmdEscPos.AlinhadoEsquerda]);
                    break;

                case CmdAlinhamento.Centro when Commandos.ContainsKey(CmdEscPos.AlinhadoCentro):
                    list.AddRange(Commandos[CmdEscPos.AlinhadoCentro]);
                    break;

                case CmdAlinhamento.Direita when Commandos.ContainsKey(CmdEscPos.AlinhadoDireita):
                    list.AddRange(Commandos[CmdEscPos.AlinhadoDireita]);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            switch (cmd.Tamanho)
            {
                case CmdTamanhoFonte.Expandida when Commandos.ContainsKey(CmdEscPos.LigaExpandido):
                    list.AddRange(Commandos[CmdEscPos.LigaExpandido]);
                    break;

                case CmdTamanhoFonte.Condensada when Commandos.ContainsKey(CmdEscPos.LigaCondensado):
                    list.AddRange(Commandos[CmdEscPos.LigaCondensado]);
                    break;
            }

            if (cmd.Estilo.HasValue)
            {
                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Negrito) && Commandos.ContainsKey(CmdEscPos.LigaNegrito))
                    list.AddRange(Commandos[CmdEscPos.LigaNegrito]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Sublinhado) && Commandos.ContainsKey(CmdEscPos.LigaSublinhado))
                    list.AddRange(Commandos[CmdEscPos.LigaSublinhado]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Invertido) && Commandos.ContainsKey(CmdEscPos.LigaInvertido))
                    list.AddRange(Commandos[CmdEscPos.LigaSublinhado]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Italico) && Commandos.ContainsKey(CmdEscPos.LigaItalico))
                    list.AddRange(Commandos[CmdEscPos.LigaItalico]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.AlturaDupla) && Commandos.ContainsKey(CmdEscPos.LigaAlturaDupla))
                    list.AddRange(Commandos[CmdEscPos.LigaAlturaDupla]);
            }

            // Adiciona o texto, fazendo o tratamento para quebra de linha
            list.AddRange(Encoding.UTF8.GetBytes(TratarString(cmd.Texto)));

            if (cmd.Estilo.HasValue)
            {
                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Negrito) && Commandos.ContainsKey(CmdEscPos.DesligaNegrito))
                    list.AddRange(Commandos[CmdEscPos.DesligaNegrito]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Sublinhado) && Commandos.ContainsKey(CmdEscPos.DesligaSublinhado))
                    list.AddRange(Commandos[CmdEscPos.DesligaSublinhado]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Invertido) && Commandos.ContainsKey(CmdEscPos.DesligaInvertido))
                    list.AddRange(Commandos[CmdEscPos.LigaSublinhado]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Italico) && Commandos.ContainsKey(CmdEscPos.DesligaItalico))
                    list.AddRange(Commandos[CmdEscPos.DesligaItalico]);

                if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.AlturaDupla) && Commandos.ContainsKey(CmdEscPos.DesligaAlturaDupla))
                    list.AddRange(Commandos[CmdEscPos.DesligaAlturaDupla]);
            }

            switch (cmd.Tamanho)
            {
                case CmdTamanhoFonte.Expandida when Commandos.ContainsKey(CmdEscPos.DesligaExpandido):
                    list.AddRange(Commandos[CmdEscPos.DesligaExpandido]);
                    break;

                case CmdTamanhoFonte.Condensada when Commandos.ContainsKey(CmdEscPos.DesligaCondensado):
                    list.AddRange(Commandos[CmdEscPos.DesligaCondensado]);
                    break;
            }

            // Volta a Fonte para Normal.
            if (Commandos.ContainsKey(CmdEscPos.FonteNormal) && cmd.Fonte != CmdFonte.Normal)
                list.AddRange(Commandos[CmdEscPos.FonteNormal]);

            // Volta alinhamento para Esquerda.
            if (Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda) && cmd.Alinhamento != CmdAlinhamento.Esquerda)
                list.AddRange(Commandos[CmdEscPos.AlinhadoEsquerda]);

            // Texto sempre tem que terminar com o pulo de linha
            list.AddRange(Commandos[CmdEscPos.PuloDeLinha]);
            return list.ToArray();
        }

        /// <summary>
        /// Processa o comando Zera.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected virtual byte[] ProcessCommand(ZeraCommand cmd) => Commandos.ContainsKey(CmdEscPos.Zera) ? Commandos[CmdEscPos.Zera] : new byte[0];

        /// <summary>
        /// Processa o comando de Espaco Entre Linhas.
        /// </summary>
        /// <param name="cmd"></param>
        /// <returns></returns>
        protected virtual byte[] ProcessCommand(EspacoEntreLinhasCommand cmd)
        {
            if (!Commandos.ContainsKey(CmdEscPos.EspacoEntreLinhasPadrao)) return new byte[0];

            var espacos = Math.Max((byte)0, cmd.Espaco);
            if (espacos == 0) return Commandos[CmdEscPos.EspacoEntreLinhasPadrao];

            if (!Commandos.ContainsKey(CmdEscPos.EspacoEntreLinhas)) return new byte[0];

            return Commandos[CmdEscPos.EspacoEntreLinhas].Concat(new[] { espacos }).ToArray();
        }

        protected virtual byte[] ProcessCommand(JumpLineCommand cmd)
        {
            if (!Commandos.ContainsKey(CmdEscPos.PuloDeLinha)) return new byte[0];

            var linhas = Math.Max(1, cmd.Linhas);
            var list = new List<byte>();
            for (var i = 0; i < linhas; i++)
                list.AddRange(Commandos[CmdEscPos.PuloDeLinha]);

            return list.ToArray();
        }

        protected virtual byte[] ProcessCommand(CutCommand cmd)
        {
            if (!Commandos.ContainsKey(CmdEscPos.CorteParcial) && cmd.Parcial) return new byte[0];
            if (!Commandos.ContainsKey(CmdEscPos.CorteTotal)) return new byte[0];

            return cmd.Parcial ? Commandos[CmdEscPos.CorteParcial] : Commandos[CmdEscPos.CorteTotal];
        }

        protected virtual byte[] ProcessCommand(CashDrawerCommand cmd)
        {
            return Commandos.ContainsKey(CmdEscPos.Gaveta)
                ? Commandos[CmdEscPos.Gaveta].Concat(new[] { (byte)cmd.Gaveta, cmd.TempoON, cmd.TempoOFF }).ToArray()
                : new byte[0];
        }

        protected virtual byte[] ProcessCommand(BeepCommand cmd) => !Commandos.ContainsKey(CmdEscPos.Beep) ? new byte[0] : Commandos[CmdEscPos.Beep];

        protected virtual byte[] ProcessCommand(BarcodeCommand cmd)
        {
            if (!Commandos.ContainsKey(CmdEscPos.IniciarBarcode)) return new byte[0];

            var list = new List<byte>();

            switch (cmd.Alinhamento)
            {
                case CmdAlinhamento.Esquerda when Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda):
                    list.AddRange(Commandos[CmdEscPos.AlinhadoEsquerda]);
                    break;

                case CmdAlinhamento.Centro when Commandos.ContainsKey(CmdEscPos.AlinhadoCentro):
                    list.AddRange(Commandos[CmdEscPos.AlinhadoCentro]);
                    break;

                case CmdAlinhamento.Direita when Commandos.ContainsKey(CmdEscPos.AlinhadoDireita):
                    list.AddRange(Commandos[CmdEscPos.AlinhadoDireita]);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            // Formando o codigo de barras
            byte[] barCode;
            switch (cmd.Tipo)
            {
                case CmdBarcode.UPCA when Commandos.ContainsKey(CmdEscPos.BarcodeUPCA):
                    barCode = Commandos[CmdEscPos.BarcodeUPCA];
                    break;

                case CmdBarcode.UPCE when Commandos.ContainsKey(CmdEscPos.BarcodeUPCE):
                    barCode = Commandos[CmdEscPos.BarcodeUPCE];
                    break;

                case CmdBarcode.EAN13 when Commandos.ContainsKey(CmdEscPos.BarcodeEAN13):
                    barCode = Commandos[CmdEscPos.BarcodeEAN13];
                    break;

                case CmdBarcode.EAN8 when Commandos.ContainsKey(CmdEscPos.BarcodeEAN8):
                    barCode = Commandos[CmdEscPos.BarcodeEAN8];
                    break;

                case CmdBarcode.CODE39 when Commandos.ContainsKey(CmdEscPos.BarcodeCODE39):
                    barCode = Commandos[CmdEscPos.BarcodeCODE39];
                    break;

                case CmdBarcode.Inter2of5 when Commandos.ContainsKey(CmdEscPos.BarcodeInter2of5):
                    barCode = Commandos[CmdEscPos.BarcodeInter2of5];
                    break;

                case CmdBarcode.CodaBar when Commandos.ContainsKey(CmdEscPos.BarcodeCodaBar):
                    barCode = Commandos[CmdEscPos.BarcodeCodaBar];
                    break;

                case CmdBarcode.CODE93 when Commandos.ContainsKey(CmdEscPos.BarcodeCODE93):
                    barCode = Commandos[CmdEscPos.BarcodeCODE93];
                    break;

                case CmdBarcode.CODE128b when Commandos.ContainsKey(CmdEscPos.BarcodeCODE128):
                case CmdBarcode.CODE128 when Commandos.ContainsKey(CmdEscPos.BarcodeCODE128):
                    barCode = Commandos[CmdEscPos.BarcodeCODE128];
                    if (!cmd.Code.StartsWith("{"))
                        cmd.Code = "{B" + cmd.Code;
                    break;

                case CmdBarcode.CODE128a when Commandos.ContainsKey(CmdEscPos.BarcodeCODE128):
                    barCode = Commandos[CmdEscPos.BarcodeCODE128];
                    if (!cmd.Code.StartsWith("{"))
                        cmd.Code = "{A" + cmd.Code;
                    break;

                case CmdBarcode.CODE128c when Commandos.ContainsKey(CmdEscPos.BarcodeCODE128):
                    barCode = Commandos[CmdEscPos.BarcodeCODE128];
                    var code = cmd.Code.OnlyNumbers();
                    var s = code.Length;
                    if (s % 2 != 0)
                    {
                        code = "0" + code;
                        s++;
                    }

                    var code128c = "";
                    var i = 0;
                    while (i < s)
                    {
                        code128c += (char)code.Substring(i, 2).ToInt32();
                        i += 2;
                    }

                    cmd.Code = "{C" + code128c;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            var largura = cmd.Largura == 0 ? (byte)2 : Math.Max(Math.Min((byte)cmd.Largura, (byte)4), (byte)1);
            var altura = cmd.Altura == 0 ? (byte)50 : Math.Max(Math.Min((byte)cmd.Altura, (byte)255), (byte)1);

            var showCode = cmd.Exibir switch
            {
                CmdBarcodeText.SemTexto when Commandos.ContainsKey(CmdEscPos.BarcodeNoText) => Commandos[CmdEscPos.BarcodeNoText],
                CmdBarcodeText.Acima when Commandos.ContainsKey(CmdEscPos.BarcodeTextAbove) => Commandos[CmdEscPos.BarcodeTextAbove],
                CmdBarcodeText.Abaixo when Commandos.ContainsKey(CmdEscPos.BarcodeTextBelow) => Commandos[CmdEscPos.BarcodeTextBelow],
                CmdBarcodeText.Ambos when Commandos.ContainsKey(CmdEscPos.BarcodeTextBoth) => Commandos[CmdEscPos.BarcodeTextBoth],
                _ => throw new ArgumentOutOfRangeException()
            };

            if (Commandos.ContainsKey(CmdEscPos.BarcodeWidth))
                list.AddRange(Commandos[CmdEscPos.BarcodeHeight], largura);

            if (Commandos.ContainsKey(CmdEscPos.BarcodeHeight))
                list.AddRange(Commandos[CmdEscPos.BarcodeHeight], altura);

            if (!showCode.IsNullOrEmpty())
                list.AddRange(showCode);

            list.AddRange(Commandos[CmdEscPos.IniciarBarcode], barCode);
            list.Add((byte)cmd.Code.Length);
            list.AddRange(Encoding.UTF8.GetBytes(cmd.Code));

            // Volta alinhamento para Esquerda.
            if (Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda) && cmd.Alinhamento != CmdAlinhamento.Esquerda)
                list.AddRange(Commandos[CmdEscPos.AlinhadoEsquerda]);

            return list.ToArray();
        }

        protected virtual byte[] ProcessCommand(LogoCommand cmd)
        {
            if (!Commandos.ContainsKey(CmdEscPos.LogoNew) &&
               !Commandos.ContainsKey(CmdEscPos.LogoOld)) return new byte[0];

            // Verificando se informou o KeyCode compatível com o comando Novo ou Antigo.

            // Nota: O Comando novo da Epson "GS + '(L'", não é compatível em alguns
            // Equipamentos(não Epson), mas que usam EscPosEpson...
            // Nesse caso, vamos usar o comando "FS + 'p'", para tal, informe:
            // KeyCode1:= 1..255; KeyCode2:= 0

            var keyCodeUnico = new Func<byte, byte>(keycode => (keycode is < 32 or > 126) ? (byte)((char)keycode).ToInt32() : keycode);

            if (cmd.KC2 == 0)
            {
                var keyCode = keyCodeUnico((byte)cmd.KC1);

                byte m = 0;
                if (cmd.FatorX > 1)
                    m += 1;

                if (cmd.FatorY > 1)
                    m += 2;

                return Commandos[CmdEscPos.LogoOld].Concat(new[] { keyCode, m }).ToArray();
            }

            return Commandos[CmdEscPos.LogoNew].Concat(new[] { cmd.KC1, cmd.KC2, cmd.FatorX, cmd.FatorY }).ToArray();
        }

        protected virtual byte[] ProcessCommand(QrCodeCommand cmd)
        {
            //ToDo: Comando não funcionando corretamente ainda.
            if (!Commandos.ContainsKey(CmdEscPos.QrCodePrint)) return new byte[0];

            var list = new List<byte>();

            switch (cmd.Alinhamento)
            {
                case CmdAlinhamento.Esquerda when Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda):
                    list.AddRange(Commandos[CmdEscPos.AlinhadoEsquerda]);
                    break;

                case CmdAlinhamento.Centro when Commandos.ContainsKey(CmdEscPos.AlinhadoCentro):
                    list.AddRange(Commandos[CmdEscPos.AlinhadoCentro]);
                    break;

                case CmdAlinhamento.Direita when Commandos.ContainsKey(CmdEscPos.AlinhadoDireita):
                    list.AddRange(Commandos[CmdEscPos.AlinhadoDireita]);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            var num = cmd.Code.Length + 3;
            var pL = (byte)(num % 256);
            var pH = (byte)(num / 256);

            var initial = Commandos[CmdEscPos.QrCodeInitial];
            list.AddRange(initial, Commandos[CmdEscPos.QrCodeModel], (byte)cmd.Tipo, (byte)0);
            list.AddRange(initial, Commandos[CmdEscPos.QrCodeSize], (byte)cmd.Tamanho);
            list.AddRange(initial, Commandos[CmdEscPos.QrCodeError], (byte)cmd.ErrorLevel);
            list.AddRange(initial, pL, pH, Commandos[CmdEscPos.QrCodeStore], Encoding.UTF8.GetBytes(cmd.Code));
            list.AddRange(initial, Commandos[CmdEscPos.QrCodePrint]);

            // Volta alinhamento para Esquerda.
            if (Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda) && cmd.Alinhamento != CmdAlinhamento.Esquerda)
                list.AddRange(Commandos[CmdEscPos.AlinhadoEsquerda]);

            return list.ToArray();
        }

        /// <summary>
        /// Função para tratar a quebra de linha da string para o formato que a impressora aceita.
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        protected virtual string TratarString(string texto) => texto.Replace("\r\n", "\n").Replace("\r", "\n");

        /// <summary>
        /// Função para inicializar o dicionario de comandos para ser usados no interpreter.
        /// </summary>
        protected abstract void InicializarComandos();

        #endregion Methods
    }
}