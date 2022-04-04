// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="ResolverCache.cs" company="OpenAC .Net">
//		        		   The MIT License (MIT)
//	     		    Copyright (c) 2014 - 2022 Projeto OpenAC .Net
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
using OpenAC.Net.Core;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter
{
    public sealed class ResolverCache
    {
        #region Fields

        private readonly Dictionary<Type, ICommandResolver> resolverCache;

        #endregion Fields

        #region Constructors

        public ResolverCache()
        {
            resolverCache = new Dictionary<Type, ICommandResolver>();
        }

        #endregion Constructors

        #region Methods

        public bool HasResolver<TCommand>() where TCommand : PrintCommand<TCommand>
        {
            return resolverCache.ContainsKey(typeof(TCommand));
        }

        public void AddResolver<TCommand, TResolver>(TResolver resolver)
            where TCommand : PrintCommand<TCommand>
            where TResolver : CommandResolver<TCommand>
        {
            if (HasResolver<TCommand>()) throw new OpenException("Resolver já cadastrado para este comando.");

            resolverCache.Add(typeof(TCommand), resolver);
        }

        public void RemoveResolver<TCommand>() where TCommand : PrintCommand<TCommand>
        {
            if (!HasResolver<TCommand>()) throw new ResolverException("Resolver não cadastrado para este comando.");

            resolverCache.Remove(typeof(TCommand));
        }

        public ICommandResolver GetResolver<TCommand>() where TCommand : PrintCommand<TCommand>
        {
            return resolverCache[typeof(TCommand)];
        }

        #endregion Methods
    }
}