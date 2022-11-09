using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Camera_Check_Component
{
    public partial class Connection : Form
    {
        public static Boolean connection = false;
        public static Boolean connection1 = false;
        public Connection()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
          
            if(connection == true)
            {
                lbl1.Visible = false;
                lbl2.Visible = false;
                pic1.Visible = false;
                pic2.Visible = true;
                lbl3.Visible = true;
                lbl4.Visible = true;
                lbl5.Visible = true;
                btnOK.Visible = true;
                connection = false;
                timer1.Stop();
            }
        }

        private void Connection_Load(object sender, EventArgs e)
        {
            if (connection1 == true)
            {
                lbl1.Visible = false;
                lbl2.Visible = false;
                pic1.Visible = false;
                pic3.Visible = true;
                lbl3.Visible = true;
                lbl4.Visible = true;
                lbl5.Visible = true;
                btnOK.Visible = true;
                lbl3.Text = "FAILED!";
                lbl3.ForeColor = Color.Red;
                lbl4.Text = "Your connection PLC has failed!";
            }
            if (connection1 == false)
            {
                timer1.Start();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
