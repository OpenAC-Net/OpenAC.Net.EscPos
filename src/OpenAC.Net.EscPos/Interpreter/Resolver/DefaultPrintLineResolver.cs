using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Interpreter.Resolver;

/// <summary>
/// Resolve comandos de impressão de linha padrão para ESC/POS.
/// </summary>
public sealed class DefaultPrintLineResolver : CommandResolver<PrintLineCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância de <see cref="DefaultPrintLineResolver"/>.
    /// </summary>
    /// <param name="enconder">Codificação de caracteres a ser utilizada.</param>
    /// <param name="dictionary">Dicionário de comandos ESC/POS.</param>
    public DefaultPrintLineResolver(Encoding enconder, IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
    {
        Enconder = enconder;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtém a codificação de caracteres utilizada.
    /// </summary>
    public Encoding Enconder { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Resolve o comando <see cref="PrintLineCommand"/> em bytes ESC/POS.
    /// </summary>
    /// <param name="command">Comando de impressão de linha.</param>
    /// <returns>Array de bytes correspondente ao comando.</returns>
    public override byte[] Resolve(PrintLineCommand command)
    {
        return Enconder.GetBytes(new string(command.Dupla ? '=' : '-', command.Tamanho))
            .Concat(Commandos[CmdEscPos.PuloDeLinha]).ToArray();
    }

    #endregion Methods
}