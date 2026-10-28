// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 27-10-2026
//
// Last Modified By : Rafael Dias
// Last Modified On : 27-10-2026
// ***********************************************************************
// <copyright file="EscPosPreviewRenderer.cs" company="OpenAC .Net">
//		        		   The MIT License (MIT)
//	     		    Copyright (c) 2014 - 2026 Projeto OpenAC .Net
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
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter;
using SkiaSharp;

namespace OpenAC.Net.EscPos.Preview;

/// <summary>
/// Renderizador responsável por gerar uma imagem (<see cref="SKBitmap"/>) de pré-visualização (preview) dos comandos ESC/POS.
/// </summary>
internal static class EscPosPreviewRenderer
{
    #region Constants

    private const int DefaultPaperWidthPx = 576; // 80mm @ 203 DPI (~72mm área útil = 576 dots)
    private const int MarginHorizontal = 16;
    private const int MarginVertical = 20;
    private const float BaseLineHeight = 24f;

    // Tabelas EAN-13 / EAN-8 / UPC-A
    private static readonly string[] EanLeftOdd =
    [
        "0001101", "0011001", "0010011", "0111101", "0100011",
        "0110001", "0101111", "0111011", "0110111", "0001011"
    ];

    private static readonly string[] EanLeftEven =
    [
        "0100111", "0110011", "0011011", "0100001", "0011101",
        "0111001", "0000101", "0010001", "0001001", "0010111"
    ];

    private static readonly string[] EanRight =
    [
        "1110010", "1100110", "1101100", "1000010", "1011100",
        "1001110", "1010000", "1000100", "1001000", "1110100"
    ];

    private static readonly string[] Ean13Parity =
    [
        "OOOOOO", "OOEOEE", "OOEEOE", "OOEEEO", "OEOOEE",
        "OEEOOE", "OEEEOO", "OEOEOE", "OEOEEO", "OEEOEO"
    ];

    private static readonly string[] MonospaceFontCandidates =
    [
        "Consolas", "Courier New", "Liberation Mono", "DejaVu Sans Mono", "monospace"
    ];

    #endregion Constants

    #region Methods

    /// <summary>
    /// Gera um array de bytes (PNG) com a pré-visualização dos comandos de impressão fornecidos.
    /// </summary>
    /// <param name="commands">Lista de comandos no buffer.</param>
    /// <param name="interpreter">Interpretador atual da impressora.</param>
    /// <param name="colunas">Quantidade de colunas em fonte normal.</param>
    /// <param name="espacoEntreLinhas">Espaçamento entre linhas padrão.</param>
    /// <returns>Bytes da imagem PNG representando o cupom impresso.</returns>
    public static byte[] Render(
        IReadOnlyList<IPrintCommand> commands,
        EscPosInterpreter interpreter,
        int colunas,
        byte espacoEntreLinhas)
    {
        var printableWidth = DefaultPaperWidthPx;
        var totalWidth = printableWidth + MarginHorizontal * 2;
        var normalCols = Math.Max(1, colunas);

        // Passagem 1: calcula a altura total necessária
        float totalHeight = MarginVertical * 2;
        using (var measureBmp = new SKBitmap(1, 1))
        using (var measureCanvas = new SKCanvas(measureBmp))
        {
            totalHeight = RenderCommands(
                measureCanvas,
                commands,
                interpreter,
                normalCols,
                espacoEntreLinhas,
                MarginHorizontal,
                MarginVertical,
                printableWidth,
                draw: false) + MarginVertical;
        }

        var finalHeight = Math.Max(120, (int)Math.Ceiling(totalHeight));
        using var bmp = new SKBitmap(totalWidth, finalHeight);
        using var canvas = new SKCanvas(bmp);

        // Fundo de bobina térmica (papel levemente creme/branco)
        canvas.Clear(new SKColor(252, 252, 248));

        // Borda lateral sutil e serrilhado superior/inferior de bobina
        DrawReceiptPaperDecorations(canvas, totalWidth, finalHeight);

        // Passagem 2: desenha todos os comandos
        RenderCommands(
            canvas,
            commands,
            interpreter,
            normalCols,
            espacoEntreLinhas,
            MarginHorizontal,
            MarginVertical,
            printableWidth,
            draw: true);

        using var skImage = SKImage.FromBitmap(bmp);
        using var encoded = skImage.Encode(SKEncodedImageFormat.Png, 100);
        return encoded.ToArray();
    }

