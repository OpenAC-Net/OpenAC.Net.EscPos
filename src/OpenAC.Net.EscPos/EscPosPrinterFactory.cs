// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="EscPosPrinterFactory.cs" company="OpenAC .Net">
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
using OpenAC.Net.Core;
using OpenAC.Net.Devices;

namespace OpenAC.Net.EscPos
{
    public static class EscPosPrinterFactory
    {
        public static EscPosPrinter<TConfig> Create<TConfig>(ProtocoloEscPos protocolo, TConfig config) where TConfig : IDeviceConfig
        {
            return new EscPosPrinter<TConfig>(protocolo, config);
        }

        public static EscPosPrinter<SerialConfig> CreateSerial(ProtocoloEscPos protocolo, Action<SerialConfig> config = null)
        {
            var serialConfig = new SerialConfig
            {
                Encoding = OpenEncoding.IBM850
            };
            config?.Invoke(serialConfig);
            return new EscPosPrinter<SerialConfig>(protocolo, serialConfig);
        }

        public static EscPosPrinter<TCPConfig> CreateTCP(ProtocoloEscPos protocolo, Action<TCPConfig> config = null)
        {
            var tcpConfig = new TCPConfig("", 9100)
            {
                Encoding = OpenEncoding.IBM850
            };
            config?.Invoke(tcpConfig);
            return new EscPosPrinter<TCPConfig>(protocolo, tcpConfig);
        }

        public static EscPosPrinter<RawConfig> CreateRaw(ProtocoloEscPos protocolo, Action<RawConfig> config = null)
        {
            var rawConfig = new RawConfig("")
            {
                Encoding = OpenEncoding.IBM850
            };
            config?.Invoke(rawConfig);
            return new EscPosPrinter<RawConfig>(protocolo, rawConfig);
        }
    }
}