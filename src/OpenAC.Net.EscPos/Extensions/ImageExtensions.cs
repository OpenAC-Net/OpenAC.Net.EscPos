using System.Collections.Generic;
using System.Drawing;
using OpenAC.Net.Devices.Commom;

namespace OpenAC.Net.EscPos.Extensions;

/// <summary>
/// Métodos de extensão para manipulação de imagens em impressoras ESC/POS.
/// </summary>
public static class ImageExtensions
{
    /// <summary>
    /// Divide uma imagem <see cref="Bitmap"/> em fatias de 24 pixels de altura, convertendo cada fatia em um array de bytes
    /// para impressão em impressoras térmicas ESC/POS. Pixels claros são considerados como fundo e pixels escuros como pontos a serem impressos.
    /// </summary>
    /// <param name="img">A imagem a ser fatiada.</param>
    /// <param name="isHiDpi">Indica se a impressão é em alta densidade (HiDPI).</param>
    /// <returns>Um array de arrays de bytes, cada um representando uma fatia da imagem.</returns>
    internal static byte[][] SliceImage(this Bitmap img, bool isHiDpi)
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
                        if (!(pixelColor.R > 160 && pixelColor.G > 160 && pixelColor.B > 160)) data[k / 8] += (byte)(128 >> (k % 8));
                        if (isHiDpi) continue;
                        if (pixelColor.R == 0) data[k / 8] += (byte)(128 >> (k % 8));
                    }

                // Write data，24dots
                slice.Append(data);
            }

            slices.Add(slice.ToArray());
        }

        return slices.ToArray();
    }
}