using System.Drawing.Printing;
using System.IO.Ports;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Windows.Forms;
using OpenAC.Net.Core;
using OpenAC.Net.Devices;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Demo.Commom;

namespace OpenAC.Net.EscPos.Demo
{
    public partial class Form1 : Form
    {
        #region Fields

        private readonly OpenConfig config;

        #endregion Fields

        #region Constructors

        public Form1()
        {
            InitializeComponent();

            // .Net 5 e 6 n„o tem todos os CodePages.
            // Ent„o tem que instalar o pacote System.Text.Encoding.CodePages.
            // Adiciona esta linha ao programa antes de usar o EscPos.
            // Isso so precisa ser feito 1 vez ent„o faÁa na inializaÁ„o do seu programa.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            tbcDeviceConfig.HideTabHeaders();

            config = OpenConfig.CreateOrLoad(Path.Combine(Application.StartupPath, "escpos.config"));
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            InitializeLog();

            cbbConexao.EnumDataSource(TipoConexao.Serial);
            cbbProtocolo.EnumDataSource(ProtocoloEscPos.EscPos);
            cbbEnconding.EnumDataSource(PaginaCodigo.pc850);
            cbbPortas.SetDataSource(SerialPort.GetPortNames());
            cbbImpressoras.SetDataSource(PrinterSettings.InstalledPrinters.Cast<string>());
            cbbVelocidade.EnumDataSource(SerialBaud.Bd9600);
            cbbDataBits.EnumDataSource(SerialDataBits.Db8);
            cbbParity.EnumDataSource(Parity.None);
            cbbStopBits.EnumDataSource(StopBits.None);
            cbbHandshake.EnumDataSource(Handshake.None);

            LoadConfig();
        }

        #endregion Constructors

        #region Methods

        private EscPosPrinter GetPosPrinter()
        {
            var tipo = cbbConexao.GetSelectedValue<TipoConexao>();

            EscPosPrinter ret = tipo switch
            {
                TipoConexao.Serial => new EscPosPrinter<SerialConfig>()
                {
                    Device =
                    {
                        Porta = cbbPortas.GetSelectedValue<string>(),
                        Baud = (int)cbbVelocidade.GetSelectedValue<SerialBaud>(),
                        DataBits = (int)cbbDataBits.GetSelectedValue<SerialDataBits>(),
                        Parity = cbbParity.GetSelectedValue<Parity>(),
                        StopBits = cbbStopBits.GetSelectedValue<StopBits>(),
                        Handshake = cbbHandshake.GetSelectedValue<Handshake>(),
                    }
                },
                TipoConexao.TCP => new EscPosPrinter<TCPConfig>()
                {
                    Device =
                    {
                        IP = txtIP.Text,
                        Porta = (int)nudPorta.Value
                    }
                },
                TipoConexao.RAW => new EscPosPrinter<RawConfig>()
                {
                    Device =
                    {
                        Impressora = cbbImpressoras.GetSelectedValue<string>(),
                    }
                },
                TipoConexao.File => new EscPosPrinter<FileConfig>()
                {
                    Device =
                    {
                        CreateIfNotExits = true,
                        File = txtArquivo.Text,
                    }
                },
                _ => throw new ArgumentOutOfRangeException()
            };

            ret.Protocolo = cbbProtocolo.GetSelectedValue<ProtocoloEscPos>();
            ret.PaginaCodigo = cbbEnconding.GetSelectedValue<PaginaCodigo>();
            ret.Device.ControlePorta = chkControlePortas.Checked;
            ret.EspacoEntreLinhas = (byte)nudEspacos.Value;
            ret.LinhasEntreCupons = (byte)nudLinhas.Value;

            return ret;
        }

        private void InitializeLog()
        {
            var config = new LoggingConfiguration();
            var target = new RichTextBoxTarget
            {
                UseDefaultRowColoringRules = true,
                Layout = @"${date:format=dd/MM/yyyy HH\:mm\:ss} - ${message}",
                FormName = Name,
                ControlName = rtbLog.Name,
                AutoScroll = true
            };

            config.AddTarget("RichTextBox", target);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, target));

            var infoTarget = new FileTarget
            {
                FileName = "${basedir:dir=Logs:file=ACBrNFSe.log}",
                Layout = "${processid}|${longdate}|${level:uppercase=true}|" +
                         "${event-context:item=Context}|${logger}|${message}",
                CreateDirs = true,
                Encoding = Encoding.UTF8,
                MaxArchiveFiles = 93,
                ArchiveEvery = FileArchivePeriod.Day,
                ArchiveNumbering = ArchiveNumberingMode.Date,
                ArchiveFileName = "${basedir}/Logs/Archive/${date:format=yyyy}/${date:format=MM}/EscPos_{{#}}.log",
                ArchiveDateFormat = "dd.MM.yyyy"
            };

