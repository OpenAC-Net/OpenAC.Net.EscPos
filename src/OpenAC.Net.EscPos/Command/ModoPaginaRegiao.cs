// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="ModoPaginaRegiao.cs" company="OpenAC .Net">
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
using System.Reflection;
using OpenAC.Net.Core.Logging;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter;

namespace OpenAC.Net.EscPos.Command;

public sealed class ModoPaginaRegiao : IOpenLog
{
    #region Fields

    private readonly List<IPrintCommand> commands;
    private readonly ModoPaginaCommand parent;
    private readonly EscPosInterpreter interpreter;

    #endregion Fields

    #region Constructors

    internal ModoPaginaRegiao(ModoPaginaCommand parent, EscPosInterpreter interpreter)
    {
        this.parent = parent;
        this.interpreter = interpreter;
        commands = new List<IPrintCommand>();
    }

    #endregion Constructors

    #region Properties

    public int Largura { get; set; } = 0;

    public int Altura { get; set; } = 0;

    public int Esquerda { get; set; } = 0;

    public int Topo { get; set; } = 0;

    public CmdPosDirecao Direcao { get; set; } = CmdPosDirecao.EsquerdaParaDireita;

    public byte EspacoEntreLinhas { get; set; } = 0;

    /// <summary>
    /// Comandos para serem impressos dentro do modo pagina.
    /// </summary>
    public IReadOnlyList<IPrintCommand> Commands => commands;

    #endregion Properties

    #region Methods

    /// <summary>
    /// Limpa o buffer de impressão da região.
    /// </summary>
    public void Clear() => commands.Clear();

    /// <summary>
    /// Adiciona o comando de pular linhas ao buffer de impressão da região.
    /// </summary>
    /// <param name="aLinhas"></param>
    public void PularLinhas(int aLinhas)
    {
        var cmd = new JumpLineCommand(interpreter)
        {
            Linhas = Math.Max(aLinhas, 1)
        };

        commands.Add(cmd);
    }

    /// <summary>
    /// Adiciona o comando de imprimir linha ao buffer de impressão da região.
    /// </summary>
    /// <param name="tamanho"></param>
    /// <param name="dupla"></param>
    public void ImprimirLinha(int tamanho, bool dupla = false)
    {
        var cmd = new PrintLineCommand(interpreter)
        {
            Tamanho = tamanho
        };

        commands.Add(cmd);
    }

    /// <summary>
    /// Adiciona o comando de abrir gaveta ao buffer de impressão da região.
    /// </summary>
    /// <param name="aGaveta"></param>
    public void AbrirGaveta(CmdGaveta aGaveta = CmdGaveta.GavetaUm)
    {
        this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name} {(byte)aGaveta}");

        var cmd = new CashDrawerCommand(interpreter)
        {
            Gaveta = aGaveta,
            TempoON = parent.Gaveta.TempoON,
            TempoOFF = parent.Gaveta.TempoOFF
        };

