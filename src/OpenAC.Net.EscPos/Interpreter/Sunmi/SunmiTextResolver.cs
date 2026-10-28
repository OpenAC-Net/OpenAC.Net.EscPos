using System;
using System.Collections.Generic;
using System.Text;
using OpenAC.Net.Devices.Commom;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Sunmi;

/// <summary>
/// Responsável por resolver comandos de texto para impressoras Sunmi, convertendo propriedades de formatação
/// em comandos ESC/POS e texto em bytes.
/// </summary>
public class SunmiTextResolver : CommandResolver<TextCommand>
{
    #region Constructors

    /// <summary>
    /// Inicializa uma nova instância de <see cref="SunmiTextResolver"/>.
    /// </summary>
    /// <param name="enconder">Codificação de texto a ser utilizada.</param>
    /// <param name="dict">Dicionário de comandos ESC/POS.</param>
    public SunmiTextResolver(Encoding enconder, IReadOnlyDictionary<CmdEscPos, byte[]> dict) : base(dict)
    {
        Enconder = enconder;
    }

    #endregion Constructors

    #region Properties

    /// <summary>
    /// Obtém a codificação utilizada para converter texto em bytes.
    /// </summary>
    public Encoding Enconder { get; }

    #endregion Properties

    #region Methods

    /// <summary>
    /// Resolve o comando de texto, aplicando formatação e retornando os bytes correspondentes.
    /// </summary>
    /// <param name="cmd">Comando de texto a ser resolvido.</param>
    /// <returns>Array de bytes com os comandos e texto formatado.</returns>
    public override byte[] Resolve(TextCommand cmd)
    {
        using var builder = new ByteArrayBuilder();

        switch (cmd.Fonte)
        {
            case CmdFonte.Normal:
                if (Commandos.TryGetValue(CmdEscPos.FonteNormal, out var fonteNormal))
                    builder.Append(fonteNormal);
                break;

            case CmdFonte.FonteA:
                if (Commandos.TryGetValue(CmdEscPos.FonteA, out var fonteA))
                    builder.Append(fonteA);
                break;

            case CmdFonte.FonteB:
                if (Commandos.TryGetValue(CmdEscPos.FonteB, out var fonteB))
                    builder.Append(fonteB);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (cmd.Alinhamento)
        {
            case CmdAlinhamento.Esquerda:
                if (Commandos.TryGetValue(CmdEscPos.AlinhadoEsquerda, out var alinhadoEsquerda))
                    builder.Append(alinhadoEsquerda);
                break;

            case CmdAlinhamento.Centro:
                if (Commandos.TryGetValue(CmdEscPos.AlinhadoCentro, out var alinhadoCentro))
                    builder.Append(alinhadoCentro);
                break;

            case CmdAlinhamento.Direita:
                if (Commandos.TryGetValue(CmdEscPos.AlinhadoDireita, out var alinhadoDireita))
                    builder.Append(alinhadoDireita);
                break;

            default:
                throw new ArgumentOutOfRangeException();
        }

        switch (cmd.Tamanho)
        {
            case CmdTamanhoFonte.Expandida when Commandos.ContainsKey(CmdEscPos.LigaExpandido):
                builder.Append(Commandos[CmdEscPos.LigaExpandido]);
                break;

            case CmdTamanhoFonte.Condensada when Commandos.ContainsKey(CmdEscPos.LigaCondensado):
                builder.Append(Commandos[CmdEscPos.LigaCondensado]);
                break;
        }

        if (cmd.Estilo.HasValue)
        {
            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Negrito) && Commandos.TryGetValue(CmdEscPos.LigaNegrito, out var commando))
                builder.Append(commando);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Sublinhado) && Commandos.TryGetValue(CmdEscPos.LigaSublinhado, out var commando1))
                builder.Append(commando1);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Italico) && Commandos.TryGetValue(CmdEscPos.LigaItalico, out var commando2))
                builder.Append(commando2);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.AlturaDupla) && Commandos.TryGetValue(CmdEscPos.LigaAlturaDupla, out var commando3))
                builder.Append(commando3);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Invertido) && Commandos.TryGetValue(CmdEscPos.LigaInvertido, out var commando4))
                builder.Append(commando4);
        }

        // Adiciona o texto, fazendo o tratamento para quebra de linha
        builder.Append(Enconder.GetBytes(TratarString(cmd.Texto ?? string.Empty)));

        if (cmd.Estilo.HasValue)
        {
            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Negrito) && Commandos.TryGetValue(CmdEscPos.DesligaNegrito, out var commando))
                builder.Append(commando);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Sublinhado) && Commandos.TryGetValue(CmdEscPos.DesligaSublinhado, out var commando1))
                builder.Append(commando1);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Italico) && Commandos.TryGetValue(CmdEscPos.DesligaItalico, out var commando2))
                builder.Append(commando2);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.AlturaDupla) && Commandos.TryGetValue(CmdEscPos.DesligaAlturaDupla, out var commando3))
                builder.Append(commando3);

            if (cmd.Estilo.Value.HasFlag(CmdEstiloFonte.Invertido) && Commandos.TryGetValue(CmdEscPos.DesligaInvertido, out var commando4))
                builder.Append(commando4);
        }

        switch (cmd.Tamanho)
        {
            case CmdTamanhoFonte.Expandida when Commandos.ContainsKey(CmdEscPos.DesligaExpandido):
                builder.Append(Commandos[CmdEscPos.DesligaExpandido]);
                break;

            case CmdTamanhoFonte.Condensada when Commandos.ContainsKey(CmdEscPos.DesligaCondensado):
                builder.Append(Commandos[CmdEscPos.DesligaCondensado]);
                break;
        }

        // Volta a Fonte para Normal.
        if (Commandos.ContainsKey(CmdEscPos.FonteNormal) && cmd.Fonte != CmdFonte.Normal)
            builder.Append(Commandos[CmdEscPos.FonteNormal]);

        // Volta alinhamento para Esquerda.
        if (Commandos.ContainsKey(CmdEscPos.AlinhadoEsquerda) && cmd.Alinhamento != CmdAlinhamento.Esquerda)
            builder.Append(Commandos[CmdEscPos.AlinhadoEsquerda]);

        // Texto sempre tem que terminar com o pulo de linha
        builder.Append(Commandos[CmdEscPos.PuloDeLinha]);
        return builder.ToArray();
    }

    /// <summary>
    /// Função para tratar a quebra de linha da string para o formato que a impressora aceita.
    /// </summary>
    /// <param name="texto">Texto a ser tratado.</param>
    /// <returns>Texto com quebras de linha normalizadas.</returns>
    private static string TratarString(string texto) => texto.Replace("\r\n", "\n").Replace("\r", "\n");

    #endregion Methods
}