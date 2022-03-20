using System.Text;
using OpenAC.Net.EscPos.Commom;

namespace OpenAC.Net.EscPos.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // .Net 5 e 6 n„o tem todos os CodePages.
            // Ent„o tem que instalar o pacote System.Text.Encoding.CodePages.
            // Adiciona esta linha ao programa antes de usar o EscPos.
            // Isso so precisa ser feito 1 vez ent„o faÁa na inializaÁ„o do seu programa.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using var posprinter = EscPosPrinterFactory.CreateTCP(ProtocoloEscPos.Epson, o =>
            {
                o.ControlePorta = true;
                o.IP = "192.168.0.10";
            });

            //using var posprinter = EscPosPrinterFactory.CreateSerial(ProtocoloEscPos.Epson, o =>
            //{
            //    o.ControlePorta = true;
            //    o.Porta = "COM5";
            //});

            posprinter.LinhasEntreCupons = 4;

            posprinter.Conectar();
            //var status = posprinter.LerStatusImpressora();
            //MessageBox.Show(status.ToString());

            // QrCode NFCe
            posprinter.ImprimirQrCode("https://www.homologacao.nfce.fazenda.sp.gov.br/NFCeConsultaPublica/Paginas/ConsultaQRCode.aspx?chNFe=3515080548133600013765022000" +
                                      "0000711000001960&nVersao=100&tpAmb=2&dhEmi=323031352D30382D31395432323A33333A32352D30333A3030&vNF=3.00&vICMS=0.12&digVal=77696739" +
                                      "6F2B665861706673396878776E64594C396F61654C35493D&cIdToken=000001&cHashQRCode=9BD312D558823E1EC68CEDB338A39B6150B0480E", CmdAlinhamento.Centro);

            posprinter.ImprimirLinha();

            // QrCode SAT
            posprinter.ImprimirQrCode("35150811111111111111591234567890001672668828|20150820201736|118.72|05481336000137|TCbeD81ePUpMvso4VjFqRTvs4ov" +
                                      "qmR1ZG3bwSCumzHtW8bbMedVJjVnww103v3LxKfgckAyuizcR/9pXaKay6M4Gu8kyDef+6VH5qONIZV1cB+mFfXiaCgeZALuRDCH1PRyb6hoB" +
                                      "eRUkUk6lOdXSczRW9Y83GJMXdOFroEbzFmpf4+WOhe2BZ3mEdXKKGMfl1EB0JWnAThkGT+1Er9Jh/3En5YI4hgQP3NC2BiJVJ6oCEbKb85s59" +
                                      "15DSZAw4qB/MlESWViDsDVYEnS/FQgA2kP2A9pR4+agdHmgWiz30MJYqX5Ng9XEYvvOMzl1Y6+7/frzsocOxfuQyFsnfJzogw==", CmdAlinhamento.Centro);

            //posprinter.ImprimirTexto("¡…Õ”⁄·ÈÌÛ˙Á«„ı√’ Í¿‡", CmdAlinhamento.Centro);
            //posprinter.ImprimirLinha();
            //posprinter.ImprimirBarcode("ABCDE12345", CmdBarcode.CODE39, CmdAlinhamento.Centro, CmdBarcodeText.Acima);
            //posprinter.ImprimirLinha();
            //posprinter.ImprimirBarcode("1234567890", CmdBarcode.Inter2of5, CmdAlinhamento.Centro, CmdBarcodeText.Ambos);
            //posprinter.ImprimirLinha();
            //posprinter.ImprimirBarcode("$-=+ABC123abc", CmdBarcode.CODE128, CmdAlinhamento.Centro, CmdBarcodeText.Abaixo);
            //posprinter.ImprimirLinha();
            //posprinter.ImprimirBarcode("3515071111111111111159", CmdBarcode.CODE128c, CmdAlinhamento.Centro, CmdBarcodeText.Abaixo);

            posprinter.CortarPapel();
            posprinter.Imprimir();
        }
    }
}