        commands.Add(cmd);
    }

    /// <summary>
    /// Adiciona o comando de impressão de logo ao buffer de impressão da região.
    /// </summary>
    /// <param name="kc1"></param>
    /// <param name="kc2"></param>
    /// <param name="fatorX"></param>
    /// <param name="fatorY"></param>
    public void ImprimirLogo(byte? kc1 = null, byte? kc2 = null, byte? fatorX = null, byte? fatorY = null)
    {
        this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

        var cmd = new LogoCommand(interpreter)
        {
            KC1 = kc1 ?? parent.Logo.KC1,
            KC2 = kc2 ?? parent.Logo.KC2,
            FatorX = fatorX ?? parent.Logo.FatorX,
            FatorY = fatorY ?? parent.Logo.FatorY
        };

        commands.Add(cmd);
    }

    /// <summary>
    /// Adicionao o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    public void ImprimirTexto(string aTexto)
    {
        ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, null);
    }

    /// <summary>
    /// Adicionao o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="aAlinhamento"></param>
    public void ImprimirTexto(string aTexto, CmdAlinhamento aAlinhamento)
    {
        ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, aAlinhamento, null);
    }

    /// <summary>
    /// Adicionao o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="tamanho"></param>
    /// <param name="aAlinhamento"></param>
    public void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento)
    {
        ImprimirTexto(aTexto, CmdFonte.Normal, tamanho, aAlinhamento, null);
    }

    /// <summary>
    /// Adicionao o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="tamanho"></param>
    public void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho)
    {
        ImprimirTexto(aTexto, CmdFonte.Normal, tamanho, CmdAlinhamento.Esquerda, null);
    }

    /// <summary>
    /// Adicionao o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="tamanho"></param>
    /// <param name="aEstilo"></param>
    public void ImprimirTexto(string aTexto, CmdTamanhoFonte tamanho, CmdEstiloFonte aEstilo)
    {
        ImprimirTexto(aTexto, CmdFonte.Normal, tamanho, CmdAlinhamento.Esquerda, aEstilo);
    }

    /// <summary>
    /// Adicionao o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="aEstilo"></param>
    public void ImprimirTexto(string aTexto, CmdEstiloFonte aEstilo)
    {
        ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, aEstilo);
    }

    /// <summary>
    /// Adicionao o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="aAlinhamento"></param>
    /// <param name="aEstilo"></param>
    public void ImprimirTexto(string aTexto, CmdAlinhamento aAlinhamento, CmdEstiloFonte aEstilo)
    {
        ImprimirTexto(aTexto, CmdFonte.Normal, CmdTamanhoFonte.Normal, aAlinhamento, aEstilo);
    }

    /// <summary>
    /// Adicionao o comando de impressão de texto ao buffer de impressão da região.
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
    /// Adicionao o comando de impressão de texto ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="fonte"></param>
    /// <param name="tamanho"></param>
    /// <param name="aAlinhamento"></param>
    /// <param name="aEstilo"></param>
    public void ImprimirTexto(string aTexto, CmdFonte fonte, CmdTamanhoFonte tamanho, CmdAlinhamento aAlinhamento, CmdEstiloFonte? aEstilo)
    {
        this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

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
    /// Adiciona o comando de impressão de codigo de barras ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="barcode"></param>
    public void ImprimirBarcode(string aTexto, CmdBarcode barcode)
    {
        ImprimirBarcode(aTexto, barcode, CmdAlinhamento.Esquerda);
    }

    /// <summary>
    /// Adiciona o comando de impressão de codigo de barras ao buffer de impressão da região.
    /// </summary>
    /// <param name="aTexto"></param>
    /// <param name="barcode"></param>
    /// <param name="exibir"></param>
    public void ImprimirBarcode(string aTexto, CmdBarcode barcode, CmdBarcodeText exibir)
    {
        ImprimirBarcode(aTexto, barcode, CmdAlinhamento.Esquerda, exibir);
    }

    /// <summary>
    /// Adiciona o comando de impressão de codigo de barras ao buffer de impressão da região.
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

        var cmd = new BarcodeCommand(interpreter)
        {
            Tipo = barcode,
            Code = aTexto,
            Alinhamento = aAlinhamento,
            Exibir = exibir ?? parent.CodigoBarras.Exibir,
            Altura = altura ?? parent.CodigoBarras.Altura,
            Largura = largura ?? parent.CodigoBarras.Largura
        };

        commands.Add(cmd);
    }

    /// <summary>
    /// Adiciona o comando de impressão de QrCode ao buffer de impressão da região.
    /// </summary>
    /// <param name="texto"></param>
    /// <param name="aAlinhamento"></param>
    public void ImprimirQrCode(string texto, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda)
    {
        ImprimirQrCode(texto, null, 0, null, aAlinhamento);
    }

    /// <summary>
    /// Adiciona o comando de impressão de QrCode ao buffer de impressão da região.
    /// </summary>
    /// <param name="texto"></param>
    /// <param name="tipo"></param>
    /// <param name="tamanho"></param>
    /// <param name="erroLevel"></param>
    /// <param name="aAlinhamento"></param>
    public void ImprimirQrCode(string texto, QrCodeTipo? tipo = null, QrCodeModSize? tamanho = null, QrCodeErrorLevel? erroLevel = null, CmdAlinhamento aAlinhamento = CmdAlinhamento.Esquerda)
    {
        this.Log().Debug($"{MethodBase.GetCurrentMethod()?.Name}");

        var cmd = new QrCodeCommand(interpreter)
        {
            Code = texto,
            Alinhamento = aAlinhamento,
            Tipo = tipo ?? parent.QrCode.Tipo,
            LarguraModulo = tamanho ?? parent.QrCode.Tamanho,
            ErrorLevel = erroLevel ?? parent.QrCode.ErrorLevel,
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

        commands.Add(new EspacoEntreLinhasCommand(interpreter) { Espaco = 16 });

        commands.Add(new ImageCommand(interpreter)
        {
            Imagem = imagem,
            IsHiDPI = isHdpi,
            Alinhamento = aAlinhamento
        });

        commands.Add(new EspacoEntreLinhasCommand(interpreter) { Espaco = EspacoEntreLinhas });
    }

    #endregion Methods
}