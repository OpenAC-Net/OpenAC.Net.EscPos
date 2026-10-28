using System.Globalization;
using System.IO.Ports;
using System.Text;
using Avalonia.Controls;
using Avalonia.Input.Platform;
using Avalonia.Platform.Storage;
using Avalonia.Threading;
using NLog;
using NLog.Config;
using NLog.Targets;
using OpenAC.Net.Devices;
using OpenAC.Net.EscPos.Command;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Demo.Commom;
using OpenAC.Net.EscPos.Extensions;
using AvaloniaBitmap = Avalonia.Media.Imaging.Bitmap;

namespace OpenAC.Net.EscPos.Demo;

public enum PreviewTemplate
{
    CupomCompleto,
    TesteTextos,
    CodigoBarras,
    QrCodes,
    ModoPagina
}

public partial class MainWindow : Window
{
    private readonly OpenConfig config;
    private readonly string configurationPath;
    private AvaloniaBitmap? currentPreviewBitmap;
    private byte[]? currentPreviewPngBytes;
    private bool closed;
    private bool initialized;

    public MainWindow()
    {
        InitializeComponent();

        configurationPath = Path.Combine(AppContext.BaseDirectory, "escpos.config");
        config = OpenConfig.CreateOrLoad(configurationPath);

        InitializeLogging();
        InitializeControls();
        LoadConfig();
        ConnectEvents();

        initialized = true;
        UpdateConnectionPanels();
        UpdateContext();
    }

    private void InitializeControls()
    {
        Fill(ProtocolCombo, Enum.GetValues<ProtocoloEscPos>(), ProtocoloEscPos.EscPos);
        Fill(ConnectionTypeCombo, Enum.GetValues<TipoConexao>(), TipoConexao.Serial);
        Fill(EncodingCombo, Enum.GetValues<PaginaCodigo>(), PaginaCodigo.pc850);
        Fill(SerialBaudCombo, Enum.GetValues<SerialBaud>(), SerialBaud.Bd9600, b => $"{(int)b} bps");
        Fill(SerialDataBitsCombo, Enum.GetValues<SerialDataBits>(), SerialDataBits.Db8, d => $"{(int)d} bits");
        Fill(SerialParityCombo, Enum.GetValues<Parity>(), Parity.None);
        Fill(SerialStopBitsCombo, Enum.GetValues<StopBits>(), StopBits.None);
        Fill(SerialHandshakeCombo, Enum.GetValues<Handshake>(), Handshake.None);

        Fill(PreviewTemplateCombo, Enum.GetValues<PreviewTemplate>(), PreviewTemplate.CupomCompleto, template => template switch
        {
            PreviewTemplate.CupomCompleto => "Cupom Completo (Textos + EAN13 + QR Code)",
            PreviewTemplate.TesteTextos => "Teste de Textos, Fontes e Estilos",
            PreviewTemplate.CodigoBarras => "Teste de Códigos de Barras (CODE39, ITF, CODE128)",
            PreviewTemplate.QrCodes => "Teste de QR Codes (NFC-e e SAT)",
            PreviewTemplate.ModoPagina => "Teste de Modo Página (4 Regiões e Direções)",
            _ => template.ToString()
        });

        RefreshSerialPorts();
        RefreshInstalledPrinters();

        ConfigurationPathText.Text = $"Arquivo de configuração: {configurationPath}";
    }

    private void RefreshSerialPorts()
    {
        var current = GetComboText(SerialPortCombo, "COM1");
        string[] ports;
        try
        {
            ports = SerialPort.GetPortNames();
        }
        catch
        {
            ports = [];
        }

        if (ports.Length == 0)
            ports = ["COM1", "/dev/ttyS0", "/dev/ttyUSB0"];

        SerialPortCombo.ItemsSource = ports;
        SerialPortCombo.SelectedItem = ports.FirstOrDefault(p => string.Equals(p, current, StringComparison.OrdinalIgnoreCase)) ?? ports[0];
        SerialPortCombo.Text = current;
    }

    private void RefreshInstalledPrinters()
    {
        var printers = new List<string> { "Generic / Text Only" };
        RawPrinterCombo.ItemsSource = printers;
        RawPrinterCombo.SelectedItem = printers[0];
        RawPrinterCombo.Text = printers[0];
    }

    private async Task RefreshBluetoothDevicesAsync()
    {
        var address = BluetoothAddressText.Text?.Trim();
        var devices = await Task.Run(() => BluetoothConfig.GetDevices());
        BluetoothDeviceCombo.ItemsSource = devices;
        BluetoothDeviceCombo.SelectedItem = devices.FirstOrDefault(device =>
            string.Equals(device.Address, address, StringComparison.OrdinalIgnoreCase));
        ResultText.Text = $"Dispositivos Bluetooth encontrados: {devices.Count}.\nSelecione um dispositivo ou informe o endereço manualmente.";
        ResponseTabs.SelectedIndex = 1;
    }

    private async Task RefreshUsbDevicesAsync()
    {
        var path = UsbDevicePathText.Text?.Trim();
        var devices = await Task.Run(() => UsbConfig.GetDevices());
        UsbDeviceCombo.ItemsSource = devices;
        UsbDeviceCombo.SelectedItem = devices.FirstOrDefault(device =>
            string.Equals(device.DevicePath, path, StringComparison.Ordinal));
        ResultText.Text = $"Dispositivos USB encontrados: {devices.Count}.\nSelecione um dispositivo ou informe o caminho ou VID/PID manualmente.";
        ResponseTabs.SelectedIndex = 1;
    }

