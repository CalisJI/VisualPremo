using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Configuration;
using System.Data.SqlClient;

namespace Camera_Check_Component
{
    public partial class Login : Form
    {
        System_config System_Config = new System_config();
        SQL_action sQL_Action = new SQL_action();
        public Login()
        {
            InitializeComponent();
        }
        public string ID_user = "";
        private void Login_Load(object sender, EventArgs e)
        {
                        DataTable dt = SQL_action.GetSQL_SeverList();
            System_Config = Program_Configuration.GetSystem_Config();
            if (System_Config.SQL_server == null || System_Config.Database == null || System_Config.SQL_server == "" || System_Config.Database == "")
            {
                System_config system_Config = new System_config();
                system_Config.SQL_server = @"Data Source=CALIS_YII\WINCC;Initial Catalog=ComponentState;Integrated Security=True";
                system_Config.Database = "ComponentState";
            }

            if (System_Config.SQL_server != "" && System_Config.Database != "")
            {
                comboBox1.Text = System_Config.SQL_server;
                comboBox2.Text = System_Config.Database;
            }
          
            comboBox1.Items.Clear();
            foreach (DataRow r in dt.Rows)
            {
                if (r["InstanceName"].ToString() == "")
                {
                    comboBox1.Items.Add(r["ServerName"].ToString());
                }
                else
                {
                    comboBox1.Items.Add(r["ServerName"].ToString() + "\\" + r["InstanceName"].ToString());
                }
            }
        }
        bool data = false;

        private void button1_Click(object sender, EventArgs e)
        {
       
            byte[] temp = ASCIIEncoding.ASCII.GetBytes(TB_pass.Text);
            byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);
            string hasPass = "";

            foreach (byte item in hasData)
            {
                hasPass += item;
            }
            ID_user = sQL_Action.getID_user(TB_user.Text, hasPass);
            try
            {
                if (ID_user != "")
                {
                    MessageBox.Show(" Login Successfully");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("The User ID or Password is wrong");
                }
            }
            catch (Exception e1 )
            {

                MessageBox.Show(e1.ToString());
            }
          
           
        }

        private void exit_btn_Click(object sender, EventArgs e)
        {
            string mesage = "Do you want to exit the program";
            string cap = "Close the program";
            var result = MessageBox.Show(mesage, cap, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes) Environment.Exit(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > 0 || comboBox2.SelectedIndex > 0 || System_Config.SQL_server != "" || System_Config.Database != "")
            {
                System_Config = Program_Configuration.GetSystem_Config();
                Program_Configuration.UpdateSystem_Config("SQL_server", comboBox1.Text);
                Program_Configuration.UpdateSystem_Config("Database", comboBox2.Text);
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (data)
            {
                DataTable dt = SQL_action.GetSQL_DatabaseList(comboBox1.SelectedItem.ToString());
                comboBox2.Items.Clear();
                foreach (DataRow r in dt.Rows)
                {
                    string databaseName = r["database_name"].ToString();
                    comboBox2.Items.Add(databaseName);
                }
            }
        }

       

        private void comboBox1_MouseDown(object sender, MouseEventArgs e)
        {
            data = true;
        }
    }
}
