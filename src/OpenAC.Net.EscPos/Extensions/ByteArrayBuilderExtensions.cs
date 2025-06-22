// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="ByteArrayBuilderExtensions.cs" company="OpenAC .Net">
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

using System.Collections.Generic;
using OpenAC.Net.Devices.Commom;

namespace OpenAC.Net.EscPos.Extensions;

/// <summary>
/// Métodos de extensão para <see cref="ByteArrayBuilder"/>.
/// </summary>
internal static class ByteArrayBuilderExtensions
{
    /// <summary>
    /// Adiciona itens ao <see cref="ByteArrayBuilder"/>. Aceita bytes individuais ou coleções de bytes.
    /// Retorna <c>true</c> se todos os itens foram adicionados com sucesso, <c>false</c> caso contrário.
    /// </summary>
    /// <param name="list">A instância de <see cref="ByteArrayBuilder"/>.</param>
    /// <param name="items">Itens a serem adicionados (byte ou IEnumerable&lt;byte&gt;).</param>
    /// <returns><c>true</c> se todos os itens foram adicionados; caso contrário, <c>false</c>.</returns>
    public static bool Append(this ByteArrayBuilder list, params object[] items)
    {
        var ignoredItems = false;
        foreach (var item in items)
        {
            switch (item)
            {
                case byte itemT:
                    list.Append(itemT);
                    break;

                case IEnumerable<byte> arrayT:
                    list.Append(arrayT);
                    break;

                default:
                    ignoredItems = true;
                    break;
            }
        }

        return !ignoredItems;
    }
}