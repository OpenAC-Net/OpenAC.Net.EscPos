// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="DefaultImageResolver.cs" company="OpenAC .Net">
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
using OpenAC.Net.Devices.Commom;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Interpreter.Resolver
{
    public sealed class DefaultImageResolver : CommandResolver<ImageCommand>
    {
        #region Constructors

        public DefaultImageResolver(IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
        {
        }

        #endregion Constructors

        #region Methods

        public override byte[] Resolve(ImageCommand command)
        {
            if (command.Imagem == null) return new byte[0];

            using var builder = new ByteArrayBuilder();

            switch (command.Alinhamento)
            {
                case CmdAlinhamento.Esquerda when Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda):
                    builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);
                    break;

                case CmdAlinhamento.Centro when Commandos.ContainsKey(CmdEscPos.AlinhadoCentro):
                    builder.Append(Commandos[CmdEscPos.AlinhadoCentro]);
                    break;

                case CmdAlinhamento.Direita when Commandos.ContainsKey(CmdEscPos.AlinhadoDireita):
                    builder.Append(Commandos[CmdEscPos.AlinhadoDireita]);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            var bmp = new Bitmap(command.Imagem);

            // Set character line spacing to n dotlines
            var send = "" + (char)27 + (char)51 + (char)0;
            var data = new byte[send.Length];
            for (var i = 0; i < send.Length; i++) data[i] = (byte)send[i];

            builder.Append(data);
            data[0] = (byte)'\x00';
            data[1] = (byte)'\x00';
            data[2] = (byte)'\x00'; // Clear

            // ESC * m nL nH d1…dk   Select bitmap mode
            byte[] escBmp = { 0x1B, 0x2A, 0x00, 0x00, 0x00 };
            escBmp[2] = (byte)'\x21';
            //nL, nH
            escBmp[3] = (byte)(bmp.Width % 256);
            escBmp[4] = (byte)(bmp.Width / 256);

            // Cycle picture pixel print
            // High cycle
            for (var i = 0; i < bmp.Height / 24 + 1; i++)
            {
                // Set the bitmap mode
                builder.Append(escBmp);

                // Width
                for (var j = 0; j < bmp.Width; j++)
                {
                    for (var k = 0; k < 24; k++)
                        if (i * 24 + k < bmp.Height) // if within the BMP size
                        {
                            var pixelColor = bmp.GetPixel(j, i * 24 + k);
                            if (!(pixelColor.R > 160 && pixelColor.G > 160 && pixelColor.B > 160)) data[k / 8] += (byte)(128 >> (k % 8));
                            if (command.IsHiDPI) continue;
                            if (pixelColor.R == 0) data[k / 8] += (byte)(128 >> (k % 8));
                        }

                    // Write data，24dots
                    builder.Append(data);

                    data[0] = (byte)'\x00';
                    data[1] = (byte)'\x00';
                    data[2] = (byte)'\x00'; // Clear
                }

                byte[] data2 = { 0xA };
                builder.Append(data2);
            }

            builder.Append(new byte[] { 27, (byte)'@' });

            // Volta alinhamento para Esquerda.
            if (Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda) && command.Alinhamento != CmdAlinhamento.Esquerda)
                builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);

            return builder.ToArray();
        }

        #endregion Methods
    }
}