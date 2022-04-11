using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using OpenAC.Net.Devices.Commom;

namespace OpenAC.Net.EscPos.Extensions;

public static class ImageExtensions
{
    /// <summary>
    /// Resize the image to the specified width and height.
    /// </summary>
    /// <param name="image">The image to resize.</param>
    /// <param name="width">The width to resize to.</param>
    /// <param name="height">The height to resize to.</param>
    /// <returns>The resized image.</returns>
    public static Image ResizeImage(this Image image, int width, int height)
    {
        var destRect = new Rectangle(0, 0, width, height);
        var destImage = new Bitmap(width, height);

        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

        using var graphics = Graphics.FromImage(destImage);
        graphics.CompositingMode = CompositingMode.SourceCopy;
        graphics.CompositingQuality = CompositingQuality.HighQuality;
        graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
        graphics.SmoothingMode = SmoothingMode.HighQuality;
        graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

        using var wrapMode = new ImageAttributes();
        wrapMode.SetWrapMode(WrapMode.TileFlipXY);
        graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);

        return destImage;
    }

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