    private static void DrawReceiptPaperDecorations(SKCanvas canvas, int width, int height)
    {
        using var borderPaint = new SKPaint
        {
            Color = new SKColor(220, 220, 215),
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1f,
            IsAntialias = true
        };
        canvas.DrawRect(0.5f, 0.5f, width - 1f, height - 1f, borderPaint);

        using var serrationPaint = new SKPaint
        {
            Color = new SKColor(210, 210, 205),
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        const int toothWidth = 12;
        const int toothHeight = 6;
        for (var x = 0; x < width; x += toothWidth)
        {
            using var topTooth = new SKPath();
            topTooth.MoveTo(x, 0);
            topTooth.LineTo(x + toothWidth / 2f, toothHeight);
            topTooth.LineTo(x + toothWidth, 0);
            topTooth.Close();
            canvas.DrawPath(topTooth, serrationPaint);

            using var bottomTooth = new SKPath();
            bottomTooth.MoveTo(x, height);
            bottomTooth.LineTo(x + toothWidth / 2f, height - toothHeight);
            bottomTooth.LineTo(x + toothWidth, height);
            bottomTooth.Close();
            canvas.DrawPath(bottomTooth, serrationPaint);
        }
    }

    private static float RenderCommands(
        SKCanvas canvas,
        IEnumerable<IPrintCommand> commands,
        EscPosInterpreter interpreter,
        int normalCols,
        byte defaultLineSpace,
        float offsetX,
        float startY,
        float areaWidth,
        bool draw)
    {
        var currentY = startY;
        var currentLineSpace = defaultLineSpace;
        var razao = interpreter?.RazaoColuna ?? new RazaoColunaFonte();

        foreach (var command in commands)
        {
            switch (command)
            {
                case ZeraCommand:
                    currentLineSpace = defaultLineSpace;
                    break;

                case EspacoEntreLinhasCommand espacoCmd:
                    currentLineSpace = espacoCmd.Espaco;
                    break;

                case JumpLineCommand jumpCmd:
                {
                    var linhas = Math.Max(1, jumpCmd.Linhas);
                    var lineAdvance = GetLineAdvance(BaseLineHeight, currentLineSpace);
                    currentY += linhas * lineAdvance;
                    break;
                }

                case PrintLineCommand lineCmd:
                {
                    var ch = lineCmd.Dupla ? '=' : '-';
                    var text = new string(ch, normalCols);
                    var textCmd = new TextCommand(interpreter!)
                    {
                        Texto = text,
                        Fonte = CmdFonte.Normal,
                        Tamanho = CmdTamanhoFonte.Normal,
                        Alinhamento = CmdAlinhamento.Esquerda
                    };
                    currentY = RenderTextCommand(canvas, textCmd, normalCols, razao, currentLineSpace, offsetX, currentY, areaWidth, draw);
                    break;
                }

                case TextCommand textCmd:
                    currentY = RenderTextCommand(canvas, textCmd, normalCols, razao, currentLineSpace, offsetX, currentY, areaWidth, draw);
                    break;

                case TextSliceCommand sliceCmd:
                    currentY = RenderTextSliceCommand(canvas, sliceCmd, normalCols, razao, currentLineSpace, offsetX, currentY, areaWidth, draw);
                    break;

                case ImageCommand imgCmd:
                    currentY = RenderImageCommand(canvas, imgCmd, offsetX, currentY, areaWidth, draw);
                    break;

                case LogoCommand logoCmd:
                    currentY = RenderLogoCommand(canvas, logoCmd, offsetX, currentY, areaWidth, draw);
                    break;

                case BarcodeCommand barcodeCmd:
                    currentY = RenderBarcodeCommand(canvas, barcodeCmd, offsetX, currentY, areaWidth, draw);
                    break;

                case QrCodeCommand qrCmd:
                    currentY = RenderQrCodeCommand(canvas, qrCmd, offsetX, currentY, areaWidth, draw);
                    break;

                case CutCommand cutCmd:
                    currentY = RenderCutCommand(canvas, cutCmd, offsetX, currentY, areaWidth, draw);
                    break;

                case ModoPaginaCommand pageCmd:
                    currentY = RenderModoPaginaCommand(canvas, pageCmd, interpreter!, normalCols, offsetX, currentY, areaWidth, draw);
                    break;
            }
        }

        return currentY;
    }

    private static float GetLineAdvance(float fontHeight, byte espacoEntreLinhas)
    {
        if (espacoEntreLinhas == 0)
            return fontHeight + 2f;

        return Math.Max(fontHeight * 0.75f, espacoEntreLinhas);
    }

    private static int GetEffectiveColumns(int baseColunas, RazaoColunaFonte razao, CmdFonte fonte, CmdTamanhoFonte tamanho)
    {
        var cols = (decimal)Math.Max(1, baseColunas);
        if (fonte == CmdFonte.FonteB)
            cols /= 0.75M;

        if (tamanho == CmdTamanhoFonte.Condensada && razao.Condensada > 0)
            cols /= razao.Condensada;
        else if (tamanho == CmdTamanhoFonte.Expandida && razao.Expandida > 0)
            cols /= razao.Expandida;

        return Math.Max(1, (int)Math.Truncate(cols));
    }

    private static SKFont CreatePrinterFont(float pixelSize, CmdEstiloFonte? estilo)
    {
        var weight = estilo.HasValue && estilo.Value.HasFlag(CmdEstiloFonte.Negrito)
            ? SKFontStyleWeight.Bold
            : SKFontStyleWeight.Normal;
        var slant = estilo.HasValue && estilo.Value.HasFlag(CmdEstiloFonte.Italico)
            ? SKFontStyleSlant.Italic
            : SKFontStyleSlant.Upright;

        SKTypeface? typeface = null;
        foreach (var candidate in MonospaceFontCandidates)
        {
            typeface = SKTypeface.FromFamilyName(candidate, weight, SKFontStyleWidth.Normal, slant);
            if (typeface != null)
                break;
        }

        typeface ??= SKTypeface.Default;
        return new SKFont(typeface, pixelSize)
        {
            Subpixel = true,
            Edging = SKFontEdging.Antialias
        };
    }

    private static List<string> SplitAndWrapText(string? rawText, int maxColumns)
    {
        var result = new List<string>();
        if (string.IsNullOrEmpty(rawText))
        {
            result.Add(string.Empty);
            return result;
        }

        var normalized = rawText!.Replace("\r\n", "\n").Replace("\r", "\n");
        var paragraphs = normalized.Split('\n');
        var width = Math.Max(1, maxColumns);

        foreach (var paragraph in paragraphs)
        {
            if (paragraph.Length <= width)
            {
                result.Add(paragraph);
                continue;
            }

            for (var idx = 0; idx < paragraph.Length; idx += width)
            {
                var len = Math.Min(width, paragraph.Length - idx);
                result.Add(paragraph.Substring(idx, len));
            }
        }

        return result;
    }

    private static float RenderTextCommand(
        SKCanvas canvas,
        TextCommand cmd,
        int normalCols,
        RazaoColunaFonte razao,
        byte espacoEntreLinhas,
        float offsetX,
        float currentY,
        float areaWidth,
        bool draw)
    {
        var effectiveCols = GetEffectiveColumns(normalCols, razao, cmd.Fonte, cmd.Tamanho);
        var lines = SplitAndWrapText(cmd.Texto, effectiveCols);

        var scaleY = cmd.Estilo.HasValue && cmd.Estilo.Value.HasFlag(CmdEstiloFonte.AlturaDupla) ? 2.0f : 1.0f;
        var baseFontPx = cmd.Fonte == CmdFonte.FonteB ? 15f : 18f;
        var lineBoxHeight = baseFontPx * scaleY + 4f;
        var lineAdvance = GetLineAdvance(lineBoxHeight, espacoEntreLinhas);

        if (!draw)
            return currentY + lines.Count * lineAdvance;

        using var font = CreatePrinterFont(baseFontPx, cmd.Estilo);
        var singleCharWidth = Math.Max(1f, font.MeasureText("M"));
        var targetCharWidth = areaWidth / effectiveCols;
        var scaleX = targetCharWidth / singleCharWidth;
        var isInverted = cmd.Estilo.HasValue && cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Invertido);
        var isUnderline = cmd.Estilo.HasValue && cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Sublinhado);

