using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging.Filters;

using System.IO.Ports;
using System.IO;
using AForge;
using System.Drawing.Imaging;
using S7.Net;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;



namespace Camera_Check_Component
{
    public partial class Camera_Check_component : Form
    {
        #region/////////////////////////////////////////////////////// DECLARE
        private FileSystemWatcher watcher;
        private FilterInfoCollection filterInfoCollection;
        private VideoCaptureDevice Cam1VIDEO_Device;
        private VideoCaptureDevice Cam2VIDEO_Device;
        private VideoCaptureDevice Cam3VIDEO_Device;
        private VideoCaptureDevice Cam4VIDEO_Device;
        private VideoCaptureDevice Cam5VIDEO_Device;
        private VideoCaptureDevice Cam6VIDEO_Device;
        private VideoCaptureDevice Cam7VIDEO_Device;
        private FilterInfo Cam1_Device;
        private FilterInfo Cam2_Device;
        private FilterInfo Cam3_Device;
        private FilterInfo Cam4_Device;
        private FilterInfo Cam5_Device;
        private FilterInfo Cam6_Device;
        private FilterInfo Cam7_Device;
        private System_config system_config;

        Bitmap Live_Cam_1;
        Bitmap Live_Cam_2;
        Bitmap Live_Cam_3;
        Bitmap Live_Cam_4;
        Bitmap Live_Cam_5;
        Bitmap Live_Cam_6;
        Bitmap Live_Cam_7;
        private BackgroundWorker backgroundWorker_1 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_2 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_3 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_4 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_5 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_6 = new BackgroundWorker();
        private BackgroundWorker backgroundWorker_7 = new BackgroundWorker();
        private BackgroundWorker ledinf = new BackgroundWorker();
        BackgroundWorker shot_pic = new BackgroundWorker();
        BackgroundWorker wdata = new BackgroundWorker();
        private SQL_action sql_action = new SQL_action();
        bool order_1 = false;
        bool order_2 = false;
        bool order_3 = false;
        bool order_4 = false;
        bool order_5 = false;
        bool order_6 = false;
        bool order_7 = false;

        bool start_check = false;
        bool allow_check = false;
        bool loadform = false;
        private System.Windows.Forms.Timer timer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer D_timer = new System.Windows.Forms.Timer();
        //private System.Windows.Forms.Timer cam_call_back = new System.Windows.Forms.Timer();

        private double startPR_Count = 0;
        private double timer_sum = 0;
        private double timer_star = 0;
        private Int64 count_1 = 0;
        private Int64 count_2 = 0;
        private Int64 count_3 = 0;
        private Int64 count_4 = 0;
        private Int64 count_5 = 0;
        private Int64 count_6 = 0;
        private Int64 count_7 = 0;
        private Int64 folderIndex = 0;
        private Int64 load1 = 0;
        private Int64 load2 = 0;
        bool started = false;
        double ratio;
        int stt = 0;
        string DMY = "";

        #endregion
        public Camera_Check_component()
        {
            InitializeComponent();
        }
        List<String> infos = new List<String>();
        #region////////////////////////////////////////////////////////////////////////////////////////////////SET UP
        private void Camera_Check_component_Load(object sender, EventArgs e)
        {
            if (loadform) return;
           

            button2.Enabled = true;
            button3.Enabled = true;
            Process[] Pname = Process.GetProcessesByName("comport");
            if (Pname.Length == 0)
            {
                
                Gen_check_Com.Checked = false;
            }
            else
            {

                Gen_check_Com.Checked = true;              
            }
            loadform = true;
            this.Location = new System.Drawing.Point(0, 0);
            system_config = Program_Configuration.GetSystem_Config();
            if (Screen.AllScreens.Length > 1)
            {
                //this.Size = Screen.PrimaryScreen.WorkingArea.Size;
                this.Width = 3840;
                this.Height = 1080;
            }
            else
            {
                this.Size = Screen.PrimaryScreen.WorkingArea.Size;
            }
            //unable();
            listviewInit();
            system_config = Program_Configuration.GetSystem_Config();
            DateTime dt = DateTime.Now;
            string daytime = dt.Day.ToString() + "-" + dt.Month.ToString() + "-" + dt.Year.ToString();
            DMY = daytime;
            foreach (DriveInfo di in DriveInfo.GetDrives())
            {
                infos.Add(di.Name);
            }
            string drive_letter = system_config.Map_Path_File_2.Substring(0, 1);
            label19.Text = "Disk"+ drive_letter;
            DriveInfo di1 = new DriveInfo(drive_letter);
            if (di1.IsReady)
            {
                progressBar2.Value = (int)((double)(di1.TotalSize - di1.AvailableFreeSpace) * 100 / di1.TotalSize);
            }
            else
            {
                progressBar2.BackColor = Color.Red;
            }
            if (system_config.PN_Selector != "" || system_config.PN_Selector != null)
            {
                tb_PN.Text = system_config.PN_Selector;

            }
            else
            {
                Program_Configuration.UpdateSystem_Config("PN_Selector", "xxx");
            }

            if (File.Exists(@"C:\Users\Admin\source\repos\Visual\Camera_Check_Component\bin\Debug\Output.txt"))
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\Admin\source\repos\Visual\Camera_Check_Component\bin\Debug\Output.txt"))
                {
                    int a = 0;
                    string[] read = new string[2];
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {

                        read[a] = line;
                        a++;

                    }
                    if (a > 1)
                    {
                        _OKnum = Convert.ToInt16(read[0]);
                        _NGnum = Convert.ToInt16(read[1]);
                        _sum = (short)(_NGnum + _OKnum);
                    }
                    else
                    {
                        _OKnum = 0;
                        _NGnum = 0;
                        _sum = (short)(_NGnum + _OKnum);
                    }

                }
            }
            if (count_1 != system_config.Location_cam1_folder || count_2 != system_config.Location_cam2_folder || count_3 != system_config.Location_cam3_folder || count_4 != system_config.Location_cam4_folder || count_5 != system_config.Location_cam5_folder || count_6 != system_config.Location_cam6_folder || count_7 != system_config.Location_cam7_folder || load1 != system_config.Folder_index_tranfer || load2 != system_config.Folder_load_check || folderIndex != system_config.same_folder_1)
            {
                folderIndex = system_config.same_folder_1;
                load1 = system_config.Folder_index_tranfer;
                load2 = system_config.Folder_load_check;
                count_1 = system_config.Location_cam1_folder;
                count_2 = system_config.Location_cam2_folder;
                count_3 = system_config.Location_cam3_folder;
                count_4 = system_config.Location_cam4_folder;
                count_5 = system_config.Location_cam5_folder;
                count_6 = system_config.Location_cam6_folder;
                count_7 = system_config.Location_cam7_folder;
            }
            Start_btn.Enabled = true;
            Stop_btn.Enabled = false;

            Pic_Cam1.SizeMode = PictureBoxSizeMode.StretchImage;
            Pic_Cam2.SizeMode = PictureBoxSizeMode.StretchImage;
            Pic_Cam3.SizeMode = PictureBoxSizeMode.StretchImage;
            Pic_Cam4.SizeMode = PictureBoxSizeMode.StretchImage;
            Pic_Cam5.SizeMode = PictureBoxSizeMode.StretchImage;
            Pic_Cam6.SizeMode = PictureBoxSizeMode.StretchImage;

            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox4.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox5.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox6.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox15.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox16.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox17.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox18.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox19.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox20.SizeMode = PictureBoxSizeMode.StretchImage;

            pic_full1.SizeMode = PictureBoxSizeMode.StretchImage;
            picfull_2.SizeMode = PictureBoxSizeMode.StretchImage;
            pic_full1.Hide();
            picfull_2.Hide();

            p811.Visible = false;
            p812.Visible = false;
            p831.Visible = false;
            p821.Visible = false;
            p832.Visible = false;
            p822.Visible = false;
            p841.Visible = false;
            p842.Visible = false;
            p851.Visible = false;
            p852.Visible = false;
            p861.Visible = false;
            p862.Visible = false;
            p871.Visible = false;
            p872.Visible = false;
            p881.Visible = false;
            p882.Visible = false;
            p411.Visible = false;
            p421.Visible = false;
            p412.Visible = false;
            p422.Visible = false;
            p431.Visible = false;
            p432.Visible = false;
            p441.Visible = false;
            p442.Visible = false;



            PB_active1.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active1.Hide();
            PB_active2.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active2.Hide();
            PB_active3.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active3.Hide();
            PB_active4.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active4.Hide();
            PB_active5.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active5.Hide();
            PB_active6.SizeMode = PictureBoxSizeMode.StretchImage;
            PB_active6.Hide();


            #region ///////////////////////////////////////////////////////////// khai báo background worker
            backgroundWorker_1.DoWork += backgroundWorker_1_DoWork;
            backgroundWorker_1.RunWorkerCompleted += backgroundWorker_1_RunWorkerCompleted;
            backgroundWorker_1.WorkerSupportsCancellation = true;

            backgroundWorker_2.DoWork += backgroundWorker_2_DoWork;
            backgroundWorker_2.RunWorkerCompleted += backgroundWorker_2_RunWorkerCompleted;
            backgroundWorker_2.WorkerSupportsCancellation = true;

            backgroundWorker_3.DoWork += backgroundWorker_3_DoWork;
            backgroundWorker_3.RunWorkerCompleted += backgroundWorker_3_RunWorkerCompleted;
            backgroundWorker_3.WorkerSupportsCancellation = true;

            backgroundWorker_4.DoWork += backgroundWorker_4_DoWork;
            backgroundWorker_4.RunWorkerCompleted += backgroundWorker_4_RunWorkerCompleted;
            backgroundWorker_4.WorkerSupportsCancellation = true;

            backgroundWorker_5.DoWork += backgroundWorker_5_DoWork;
            backgroundWorker_5.RunWorkerCompleted += backgroundWorker_5_RunWorkerCompleted;
            backgroundWorker_5.WorkerSupportsCancellation = true;

            backgroundWorker_6.DoWork += backgroundWorker_6_DoWork;
            backgroundWorker_6.RunWorkerCompleted += backgroundWorker_6_RunWorkerCompleted;
            backgroundWorker_6.WorkerSupportsCancellation = true;

            backgroundWorker_7.DoWork += BackgroundWorker_7_DoWork;
            backgroundWorker_7.RunWorkerCompleted += BackgroundWorker_7_RunWorkerCompleted;
            backgroundWorker_7.WorkerSupportsCancellation = true;

            ledinf.DoWork += Ledinf_DoWork;
            ledinf.RunWorkerCompleted += Ledinf_RunWorkerCompleted;
            ledinf.WorkerSupportsCancellation = true;
            shot_pic.DoWork += Shot_pic_DoWork;
            shot_pic.RunWorkerCompleted += Shot_pic_RunWorkerCompleted;
            shot_pic.WorkerSupportsCancellation = true;

            wdata.DoWork += Wdata_DoWork;
            wdata.RunWorkerCompleted += Wdata_RunWorkerCompleted;
            wdata.WorkerSupportsCancellation = true;
            #endregion
            Program_Configuration.UpdateSystem_Config("Location_cam1_folder", count_1.ToString());
            system_config.Location_cam1_folder = Convert.ToInt32(Program_Configuration.GetSystem_Config_Value("Location_cam1_folder"));

            //Parameter_app.OK_TEMP(daytime, system_config.Location_cam1_folder.ToString());
            //Parameter_app.ERROR_TEMP(daytime, system_config.Location_cam1_folder.ToString());
            //label_time.Text = DateTime.Now.ToString();
            if (system_config.inf_process == null)
            {
                TB_LTdate.Text = "";
            }
            else
            {
                TB_LTdate.Text = system_config.inf_process.ToString();
            }
            progressBar1.Minimum = 0;
            progressBar1.Maximum = 100;
            timer.Interval = 1000;
            timer.Tick += Timer_Tick;
            timer.Enabled = true;
            call_back_cam.Enabled = false;
            D_timer.Tick += D_timer_Tick;
            D_timer.Enabled = false;
            D_timer.Interval = 1000;
            timer.Start();
            //TB_idworker.Enabled = false;
            //TB_wker2.Enabled = false;
            //textBox_stt.Enabled = false;           
            //listView1.Enabled = false;
            listView1.Scrollable = true;
            OKnum.Text = _OKnum.ToString();
            NGnum.Text = _NGnum.ToString();
            totalPN.Text = _sum.ToString();

        }
        int coun_d = 0;
        bool d_check = false;
       
        private void D_timer_Tick(object sender, EventArgs e)
        {
            coun_d++;           
            if (coun_d == 3) 
            {
                if (d_check) 
                {
                    MethodInvoker inv = delegate 
                    {
                        PLCS7_1200.Write("M103.5", true);
                    };this.Invoke(inv);                 
                }
                coun_d = 0;
                d_check = false;
                D_timer.Stop();
                D_timer.Enabled = false;
               
            }

        }

