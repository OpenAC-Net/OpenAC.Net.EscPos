using System.Drawing.Printing;
using System.IO.Ports;
using System.Text;
using OpenAC.Net.Core;
using OpenAC.Net.EscPos.Commom;
using OpenAC.Net.EscPos.Demo.Commom;

namespace OpenAC.Net.EscPos.Demo
{
    public partial class Form1 : Form
    {
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
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
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
        }

        #endregion Constructors

        #region Methods

        private EscPosPrinter GetPosPrinter()
        {
            var tipo = cbbConexao.GetSelectedValue<TipoConexao>();
            var protocolo = cbbProtocolo.GetSelectedValue<ProtocoloEscPos>();
            var paginaCodigo = cbbEnconding.GetSelectedValue<PaginaCodigo>();

            var encoding = paginaCodigo switch
            {
                PaginaCodigo.pc437 => Encoding.GetEncoding(437),
                PaginaCodigo.pc850 => OpenEncoding.IBM850,
                PaginaCodigo.pc852 => Encoding.GetEncoding("IBM852"),
                PaginaCodigo.pc860 => OpenEncoding.IBM860,
                PaginaCodigo.pcUTF8 => Encoding.UTF8,
                PaginaCodigo.pc1252 => OpenEncoding.CP1252,
                _ => throw new ArgumentOutOfRangeException()
            };

            EscPosPrinter ret = tipo switch
            {
                TipoConexao.Serial => EscPosPrinterFactory.CreateSerial(o =>
                {
                    o.Device.Porta = cbbPortas.GetSelectedValue<string>();
                    o.Device.Baud = (int)cbbVelocidade.GetSelectedValue<SerialBaud>();
                    o.Device.DataBits = (int)cbbDataBits.GetSelectedValue<SerialDataBits>();
                    o.Device.Parity = cbbParity.GetSelectedValue<Parity>();
                    o.Device.StopBits = cbbStopBits.GetSelectedValue<StopBits>();
                    o.Device.Handshake = cbbHandshake.GetSelectedValue<Handshake>();
                }),
                TipoConexao.TCP => EscPosPrinterFactory.CreateTCP(o =>
                {
                    o.Device.IP = txtIP.Text;
                    o.Device.Porta = (int)nudPorta.Value;
                }),
                TipoConexao.RAW => EscPosPrinterFactory.CreateRaw(o =>
                {
                    o.Device.Impressora = cbbImpressoras.GetSelectedValue<string>();
                }),
                TipoConexao.File => EscPosPrinterFactory.CreateFile(o =>
                {
                    o.Device.CreateIfNotExits = true;
                    o.Device.File = txtArquivo.Text;
                }),
                _ => throw new ArgumentOutOfRangeException()
            };

            ret.Protocolo = protocolo;
            ret.Encoder = encoding;
            ret.Device.ControlePorta = chkControlePortas.Checked;
            ret.EspacoEntreLinhas = (byte)nudEspacos.Value;
            ret.LinhasEntreCupons = (byte)nudLinhas.Value;

            return ret;
        }

        #endregion Methods

        #region EventHandlers

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

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            txtArquivo.Text = Helpers.OpenFile("txt files (*.txt)|*.txt|All files (*.*)|*.*");
        }

        private void btnTxt_Click(object sender, EventArgs e)
        {
            using var posprinter = GetPosPrinter();

            posprinter.LinhasEntreCupons = 4;

            posprinter.Conectar();

            posprinter.ImprimirTexto("¡…Õ”⁄·ÈÌÛ˙Á«„ı√’ Í¿‡", CmdAlinhamento.Centro);

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

            // QrCode NFCe
            posprinter.ImprimirTexto("Exemplo QrCode NFCe.", CmdEstiloFonte.Negrito);
            posprinter.ImprimirQrCode("https://www.homologacao.nfce.fazenda.sp.gov.br/NFCeConsultaPublica/Paginas/ConsultaQRCode.aspx?chNFe=3515080548133600013765022000" +
                                      "0000711000001960&nVersao=100&tpAmb=2&dhEmi=323031352D30382D31395432323A33333A32352D30333A3030&vNF=3.00&vICMS=0.12&digVal=77696739" +
                                      "6F2B665861706673396878776E64594C396F61654C35493D&cIdToken=000001&cHashQRCode=9BD312D558823E1EC68CEDB338A39B6150B0480E", CmdAlinhamento.Centro);

            posprinter.ImprimirLinha(48);

            // QrCode SAT
            posprinter.ImprimirTexto("Exemplo QrCode SAT.", CmdEstiloFonte.Negrito);
            posprinter.ImprimirQrCode("35150811111111111111591234567890001672668828|20150820201736|118.72|05481336000137|TCbeD81ePUpMvso4VjFqRTvs4ov" +
                                      "qmR1ZG3bwSCumzHtW8bbMedVJjVnww103v3LxKfgckAyuizcR/9pXaKay6M4Gu8kyDef+6VH5qONIZV1cB+mFfXiaCgeZALuRDCH1PRyb6hoB" +
                                      "eRUkUk6lOdXSczRW9Y83GJMXdOFroEbzFmpf4+WOhe2BZ3mEdXKKGMfl1EB0JWnAThkGT+1Er9Jh/3En5YI4hgQP3NC2BiJVJ6oCEbKb85s59" +
                                      "15DSZAw4qB/MlESWViDsDVYEnS/FQgA2kP2A9pR4+agdHmgWiz30MJYqX5Ng9XEYvvOMzl1Y6+7/frzsocOxfuQyFsnfJzogw==", CmdAlinhamento.Centro);

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

        #endregion EventHandlers
    }
}