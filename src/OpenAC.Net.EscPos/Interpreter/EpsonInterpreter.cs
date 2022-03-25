// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="EpsonInterpreter.cs" company="OpenAC .Net">
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
using System.Text;
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Interpreter
{
    /// <summary>
    /// Classe de interpretação com comandos da Epson.
    /// </summary>
    public class EpsonInterpreter : EscPosInterpreter
    {
        #region Constructors

        public EpsonInterpreter(Encoding enconder) : base(enconder)
        {
        }

        #endregion Constructors

        #region Methods

        /// <inheritdoc />
        public override byte[][] GetStatusCommand()
        {
            return new[]
            {
                new byte[] {16, 4, 1},
                new byte[] {16, 4, 2},
                new byte[] {16, 4, 4},
            };
        }

        /// <inheritdoc />
        public override EscPosTipoStatus ProcessarStatus(byte[][] dados)
        {
            if (dados.IsNullOrEmpty()) return EscPosTipoStatus.ErroLeitura;
            if (dados.Length < 3) return EscPosTipoStatus.ErroLeitura;

            EscPosTipoStatus? status = null;

            var bitTest = new Func<int, byte, bool>((value, index) => ((value >> index) & 1) == 1);

            var ret = dados[0];
            if (ret.Length > 0)
            {
                var b = ret[0];
                if (!bitTest(b, 2))
                    status |= EscPosTipoStatus.GavetaAberta;
                if (bitTest(b, 3))
                    status |= EscPosTipoStatus.OffLine;
                if (bitTest(b, 5))
                    status |= EscPosTipoStatus.Erro;
                if (bitTest(b, 6))
                    status |= EscPosTipoStatus.Imprimindo;
            }

            ret = dados[1];
            if (ret.Length > 0)
            {
                var b = ret[0];
                if (bitTest(b, 2))
                    status |= EscPosTipoStatus.TampaAberta;
                if (bitTest(b, 3))
                    status |= EscPosTipoStatus.Imprimindo;
                if (bitTest(b, 5))
                    status |= EscPosTipoStatus.SemPapel;
                if (bitTest(b, 6))
                    status |= EscPosTipoStatus.Erro;
            }

            ret = dados[2];
            if (ret.Length > 0)
            {
                var b = ret[0];
                if (bitTest(b, 2) && bitTest(b, 3))
                    status |= EscPosTipoStatus.PoucoPapel;

                if (bitTest(b, 5) && bitTest(b, 6))
                    status |= EscPosTipoStatus.SemPapel;
            }

            return status ?? EscPosTipoStatus.ErroLeitura;
        }

        /// <inheritdoc />
        protected override void InicializarComandos()
        {
            // Diversos
            Commandos.Add(CmdEscPos.Zera, new[] { CmdConst.ESC, (byte)'@' });
            Commandos.Add(CmdEscPos.Beep, new byte[] { CmdConst.ESC, (byte)'(', (byte)'A', 5, 0, 97, 100, 1, 50, 50 });
            Commandos.Add(CmdEscPos.EspacoEntreLinhasPadrao, new byte[] { CmdConst.ESC, 2 });
            Commandos.Add(CmdEscPos.EspacoEntreLinhas, new byte[] { CmdConst.ESC, 3 });
            Commandos.Add(CmdEscPos.PaginaDeCodigo, new[] { CmdConst.ESC, (byte)'t' });
            Commandos.Add(CmdEscPos.PuloDeLinha, new[] { CmdConst.LF });
            Commandos.Add(CmdEscPos.PuloDePagina, new[] { CmdConst.FF });
            Commandos.Add(CmdEscPos.ImprimePagina, new[] { CmdConst.ESC, CmdConst.FF });

            // Alinhamento
            Commandos.Add(CmdEscPos.AlinhadoEsquerda, new byte[] { CmdConst.ESC, (byte)'a', 0 });
            Commandos.Add(CmdEscPos.AlinhadoCentro, new byte[] { CmdConst.ESC, (byte)'a', 1 });
            Commandos.Add(CmdEscPos.AlinhadoDireita, new byte[] { CmdConst.ESC, (byte)'a', 2 });

            // Fonte
            Commandos.Add(CmdEscPos.FonteNormal, new byte[] { CmdConst.ESC, (byte)'!', 0 });
            Commandos.Add(CmdEscPos.FonteA, new byte[] { CmdConst.ESC, (byte)'M', 0 });
            Commandos.Add(CmdEscPos.FonteB, new byte[] { CmdConst.ESC, (byte)'M', 1 });
            Commandos.Add(CmdEscPos.LigaExpandido, new byte[] { CmdConst.GS, (byte)'!', 16 });
            Commandos.Add(CmdEscPos.DesligaExpandido, new byte[] { CmdConst.GS, (byte)'!', 0 });
            Commandos.Add(CmdEscPos.LigaCondensado, new[] { CmdConst.SI });
            Commandos.Add(CmdEscPos.DesligaCondensado, new[] { CmdConst.DC2 });
            Commandos.Add(CmdEscPos.LigaNegrito, new byte[] { CmdConst.ESC, (byte)'E', 1 });
            Commandos.Add(CmdEscPos.DesligaNegrito, new byte[] { CmdConst.ESC, (byte)'E', 0 });
            Commandos.Add(CmdEscPos.LigaSublinhado, new byte[] { CmdConst.ESC, (byte)'-', 1 });
            Commandos.Add(CmdEscPos.DesligaSublinhado, new byte[] { CmdConst.ESC, (byte)'-', 0 });
            Commandos.Add(CmdEscPos.LigaInvertido, new byte[] { CmdConst.GS, (byte)'B', 1 });
            Commandos.Add(CmdEscPos.DesligaInvertido, new byte[] { CmdConst.GS, (byte)'B', 0 });
            Commandos.Add(CmdEscPos.LigaAlturaDupla, new byte[] { CmdConst.GS, (byte)'!', 1 });
            Commandos.Add(CmdEscPos.DesligaAlturaDupla, new byte[] { CmdConst.GS, (byte)'!', 0 });

            //Corte
            Commandos.Add(CmdEscPos.CorteTotal, new byte[] { CmdConst.GS, (byte)'V', 0 });
            Commandos.Add(CmdEscPos.CorteParcial, new byte[] { CmdConst.GS, (byte)'V', 1 });

            // ModoPagina
            Commandos.Add(CmdEscPos.LigaModoPagina, new byte[] { CmdConst.ESC, (byte)'L', 0 });
            Commandos.Add(CmdEscPos.DesligaModoPagina, new byte[] { CmdConst.ESC, (byte)'S', 0 });

            // Gaveta
            Commandos.Add(CmdEscPos.Gaveta, new[] { CmdConst.ESC, (byte)'p' });

            // Barcodes
            Commandos.Add(CmdEscPos.IniciarBarcode, new byte[] { CmdConst.GS, 107 });
            Commandos.Add(CmdEscPos.BarcodeWidth, new byte[] { CmdConst.GS, 119 });
            Commandos.Add(CmdEscPos.BarcodeHeight, new byte[] { CmdConst.GS, 104 });
            Commandos.Add(CmdEscPos.BarcodeNoText, new byte[] { CmdConst.GS, 72, 0 });
            Commandos.Add(CmdEscPos.BarcodeTextAbove, new byte[] { CmdConst.GS, 72, 1 });
            Commandos.Add(CmdEscPos.BarcodeTextBelow, new byte[] { CmdConst.GS, 72, 2 });
            Commandos.Add(CmdEscPos.BarcodeTextBoth, new byte[] { CmdConst.GS, 72, 3 });
            Commandos.Add(CmdEscPos.BarcodeUPCA, new byte[] { 65 });
            Commandos.Add(CmdEscPos.BarcodeUPCE, new byte[] { 66 });
            Commandos.Add(CmdEscPos.BarcodeEAN13, new byte[] { 67 });
            Commandos.Add(CmdEscPos.BarcodeEAN8, new byte[] { 68 });
            Commandos.Add(CmdEscPos.BarcodeCODE39, new byte[] { 69 });
            Commandos.Add(CmdEscPos.BarcodeInter2of5, new byte[] { 70 });
            Commandos.Add(CmdEscPos.BarcodeCodaBar, new byte[] { 71 });
            Commandos.Add(CmdEscPos.BarcodeCODE93, new byte[] { 72 });
            Commandos.Add(CmdEscPos.BarcodeCODE128, new byte[] { 73 });

            // Logo
            Commandos.Add(CmdEscPos.LogoNew, new byte[] { CmdConst.GS, 40, 76, 6, 0, 48, 69 });
            Commandos.Add(CmdEscPos.LogoOld, new byte[] { CmdConst.FS, 112 });

            // QrCode
            Commandos.Add(CmdEscPos.QrCodeInitial, new byte[] { CmdConst.GS, 40, 107 });
            Commandos.Add(CmdEscPos.QrCodeModel, new byte[] { 4, 0, 49, 65 });
            Commandos.Add(CmdEscPos.QrCodeSize, new byte[] { 3, 0, 49, 67 });
            Commandos.Add(CmdEscPos.QrCodeError, new byte[] { 3, 0, 49, 69 });
            Commandos.Add(CmdEscPos.QrCodeStore, new byte[] { 49, 80, 48 });
            Commandos.Add(CmdEscPos.QrCodePrint, new byte[] { 3, 0, 49, 81, 48 });
        }

        #endregion Methods
    }
}