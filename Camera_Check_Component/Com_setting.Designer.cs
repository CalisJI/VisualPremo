namespace Camera_Check_Component
{
    partial class Com_setting
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Com_setting));
            this.Com_setting_box = new System.Windows.Forms.ComboBox();
            this.connect_com_btn = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SAVE_btn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Baudrate_box = new System.Windows.Forms.ComboBox();
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.SuspendLayout();
            // 
            // Com_setting_box
            // 
            this.Com_setting_box.FormattingEnabled = true;
            this.Com_setting_box.Location = new System.Drawing.Point(89, 16);
            this.Com_setting_box.Name = "Com_setting_box";
            this.Com_setting_box.Size = new System.Drawing.Size(121, 21);
            this.Com_setting_box.TabIndex = 0;
            this.Com_setting_box.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Com_setting_box_MouseDown);
            // 
            // connect_com_btn
            // 
            this.connect_com_btn.Location = new System.Drawing.Point(248, 14);
            this.connect_com_btn.Name = "connect_com_btn";
            this.connect_com_btn.Size = new System.Drawing.Size(75, 23);
            this.connect_com_btn.TabIndex = 1;
            this.connect_com_btn.Text = "SET";
            this.connect_com_btn.UseVisualStyleBackColor = true;
            this.connect_com_btn.Click += new System.EventHandler(this.connect_com_btn_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "COM PORT";
            // 
            // SAVE_btn
            // 
            this.SAVE_btn.Location = new System.Drawing.Point(248, 49);
            this.SAVE_btn.Name = "SAVE_btn";
            this.SAVE_btn.Size = new System.Drawing.Size(75, 23);
            this.SAVE_btn.TabIndex = 3;
            this.SAVE_btn.Text = "SAVE";
            this.SAVE_btn.UseVisualStyleBackColor = true;
            this.SAVE_btn.Click += new System.EventHandler(this.SAVE_btn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "BAUDRATE";
            // 
            // Baudrate_box
            // 
            this.Baudrate_box.FormattingEnabled = true;
            this.Baudrate_box.Location = new System.Drawing.Point(89, 51);
            this.Baudrate_box.Name = "Baudrate_box";
            this.Baudrate_box.Size = new System.Drawing.Size(121, 21);
            this.Baudrate_box.TabIndex = 5;
            // 
            // Com_setting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 85);
            this.Controls.Add(this.Baudrate_box);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SAVE_btn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.connect_com_btn);
            this.Controls.Add(this.Com_setting_box);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Com_setting";
            this.Text = "Com_setting";
            this.Load += new System.EventHandler(this.Com_setting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Com_setting_box;
        private System.Windows.Forms.Button connect_com_btn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SAVE_btn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox Baudrate_box;
        private System.IO.Ports.SerialPort serialPort1;
    }
}