        private void Wdata_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!wdata.IsBusy) wdata.RunWorkerAsync();
        }

        private void Wdata_DoWork(object sender, DoWorkEventArgs e)
        {
            if (w1)
            {
                string Addr = "DB5.DBX26.0";
                PLCS7_1200.Write(Addr, true);
                w1 = false;
            }
            if (w2)
            {
                string Addr = "DB5.DBX26.1";
                PLCS7_1200.Write(Addr, true);
                w2 = false;
            }
            if (w3)
            {
                string Addr = "DB5.DBX26.2";
                PLCS7_1200.Write(Addr, true);
                w3 = false;
            }
            if (w4)
            {
                string Addr = "DB5.DBX26.3";
                PLCS7_1200.Write(Addr, true);
                w4 = false;
                Thread.Sleep(5);
            }
            if (w5)
            {
                string Addr = "DB5.DBX26.4";
                PLCS7_1200.Write(Addr, true);
                w5 = false;
            }
            if (w6)
            {

                string Addr = "DB5.DBX26.5";
                PLCS7_1200.Write(Addr, true);
                w6 = false;
            }

        }

        private void Ledinf_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //if (!ledinf.IsBusy) ledinf.RunWorkerAsync();
        }

        private void Ledinf_DoWork(object sender, DoWorkEventArgs e)
        {

           
        }

        private void unable()
        {

            foreach (Control ctl in General_tab.Controls)
            {
                if (ctl.Name == "tabPage3")
                {
                    ctl.Enabled = true;
                    foreach (Control ctrl in tabPage3.Controls)
                    {
                        if (ctrl.Name == "login_btn")
                        {
                            ctrl.Enabled = true;

                        }
                        else
                        {
                            ctrl.Enabled = false;
                        }
                    }
                }
                else
                {
                    ctl.Enabled = false;
                }
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {

            timer_sum++;
            if (started)
            {
                startPR_Count++;
                TimeSpan time = TimeSpan.FromSeconds(startPR_Count);
                LB_TIMER.Text = time.ToString(@"hh\:mm\:ss");
                timer_star++;
                //  if (serialPort_communicate.IsOpen) serialPort_communicate.Close();
                //if (!serialPort_communicate.IsOpen) serialPort_communicate.Open();
                if (General_tab.SelectedIndex == 2 && run_out1 && folderIndex < count_6)
                {
                    upload_image();
                    run_out1 = false;
                }
                if (General_tab.SelectedIndex == 2 && run_out2 && folderIndex < count_6)
                {
                    update_image2();
                    run_out2 = false;
                }
            }
            else
            {
                ratio = (timer_star / timer_sum) * 100;
            }

            ratio = (timer_star / timer_sum) * 100;
            progressBar1.Value = Convert.ToInt32(ratio);
            LB_oee.Text = Convert.ToInt32(ratio).ToString() + "%";

        }
        private void listviewInit()
        {
            listView1.View = View.Details;
            listView1.Columns.Add("PN Selector");
            listView1.Columns.Add("Status");
            listView1.Columns.Add("Error Type");
            listView1.Columns[1].Width = 42;
            listView1.Columns[0].Width = 140;
            listView1.Columns[2].Width = 140;
        }
        private void set_up()
        {

            if (!Directory.Exists(system_config.Map_Path_File + @"/" + Parameter_app.IMAGE_FOLDER_PATH))
            {
                Directory.CreateDirectory(system_config.Map_Path_File + @"/" + Parameter_app.IMAGE_FOLDER_PATH);
            }

        }
        #endregion

        #region ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////CHỤP ẢNH
        private void BackgroundWorker_7_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (w7)
            {
                shot7 = "";
                taked7 = false;
                count_7++;
                //MethodInvoker inv = delegate
                //{
                Cam7VIDEO_Device.SignalToStop();
                status(" [SYSTEM]" + " CAM 7 Save image" + " " + count_7.ToString());
                Live_Cam_7.Dispose();
                order_7 = false;
            }


        }
        private void BackgroundWorker_7_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_7.CancellationPending)
            {
                e.Cancel = true;
            }

            if (system_config.add_cam == "true")
            {

                DateTime date = DateTime.Now;
                date.ToString("HH:MM:ss");

                set_up();
                //MethodInvoker inv = delegate
                //{
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-7" + "-" + count_7.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + str + "";

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //   RotateImage(Live_Cam_6, 45).Save(memory, ImageFormat.Jpeg);
                        Live_Cam_7.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                        if (File.Exists(outputFileName))
                        {
                            w7 = true;

                            //MethodInvoker inv1 = delegate
                            //{
                            //    //string Addr = "DB20.DBX0.5";
                            //    //PLCS7_1200.Write(Addr, true);
                            //}; this.Invoke(inv1);
                        }
                    }
                }
            }
        }
        void backgroundWorker_6_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (w6)
            {
                shot6 = "";
                taked6 = false;
                panel6.BackColor = Color.Black;

                count_6++;
               
                status(" [SYSTEM]" + " CAM 6 Save image" + " " + count_6.ToString());
                Live_Cam_6.Dispose();
                order_6 = false;
            }

        }
        void backgroundWorker_6_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_6.CancellationPending)
            {
                e.Cancel = true;
            }

            try
            {
                DateTime date = DateTime.Now;
                date.ToString("HH:MM:ss");
                panel6.BackColor = Color.GreenYellow;
                set_up();
                //MethodInvoker inv = delegate
                //{
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-6" + "-" + count_6.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + str + "";

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //   RotateImage(Live_Cam_6, 45).Save(memory, ImageFormat.Jpeg);
                        Live_Cam_6.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                        if (File.Exists(outputFileName))
                        {
                            //Cam6VIDEO_Device.Stop();
                           
                            w6 = true;

                            MethodInvoker inv1 = delegate
                            {
                                string Addr = "DB20.DBX0.5";
                                PLCS7_1200.Write(Addr, true);
                            }; this.Invoke(inv1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);

            }

        }


        void backgroundWorker_5_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (w5)
            {
                taked5 = false;
                shot5 = "";
                panel5.BackColor = Color.Black;

                count_5++;
                
                status(" [SYSTEM]" + " CAM 5 Save image" + " " + count_5.ToString());
                Live_Cam_5.Dispose();
                order_5 = false;
                w5 = false;
            }


        }

        void backgroundWorker_5_DoWork(object sender, DoWorkEventArgs e)
        {

            if (backgroundWorker_5.CancellationPending)
            {
                e.Cancel = true;
            }
            try
            {

                DateTime date = DateTime.Now;
                date.ToString("HH:MM:ss");
                panel5.BackColor = Color.GreenYellow;
                set_up();
                //MethodInvoker inv = delegate
                //{
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-5" + "-" + count_5.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + str + "";

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //RotateImage(Live_Cam_5, 90).Save(memory, ImageFormat.Jpeg);
                        Live_Cam_5.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                        if (File.Exists(outputFileName))
                        {
                            //Cam5VIDEO_Device.Stop();
                        
                            w5 = true;
                            MethodInvoker inv1 = delegate
                            {
                                string Addr = "DB20.DBX0.4";
                                PLCS7_1200.Write(Addr, true);
                            }; this.Invoke(inv1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);

            }

        }
        void backgroundWorker_4_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (w4)
            {

                w4 = false;
                taked4 = false;
                shot4 = "";
                panel4.BackColor = Color.Black;

                count_4++;
               
                status(" [SYSTEM]" + " CAM 4 Save image" + " " + count_4.ToString());
                Live_Cam_4.Dispose();
                order_4 = false;
            }


        }

        void backgroundWorker_4_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_4.CancellationPending)
            {
                e.Cancel = true;
            }
            try
            {
                DateTime date = DateTime.Now;
                date.ToString("HH:MM:ss");

                panel4.BackColor = Color.GreenYellow;
                set_up();
                //MethodInvoker inv = delegate
                //{
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-4" + "-" + count_4.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + str + "";

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //RotateImage(Live_Cam_4, 90).Save(memory, ImageFormat.Jpeg);
                        Live_Cam_4.Save(memory, ImageFormat.Jpeg);

                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                        if (File.Exists(outputFileName))
                        {
                            //Cam4VIDEO_Device.Stop();
                   
                            w4 = true;
                            MethodInvoker inv1 = delegate
                            {
                                string Addr = "DB20.DBX0.3";
                                PLCS7_1200.Write(Addr, true);
                            }; this.Invoke(inv1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);

            }

        }

        void backgroundWorker_3_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (w3)
            {

                taked3 = false;
                shot3 = "";
                panel3.BackColor = Color.Black;

                count_3++;
                

                status(" [SYSTEM]" + " CAM 3 Save image" + " " + count_3.ToString());
                w3 = false;
                Live_Cam_3.Dispose();
                order_3 = false;
            }

        }
        void backgroundWorker_3_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_3.CancellationPending)
            {
                e.Cancel = true;
            }
            try
            {
                DateTime date = DateTime.Now;
                date.ToString("HH:MM:ss");

                panel3.BackColor = Color.GreenYellow;
                set_up();
                //MethodInvoker inv = delegate
                //{
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-3" + "-" + count_3.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + str + "";

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //RotateImage(Live_Cam_3, -90).Save(memory, ImageFormat.Jpeg);
                        Live_Cam_3.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                        
                        if (File.Exists(outputFileName))
                        {
                            //Cam3VIDEO_Device.Stop();
                  
                            w3 = true;
                            MethodInvoker inv1 = delegate
                            {
                                string Addr = "DB20.DBX0.2";
                                PLCS7_1200.Write(Addr, true);
                            }; this.Invoke(inv1);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);

            }

        }

        void backgroundWorker_2_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (w2)
            {
                w2 = false;
                taked2 = false;
                shot2 = "";
                panel2.BackColor = Color.Black;

                count_2++;
               
                status(" [SYSTEM]" + " CAM 2 Save image" + " " + count_2.ToString());
                Live_Cam_2.Dispose();
                order_2 = false;
            }



        }
        void backgroundWorker_2_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_2.CancellationPending)
            {
                e.Cancel = true;
            }
            try
            {
                panel2.BackColor = Color.GreenYellow;
                DateTime date = DateTime.Now;
                date.ToString("HH:MM:ss");

                set_up();
                //MethodInvoker inv = delegate
                //{
                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-2" + "-" + count_2.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + str + "";

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {
                        //RotateImage(Live_Cam_2, -90).Save(memory, ImageFormat.Jpeg);
                        Live_Cam_2.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                        if (File.Exists(outputFileName))
                        {
                            //Cam2VIDEO_Device.Stop();
                   
                            w2 = true;
                            MethodInvoker inv1 = delegate
                            {
                                string Addr = "DB20.DBX0.1";
                                PLCS7_1200.Write(Addr, true);
                            }; this.Invoke(inv1);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);

            }

        }
        int cc = 0;
        bool w1 = false;
        bool w2 = false;
        bool w3 = false;
        bool w4 = false;
        bool w5 = false;
        bool w6 = false;
        void backgroundWorker_1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (w1)
            {
                taked1 = false;
                cc++;
                Int64 n = count_1 / 6;
                count_1++;
                panel1.BackColor = Color.Black;
                shot1 = "";
                
                status(" [SYSTEM]" + " CAM 1 Save image" + " " + count_1.ToString());
                w1 = false;
                Live_Cam_1.Dispose();
                order_1 = false;
            }

        }

        void backgroundWorker_1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (backgroundWorker_1.CancellationPending)
            {
                e.Cancel = true;
            }
            try
            {

                panel1.BackColor = Color.GreenYellow;
                DateTime date = DateTime.Now;
                date.ToString("HH:MM:ss");

                set_up();

                string str = PN_Selector + "-" + date.Day.ToString() + "." + date.Month.ToString() + "." + date.Year.ToString() + "-" + date.Hour.ToString() + "-" + date.Minute.ToString() + "-" + date.Second.ToString() + "-1" + "-" + count_1.ToString() + ".jpeg";
                string outputFileName = system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + str + "";

                using (MemoryStream memory = new MemoryStream())
                {
                    using (FileStream fs = new FileStream(outputFileName, FileMode.Create, FileAccess.ReadWrite))
                    {

                        Live_Cam_1.Save(memory, ImageFormat.Jpeg);
                        byte[] bytes = memory.ToArray();
                        fs.Write(bytes, 0, bytes.Length);
                        fs.Dispose();
                        if (File.Exists(outputFileName))
                        {
                           
             
                            w1 = true;
                            MethodInvoker inv1 = delegate
                            {
                                string Addr = "DB20.DBX0.0";
                                PLCS7_1200.Write(Addr, true);
                            }; this.Invoke(inv1);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);

            }

        }
        bool taked7 = false;
        bool w7 = false;
        void Cam7VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (order_7 && system_config.add_cam == "true")
            {
                if (!taked7)
                {
                    Live_Cam_7 = (Bitmap)eventArgs.Frame.Clone();
                    taked7 = true;
                    if (!backgroundWorker_7.IsBusy) backgroundWorker_7.RunWorkerAsync();
                }

            }
            else if (Live_Cam_7 != null)
            {
                w7 = false;
                taked7 = false;
                Live_Cam_7.Dispose();

            }
            Cam7VIDEO_Device.Stop();
        }
        void Cam6VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (order_6)
            {
                if (!taked6)
                {
                    Live_Cam_6 = (Bitmap)eventArgs.Frame.Clone();
                    taked6 = true;
                    if (!backgroundWorker_6.IsBusy) backgroundWorker_6.RunWorkerAsync();
                }
            }
            //else if (Live_Cam_6 != null)
            //{
            //    w6 = false;
            //    taked6 = false;
            //    Live_Cam_6.Dispose();

            //}
            Cam6VIDEO_Device.SignalToStop();
        }
        bool taked1 = false;
        bool taked2 = false;
        bool taked3 = false;
        bool taked4 = false;
        bool taked5 = false;
        bool taked6 = false;

        void Cam5VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (order_5)
            {
                if (!taked5)
                {
                    Live_Cam_5 = (Bitmap)eventArgs.Frame.Clone();
                    taked5 = true;
                    if (!backgroundWorker_5.IsBusy) backgroundWorker_5.RunWorkerAsync();
                }


            }
            //else if (Live_Cam_5 != null)
            //{
            //    w5 = false;
            //    taked5 = false;
            //    Live_Cam_5.Dispose();

            //}
            Cam5VIDEO_Device.SignalToStop();
        }

        void Cam4VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (order_4)
            {
                if (!taked4)
                {
                    Live_Cam_4 = (Bitmap)eventArgs.Frame.Clone();
                    taked4 = true;
                    if (!backgroundWorker_4.IsBusy) backgroundWorker_4.RunWorkerAsync();
                }

            }
            //else if (Live_Cam_4 != null)
            //{
            //    w4 = false;
            //    taked4 = false;
            //    Live_Cam_4.Dispose();

            //}
            Cam4VIDEO_Device.SignalToStop();
        }

        void Cam3VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (order_3)
            {
                if (!taked3)
                {
                    taked3 = true;
                    Live_Cam_3 = (Bitmap)eventArgs.Frame.Clone();
                    if (!backgroundWorker_3.IsBusy) backgroundWorker_3.RunWorkerAsync();
                }

            }
            //else if (Live_Cam_3 != null)
            //{
            //    w3 = false;
            //    taked3 = false;
            //    Live_Cam_3.Dispose();

            //}
            Cam3VIDEO_Device.SignalToStop();
        }

        void Cam2VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            if (order_2)
            {
                if (!taked2)
                {
                    taked2 = true;
                    Live_Cam_2 = (Bitmap)eventArgs.Frame.Clone();
                    if (!backgroundWorker_2.IsBusy) backgroundWorker_2.RunWorkerAsync();
                }


            }
            //else if (Live_Cam_2 != null)
            //{
            //    w2 = false;
            //    taked2 = false;
            //    Live_Cam_2.Dispose();

            //}
            Cam2VIDEO_Device.SignalToStop();
        }
        void Cam1VIDEO_Device_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {

            if (order_1)
            {
                if (!taked1)
                {
                    taked1 = true;
                    Live_Cam_1 = (Bitmap)eventArgs.Frame.Clone();
                    if (!backgroundWorker_1.IsBusy) backgroundWorker_1.RunWorkerAsync();
                }


            }
            //else if (Live_Cam_1 != null)
            //{
            //    taked1 = false;
            //    //Live_Cam_1.Dispose();
            //    w1 = false;

            //}
            Cam1VIDEO_Device.SignalToStop();
        }
        #endregion

        #region ///////////////////////////////////////////////////////////////////////////////////////////////////////////////program process
        private void Start_btn_Click(object sender, EventArgs e)
        {
           if(!checkBox4.Checked) 
            {
                MessageBox.Show("Vui lòng thực hiện đủ các bước trước khi khỏi động", "Information", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            system_config = Program_Configuration.GetSystem_Config();
            Start_program();
            if (File.Exists(@"C:\Users\Admin\source\repos\Visual\Camera_Check_Component\bin\Debug\Output.txt"))
            {
                using (StreamReader sr = new StreamReader(@"C:\Users\Admin\source\repos\Visual\Camera_Check_Component\bin\Debug\Output.txt"))
                {
                    int a = 0;
                    string[] read = new string[2];
                    string line = "";
                    while ((line = sr.ReadLine()) != null)
                    {

                        read[a] = line;
                        a++;

                    }
                    if (a > 1)
                    {
                        _OKnum = Convert.ToInt16(read[0]);
                        _NGnum = Convert.ToInt16(read[1]);
                        _sum = (short)(_NGnum + _OKnum);
                    }
                    else
                    {
                        _OKnum = 0;
                        _NGnum = 0;
                        _sum = (short)(_NGnum + _OKnum);
                    }

                }
            }
        }

        string ID_Operator1 = "";
        string ID_Operator2 = "";
        string PN_Selector = "";
        private void Start_program()
        {
            if (!PLC_con)
            {
                MessageBox.Show("PLC is not connect, Please Connect PLC first");
                return;
            }
            ID_Operator1 = TB_idworker.Text;
            ID_Operator2 = TB_wker2.Text;
            PN_Selector = tb_PN.Text;

            Program_Configuration.UpdateSystem_Config("PN_Selector", PN_Selector);
            system_config = Program_Configuration.GetSystem_Config();
            if (tb_PN.Text == "" || TB_idworker.Text == "" || TB_wker2.Text == "")
            {
                MessageBox.Show("DO NOT HAVE PN Selector or ID Operator");
                return;
            }
            if (!Directory.Exists(system_config.Map_Path_File))
            {
                MessageBox.Show("Could not find Map Path File, please check setting again");
                status("[START] Could not find Map Path File");
                Start_btn.Enabled = true;
                Stop_btn.Enabled = false;
                return;
            }
            tb_PN.Enabled = false;
            TB_idworker.Enabled = false;
            TB_wker2.Enabled = false;
            system_config = Program_Configuration.GetSystem_Config();
            filterInfoCollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            
            foreach (FilterInfo item in filterInfoCollection)
            {
                if (item.MonikerString == system_config.Camera1)
                {
                    Cam1_Device = item;
                }
                else if (item.MonikerString == system_config.Camera2)
                {
                    Cam2_Device = item;
                }
                else if (item.MonikerString == system_config.Camera3)
                {
                    Cam3_Device = item;
                }
                else if (item.MonikerString == system_config.Camera4)
                {
                    Cam4_Device = item;
                }
                else if (item.MonikerString == system_config.Camera5)
                {
                    Cam5_Device = item;
                }
                else if (item.MonikerString == system_config.Camera6)
                {
                    Cam6_Device = item;
                }

            }
            if (Cam1_Device == null)
            {
                MessageBox.Show("Camera 1 is not available, please check connection setting of device and preview");
                status("[START Camera 1 is not availble]");
                return;
            }
            if (Cam2_Device == null)
            {
                MessageBox.Show("Camera 2 is not available, please check connection setting of device and preview");
                status("[START Camera 2 is not availble]");
                return;
            }
            if (Cam3_Device == null)
            {
                MessageBox.Show("Camera 3 is not available, please check connection setting of device and preview");
                status("[START Camera 3 is not availble]");
                return;
            }
            if (Cam4_Device == null)
            {
                MessageBox.Show("Camera 4 is not available, please check connection setting of device and preview");
                status("[START Camera 4 is not availble]");
                return;
            }
            if (Cam5_Device == null)
            {
                MessageBox.Show("Camera 5 is not available, please check connection setting of device and preview");
                status("[START Camera 5 is not availble]");
                return;
            }
            if (Cam6_Device == null)
            {
                MessageBox.Show("Camera 6 is not available, please check connection setting of device and preview");
                status("[START Camera 6 is not availble]");
                return;
            }
            if (Cam7_Device == null && system_config.add_cam == "true")
            {
                MessageBox.Show("Camera 7 is not available, please check connection setting of device and preview");
                status("[START Camera 7 is not availble]");
                return;
            }
            //try
            //{
            //    if (serialPort_communicate.IsOpen) serialPort_communicate.Close();
            //    serialPort_communicate.PortName = system_config.DefaultComport;
            //    serialPort_communicate.BaudRate = Convert.ToInt32(system_config.DefaultCOMBaudrate);

            //    serialPort_communicate.Open();
            //    status("[COMPORT] Comport " + serialPort_communicate.PortName + " Connected");

            //}
            //catch (Exception)
            //{
            //    MessageBox.Show(system_config.DefaultComport + " Not Existing, please try another one");
            //    status(" [COMPORT] Comport " + serialPort_communicate.PortName + " Not found");
            //    RESET();
            //    return;
            //}
            // Com_setting_form.open_serial();
            if (Cam1VIDEO_Device == null || !Cam1VIDEO_Device.IsRunning)
            {
                Cam1VIDEO_Device = new VideoCaptureDevice(Cam1_Device.MonikerString);
                Cam1VIDEO_Device.VideoResolution = Cam1VIDEO_Device.VideoCapabilities[system_config.pixel_cam1];
                Cam1VIDEO_Device.NewFrame += Cam1VIDEO_Device_NewFrame;
                Cam1VIDEO_Device.Start();
                PB_active1.Show();
            }

            if (Cam2VIDEO_Device == null || !Cam2VIDEO_Device.IsRunning)
            {
                Cam2VIDEO_Device = new VideoCaptureDevice(Cam2_Device.MonikerString);
                Cam2VIDEO_Device.VideoResolution = Cam2VIDEO_Device.VideoCapabilities[system_config.pixel_cam2];
                Cam2VIDEO_Device.NewFrame += Cam2VIDEO_Device_NewFrame;
                Cam2VIDEO_Device.Start();
                PB_active2.Show();
            }
            if (Cam3VIDEO_Device == null || !Cam3VIDEO_Device.IsRunning)
            {
                Cam3VIDEO_Device = new VideoCaptureDevice(Cam3_Device.MonikerString);
                Cam3VIDEO_Device.VideoResolution = Cam3VIDEO_Device.VideoCapabilities[system_config.pixel_cam3];
                Cam3VIDEO_Device.NewFrame += Cam3VIDEO_Device_NewFrame;
                Cam3VIDEO_Device.Start();
                PB_active3.Show();

            }

            if (Cam4VIDEO_Device == null || !Cam4VIDEO_Device.IsRunning)
            {
                Cam4VIDEO_Device = new VideoCaptureDevice(Cam4_Device.MonikerString);
                Cam4VIDEO_Device.VideoResolution = Cam4VIDEO_Device.VideoCapabilities[system_config.pixel_cam4];
                Cam4VIDEO_Device.NewFrame += Cam4VIDEO_Device_NewFrame;
                Cam4VIDEO_Device.Start();
                PB_active4.Show();
            }
            if (Cam5VIDEO_Device == null || !Cam5VIDEO_Device.IsRunning)
            {
                Cam5VIDEO_Device = new VideoCaptureDevice(Cam5_Device.MonikerString);
                Cam5VIDEO_Device.VideoResolution = Cam5VIDEO_Device.VideoCapabilities[system_config.pixel_cam5];
                Cam5VIDEO_Device.NewFrame += Cam5VIDEO_Device_NewFrame;
                Cam5VIDEO_Device.Start();
                PB_active5.Show();
            }

            if (Cam6VIDEO_Device == null || !Cam6VIDEO_Device.IsRunning)
            {
                Cam6VIDEO_Device = new VideoCaptureDevice(Cam6_Device.MonikerString);
                Cam6VIDEO_Device.VideoResolution = Cam6VIDEO_Device.VideoCapabilities[system_config.pixel_cam6];
                Cam6VIDEO_Device.NewFrame += Cam6VIDEO_Device_NewFrame;
                Cam6VIDEO_Device.Start();
                PB_active6.Show();
            }
            if (system_config.add_cam == "true")
            {
                if (Cam7VIDEO_Device == null || !Cam7VIDEO_Device.IsRunning)
                {
                    Cam7VIDEO_Device = new VideoCaptureDevice(Cam7_Device.MonikerString);
                    Cam7VIDEO_Device.VideoResolution = Cam7VIDEO_Device.VideoCapabilities[system_config.pixel_cam7];

                  
                    Cam7VIDEO_Device.NewFrame += Cam7VIDEO_Device_NewFrame;
                    Cam7VIDEO_Device.Start();
                }
            }
            system_config = Program_Configuration.GetSystem_Config();
            watcher = new FileSystemWatcher(Path.GetDirectoryName(@"C:\Users\Admin\source\repos\comport\comport\bin\Debug\Output.txt"));
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.txt";
            watcher.Changed += Watcher_Changed;
            watcher.EnableRaisingEvents = true;
            timer.Start();
            Start_btn.Enabled = false;
            Stop_btn.Enabled = true;
            start_check = true;
            started = true;

            status("[START]" + "Program has been started");
        }
        
        int j = 0;
        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (e.FullPath == @"C:\Users\Admin\source\repos\comport\comport\bin\Debug\Output.txt")
            {
                j++;
                if (j == 2)
                {
                    read_data();
                }

            }
        }
        private void read_data()
        {
            if (InvokeRequired) this.Invoke(new MethodInvoker(read_data));
            else
            {

                try
                {
                    string cap_order = File.ReadAllText(@"C:\Users\Admin\source\repos\comport\comport\bin\Debug\Output.txt");
                    cap_order.Trim(new char[] { '\r', '\n' });
                    string[] shot = new string[7];
                    string[] NG_code = new string[3];
                    //Thread.Sleep(10);
                    j = 0;
                    status("[RS232] " + cap_order + "");
                    switch (cap_order)
                    {
                        case ("a"):  //take_photo
                            PLCS7_1200.Write("M100.2", true);
                            var reada = PLCS7_1200.Read(DataType.DataBlock, 117, 0, VarType.String, 20);
                            var reada1 = reada.ToString().Substring(2, 13);
                            status(reada1);
                            camera_readA(reada1);


                            break;
                        case ("b")://hmi1
                            PLCS7_1200.Write("M100.2", true);
                            var readb = PLCS7_1200.Read(DataType.DataBlock, 117, 256, VarType.String, 20);
                            var readb1 = readb.ToString().Substring(2, 13);
                            status(readb1);
                            HMI1_readB(readb1);



                            break;
                        case ("c")://hmi2
                            PLCS7_1200.Write("M100.2", true);
                            var readc = PLCS7_1200.Read(DataType.DataBlock, 117, 512, VarType.String, 20);
                            var readc1 = readc.ToString().Substring(2, 13);
                            status(readc1);
                            HMI2_readC(readc1);
                            break;
                        case ("d")://confirm
                            PLCS7_1200.Write("M100.2", true);
                            var readd = PLCS7_1200.Read(DataType.DataBlock, 117, 768, VarType.String, 20);
                            var readd1 = readd.ToString().Substring(2, 13);
                            status(readd1);
                            ReadD(readd1);

                            break;
                        case ("e"):                                                       
                            coun_d = -1;
                            PLCS7_1200.Write("M100.2", true);
                            break;
                        case ("r"):
                            fall_pn ++;
                            PLCS7_1200.Write("M121.6", true);
                            PLCS7_1200.ReadClass(cl4, 117, 1024);
                            double r = cl4.Data_product;

                            fall_number[fall_pn] = Convert.ToInt16(r-1);

                        fall_down((r-1).ToString()); ;
                            break;
                    }

                }
                catch (Exception ex)
                {
                    status(ex.Message);
                    MessageBox.Show(ex.Message);

                }
            }
        }

        private void camera_readA(string cap_order)
        {

            if (cap_order.Contains('.'))
            {
                if (cap_order.Length == 13)
                {
                    string[] shot = new string[7];
                    shot = cap_order.Split('.');
                    shot1 = shot[0];
                    shot2 = shot[1];
                    shot3 = shot[2];
                    shot4 = shot[3];
                    shot5 = shot[4];
                    shot6 = shot[5];
                    shot7 = shot[6];
                   
                    if (shot6 == "1")
                    {
                        order_6 = true;
                        //Take_Photo();
                    }
                    if (shot5 == "1")
                    {
                        order_5 = true;
                        //Take_Photo();
                    }
                    if (shot4 == "1")
                    {
                        order_4 = true;
                        //Take_Photo();
                    }
                    if (shot1 == "1")
                    {
                        order_1 = true;
                        //Take_Photo();
                    }
                    if (shot2 == "1")
                    {
                        order_2 = true;
                        //Take_Photo();
                    }
                    if (shot3 == "1")
                    {
                        order_3 = true;
                        //Take_Photo();
                    }
                    if (shot7 == "1")
                    {
                        order_7 = true;
                        //Take_Photo();
                    }
                    MethodInvoker inv1 = delegate
                    {
                        if (call_back_cam.Enabled)
                        {
                            time_to_reset_cam = 0;
                            call_back_cam.Stop();
                            call_back_cam.Enabled = false;
                           

                        }
                        if (!call_back_cam.Enabled)
                        {
                            time_to_reset_cam = 0;
                            call_back_cam.Enabled = true;
                            call_back_cam.Start();
                        }
                    };this.Invoke(inv1);
                    Take_Photo();
                }
            }

        }
        private void HMI1_readB(string readb1)
        {
            if (readb1.Contains(';'))
            {
                if (readb1.Length == 13)
                {
                    string[] OK_NG = new string[7];
                    OK_NG = readb1.Split(';');
                    if (OK_NG[0] == "OK1" && allow_check && !run_out1)
                    {
                        if (on1 != 1)
                        {
                            if (En_chek1.Checked)
                            {
                                try
                                {

                                    ready1 = true;
                                    begin_ok1 = true;
                                    wr1 = false;
                                    string Addr = "M26.1";
                                    PLCS7_1200.Write(Addr, true);
                                    d_check = true;
                                    MethodInvoker inv1 = delegate
                                    {
                                        D_timer.Enabled = true;
                                        D_timer.Start();
                                    }; this.Invoke(inv1);
                                }
                                catch (Exception)
                                {
                                    ready1 = false;
                                    begin_ok1 = false;
                                    wr1 = true;
                                    //tranfer1 = 0;
                                }

                            }

                        }

                        if (on1 == 1 && !wr1)
                        {


                            if (En_chek1.Checked)
                            {
                                try
                                {

                                    ready1 = true;
                                    begin_ok1 = true;
                                    wr1 = false;
                                    string Addr = "M26.1";
                                    PLCS7_1200.Write(Addr, true);
                                    d_check = true;
                                    MethodInvoker inv1 = delegate
                                    {
                                        D_timer.Enabled = true;
                                        D_timer.Start();
                                    }; this.Invoke(inv1);
                                }
                                catch (Exception)
                                {
                                    ready1 = false;
                                    begin_ok1 = false;
                                    wr1 = true;
                                    //tranfer1 = 0;
                                }

                            }

                        }

                    }
                    if (OK_NG[0] == "NG1" && allow_check && !run_out1)
                    {
                        if (on1 != 1)
                        {
                            if (En_chek1.Checked)
                            {
                                try
                                {

                                    begin_ng1 = true;
                                    ready1 = true;
                                    loi_tam1 = OK_NG[1];
                                    vitri_Erpic(OK_NG[2]);
                                    err_pic1 = (OK_NG[2]);
                                    wr1 = false;
                                    string Addr = "M26.1";
                                    PLCS7_1200.Write(Addr, true);
                                    d_check = true;
                                    MethodInvoker inv1 = delegate 
                                    {
                                        D_timer.Enabled = true;
                                        D_timer.Start();
                                    };this.Invoke(inv1);
                                    
                                }
                                catch (Exception)
                                {
                                    begin_ng1 = false;
                                    ready1 = false;
                                    wr1 = true;
                                }

                            }

                        }

                        if (on1 == 1 && !wr1)
                        {


                            if (En_chek1.Checked)
                            {
                                try
                                {

                                    begin_ng1 = true;
                                    ready1 = true;
                                    loi_tam1 = OK_NG[1];
                                    vitri_Erpic(OK_NG[2]);
                                    err_pic1 = (OK_NG[2]);
                                    wr1 = false;
                                    string Addr = "M26.1";
                                    PLCS7_1200.Write(Addr, true);
                                    d_check = true;
                                    MethodInvoker inv1 = delegate
                                    {
                                        D_timer.Enabled = true;
                                        D_timer.Start();
                                    }; this.Invoke(inv1);
                                }
                                catch (Exception)
                                {
                                    begin_ng1 = false;
                                    ready1 = false;
                                    wr1 = true;
                                }

                            }

                        }

                    }

                }
            }
            if (readb1.Contains('#') && readb1.Length == 13)
            {
                string[] scale = new string[7];
                scale = readb1.Split('#');
                if (scale[0] == "TT1")
                {
                    zoom1(1, true, 1);
                }
                if (scale[0] == "LL1")
                {
                    zoom1(1, true, 2);
                }
                if (scale[0] == "Z11" && allow_check)
                {
                    //zoom1(1);
                    zoom1(1, false, 0);
                }
                if (scale[0] == "Z12" && allow_check)
                {
                    zoom1(2, false, 0);
                }
                if (scale[0] == "Z13" && allow_check)
                {
                    //zoom1(3);
                    zoom1(3, false, 0);
                }
                if (scale[0] == "Z14" && allow_check)
                {
                    zoom1(4, false, 0);
                }
                if (scale[0] == "Z15" && allow_check)
                {
                    zoom1(5, false, 0);
                }
                if (scale[0] == "Z16" && allow_check)
                {
                    zoom1(6, false, 0);
                }
            }
        }
        private void HMI2_readC(string readc1)
        {
            if (readc1.Contains(';') && readc1.Length == 13)
            {
                string[] OK_NG = new string[7];
                OK_NG = readc1.Split(';');
                if (OK_NG[0] == "OK2" && allow_check && !run_out2)
                {
                    if (on2 != 1)
                    {
                        if (En_chek2.Checked)
                        {
                            try
                            {

                                begin_ok2 = true;
                                ready2 = true;
                                wr2 = false;
                                string Addr = "M27.1";
                                PLCS7_1200.Write(Addr, true);
                                d_check = true;
                                MethodInvoker inv1 = delegate
                                {
                                    D_timer.Enabled = true;
                                    D_timer.Start();
                                }; this.Invoke(inv1);
                            }
                            catch (Exception)
                            {
                                begin_ok2 = false;
                                ready2 = false;
                                wr2 = true;
                                //tranfer2 = 0;
                            }

                        }

                    }

                    if (on2 == 1 && !wr2)
                    {


                        if (En_chek2.Checked)
                        {
                            try
                            {

                                begin_ok2 = true;
                                ready2 = true;
                                wr2 = false;
                                string Addr = "M27.1";
                                PLCS7_1200.Write(Addr, true);
                                d_check = true;
                                MethodInvoker inv1 = delegate
                                {
                                    D_timer.Enabled = true;
                                    D_timer.Start();
                                }; this.Invoke(inv1);
                            }
                            catch (Exception)
                            {
                                begin_ok2 = false;
                                ready2 = false;
                                wr2 = true;
                                //tranfer2 = 0;
                            }

                        }

                    }

                }
                if (OK_NG[0] == "NG2" && allow_check && !run_out2)
                {
                    if (on2 != 1)
                    {
                        if (En_chek2.Checked)
                        {
                            try
                            {

                                begin_ng2 = true;
                                ready2 = true;
                                loi_tam2 = OK_NG[1];
                                vitri_Erpic(OK_NG[2]);
                                err_pic2 = (OK_NG[2]);
                                wr2 = false;
                                string Addr = "M27.1";
                                PLCS7_1200.Write(Addr, true);
                                d_check = true;
                                MethodInvoker inv1 = delegate
                                {
                                    D_timer.Enabled = true;
                                    D_timer.Start();
                                }; this.Invoke(inv1);
                            }
                            catch (Exception)
                            {
                                begin_ng2 = false;
                                ready2 = false;
                                wr2 = true;
                            }

                        }

                    }

                    if (on2 == 1 && !wr2)
                    {


                        if (En_chek2.Checked)
                        {
                            try
                            {

                                begin_ng2 = true;
                                ready2 = true;
                                loi_tam2 = OK_NG[1];
                                vitri_Erpic(OK_NG[2]);
                                err_pic2 = (OK_NG[2]);
                                wr2 = false;
                                string Addr = "M27.1";
                                PLCS7_1200.Write(Addr, true);
                                d_check = true;
                                MethodInvoker inv1 = delegate
                                {
                                    D_timer.Enabled = true;
                                    D_timer.Start();
                                }; this.Invoke(inv1);
                            }
                            catch (Exception)
                            {
                                begin_ng2 = false;
                                ready2 = false;
                                wr2 = true;
                            }

                        }

                    }
                }
            }
            if (readc1.Contains('#') && readc1.Length == 13)
            {
                string[] scale = new string[7];
                scale = readc1.Split('#');
                if (scale[0] == "TT2")
                {
                    zoom2(1, true, 1);
                }
                if (scale[0] == "LL2")
                {
                    zoom2(1, true, 2);
                }
                if (scale[0] == "Z21" && allow_check)
                {
                    zoom2(1, false, 0);
                }
                if (scale[0] == "Z22" && allow_check)
                {
                    zoom2(2, false, 0);
                }
                if (scale[0] == "Z23" && allow_check)
                {
                    zoom2(3, false, 0);
                }
                if (scale[0] == "Z24" && allow_check)
                {
                    zoom2(4, false, 0);
                }
                if (scale[0] == "Z25" && allow_check)
                {
                    zoom2(5, false, 0);
                }
                if (scale[0] == "Z26" && allow_check)
                {
                    zoom2(6, false, 0);
                }
            }
        }
        private void ReadD(string readd1)
        {
            if (readd1.Contains(';') && readd1.Length == 13)
            {
                string[] OK_NG = new string[7];
                OK_NG = readd1.Split(';');
                if (OK_NG[0] == "D" && ready2 && begin_ok2)
                {

                    PLCS7_1200.Write("M79.3", true);
                    if (!wr2)
                    {
                        if (on2 == 1)
                        {
                            zoom2(1, false, 0);
                        }

                        OK2_check();
                        MethodInvoker inv = delegate
                        {

                            waiting2.Text = "CHỜ MỘT CHÚT.....";
                            waiting1.Text = "ĐẾN LƯỢT BẠN";
                            button2.Enabled = true;
                            button3.Enabled = false;

                        }; this.Invoke(inv);
                        wr2 = false;
                        begin_ok2 = false;
                        ready2 = false;
                        d_check = false;
                        D_timer.Stop();
                        D_timer.Enabled = false;
                        coun_d = 0;
                    }
                }
                if (OK_NG[0] == "D" && ready2 && begin_ng2)
                {

                    PLCS7_1200.Write("M79.3", true);
                    if (!wr2)
                    {

                        if (on2 == 1)
                        {
                            zoom2(1, false, 0);
                        }

                        NG2_check();
                        MethodInvoker inv = delegate
                        {

                            waiting2.Text = "CHỜ MỘT CHÚT.....";
                            waiting1.Text = "ĐẾN LƯỢT BẠN";
                            button2.Enabled = true;
                            button3.Enabled = false;
                        }; this.Invoke(inv);
                        wr2 = false;
                        begin_ng2 = false;
                        ready2 = false;
                        d_check = false;
                        D_timer.Stop();
                        D_timer.Enabled = false;
                        coun_d = 0;
                    }
                }
                if (OK_NG[0] == "D" && ready1 && begin_ok1)
                {
                    //if ( ready1 && begin_ok1)
                    //{
                    PLCS7_1200.Write("M79.3", true);
                    if (!wr1)
                    {
                        if (on1 == 1)
                        {
                            zoom1(1, false, 0);
                        }

                        OK1_check();
                        MethodInvoker inv = delegate
                        {

                            waiting1.Text = "CHỜ MỘT CHÚT.....";
                            waiting2.Text = "ĐẾN LƯỢT BẠN";
                            button3.Enabled = true;
                            button2.Enabled = false;
                        }; this.Invoke(inv);
                        wr1 = false;
                        begin_ok1 = false;
                        ready1 = false;
                        d_check = false;
                        D_timer.Stop();
                        D_timer.Enabled = false;
                        coun_d = 0;
                    }
                }
                if (OK_NG[0] == "D" && ready1 && begin_ng1)
                {

                    PLCS7_1200.Write("M79.3", true);
                    if (!wr1)
                    {

                        if (on1 == 1)
                        {
                            zoom1(1, false, 0);
                        }

                        NG1_check();
                        MethodInvoker inv = delegate
                        {

                            waiting1.Text = "CHỜ MỘT CHÚT.....";
                            waiting2.Text = "ĐẾN LƯỢT BẠN";
                            button3.Enabled = true;
                            button2.Enabled = false;
                        }; this.Invoke(inv);
                        wr1 = false;
                        begin_ng1 = false;
                        ready1 = false;
                        d_check = false;
                        D_timer.Stop();
                        D_timer.Enabled = false;
                         coun_d = 0;
                    }
                }
            }

        }
        string loi_tam1 = "";
        string loi_tam2 = "";
       
        string shot1 = "";
        string shot2 = "";
        string shot3 = "";
        string shot4 = "";
        string shot5 = "";
        string shot6 = "";
        string shot7 = "";
        bool ready1 = false;
        bool ready2 = false;
        bool begin_ok1 = false;
        bool begin_ok2 = false;
        bool begin_ng1 = false;
        bool begin_ng2 = false;
        int set_cam = 0;
        private void serialPort_communicate_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string cap_order = serialPort_communicate.ReadExisting();

        }

        bool wr1 = false;
        bool wr2 = false;
        private void Shot_pic_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //  if (!shot_pic.IsBusy) shot_pic.RunWorkerAsync();
        }

        private void Shot_pic_DoWork(object sender, DoWorkEventArgs e)
        {
            if (shot_pic.CancellationPending)
            {
                e.Cancel = true;
            }
            //Take_Photo();
        }

        double time_to_reset_cam = 0;

        private void call_back_cam_Tick(object sender, EventArgs e)
        {
            try
            {
                if (!started) return;
                time_to_reset_cam++;
                MethodInvoker inv1 = delegate
                {
                    label15.Text = set_cam.ToString();
                    label17.Text = time_to_reset_cam.ToString();

                }; this.Invoke(inv1);
                if (time_to_reset_cam >= 2)
                {
                    if (order_1)
                    {

                        if (Cam1VIDEO_Device != null || Cam1VIDEO_Device.IsRunning)
                        {
                            Cam1VIDEO_Device.SignalToStop();
                            //Cam1VIDEO_Device.WaitForStop();

                        }
                     
                        if (Cam1VIDEO_Device == null || !Cam1VIDEO_Device.IsRunning)
                        {
                            //Cam1VIDEO_Device = new VideoCaptureDevice(Cam1_Device.MonikerString);
                            //Cam1VIDEO_Device.VideoResolution = Cam1VIDEO_Device.VideoCapabilities[system_config.pixel_cam1];
                            //Cam1VIDEO_Device.NewFrame += Cam1VIDEO_Device_NewFrame;
                            Cam1VIDEO_Device.Start();
                            set_cam++;
                        }
                       
                    }
                    if (order_2)
                    {
                        if (Cam2VIDEO_Device != null || Cam2VIDEO_Device.IsRunning)
                        {
                            Cam2VIDEO_Device.SignalToStop();
                            //Cam2VIDEO_Device.WaitForStop();
                        }
                       
                        if (Cam2VIDEO_Device == null || !Cam2VIDEO_Device.IsRunning)
                        {
                            //Cam2VIDEO_Device = new VideoCaptureDevice(Cam2_Device.MonikerString);
                            //Cam2VIDEO_Device.VideoResolution = Cam2VIDEO_Device.VideoCapabilities[system_config.pixel_cam2];
                            //Cam2VIDEO_Device.NewFrame += Cam2VIDEO_Device_NewFrame;
                            Cam2VIDEO_Device.Start();
                            set_cam++;
                        }
                        
                    }
                    if (order_3)
                    {

                        if (Cam3VIDEO_Device != null || Cam3VIDEO_Device.IsRunning)
                        {
                            Cam3VIDEO_Device.SignalToStop();
                            //Cam3VIDEO_Device.WaitForStop();
                        }
                      
                        if (Cam3VIDEO_Device == null || !Cam3VIDEO_Device.IsRunning)
                        {
                            //Cam3VIDEO_Device = new VideoCaptureDevice(Cam3_Device.MonikerString);
                            //Cam3VIDEO_Device.VideoResolution = Cam3VIDEO_Device.VideoCapabilities[system_config.pixel_cam3];
                            //Cam3VIDEO_Device.NewFrame += Cam3VIDEO_Device_NewFrame;
                            Cam3VIDEO_Device.Start();
                            set_cam++;
                        }
                       
                    }
                    if (order_4)
                    {

                        if (Cam4VIDEO_Device != null || Cam4VIDEO_Device.IsRunning)
                        {
                            Cam4VIDEO_Device.SignalToStop();
                            //Cam4VIDEO_Device.WaitForStop();
                        }
                      
                        if (Cam4VIDEO_Device == null || !Cam4VIDEO_Device.IsRunning)
                        {
                            //Cam4VIDEO_Device = new VideoCaptureDevice(Cam4_Device.MonikerString);
                            //Cam4VIDEO_Device.VideoResolution = Cam4VIDEO_Device.VideoCapabilities[system_config.pixel_cam4];
                            //Cam4VIDEO_Device.NewFrame += Cam4VIDEO_Device_NewFrame;
                            Cam4VIDEO_Device.Start();
                            set_cam++;
                        }
                       
                    }
                    if (order_5)
                    {

                        if (Cam5VIDEO_Device != null || Cam5VIDEO_Device.IsRunning)
                        {
                            Cam5VIDEO_Device.SignalToStop();
                            //Cam5VIDEO_Device.WaitForStop();

                        }
                       
                        if (Cam5VIDEO_Device == null || !Cam5VIDEO_Device.IsRunning)
                        {
                            //Cam5VIDEO_Device = new VideoCaptureDevice(Cam5_Device.MonikerString);
                            //Cam5VIDEO_Device.VideoResolution = Cam5VIDEO_Device.VideoCapabilities[system_config.pixel_cam5];
                            //Cam5VIDEO_Device.NewFrame += Cam5VIDEO_Device_NewFrame;
                            Cam5VIDEO_Device.Start();
                            set_cam++;
                        }
                       
                    }
                    if (order_6)
                    {

                        if (Cam6VIDEO_Device != null || Cam6VIDEO_Device.IsRunning)
                        {
                            Cam6VIDEO_Device.SignalToStop();
                            //Cam6VIDEO_Device.WaitForStop();

                        }
                      
                        if (Cam6VIDEO_Device == null || !Cam6VIDEO_Device.IsRunning)
                        {
                            //Cam6VIDEO_Device = new VideoCaptureDevice(Cam6_Device.MonikerString);
                            //Cam6VIDEO_Device.VideoResolution = Cam6VIDEO_Device.VideoCapabilities[system_config.pixel_cam6];
                            //Cam6VIDEO_Device.NewFrame += Cam6VIDEO_Device_NewFrame;
                            Cam6VIDEO_Device.Start();
                            set_cam++;
                        }
                        
                    }

                }
                
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message +" CALLBACK cam");
            }
        }
        private void Take_Photo()
        {
            try
            {
                MethodInvoker inv = delegate
                {
                    if (order_1)
                    {
                        Cam1VIDEO_Device.Start();

                    }
                    if (order_2)
                    {
                        Cam2VIDEO_Device.Start();

                    }
                    if (order_3)
                    {
                        Cam3VIDEO_Device.Start();

                    }
                    if (order_4)
                    {
                        Cam4VIDEO_Device.Start();

                    }
                    if (order_5)
                    {
                        Cam5VIDEO_Device.Start();

                    }
                    if (order_6)
                    {
                        Cam6VIDEO_Device.Start();

                    }                

                };
                this.Invoke(inv);
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message + "START CAM");
            }
        }
        int dem = 2;
        int w = 0;
        private void status(string text)
        {
            MethodInvoker inv = delegate
            {

                string[] txt = new string[dem];
                textBox_stt.AppendText("[" + DateTime.Now.ToString() + "]" + text + Environment.NewLine);


                txt = textBox_stt.Text.Split(new char[] { '\n', '\r' });
                using (StreamWriter sw = File.AppendText("Console.txt"))
                {
                    sw.WriteLine("" + txt[w] + "");
                }
                dem++;
                w = w + 2;
            };
            this.Invoke(inv);
        }
        private void inf_process()
        {
            MethodInvoker inv = delegate
            {
                TB_LTdate.Text = system_config.inf_process.ToString();
                TB_testpart.Text = (folderIndex - 1).ToString();
            };
            this.Invoke(inv);
        }
        #endregion
        #region SETTING
        private void cameraSettingToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please stop program first!");
                return;
            }
            Form cameraform = new Camera_setting_Form();
            cameraform.FormClosed += (object sender2, FormClosedEventArgs e2) =>
            {
                this.Show();
            };
            this.Hide();
            cameraform.Show();
        }

        private void pathFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please stop program first!");
                return;
            }
            Form Path_form = new Path_File_Component();
            Path_form.FormClosed += (object sender3, FormClosedEventArgs e3) =>
            {
                this.Show();
            };
            this.Hide();
            Path_form.Show();
        }
        OpenFileDialog openFileDialog;

        private void cOMConnectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please stop program first!");
                return;
            }
            Process[] Pname = Process.GetProcessesByName("comport");
            if (Pname.Length==0)
            {
                Process.Start(@"C:\Users\Admin\source\repos\comport\comport\bin\Debug\comport.exe");
                Gen_check_Com.Checked = true;
            }
            else 
            {

                Gen_check_Com.Checked = true;
                MessageBox.Show("Com Port connected");
            }
            //Process.Start(@"C:\Users\Admin\source\repos\comport\comport\bin\Debug\comport.exe");

            //Gen_check_Com.Checked = true;
        }


        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string mesage = "Do you want to exit the program";
            string cap = "Close the program";
            var result = MessageBox.Show(mesage, cap, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                update_system();
                RESET();
                Environment.Exit(0);
            }

        }
        private void RESET()
        {
            if (backgroundWorker_1.IsBusy) backgroundWorker_1.CancelAsync();
            if (backgroundWorker_2.IsBusy) backgroundWorker_2.CancelAsync();
            if (backgroundWorker_3.IsBusy) backgroundWorker_3.CancelAsync();
            if (backgroundWorker_4.IsBusy) backgroundWorker_4.CancelAsync();
            if (backgroundWorker_5.IsBusy) backgroundWorker_5.CancelAsync();
            if (backgroundWorker_6.IsBusy) backgroundWorker_6.CancelAsync();
            if (backgroundWorker_7.IsBusy) backgroundWorker_7.CancelAsync();
            if (shot_pic.IsBusy) shot_pic.CancelAsync();
            //if (serialPort_communicate.IsOpen) serialPort_communicate.Close();
            PB_active1.Hide();
            if (Cam1VIDEO_Device != null && Cam1VIDEO_Device.IsRunning)
            {
                Cam1VIDEO_Device.Stop();
 
            }
            if (Cam2VIDEO_Device != null && Cam2VIDEO_Device.IsRunning)
            {
                Cam2VIDEO_Device.Stop();

            }
            PB_active2.Hide();
            if (Cam3VIDEO_Device != null && Cam3VIDEO_Device.IsRunning)
            {
                Cam3VIDEO_Device.Stop();

            }
            if (Cam4VIDEO_Device != null && Cam4VIDEO_Device.IsRunning)
            {
                Cam4VIDEO_Device.Stop();
           
            }
            PB_active4.Hide();
            PB_active3.Hide();
            if (Cam5VIDEO_Device != null && Cam5VIDEO_Device.IsRunning)
            {
                Cam5VIDEO_Device.Stop();
  

            }
            PB_active5.Hide();
            if (Cam6VIDEO_Device != null && Cam6VIDEO_Device.IsRunning)
            {
                Cam6VIDEO_Device.Stop();
  

            }
            PB_active6.Hide();
            if (system_config.add_cam == "true")
            {
                if (Cam7VIDEO_Device != null && Cam7VIDEO_Device.IsRunning) Cam7VIDEO_Device.Stop();
            }
            update_system();
        }

        private void update_system()
        {
            Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", load1.ToString());
            Program_Configuration.UpdateSystem_Config("Folder_load_check", load2.ToString());
            Program_Configuration.UpdateSystem_Config("same_folder_1", folderIndex.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam1_folder", count_1.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam2_folder", count_2.ToString());


            Program_Configuration.UpdateSystem_Config("Location_cam3_folder", count_3.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam4_folder", count_4.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam5_folder", count_5.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam6_folder", count_6.ToString());
            Program_Configuration.UpdateSystem_Config("Location_cam7_folder", count_7.ToString());
            using (StreamWriter sw = new StreamWriter(@"C:\Users\Admin\source\repos\Visual\Camera_Check_Component\bin\Debug\Output.txt"))
            {
                sw.Write("");
                sw.WriteLine("" + _OKnum.ToString() + "");
                sw.WriteLine("" + _NGnum.ToString() + "");

            }

        }
        private void Camera_Check_component_FormClosing(object sender, FormClosingEventArgs e)
        {
            update_system();
            RESET();
            timer.Stop();


            if (PLC_con)
            {
                string Addr = "M170.0";
                PLCS7_1200.Write(Addr, int.Parse("1"));
                string Addr1 = "M170.2";
                PLCS7_1200.Write(Addr1, int.Parse("1"));
                loadform = false;
                PLC_con = false;
                PLCS7_1200.Close();
            }
            else
            {
                PLCS7_1200 = new Plc(CpuType.S71200, "192.168.0.7", 0, 0);
                if (PLCS7_1200.IsAvailable)
                {
                    PLCS7_1200.Open();
                    if (PLCS7_1200.IsConnected == true)
                    {
                        string Addr = "M170.0";
                        PLCS7_1200.Write(Addr, int.Parse("1"));
                        string Addr1 = "M170.2";
                        PLCS7_1200.Write(Addr1, int.Parse("1"));
                        loadform = false;
                        PLC_con = false;
                    }
                    PLCS7_1200.Close();
                }
            }
            if (ledinf.IsBusy) ledinf.CancelAsync();
        }

        private void Stop_btn_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("Do you want to Stop the Program", "Confirm", MessageBoxButtons.OKCancel);
            if (timer.Enabled && result == DialogResult.OK)
            {
                update_system();
                LB_TIMER.Text = "00:00:00";
                start_check = false;
                started = false;

                tb_PN.Enabled = true;
                TB_idworker.Enabled = true;
                TB_wker2.Enabled = true;
                //TB_idworker.Text = "";
                //TB_wker2.Text = "";
                _OKnum = 0;
                _NGnum = 0;
                _sum = 0;
                RESET();

                Start_btn.Enabled = true;
                Stop_btn.Enabled = false;

                Pic_Cam1.Image = null;
                Pic_Cam2.Image = null;
                Pic_Cam3.Image = null;
                Pic_Cam4.Image = null;
                Pic_Cam5.Image = null;
                Pic_Cam6.Image = null;
                if (watcher.EnableRaisingEvents) watcher.EnableRaisingEvents = false;
                status("[SYSTEM] Program STOPPED");
                startPR_Count = 0;
            }

        }
        #endregion
        #region///////////////////////////////////////////////////////////////////////////////////////////////////////////////////// XỬ LÝ ẢNH
        private void delete_image()
        {
            DirectoryInfo d = new DirectoryInfo(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH);
            if (!d.Exists)
            {
                MessageBox.Show("Folder do not exists ");
                return;
            }
            MethodInvoker inv = delegate
            {
                if (pictureBox1.Image != null && pictureBox2.Image != null && pictureBox3.Image != null && pictureBox4.Image != null && pictureBox5.Image != null && pictureBox6.Image != null && !run_out1)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox2.Image.Dispose();
                    pictureBox3.Image.Dispose();
                    pictureBox4.Image.Dispose();
                    pictureBox5.Image.Dispose();
                    pictureBox6.Image.Dispose();

                    pictureBox1.Image = null;
                    pictureBox2.Image = null;
                    pictureBox3.Image = null;
                    pictureBox4.Image = null;
                    pictureBox5.Image = null;
                    pictureBox6.Image = null;
                }
                if (pictureBox15.Image != null && pictureBox16.Image != null && pictureBox17.Image != null && pictureBox18.Image != null && pictureBox19.Image != null && pictureBox20.Image != null && !run_out2)
                {
                    pictureBox15.Image.Dispose();
                    pictureBox16.Image.Dispose();
                    pictureBox17.Image.Dispose();
                    pictureBox18.Image.Dispose();
                    pictureBox19.Image.Dispose();
                    pictureBox20.Image.Dispose();

                    pictureBox15.Image = null;
                    pictureBox16.Image = null;
                    pictureBox17.Image = null;
                    pictureBox18.Image = null;
                    pictureBox19.Image = null;
                    pictureBox20.Image = null;
                }
            }; this.Invoke(inv);

            FileInfo[] fileInfor = d.GetFiles("*.jpeg");
            foreach (FileInfo file in fileInfor)
            {
                if (file == null) break;
                file.Delete();
            }
            up_hinh = false;
        }
        private static Image RotateImage(Bitmap img, float rotationAngle)
        {
            //create an empty Bitmap image
            Bitmap bmp = new Bitmap(img.Width, img.Height);

            //turn the Bitmap into a Graphics object
            Graphics gfx = Graphics.FromImage(bmp);

            //now we set the rotation point to the center of our image
            gfx.TranslateTransform((float)bmp.Width / 2, (float)bmp.Height / 2);

            //now rotate the image

            gfx.RotateTransform(rotationAngle);

            gfx.TranslateTransform(-(float)bmp.Width / 2, -(float)bmp.Height / 2);

            //set the InterpolationMode to HighQualityBicubic so to ensure a high
            //quality image once it is transformed to the specified size
            gfx.InterpolationMode = InterpolationMode.HighQualityBicubic;

            //now draw our new image onto the graphics object
            gfx.DrawImage(img, new System.Drawing.Point(0, 0));

            //dispose of our Graphics object
            gfx.Dispose();

            //return the image
            return bmp;
        }

        bool run_out1 = false;
        bool run_out2 = false;
        private void upload_image()
        {
            DirectoryInfo d = new DirectoryInfo(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH);
            system_config = Program_Configuration.GetSystem_Config();
            if (!d.Exists)
            {
                MessageBox.Show("Folder do not exists ");
                return;
            }
            FileInfo[] fileInfor = d.GetFiles("*.jpeg");
            string[] into_pic = new string[7];

            string[] getpath = new string[14];
            int i = 0;

            if (fileInfor == null)
            {
                return;
            }
            foreach (FileInfo file in fileInfor)
            {
                into_pic = file.Name.Split('-');
                if (into_pic[6] == "" + load1.ToString() + ".jpeg")
                {
                    getpath[i] = file.Name;
                    i++;
                }
            }

            bool a1 = false; bool a2 = false; bool a3 = false; bool a4 = false; bool a5 = false; bool a6 = false; bool a7 = false;
            foreach (string file in getpath)
            {

                if (file == null)
                {
                    if (!a1) path_1_1 = "";
                    if (!a2) path_1_2 = "";
                    if (!a3) path_1_3 = "";
                    if (!a4) path_1_4 = "";
                    if (!a5) path_1_5 = "";
                    if (!a6) path_1_6 = "";

                    break;
                }

                into_pic = file.Split('-');

                if (into_pic[5] == "1")
                {
                    a1 = true;
                    path_1_1 = file;
                }
                else if (!a1 && into_pic[5] != "1")
                {
                    path_1_1 = "";
                }
                if (into_pic[5] == "2")
                {
                    a2 = true;
                    path_1_2 = file;
                }
                else if (!a2 && into_pic[5] != "2")
                {
                    path_1_2 = "";
                }
                if (into_pic[5] == "3")
                {
                    a3 = true;
                    path_1_3 = file;
                }
                else if (!a3 && into_pic[5] != "3")
                {
                    path_1_3 = "";
                }
                if (into_pic[5] == "4")
                {
                    a4 = true;
                    path_1_4 = file;
                }
                else if (!a4 && into_pic[5] != "4")
                {
                    path_1_4 = "";
                }
                if (into_pic[5] == "5")
                {
                    a5 = true;
                    path_1_5 = file;
                }
                else if (!a5 && into_pic[5] != "5")
                {
                    path_1_5 = "";
                }
                if (into_pic[5] == "6")
                {
                    a6 = true;
                    path_1_6 = file;
                }
                else if (!a6 && into_pic[5] != "6")
                {
                    path_1_6 = "";
                }
                if (into_pic[5] == "7")
                {
                    a7 = true;
                    path_1_7 = file;
                }
                else if (!a7 && into_pic[5] != "7")
                {
                    path_1_7 = "";
                }

            }
            if (path_1_1 == "" || path_1_2 == "" || path_1_3 == "" || path_1_4 == "" || path_1_5 == "" || path_1_6 == "")
            {
                pictureBox1.Image = null;
                pictureBox2.Image = null;
                pictureBox3.Image = null;
                pictureBox4.Image = null;
                pictureBox5.Image = null;
                pictureBox6.Image = null;
                Hname1.Text = "";
                Hname2.Text = "";
                Hname3.Text = "";
                Hname4.Text = "";
                Hname5.Text = "";
                Hname6.Text = "";

                run_out1 = true;
                return;
            }
            if (path_1_1 != "")
            {
                pictureBox1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_1 + "");
                Hname1.Text = path_1_1;

            }
            else
            {
                pictureBox1.Image = null;
                Hname1.Text = "";

            }
            if (path_1_2 != "")
            {
                pictureBox2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_2 + "");
                Hname2.Text = path_1_2;

                c1_12.Visible = true; c4_12.Visible = true;
            }
            else
            {
                pictureBox2.Image = null;
                Hname2.Text = "";
                c1_12.Visible = false; c4_12.Visible = false;
            }
            if (path_1_3 != "")
            {
                pictureBox3.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_3 + "");
                Hname3.Text = path_1_3;

            }
            else
            {
                pictureBox3.Image = null;
                Hname3.Text = "";

            }
            if (path_1_4 != "")
            {
                pictureBox4.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_4 + "");
                Hname4.Text = path_1_4;

            }
            else
            {
                pictureBox4.Image = null;
                Hname4.Text = "";

            }
            if (path_1_5 != "")
            {
                pictureBox5.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_5 + "");
                Hname5.Text = path_1_5;

            }
            else
            {
                pictureBox5.Image = null;
                Hname5.Text = "";

            }
            if (path_1_6 != "")
            {
                pictureBox6.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_6 + "");
                Hname6.Text = path_1_6;

            }
            else
            {
                pictureBox6.Image = null;
                Hname6.Text = "";

            }

        }

        string path_1_1 = "";
        string path_1_2 = "";
        string path_1_3 = "";
        string path_1_4 = "";
        string path_1_5 = "";
        string path_1_6 = "";
        string path_1_7 = "";

        private void update_image2()
        {
            DirectoryInfo d = new DirectoryInfo(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH);
            system_config = Program_Configuration.GetSystem_Config();
            if (!d.Exists)
            {
                MessageBox.Show("Folder do not exists ");
                return;
            }
            FileInfo[] fileInfor = d.GetFiles("*.jpeg");
            if (fileInfor == null)
            {
                return;
            }
            string[] into_pic = new string[7];

            string[] getpath = new string[14];
            int i = 0;

            bool a1 = false; bool a2 = false; bool a3 = false; bool a4 = false; bool a5 = false; bool a6 = false; bool a7 = false;
            foreach (FileInfo file in fileInfor)
            {
                into_pic = file.Name.Split('-');
                if (into_pic[6] == "" + load2.ToString() + ".jpeg")
                {
                    getpath[i] = file.Name;
                    i++;
                }
            }
            foreach (string file in getpath)
            {

                if (file == null)
                {
                    if (!a1) path_2_1 = "";
                    if (!a2) path_2_2 = "";
                    if (!a3) path_2_3 = "";
                    if (!a4) path_2_4 = "";
                    if (!a5) path_2_5 = "";
                    if (!a6) path_2_6 = "";

                    break;
                }
                into_pic = file.Split('-');

                if (into_pic[5] == "1")
                {
                    a1 = true;
                    path_2_1 = file;
                }
                else if (!a1 && into_pic[5] != "1")
                {
                    path_2_1 = "";
                }
                if (into_pic[5] == "2")
                {
                    a2 = true;
                    path_2_2 = file;
                }
                else if (!a2 && into_pic[5] != "2")
                {
                    path_2_2 = "";
                }
                if (into_pic[5] == "3")
                {
                    a3 = true;
                    path_2_3 = file;
                }
                else if (!a3 && into_pic[5] != "3")
                {
                    path_2_3 = "";
                }
                if (into_pic[5] == "4")
                {
                    a4 = true;
                    path_2_4 = file;
                }
                else if (!a4 && into_pic[5] != "4")
                {
                    path_2_4 = "";
                }
                if (into_pic[5] == "5")
                {
                    a5 = true;
                    path_2_5 = file;
                }
                else if (!a5 && into_pic[5] != "5")
                {
                    path_2_5 = "";
                }
                if (into_pic[5] == "6")
                {
                    a6 = true;
                    path_2_6 = file;
                }
                else if (!a6 && into_pic[5] != "6")
                {
                    path_2_6 = "";
                }
                if (into_pic[5] == "7")
                {
                    a7 = true;
                    path_2_7 = file;
                }
                else if (!a7 && into_pic[5] != "7")
                {
                    path_2_7 = "";
                }

            }
            if (path_2_1 == "" || path_2_2 == "" || path_2_3 == "" || path_2_4 == "" || path_2_5 == "" || path_2_6 == "")
            {
                pictureBox15.Image = null;
                pictureBox16.Image = null;
                pictureBox17.Image = null;
                pictureBox18.Image = null;
                pictureBox19.Image = null;
                pictureBox20.Image = null;
                Hname_7.Text = "";
                Hname_8.Text = "";
                Hname_9.Text = "";
                Hname_10.Text = "";
                Hname_11.Text = "";
                Hname_12.Text = "";

                run_out2 = true;
                return;
            }
            if (path_2_1 != "")
            {
                pictureBox15.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_1 + "");
                Hname_7.Text = path_2_1;

            }
            else
            {
                pictureBox15.Image = null;
                Hname_7.Text = "";

            }
            if (path_2_2 != "")
            {
                pictureBox16.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_2 + "");
                Hname_8.Text = path_2_2;

            }
            else
            {
                pictureBox16.Image = null;
                Hname_8.Text = "";

            }
            if (path_2_3 != "")
            {
                pictureBox17.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_3 + "");
                Hname_9.Text = path_2_3;

            }
            else
            {
                pictureBox17.Image = null;
                Hname_9.Text = "";

            }
            if (path_2_4 != "")
            {
                pictureBox18.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_4 + "");
                Hname_10.Text = path_2_4;

            }
            else
            {
                pictureBox18.Image = null;
                Hname_10.Text = "";

            }
            if (path_2_5 != "")
            {
                pictureBox19.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_5 + "");
                Hname_11.Text = path_2_5;

            }
            else
            {
                pictureBox19.Image = null;
                Hname_11.Text = "";

            }
            if (path_2_6 != "")
            {
                pictureBox20.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_6 + "");
                Hname_12.Text = path_2_6;

            }
            else
            {
                pictureBox20.Image = null;
                Hname_12.Text = "";

            }

        }

        string path_2_1 = "";
        string path_2_2 = "";
        string path_2_3 = "";
        string path_2_4 = "";
        string path_2_5 = "";
        string path_2_6 = "";
        string path_2_7 = "";
        private void En_chek1_CheckedChanged(object sender, EventArgs e)
        {
            if (En_chek1.Checked)
            {
                OK1_check_pause();
                //last1 = false;
            }
        }

        private void En_chek2_CheckedChanged(object sender, EventArgs e)
        {
            MethodInvoker inv = delegate
            {
                if (En_chek2.Checked)
                {
                    OK2_check_pause();
                    //last2 = false;
                }
            }; this.Invoke(inv);
        }
        private void OK1_check_pause()
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null && pictureBox4.Image == null && pictureBox5.Image == null && pictureBox6.Image == null && !run_out1)
            {
                return;
            }
            MethodInvoker inv = delegate
            {
                load1 = folderIndex;
                update_image2();
            }; this.Invoke(inv);

        }
        private void OK2_check_pause()
        {
            if (pictureBox15.Image == null && pictureBox16.Image == null && pictureBox17.Image == null && pictureBox18.Image == null && pictureBox19.Image == null && pictureBox20.Image == null && !run_out2)
            {
                return;
            }
            MethodInvoker inv = delegate
            {
                load2 = folderIndex;
                upload_image();
            };
            this.Invoke(inv);

        }
        private void null_pic1()
        {
            if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null && pictureBox4.Image == null && pictureBox5.Image == null && pictureBox6.Image == null && !run_out1)
            {
                return;
            }
            MethodInvoker inv = delegate
            {
                if (pictureBox1.Image != null && pictureBox2.Image != null && pictureBox3.Image != null && pictureBox4.Image != null && pictureBox5.Image != null && pictureBox6.Image != null && !run_out1)
                {
                    pictureBox1.Image.Dispose();
                    pictureBox2.Image.Dispose();
                    pictureBox3.Image.Dispose();
                    pictureBox4.Image.Dispose();
                    pictureBox5.Image.Dispose();
                    pictureBox6.Image.Dispose();
                    if (pic_full1.Image != null) { pic_full1.Image.Dispose(); pic_full1.Image = null; }
                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_1);
                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_2);
                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_3);
                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_4);
                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_5);
                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_6);

                    folderIndex++;

                    for (int i = 1; i <= fall_number.Length - 1; i++)
                    {
                        if (folderIndex == fall_number[i])
                        {
                            folderIndex++;

                            load1 = folderIndex;
                            if (En_chek1.Checked)
                            {
                                load1 = folderIndex;
                            }
                        }
                        if (fall_pn > 0)
                        {
                            fall_pn--;
                        }
                    }


                    load1 = folderIndex;
                    if (load1 != folderIndex) load1 = folderIndex;
                    if (En_chek1.Checked)
                    {
                        load1 = folderIndex;
                    }
                    upload_image();
                }
            }; this.Invoke(inv);
        }
        private void null_pic2()
        {
            if (pictureBox15.Image == null && pictureBox16.Image == null && pictureBox17.Image == null && pictureBox18.Image == null && pictureBox19.Image == null && pictureBox20.Image == null && !run_out2)
            {
                return;
            }
            MethodInvoker inv = delegate
            {
                if (pictureBox15.Image != null && pictureBox16.Image != null && pictureBox17.Image != null && pictureBox18.Image != null && pictureBox19.Image != null && pictureBox20.Image != null && !run_out2)
                {
                    pictureBox15.Image.Dispose();
                    pictureBox16.Image.Dispose();
                    pictureBox17.Image.Dispose();
                    pictureBox18.Image.Dispose();
                    pictureBox19.Image.Dispose();
                    pictureBox20.Image.Dispose();
                    if (picfull_2.Image != null) { picfull_2.Image.Dispose(); picfull_2.Image = null; }

                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_1);
                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_2);
                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_3);
                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_4);
                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_5);
                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_6);
                    folderIndex++;
                    for (int i = 1; i <= fall_number.Length-1; i++)
                    {
                        if (folderIndex == fall_number[i] )
                        {
                            folderIndex++;

                            load2 = folderIndex;
                            if (En_chek2.Checked)
                            {
                                load2 = folderIndex;
                            }
                        }
                        if (fall_pn>0)
                        {
                            fall_pn--;
                        }
                    }


                    load2 = folderIndex;
                    if (load2 != folderIndex) load2 = folderIndex;
                    if (En_chek2.Checked)
                    {
                        load2 = folderIndex;
                    }
                    update_image2();
                }
            }; this.Invoke(inv);

        }
        string fall_1 = string.Empty;
        string fall_2 = string.Empty;
        string fall_3 = string.Empty;
        string fall_4 = string.Empty;
        string fall_5 = string.Empty;
        string fall_6 = string.Empty;
        string fall_7 = string.Empty;
        private void fall_down(string number) 
        {
            try
            {
                MethodInvoker inv = delegate 
                {
                    
                        bool have = true;
                        DirectoryInfo d = new DirectoryInfo(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH);
                        system_config = Program_Configuration.GetSystem_Config();
                        if (!d.Exists)
                        {
                            MessageBox.Show("Folder do not exists ");
                            return;
                        }
                        FileInfo[] fileInfor = d.GetFiles(number+".jpeg");
                        if (fileInfor == null)
                        {
                            return;
                        }
                        for (int b = 1; b <= fall_number.Length-1; b++)
                        {
                            string[] into_pic = new string[7];
                            string[] getpath = new string[14];
                            int i = 0;
                            int j = 0;
                            foreach (FileInfo file in fileInfor)
                            {
                                into_pic = file.Name.Split('-');
                                if (into_pic[6] == "" + fall_number[b].ToString() + ".jpeg")
                                {
                                    getpath[i] = file.Name;
                                    i++;
                                }
                            }
                            foreach (string file in getpath)
                            {
                                
                                if (file == null)
                                {
                                if (j == 6)
                                {
                                    have = true;
                                }
                                else have = false;
                                break;
                                    
                                }
                                
                                    
                               
                                j++;
                        }
                            if (have)
                            {
                                for (int a = 0; a < getpath.Length; a++)
                                {
                                    if (getpath[a] == null) break;
                                    File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + getpath[a]);
                                }


                            }
                        }
                       

                    
                };this.Invoke(inv);
              
            }
            catch (Exception ex)
            {

                status("[SYSTEM BUG] " + ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        private void Tranfer(string OPTION)
        {
            try
            {
                if (OPTION == "OK" && allow_check)
                {
                    MethodInvoker inv = delegate
                    {
                        DateTime dt = DateTime.Now;
                        string daytime = dt.Day.ToString() + "-" + dt.Month.ToString() + "-" + dt.Year.ToString();
                        DMY = daytime;
                        Parameter_app.OK_TEMP(DMY, load1.ToString());
                        string[] getpath = new string[7];

                        getpath = path_1_1.Split('-');

                        if (!Directory.Exists(system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH))
                        {
                            Directory.CreateDirectory(system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH);
                        }


                        pictureBox1.Image.Dispose();
                        pictureBox2.Image.Dispose();
                        pictureBox3.Image.Dispose();
                        pictureBox4.Image.Dispose();
                        pictureBox5.Image.Dispose();
                        pictureBox6.Image.Dispose();
                        if (pic_full1.Image != null) { pic_full1.Image.Dispose(); pic_full1.Image = null; }


                        if (auto_man)
                        {
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_1, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator1 + "-" + path_1_1);
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_2, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator1 + "-" + path_1_2);
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_3, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator1 + "-" + path_1_3);
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_4, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator1 + "-" + path_1_4);
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_5, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator1 + "-" + path_1_5);
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_6, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator1 + "-" + path_1_6);
                            if (system_config.add_cam == "true")
                            {
                                File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_7, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator1 + "-" + path_1_7);
                            }


                            string[] tach = new string[2];
                            tach = getpath[6].Split('.');
                            Boolean check = sql_action.excute_data("INSERT INTO component_status (PN_Selector,Date,Time,Trace,ID,Status,Picture1,Picture2,Picture3,Picture4,Picture5,Picture6) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "','" + ID_Operator1 + "','OK','OK','OK','OK','OK','OK','OK')");
                            Boolean orderby = sql_action.excute_data("SELECT Time FROM component_status ORDER BY Time DESC");
                            var item = new ListViewItem(new[] { "" + PN_Selector + "-" + folderIndex.ToString() + "", "OK", "" });
                            listView1.Items.Add(item);
                        }

                        status(" [SYSTEM] " + " [OK]" + " SAVED IMAGE[" + load1.ToString() + "]");
                        folderIndex++;
                        
                        for (int i = 1; i <= fall_number.Length-1; i++)
                        {
                            if (folderIndex == fall_number[i]) 
                            {
                                folderIndex++;
                                   
                                load1 = folderIndex;
                                if (En_chek1.Checked)
                                {
                                    load1 = folderIndex;
                                }
                            }
                            if (fall_pn > 0)
                            {
                                fall_pn--;
                            }
                        }
                       
                       
                            load1 = folderIndex;
                            if (load1 != folderIndex) load1 = folderIndex;
                            if (En_chek1.Checked)
                            {
                                load1 = folderIndex;
                            }
                       
                        update_system();
                    }; this.Invoke(inv);
                }

                if (OPTION == "ERROR" && allow_check)
                {
                    MethodInvoker inv = delegate
                    {
                        DateTime dt = DateTime.Now;
                        string daytime = dt.Day.ToString() + "-" + dt.Month.ToString() + "-" + dt.Year.ToString();
                        DMY = daytime;
                        string[] save_path = new string[] { path_1_1, path_1_2, path_1_3, path_1_4, path_1_5, path_1_6, path_1_7 };
                        string[] getpath = new string[7];

                        getpath = path_1_1.Split('-');
                        Parameter_app.ERROR_TEMP(DMY);
                        if (!Directory.Exists(system_config.Map_Path_File_2 + @"/" + Parameter_app.ERROR_IMAGE_FOLDER_PATH))
                        {

                            Directory.CreateDirectory(system_config.Map_Path_File_2 + @"/" + Parameter_app.ERROR_IMAGE_FOLDER_PATH);
                        }

                        pictureBox1.Image.Dispose();
                        pictureBox2.Image.Dispose();
                        pictureBox3.Image.Dispose();
                        pictureBox4.Image.Dispose();
                        pictureBox5.Image.Dispose();
                        pictureBox6.Image.Dispose();
                        if (pic_full1.Image != null) { pic_full1.Image.Dispose(); pic_full1.Image = null; }
                        if (auto_man)
                        {
                            int s = 0;
                            foreach (string luu in save_path)
                            {
                                if (save_path[s] == null) break;
                                if (s == save_pic)
                                {
                                    File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + save_path[s], system_config.Map_Path_File_2 + @"/" + Parameter_app.ERROR_IMAGE_FOLDER_PATH + "/" + error_Type(loi_tam1) + "-" + ID_Operator1 + "-" + save_path[s]);
                                }
                                else
                                {
                                    if (File.Exists(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + save_path[s]))
                                    {
                                        File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + save_path[s]);
                                    }
                                }
                                s++;
                            }

                            string[] tach = new string[2];
                            tach = getpath[6].Split('.');
                            Boolean check = sql_action.excute_data("INSERT INTO component_status (PN_Selector,Date,Time,Trace,ID,Status,Picture1,Picture2,Picture3,Picture4,Picture5,Picture6,NG_Type) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "','" + ID_Operator1 + "','NG','" + h1 + "','" + h2 + "','" + h3 + "','" + h4 + "','" + h5 + "','" + h6 + "','" + error_Type(loi_tam1) + "')");
                            Boolean insert = sql_action.excute_data("INSERT INTO NG_detail ([PN_Selector],[Date],[Time],[Trace]) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "_" + err_pic1 + "')");
                            Boolean orderby = sql_action.excute_data("SELECT Time FROM component_status ORDER BY (Time) DESC");
                            var item = new ListViewItem(new[] { "" + PN_Selector + "-" + folderIndex.ToString() + "", "NG", error_Type(loi_tam1) });
                            listView1.Items.Add(item);
                            err_pic1 = "";
                        }
                        status(" [SYSTEM]" + " [ERROR]" + " SAVED IMAGE[" + load1.ToString() + "]");
                        folderIndex++;
                        
                            for (int i = 1; i <= fall_number.Length-1; i++)
                            {
                                if (folderIndex == fall_number[i] )
                                {
                                    folderIndex++;
                                   
                                    load1 = folderIndex;
                                    if (En_chek1.Checked)
                                    {
                                        load1 = folderIndex;
                                    }
                                }
                            if (fall_pn > 0)
                            {
                                fall_pn--;
                            }
                        }
                       
                        
                            load1 = folderIndex;
                            if (load1 != folderIndex) load1 = folderIndex;
                            if (En_chek1.Checked)
                            {
                                load1 = folderIndex;
                            }
                        
                        update_system();
                    }; this.Invoke(inv);
                }

            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }

        string h1 = "OK";
        string h2 = "OK";
        string h3 = "OK";
        string h4 = "OK";
        string h5 = "OK";
        string h6 = "OK";

        string err_pic1 = "";
        string err_pic2 = "";
        Int16 _OKnum = 0;
        Int16 _NGnum = 0;
        Int16 _sum = 0;
        int save_pic = 0;
        private string vitri_Erpic(string so)
        {
            if (so == "1")
            {
                h1 = "NG";
                save_pic = 0;
            }
            else

            {
                h1 = "OK";

            }
            if (so == "2")
            {
                h2 = "NG";
                save_pic = 1;
            }
            else
            {
                h2 = "OK";

            }
            if (so == "3")
            {
                h3 = "NG";
                save_pic = 2;
            }
            else
            {
                h3 = "OK";

            }
            if (so == "4")
            {
                h4 = "NG";
                save_pic = 3;
            }
            else
            {
                h4 = "OK";

            }
            if (so == "5")
            {
                h5 = "NG";
                save_pic = 4;
            }
            else
            {
                h5 = "OK";

            }
            if (so == "6")
            {
                h6 = "NG";
                save_pic = 5;
            }
            else
            {
                h6 = "OK";

            }
            return err_pic;
        }
        string err_pic = "";
        private string error_Type(string get_error)
        {
            string error_type = "";
            if (get_error == "1")
            {
                error_type = "Incompleted Soldering";
            }
            if (get_error == "2")
            {
                error_type = "Flux";
            }
            if (get_error == "3")
            {
                error_type = "Tinned Winding";
            }
            if (get_error == "4")
            {
                error_type = "Tin on Base(Tin ball)";
            }
            if (get_error == "5")
            {
                error_type = "Damaged(Scratched)";
            }
            if (get_error == "6")
            {
                error_type = "Others";
            }
            return error_type;
        }
       int fall_pn = 0;
        Int64[] fall_number = new Int64[10];
        private void Tranfer1(string OPTION)
        {
            try
            {
                if (OPTION == "OK" && allow_check)
                {
                    MethodInvoker inv = delegate
                    {
                        DateTime dt = DateTime.Now;
                        string daytime = dt.Day.ToString() + "-" + dt.Month.ToString() + "-" + dt.Year.ToString();
                        DMY = daytime;
                        Parameter_app.OK_TEMP(DMY, load2.ToString());

                        string[] getpath = new string[7];

                        getpath = path_2_1.Split('-');

                        if (!Directory.Exists(system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH))
                        {
                            Directory.CreateDirectory(system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH);
                        }

                        pictureBox15.Image.Dispose();
                        pictureBox16.Image.Dispose();
                        pictureBox17.Image.Dispose();
                        pictureBox18.Image.Dispose();
                        pictureBox19.Image.Dispose();
                        pictureBox20.Image.Dispose();
                        if (picfull_2.Image != null) { picfull_2.Image.Dispose(); picfull_2.Image = null; }

                        if (auto_man)
                        {
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_1, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator2 + "-" + path_2_1);
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_2, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator2 + "-" + path_2_2);
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_3, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator2 + "-" + path_2_3);
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_4, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator2 + "-" + path_2_4);
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_5, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator2 + "-" + path_2_5);
                            File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_6, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator2 + "-" + path_2_6);
                            if (system_config.add_cam == "true")
                            {
                                File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_7, system_config.Map_Path_File_2 + @"/" + Parameter_app.OK_IMAGE_FOLDER_PATH + "/" + "OK-" + ID_Operator2 + "-" + path_2_7);
                            }


                            string[] tach = new string[2];
                            tach = getpath[6].Split('.');
                            Boolean check = sql_action.excute_data("INSERT INTO component_status (PN_Selector,Date,Time,Trace,ID,Status,Picture1,Picture2,Picture3,Picture4,Picture5,Picture6) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "','" + ID_Operator2 + "','OK','OK','OK','OK','OK','OK','OK')");
                            Boolean orderby = sql_action.excute_data("SELECT Time FROM component_status ORDER BY Time DESC");
                            var item = new ListViewItem(new[] { "" + PN_Selector + "-" + folderIndex.ToString() + "", "OK", "" });
                            listView1.Items.Add(item);
                        }

                        status(" [SYSTEM] " + " [OK]" + " SAVED IMAGE[" + load2.ToString() + "]");
                        folderIndex++;
                       
                            for (int i = 1; i <= fall_number.Length-1; i++)
                            {
                                if (folderIndex == fall_number[i] ) 
                                {
                                    folderIndex++;
                                   
                                    load2 = folderIndex;
                                    if (En_chek2.Checked)
                                    {
                                        load2 = folderIndex;
                                    }
                                }
                                if (fall_pn > 0)
                                {
                                    fall_pn--;
                                }
                            }
                            
                       
                        
                            load2 = folderIndex;
                            if (load2 != folderIndex) load2 = folderIndex;
                            if (En_chek2.Checked)
                            {
                                load2 = folderIndex;
                            }
                        
                        update_system();
                    }; this.Invoke(inv);
                }

                if (OPTION == "ERROR" && allow_check)
                {
                    MethodInvoker inv = delegate
                    {
                        DateTime dt = DateTime.Now;
                        string daytime = dt.Day.ToString() + "-" + dt.Month.ToString() + "-" + dt.Year.ToString();
                        DMY = daytime;
                        string[] save_path = new string[] { path_2_1, path_2_2, path_2_3, path_2_4, path_2_5, path_2_6, path_2_7 };
                        string[] getpath = new string[7];

                        getpath = path_2_1.Split('-');

                        Parameter_app.ERROR_TEMP(DMY);
                        if (!Directory.Exists(system_config.Map_Path_File_2 + @"/" + Parameter_app.ERROR_IMAGE_FOLDER_PATH))
                        {
                            Directory.CreateDirectory(system_config.Map_Path_File_2 + @"/" + Parameter_app.ERROR_IMAGE_FOLDER_PATH);
                        }

                        pictureBox15.Image.Dispose();
                        pictureBox16.Image.Dispose();
                        pictureBox17.Image.Dispose();
                        pictureBox18.Image.Dispose();
                        pictureBox19.Image.Dispose();
                        pictureBox20.Image.Dispose();
                        if (picfull_2.Image != null) { picfull_2.Image.Dispose(); picfull_2.Image = null; }
                        if (auto_man)
                        {
                            int s = 0;
                            foreach (string luu in save_path)
                            {
                                if (save_path[s] == null) break;
                                if (s == save_pic)
                                {
                                    File.Move(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + save_path[s], system_config.Map_Path_File_2 + @"/" + Parameter_app.ERROR_IMAGE_FOLDER_PATH + "/" + error_Type(loi_tam2) + "-" + ID_Operator2 + "-" + save_path[s]);
                                }
                                else
                                {
                                    if (File.Exists(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + save_path[s]))
                                    {
                                        File.Delete(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + save_path[s]);
                                    }

                                }
                                s++;
                            }
                            string[] tach = new string[2];
                            tach = getpath[6].Split('.');
                            Boolean check = sql_action.excute_data("INSERT INTO component_status (PN_Selector,Date,Time,Trace,ID,Status,Picture1,Picture2,Picture3,Picture4,Picture5,Picture6,NG_Type) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "','" + ID_Operator1 + "','NG','" + h1 + "','" + h2 + "','" + h3 + "','" + h4 + "','" + h5 + "','" + h6 + "','" + error_Type(loi_tam2) + "')");
                            Boolean insert = sql_action.excute_data("INSERT INTO NG_detail ([PN_Selector],[Date],[Time],[Trace]) VALUES (N'" + PN_Selector + "','" + getpath[1] + "','" + getpath[2] + "-" + getpath[3] + "-" + getpath[4] + "','" + tach[0] + "_" + err_pic2 + "')");
                            Boolean orderby = sql_action.excute_data("SELECT Time FROM component_status ORDER BY Time DESC");
                            var item = new ListViewItem(new[] { "" + PN_Selector + "-" + folderIndex.ToString() + "", "NG", error_Type(loi_tam2) });
                            listView1.Items.Add(item);
                            err_pic2 = "";
                        }
                        status(" [SYSTEM]" + " [ERROR]" + " SAVED IMAGE[" + system_config.Folder_load_check.ToString() + "]");
                        folderIndex++;
                        
                            for (int i = 1; i <=fall_number.Length-1; i++)
                            {
                                if (folderIndex == fall_number[i] )
                                {
                                    folderIndex++;
                                   
                                    load2 = folderIndex;
                                    if (En_chek2.Checked)
                                    {
                                        load2 = folderIndex;
                                    }
                                }
                            if (fall_pn > 0)
                            {
                                fall_pn--;
                            }
                        }
                        
                        
                            load2 = folderIndex;
                            if (load2 != folderIndex) load2 = folderIndex;
                            if (En_chek2.Checked)
                            {
                                load2 = folderIndex;
                            }
                       
                        update_system();
                    }; this.Invoke(inv);
                }
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        Int16 num1 = 0;
        Int16 num2 = 0;
        private void OK1_check()
        {
            try
            {
                if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null && pictureBox4.Image == null && pictureBox5.Image == null && pictureBox6.Image == null && !run_out1)
                {
                    return;
                }
                MethodInvoker inv = delegate
                {

                    num1++;
                    Tranfer("OK");

                    Program_Configuration.UpdateSystem_Config("inf_process", DateTime.Now.ToString());
                    inf_process();
                    upload_image();
                    _OKnum++;
                    _sum = (short)(_NGnum + _OKnum);
                    OKnum.Text = _OKnum.ToString();
                    totalPN.Text = _sum.ToString();
                    ck_num1.Text = num1.ToString();
                    //PLCS7_1200.Write("M68.5", 1);

                };
                this.Invoke(inv);
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        private void NG1_check()
        {
            try
            {
                if (pictureBox1.Image == null && pictureBox2.Image == null && pictureBox3.Image == null && pictureBox4.Image == null && pictureBox5.Image == null && pictureBox6.Image == null && !run_out1)
                {
                    return;
                }
                MethodInvoker inv = delegate
                {
                    num1++;
                    DateTime dt = DateTime.Now;
                    Tranfer("ERROR");

                    Program_Configuration.UpdateSystem_Config("inf_process", DateTime.Now.ToString());
                    inf_process();
                    upload_image();
                    _NGnum++;
                    _sum = (short)(_NGnum + _OKnum);
                    NGnum.Text = _NGnum.ToString();
                    totalPN.Text = _sum.ToString();
                    ck_num1.Text = num1.ToString();
                    //PLCS7_1200.Write("M68.5", 1);

                };
                this.Invoke(inv);
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        private void OK2_check()
        {

            try
            {
                if (pictureBox15.Image == null && pictureBox16.Image == null && pictureBox17.Image == null && pictureBox18.Image == null && pictureBox19.Image == null && pictureBox20.Image == null && !run_out2)
                {
                    return;
                }
                MethodInvoker inv = delegate
                {
                    num2++;
                    DateTime dt = DateTime.Now;
                    Tranfer1("OK");

                    Program_Configuration.UpdateSystem_Config("inf_process", dt.ToString());
                    inf_process();
                    update_image2();
                    _OKnum++;
                    _sum = (short)(_NGnum + _OKnum);
                    OKnum.Text = _OKnum.ToString();
                    totalPN.Text = _sum.ToString();
                    ck_num2.Text = num2.ToString();
                    //PLCS7_1200.Write("M68.6", 1);
                };
                this.Invoke(inv);
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        private void NG2_check()
        {
            try
            {
                if (pictureBox15.Image == null && pictureBox16.Image == null && pictureBox17.Image == null && pictureBox18.Image == null && pictureBox19.Image == null && pictureBox20.Image == null && !run_out2)
                {
                    return;
                }
                MethodInvoker inv = delegate
                {
                    num2++;
                    DateTime dt = DateTime.Now;
                    Tranfer1("ERROR");
                    Program_Configuration.UpdateSystem_Config("inf_process", dt.ToString());
                    inf_process();
                    update_image2();
                    _NGnum++;
                    _sum = (short)(_NGnum + _OKnum);
                    NGnum.Text = _NGnum.ToString();
                    totalPN.Text = _sum.ToString();
                    ck_num2.Text = num2.ToString();
                    //PLCS7_1200.Write("M68.6", 1);
                };
                this.Invoke(inv);
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        private int on1 = 0;

        #endregion
        private void zoom1(int pic, bool tran, int mode)
        {
            try
            {
                MethodInvoker inv = delegate
                {
                    if (!tran)
                    {
                        on1++;
                        ezoom1 = false;
                    }

                    if (on1 == 1 && allow_check)
                    {
                        pictureBox1.Hide();
                        pictureBox2.Hide();
                        pictureBox3.Hide();
                        pictureBox4.Hide();
                        pictureBox5.Hide();
                        pictureBox6.Hide();
                        capture1.Visible = false;
                        capture2.Visible = false;
                        capture3.Visible = false;
                        capture4.Visible = false;
                        capture5.Visible = false;
                        capture6.Visible = false;
                        Hname1.Visible = false;
                        Hname2.Visible = false;
                        Hname3.Visible = false;
                        Hname4.Visible = false;
                        Hname5.Visible = false;
                        Hname6.Visible = false;
                        c1_11.Visible = false;
                        c2_11.Visible = false;
                        c3_11.Visible = false;
                        c4_11.Visible = false;
                        C5_11.Visible = false;
                        C6_11.Visible = false;
                        C7_11.Visible = false;
                        c8_11.Visible = false;

                        c7_16.Visible = false;
                        c8_16.Visible = false;

                        c1_16.Visible = false;
                        c2_16.Visible = false;
                        c3_16.Visible = false;
                        c4_16.Visible = false;
                        c5_16.Visible = false;
                        c6_16.Visible = false;

                        c1_12.Visible = false;
                        c2_12.Visible = false;
                        c3_12.Visible = false;
                        c4_12.Visible = false;

                        c8_13.Visible = false;
                        c7_13.Visible = false;
                        c6_13.Visible = false;
                        c5_13.Visible = false;

                        c8_14.Visible = false;
                        c1_14.Visible = false;

                        c4_15.Visible = false;
                        c5_15.Visible = false;
                        //c1_11.Visible = false; c1_12.Visible = false;
                        //c2_11.Visible = false; c4_12.Visible = false;
                        //c3h1_1.Visible = false; c1h3_1.Visible = false;
                        //c4_11.Visible = false; c2h3_1.Visible = false;
                        //c1h6_1.Visible = false; c5_14.Visible = false;
                        //c2h6_1.Visible = false; c4_14.Visible = false;
                        //c3h6_1.Visible = false; c4_15.Visible = false;
                        //c4h6_1.Visible = false; c8_15.Visible = false;
                        if (!tran)
                        {
                            switch (pic)
                            {
                                case 1:
                                    pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_1);
                                    pic_full1.Show();
                                    p811.Text = c8_11.Text;
                                    p821.Text = C7_11.Text;
                                    p831.Text = C6_11.Text;
                                    p841.Text = C5_11.Text;
                                    p851.Text = c4_11.Text;
                                    p861.Text = c3_11.Text;
                                    p871.Text = c2_11.Text;
                                    p881.Text = c1_11.Text;



                                    p811.Visible = true;
                                    p821.Visible = true;
                                    p831.Visible = true;
                                    p841.Visible = true;
                                    p851.Visible = true;
                                    p861.Visible = true;
                                    p871.Visible = true;
                                    p881.Visible = true;

                                    p411.Visible = false;
                                    p421.Visible = false;
                                    p431.Visible = false;
                                    p441.Visible = false;
                                    hinh1 = 1;
                                    break;
                                case 2:
                                    pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_2);
                                    pic_full1.Show();
                                    p411.Text = c1_12.Text;
                                    p421.Text = c2_12.Text;
                                    p431.Text = c3_12.Text;
                                    p441.Text = c4_12.Text;

                                    p811.Visible = false;
                                    p821.Visible = false;
                                    p831.Visible = false;
                                    p841.Visible = false;
                                    p851.Visible = false;
                                    p861.Visible = false;
                                    p871.Visible = false;
                                    p881.Visible = false;

                                    p411.Visible = true;
                                    p421.Visible = true;
                                    p431.Visible = true;
                                    p441.Visible = true;
                                    hinh1 = 2;
                                    break;
                                case 3:
                                    pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_3);
                                    pic_full1.Show();
                                    p411.Text = c8_13.Text;
                                    p421.Text = c7_13.Text;
                                    p431.Text = c6_13.Text;
                                    p441.Text = c5_13.Text;

                                    p811.Visible = false;
                                    p821.Visible = false;
                                    p831.Visible = false;
                                    p841.Visible = false;
                                    p851.Visible = false;
                                    p861.Visible = false;
                                    p871.Visible = false;
                                    p881.Visible = false;

                                    p411.Visible = true;
                                    p421.Visible = true;
                                    p431.Visible = true;
                                    p441.Visible = true;
                                    hinh1 = 3;
                                    break;
                                case 4:
                                    pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_4);
                                    pic_full1.Show();
                                    p411.Text = c8_14.Text;
                                    p441.Text = c1_14.Text;

                                    p811.Visible = false;
                                    p821.Visible = false;
                                    p831.Visible = false;
                                    p841.Visible = false;
                                    p851.Visible = false;
                                    p861.Visible = false;
                                    p871.Visible = false;
                                    p881.Visible = false;

                                    p441.Visible = true;
                                    p411.Visible = true;
                                    p421.Visible = false;
                                    p431.Visible = false;
                                    hinh1 = 4;
                                    break;
                                case 5:
                                    pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_5);
                                    pic_full1.Show();
                                    p411.Text = c4_15.Text;
                                    p441.Text = c5_15.Text;

                                    p811.Visible = false;
                                    p821.Visible = false;
                                    p831.Visible = false;
                                    p841.Visible = false;
                                    p851.Visible = false;
                                    p861.Visible = false;
                                    p871.Visible = false;
                                    p881.Visible = false;

                                    p441.Visible = true;
                                    p411.Visible = true;
                                    p421.Visible = false;
                                    p431.Visible = false;
                                    hinh1 = 5;
                                    break;
                                case 6:
                                    pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_6);
                                    pic_full1.Show();
                                    p811.Text = c1_16.Text;
                                    p821.Text = c2_16.Text;
                                    p831.Text = c3_16.Text;
                                    p841.Text = c4_16.Text;
                                    p851.Text = c5_16.Text;
                                    p861.Text = c6_16.Text;
                                    p871.Text = c7_16.Text;
                                    p881.Text = c8_16.Text;



                                    p811.Visible = true;
                                    p821.Visible = true;
                                    p831.Visible = true;
                                    p841.Visible = true;
                                    p851.Visible = true;
                                    p861.Visible = true;
                                    p871.Visible = true;
                                    p881.Visible = true;

                                    p411.Visible = false;
                                    p421.Visible = false;
                                    p431.Visible = false;
                                    p441.Visible = false;
                                    hinh1 = 6;
                                    break;

                            }
                        }
                        if (tran)
                        {
                            if (mode == 1)
                            {
                                hinh1++;
                                if (hinh1 > 6)
                                {
                                    ezoom1 = true;
                                    if (pic_full1.Image != null)
                                    {
                                        pic_full1.Image.Dispose();
                                    }
                                    pic_full1.Hide();
                                    pictureBox1.Show();
                                    pictureBox2.Show();
                                    pictureBox3.Show();
                                    pictureBox4.Show();
                                    pictureBox5.Show();
                                    pictureBox6.Show();
                                    capture1.Visible = true;
                                    capture2.Visible = true;
                                    capture3.Visible = true;
                                    capture4.Visible = true;
                                    capture5.Visible = true;
                                    capture6.Visible = true;
                                    Hname1.Visible = true;
                                    Hname2.Visible = true;
                                    Hname3.Visible = true;
                                    Hname4.Visible = true;
                                    Hname5.Visible = true;
                                    Hname6.Visible = true;



                                    c1_11.Visible = true;
                                    c2_11.Visible = true;
                                    c3_11.Visible = true;
                                    c4_11.Visible = true;
                                    C5_11.Visible = true;
                                    C6_11.Visible = true;
                                    C7_11.Visible = true;
                                    c8_11.Visible = true;

                                    c1_16.Visible = true;
                                    c2_16.Visible = true;
                                    c3_16.Visible = true;
                                    c4_16.Visible = true;
                                    c5_16.Visible = true;
                                    c6_16.Visible = true;

                                    c1_12.Visible = true;
                                    c2_12.Visible = true;
                                    c3_12.Visible = true;
                                    c4_12.Visible = true;

                                    c8_13.Visible = true;
                                    c7_13.Visible = true;
                                    c6_13.Visible = true;
                                    c5_13.Visible = true;

                                    c8_14.Visible = true;
                                    c1_14.Visible = true;

                                    c4_15.Visible = true;
                                    c5_15.Visible = true;

                                    c7_16.Visible = true;
                                    c8_16.Visible = true;

                                    p811.Visible = false;
                                    p821.Visible = false;
                                    p831.Visible = false;
                                    p841.Visible = false;
                                    p851.Visible = false;
                                    p861.Visible = false;
                                    p871.Visible = false;
                                    p881.Visible = false;

                                    p411.Visible = false;
                                    p421.Visible = false;
                                    p431.Visible = false;
                                    p441.Visible = false;
                                    on1 = 0;
                                }
                            }
                            if (mode == 2)
                            {
                                hinh1--;
                                if (hinh1 < 1)
                                {
                                    ezoom1 = true;
                                    if (pic_full1.Image != null)
                                    {
                                        pic_full1.Image.Dispose();
                                    }
                                    pic_full1.Hide();
                                    pictureBox1.Show();
                                    pictureBox2.Show();
                                    pictureBox3.Show();
                                    pictureBox4.Show();
                                    pictureBox5.Show();
                                    pictureBox6.Show();
                                    capture1.Visible = true;
                                    capture2.Visible = true;
                                    capture3.Visible = true;
                                    capture4.Visible = true;
                                    capture5.Visible = true;
                                    capture6.Visible = true;
                                    Hname1.Visible = true;
                                    Hname2.Visible = true;
                                    Hname3.Visible = true;
                                    Hname4.Visible = true;
                                    Hname5.Visible = true;
                                    Hname6.Visible = true;



                                    c1_11.Visible = true;
                                    c2_11.Visible = true;
                                    c3_11.Visible = true;
                                    c4_11.Visible = true;
                                    C5_11.Visible = true;
                                    C6_11.Visible = true;
                                    C7_11.Visible = true;
                                    c8_11.Visible = true;

                                    c1_16.Visible = true;
                                    c2_16.Visible = true;
                                    c3_16.Visible = true;
                                    c4_16.Visible = true;
                                    c5_16.Visible = true;
                                    c6_16.Visible = true;

                                    c1_12.Visible = true;
                                    c2_12.Visible = true;
                                    c3_12.Visible = true;
                                    c4_12.Visible = true;

                                    c8_13.Visible = true;
                                    c7_13.Visible = true;
                                    c6_13.Visible = true;
                                    c5_13.Visible = true;

                                    c8_14.Visible = true;
                                    c1_14.Visible = true;

                                    c4_15.Visible = true;
                                    c5_15.Visible = true;

                                    c7_16.Visible = true;
                                    c8_16.Visible = true;

                                    p811.Visible = false;
                                    p821.Visible = false;
                                    p831.Visible = false;
                                    p841.Visible = false;
                                    p851.Visible = false;
                                    p861.Visible = false;
                                    p871.Visible = false;
                                    p881.Visible = false;

                                    p411.Visible = false;
                                    p421.Visible = false;
                                    p431.Visible = false;
                                    p441.Visible = false;
                                    on1 = 0;
                                }
                            }
                            if (!ezoom1)
                            {
                                switch (hinh1)
                                {
                                    case 1:
                                        if (pic_full1.Image != null)
                                        {
                                            pic_full1.Image.Dispose();
                                        }
                                        pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_1);
                                        pic_full1.Show();
                                        p811.Text = c8_11.Text;
                                        p821.Text = C7_11.Text;
                                        p831.Text = C6_11.Text;
                                        p841.Text = C5_11.Text;
                                        p851.Text = c4_11.Text;
                                        p861.Text = c3_11.Text;
                                        p871.Text = c2_11.Text;
                                        p881.Text = c1_11.Text;



                                        p811.Visible = true;
                                        p821.Visible = true;
                                        p831.Visible = true;
                                        p841.Visible = true;
                                        p851.Visible = true;
                                        p861.Visible = true;
                                        p871.Visible = true;
                                        p881.Visible = true;

                                        p411.Visible = false;
                                        p421.Visible = false;
                                        p431.Visible = false;
                                        p441.Visible = false;

                                        break;
                                    case 2:
                                        if (pic_full1.Image != null)
                                        {
                                            pic_full1.Image.Dispose();
                                        }
                                        pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_2);
                                        pic_full1.Show();
                                        p411.Text = c1_12.Text;
                                        p421.Text = c2_12.Text;
                                        p431.Text = c3_12.Text;
                                        p441.Text = c4_12.Text;

                                        p811.Visible = false;
                                        p821.Visible = false;
                                        p831.Visible = false;
                                        p841.Visible = false;
                                        p851.Visible = false;
                                        p861.Visible = false;
                                        p871.Visible = false;
                                        p881.Visible = false;

                                        p411.Visible = true;
                                        p421.Visible = true;
                                        p431.Visible = true;
                                        p441.Visible = true;

                                        break;
                                    case 3:
                                        if (pic_full1.Image != null)
                                        {
                                            pic_full1.Image.Dispose();
                                        }
                                        pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_3);
                                        pic_full1.Show();
                                        p411.Text = c8_13.Text;
                                        p421.Text = c7_13.Text;
                                        p431.Text = c6_13.Text;
                                        p441.Text = c5_13.Text;

                                        p811.Visible = false;
                                        p821.Visible = false;
                                        p831.Visible = false;
                                        p841.Visible = false;
                                        p851.Visible = false;
                                        p861.Visible = false;
                                        p871.Visible = false;
                                        p881.Visible = false;

                                        p411.Visible = true;
                                        p421.Visible = true;
                                        p431.Visible = true;
                                        p441.Visible = true;
                                        break;
                                    case 4:
                                        if (pic_full1.Image != null)
                                        {
                                            pic_full1.Image.Dispose();
                                        }
                                        pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_4);
                                        pic_full1.Show();
                                        p411.Text = c8_14.Text;
                                        p441.Text = c1_14.Text;

                                        p811.Visible = false;
                                        p821.Visible = false;
                                        p831.Visible = false;
                                        p841.Visible = false;
                                        p851.Visible = false;
                                        p861.Visible = false;
                                        p871.Visible = false;
                                        p881.Visible = false;

                                        p441.Visible = true;
                                        p411.Visible = true;
                                        p421.Visible = false;
                                        p431.Visible = false;
                                        break;
                                    case 5:
                                        if (pic_full1.Image != null)
                                        {
                                            pic_full1.Image.Dispose();
                                        }
                                        pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_5);
                                        pic_full1.Show();
                                        p411.Text = c4_15.Text;
                                        p441.Text = c5_15.Text;

                                        p811.Visible = false;
                                        p821.Visible = false;
                                        p831.Visible = false;
                                        p841.Visible = false;
                                        p851.Visible = false;
                                        p861.Visible = false;
                                        p871.Visible = false;
                                        p881.Visible = false;

                                        p441.Visible = true;
                                        p411.Visible = true;
                                        p421.Visible = false;
                                        p431.Visible = false;
                                        break;
                                    case 6:
                                        if (pic_full1.Image != null)
                                        {
                                            pic_full1.Image.Dispose();
                                        }
                                        pic_full1.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_1_6);
                                        pic_full1.Show();
                                        p811.Text = c1_16.Text;
                                        p821.Text = c2_16.Text;
                                        p831.Text = c3_16.Text;
                                        p841.Text = c4_16.Text;
                                        p851.Text = c5_16.Text;
                                        p861.Text = c6_16.Text;
                                        p871.Text = c7_16.Text;
                                        p881.Text = c8_16.Text;



                                        p811.Visible = true;
                                        p821.Visible = true;
                                        p831.Visible = true;
                                        p841.Visible = true;
                                        p851.Visible = true;
                                        p861.Visible = true;
                                        p871.Visible = true;
                                        p881.Visible = true;

                                        p411.Visible = false;
                                        p421.Visible = false;
                                        p431.Visible = false;
                                        p441.Visible = false;
                                        break;

                                }
                            }

                        }
                    }
                    if (on1 == 2 && allow_check)
                    {
                        ezoom1 = false;
                        if (pic_full1.Image != null)
                        {
                            pic_full1.Image.Dispose();
                        }
                        on1 = 0;
                        pic_full1.Hide();
                        pictureBox1.Show();
                        pictureBox2.Show();
                        pictureBox3.Show();
                        pictureBox4.Show();
                        pictureBox5.Show();
                        pictureBox6.Show();
                        capture1.Visible = true;
                        capture2.Visible = true;
                        capture3.Visible = true;
                        capture4.Visible = true;
                        capture5.Visible = true;
                        capture6.Visible = true;
                        Hname1.Visible = true;
                        Hname2.Visible = true;
                        Hname3.Visible = true;
                        Hname4.Visible = true;
                        Hname5.Visible = true;
                        Hname6.Visible = true;



                        c1_11.Visible = true;
                        c2_11.Visible = true;
                        c3_11.Visible = true;
                        c4_11.Visible = true;
                        C5_11.Visible = true;
                        C6_11.Visible = true;
                        C7_11.Visible = true;
                        c8_11.Visible = true;

                        c1_16.Visible = true;
                        c2_16.Visible = true;
                        c3_16.Visible = true;
                        c4_16.Visible = true;
                        c5_16.Visible = true;
                        c6_16.Visible = true;

                        c1_12.Visible = true;
                        c2_12.Visible = true;
                        c3_12.Visible = true;
                        c4_12.Visible = true;

                        c8_13.Visible = true;
                        c7_13.Visible = true;
                        c6_13.Visible = true;
                        c5_13.Visible = true;

                        c8_14.Visible = true;
                        c1_14.Visible = true;

                        c4_15.Visible = true;
                        c5_15.Visible = true;

                        c7_16.Visible = true;
                        c8_16.Visible = true;

                        p811.Visible = false;
                        p821.Visible = false;
                        p831.Visible = false;
                        p841.Visible = false;
                        p851.Visible = false;
                        p861.Visible = false;
                        p871.Visible = false;
                        p881.Visible = false;

                        p411.Visible = false;
                        p421.Visible = false;
                        p431.Visible = false;
                        p441.Visible = false;

                    }
                };
                this.Invoke(inv);
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        private int on2 = 0;
        bool ezoom1 = false;
        bool ezoom2 = false;
        int hinh1 = 1;
        int hinh2 = 1;
        private void zoom2(int pic, bool tran, int mode)
        {
            try
            {
                MethodInvoker inv = delegate
                {
                    if (!tran)
                    {
                        on2++;
                        ezoom2 = false;
                    }

                    if (on2 == 1 && allow_check)
                    {
                        pictureBox15.Hide();
                        pictureBox16.Hide();
                        pictureBox17.Hide();
                        pictureBox18.Hide();
                        pictureBox19.Hide();
                        pictureBox20.Hide();
                        capture_7.Visible = false;
                        capture_8.Visible = false;
                        capture_9.Visible = false;
                        capture_10.Visible = false;
                        capture_11.Visible = false;
                        capture_12.Visible = false;
                        Hname_7.Visible = false;
                        Hname_8.Visible = false;
                        Hname_9.Visible = false;
                        Hname_10.Visible = false;
                        Hname_11.Visible = false;
                        Hname_12.Visible = false;

                        c1_21.Visible = false;
                        c2_21.Visible = false;
                        c3_21.Visible = false;
                        c4_21.Visible = false;
                        c5_21.Visible = false;
                        c6_21.Visible = false;
                        c7_21.Visible = false;
                        c8_21.Visible = false;

                        c1_26.Visible = false;
                        c2_26.Visible = false;
                        c3_26.Visible = false;
                        c4_26.Visible = false;
                        c5_26.Visible = false;
                        c6_26.Visible = false;

                        c1_22.Visible = false;
                        c2_22.Visible = false;
                        c3_22.Visible = false;
                        c4_22.Visible = false;

                        c8_23.Visible = false;
                        c7_23.Visible = false;
                        c6_23.Visible = false;
                        c5_23.Visible = false;

                        c8_24.Visible = false;
                        c1_24.Visible = false;

                        c4_25.Visible = false;
                        c5_25.Visible = false;

                        c8_26.Visible = false;
                        c7_26.Visible = false;
                        if (!tran)
                        {
                            switch (pic)
                            {
                                case 1:
                                    picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_1);
                                    picfull_2.Show();
                                    p812.Text = c8_21.Text;
                                    p822.Text = c7_21.Text;
                                    p832.Text = c6_21.Text;
                                    p842.Text = c5_21.Text;
                                    p852.Text = c4_21.Text;
                                    p862.Text = c3_21.Text;
                                    p872.Text = c2_21.Text;
                                    p882.Text = c1_21.Text;



                                    p812.Visible = true;
                                    p822.Visible = true;
                                    p832.Visible = true;
                                    p842.Visible = true;
                                    p852.Visible = true;
                                    p862.Visible = true;
                                    p872.Visible = true;
                                    p882.Visible = true;

                                    p412.Visible = false;
                                    p422.Visible = false;
                                    p432.Visible = false;
                                    p442.Visible = false;
                                    hinh2 = 1;
                                    break;
                                case 2:
                                    picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_2);
                                    picfull_2.Show();
                                    p412.Text = c1_22.Text;
                                    p422.Text = c2_22.Text;
                                    p432.Text = c3_22.Text;
                                    p442.Text = c4_22.Text;

                                    p812.Visible = false;
                                    p822.Visible = false;
                                    p832.Visible = false;
                                    p842.Visible = false;
                                    p852.Visible = false;
                                    p862.Visible = false;
                                    p872.Visible = false;
                                    p882.Visible = false;

                                    p412.Visible = true;
                                    p422.Visible = true;
                                    p432.Visible = true;
                                    p442.Visible = true;
                                    hinh2 = 2;
                                    break;
                                case 3:
                                    picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_3);
                                    picfull_2.Show();
                                    p412.Text = c8_23.Text;
                                    p422.Text = c7_23.Text;
                                    p432.Text = c6_23.Text;
                                    p442.Text = c5_23.Text;

                                    p812.Visible = false;
                                    p822.Visible = false;
                                    p832.Visible = false;
                                    p842.Visible = false;
                                    p852.Visible = false;
                                    p862.Visible = false;
                                    p872.Visible = false;
                                    p882.Visible = false;

                                    p412.Visible = true;
                                    p422.Visible = true;
                                    p432.Visible = true;
                                    p442.Visible = true;
                                    hinh2 = 3;

                                    break;
                                case 4:
                                    picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_4);
                                    picfull_2.Show();
                                    p412.Text = c8_24.Text;
                                    p442.Text = c1_24.Text;

                                    p812.Visible = false;
                                    p822.Visible = false;
                                    p832.Visible = false;
                                    p842.Visible = false;
                                    p852.Visible = false;
                                    p862.Visible = false;
                                    p872.Visible = false;
                                    p882.Visible = false;

                                    p442.Visible = true;
                                    p412.Visible = true;
                                    p422.Visible = false;
                                    p432.Visible = false;
                                    hinh2 = 4;
                                    break;

                                case 5:
                                    picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_5);
                                    picfull_2.Show();
                                    p412.Text = c4_25.Text;
                                    p442.Text = c5_25.Text;

                                    p812.Visible = false;
                                    p822.Visible = false;
                                    p832.Visible = false;
                                    p842.Visible = false;
                                    p852.Visible = false;
                                    p862.Visible = false;
                                    p872.Visible = false;
                                    p882.Visible = false;

                                    p442.Visible = true;
                                    p412.Visible = true;
                                    p422.Visible = false;
                                    p432.Visible = false;
                                    hinh2 = 5;
                                    break;
                                case 6:
                                    picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_6);
                                    picfull_2.Show();
                                    p812.Text = c1_26.Text;
                                    p822.Text = c2_26.Text;
                                    p832.Text = c3_26.Text;
                                    p842.Text = c4_26.Text;
                                    p852.Text = c5_26.Text;
                                    p862.Text = c6_26.Text;
                                    p872.Text = c7_26.Text;
                                    p882.Text = c8_26.Text;



                                    p812.Visible = true;
                                    p822.Visible = true;
                                    p832.Visible = true;
                                    p842.Visible = true;
                                    p852.Visible = true;
                                    p862.Visible = true;
                                    p872.Visible = true;
                                    p882.Visible = true;

                                    p412.Visible = false;
                                    p422.Visible = false;
                                    p432.Visible = false;
                                    p442.Visible = false;
                                    hinh2 = 6;
                                    break;

                            }
                        }
                        if (tran)
                        {
                            if (mode == 1)
                            {
                                hinh2++;
                                if (hinh2 > 6)
                                {
                                    ezoom2 = true;
                                    if (picfull_2.Image != null)
                                    {
                                        picfull_2.Image.Dispose();
                                    }
                                    picfull_2.Hide();
                                    pictureBox15.Show();
                                    pictureBox16.Show();
                                    pictureBox17.Show();
                                    pictureBox18.Show();
                                    pictureBox19.Show();
                                    pictureBox20.Show();
                                    capture_7.Visible = true;
                                    capture_8.Visible = true;
                                    capture_9.Visible = true;
                                    capture_10.Visible = true;
                                    capture_11.Visible = true;
                                    capture_12.Visible = true;
                                    Hname_7.Visible = true;
                                    Hname_8.Visible = true;
                                    Hname_9.Visible = true;
                                    Hname_10.Visible = true;
                                    Hname_11.Visible = true;
                                    Hname_12.Visible = true;




                                    c1_21.Visible = true;
                                    c2_21.Visible = true;
                                    c3_21.Visible = true;
                                    c4_21.Visible = true;
                                    c5_21.Visible = true;
                                    c6_21.Visible = true;
                                    c7_21.Visible = true;
                                    c8_21.Visible = true;

                                    c1_26.Visible = true;
                                    c2_26.Visible = true;
                                    c3_26.Visible = true;
                                    c4_26.Visible = true;
                                    c5_26.Visible = true;
                                    c6_26.Visible = true;

                                    c1_22.Visible = true;
                                    c2_22.Visible = true;
                                    c3_22.Visible = true;
                                    c4_22.Visible = true;

                                    c8_23.Visible = true;
                                    c7_23.Visible = true;
                                    c6_23.Visible = true;
                                    c5_23.Visible = true;

                                    c8_24.Visible = true;
                                    c1_24.Visible = true;

                                    c4_25.Visible = true;
                                    c5_25.Visible = true;

                                    c8_26.Visible = true;
                                    c7_26.Visible = true;

                                    p812.Visible = false;
                                    p822.Visible = false;
                                    p832.Visible = false;
                                    p842.Visible = false;
                                    p852.Visible = false;
                                    p862.Visible = false;
                                    p872.Visible = false;
                                    p882.Visible = false;

                                    p411.Visible = false;
                                    p421.Visible = false;
                                    p431.Visible = false;
                                    p441.Visible = false;
                                    on2 = 0;
                                }
                            }
                            if (mode == 2)
                            {
                                hinh2--;
                                if (hinh2 < 1)
                                {
                                    ezoom2 = true;
                                    if (picfull_2.Image != null)
                                    {
                                        picfull_2.Image.Dispose();
                                    }
                                    picfull_2.Hide();
                                    pictureBox15.Show();
                                    pictureBox16.Show();
                                    pictureBox17.Show();
                                    pictureBox18.Show();
                                    pictureBox19.Show();
                                    pictureBox20.Show();
                                    capture_7.Visible = true;
                                    capture_8.Visible = true;
                                    capture_9.Visible = true;
                                    capture_10.Visible = true;
                                    capture_11.Visible = true;
                                    capture_12.Visible = true;
                                    Hname_7.Visible = true;
                                    Hname_8.Visible = true;
                                    Hname_9.Visible = true;
                                    Hname_10.Visible = true;
                                    Hname_11.Visible = true;
                                    Hname_12.Visible = true;




                                    c1_21.Visible = true;
                                    c2_21.Visible = true;
                                    c3_21.Visible = true;
                                    c4_21.Visible = true;
                                    c5_21.Visible = true;
                                    c6_21.Visible = true;
                                    c7_21.Visible = true;
                                    c8_21.Visible = true;

                                    c1_26.Visible = true;
                                    c2_26.Visible = true;
                                    c3_26.Visible = true;
                                    c4_26.Visible = true;
                                    c5_26.Visible = true;
                                    c6_26.Visible = true;

                                    c1_22.Visible = true;
                                    c2_22.Visible = true;
                                    c3_22.Visible = true;
                                    c4_22.Visible = true;

                                    c8_23.Visible = true;
                                    c7_23.Visible = true;
                                    c6_23.Visible = true;
                                    c5_23.Visible = true;

                                    c8_24.Visible = true;
                                    c1_24.Visible = true;

                                    c4_25.Visible = true;
                                    c5_25.Visible = true;

                                    c8_26.Visible = true;
                                    c7_26.Visible = true;

                                    p812.Visible = false;
                                    p822.Visible = false;
                                    p832.Visible = false;
                                    p842.Visible = false;
                                    p852.Visible = false;
                                    p862.Visible = false;
                                    p872.Visible = false;
                                    p882.Visible = false;

                                    p411.Visible = false;
                                    p421.Visible = false;
                                    p431.Visible = false;
                                    p441.Visible = false;
                                    on2 = 0;
                                }
                            }
                            if (!ezoom2)
                            {
                                switch (hinh2)
                                {
                                    case 1:
                                        if (picfull_2.Image != null)
                                        {
                                            picfull_2.Image.Dispose();
                                        }
                                        picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_1);
                                        picfull_2.Show();
                                        p812.Text = c8_21.Text;
                                        p822.Text = c7_21.Text;
                                        p832.Text = c6_21.Text;
                                        p842.Text = c5_21.Text;
                                        p852.Text = c4_21.Text;
                                        p862.Text = c3_21.Text;
                                        p872.Text = c2_21.Text;
                                        p882.Text = c1_21.Text;



                                        p812.Visible = true;
                                        p822.Visible = true;
                                        p832.Visible = true;
                                        p842.Visible = true;
                                        p852.Visible = true;
                                        p862.Visible = true;
                                        p872.Visible = true;
                                        p882.Visible = true;

                                        p412.Visible = false;
                                        p422.Visible = false;
                                        p432.Visible = false;
                                        p442.Visible = false;
                                        hinh2 = 1;
                                        break;
                                    case 2:
                                        if (picfull_2.Image != null)
                                        {
                                            picfull_2.Image.Dispose();
                                        }
                                        picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_2);
                                        picfull_2.Show();
                                        p412.Text = c1_22.Text;
                                        p422.Text = c2_22.Text;
                                        p432.Text = c3_22.Text;
                                        p442.Text = c4_22.Text;

                                        p812.Visible = false;
                                        p822.Visible = false;
                                        p832.Visible = false;
                                        p842.Visible = false;
                                        p852.Visible = false;
                                        p862.Visible = false;
                                        p872.Visible = false;
                                        p882.Visible = false;

                                        p412.Visible = true;
                                        p422.Visible = true;
                                        p432.Visible = true;
                                        p442.Visible = true;
                                        hinh2 = 2;
                                        break;
                                    case 3:
                                        if (picfull_2.Image != null)
                                        {
                                            picfull_2.Image.Dispose();
                                        }
                                        picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_3);
                                        picfull_2.Show();
                                        p412.Text = c8_23.Text;
                                        p422.Text = c7_23.Text;
                                        p432.Text = c6_23.Text;
                                        p442.Text = c5_23.Text;

                                        p812.Visible = false;
                                        p822.Visible = false;
                                        p832.Visible = false;
                                        p842.Visible = false;
                                        p852.Visible = false;
                                        p862.Visible = false;
                                        p872.Visible = false;
                                        p882.Visible = false;

                                        p412.Visible = true;
                                        p422.Visible = true;
                                        p432.Visible = true;
                                        p442.Visible = true;
                                        hinh2 = 3;

                                        break;
                                    case 4:
                                        if (picfull_2.Image != null)
                                        {
                                            picfull_2.Image.Dispose();
                                        }
                                        picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_4);
                                        picfull_2.Show();
                                        p412.Text = c8_24.Text;
                                        p442.Text = c1_24.Text;

                                        p812.Visible = false;
                                        p822.Visible = false;
                                        p832.Visible = false;
                                        p842.Visible = false;
                                        p852.Visible = false;
                                        p862.Visible = false;
                                        p872.Visible = false;
                                        p882.Visible = false;

                                        p442.Visible = true;
                                        p412.Visible = true;
                                        hinh2 = 4;
                                        break;

                                    case 5:
                                        if (picfull_2.Image != null)
                                        {
                                            picfull_2.Image.Dispose();
                                        }
                                        picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_5);
                                        picfull_2.Show();
                                        p412.Text = c4_25.Text;
                                        p442.Text = c5_25.Text;

                                        p812.Visible = false;
                                        p822.Visible = false;
                                        p832.Visible = false;
                                        p842.Visible = false;
                                        p852.Visible = false;
                                        p862.Visible = false;
                                        p872.Visible = false;
                                        p882.Visible = false;

                                        p442.Visible = true;
                                        p412.Visible = true;
                                        hinh2 = 5;
                                        break;
                                    case 6:
                                        if (picfull_2.Image != null)
                                        {
                                            picfull_2.Image.Dispose();
                                        }
                                        picfull_2.Image = Image.FromFile(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH + "/" + path_2_6);
                                        picfull_2.Show();
                                        p812.Text = c1_26.Text;
                                        p822.Text = c2_26.Text;
                                        p832.Text = c3_26.Text;
                                        p842.Text = c4_26.Text;
                                        p852.Text = c5_26.Text;
                                        p862.Text = c6_26.Text;
                                        p872.Text = c7_26.Text;
                                        p882.Text = c8_26.Text;



                                        p812.Visible = true;
                                        p822.Visible = true;
                                        p832.Visible = true;
                                        p842.Visible = true;
                                        p852.Visible = true;
                                        p862.Visible = true;
                                        p872.Visible = true;
                                        p882.Visible = true;

                                        p412.Visible = false;
                                        p422.Visible = false;
                                        p432.Visible = false;
                                        p442.Visible = false;
                                        hinh2 = 6;
                                        break;

                                }
                            }
                        }

                    }
                    if (on2 == 2 && allow_check)
                    {
                        ezoom2 = false;
                        if (picfull_2.Image != null)
                        {
                            picfull_2.Image.Dispose();
                        }
                        on2 = 0;
                        picfull_2.Hide();
                        pictureBox15.Show();
                        pictureBox16.Show();
                        pictureBox17.Show();
                        pictureBox18.Show();
                        pictureBox19.Show();
                        pictureBox20.Show();
                        capture_7.Visible = true;
                        capture_8.Visible = true;
                        capture_9.Visible = true;
                        capture_10.Visible = true;
                        capture_11.Visible = true;
                        capture_12.Visible = true;
                        Hname_7.Visible = true;
                        Hname_8.Visible = true;
                        Hname_9.Visible = true;
                        Hname_10.Visible = true;
                        Hname_11.Visible = true;
                        Hname_12.Visible = true;




                        c1_21.Visible = true;
                        c2_21.Visible = true;
                        c3_21.Visible = true;
                        c4_21.Visible = true;
                        c5_21.Visible = true;
                        c6_21.Visible = true;
                        c7_21.Visible = true;
                        c8_21.Visible = true;

                        c1_26.Visible = true;
                        c2_26.Visible = true;
                        c3_26.Visible = true;
                        c4_26.Visible = true;
                        c5_26.Visible = true;
                        c6_26.Visible = true;

                        c1_22.Visible = true;
                        c2_22.Visible = true;
                        c3_22.Visible = true;
                        c4_22.Visible = true;

                        c8_23.Visible = true;
                        c7_23.Visible = true;
                        c6_23.Visible = true;
                        c5_23.Visible = true;

                        c8_24.Visible = true;
                        c1_24.Visible = true;

                        c4_25.Visible = true;
                        c5_25.Visible = true;

                        c8_26.Visible = true;
                        c7_26.Visible = true;

                        p812.Visible = false;
                        p822.Visible = false;
                        p832.Visible = false;
                        p842.Visible = false;
                        p852.Visible = false;
                        p862.Visible = false;
                        p872.Visible = false;
                        p882.Visible = false;

                        p411.Visible = false;
                        p421.Visible = false;
                        p431.Visible = false;
                        p441.Visible = false;

                    }
                };
                this.Invoke(inv);
            }
            catch (Exception ex)
            {
                status(ex.Message);
                MessageBox.Show(ex.Message);
            }
        }
        bool up_hinh = false;
        private void General_tab_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            if (General_tab.SelectedIndex == 2 && start_check && count_6 > 1)
            {
                stt++;
                allow_check = true;
                if (stt == 1)
                {
                    //DirectoryInfo d = new DirectoryInfo(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH );
                    upload_image();
                    if (folderIndex == 0)
                    {
                        folderIndex++;
                        load2 = folderIndex;
                    }
                    update_image2();
                    up_hinh = true;

                }

                if (stt > 1)
                {
                    if (start_check && count_6 > 1 && !up_hinh)
                    {
                        upload_image();
                        if (folderIndex == 0)
                        {
                            folderIndex++;
                            load2 = folderIndex;
                        }
                        update_image2();
                        up_hinh = true;
                    }
                    if (run_out1 && folderIndex < count_6)
                    {
                        upload_image();
                        run_out1 = false;
                    }
                    if (run_out2 && folderIndex < count_6)
                    {
                        update_image2();
                        run_out2 = false;
                    }
                }
            }
            else allow_check = false;
            
           
            //else nhapid = false;
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please stop program first!");
                return;
            }
            var result = MessageBox.Show("Do you want to reset Program to default setting", "RESET", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                count_1 = 0;
                count_2 = 0;
                count_3 = 0;
                count_4 = 0;
                count_5 = 0;
                count_6 = 0;
                count_7 = 0;
                folderIndex = 0;
                load1 = 0;
                load2 = 1;
                _OKnum = 0;
                _NGnum = 0;
                _sum = 0;
                Program_Configuration.UpdateSystem_Config("Folder_index_tranfer", load1.ToString());
                Program_Configuration.UpdateSystem_Config("same_folder_1", folderIndex.ToString());
                Program_Configuration.UpdateSystem_Config("Folder_load_check", load2.ToString());
                Program_Configuration.UpdateSystem_Config("Location_cam1_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam2_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam3_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam4_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam5_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam6_folder", "0");
                Program_Configuration.UpdateSystem_Config("Location_cam7_folder", "0");

                using (StreamWriter sw = new StreamWriter(@"C:\Users\Admin\source\repos\Visual\Camera_Check_Component\bin\Debug\Output.txt"))
                {
                    sw.Write(string.Empty);
                    sw.WriteLine("" + _OKnum.ToString() + "");
                    sw.WriteLine("" + _NGnum.ToString() + "");
                }

                delete_image();

            }
        }


        #region /////////////////////////////////////////////////////////////////////////////////////////////////////////////LOGIN____LOGOUT
        private string UserID = "";
        bool load = false;

        private void login_btn_Click(object sender, EventArgs e)
        {
            if (load) return;
            load = true;

            System.Threading.Thread.Sleep(10);
            //picload_in.Visible = true;
            Login loginfrm = new Login();
            loginfrm.Show();

            loginfrm.FormClosed += (object sender1, FormClosedEventArgs e1) =>
            {
                UserID = loginfrm.ID_user;
                if (UserID != "")
                {
                    login(UserID);
                }
                load = false;

            };


        }
        private void permiss_1() // master
        {
            foreach (Control ctrl in General_tab.Controls)
            {
                if (ctrl.Name == "tabPage3")
                {
                    foreach (Control ctl in tabPage3.Controls)
                    {
                        if (ctl.Name == "login_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else
                        {
                            ctl.Enabled = true;
                        }
                    }
                }
                ctrl.Enabled = true;
            }
        }
        private void permiss_2() //operator
        {
            foreach (Control ctrl in General_tab.Controls)
            {
                if (ctrl.Name == "tabPage3")
                {
                    foreach (Control ctl in tabPage3.Controls)
                    {

                        if (ctl.Name == "Logout_btn")
                        {
                            ctl.Enabled = true;
                        }
                        else if (ctl.Name == "TB_idworker")
                        {
                            ctl.Enabled = true;
                        }
                        else if (ctl.Name == "TB_wker2")
                        {
                            ctl.Enabled = true;
                        }
                        else if (ctl.Name == "label8")
                        {
                            ctl.Enabled = true;
                        }
                        else if (ctl.Name == "label13")
                        {
                            ctl.Enabled = true;
                        }
                        else if (ctl.Name == "view_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else
                        {
                            ctl.Enabled = false;
                        }
                    }
                }
                
            }
        }
        private void permiss_3() // machanic
        {
            foreach (Control ctrl in General_tab.Controls)
            {
                if (ctrl.Name == "tabPage3")
                {
                    foreach (Control ctl in tabPage3.Controls)
                    {
                        if (ctl.Name == "delete_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else if (ctl.Name == "login_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else if (ctl.Name == "view_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else if (ctl.Name == "comboBox1")
                        {
                            ctl.Enabled = false;
                        }
                        else if (ctl.Name == "sign_up")
                        {
                            ctl.Enabled = false;
                        }
                        else
                        {
                            ctl.Enabled = true;
                        }
                    }
                }

                else
                {
                    ctrl.Enabled = true;
                }
            }
        }
        private void permiss_4() //production engineer
        {
            foreach (Control ctrl in General_tab.Controls)
            {
                if (ctrl.Name == "tabPage3")
                {
                    foreach (Control ctl in tabPage3.Controls)
                    {
                        if (ctl.Name == "login_btn")
                        {
                            ctl.Enabled = false;
                        }
                        else
                        {
                            ctl.Enabled = true;
                        }
                    }
                }
                ctrl.Enabled = true;
            }
        }
        private void login(string id)
        {
            string per = "";
            per = sql_action.getID_per_group(id);
            if (per == "1")
            {
                permiss_1();
            }
            if (per == "2")
            {
                permiss_2();
            }
            if (per == "3")
            {
                permiss_3();
            }
            if (per == "4")
            {
                permiss_4();
            }
        }

        private void Logout_btn_Click(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please Stop Program Fisrt");
                return;
            }
            else
            {
                var result = MessageBox.Show("Do you want to Log out?", "Confirm", MessageBoxButtons.OKCancel);
                if (timer.Enabled && result == DialogResult.OK)
                {
                    unable();
                    UserID = "";
                    // if (timer.Enabled) timer.Stop();
                    LB_TIMER.Text = "00:00:00";
                    start_check = false;
                    started = false;
                    RESET();
                    Start_btn.Enabled = true;
                    Stop_btn.Enabled = false;
                    Pic_Cam1.Image = null;
                    Pic_Cam2.Image = null;
                    Pic_Cam3.Image = null;
                    Pic_Cam4.Image = null;
                    Pic_Cam5.Image = null;
                    Pic_Cam6.Image = null;
                    status("[SYSTEM] Program STOPPED");
                    startPR_Count = 0;

                    Login loginfrm = new Login();
                    loginfrm.FormClosed += (object sender1, FormClosedEventArgs e1) =>
                    {
                        UserID = loginfrm.ID_user;
                        if (UserID != "")
                        {
                            login(UserID);
                        }
                        load = false;
                    };
                    loginfrm.Show();
                }
            }
        }
        private void sign_up_Click(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please stop the program first");
                return;
            }
            Sign_up sign_form = new Sign_up();
            sign_form.FormClosed += (object sender2, FormClosedEventArgs e2) =>
            {
                this.Show();
            };
            this.Hide();
            sign_form.Show();
        }

        #endregion
        private void view_btn_Click(object sender, EventArgs e)
        {
            dataGridView1.DataBindings.Clear();
            dataGridView2.DataBindings.Clear();

            DataTable dt = sql_action.result_tbl("component_status");
            dataGridView1.DataSource = dt;

            DataTable dt2 = sql_action.result_tbl("NG_Detail");
            dataGridView2.DataSource = dt2;
        }
        #region ////////////////////////////////////////////////////////////////////////////////////////////////////////////manual s7net PLC
        Plc PLCS7_1200 = null;
        ClassServoInput CLASS_IP = new ClassServoInput();
        ClassSerVoOutput CLASS_OP = new ClassSerVoOutput();
        bool PLC_con = false;
        private void btnConnectPLC_Click(object sender, EventArgs e)
        {
            PLCS7_1200 = new Plc(CpuType.S71200, "192.168.0.7", 0, 1);
          //  if (PLCS7_1200.IsAvailable)
         // {
                PLCS7_1200.Open();
                if (PLCS7_1200.IsConnected == true)
                {
                    MessageBox.Show("Connect to PLC Successful", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    PLC_con = true;
                    check_PLC_Con.Checked = true;
                }
                else
                {
                    PLC_con = false;
                    check_PLC_Con.Checked = false;
                    MessageBox.Show("Error");
                }

           // }
        }
       private void btnAutoHome_Click(object sender, EventArgs e)
        {

            var result = MessageBox.Show("Do you want to Auto Home", "Information", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                PLCS7_1200.Write("DB1.DBX0.4", 1);
                PLCS7_1200.Write("DB2.DBX0.4", 1);
                PLCS7_1200.Write("M10.1", 1);
                Thread.Sleep(1);
                PLCS7_1200.Write("DB1.DBX0.4", 0);
                PLCS7_1200.Write("DB2.DBX0.4", 0);
                PLCS7_1200.Write("M10.1", 0);
                check_Auto_home.Checked = true;
            }
            else
            {
                check_Auto_home.Checked = false;
            }


        }

        #endregion

        #region////////////////////////////////////////////////////////////////////////////////////////////// Recover 

        /*
         chu trình 1        1      2    3     4     5     6     7     8     9     10    11
        vi trí -------------1           2           3           4           5           6           7           8       Cam1    Cam2    Cam3    Cam4    Cam5    Cam6
        no1                Cam1         x           x           x           x           x           x           x        cp1
        no2                Cam1   cp1   x           x           x           x           x           x           x        cp2    
        no3                Cam1   cp2   Cam2        x           x           x           x           x           x        cp3    cp1
        no4                Cam1   cp3   Cam2  cp1   x           X           x           x           x           x        cp4    cp2
        no5                Cam1   cp4   Cam2  cp2   Cam3        x           x           x           x           x        cp5    cp3     cp1
        no6                Cam1   cp5   Cam2  cp3   Cam3       Cam4         x           x           x           x        cp6    cp4     cp2     cp1
        no7                x      cp6   Cam2  cp4   Cam3       Cam4         x           x           x           x               cp5     cp3     cp2
        no8                x            Cam2  cp5   Cam3       Cam4         x           x           x           x               cp6     cp4     cp3     
        no9                x            x     cp6   Cam3       Cam4        Cam5         x           x           x                       cp5     cp4     cp1
        no10               x            x           Cam3       Cam4        Cam5        Cam6         x           x                       cp6     cp5     cp2     cp1       grip(1)   
        bonus              x            x            x          x           x           x         grip          x                                                                     

        chu trình 2
        no13              Cam1          x           x           x           x           x           x           x       cp11                           
        no14              Cam1  cp11    x           x           x           x           x           x           x       cp21
        no15              Cam1  cp21   Cam2         x           x           x           x           x           x       cp31    cp11
        no16              Cam1  cp31   Cam2  cp11   x           x           x           x           x           x       cp41    cp21        
        no17              cam1  cp41   Cam2  cp21   Cam3        Cam4        Cam5        Cam6        x           x       cp51    cp31    cp11    cp6     cp3     cp2     grip(2)
        no18              Cam1  cp51   Cam2  cp31   Cam3        Cam4        Cam5        Cam6     grip()         x       cp61    cp41    cp21    cp11    cp4     cp3     grip(3)
        no19               x    cp61   Cam2  cp41   Cam3        Cam4        Cam5        Cam6     grip()         x               cp51    cp31    cp21    cp5     cp4     grip(4)
        no20               x           Cam2  cp51   Cam3        Cam4        Cam5        Cam6     grip()         x               cp61    cp41    cp31    cp6     cp5     grip(5)
        no21               x            x    cp61   Cam3        Cam4        Cam5        Cam6     grip()         x                       cp51    cp41    cp11    cp6     grip(6)
        no22               x            x           Cam3        Cam4        Cam5        Cam6     grip()         x                       cp61    cp51    cp21    cp11    grip(11) 
        bonus              x            x           x           x           x           x        grip()         x                                                                                                                                                                                                                       
        
        chu trình 3
        no23              Cam1          x           x           x           x           x           x           x       cp12
        no24              Cam1  cp12    x           x           x           x           x           x           x       cp22
        no25              Cam1  cp22    Cam2        x           x           x           x           x           x       cp32    cp12
        no26              Cam1  cp32    Cam2 cp12   x           x           x           x           x           x       cp42    cp22
        no27              Cam1  cp42    Cam2 cp22   Cam3        Cam4        Cam5        Cam6        grip()      x       cp52    cp32    cp12    cp61    cp31    cp21    grip(21)
        no28              Cam1  cp52    Cam2 cp31   Cam3        Cam4        Cam5        Cam6        grip()      x       cp62    cp42    cp22    cp12    cp41    cp31    grip(31)
        no29               x    cp62    Cam2 cp41   Cam3        Cam4        Cam5        Cam6        grip()      x               cp52    cp32    cp22    cp51    cp41    grip(41)
        no30               x            Cam2 cp51   Cam3        Cam4        Cam5        Cam6        grip()      x               cp62    cp42    cp32    cp61    cp51    grip(51)
        no31               x            x    cp61   cam3        Cam4        Cam5        Cam6        grip()      x                       cp52    cp42    cp12    cp61    grip(61)
        no21               x            x           Cam3        Cam4        Cam5        Cam6        grip()      x                       cp62    cp52    cp22    cp12    grip(12)
        */

        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write("M73.2", 1);

            null_pic1();
            MethodInvoker inv = delegate
            {
                waiting1.Text = "CHỜ MỘT CHÚT.....";
                waiting2.Text = "ĐẾN LƯỢT BẠN";
                button2.Enabled = false;
                button3.Enabled = true;
            }; this.Invoke(inv);
            wr1 = false;
            begin_ok1 = false;
            ready1 = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            PLCS7_1200.Write("M73.3", 1);
            null_pic2();
            MethodInvoker inv = delegate
            {
                waiting2.Text = "CHỜ MỘT CHÚT.....";
                waiting1.Text = "ĐẾN LƯỢT BẠN";
                button3.Enabled = false;
                button2.Enabled = true;
            }; this.Invoke(inv);
            wr2 = false;
            begin_ok2 = false;
            ready2 = false;
        }

        private void label28_Click(object sender, EventArgs e)
        {

        }

        private void pinf221_Click(object sender, EventArgs e)
        {

        }

        private void pinf421_Click(object sender, EventArgs e)
        {

        }

        private void c4_14_Click(object sender, EventArgs e)
        {

        }

        private void clearImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (started)
            {
                MessageBox.Show("Please stop program first!");
                return;
            }
            var result = MessageBox.Show("Do you want to Clear Image", "RESET", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                delete_image();
            }

        }
        string chon1 = string.Empty;
        string chon2 = string.Empty;
        string chon3 = string.Empty;
        private void btn_del1_Click(object sender, EventArgs e)
        {
            Int64 sel = dataGridView1.SelectedRows.Count;
            try
            {
                while (sel > 0)
                {
                    if (!dataGridView1.SelectedRows[0].IsNewRow)
                    {
                        chon1 = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                        chon1 = chon1.Replace(" ", string.Empty);
                        chon2 = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                        chon2 = chon2.Replace(" ", string.Empty);
                        chon3 = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                        chon3 = chon3.Replace(" ", string.Empty);
                        Boolean del = sql_action.excute_data("DELETE FROM component_status WHERE [Date] = '" + chon1 + "' AND [Time] = '" + chon2 + "' AND [Trace] = '" + chon3 + "'");
                        dataGridView1.Rows.RemoveAt(dataGridView1.SelectedRows[0].Index);
                        sel--;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        string chon4 = string.Empty;
        string chon5 = string.Empty;
        string chon6 = string.Empty;
        private void btn_del2_Click(object sender, EventArgs e)
        {
            Int64 sel = dataGridView2.SelectedRows.Count;
            try
            {
                while (sel > 0)
                {
                    if (!dataGridView2.SelectedRows[0].IsNewRow)
                    {
                        chon4 = dataGridView2.SelectedRows[0].Cells[1].Value.ToString();
                        chon4 = chon4.Replace(" ", string.Empty);
                        chon5 = dataGridView2.SelectedRows[0].Cells[2].Value.ToString();
                        chon5 = chon5.Replace(" ", string.Empty);
                        chon6 = dataGridView2.SelectedRows[0].Cells[3].Value.ToString();
                        chon6 = chon6.Replace(" ", string.Empty);
                        Boolean del = sql_action.excute_data("DELETE FROM NG_detail WHERE [Date] = '" + chon4 + "' AND [Time] = '" + chon5 + "' AND [Trace] = '" + chon6 + "'");
                        dataGridView2.Rows.RemoveAt(dataGridView2.SelectedRows[0].Index);
                        sel--;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        Class4 cl4 = new Class4();
        private void button5_Click(object sender, EventArgs e)
        {
            //var read = PLCS7_1200.Read(DataType.DataBlock, 117, 1024, VarType.String, 8);
             PLCS7_1200.ReadClass(cl4, 117, 1024);
            double read = cl4.Data_product;
            //ShowDataReadSV(textBox1, cl4.Data_product);
            status(read.ToString());
            //MethodInvoker inv = delegate
            //{
            //    textBox1.Text = read1;
            //};this.Invoke(inv);

        }

        private void RATIO_POWERON_CheckedChanged(object sender, EventArgs e)
        {
            if (RATIO_POWERON.Checked) 
            {
                btnAutoHome.Enabled = true;
            }
            else 
            {
                btnAutoHome.Enabled = false;
            }
            if (check_PLC_Con.Checked && RATIO_POWERON.Checked && !RESET_SYSTEM.Checked && check_Auto_home.Checked)
            {
                Gen_check_PLC.Checked = true;
            }
            else if (check_PLC_Con.Checked && !RATIO_POWERON.Checked && RESET_SYSTEM.Checked)
            {
                Gen_check_PLC.Checked = true;
            }
            else
            {
                Gen_check_PLC.Checked = false;
            }
        }

        private void TB_idworker_TextChanged(object sender, EventArgs e)
        {
            if (TB_idworker.Text != "")
            {
                check_ID1.Checked = true;

            }
            else 
            {
                check_ID1.Checked = false;
            }
        }

        private void TB_wker2_TextChanged(object sender, EventArgs e)
        {
            if (TB_wker2.Text != "")
            {
                check_ID2.Checked = true;

            }
            else
            {
                check_ID2.Checked = false;
            }
        }

        private void check_PLC_Con_CheckedChanged(object sender, EventArgs e)
        {
            if(check_PLC_Con.Checked && RATIO_POWERON.Checked && !RESET_SYSTEM.Checked && check_Auto_home.Checked) 
            {
                Gen_check_PLC.Checked = true;
            }
            else if(check_PLC_Con.Checked && !RATIO_POWERON.Checked && RESET_SYSTEM.Checked)
            {
                Gen_check_PLC.Checked = true;
            }
            else 
            {
                Gen_check_PLC.Checked = false;
            }
        }

        private void RESET_SYSTEM_CheckedChanged(object sender, EventArgs e)
        {
            if (RATIO_POWERON.Checked)
            {
                btnAutoHome.Enabled = true;
            }
            else
            {
                btnAutoHome.Enabled = false;
            }
            if (check_PLC_Con.Checked && RATIO_POWERON.Checked && !RESET_SYSTEM.Checked && check_Auto_home.Checked)
            {
                Gen_check_PLC.Checked = true;
            }
            else if (check_PLC_Con.Checked && !RATIO_POWERON.Checked && RESET_SYSTEM.Checked)
            {
                Gen_check_PLC.Checked = true;
            }
            else
            {
                Gen_check_PLC.Checked = false;
            }
        }

        private void check_Auto_home_CheckedChanged(object sender, EventArgs e)
        {
            if (check_PLC_Con.Checked && RATIO_POWERON.Checked && !RESET_SYSTEM.Checked && check_Auto_home.Checked)
            {
                Gen_check_PLC.Checked = true;
            }
            else if (check_PLC_Con.Checked && !RATIO_POWERON.Checked && RESET_SYSTEM.Checked)
            {
                Gen_check_PLC.Checked = true;
            }
            else
            {
                Gen_check_PLC.Checked = false;
            }
        }

        private void check_ID1_CheckedChanged(object sender, EventArgs e)
        {
            if (check_ID1.Checked && check_ID2.Checked && chek_PN.Checked) 
            {
                Gen_ID_check.Checked = true;
            }
            else Gen_ID_check.Checked = false;
        }

        private void check_ID2_CheckedChanged(object sender, EventArgs e)
        {
            if (check_ID1.Checked && check_ID2.Checked && chek_PN.Checked)
            {
                Gen_ID_check.Checked = true;
            }
            else Gen_ID_check.Checked = false;
        }

        private void chek_PN_CheckedChanged(object sender, EventArgs e)
        {
            if (check_ID1.Checked && check_ID2.Checked && chek_PN.Checked)
            {
                Gen_ID_check.Checked = true;
            }
            else Gen_ID_check.Checked = false;
        }

        private void Gen_check_PLC_CheckedChanged(object sender, EventArgs e)
        {
            if(Gen_check_Com.Checked&& Gen_check_PLC.Checked && Gen_ID_check.Checked) 
            {
                checkBox4.Checked = true;
            }
            else checkBox4.Checked = false;
        }

        private void Gen_check_Com_CheckedChanged(object sender, EventArgs e)
        {
            if (Gen_check_Com.Checked && Gen_check_PLC.Checked && Gen_ID_check.Checked)
            {
                checkBox4.Checked = true;
            }
            else checkBox4.Checked = false;
        }

        private void Gen_ID_check_CheckedChanged(object sender, EventArgs e)
        {
            if (Gen_check_Com.Checked && Gen_check_PLC.Checked && Gen_ID_check.Checked)
            {
                checkBox4.Checked = true;
            }
            else checkBox4.Checked = false;
        }
        private void Startlistener()
        {
            Thread listenerThread = new Thread(LSThread);
            listenerThread.IsBackground = true;
            listenerThread.Start();
        }
        private void UpdateStatus(string message)
        {
            if (InvokeRequired)
                Invoke((MethodInvoker)delegate { UpdateStatus(message); });
            else
            {
                //txttest.Text += message + Environment.NewLine;
                //txttest.SelectionStart = txttest.Text.Length;
                //txttest.ScrollToCaret();
            }
        }
        char[] qa = { '?', '\0', '\u0001' };
        private void LSThread()
        {
            TcpListener server = null;
            try
            {
                var port = 2000;
                var ipaddr = IPAddress.Parse("192.168.0.15");
                server = new TcpListener(ipaddr, port);
                server.Start(); 
                var bytes = new byte[256];
                while (true)
                {
                    var client = server.AcceptTcpClient();
                    MessageBox.Show("connect");
                    if (client.Connected)
                    {
                        NetworkStream stream = client.GetStream();
                        while (true)
                        {
                            if (stream.DataAvailable)
                            {
                                var i = stream.Read(bytes, 0, bytes.Length);
                                var data = Encoding.ASCII.GetString(bytes, 0, i).Trim(qa);
                                UpdateStatus(data);
                            }
                            else
                            {
                                if (!server.Pending()) continue;
                                break;
                            }                      
                        }
                        client.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                server.Stop();
            }
        }
        private void btntest_Click(object sender, EventArgs e)
        {
            Startlistener();
        }

        private void tb_PN_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {

        }
        private void load_control() 
        {
            if (General_tab.SelectedIndex == 2 && start_check && count_6 > 1)
            {
                stt++;
                allow_check = true;
                if (stt == 1)
                {
                    //DirectoryInfo d = new DirectoryInfo(system_config.Map_Path_File + @"\" + Parameter_app.IMAGE_FOLDER_PATH );
                    upload_image();
                    if (folderIndex == 0)
                    {
                        folderIndex++;
                        load2 = folderIndex;
                    }
                    update_image2();
                    up_hinh = true;

                }

                if (stt > 1)
                {
                    if (start_check && count_6 > 1 && !up_hinh)
                    {
                        upload_image();
                        if (folderIndex == 0)
                        {
                            folderIndex++;
                            load2 = folderIndex;
                        }
                        update_image2();
                        up_hinh = true;
                    }
                    if (run_out1 && folderIndex < count_6)
                    {
                        upload_image();
                        run_out1 = false;
                    }
                    if (run_out2 && folderIndex < count_6)
                    {
                        update_image2();
                        run_out2 = false;
                    }
                }
            }
            else allow_check = false;
            
        }
        bool auto_man = true;
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                auto_man = true;
            }
            else 
            {
                auto_man = false;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                auto_man = false;
            }
            else 
            {
                auto_man = true;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void checkBox1_CheckedChanged_1(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                allow_check = true;
            }
            else { allow_check = false; }
        }
    }
    }


