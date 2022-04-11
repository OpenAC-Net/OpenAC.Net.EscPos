using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Interpreter.Resolver;

public sealed class DefaultPrintLineResolver : CommandResolver<PrintLineCommand>
{
    #region Constructors

    public DefaultPrintLineResolver(Encoding enconder, IReadOnlyDictionary<CmdEscPos, byte[]> dictionary) : base(dictionary)
    {
        Enconder = enconder;
    }

    #endregion Constructors

    #region Properties

    public Encoding Enconder { get; }

    #endregion Properties

    public override byte[] Resolve(PrintLineCommand command)
    {
        return Enconder.GetBytes(new string(command.Dupla ? '=' : '-', command.Tamanho))
            .Concat(Commandos[CmdEscPos.PuloDeLinha]).ToArray();
    }
}