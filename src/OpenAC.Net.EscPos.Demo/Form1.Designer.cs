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
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.btnCodBar = new System.Windows.Forms.Button();
            this.btnQrCode = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnStatus = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnText
            // 
            this.btnText.Location = new System.Drawing.Point(510, 518);
            this.btnText.Name = "btnText";
            this.btnText.Size = new System.Drawing.Size(182, 60);
            this.btnText.TabIndex = 0;
            this.btnText.Text = "Texto e Formatação";
            this.btnText.UseVisualStyleBackColor = true;
            this.btnText.Click += new System.EventHandler(this.btnTxt_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.SystemColors.Info;
            this.richTextBox1.Location = new System.Drawing.Point(510, 12);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedBoth;
            this.richTextBox1.Size = new System.Drawing.Size(982, 500);
            this.richTextBox1.TabIndex = 1;
            this.richTextBox1.Text = "";
            this.richTextBox1.WordWrap = false;
            // 
            // btnCodBar
            // 
            this.btnCodBar.Location = new System.Drawing.Point(698, 518);
            this.btnCodBar.Name = "btnCodBar";
            this.btnCodBar.Size = new System.Drawing.Size(182, 60);
            this.btnCodBar.TabIndex = 2;
            this.btnCodBar.Text = "Código de Barras";
            this.btnCodBar.UseVisualStyleBackColor = true;
            this.btnCodBar.Click += new System.EventHandler(this.btnCodBar_Click);
            // 
            // btnQrCode
            // 
            this.btnQrCode.Location = new System.Drawing.Point(886, 518);
            this.btnQrCode.Name = "btnQrCode";
            this.btnQrCode.Size = new System.Drawing.Size(182, 60);
            this.btnQrCode.TabIndex = 3;
            this.btnQrCode.Text = "QrCode";
            this.btnQrCode.UseVisualStyleBackColor = true;
            this.btnQrCode.Click += new System.EventHandler(this.btnQrCode_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 209);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(492, 448);
            this.tabControl1.TabIndex = 4;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 34);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(484, 410);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 34);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(484, 410);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnStatus
            // 
            this.btnStatus.Location = new System.Drawing.Point(1074, 518);
            this.btnStatus.Name = "btnStatus";
            this.btnStatus.Size = new System.Drawing.Size(182, 60);
            this.btnStatus.TabIndex = 5;
            this.btnStatus.Text = "Verificar Status";
            this.btnStatus.UseVisualStyleBackColor = true;
            this.btnStatus.Click += new System.EventHandler(this.btnStatus_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1504, 840);
            this.Controls.Add(this.btnStatus);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnQrCode);
            this.Controls.Add(this.btnCodBar);
            this.Controls.Add(this.richTextBox1);
            this.Controls.Add(this.btnText);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button btnText;
        private RichTextBox richTextBox1;
        private Button btnCodBar;
        private Button btnQrCode;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button btnStatus;
    }
}