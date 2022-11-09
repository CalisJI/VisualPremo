using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Sql;

namespace Camera_Check_Component
{
    public partial class Sign_up : Form
    {
        SQL_action sQL_Action = new SQL_action();
        SqlConnection conn = new SqlConnection();
       
        public Sign_up()
        {
            InitializeComponent();
            conn = new SqlConnection(sQL_Action.GetSource());

        }
        
        private void button1_Click(object sender, EventArgs e)
        {
            string chek_acc = sQL_Action.get_exist_account(textBox1.Text);
            if (chek_acc == textBox1.Text) 
            {
                MessageBox.Show("This Account already exists, Please try another one");
                return;
            }
            if (comboBox1.Text == "")
            {
                MessageBox.Show("Please choose permissions");
                return;
            }
            string mesage = "Do you want to sign up this account";
            string cap = "SIGN UP";
            var result = MessageBox.Show(mesage, cap, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if(result == DialogResult.Yes && textBox2.Text==textBox3.Text) 
            {           
                byte[] temp = ASCIIEncoding.ASCII.GetBytes(textBox2.Text);
                byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);
                string hasPass = "";
                foreach (byte item in hasData)
                {
                    hasPass += item;
                }
                string sql = "INSERT INTO tbl_user_ID([user],[pass]) VALUES (N'" + textBox1.Text + "','" + hasPass + "')";
                Boolean sign_in = sQL_Action.excute_data(sql);
               
                if (comboBox1.SelectedIndex == 0)
                {
                    string ID_user = sQL_Action.getID_user(textBox1.Text, hasPass);
                    string sql2 = "INSERT INTO tbl_per_rel ([ID_user_rel],[ID_per_rel]) VALUES (N'" + ID_user + "','2')";
                    Boolean sign_up1 = sQL_Action.excute_data(sql2);
                    if (sign_up1)                     
                    {
                        MessageBox.Show("Create account sucessfully");
                    }
                    else 
                    {
                        MessageBox.Show("SQL Connection error");
                    }                      
                }
                if (comboBox1.SelectedIndex == 1)
                {
                    string ID_user = sQL_Action.getID_user(textBox1.Text, hasPass);
                    string sql2 = "INSERT INTO tbl_per_rel ([ID_user_rel],[ID_per_rel]) VALUES (N'" + ID_user + "','3')";
                    Boolean sign_up2 = sQL_Action.excute_data(sql2);
                    if (sign_up2)
                    {
                        MessageBox.Show("Create account sucessfully");
                    }
                    else
                    {
                        MessageBox.Show("SQL Connection error");
                    }
                }
                if (comboBox1.SelectedIndex == 2)
                {
                    string ID_user = sQL_Action.getID_user(textBox1.Text, hasPass);
                    string sql2 = "INSERT INTO tbl_per_rel ([ID_user_rel],[ID_per_rel]) VALUES (N'" + ID_user + "','4')";
                    Boolean sign_up3 = sQL_Action.excute_data(sql2);
                    if (sign_up3)
                    {
                        MessageBox.Show("Create account sucessfully");
                    }
                    else
                    {
                        MessageBox.Show("SQL Connection error");
                    }
                }
                if (comboBox1.SelectedIndex == 3)
                {
                    string ID_user = sQL_Action.getID_user(textBox1.Text, hasPass);
                    string sql2 = "INSERT INTO tbl_per_rel ([ID_user_rel],[ID_per_rel]) VALUES (N'" + ID_user + "','1')";
                    Boolean sign_up4 = sQL_Action.excute_data(sql2);
                    if (sign_up4)
                    {
                        MessageBox.Show("Create account sucessfully");
                    }
                    else
                    {
                        MessageBox.Show("SQL Connection error");
                    }
                }
                
            }
            else
            {              
                MessageBox.Show("Password incorrect");               
            }
           
        }
    }
}