        using var textPaint = new SKPaint
        {
            Color = isInverted ? SKColors.White : SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };
        using var bgPaint = new SKPaint
        {
            Color = SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };
        using var underlinePaint = new SKPaint
        {
            Color = isInverted ? SKColors.White : SKColors.Black,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1.2f
        };

        foreach (var line in lines)
        {
            if (!string.IsNullOrEmpty(line))
            {
                var unscaledWidth = font.MeasureText(line);
                var renderedWidth = Math.Min(areaWidth, unscaledWidth * scaleX);

                var drawX = cmd.Alinhamento switch
                {
                    CmdAlinhamento.Centro => offsetX + (areaWidth - renderedWidth) / 2f,
                    CmdAlinhamento.Direita => offsetX + areaWidth - renderedWidth,
                    _ => offsetX
                };

                canvas.Save();
                canvas.Translate(drawX, currentY);
                canvas.Scale(scaleX, scaleY);

                if (isInverted)
                    canvas.DrawRect(0, 0, unscaledWidth, baseFontPx + 2f, bgPaint);

                var baselineY = -font.Metrics.Ascent;
                canvas.DrawText(line, 0, baselineY, SKTextAlign.Left, font, textPaint);

                if (isUnderline)
                    canvas.DrawLine(0, baselineY + 2f, unscaledWidth, baselineY + 2f, underlinePaint);

                canvas.Restore();
            }

            currentY += lineAdvance;
        }

