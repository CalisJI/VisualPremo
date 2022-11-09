namespace Path_Flie_Component
{
    partial class Form1
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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Open_Dialog = new System.Windows.Forms.Button();
            this.Save_Path = new System.Windows.Forms.Button();
            this._Cancel_ = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(66, 19);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(431, 20);
            this.textBox1.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Path File :";
            // 
            // Open_Dialog
            // 
            this.Open_Dialog.Location = new System.Drawing.Point(503, 16);
            this.Open_Dialog.Name = "Open_Dialog";
            this.Open_Dialog.Size = new System.Drawing.Size(40, 23);
            this.Open_Dialog.TabIndex = 2;
            this.Open_Dialog.Text = "...";
            this.Open_Dialog.UseVisualStyleBackColor = true;
            // 
            // Save_Path
            // 
            this.Save_Path.Location = new System.Drawing.Point(170, 49);
            this.Save_Path.Name = "Save_Path";
            this.Save_Path.Size = new System.Drawing.Size(90, 32);
            this.Save_Path.TabIndex = 3;
            this.Save_Path.Text = "SAVE";
            this.Save_Path.UseVisualStyleBackColor = true;
            // 
            // _Cancel_
            // 
            this._Cancel_.Location = new System.Drawing.Point(334, 49);
            this._Cancel_.Name = "_Cancel_";
            this._Cancel_.Size = new System.Drawing.Size(90, 32);
            this._Cancel_.TabIndex = 4;
            this._Cancel_.Text = "CANCEL";
            this._Cancel_.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(590, 99);
            this.Controls.Add(this._Cancel_);
            this.Controls.Add(this.Save_Path);
            this.Controls.Add(this.Open_Dialog);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button Open_Dialog;
        private System.Windows.Forms.Button Save_Path;
        private System.Windows.Forms.Button _Cancel_;
    }
}

