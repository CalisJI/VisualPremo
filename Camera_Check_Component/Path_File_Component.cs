using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Camera_Check_Component;

namespace Camera_Check_Component
{
    public partial class Path_File_Component : Form
    {
        private OpenFileDialog opendialog1 = new OpenFileDialog();
        private System_config system_config;
        private FolderBrowserDialog openfolder1 = new FolderBrowserDialog();
        private FolderBrowserDialog openfolder2 = new FolderBrowserDialog();
        private FolderBrowserDialog openfolder3 = new FolderBrowserDialog();
        private FolderBrowserDialog openfolder4 = new FolderBrowserDialog();

        //CSV_Action Csv = new CSV_Action(@"D:\");
        public Path_File_Component()
        {
            InitializeComponent();
        }

        private void Path_File_Component_Load(object sender, EventArgs e)
        {
            // Đọc file Config rồi trả về
            system_config = Program_Configuration.GetSystem_Config();

            TextBox_PathFile.Text = system_config.Map_Path_File;
            textBox1.Text = system_config.Map_Path_File_2;
            textBox2.Text = system_config.DefaultComport;
            openfolder1.RootFolder = Environment.SpecialFolder.MyComputer;
            
        }

        private void Open_Dialog_btn_Click(object sender, EventArgs e)
        {
            if (openfolder1.ShowDialog() == DialogResult.OK) 
            {
                TextBox_PathFile.Text = openfolder1.SelectedPath.ToString();
            }
        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Saving_btn_Click(object sender, EventArgs e)
        {
            try
            {

               
                //Lưu Config 
                Program_Configuration.UpdateSystem_Config(nameof(system_config.Map_Path_File), TextBox_PathFile.Text);
                
                Program_Configuration.UpdateSystem_Config("Map_Path_File_2", textBox1.Text);
                Program_Configuration.UpdateSystem_Config(nameof(system_config.DefaultComport), textBox2.Text);
                Program_Configuration.UpdateSystem_Config(nameof(system_config.CSV_Path), textBox3.Text);
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (openfolder2.ShowDialog() == DialogResult.OK)
            {
               textBox1.Text = openfolder2.SelectedPath.ToString();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (openfolder3.ShowDialog() == DialogResult.OK)
            {
                textBox2.Text = openfolder3.SelectedPath.ToString();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
        public async Task Async_write(string jd)
        {
            void act()
            {
                try
                {
                    if(!Directory.Exists(system_config.DefaultComport + @"/" + Parameter_app.TXT_FOLDER_NAME))
                    {
                        Directory.CreateDirectory(system_config.DefaultComport + @"/" + Parameter_app.TXT_FOLDER_NAME);
                    }
                    if (!File.Exists(system_config.DefaultComport + @"/" + Parameter_app.TXT_FOLDER_NAME + @"/" + "IMG_NUM.txt"))
                    {
                        File.Create(system_config.DefaultComport + @"/" + Parameter_app.TXT_FOLDER_NAME + @"/" + "IMG_NUM.txt").Close();
                    }
                    using (StreamWriter sw = new StreamWriter(system_config.DefaultComport + @"/" + Parameter_app.TXT_FOLDER_NAME + @"/" + "IMG_NUM.txt"))
                    {
                        sw.WriteLine(jd);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

            };
            Task a = new Task(act);
            a.Start();
            await a;
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (openfolder4.ShowDialog() == DialogResult.OK)
            {
                textBox3.Text = openfolder4.SelectedPath.ToString();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {

            try
            {
                var b = new ObjectCSVData();
                b.Month = "11";
                b.Date = "12";
                b.Time = "13:12:58";
                b.Result = 1;
                b.ID_Bath = 2;
                //Csv.Add_Data(b);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                
            }
          
        }
    }
}
