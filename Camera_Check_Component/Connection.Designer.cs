
namespace Camera_Check_Component
{
    partial class Connection
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
            this.lbl1 = new System.Windows.Forms.Label();
            this.lbl2 = new System.Windows.Forms.Label();
            this.lbl3 = new System.Windows.Forms.Label();
            this.lbl4 = new System.Windows.Forms.Label();
            this.lbl5 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.pic3 = new System.Windows.Forms.PictureBox();
            this.pic2 = new System.Windows.Forms.PictureBox();
            this.pic1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pic3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).BeginInit();
            this.SuspendLayout();
            // 
            // lbl1
            // 
            this.lbl1.AutoSize = true;
            this.lbl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl1.Location = new System.Drawing.Point(46, 26);
            this.lbl1.Name = "lbl1";
            this.lbl1.Size = new System.Drawing.Size(219, 16);
            this.lbl1.TabIndex = 1;
            this.lbl1.Text = "Please wait while we setup connect ";
            // 
            // lbl2
            // 
            this.lbl2.AutoSize = true;
            this.lbl2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl2.Location = new System.Drawing.Point(57, 49);
            this.lbl2.Name = "lbl2";
            this.lbl2.Size = new System.Drawing.Size(193, 16);
            this.lbl2.TabIndex = 1;
            this.lbl2.Text = "to IP address 192.168.0.15:2000";
            // 
            // lbl3
            // 
            this.lbl3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl3.ForeColor = System.Drawing.Color.DarkGreen;
            this.lbl3.Location = new System.Drawing.Point(118, 55);
            this.lbl3.Name = "lbl3";
            this.lbl3.Size = new System.Drawing.Size(81, 18);
            this.lbl3.TabIndex = 3;
            this.lbl3.Text = "Congrats!";
            this.lbl3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl3.Visible = false;
            // 
            // lbl4
            // 
            this.lbl4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl4.Location = new System.Drawing.Point(21, 78);
            this.lbl4.Name = "lbl4";
            this.lbl4.Size = new System.Drawing.Size(279, 20);
            this.lbl4.TabIndex = 1;
            this.lbl4.Text = "Your connection has successful";
            this.lbl4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl4.Visible = false;
            // 
            // lbl5
            // 
            this.lbl5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbl5.Location = new System.Drawing.Point(84, 99);
            this.lbl5.Name = "lbl5";
            this.lbl5.Size = new System.Drawing.Size(161, 15);
            this.lbl5.TabIndex = 1;
            this.lbl5.Text = "Please press OK to close";
            this.lbl5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl5.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.DarkGreen;
            this.btnOK.FlatAppearance.BorderSize = 0;
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.ForeColor = System.Drawing.Color.White;
            this.btnOK.Location = new System.Drawing.Point(124, 122);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 27);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Visible = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // pic3
            // 
            this.pic3.Image = global::Camera_Check_Component.Properties.Resources.sign_error_icon;
            this.pic3.Location = new System.Drawing.Point(133, 4);
            this.pic3.Name = "pic3";
            this.pic3.Size = new System.Drawing.Size(48, 48);
            this.pic3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pic3.TabIndex = 5;
            this.pic3.TabStop = false;
            this.pic3.Visible = false;
            // 
            // pic2
            // 
            this.pic2.Image = global::Camera_Check_Component.Properties.Resources.check_1_icon;
            this.pic2.Location = new System.Drawing.Point(133, 4);
            this.pic2.Name = "pic2";
            this.pic2.Size = new System.Drawing.Size(48, 48);
            this.pic2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pic2.TabIndex = 2;
            this.pic2.TabStop = false;
            this.pic2.Visible = false;
            // 
            // pic1
            // 
            this.pic1.Image = global::Camera_Check_Component.Properties.Resources.output_onlinegiftools;
            this.pic1.Location = new System.Drawing.Point(127, 73);
            this.pic1.Name = "pic1";
            this.pic1.Size = new System.Drawing.Size(64, 64);
            this.pic1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pic1.TabIndex = 0;
            this.pic1.TabStop = false;
            // 
            // Connection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(312, 153);
            this.Controls.Add(this.pic3);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.lbl3);
            this.Controls.Add(this.pic2);
            this.Controls.Add(this.lbl2);
            this.Controls.Add(this.lbl5);
            this.Controls.Add(this.lbl4);
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.pic1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Connection";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Connection";
            this.Load += new System.EventHandler(this.Connection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pic3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pic1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pic1;
        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label lbl2;
        private System.Windows.Forms.PictureBox pic2;
        private System.Windows.Forms.Label lbl3;
        private System.Windows.Forms.Label lbl4;
        private System.Windows.Forms.Label lbl5;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.PictureBox pic3;
    }
}