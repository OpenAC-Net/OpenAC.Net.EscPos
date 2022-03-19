using System.Text;
using OpenAC.Net.EscPos.Command;

namespace OpenAC.Net.EscPos.Demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            // .Net 5 e 6 não tem todos os CodePages.
            // Então tem que instalar o pacote System.Text.Encoding.CodePages.
            // Adiciona esta linha ao programa antes de usar o EscPos.
            // Isso so precisa ser feito 1 vez então faça na inialização do seu programa.
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using var posprinter = EscPosPrinterFactory.CreateTCP(ProtocoloEscPos.Epson, o =>
            {
                o.ControlePorta = true;
                o.IP = "192.168.0.10";
            });

            posprinter.EspacoEntreLinhas = 40;

            posprinter.Conectar();
            posprinter.ImprimirTexto("ÁÉÍÓÚáéíóúçÇãõÃÕÊêÀà", CmdAlinhamento.Centro);
            posprinter.Imprimir();
        }
    }
}