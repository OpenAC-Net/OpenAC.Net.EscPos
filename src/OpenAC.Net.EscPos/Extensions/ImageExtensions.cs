using System;
using System.Collections.Generic;
using OpenAC.Net.Devices.Commom;
using SkiaSharp;

namespace OpenAC.Net.EscPos.Extensions;

/// <summary>
/// Métodos de extensão para manipulação de imagens em impressoras ESC/POS.
/// </summary>
public static class ImageExtensions
{
    /// <summary>
    /// Redimensiona uma imagem representada em array de bytes mantendo a proporção dentro dos limites informados.
    /// </summary>
    /// <param name="imageBytes">Bytes da imagem original.</param>
    /// <param name="maxWidth">Largura máxima em pixels.</param>
    /// <param name="maxHeight">Altura máxima em pixels.</param>
    /// <returns>Array de bytes (PNG) da imagem redimensionada.</returns>
    public static byte[] ResizeImage(this byte[] imageBytes, int maxWidth, int maxHeight)
    {
        if (imageBytes == null) throw new ArgumentNullException(nameof(imageBytes));

        using var img = SKBitmap.Decode(imageBytes);
        if (img == null) return imageBytes;

        var ratioX = (double)maxWidth / img.Width;
        var ratioY = (double)maxHeight / img.Height;
        var ratio = Math.Min(ratioX, ratioY);

        var newWidth = Math.Max(1, (int)(img.Width * ratio));
        var newHeight = Math.Max(1, (int)(img.Height * ratio));

        var info = new SKImageInfo(newWidth, newHeight, img.ColorType, img.AlphaType);
        using var resized = img.Resize(info, SKSamplingOptions.Default) ?? img.Copy();
        using var skImage = SKImage.FromBitmap(resized);
        using var data = skImage.Encode(SKEncodedImageFormat.Png, 100);
        return data.ToArray();
    }

    /// <summary>
    /// Divide uma imagem <see cref="SKBitmap"/> em fatias de 24 pixels de altura, convertendo cada fatia em um array de bytes
    /// para impressão em impressoras térmicas ESC/POS. Pixels claros são considerados como fundo e pixels escuros como pontos a serem impressos.
    /// </summary>
    /// <param name="img">A imagem a ser fatiada.</param>
    /// <param name="isHiDpi">Indica se a impressão é em alta densidade (HiDPI).</param>
    /// <returns>Um array de arrays de bytes, cada um representando uma fatia da imagem.</returns>
    internal static byte[][] SliceImage(this SKBitmap img, bool isHiDpi)
    {
        var slices = new List<byte[]>();

        // Cycle picture pixel print
        // High cycle
        for (var i = 0; i < img.Height / 24 + 1; i++)
        {
            using var slice = new ByteArrayBuilder();

            // Width
            for (var j = 0; j < img.Width; j++)
            {
                var data = new[] { (byte)'\x00', (byte)'\x00', (byte)'\x00' };

                for (var k = 0; k < 24; k++)
                    if (i * 24 + k < img.Height) // if within the BMP size
                    {
                        var pixelColor = img.GetPixel(j, i * 24 + k);
                        if (pixelColor.Alpha > 64 && !(pixelColor.Red > 160 && pixelColor.Green > 160 && pixelColor.Blue > 160))
                            data[k / 8] += (byte)(128 >> (k % 8));
                        if (isHiDpi) continue;
                        if (pixelColor.Alpha > 64 && pixelColor.Red == 0)
                            data[k / 8] += (byte)(128 >> (k % 8));
                    }

                // Write data，24dots
                slice.Append(data);
            }

            slices.Add(slice.ToArray());
        }

        return slices.ToArray();
    }
}