    private void ConnectEvents()
    {
        Closing += (_, _) =>
        {
            closed = true;
            currentPreviewBitmap?.Dispose();
        };

        Navigation.SelectionChanged += (_, _) =>
        {
            OperationsPage.IsVisible = Navigation.SelectedIndex == 0;
            SettingsPage.IsVisible = Navigation.SelectedIndex == 1;
        };

        ConfigureButton.Click += (_, _) => Navigation.SelectedIndex = 1;

        ProtocolCombo.SelectionChanged += (_, _) => UpdateContext();
        EncodingCombo.SelectionChanged += (_, _) => UpdateContext();
        ConnectionTypeCombo.SelectionChanged += (_, _) =>
        {
            UpdateConnectionPanels();
            UpdateContext();
        };
        SerialPortCombo.SelectionChanged += (_, _) => UpdateContext();
        SerialBaudCombo.SelectionChanged += (_, _) => UpdateContext();
        TcpIpText.TextChanged += (_, _) => UpdateContext();
        TcpPortInput.ValueChanged += (_, _) => UpdateContext();
        RawPrinterCombo.SelectionChanged += (_, _) => UpdateContext();
        OutputFileText.TextChanged += (_, _) => UpdateContext();
        BluetoothAddressText.TextChanged += (_, _) => UpdateContext();
        BluetoothChannelInput.ValueChanged += (_, _) => UpdateContext();
        UsbDevicePathText.TextChanged += (_, _) => UpdateContext();
        UsbVendorIdText.TextChanged += (_, _) => UpdateContext();
        UsbProductIdText.TextChanged += (_, _) => UpdateContext();

        BluetoothDeviceCombo.SelectionChanged += (_, _) =>
        {
            if (BluetoothDeviceCombo.SelectedItem is BluetoothDeviceInfo device)
                BluetoothAddressText.Text = device.Address;
        };
        UsbDeviceCombo.SelectionChanged += (_, _) =>
        {
            if (UsbDeviceCombo.SelectedItem is not UsbDeviceInfo device) return;
            UsbDevicePathText.Text = device.DevicePath;
            UsbVendorIdText.Text = device.VendorId?.ToString("X4", CultureInfo.InvariantCulture) ?? string.Empty;
            UsbProductIdText.Text = device.ProductId?.ToString("X4", CultureInfo.InvariantCulture) ?? string.Empty;
        };

        RefreshPortsButton.Click += (_, _) => RefreshSerialPorts();
        RefreshBluetoothButton.Click += async (_, _) => await RunAsync("Buscar dispositivos Bluetooth", RefreshBluetoothDevicesAsync);
        RefreshUsbButton.Click += async (_, _) => await RunAsync("Buscar dispositivos USB", RefreshUsbDevicesAsync);

        BrowseOutputFileButton.Click += async (_, _) => await RunAsync("Selecionar arquivo de saída", async () =>
        {
            var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
            {
                Title = "Selecionar arquivo de saída ESC/POS",
                DefaultExtension = "txt",
                SuggestedFileName = "escpos_output.txt",
                FileTypeChoices =
                [
                    new FilePickerFileType("Arquivos de texto / impressão") { Patterns = ["*.txt", "*.prn", "*.bin"] },
                    new FilePickerFileType("Todos os arquivos") { Patterns = ["*.*"] }
                ]
            });

            if (file != null)
                OutputFileText.Text = file.TryGetLocalPath() ?? file.Name;
        });

        SaveSettingsButton.Click += async (_, _) => await RunAsync("Salvar configurações", () =>
        {
            SaveConfig();
            ResultText.Text = $"Configurações salvas com sucesso em:\n{configurationPath}";
            ResponseTabs.SelectedIndex = 1;
            return Task.CompletedTask;
        });

        PrintTextButton.Click += async (_, _) => await RunAsync("Imprimir Texto", () => Task.Run(PrintTextTest));
        PrintBarcodeButton.Click += async (_, _) => await RunAsync("Imprimir Código de Barras", () => Task.Run(PrintBarcodeTest));
        PrintQrCodeButton.Click += async (_, _) => await RunAsync("Imprimir QR Code", () => Task.Run(PrintQrCodeTest));
        PrintPageModeButton.Click += async (_, _) => await RunAsync("Imprimir Modo Página", () => Task.Run(PrintPageModeTest));
        PrintImageButton.Click += async (_, _) => await RunAsync("Imprimir Imagem", PrintImageTestAsync);
        CashDrawerButton.Click += async (_, _) => await RunAsync("Abrir Gaveta", () => Task.Run(OpenCashDrawerTest));
        CutPaperButton.Click += async (_, _) => await RunAsync("Cortar Papel", () => Task.Run(CutPaperTest));
        BeepButton.Click += async (_, _) => await RunAsync("Emitir Beep", () => Task.Run(BeepTest));

        ReadStatusButton.Click += async (_, _) => await RunAsync("Ler Status da Impressora", () => Task.Run(ReadStatus));
        ReadInfoButton.Click += async (_, _) => await RunAsync("Ler Informações da Impressora", () => Task.Run(ReadInfo));

        GeneratePreviewButton.Click += async (_, _) => await RunAsync("Gerar Preview", GeneratePreviewAsync);
        SavePreviewButton.Click += async (_, _) => await RunAsync("Salvar Preview PNG", SavePreviewAsync);

        CopyButton.Click += async (_, _) =>
        {
            if (Clipboard == null) return;
            var text = ResponseTabs.SelectedIndex == 2 ? LogText.Text : ResultText.Text;
            await Clipboard.SetTextAsync(text ?? string.Empty);
            StatusText.Text = "Conteúdo copiado para a área de transferência";
        };

