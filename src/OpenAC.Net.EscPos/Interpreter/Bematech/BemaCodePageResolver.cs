// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="BemaCodePageResolver.cs" company="OpenAC .Net">
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
using OpenAC.Net.Core.Extensions;
using OpenAC.Net.Devices.Commom;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Bematech
{
    public sealed class BemaCodePageResolver : CommandResolver<CodePageCommand>
    {
        #region Constructors

        public BemaCodePageResolver(IReadOnlyDictionary<CmdEscPos, byte[]> dict) : base(dict)
        {
        }

        #endregion Constructors

        #region Methods

        public override byte[] Resolve(CodePageCommand command)
        {
            if (command.PaginaCodigo.IsIn(PaginaCodigo.pc852, PaginaCodigo.pc1252)) return new byte[0];

            using var builder = new ByteArrayBuilder();

            var codePage = command.PaginaCodigo switch
            {
                PaginaCodigo.pc437 => new byte[] { 3 },
                PaginaCodigo.pc850 => new byte[] { 2 },
                PaginaCodigo.pc860 => new byte[] { 4 },
                PaginaCodigo.pcUTF8 => new byte[] { 8 },
                _ => throw new ArgumentOutOfRangeException()
            };

            builder.Append(new byte[] { CmdConst.ESC, 116 });
            builder.Append(codePage);

            return builder.ToArray();
        }

        #endregion Methods
    }
}