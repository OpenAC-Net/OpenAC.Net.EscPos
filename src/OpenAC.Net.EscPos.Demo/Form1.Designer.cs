namespace OpenAC.Net.EscPos.Demo
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnText = new System.Windows.Forms.Button();
            this.rtbLog = new System.Windows.Forms.RichTextBox();
            this.btnCodBar = new System.Windows.Forms.Button();
            this.btnQrCode = new System.Windows.Forms.Button();
            this.tbcDeviceConfig = new System.Windows.Forms.TabControl();
            this.tbpSerial = new System.Windows.Forms.TabPage();
            this.cbbStopBits = new System.Windows.Forms.ComboBox();
            this.cbbHandshake = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.cbbParity = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cbbDataBits = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbbVelocidade = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cbbPortas = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbpTCP = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.nudPorta = new System.Windows.Forms.NumericUpDown();
            this.txtIP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbpRAW = new System.Windows.Forms.TabPage();
            this.cbbImpressoras = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbpFile = new System.Windows.Forms.TabPage();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.txtArquivo = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.btnStatus = new System.Windows.Forms.Button();
            this.btnImagem = new System.Windows.Forms.Button();
            this.txtModoPagina = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.tbcConfigurações = new System.Windows.Forms.TabControl();
            this.tbpConfigurações = new System.Windows.Forms.TabPage();
            this.label10 = new System.Windows.Forms.Label();
            this.nudLinhas = new System.Windows.Forms.NumericUpDown();
            this.nudEspacos = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.chkControlePortas = new System.Windows.Forms.CheckBox();
            this.cbbEnconding = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cbbProtocolo = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cbbConexao = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbpCodigoBarras = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnInfo = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.tbcDeviceConfig.SuspendLayout();
            this.tbpSerial.SuspendLayout();
            this.tbpTCP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPorta)).BeginInit();
            this.tbpRAW.SuspendLayout();
            this.tbpFile.SuspendLayout();
            this.tbcConfigurações.SuspendLayout();
            this.tbpConfigurações.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLinhas)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEspacos)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnText
            // 
            this.btnText.Location = new System.Drawing.Point(510, 452);
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(182, 60);
            this.btnText.TabIndex = 0;
            this.btnText.Text = "Texto e Formatação";
            this.btnText.UseVisualStyleBackColor = true;
            this.btnText.Click += new System.EventHandler(this.btnTxt_Click);
            // 
            // rtbLog
            // 
            this.rtbLog.BackColor = System.Drawing.SystemColors.Info;
            this.rtbLog.Location = new System.Drawing.Point(510, 12);
            this.rtbLog.Name = "rtbLog";
            this.rtbLog.ReadOnly = true;
            this.rtbLog.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.rtbLog.Size = new System.Drawing.Size(746, 434);
            this.rtbLog.TabIndex = 1;
            this.rtbLog.Text = "";
            this.rtbLog.WordWrap = false;
            // 
            // btnCodBar
            // 
            this.btnCodBar.Location = new System.Drawing.Point(698, 452);
            this.btnCodBar.Name = "btnCodBar";
            this.btnCodBar.Size = new System.Drawing.Size(182, 60);
            this.btnCodBar.TabIndex = 2;
            this.btnCodBar.Text = "Código de Barras";
            this.btnCodBar.UseVisualStyleBackColor = true;
            this.btnCodBar.Click += new System.EventHandler(this.btnCodBar_Click);
            // 
            // btnQrCode
            // 
            this.btnQrCode.Location = new System.Drawing.Point(886, 452);
            this.btnQrCode.Name = "btnQrCode";
            this.btnQrCode.Size = new System.Drawing.Size(182, 60);
            this.btnQrCode.TabIndex = 3;
            this.btnQrCode.Text = "QrCode";
            this.btnQrCode.UseVisualStyleBackColor = true;
            this.btnQrCode.Click += new System.EventHandler(this.btnQrCode_Click);
            // 
            // tbcDeviceConfig
            // 
            this.tbcDeviceConfig.Controls.Add(this.tbpSerial);
            this.tbcDeviceConfig.Controls.Add(this.tbpTCP);
            this.tbcDeviceConfig.Controls.Add(this.tbpRAW);
            this.tbcDeviceConfig.Controls.Add(this.tbpFile);
            this.tbcDeviceConfig.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbcDeviceConfig.Location = new System.Drawing.Point(3, 27);
            this.tbcDeviceConfig.Name = "tbcDeviceConfig";
            this.tbcDeviceConfig.SelectedIndex = 0;
            this.tbcDeviceConfig.Size = new System.Drawing.Size(478, 300);
            this.tbcDeviceConfig.TabIndex = 4;
            // 
            // tbpSerial
            // 
            this.tbpSerial.Controls.Add(this.cbbStopBits);
            this.tbpSerial.Controls.Add(this.cbbHandshake);
            this.tbpSerial.Controls.Add(this.label15);
            this.tbpSerial.Controls.Add(this.label14);
            this.tbpSerial.Controls.Add(this.cbbParity);
            this.tbpSerial.Controls.Add(this.label13);
            this.tbpSerial.Controls.Add(this.cbbDataBits);
            this.tbpSerial.Controls.Add(this.label12);
            this.tbpSerial.Controls.Add(this.cbbVelocidade);
            this.tbpSerial.Controls.Add(this.label11);
            this.tbpSerial.Controls.Add(this.cbbPortas);
            this.tbpSerial.Controls.Add(this.label3);
            this.tbpSerial.Location = new System.Drawing.Point(4, 34);
            this.tbpSerial.Name = "tbpSerial";
            this.tbpSerial.Padding = new System.Windows.Forms.Padding(3);
            this.tbpSerial.Size = new System.Drawing.Size(470, 262);
            this.tbpSerial.TabIndex = 0;
            this.tbpSerial.Text = "Serial";
            this.tbpSerial.UseVisualStyleBackColor = true;
            // 
            // cbbStopBits
            // 
            this.cbbStopBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbStopBits.FormattingEnabled = true;
            this.cbbStopBits.Location = new System.Drawing.Point(117, 165);
            this.cbbStopBits.Name = "cbbStopBits";
            this.cbbStopBits.Size = new System.Drawing.Size(265, 33);
            this.cbbStopBits.TabIndex = 24;
            // 
            // cbbHandshake
            // 
            this.cbbHandshake.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbHandshake.FormattingEnabled = true;
            this.cbbHandshake.Location = new System.Drawing.Point(117, 204);
            this.cbbHandshake.Name = "cbbHandshake";
            this.cbbHandshake.Size = new System.Drawing.Size(265, 33);
            this.cbbHandshake.TabIndex = 23;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(9, 207);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(107, 25);
            this.label15.TabIndex = 22;
            this.label15.Text = "Handshake";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(6, 168);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(83, 25);
            this.label14.TabIndex = 20;
            this.label14.Text = "StopBits\n";
            // 
            // cbbParity
            // 
            this.cbbParity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbParity.FormattingEnabled = true;
            this.cbbParity.Location = new System.Drawing.Point(117, 126);
            this.cbbParity.Name = "cbbParity";
            this.cbbParity.Size = new System.Drawing.Size(265, 33);
            this.cbbParity.TabIndex = 19;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(6, 129);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(62, 25);
            this.label13.TabIndex = 18;
            this.label13.Text = "Parity";
            // 
            // cbbDataBits
            // 
            this.cbbDataBits.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbDataBits.FormattingEnabled = true;
            this.cbbDataBits.Location = new System.Drawing.Point(117, 87);
            this.cbbDataBits.Name = "cbbDataBits";
            this.cbbDataBits.Size = new System.Drawing.Size(265, 33);
            this.cbbDataBits.TabIndex = 17;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 90);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(89, 25);
            this.label12.TabIndex = 16;
            this.label12.Text = "Data Bits";
            // 
            // cbbVelocidade
            // 
            this.cbbVelocidade.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbVelocidade.FormattingEnabled = true;
            this.cbbVelocidade.Location = new System.Drawing.Point(117, 48);
            this.cbbVelocidade.Name = "cbbVelocidade";
            this.cbbVelocidade.Size = new System.Drawing.Size(265, 33);
            this.cbbVelocidade.TabIndex = 15;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(6, 51);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 25);
            this.label11.TabIndex = 14;
            this.label11.Text = "Velocidade";
            // 
            // cbbPortas
            // 
            this.cbbPortas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbPortas.FormattingEnabled = true;
            this.cbbPortas.Location = new System.Drawing.Point(117, 9);
            this.cbbPortas.Name = "cbbPortas";
            this.cbbPortas.Size = new System.Drawing.Size(265, 33);
            this.cbbPortas.TabIndex = 13;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(58, 25);
            this.label3.TabIndex = 0;
            this.label3.Text = "Porta";
            // 
            // tbpTCP
            // 
            this.tbpTCP.Controls.Add(this.label6);
            this.tbpTCP.Controls.Add(this.nudPorta);
            this.tbpTCP.Controls.Add(this.txtIP);
            this.tbpTCP.Controls.Add(this.label4);
            this.tbpTCP.Location = new System.Drawing.Point(4, 34);
            this.tbpTCP.Name = "tbpTCP";
            this.tbpTCP.Padding = new System.Windows.Forms.Padding(3);
            this.tbpTCP.Size = new System.Drawing.Size(470, 262);
            this.tbpTCP.TabIndex = 1;
            this.tbpTCP.Text = "TCP";
            this.tbpTCP.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(58, 25);
            this.label6.TabIndex = 3;
            this.label6.Text = "Porta";
            // 
            // nudPorta
            // 
            this.nudPorta.Location = new System.Drawing.Point(74, 43);
            this.nudPorta.Maximum = new decimal(new int[] {
            999999,
            0,
            0,
            0});
            this.nudPorta.Name = "nudPorta";
            this.nudPorta.Size = new System.Drawing.Size(180, 31);
            this.nudPorta.TabIndex = 2;
            this.nudPorta.Value = new decimal(new int[] {
            9100,
            0,
            0,
            0});
            // 
            // txtIP
            // 
            this.txtIP.Location = new System.Drawing.Point(74, 6);
            this.txtIP.Name = "txtIP";
            this.txtIP.Size = new System.Drawing.Size(390, 31);
            this.txtIP.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 12);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "IP";
            // 
            // tbpRAW
            // 
            this.tbpRAW.Controls.Add(this.cbbImpressoras);
            this.tbpRAW.Controls.Add(this.label7);
            this.tbpRAW.Location = new System.Drawing.Point(4, 34);
            this.tbpRAW.Name = "tbpRAW";
            this.tbpRAW.Padding = new System.Windows.Forms.Padding(3);
            this.tbpRAW.Size = new System.Drawing.Size(470, 262);
            this.tbpRAW.TabIndex = 2;
            this.tbpRAW.Text = "Raw";
            this.tbpRAW.UseVisualStyleBackColor = true;
            // 
            // cbbImpressoras
            // 
            this.cbbImpressoras.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbImpressoras.FormattingEnabled = true;
            this.cbbImpressoras.Location = new System.Drawing.Point(118, 6);
            this.cbbImpressoras.Name = "cbbImpressoras";
            this.cbbImpressoras.Size = new System.Drawing.Size(346, 33);
            this.cbbImpressoras.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(106, 25);
            this.label7.TabIndex = 2;
            this.label7.Text = "Impressora";
            // 
            // tbpFile
            // 
            this.tbpFile.Controls.Add(this.btnOpenFile);
            this.tbpFile.Controls.Add(this.txtArquivo);
            this.tbpFile.Controls.Add(this.label8);
            this.tbpFile.Location = new System.Drawing.Point(4, 34);
            this.tbpFile.Name = "tbpFile";
            this.tbpFile.Padding = new System.Windows.Forms.Padding(3);
            this.tbpFile.Size = new System.Drawing.Size(470, 262);
            this.tbpFile.TabIndex = 3;
            this.tbpFile.Text = "Arquivo";
            this.tbpFile.UseVisualStyleBackColor = true;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.Location = new System.Drawing.Point(426, 6);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(35, 32);
            this.btnOpenFile.TabIndex = 4;
            this.btnOpenFile.Text = "...";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // txtArquivo
            // 
            this.txtArquivo.Location = new System.Drawing.Point(92, 6);
            this.txtArquivo.Name = "txtArquivo";
            this.txtArquivo.Size = new System.Drawing.Size(337, 31);
            this.txtArquivo.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(80, 25);
            this.label8.TabIndex = 2;
            this.label8.Text = "Arquivo";
            // 
            // btnStatus
            // 
            this.btnStatus.Location = new System.Drawing.Point(1074, 452);
            this.btnStatus.Name = "btnStatus";
            this.btnStatus.Size = new System.Drawing.Size(182, 60);
            this.btnStatus.TabIndex = 5;
            this.btnStatus.Text = "Verificar Status";
            this.btnStatus.UseVisualStyleBackColor = true;
            this.btnStatus.Click += new System.EventHandler(this.btnStatus_Click);
            // 
            // btnImagem
            // 
            this.btnImagem.Location = new System.Drawing.Point(698, 518);
            this.btnImagem.Name = "btnImagem";
            this.btnImagem.Size = new System.Drawing.Size(182, 60);
            this.btnImagem.TabIndex = 6;
            this.btnImagem.Text = "Imprimir Imagem";
            this.btnImagem.UseVisualStyleBackColor = true;
            this.btnImagem.Click += new System.EventHandler(this.btnImagem_Click);
            // 
            // txtModoPagina
            // 
            this.txtModoPagina.Location = new System.Drawing.Point(510, 518);
            this.txtModoPagina.Name = "txtModoPagina";
            this.txtModoPagina.Size = new System.Drawing.Size(182, 60);
            this.txtModoPagina.TabIndex = 7;
            this.txtModoPagina.Text = "Modo Pagina";
            this.txtModoPagina.UseVisualStyleBackColor = true;
            this.txtModoPagina.Click += new System.EventHandler(this.txtModoPagina_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(510, 584);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(182, 60);
            this.button1.TabIndex = 10;
            this.button1.UseVisualStyleBackColor = true;
            // 
            // tbcConfigurações
            // 
            this.tbcConfigurações.Controls.Add(this.tbpConfigurações);
            this.tbcConfigurações.Controls.Add(this.tbpCodigoBarras);
            this.tbcConfigurações.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbcConfigurações.Location = new System.Drawing.Point(12, 12);
            this.tbcConfigurações.Name = "tbcConfigurações";
            this.tbcConfigurações.SelectedIndex = 0;
            this.tbcConfigurações.Size = new System.Drawing.Size(488, 256);
            this.tbcConfigurações.TabIndex = 11;
            // 
            // tbpConfigurações
            // 
            this.tbpConfigurações.Controls.Add(this.label10);
            this.tbpConfigurações.Controls.Add(this.nudLinhas);
            this.tbpConfigurações.Controls.Add(this.nudEspacos);
            this.tbpConfigurações.Controls.Add(this.label9);
            this.tbpConfigurações.Controls.Add(this.chkControlePortas);
            this.tbpConfigurações.Controls.Add(this.cbbEnconding);
            this.tbpConfigurações.Controls.Add(this.label5);
            this.tbpConfigurações.Controls.Add(this.cbbProtocolo);
            this.tbpConfigurações.Controls.Add(this.label2);
            this.tbpConfigurações.Controls.Add(this.cbbConexao);
            this.tbpConfigurações.Controls.Add(this.label1);
            this.tbpConfigurações.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbpConfigurações.Location = new System.Drawing.Point(4, 34);
            this.tbpConfigurações.Name = "tbpConfigurações";
            this.tbpConfigurações.Padding = new System.Windows.Forms.Padding(3);
            this.tbpConfigurações.Size = new System.Drawing.Size(480, 218);
            this.tbpConfigurações.TabIndex = 0;
            this.tbpConfigurações.Text = "Configurações";
            this.tbpConfigurações.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label10.Location = new System.Drawing.Point(249, 165);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 25);
            this.label10.TabIndex = 22;
            this.label10.Text = "Linhas";
            // 
            // nudLinhas
            // 
            this.nudLinhas.Location = new System.Drawing.Point(350, 163);
            this.nudLinhas.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudLinhas.Name = "nudLinhas";
            this.nudLinhas.Size = new System.Drawing.Size(123, 31);
            this.nudLinhas.TabIndex = 21;
            // 
            // nudEspacos
            // 
            this.nudEspacos.Location = new System.Drawing.Point(107, 161);
            this.nudEspacos.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudEspacos.Name = "nudEspacos";
            this.nudEspacos.Size = new System.Drawing.Size(123, 31);
            this.nudEspacos.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label9.Location = new System.Drawing.Point(6, 163);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(79, 25);
            this.label9.TabIndex = 19;
            this.label9.Text = "Espaços";
            // 
            // chkControlePortas
            // 
            this.chkControlePortas.AutoSize = true;
            this.chkControlePortas.Checked = true;
            this.chkControlePortas.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkControlePortas.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.chkControlePortas.Location = new System.Drawing.Point(107, 126);
            this.chkControlePortas.Name = "chkControlePortas";
            this.chkControlePortas.Size = new System.Drawing.Size(196, 29);
            this.chkControlePortas.TabIndex = 18;
            this.chkControlePortas.Text = "Controle de Portas";
            this.chkControlePortas.UseVisualStyleBackColor = true;
            // 
            // cbbEnconding
            // 
            this.cbbEnconding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbEnconding.FormattingEnabled = true;
            this.cbbEnconding.Location = new System.Drawing.Point(107, 87);
            this.cbbEnconding.Name = "cbbEnconding";
            this.cbbEnconding.Size = new System.Drawing.Size(231, 33);
            this.cbbEnconding.TabIndex = 17;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label5.Location = new System.Drawing.Point(6, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 25);
            this.label5.TabIndex = 16;
            this.label5.Text = "Encoding";
            // 
            // cbbProtocolo
            // 
            this.cbbProtocolo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbProtocolo.FormattingEnabled = true;
            this.cbbProtocolo.Location = new System.Drawing.Point(107, 48);
            this.cbbProtocolo.Name = "cbbProtocolo";
            this.cbbProtocolo.Size = new System.Drawing.Size(231, 33);
            this.cbbProtocolo.TabIndex = 15;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 25);
            this.label2.TabIndex = 14;
            this.label2.Text = "Protocolo";
            // 
            // cbbConexao
            // 
            this.cbbConexao.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbbConexao.FormattingEnabled = true;
            this.cbbConexao.Location = new System.Drawing.Point(107, 9);
            this.cbbConexao.Name = "cbbConexao";
            this.cbbConexao.Size = new System.Drawing.Size(231, 33);
            this.cbbConexao.TabIndex = 12;
            this.cbbConexao.SelectedIndexChanged += new System.EventHandler(this.cbbConexao_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.label1.Location = new System.Drawing.Point(6, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 25);
            this.label1.TabIndex = 13;
            this.label1.Text = "Conexão";
            // 
            // tbpCodigoBarras
            // 
            this.tbpCodigoBarras.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.tbpCodigoBarras.Location = new System.Drawing.Point(4, 34);
            this.tbpCodigoBarras.Name = "tbpCodigoBarras";
            this.tbpCodigoBarras.Padding = new System.Windows.Forms.Padding(3);
            this.tbpCodigoBarras.Size = new System.Drawing.Size(480, 218);
            this.tbpCodigoBarras.TabIndex = 1;
            this.tbpCodigoBarras.Text = "Código de Barras";
            this.tbpCodigoBarras.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbcDeviceConfig);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.groupBox1.Location = new System.Drawing.Point(12, 274);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(484, 330);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Conexão";
            // 
            // btnInfo
            // 
            this.btnInfo.Location = new System.Drawing.Point(886, 518);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(182, 60);
            this.btnInfo.TabIndex = 13;
            this.btnInfo.Text = "Info. Impressora";
            this.btnInfo.UseVisualStyleBackColor = true;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(1074, 518);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(182, 60);
            this.button3.TabIndex = 14;
            this.button3.UseVisualStyleBackColor = true;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(698, 584);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(182, 60);
            this.button4.TabIndex = 15;
            this.button4.UseVisualStyleBackColor = true;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(886, 584);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(182, 60);
            this.button5.TabIndex = 16;
            this.button5.UseVisualStyleBackColor = true;
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(1074, 584);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(182, 60);
            this.button6.TabIndex = 17;
            this.button6.UseVisualStyleBackColor = true;
            // 
            // btnSalvar
            // 
            this.btnSalvar.Location = new System.Drawing.Point(384, 610);
            this.btnSalvar.Name = "btnSalvar";
            this.btnSalvar.Size = new System.Drawing.Size(112, 34);
            this.btnSalvar.TabIndex = 18;
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.UseVisualStyleBackColor = true;
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1268, 658);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.btnInfo);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tbcConfigurações);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtModoPagina);
            this.Controls.Add(this.btnImagem);
            this.Controls.Add(this.btnStatus);
            this.Controls.Add(this.btnQrCode);
            this.Controls.Add(this.btnCodBar);
            this.Controls.Add(this.rtbLog);
            this.Controls.Add(this.btnText);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OpenAC .Net EscPos Demo";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.tbcDeviceConfig.ResumeLayout(false);
            this.tbpSerial.ResumeLayout(false);
            this.tbpSerial.PerformLayout();
            this.tbpTCP.ResumeLayout(false);
            this.tbpTCP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPorta)).EndInit();
            this.tbpRAW.ResumeLayout(false);
            this.tbpRAW.PerformLayout();
            this.tbpFile.ResumeLayout(false);
            this.tbpFile.PerformLayout();
            this.tbcConfigurações.ResumeLayout(false);
            this.tbpConfigurações.ResumeLayout(false);
            this.tbpConfigurações.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudLinhas)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudEspacos)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnText;
        private RichTextBox rtbLog;
        private Button btnCodBar;
        private Button btnQrCode;
        private TabControl tbcDeviceConfig;
        private TabPage tbpSerial;
        private TabPage tbpTCP;
        private Button btnStatus;
        private Button btnImagem;
        private Button txtModoPagina;
        private Button button1;
        private TabControl tbcConfigurações;
        private TabPage tbpConfigurações;
        private ComboBox cbbProtocolo;
        private Label label2;
        private ComboBox cbbConexao;
        private Label label1;
        private TabPage tbpCodigoBarras;
        private GroupBox groupBox1;
        private Label label3;
        private Label label4;
        private TabPage tbpRAW;
        private TabPage tbpFile;
        private Button btnInfo;
        private Button button3;
        private Button button4;
        private Button button5;
        private Button button6;
        private ComboBox cbbEnconding;
        private Label label5;
        private Label label6;
        private NumericUpDown nudPorta;
        private TextBox txtIP;
        private Label label7;
        private ComboBox cbbImpressoras;
        private Button btnOpenFile;
        private TextBox txtArquivo;
        private Label label8;
        private ComboBox cbbPortas;
        private CheckBox chkControlePortas;
        private Label label10;
        private NumericUpDown nudLinhas;
        private NumericUpDown nudEspacos;
        private Label label9;
        private ComboBox cbbVelocidade;
        private Label label11;
        private ComboBox cbbDataBits;
        private Label label12;
        private Label label14;
        private ComboBox cbbParity;
        private Label label13;
        private ComboBox cbbHandshake;
        private Label label15;
        private ComboBox cbbStopBits;
        private Button btnSalvar;
    }
}