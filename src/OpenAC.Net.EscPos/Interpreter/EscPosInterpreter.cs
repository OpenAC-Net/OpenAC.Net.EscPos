﻿// ***********************************************************************
// Assembly         : OpenAC.Net.EscPos
// Author           : Rafael Dias
// Created          : 17-03-2022
//
// Last Modified By : Rafael Dias
// Last Modified On : 17-03-2022
// ***********************************************************************
// <copyright file="EpsonEscPosEscPosInterpreter.cs" company="OpenAC .Net">
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
using OpenAC.Net.Core.Logging;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter
{
    /// <summary>
    /// Classe base para geração de comandos EscPos.
    /// </summary>
    public abstract class EscPosInterpreter : IOpenLog
    {
        #region Constructors

        /// <summary>
        ///
        /// </summary>
        /// <param name="enconder"></param>
        protected EscPosInterpreter(Encoding enconder)
        {
            Guard.Against<ArgumentNullException>(enconder == null, $"{nameof(enconder)} não pode ser nulo.");

            Enconder = enconder;
            ResolverInitialize();
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        /// Encoding utilizado nos textos para envio a impressora.
        /// </summary>
        public Encoding Enconder { get; }

        public byte[][] StatusCommand => StatusResolver?.StatusCommand ?? new byte[0][];

        /// <summary>
        /// Cache que contem os resolvers dos comandos.
        /// </summary>
        protected ResolverCache CommandResolver { get; } = new();

        protected StatusResolver StatusResolver { get; set; }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Processa o comando e retornar os bytes correspondente.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public byte[] ProcessCommand<TCommand>(TCommand command) where TCommand : PrintCommand<TCommand>
        {
            if (!CommandResolver.HasResolver<TCommand>())
            {
                this.Log().Debug($"[{GetType().Name}] - [{nameof(TCommand)}]: comando não implementado.");
                return new byte[0];
            }

            var resolver = CommandResolver.GetResolver<TCommand>();
            return resolver.Resolve(command);
        }

        /// <summary>
        /// Metodo usado para processar o status retornado pela impressora.
        /// </summary>
        /// <param name="dados"></param>
        /// <returns></returns>
        public EscPosTipoStatus ProcessarStatus(byte[][] dados) => StatusResolver?.Resolve(dados) ?? EscPosTipoStatus.ErroLeitura;

        /// <summary>
        /// Função para inicializar o dicionario de comandos para ser usados no interpreter.
        /// </summary>
        protected abstract void ResolverInitialize();

        #endregion Methods
    }
}