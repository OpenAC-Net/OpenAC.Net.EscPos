﻿// ***********************************************************************
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
using OpenAC.Net.Devices;

namespace OpenAC.Net.EscPos;

/// <summary>
/// Representa uma impressora EscPos genérica com configuração fortemente tipada.
/// </summary>
/// <typeparam name="TConfig">Tipo da configuração do dispositivo, que implementa <see cref="IDeviceConfig"/>.</typeparam>

public sealed class EscPosPrinter<TConfig> : EscPosPrinter where TConfig : IDeviceConfig
{
    #region Constructors
    
    /// <summary>
    /// Inicializa uma nova instância de <see cref="EscPosPrinter{TConfig}"/> com uma configuração padrão.
    /// </summary>
    public EscPosPrinter() : base(Activator.CreateInstance<TConfig>())
    {
    }
    
    /// <summary>
    /// Inicializa uma nova instância de <see cref="EscPosPrinter{TConfig}"/> com a configuração informada.
    /// </summary>
    /// <param name="device">Configuração do dispositivo.</param>
    public EscPosPrinter(TConfig device) : base(device)
    {
    }

    #endregion Constructors

    #region properties

    /// <summary>
    /// Obtém a configuração do dispositivo fortemente tipada.
    /// </summary>
    public new TConfig Device => (TConfig)base.Device;

    #endregion properties
}