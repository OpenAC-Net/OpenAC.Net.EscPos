using System;
using System.Runtime.Serialization;
using OpenAC.Net.Core;

namespace OpenAC.Net.EscPos.Interpreter.Resolver;

/// <summary>
/// Exceção lançada ao ocorrer um erro durante a resolução de comandos no interpretador ESC/POS.
/// </summary>
[Serializable]
public sealed class ResolverException : OpenException
{
    #region Constructor

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ResolverException"/> com uma mensagem de erro especificada.
    /// </summary>
    /// <param name="message">A mensagem que descreve o erro.</param>
    public ResolverException(string message)
        : base(message)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ResolverException"/>.
    /// </summary>
    public ResolverException()
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ResolverException"/> com uma mensagem formatada.
    /// </summary>
    /// <param name="format">A string de formatação.</param>
    /// <param name="args">Os argumentos para a formatação.</param>
    public ResolverException(string format, params object[] args) : base(string.Format(format, args))
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ResolverException"/> com uma mensagem de erro e uma exceção interna.
    /// </summary>
    /// <param name="message">A mensagem de erro que explica o motivo da exceção.</param>
    /// <param name="innerException">A exceção que é a causa da exceção atual.</param>
    public ResolverException(string message, Exception innerException)
        : base(message, innerException)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ResolverException"/> com uma exceção interna e uma mensagem formatada.
    /// </summary>
    /// <param name="innerException">A exceção interna.</param>
    /// <param name="message">A mensagem de erro.</param>
    /// <param name="args">Os argumentos para a formatação.</param>
    public ResolverException(Exception innerException, string message, params object[] args)
        : base(string.Format(message, args), innerException)
    {
    }

    /// <summary>
    /// Inicializa uma nova instância da classe <see cref="ResolverException"/> com dados serializados.
    /// </summary>
    /// <param name="info">O objeto <see cref="SerializationInfo"/> que contém os dados serializados.</param>
    /// <param name="context">O objeto <see cref="StreamingContext"/> que contém informações contextuais.</param>
    public ResolverException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }

    #endregion Constructor
}