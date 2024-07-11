using System.Linq;
using System.Text;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Interpreter.Epson;
using OpenAC.Net.EscPos.Interpreter.Resolver;

namespace OpenAC.Net.EscPos.Interpreter.Sunmi;

public class SunmiInterpreter : EpsonInterpreter
{
    #region Constructors
    
    internal SunmiInterpreter(Encoding enconder) : base(enconder)
    {
    }
    
    #endregion Constructors
    
    #region Methods

    protected override void IniciarInterpreter()
    {
        Status = new EpsonStatusResolver();
        InfoImpressora = new EpsonInfoImpressoraResolver(Enconder);

        var commandos = DefaultCommands.EscPos.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        commandos[CmdEscPos.FonteA] = [CmdConst.ESC, (byte)'M', 0];
        commandos[CmdEscPos.Zera] = [CmdConst.ESC, (byte)'@', CmdConst.ESC, (byte)'M', 0];
        commandos[CmdEscPos.Beep] = [CmdConst.ESC, CmdConst.GS, CmdConst.BELL, (byte)'1', 2, 5];
        commandos[CmdEscPos.LigaCondensado] = [];
        commandos[CmdEscPos.DesligaCondensado] = [];

        CommandResolver.AddResolver<CodePageCommand, DefaultCodePageResolver>(new DefaultCodePageResolver(commandos));
        CommandResolver.AddResolver<TextCommand, SunmiTextResolver>(new SunmiTextResolver(Enconder, commandos));
        CommandResolver.AddResolver<TextSliceCommand, DefaultTextSliceResolver>(new DefaultTextSliceResolver(Enconder, commandos));
        CommandResolver.AddResolver<ZeraCommand, DefaultZeraResolver>(new DefaultZeraResolver(commandos));
        CommandResolver.AddResolver<EspacoEntreLinhasCommand, DefaultEspacoEntreLinhasResolver>(new DefaultEspacoEntreLinhasResolver(commandos));
        CommandResolver.AddResolver<PrintLineCommand, DefaultPrintLineResolver>(new DefaultPrintLineResolver(Enconder, commandos));
        CommandResolver.AddResolver<JumpLineCommand, DefaultJumpLineResolver>(new DefaultJumpLineResolver(commandos));
        CommandResolver.AddResolver<CutCommand, DefaultCutResolver>(new DefaultCutResolver(commandos));
        CommandResolver.AddResolver<CashDrawerCommand, DefaultCashDrawerResolver>(new DefaultCashDrawerResolver(commandos));
        CommandResolver.AddResolver<BeepCommand, DefaultBeepResolver>(new DefaultBeepResolver(commandos));
        CommandResolver.AddResolver<BarcodeCommand, DefaultBarcodeResolver>(new DefaultBarcodeResolver(Enconder, commandos));
        CommandResolver.AddResolver<QrCodeCommand, DefaultQrCodeResolver>(new DefaultQrCodeResolver(commandos));
        CommandResolver.AddResolver<ImageCommand, DefaultImageResolver>(new DefaultImageResolver(commandos));
        CommandResolver.AddResolver<ModoPaginaCommand, DefaultModoPaginaResolver>(new DefaultModoPaginaResolver(commandos));
    }

    #endregion Methods
}