        ClearButton.Click += (_, _) =>
        {
            ResultText.Text = string.Empty;
            LogText.Text = string.Empty;
            PreviewImage.Source = null;
            PreviewEmptyState.IsVisible = true;
            currentPreviewBitmap?.Dispose();
            currentPreviewBitmap = null;
            currentPreviewPngBytes = null;
            LastOperationText.Text = string.Empty;
            StatusText.Text = "Pronto para operar";
        };
    }

    private void UpdateConnectionPanels()
    {
        if (!initialized) return;

        var tipo = Selected<TipoConexao>(ConnectionTypeCombo);
        SerialPanel.IsVisible = tipo == TipoConexao.Serial;
        TcpPanel.IsVisible = tipo == TipoConexao.TCP;
        RawPanel.IsVisible = tipo == TipoConexao.RAW;
        FilePanel.IsVisible = tipo == TipoConexao.File;
        BluetoothPanel.IsVisible = tipo == TipoConexao.Bluetooth;
        UsbPanel.IsVisible = tipo == TipoConexao.USB;
    }

    private void UpdateContext()
    {
        if (!initialized) return;

        var protocolo = Selected<ProtocoloEscPos>(ProtocolCombo);
        var encoding = Selected<PaginaCodigo>(EncodingCombo);
        var conexao = Selected<TipoConexao>(ConnectionTypeCombo);

        ProtocolBadgeText.Text = protocolo.ToString();

        var targetSummary = conexao switch
        {
            TipoConexao.Serial => $"{GetComboText(SerialPortCombo, "COM1")} @ {(int)Selected<SerialBaud>(SerialBaudCombo)}",
            TipoConexao.TCP => $"{(string.IsNullOrWhiteSpace(TcpIpText.Text) ? "127.0.0.1" : TcpIpText.Text)}:{(int)(TcpPortInput.Value ?? 9100)}",
            TipoConexao.RAW => GetComboText(RawPrinterCombo, "RAW"),
            TipoConexao.File => string.IsNullOrWhiteSpace(OutputFileText.Text) ? "Arquivo" : Path.GetFileName(OutputFileText.Text),
            TipoConexao.Bluetooth => $"{BluetoothAddressText.Text} · Canal {(int)(BluetoothChannelInput.Value ?? 0)}",
            TipoConexao.USB => string.IsNullOrWhiteSpace(UsbDevicePathText.Text)
                ? $"VID:{UsbVendorIdText.Text} PID:{UsbProductIdText.Text}"
                : UsbDevicePathText.Text,
            _ => conexao.ToString()
        };

        ConnectionText.Text = $"{conexao} · {targetSummary}";
        ContextText.Text = $"Protocolo {protocolo} · Encoding {encoding} · Espaço {LineSpaceInput.Value ?? 0} · Linhas {CouponLinesInput.Value ?? 4}";
    }

    private EscPosPrinter GetPosPrinter()
    {
        var tipo = Selected<TipoConexao>(ConnectionTypeCombo);
        var serialPort = GetComboText(SerialPortCombo, "COM1");
        var serialBaud = (int)Selected<SerialBaud>(SerialBaudCombo);
        var serialDataBits = (int)Selected<SerialDataBits>(SerialDataBitsCombo);
        var serialParity = Selected<Parity>(SerialParityCombo);
        var serialStopBits = Selected<StopBits>(SerialStopBitsCombo);
        var serialHandshake = Selected<Handshake>(SerialHandshakeCombo);
        var tcpIp = TcpIpText.Text ?? string.Empty;
        var tcpPort = (int)(TcpPortInput.Value ?? 9100);
        var rawPrinter = GetComboText(RawPrinterCombo, string.Empty);
        var outputFile = OutputFileText.Text ?? string.Empty;
        var protocolo = Selected<ProtocoloEscPos>(ProtocolCombo);
        var paginaCodigo = Selected<PaginaCodigo>(EncodingCombo);
        var controlePorta = PortControlCheck.IsChecked == true;
        var espacoLinhas = (byte)(LineSpaceInput.Value ?? 0);
        var linhasCupons = (byte)(CouponLinesInput.Value ?? 4);

        EscPosPrinter ret = tipo switch
        {
            TipoConexao.Serial => new EscPosPrinter<SerialConfig>
            {
                Device =
                {
                    Porta = serialPort,
                    Baud = serialBaud,
                    DataBits = serialDataBits,
                    Parity = serialParity,
                    StopBits = serialStopBits,
                    Handshake = serialHandshake
                }
            },
            TipoConexao.TCP => new EscPosPrinter<TCPConfig>
            {
                Device =
                {
                    IP = tcpIp,
                    Porta = tcpPort
                }
            },
            TipoConexao.RAW => new EscPosPrinter<RawConfig>
            {
                Device =
                {
                    Impressora = rawPrinter
                }
            },
            TipoConexao.File => new EscPosPrinter<FileConfig>
            {
                Device =
                {
                    CreateIfNotExists = true,
                    File = outputFile
                }
            },
            TipoConexao.Bluetooth => new EscPosPrinter<BluetoothConfig>
            {
                Device =
                {
                    Endereco = BluetoothAddressText.Text?.Trim(),
                    Canal = (int)(BluetoothChannelInput.Value ?? 0)
                }
            },
            TipoConexao.USB => CreateUsbPrinter(),
            _ => throw new ArgumentOutOfRangeException()
        };

        ret.Protocolo = protocolo;
        ret.PaginaCodigo = paginaCodigo;
        ret.Device.ControlePorta = controlePorta;
        ret.EspacoEntreLinhas = espacoLinhas;
        ret.LinhasEntreCupons = linhasCupons;

        return ret;
    }

    private EscPosPrinter<UsbConfig> CreateUsbPrinter()
    {
        var printer = new EscPosPrinter<UsbConfig>();
        var path = UsbDevicePathText.Text?.Trim();
        if (!string.IsNullOrWhiteSpace(path))
        {
            printer.Device.DevicePath = path;
        }
        else
        {
            printer.Device.VendorId = ParseUsbId(UsbVendorIdText.Text, "VID");
            printer.Device.ProductId = ParseUsbId(UsbProductIdText.Text, "PID");
        }

        return printer;
    }

    private static int? ParseUsbId(string? text, string name)
    {
        if (string.IsNullOrWhiteSpace(text)) return null;
        var value = text.Trim();
        if (value.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            value = value[2..];
        if (ushort.TryParse(value, NumberStyles.AllowHexSpecifier, CultureInfo.InvariantCulture, out var id))
            return id;
        throw new ArgumentException($"O {name} USB deve ser um valor hexadecimal entre 0000 e FFFF.");
    }

    private void SaveConfig()
    {
        config.Set("ProtocoloEscPos", Selected<ProtocoloEscPos>(ProtocolCombo));
        config.Set("TipoConexao", Selected<TipoConexao>(ConnectionTypeCombo));
        config.Set("PaginaCodigo", Selected<PaginaCodigo>(EncodingCombo));
        config.Set("ControlePorta", PortControlCheck.IsChecked == true);
        config.Set("EspacoEntreLinhas", LineSpaceInput.Value ?? 0M);
        config.Set("LinhasEntreCupom", CouponLinesInput.Value ?? 4M);

        // Serial
        config.Set("SerialPorta", GetComboText(SerialPortCombo, "COM1"));
        config.Set("SerialVelocidade", Selected<SerialBaud>(SerialBaudCombo));
        config.Set("SerialDataBits", Selected<SerialDataBits>(SerialDataBitsCombo));
        config.Set("SerialParaty", Selected<Parity>(SerialParityCombo));
        config.Set("SerialStopBits", Selected<StopBits>(SerialStopBitsCombo));
        config.Set("SerialHandhsake", Selected<Handshake>(SerialHandshakeCombo));

        // TCP
        config.Set("TCPIP", TcpIpText.Text ?? string.Empty);
        config.Set("TCPPorta", TcpPortInput.Value ?? 9100M);

        // RAW
        config.Set("RawImpressora", GetComboText(RawPrinterCombo, string.Empty));

        // File
        config.Set("FileArquivo", OutputFileText.Text ?? string.Empty);

        // Bluetooth
        config.Set("BluetoothEndereco", BluetoothAddressText.Text ?? string.Empty);
        config.Set("BluetoothCanal", BluetoothChannelInput.Value ?? 0M);

        // USB
        config.Set("UsbDevicePath", UsbDevicePathText.Text ?? string.Empty);
        config.Set("UsbVendorId", UsbVendorIdText.Text ?? string.Empty);
        config.Set("UsbProductId", UsbProductIdText.Text ?? string.Empty);

        config.Save();
        UpdateContext();
    }

    private void LoadConfig()
    {
        SelectValue(ProtocolCombo, config.Get("ProtocoloEscPos", ProtocoloEscPos.EscPos));
        SelectValue(ConnectionTypeCombo, config.Get("TipoConexao", TipoConexao.Serial));
        SelectValue(EncodingCombo, config.Get("PaginaCodigo", PaginaCodigo.pc850));
        PortControlCheck.IsChecked = config.Get("ControlePorta", true);
        LineSpaceInput.Value = config.Get("EspacoEntreLinhas", 0M);
        CouponLinesInput.Value = config.Get("LinhasEntreCupom", 4M);

        // Serial
        var savedPort = config.Get("SerialPorta", GetComboText(SerialPortCombo, "COM1"));
        SerialPortCombo.SelectedItem = savedPort;
        SerialPortCombo.Text = savedPort;
        SelectValue(SerialBaudCombo, config.Get("SerialVelocidade", SerialBaud.Bd9600));
        SelectValue(SerialDataBitsCombo, config.Get("SerialDataBits", SerialDataBits.Db8));
        SelectValue(SerialParityCombo, config.Get("SerialParaty", Parity.None));
        SelectValue(SerialStopBitsCombo, config.Get("SerialStopBits", StopBits.None));
        SelectValue(SerialHandshakeCombo, config.Get("SerialHandhsake", Handshake.None));

        // TCP
        TcpIpText.Text = config.Get("TCPIP", string.Empty);
        TcpPortInput.Value = config.Get("TCPPorta", 9100M);

        // RAW
        var savedPrinter = config.Get("RawImpressora", GetComboText(RawPrinterCombo, string.Empty));
        RawPrinterCombo.SelectedItem = savedPrinter;
        RawPrinterCombo.Text = savedPrinter;

        // File
        OutputFileText.Text = config.Get("FileArquivo", string.Empty);

        // Bluetooth
        BluetoothAddressText.Text = config.Get("BluetoothEndereco", string.Empty);
        BluetoothChannelInput.Value = config.Get("BluetoothCanal", 0M);

        // USB
        UsbDevicePathText.Text = config.Get("UsbDevicePath", string.Empty);
        UsbVendorIdText.Text = config.Get("UsbVendorId", string.Empty);
        UsbProductIdText.Text = config.Get("UsbProductId", string.Empty);
    }

    private static void PopulateTextCommands(EscPosPrinter posprinter)
    {
        posprinter.ImprimirTexto("ÁÉÍÓÚáéíóúçÇãõÃÕÊêÀà", CmdAlinhamento.Centro);
        posprinter.ImprimirTexto("TESTE NORMAL ESQUERDA", CmdAlinhamento.Esquerda);
        posprinter.ImprimirTexto("TESTE NORMAL DIREITA", CmdAlinhamento.Direita);
        posprinter.ImprimirLinha();
        posprinter.ImprimirTexto("TESTE EXPANDIDO", CmdFonte.Normal, CmdTamanhoFonte.Expandida, CmdAlinhamento.Esquerda, null);
        posprinter.ImprimirTexto("TESTE CONDENSADO", CmdFonte.Normal, CmdTamanhoFonte.Condensada, CmdAlinhamento.Esquerda, null);
        posprinter.ImprimirTexto("TESTE NORMAL", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, null);
        posprinter.ImprimirLinha();
        posprinter.ImprimirTexto("TESTE DUPLO EXPANDIDO", CmdFonte.Normal, CmdTamanhoFonte.Expandida, CmdAlinhamento.Esquerda, CmdEstiloFonte.AlturaDupla);
        posprinter.ImprimirTexto("TESTE DUPLO CONDENSADO", CmdFonte.Normal, CmdTamanhoFonte.Condensada, CmdAlinhamento.Esquerda, CmdEstiloFonte.AlturaDupla);
        posprinter.ImprimirTexto("TESTE DUPLO NORMAL", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, CmdEstiloFonte.AlturaDupla);
        posprinter.ImprimirLinha();
        posprinter.ImprimirTexto("TESTE NEGRITO", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, CmdEstiloFonte.Negrito);
        posprinter.ImprimirTexto("TESTE ITALICO", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, CmdEstiloFonte.Italico);
        posprinter.ImprimirTexto("TESTE SUBLINHADO", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, CmdEstiloFonte.Sublinhado);
        posprinter.ImprimirTexto("TESTE INVERTIDO", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Centro, CmdEstiloFonte.Invertido);
        posprinter.ImprimirLinha();
        posprinter.ImprimirTexto("TESTE FONTE NORMAL", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, null);
        posprinter.ImprimirTexto("TESTE FONTE A", CmdFonte.FonteA, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, null);
        posprinter.ImprimirTexto("TESTE FONTE B", CmdFonte.FonteB, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, null);
        posprinter.ImprimirLinha();
        posprinter.ImprimirTexto(new TextSlice("Teste Texto: ", CmdEstiloFonte.Negrito), new TextSlice("Funcionou"));
        posprinter.CortarPapel();
    }

    private static void PopulateBarcodeCommands(EscPosPrinter posprinter)
    {
        posprinter.ImprimirBarcode("ABCDE12345", CmdBarcode.CODE39, CmdAlinhamento.Centro, CmdBarcodeText.Acima);
        posprinter.ImprimirLinha();
        posprinter.ImprimirBarcode("1234567890", CmdBarcode.Inter2of5, CmdAlinhamento.Centro, CmdBarcodeText.Ambos);
        posprinter.ImprimirLinha();
        posprinter.ImprimirBarcode("$-=+ABC123abc", CmdBarcode.CODE128, CmdAlinhamento.Centro, CmdBarcodeText.Abaixo);
        posprinter.ImprimirLinha();
        posprinter.ImprimirBarcode("3515071111111111111159", CmdBarcode.CODE128c, CmdAlinhamento.Centro, CmdBarcodeText.Abaixo);
        posprinter.CortarPapel();
    }

    private static void PopulateQrCodeCommands(EscPosPrinter posprinter)
    {
        var qrCode = "https://www.homologacao.nfce.fazenda.sp.gov.br/NFCeConsultaPublica/Paginas/ConsultaQRCode.aspx?chNFe=3515080548133600013765022000" +
                     "0000711000001960&nVersao=100&tpAmb=2&dhEmi=323031352D30382D31395432323A33333A32352D30333A3030&vNF=3.00&vICMS=0.12&digVal=77696739" +
                     "6F2B665861706673396878776E64594C396F61654C35493D&cIdToken=000001&cHashQRCode=9BD312D558823E1EC68CEDB338A39B6150B0480E";

        posprinter.ImprimirTexto("Exemplo QrCode NFCe.", CmdEstiloFonte.Negrito);
        posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Minusculo, aAlinhamento: CmdAlinhamento.Centro);
        posprinter.ImprimirLinha();
        posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Pequeno, aAlinhamento: CmdAlinhamento.Centro);
        posprinter.ImprimirLinha();
        posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Normal, aAlinhamento: CmdAlinhamento.Centro);
        posprinter.ImprimirLinha();
        posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Grande, aAlinhamento: CmdAlinhamento.Centro);
        posprinter.ImprimirLinha();
        posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.ExtraGrande, aAlinhamento: CmdAlinhamento.Centro);
        posprinter.ImprimirLinha();

        posprinter.PularLinhas(2);

        qrCode = "35150811111111111111591234567890001672668828|20150820201736|118.72|05481336000137|TCbeD81ePUpMvso4VjFqRTvs4ov" +
                 "qmR1ZG3bwSCumzHtW8bbMedVJjVnww103v3LxKfgckAyuizcR/9pXaKay6M4Gu8kyDef+6VH5qONIZV1cB+mFfXiaCgeZALuRDCH1PRyb6hoB" +
                 "eRUkUk6lOdXSczRW9Y83GJMXdOFroEbzFmpf4+WOhe2BZ3mEdXKKGMfl1EB0JWnAThkGT+1Er9Jh/3En5YI4hgQP3NC2BiJVJ6oCEbKb85s59" +
                 "15DSZAw4qB/MlESWViDsDVYEnS/FQgA2kP2A9pR4+agdHmgWiz30MJYqX5Ng9XEYvvOMzl1Y6+7/frzsocOxfuQyFsnfJzogw==";

        posprinter.ImprimirTexto("Exemplo QrCode SAT.", CmdEstiloFonte.Negrito);
        posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Minusculo, aAlinhamento: CmdAlinhamento.Centro);
        posprinter.ImprimirLinha();
        posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Pequeno, aAlinhamento: CmdAlinhamento.Centro);
        posprinter.ImprimirLinha();
        posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Normal, aAlinhamento: CmdAlinhamento.Centro);
        posprinter.ImprimirLinha();
        posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Grande, aAlinhamento: CmdAlinhamento.Centro);
        posprinter.ImprimirLinha();
        posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.ExtraGrande, aAlinhamento: CmdAlinhamento.Centro);

        posprinter.CortarPapel();
    }

    private static void PopulatePageModeCommands(EscPosPrinter posprinter)
    {
        posprinter.CodigoBarras.Altura = 40;
        posprinter.CodigoBarras.Largura = 2;
        posprinter.CodigoBarras.Exibir = CmdBarcodeText.SemTexto;

        var modoPagina = posprinter.IniciarModoPagina();

        // Região 1
        var regiao = modoPagina.NovaRegiao(0, 0, 257, 740);
        regiao.Direcao = CmdPosDirecao.EsquerdaParaDireita;
        regiao.EspacoEntreLinhas = 25;
        regiao.ImprimirTexto("CONDENSADA/NEGRITO", CmdTamanhoFonte.Condensada, CmdEstiloFonte.Negrito);
        regiao.ImprimirTexto("EXPANDIDO", CmdTamanhoFonte.Expandida);
        regiao.ImprimirTexto("INVERTIDA", CmdEstiloFonte.Invertido);
        regiao.PularLinhas(1);
        regiao.ImprimirBarcode("1234567890", CmdBarcode.Inter2of5, CmdBarcodeText.SemTexto);
        regiao.ImprimirQrCode("https://github.com/OpenAC-Net/OpenAC.Net.EscPos", CmdAlinhamento.Centro);

        // Região 2
        regiao = modoPagina.NovaRegiao(210, 0, 400, 500);
        regiao.Direcao = CmdPosDirecao.TopoParaBaixo;
        regiao.EspacoEntreLinhas = 25;
        regiao.ImprimirTexto("EXPANDIDO", CmdTamanhoFonte.Expandida);
        regiao.ImprimirTexto("INVERTIDA", CmdEstiloFonte.Invertido);
        regiao.PularLinhas(1);
        regiao.ImprimirBarcode("1234567890", CmdBarcode.Inter2of5, CmdAlinhamento.Esquerda, CmdBarcodeText.SemTexto, 2, 40);
        regiao.ImprimirQrCode("https://github.com/OpenAC-Net/OpenAC.Net.EscPos", CmdAlinhamento.Centro);

        posprinter.ImprimirTexto("MODO PAGINA DESLIGADO");

        modoPagina = posprinter.IniciarModoPagina();

        // Região 3
        regiao = modoPagina.NovaRegiao(0, 0, 257, 740);
        regiao.Direcao = CmdPosDirecao.BaixoParaTopo;
        regiao.EspacoEntreLinhas = 25;
        regiao.ImprimirTexto("CONDENSADA/NEGRITO", CmdTamanhoFonte.Condensada, CmdEstiloFonte.Negrito);
        regiao.ImprimirTexto("EXPANDIDO", CmdTamanhoFonte.Expandida);
        regiao.ImprimirTexto("INVERTIDA", CmdEstiloFonte.Invertido);
        regiao.PularLinhas(1);
        regiao.ImprimirBarcode("1234567890", CmdBarcode.Inter2of5, CmdAlinhamento.Esquerda, CmdBarcodeText.SemTexto, 2, 40);
        regiao.ImprimirQrCode("https://github.com/OpenAC-Net/OpenAC.Net.EscPos", CmdAlinhamento.Centro);

        // Região 4
        regiao = modoPagina.NovaRegiao(210, 0, 400, 500);
        regiao.Direcao = CmdPosDirecao.DireitaParaEsquerda;
        regiao.EspacoEntreLinhas = 25;
        regiao.ImprimirTexto("CONDENSADA/NEGRITO", CmdTamanhoFonte.Condensada, CmdEstiloFonte.Negrito);
        regiao.ImprimirTexto("EXPANDIDO", CmdTamanhoFonte.Expandida);
        regiao.ImprimirTexto("INVERTIDA", CmdEstiloFonte.Invertido);
        regiao.PularLinhas(1);
        regiao.ImprimirBarcode("1234567890", CmdBarcode.Inter2of5, CmdAlinhamento.Esquerda, CmdBarcodeText.SemTexto, 2, 40);
        regiao.ImprimirQrCode("https://github.com/OpenAC-Net/OpenAC.Net.EscPos", CmdAlinhamento.Centro);

        posprinter.CortarPapel();
    }

    private static void PopulateCompleteSampleReceipt(EscPosPrinter posprinter)
    {
        posprinter.ImprimirTexto("OPENAC .NET ESCPOS", CmdTamanhoFonte.Expandida, CmdAlinhamento.Centro, CmdEstiloFonte.Negrito);
        posprinter.ImprimirTexto("Pré-visualização de Cupom Térmico", CmdAlinhamento.Centro);
        posprinter.ImprimirLinha(true);
        posprinter.ImprimirTexto("TESTE NORMAL ESQUERDA", CmdAlinhamento.Esquerda);
        posprinter.ImprimirTexto("TESTE NORMAL CENTRO", CmdAlinhamento.Centro);
        posprinter.ImprimirTexto("TESTE NORMAL DIREITA", CmdAlinhamento.Direita);
        posprinter.ImprimirLinha();
        posprinter.ImprimirTexto("TESTE EXPANDIDO", CmdFonte.Normal, CmdTamanhoFonte.Expandida, CmdAlinhamento.Esquerda, null);
        posprinter.ImprimirTexto("TESTE CONDENSADO", CmdFonte.Normal, CmdTamanhoFonte.Condensada, CmdAlinhamento.Esquerda, null);
        posprinter.ImprimirTexto("TESTE ALTURA DUPLA", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, CmdEstiloFonte.AlturaDupla);
        posprinter.ImprimirTexto("TESTE INVERTIDO", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Centro, CmdEstiloFonte.Invertido);
        posprinter.ImprimirLinha();
        posprinter.ImprimirTexto(
            new TextSlice("Negrito ", CmdEstiloFonte.Negrito),
            new TextSlice("Itálico ", CmdEstiloFonte.Italico),
            new TextSlice("Sublinhado", CmdEstiloFonte.Sublinhado));
        posprinter.ImprimirLinha();
        posprinter.ImprimirBarcode("7891234567895", CmdBarcode.EAN13, CmdAlinhamento.Centro, CmdBarcodeText.Abaixo);
        posprinter.ImprimirLinha();
        posprinter.ImprimirQrCode("https://github.com/OpenAC-Net/OpenAC.Net.EscPos", tamanho: QrCodeModSize.Normal, aAlinhamento: CmdAlinhamento.Centro);
        posprinter.CortarPapel(true);
    }

    private void PrintTextTest()
    {
        using var posprinter = Dispatcher.UIThread.Invoke(GetPosPrinter);
        posprinter.Conectar();
        PopulateTextCommands(posprinter);
        posprinter.Imprimir();
        Dispatcher.UIThread.Post(() =>
        {
            ResultText.Text = "Comandos de impressão de texto enviados com sucesso.";
            ResponseTabs.SelectedIndex = 1;
        });
    }

    private void PrintBarcodeTest()
    {
        using var posprinter = Dispatcher.UIThread.Invoke(GetPosPrinter);
        posprinter.Conectar();
        PopulateBarcodeCommands(posprinter);
        posprinter.Imprimir();
        Dispatcher.UIThread.Post(() =>
        {
            ResultText.Text = "Comandos de código de barras enviados com sucesso.";
            ResponseTabs.SelectedIndex = 1;
        });
    }

    private void PrintQrCodeTest()
    {
        using var posprinter = Dispatcher.UIThread.Invoke(GetPosPrinter);
        posprinter.Conectar();
        PopulateQrCodeCommands(posprinter);
        posprinter.Imprimir();
        Dispatcher.UIThread.Post(() =>
        {
            ResultText.Text = "Comandos de QR Code enviados com sucesso.";
            ResponseTabs.SelectedIndex = 1;
        });
    }

    private void PrintPageModeTest()
    {
        using var posprinter = Dispatcher.UIThread.Invoke(GetPosPrinter);
        posprinter.Conectar();
        PopulatePageModeCommands(posprinter);
        posprinter.Imprimir();
        Dispatcher.UIThread.Post(() =>
        {
            ResultText.Text = "Comandos de Modo Página enviados com sucesso.";
            ResponseTabs.SelectedIndex = 1;
        });
    }

    private async Task PrintImageTestAsync()
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Selecionar imagem para impressão",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("Imagens") { Patterns = ["*.png", "*.bmp", "*.jpg", "*.jpeg", "*.gif"] },
                new FilePickerFileType("Todos os arquivos") { Patterns = ["*.*"] }
            ]
        });

        var file = files.FirstOrDefault();
        if (file == null) return;

        var path = file.TryGetLocalPath();
        if (string.IsNullOrEmpty(path)) return;

        await Task.Run(() =>
        {
            var resized = File.ReadAllBytes(path).ResizeImage(300, 300);
            using var posprinter = Dispatcher.UIThread.Invoke(GetPosPrinter);

            posprinter.Conectar();
            posprinter.ImprimirImagem(resized, CmdAlinhamento.Centro);
            posprinter.PularLinhas(5);
            posprinter.CortarPapel();
            posprinter.Imprimir();
        });

        ResultText.Text = $"Imagem '{file.Name}' enviada para impressão com sucesso.";
        ResponseTabs.SelectedIndex = 1;
    }

    private void OpenCashDrawerTest()
    {
        using var posprinter = Dispatcher.UIThread.Invoke(GetPosPrinter);
        posprinter.Conectar();
        posprinter.AbrirGaveta();
        posprinter.Imprimir();
        Dispatcher.UIThread.Post(() =>
        {
            ResultText.Text = "Comando de abertura de gaveta enviado com sucesso.";
            ResponseTabs.SelectedIndex = 1;
        });
    }

    private void CutPaperTest()
    {
        using var posprinter = Dispatcher.UIThread.Invoke(GetPosPrinter);
        posprinter.Conectar();
        posprinter.CortarPapel(true);
        posprinter.Imprimir();
        Dispatcher.UIThread.Post(() =>
        {
            ResultText.Text = "Comando de corte de papel enviado com sucesso.";
            ResponseTabs.SelectedIndex = 1;
        });
    }

    private void BeepTest()
    {
        using var posprinter = Dispatcher.UIThread.Invoke(GetPosPrinter);
        posprinter.Conectar();
        posprinter.Beep();
        posprinter.Imprimir();
        Dispatcher.UIThread.Post(() =>
        {
            ResultText.Text = "Comando de beep enviado com sucesso.";
            ResponseTabs.SelectedIndex = 1;
        });
    }

    private void ReadStatus()
    {
        using var posprinter = Dispatcher.UIThread.Invoke(GetPosPrinter);
        posprinter.Conectar();
        var status = posprinter.LerStatusImpressora();

        Dispatcher.UIThread.Post(() =>
        {
            ResultText.Text = $"Status da Impressora ({posprinter.Protocolo}):\n\n{status}";
            ResponseTabs.SelectedIndex = 1;
        });
    }

    private void ReadInfo()
    {
        using var posprinter = Dispatcher.UIThread.Invoke(GetPosPrinter);
        posprinter.Conectar();
        var info = posprinter.LerInfoImpressora();

        Dispatcher.UIThread.Post(() =>
        {
            var texto = info?.ToString() ?? "Nenhuma informação retornada pela impressora.";
            ResultText.Text = $"Informações da Impressora ({posprinter.Protocolo}):\n\n{texto}";
            AppendLog(texto);
            ResponseTabs.SelectedIndex = 1;
        });
    }

    private Task GeneratePreviewAsync()
    {
        using var posprinter = GetPosPrinter();
        var template = Selected<PreviewTemplate>(PreviewTemplateCombo);

        switch (template)
        {
            case PreviewTemplate.TesteTextos:
                PopulateTextCommands(posprinter);
                break;

            case PreviewTemplate.CodigoBarras:
                PopulateBarcodeCommands(posprinter);
                break;

            case PreviewTemplate.QrCodes:
                PopulateQrCodeCommands(posprinter);
                break;

            case PreviewTemplate.ModoPagina:
                PopulatePageModeCommands(posprinter);
                break;

            default:
                PopulateCompleteSampleReceipt(posprinter);
                break;
        }

        currentPreviewPngBytes = posprinter.GerarPreview(limparBuffer: true);

        using var ms = new MemoryStream(currentPreviewPngBytes);
        var avaloniaBmp = new AvaloniaBitmap(ms);

        currentPreviewBitmap?.Dispose();
        currentPreviewBitmap = avaloniaBmp;

        PreviewImage.Source = currentPreviewBitmap;
        PreviewEmptyState.IsVisible = false;
        ResultText.Text = $"Preview gerado com sucesso!\nModelo: {template}\nProtocolo: {posprinter.Protocolo}\nDimensões: {avaloniaBmp.PixelSize.Width} x {avaloniaBmp.PixelSize.Height} px";
        ResponseTabs.SelectedIndex = 0;

        return Task.CompletedTask;
    }

    private async Task SavePreviewAsync()
    {
        if (currentPreviewPngBytes == null || currentPreviewPngBytes.Length == 0)
            await GeneratePreviewAsync();

        if (currentPreviewPngBytes == null) return;

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Salvar imagem de preview do cupom",
            DefaultExtension = "png",
            SuggestedFileName = $"preview_escpos_{DateTime.Now:yyyyMMdd_HHmmss}.png",
            FileTypeChoices =
            [
                new FilePickerFileType("Imagem PNG") { Patterns = ["*.png"] }
            ]
        });

        if (file == null) return;

        await using var stream = await file.OpenWriteAsync();
        await stream.WriteAsync(currentPreviewPngBytes);

        ResultText.Text = $"Preview salvo com sucesso em:\n{file.TryGetLocalPath() ?? file.Name}";
    }

    private async Task RunAsync(string name, Func<Task> action)
    {
        Workspace.IsEnabled = false;
        Navigation.IsEnabled = false;
        BusyProgress.IsVisible = true;
        StatusText.Text = $"{name}…";

        try
        {
            await action();
            StatusText.Text = "Pronto para operar";
        }
        catch (Exception ex)
        {
            StatusText.Text = "Não foi possível concluir a operação";
            ResultText.Text = $"Erro em '{name}':\n\n{ex.Message}";
            ResponseTabs.SelectedIndex = 1;
            AppendLog($"{name}: {ex.Message}");
        }
        finally
        {
            LastOperationText.Text = $"{name} · {DateTime.Now:HH:mm:ss}";
            Workspace.IsEnabled = true;
            Navigation.IsEnabled = true;
            BusyProgress.IsVisible = false;
        }
    }

    private void AppendLog(string message)
    {
        if (closed) return;
        var lines = (LogText.Text + $"{DateTime.Now:HH:mm:ss}  {message}\n").Split('\n');
        LogText.Text = string.Join('\n', lines.TakeLast(500));
    }

    private void InitializeLogging()
    {
        var logConfig = new LoggingConfiguration();
        logConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, new DemoLogTarget(line =>
            Dispatcher.UIThread.Post(() => AppendLog(line)))
        {
            Layout = "${message}"
        });
        logConfig.AddRule(LogLevel.Debug, LogLevel.Fatal, new FileTarget("infoFile")
        {
            FileName = Path.Combine(AppContext.BaseDirectory, "Logs", "EscPos.log"),
            Layout = "${processid}|${longdate}|${level:uppercase=true}|${logger}|${message}",
            CreateDirs = true,
            Encoding = Encoding.UTF8,
            MaxArchiveFiles = 30,
            ArchiveEvery = FileArchivePeriod.Day
        });
        LogManager.Configuration = logConfig;
    }

    private static string GetComboText(ComboBox combo, string fallback)
    {
        if (!string.IsNullOrWhiteSpace(combo.Text))
            return combo.Text;
        if (combo.SelectedItem is string s && !string.IsNullOrWhiteSpace(s))
            return s;
        return fallback;
    }

    private static void Fill<T>(ComboBox combo, IEnumerable<T> values, T selected, Func<T, string>? label = null)
    {
        var choices = values.Select(value => new Choice<T>(value, label?.Invoke(value) ?? value?.ToString() ?? string.Empty)).ToArray();
        combo.ItemsSource = choices;
        combo.SelectedItem = choices.FirstOrDefault(choice => EqualityComparer<T>.Default.Equals(choice.Value, selected)) ?? choices.First();
    }

    private static void SelectValue<T>(ComboBox combo, T value)
    {
        if (combo.ItemsSource is IEnumerable<Choice<T>> choices)
        {
            var match = choices.FirstOrDefault(c => EqualityComparer<T>.Default.Equals(c.Value, value));
            if (match != null)
                combo.SelectedItem = match;
        }
    }

    private static T Selected<T>(ComboBox combo) => ((Choice<T>)combo.SelectedItem!).Value;

    private sealed record Choice<T>(T Value, string Label)
    {
        public override string ToString() => Label;
    }
}

internal sealed class DemoLogTarget(Action<string> write) : TargetWithLayout
{
    protected override void Write(LogEventInfo logEvent) => write(Layout.Render(logEvent));
}