        return currentY;
    }

    private static float RenderTextSliceCommand(
        SKCanvas canvas,
        TextSliceCommand cmd,
        int normalCols,
        RazaoColunaFonte razao,
        byte espacoEntreLinhas,
        float offsetX,
        float currentY,
        float areaWidth,
        bool draw)
    {
        if (cmd.Slices == null || cmd.Slices.Count == 0)
            return currentY;

        var effectiveCols = GetEffectiveColumns(normalCols, razao, cmd.Fonte, cmd.Tamanho);
        var hasDoubleHeight = cmd.Slices.Any(s => s.Estilo.HasValue && s.Estilo.Value.HasFlag(CmdEstiloFonte.AlturaDupla));
        var maxScaleY = hasDoubleHeight ? 2.0f : 1.0f;
        var baseFontPx = cmd.Fonte == CmdFonte.FonteB ? 15f : 18f;
        var lineBoxHeight = baseFontPx * maxScaleY + 4f;
        var lineAdvance = GetLineAdvance(lineBoxHeight, espacoEntreLinhas);

        if (!draw)
            return currentY + lineAdvance;

        using var refFont = CreatePrinterFont(baseFontPx, null);
        var singleCharWidth = Math.Max(1f, refFont.MeasureText("M"));
        var targetCharWidth = areaWidth / effectiveCols;
        var scaleX = targetCharWidth / singleCharWidth;

        float totalRenderedWidth = 0f;
        var sliceMetrics = new List<SliceMetric>(cmd.Slices.Count);

        try
        {
            foreach (var slice in cmd.Slices)
            {
                var text = slice.Texto ?? string.Empty;
                var f = CreatePrinterFont(baseFontPx, slice.Estilo);
                var uw = string.IsNullOrEmpty(text) ? 0f : f.MeasureText(text);
                var rw = uw * scaleX;
                var sy = slice.Estilo.HasValue && slice.Estilo.Value.HasFlag(CmdEstiloFonte.AlturaDupla) ? 2.0f : 1.0f;
                sliceMetrics.Add(new SliceMetric(slice, f, uw, rw, sy));
                totalRenderedWidth += rw;
            }

            var cursorX = cmd.Alinhamento switch
            {
                CmdAlinhamento.Centro => offsetX + Math.Max(0f, (areaWidth - totalRenderedWidth) / 2f),
                CmdAlinhamento.Direita => offsetX + Math.Max(0f, areaWidth - totalRenderedWidth),
                _ => offsetX
            };

            using var bgPaint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Fill, IsAntialias = true };

            foreach (var item in sliceMetrics)
            {
                var slice = item.Slice;
                var font = item.Font;
                var uw = item.UnscaledWidth;
                var rw = item.RenderedWidth;
                var sy = item.ScaleY;

                if (!string.IsNullOrEmpty(slice.Texto) && uw > 0)
                {
                    var isInverted = slice.Estilo.HasValue && slice.Estilo.Value.HasFlag(CmdEstiloFonte.Invertido);
                    var isUnderline = slice.Estilo.HasValue && slice.Estilo.Value.HasFlag(CmdEstiloFonte.Sublinhado);

                    using var textPaint = new SKPaint
                    {
                        Color = isInverted ? SKColors.White : SKColors.Black,
                        IsAntialias = true,
                        Style = SKPaintStyle.Fill
                    };
                    using var underlinePaint = new SKPaint
                    {
                        Color = isInverted ? SKColors.White : SKColors.Black,
                        IsAntialias = true,
                        Style = SKPaintStyle.Stroke,
                        StrokeWidth = 1.2f
                    };

                    var sliceTop = currentY + (maxScaleY - sy) * baseFontPx;
                    canvas.Save();
                    canvas.Translate(cursorX, sliceTop);
                    canvas.Scale(scaleX, sy);

                    if (isInverted)
                        canvas.DrawRect(0, 0, uw, baseFontPx + 2f, bgPaint);

                    var baselineY = -font.Metrics.Ascent;
                    canvas.DrawText(slice.Texto!, 0, baselineY, SKTextAlign.Left, font, textPaint);

                    if (isUnderline)
                        canvas.DrawLine(0, baselineY + 2f, uw, baselineY + 2f, underlinePaint);

                    canvas.Restore();
                    cursorX += rw;
                }
            }
        }
        finally
        {
            foreach (var item in sliceMetrics)
                item.Font.Dispose();
        }

        return currentY + lineAdvance;
    }

    private readonly struct SliceMetric
    {
        public SliceMetric(TextSlice slice, SKFont font, float unscaledWidth, float renderedWidth, float scaleY)
        {
            Slice = slice;
            Font = font;
            UnscaledWidth = unscaledWidth;
            RenderedWidth = renderedWidth;
            ScaleY = scaleY;
        }

        public TextSlice Slice { get; }
        public SKFont Font { get; }
        public float UnscaledWidth { get; }
        public float RenderedWidth { get; }
        public float ScaleY { get; }
    }

    private static float RenderImageCommand(
        SKCanvas canvas,
        ImageCommand cmd,
        float offsetX,
        float currentY,
        float areaWidth,
        bool draw)
    {
        if (cmd.Imagem == null || cmd.Imagem.Length == 0)
            return currentY;

        using var srcBmp = SKBitmap.Decode(cmd.Imagem);
        if (srcBmp == null)
            return currentY;

        var imgWidth = Math.Min((float)srcBmp.Width, areaWidth);
        var scale = imgWidth / Math.Max(1, srcBmp.Width);
        var imgHeight = srcBmp.Height * scale;

        if (!draw)
            return currentY + imgHeight + 8f;

        var drawX = cmd.Alinhamento switch
        {
            CmdAlinhamento.Centro => offsetX + (areaWidth - imgWidth) / 2f,
            CmdAlinhamento.Direita => offsetX + areaWidth - imgWidth,
            _ => offsetX
        };

        using var mono = ConvertToThermalMonochrome(srcBmp);
        var destRect = new SKRect(drawX, currentY + 4f, drawX + imgWidth, currentY + 4f + imgHeight);
        canvas.DrawBitmap(mono, destRect);

        return currentY + imgHeight + 8f;
    }

    private static SKBitmap ConvertToThermalMonochrome(SKBitmap src)
    {
        var dst = new SKBitmap(src.Width, src.Height);
        for (var y = 0; y < src.Height; y++)
        {
            for (var x = 0; x < src.Width; x++)
            {
                var c = src.GetPixel(x, y);
                if (c.Alpha < 64 || c.Red > 160 && c.Green > 160 && c.Blue > 160)
                    dst.SetPixel(x, y, SKColors.Transparent);
                else
                    dst.SetPixel(x, y, new SKColor(20, 20, 20));
            }
        }

        return dst;
    }

    private static float RenderLogoCommand(
        SKCanvas canvas,
        LogoCommand cmd,
        float offsetX,
        float currentY,
        float areaWidth,
        bool draw)
    {
        var boxHeight = 40f * Math.Max((byte)1, cmd.FatorY);
        var boxWidth = Math.Min(areaWidth, 160f * Math.Max((byte)1, cmd.FatorX));

        if (!draw)
            return currentY + boxHeight + 8f;

        var drawX = offsetX + (areaWidth - boxWidth) / 2f;
        var rect = new SKRect(drawX, currentY + 4f, drawX + boxWidth, currentY + 4f + boxHeight);

        using var pen = new SKPaint
        {
            Color = new SKColor(120, 120, 120),
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1f,
            PathEffect = SKPathEffect.CreateDash([4f, 2f], 0),
            IsAntialias = true
        };
        canvas.DrawRect(rect, pen);

        using var font = CreatePrinterFont(12f, CmdEstiloFonte.Negrito);
        using var textPaint = new SKPaint
        {
            Color = new SKColor(80, 80, 80),
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };
        var label = $"[ LOGO {cmd.KC1}:{cmd.KC2} ]";
        var textWidth = font.MeasureText(label);
        var textX = drawX + (boxWidth - textWidth) / 2f;
        var textY = currentY + 4f + boxHeight / 2f - (font.Metrics.Ascent + font.Metrics.Descent) / 2f;
        canvas.DrawText(label, textX, textY, SKTextAlign.Left, font, textPaint);

        return currentY + boxHeight + 8f;
    }

    private static float RenderBarcodeCommand(
        SKCanvas canvas,
        BarcodeCommand cmd,
        float offsetX,
        float currentY,
        float areaWidth,
        bool draw)
    {
        if (string.IsNullOrEmpty(cmd.Code))
            return currentY;

        var barHeight = cmd.Altura > 0 ? Math.Min(160, cmd.Altura) : 50;
        var moduleWidth = cmd.Largura > 0 ? Math.Min(6, Math.Max(1, cmd.Largura)) : 2;
        var showAbove = cmd.Exibir is CmdBarcodeText.Acima or CmdBarcodeText.Ambos;
        var showBelow = cmd.Exibir is CmdBarcodeText.Abaixo or CmdBarcodeText.Ambos;
        const float textRowHeight = 16f;

        var totalBlockHeight = barHeight
                               + (showAbove ? textRowHeight : 0f)
                               + (showBelow ? textRowHeight : 0f)
                               + 10f;

        if (!draw)
            return currentY + totalBlockHeight;

        var pattern = EncodeBarcodeModules(cmd.Tipo, cmd.Code!);
        var barcodeWidth = Math.Min(areaWidth, pattern.Length * moduleWidth);
        var scaleModule = barcodeWidth / Math.Max(1, pattern.Length);

        var startX = cmd.Alinhamento switch
        {
            CmdAlinhamento.Centro => offsetX + (areaWidth - barcodeWidth) / 2f,
            CmdAlinhamento.Direita => offsetX + areaWidth - barcodeWidth,
            _ => offsetX
        };

        var y = currentY + 4f;
        using var font = CreatePrinterFont(12f, null);
        using var blackPaint = new SKPaint
        {
            Color = SKColors.Black,
            Style = SKPaintStyle.Fill,
            IsAntialias = true
        };

        if (showAbove)
        {
            var tw = font.MeasureText(cmd.Code!);
            var tx = startX + (barcodeWidth - tw) / 2f;
            canvas.DrawText(cmd.Code!, tx, y - font.Metrics.Ascent, SKTextAlign.Left, font, blackPaint);
            y += textRowHeight;
        }

        for (var i = 0; i < pattern.Length; i++)
        {
            if (pattern[i] == '1')
            {
                var bx = startX + i * scaleModule;
                canvas.DrawRect(bx, y, Math.Max(1f, scaleModule), barHeight, blackPaint);
            }
        }

        y += barHeight + 2f;

        if (showBelow)
        {
            var tw = font.MeasureText(cmd.Code!);
            var tx = startX + (barcodeWidth - tw) / 2f;
            canvas.DrawText(cmd.Code!, tx, y - font.Metrics.Ascent, SKTextAlign.Left, font, blackPaint);
        }

        return currentY + totalBlockHeight;
    }

    private static string EncodeBarcodeModules(CmdBarcode tipo, string code)
    {
        var digits = new string(code.Where(char.IsDigit).ToArray());

        if (tipo == CmdBarcode.EAN13 && digits.Length >= 12)
        {
            var d = digits.PadRight(13, '0');
            var first = d[0] - '0';
            var parity = Ean13Parity[first];
            var sb = new System.Text.StringBuilder("101");
            for (var i = 0; i < 6; i++)
            {
                var digit = d[i + 1] - '0';
                sb.Append(parity[i] == 'O' ? EanLeftOdd[digit] : EanLeftEven[digit]);
            }

            sb.Append("01010");
            for (var i = 7; i < 13; i++)
            {
                var digit = d[i] - '0';
                sb.Append(EanRight[digit]);
            }

            sb.Append("101");
            return sb.ToString();
        }

        if (tipo == CmdBarcode.EAN8 && digits.Length >= 7)
        {
            var d = digits.PadRight(8, '0');
            var sb = new System.Text.StringBuilder("101");
            for (var i = 0; i < 4; i++)
                sb.Append(EanLeftOdd[d[i] - '0']);
            sb.Append("01010");
            for (var i = 4; i < 8; i++)
                sb.Append(EanRight[d[i] - '0']);
            sb.Append("101");
            return sb.ToString();
        }

        if (tipo == CmdBarcode.UPCA && digits.Length >= 11)
        {
            var d = digits.PadRight(12, '0');
            var sb = new System.Text.StringBuilder("101");
            for (var i = 0; i < 6; i++)
                sb.Append(EanLeftOdd[d[i] - '0']);
            sb.Append("01010");
            for (var i = 6; i < 12; i++)
                sb.Append(EanRight[d[i] - '0']);
            sb.Append("101");
            return sb.ToString();
        }

        // Padrão genérico determinístico estilo Code 128 (11 módulos por caractere + start/stop)
        var builder = new System.Text.StringBuilder("11010010000");
        foreach (var ch in code)
        {
            var v = (int)ch;
            builder.Append("1");
            builder.Append((v & 1) != 0 ? "10" : "01");
            builder.Append((v & 2) != 0 ? "100" : "011");
            builder.Append((v & 4) != 0 ? "10" : "01");
            builder.Append((v & 8) != 0 ? "100" : "010");
        }

        builder.Append("1100011101011");
        return builder.ToString();
    }

    private static float RenderQrCodeCommand(
        SKCanvas canvas,
        QrCodeCommand cmd,
        float offsetX,
        float currentY,
        float areaWidth,
        bool draw)
    {
        if (string.IsNullOrEmpty(cmd.Code))
            return currentY;

        var matrixSize = cmd.Code!.Length switch
        {
            <= 25 => 21,
            <= 60 => 25,
            <= 120 => 29,
            _ => 33
        };

        var modulePx = cmd.LarguraModulo switch
        {
            QrCodeModSize.Minusculo => 3,
            QrCodeModSize.Pequeno => 4,
            QrCodeModSize.Normal => 5,
            QrCodeModSize.Grande => 6,
            QrCodeModSize.ExtraGrande => 8,
            _ => 5
        };

        var qrSizePx = Math.Min(areaWidth, (matrixSize + 2) * modulePx);
        var cellPx = qrSizePx / (matrixSize + 2f);

        if (!draw)
            return currentY + qrSizePx + 10f;

        var startX = cmd.Alinhamento switch
        {
            CmdAlinhamento.Centro => offsetX + (areaWidth - qrSizePx) / 2f,
            CmdAlinhamento.Direita => offsetX + areaWidth - qrSizePx,
            _ => offsetX
        };

        var startY = currentY + 5f;
        var matrix = BuildQrPreviewMatrix(cmd.Code, matrixSize);

        using var whitePaint = new SKPaint { Color = SKColors.White, Style = SKPaintStyle.Fill };
        using var blackPaint = new SKPaint { Color = SKColors.Black, Style = SKPaintStyle.Fill };

        canvas.DrawRect(startX, startY, qrSizePx, qrSizePx, whitePaint);

        for (var r = 0; r < matrixSize; r++)
        {
            for (var c = 0; c < matrixSize; c++)
            {
                if (matrix[r, c])
                {
                    var x = startX + (c + 1) * cellPx;
                    var y = startY + (r + 1) * cellPx;
                    canvas.DrawRect(x, y, (float)Math.Ceiling(cellPx), (float)Math.Ceiling(cellPx), blackPaint);
                }
            }
        }

        return currentY + qrSizePx + 10f;
    }

    private static bool[,] BuildQrPreviewMatrix(string data, int size)
    {
        var m = new bool[size, size];
        var reserved = new bool[size, size];

        void PlaceFinder(int rowOffset, int colOffset)
        {
            for (var r = -1; r <= 7; r++)
            {
                for (var c = -1; c <= 7; c++)
                {
                    var rr = rowOffset + r;
                    var cc = colOffset + c;
                    if (rr < 0 || rr >= size || cc < 0 || cc >= size) continue;

                    reserved[rr, cc] = true;
                    var inOuter = r is >= 0 and <= 6 && c is >= 0 and <= 6 && (r is 0 or 6 || c is 0 or 6);
                    var inInner = r is >= 2 and <= 4 && c is >= 2 and <= 4;
                    m[rr, cc] = inOuter || inInner;
                }
            }
        }

        PlaceFinder(0, 0);
        PlaceFinder(0, size - 7);
        PlaceFinder(size - 7, 0);

        // Timing patterns
        for (var i = 8; i < size - 8; i++)
        {
            reserved[6, i] = true;
            m[6, i] = i % 2 == 0;
            reserved[i, 6] = true;
            m[i, 6] = i % 2 == 0;
        }

        // Alignment pattern para versões >= 2 (size >= 25)
        if (size >= 25)
        {
            var ar = size - 9;
            var ac = size - 9;
            for (var r = -2; r <= 2; r++)
            {
                for (var c = -2; c <= 2; c++)
                {
                    reserved[ar + r, ac + c] = true;
                    m[ar + r, ac + c] = Math.Abs(r) == 2 || Math.Abs(c) == 2 || (r == 0 && c == 0);
                }
            }
        }

        // Preenche os módulos de dados de forma determinística a partir do conteúdo do QR Code
        var hash = 2166136261u;
        foreach (var ch in data)
            hash = (hash ^ ch) * 16777619u;

        var idx = 0;
        for (var r = 0; r < size; r++)
        {
            for (var c = 0; c < size; c++)
            {
                if (reserved[r, c]) continue;
                var ch = data[idx % data.Length];
                var mix = (uint)(ch * 31 + r * 17 + c * 13 + idx * 7) ^ hash;
                m[r, c] = ((mix >> (idx % 16)) & 1) == 1;
                idx++;
            }
        }

        return m;
    }

    private static float RenderCutCommand(
        SKCanvas canvas,
        CutCommand cmd,
        float offsetX,
        float currentY,
        float areaWidth,
        bool draw)
    {
        const float cutHeight = 26f;
        if (!draw)
            return currentY + cutHeight;

        var y = currentY + cutHeight / 2f;
        using var dashPaint = new SKPaint
        {
            Color = new SKColor(150, 150, 150),
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1.5f,
            PathEffect = SKPathEffect.CreateDash(cmd.Parcial ? [6f, 6f] : [10f, 3f], 0),
            IsAntialias = true
        };
        canvas.DrawLine(offsetX, y, offsetX + areaWidth, y, dashPaint);

        var label = cmd.Parcial ? "✂ CORTE PARCIAL" : "✂ CORTE TOTAL";
        using var font = CreatePrinterFont(10f, CmdEstiloFonte.Negrito);
        var labelWidth = font.MeasureText(label);
        var lx = offsetX + (areaWidth - labelWidth) / 2f;

        using var bgPaint = new SKPaint
        {
            Color = new SKColor(252, 252, 248),
            Style = SKPaintStyle.Fill
        };
        canvas.DrawRect(lx - 6f, currentY + 4f, labelWidth + 12f, cutHeight - 8f, bgPaint);

        using var textPaint = new SKPaint
        {
            Color = new SKColor(120, 120, 120),
            IsAntialias = true,
            Style = SKPaintStyle.Fill
        };
        var textY = y - (font.Metrics.Ascent + font.Metrics.Descent) / 2f;
        canvas.DrawText(label, lx, textY, SKTextAlign.Left, font, textPaint);

        return currentY + cutHeight;
    }

    private static float RenderModoPaginaCommand(
        SKCanvas canvas,
        ModoPaginaCommand pageCmd,
        EscPosInterpreter interpreter,
        int normalCols,
        float offsetX,
        float currentY,
        float areaWidth,
        bool draw)
    {
        if (pageCmd.Regioes == null || pageCmd.Regioes.Count == 0)
            return currentY;

        float maxRegionBottom = 0f;
        foreach (var regiao in pageCmd.Regioes)
        {
            var regionBottom = regiao.Topo + Math.Max(40, regiao.Altura);
            if (regionBottom > maxRegionBottom)
                maxRegionBottom = regionBottom;
        }

        if (!draw)
            return currentY + maxRegionBottom + 8f;

        using var borderPen = new SKPaint
        {
            Color = new SKColor(200, 200, 195),
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 1f,
            PathEffect = SKPathEffect.CreateDash([3f, 3f], 0),
            IsAntialias = true
        };

        foreach (var regiao in pageCmd.Regioes)
        {
            var regX = offsetX + Math.Max(0, regiao.Esquerda);
            var regY = currentY + Math.Max(0, regiao.Topo);
            var regW = regiao.Largura > 0 ? Math.Min(areaWidth - regiao.Esquerda, regiao.Largura) : areaWidth - regiao.Esquerda;
            var regH = regiao.Altura > 0 ? regiao.Altura : 200f;

            if (regW <= 10 || regH <= 10) continue;

            var rect = new SKRect(regX, regY, regX + regW, regY + regH);
            canvas.DrawRect(rect, borderPen);

            canvas.Save();
            canvas.ClipRect(rect);

            switch (regiao.Direcao)
            {
                case CmdPosDirecao.BaixoParaTopo:
                    canvas.Translate(regX, regY + regH);
                    canvas.RotateDegrees(-90f);
                    RenderCommands(canvas, regiao.Commands, interpreter, normalCols, regiao.EspacoEntreLinhas, 0f, 0f, regH, draw: true);
                    break;

                case CmdPosDirecao.TopoParaBaixo:
                    canvas.Translate(regX + regW, regY);
                    canvas.RotateDegrees(90f);
                    RenderCommands(canvas, regiao.Commands, interpreter, normalCols, regiao.EspacoEntreLinhas, 0f, 0f, regH, draw: true);
                    break;

                case CmdPosDirecao.DireitaParaEsquerda:
                    canvas.Translate(regX + regW, regY + regH);
                    canvas.RotateDegrees(180f);
                    RenderCommands(canvas, regiao.Commands, interpreter, normalCols, regiao.EspacoEntreLinhas, 0f, 0f, regW, draw: true);
                    break;

                default:
                    RenderCommands(canvas, regiao.Commands, interpreter, normalCols, regiao.EspacoEntreLinhas, regX, regY, regW, draw: true);
                    break;
            }

            canvas.Restore();
        }

        return currentY + maxRegionBottom + 8f;
    }

    #endregion Methods
}