            config.AddTarget("infoFile", infoTarget);
            config.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, infoTarget));
            LogManager.Configuration = config;
        }

        private void SaveConfig()
        {
            config.Set("ProtocoloEscPos", cbbProtocolo.GetSelectedValue<ProtocoloEscPos>());
            config.Set("TipoConexao", cbbConexao.GetSelectedValue<TipoConexao>());
            config.Set("PaginaCodigo", cbbEnconding.GetSelectedValue<PaginaCodigo>());
            config.Set("ControlePorta", chkControlePortas.Checked);
            config.Set("EspacoEntreLinhas", nudEspacos.Value);
            config.Set("LinhasEntreCupom", nudLinhas.Value);

            //Serial
            config.Set("SerialPorta", cbbPortas.GetSelectedValue<string>());
            config.Set("SerialVelocidade", cbbVelocidade.GetSelectedValue<SerialBaud>());
            config.Set("SerialDataBits", cbbDataBits.GetSelectedValue<SerialDataBits>());
            config.Set("SerialParaty", cbbParity.GetSelectedValue<Parity>());
            config.Set("SerialStopBits", cbbStopBits.GetSelectedValue<StopBits>());
            config.Set("SerialHandhsake", cbbHandshake.GetSelectedValue<Handshake>());

            //TCP
            config.Set("TCPIP", txtIP.Text);
            config.Set("TCPPorta", nudPorta.Value);

            //Raw
            config.Set("RawImpressora", cbbImpressoras.GetSelectedValue<string>());

            //File
            config.Set("FileArquivo", txtArquivo.Text);

            config.Save();
        }

        private void LoadConfig()
        {
            cbbProtocolo.SetSelectedValue(config.Get("ProtocoloEscPos", ProtocoloEscPos.EscPos));
            cbbConexao.SetSelectedValue(config.Get("TipoConexao", TipoConexao.Serial));
            cbbEnconding.SetSelectedValue(config.Get("PaginaCodigo", PaginaCodigo.pc850));
            chkControlePortas.Checked = config.Get("ControlePorta", true);
            nudEspacos.Value = config.Get("EspacoEntreLinhas", 0M);
            nudLinhas.Value = config.Get("LinhasEntreCupom", 0M);

            //Serial
            cbbPortas.SetSelectedValue(config.Get("SerialPorta", cbbPortas.GetSelectedValue<string>()));
            cbbVelocidade.SetSelectedValue(config.Get("SerialVelocidade", SerialBaud.Bd9600));
            cbbDataBits.SetSelectedValue(config.Get("SerialDataBits", SerialDataBits.Db8));
            cbbParity.SetSelectedValue(config.Get("SerialParaty", Parity.None));
            cbbStopBits.SetSelectedValue(config.Get("SerialStopBits", StopBits.None));
            cbbHandshake.SetSelectedValue(config.Get("SerialHandhsake", Handshake.None));

            //TCP
            txtIP.Text = config.Get("TCPIP", "");
            nudPorta.Value = config.Get("TCPPorta", 9100M);

            //Raw
            cbbImpressoras.SetSelectedValue(config.Get("RawImpressora", cbbImpressoras.GetSelectedValue<string>()));

            //File
            txtArquivo.Text = config.Get("FileArquivo", "");
        }

        #endregion Methods

        #region EventHandlers

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            txtArquivo.Text = Helpers.OpenFile("txt files (*.txt)|*.txt|All files (*.*)|*.*");
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }

        private void cbbConexao_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tipo = cbbConexao.GetSelectedValue<TipoConexao>();
            tbcDeviceConfig.SelectedTab = tipo switch
            {
                TipoConexao.Serial => tbpSerial,
                TipoConexao.TCP => tbpTCP,
                TipoConexao.RAW => tbpRAW,
                TipoConexao.File => tbpFile,
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        private void btnTxt_Click(object sender, EventArgs e)
        {
            using var posprinter = GetPosPrinter();

            posprinter.LinhasEntreCupons = 4;

            posprinter.Conectar();

            posprinter.ImprimirTexto("¡…Õ”⁄·ÈÌÛ˙Á«„ı√’ Í¿‡", CmdAlinhamento.Centro);
            posprinter.ImprimirTexto("TESTE NORMAL ESQUERDA", CmdAlinhamento.Esquerda);
            posprinter.ImprimirTexto("TESTE NORMAL DIREITA", CmdAlinhamento.Direita);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirTexto("TESTE EXPANDIDO", CmdFonte.Normal, CmdTamanhoFonte.Expandida, CmdAlinhamento.Esquerda, null);
            posprinter.ImprimirTexto("TESTE CONDENSADO", CmdFonte.Normal, CmdTamanhoFonte.Condensada, CmdAlinhamento.Esquerda, null);
            posprinter.ImprimirTexto("TESTE NORMAL", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, null);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirTexto("TESTE DUPLO EXPANDIDO", CmdFonte.Normal, CmdTamanhoFonte.Expandida, CmdAlinhamento.Esquerda, CmdEstiloFonte.AlturaDupla);
            posprinter.ImprimirTexto("TESTE DUPLO CONDESADO", CmdFonte.Normal, CmdTamanhoFonte.Condensada, CmdAlinhamento.Esquerda, CmdEstiloFonte.AlturaDupla);
            posprinter.ImprimirTexto("TESTE DUPLO NORMAL", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, CmdEstiloFonte.AlturaDupla);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirTexto("TESTE NEGRITO", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, CmdEstiloFonte.Negrito);
            posprinter.ImprimirTexto("TESTE ITALICO", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, CmdEstiloFonte.Italico);
            posprinter.ImprimirTexto("TESTE SUBLINHADO", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, CmdEstiloFonte.Sublinhado);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirTexto("TESTE FONTE NORMAL", CmdFonte.Normal, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, null);
            posprinter.ImprimirTexto("TESTE FONTE A", CmdFonte.FonteA, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, null);
            posprinter.ImprimirTexto("TESTE FONTE B", CmdFonte.FonteB, CmdTamanhoFonte.Normal, CmdAlinhamento.Esquerda, null);

            posprinter.CortarPapel();
            posprinter.Imprimir();
        }

        private void btnCodBar_Click(object sender, EventArgs e)
        {
            using var posprinter = GetPosPrinter();

            posprinter.LinhasEntreCupons = 4;

            posprinter.Conectar();

            posprinter.ImprimirBarcode("ABCDE12345", CmdBarcode.CODE39, CmdAlinhamento.Centro, CmdBarcodeText.Acima);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirBarcode("1234567890", CmdBarcode.Inter2of5, CmdAlinhamento.Centro, CmdBarcodeText.Ambos);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirBarcode("$-=+ABC123abc", CmdBarcode.CODE128, CmdAlinhamento.Centro, CmdBarcodeText.Abaixo);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirBarcode("3515071111111111111159", CmdBarcode.CODE128c, CmdAlinhamento.Centro, CmdBarcodeText.Abaixo);

            posprinter.CortarPapel();
            posprinter.Imprimir();
        }

        private void btnQrCode_Click(object sender, EventArgs e)
        {
            using var posprinter = GetPosPrinter();

            posprinter.LinhasEntreCupons = 4;

            posprinter.Conectar();

            string qrCode = "https://www.homologacao.nfce.fazenda.sp.gov.br/NFCeConsultaPublica/Paginas/ConsultaQRCode.aspx?chNFe=3515080548133600013765022000" +
                "0000711000001960&nVersao=100&tpAmb=2&dhEmi=323031352D30382D31395432323A33333A32352D30333A3030&vNF=3.00&vICMS=0.12&digVal=77696739" +
                "6F2B665861706673396878776E64594C396F61654C35493D&cIdToken=000001&cHashQRCode=9BD312D558823E1EC68CEDB338A39B6150B0480E";

            // QrCode NFCe
            posprinter.ImprimirTexto("Exemplo QrCode NFCe.", CmdEstiloFonte.Negrito);
            posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Minusculo, aAlinhamento: CmdAlinhamento.Centro);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Pequeno, aAlinhamento: CmdAlinhamento.Centro);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Normal, aAlinhamento: CmdAlinhamento.Centro);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Grande, aAlinhamento: CmdAlinhamento.Centro);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.ExtraGrande, aAlinhamento: CmdAlinhamento.Centro);
            posprinter.ImprimirLinha(48);

            posprinter.PularLinhas(2);

            // QrCode SAT
            qrCode = "35150811111111111111591234567890001672668828|20150820201736|118.72|05481336000137|TCbeD81ePUpMvso4VjFqRTvs4ov" +
                "qmR1ZG3bwSCumzHtW8bbMedVJjVnww103v3LxKfgckAyuizcR/9pXaKay6M4Gu8kyDef+6VH5qONIZV1cB+mFfXiaCgeZALuRDCH1PRyb6hoB" +
                "eRUkUk6lOdXSczRW9Y83GJMXdOFroEbzFmpf4+WOhe2BZ3mEdXKKGMfl1EB0JWnAThkGT+1Er9Jh/3En5YI4hgQP3NC2BiJVJ6oCEbKb85s59" +
                "15DSZAw4qB/MlESWViDsDVYEnS/FQgA2kP2A9pR4+agdHmgWiz30MJYqX5Ng9XEYvvOMzl1Y6+7/frzsocOxfuQyFsnfJzogw==";

            posprinter.ImprimirTexto("Exemplo QrCode SAT.", CmdEstiloFonte.Negrito);
            posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Minusculo, aAlinhamento: CmdAlinhamento.Centro);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Pequeno, aAlinhamento: CmdAlinhamento.Centro);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Normal, aAlinhamento: CmdAlinhamento.Centro);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.Grande, aAlinhamento: CmdAlinhamento.Centro);
            posprinter.ImprimirLinha(48);
            posprinter.ImprimirQrCode(qrCode, tamanho: QrCodeModSize.ExtraGrande, aAlinhamento: CmdAlinhamento.Centro);

            posprinter.CortarPapel();
            posprinter.Imprimir();
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            using var posprinter = GetPosPrinter();

            posprinter.Conectar();

            var status = posprinter.LerStatusImpressora();
            MessageBox.Show(status.ToString());
        }

        private void btnImagem_Click(object sender, EventArgs e)
        {
            using var ofd = new OpenFileDialog();
            ofd.CheckPathExists = true;
            ofd.CheckFileExists = true;
            ofd.Multiselect = false;
            ofd.Filter = "";

            if (ofd.ShowDialog().Equals(DialogResult.Cancel)) return;

            var img = Image.FromFile(ofd.FileName);

            using var posprinter = GetPosPrinter();

            posprinter.Conectar();

            posprinter.ImprimirImagem(img);
            posprinter.CortarPapel();
            posprinter.Imprimir();
        }

        private void txtModoPagina_Click(object sender, EventArgs e)
        {
            using var posprinter = GetPosPrinter();

            posprinter.Conectar();
            posprinter.CodigoBarras.Altura = 40;
            posprinter.CodigoBarras.Largura = 2;
            posprinter.CodigoBarras.Exibir = CmdBarcodeText.SemTexto;

            var modoPagina = posprinter.IniciarModoPagina();

            //Regi„o 1
            var regiao = modoPagina.NovaRegiao(0, 0, 257, 740);
            regiao.Direcao = CmdPosDirecao.EsquerdaParaDireita;
            regiao.EspacoEntreLinhas = 25;
            regiao.ImprimirTexto("CONDENSADA/NEGRITO", CmdTamanhoFonte.Condensada, CmdEstiloFonte.Negrito);
            regiao.ImprimirTexto("EXPANDIDO", CmdTamanhoFonte.Expandida);
            regiao.ImprimirTexto("INVERTIDA", CmdEstiloFonte.Invertido);
            regiao.PularLinhas(1);
            regiao.ImprimirBarcode("1234567890", CmdBarcode.Inter2of5, CmdBarcodeText.SemTexto);
            regiao.ImprimirQrCode("https://github.com/OpenAC-Net/OpenAC.Net.EscPos", CmdAlinhamento.Centro);

            //Regi„o 2
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

            //Regi„o 3
            regiao = modoPagina.NovaRegiao(0, 0, 257, 740);
            regiao.Direcao = CmdPosDirecao.BaixoParaTopo;
            regiao.EspacoEntreLinhas = 25;
            regiao.ImprimirTexto("CONDENSADA/NEGRITO", CmdTamanhoFonte.Condensada, CmdEstiloFonte.Negrito);
            regiao.ImprimirTexto("EXPANDIDO", CmdTamanhoFonte.Expandida);
            regiao.ImprimirTexto("INVERTIDA", CmdEstiloFonte.Invertido);
            regiao.PularLinhas(1);
            regiao.ImprimirBarcode("1234567890", CmdBarcode.Inter2of5, CmdAlinhamento.Esquerda, CmdBarcodeText.SemTexto, 2, 40);
            regiao.ImprimirQrCode("https://github.com/OpenAC-Net/OpenAC.Net.EscPos", CmdAlinhamento.Centro);

            //Regi„o 4
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
            posprinter.Imprimir();
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            using var posprinter = GetPosPrinter();

            posprinter.Conectar();
            var info = posprinter.LerInfoImpressora();
            rtbLog.AppendText(info.ToString());
        }

        #endregion EventHandlers
    }
}