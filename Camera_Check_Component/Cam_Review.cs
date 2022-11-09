using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Video;
using AForge.Video.DirectShow;
using AForge.Imaging;
using Emgu.CV.UI;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.Util;
using System.Drawing.Drawing2D;

namespace Camera_Check_Component
{
    public partial class Cam_Review : Form
    {
        FilterInfoCollection filterinfocollection = new FilterInfoCollection(FilterCategory.VideoInputDevice);
        VideoCaptureDevice videoCaptureDevice = new VideoCaptureDevice();
        private int Cam_Index;
        private string Cam_name;
        private int pixel;
      
      
        public Cam_Review(int Cam_Index, string Cam_name, int pixel)
        {
            InitializeComponent();
            this.Cam_Index = Cam_Index;
            this.Cam_name = Cam_name;
            this.pixel = pixel;
        }
        private void Cam_Review_Load(object sender, EventArgs e)
        {
            MaximizeBox = false;
            ShowIcon = false;
            MinimizeBox = false;
            int w = pictureBox1.Width;
            
            label1.Text = Cam_name +" : "+ filterinfocollection[this.Cam_Index].Name;
            if(filterinfocollection.Count >0 && this.Cam_Index < filterinfocollection.Count)
            {
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                videoCaptureDevice = new VideoCaptureDevice(filterinfocollection[this.Cam_Index].MonikerString);
                if (pixel < 0) 
                {
                    MessageBox.Show("Please select your resolution first");                    
                    return;
                }
                //this.Close();
                videoCaptureDevice.VideoResolution = videoCaptureDevice.VideoCapabilities[pixel];
                videoCaptureDevice.NewFrame += HandleCaptureDeviceStreamNewFrame;
                videoCaptureDevice.Start();
            }
            else
            {
                MessageBox.Show("Camera can not found");
            }
        }
        private static System.Drawing.Image RotateImage(Bitmap img, float rotationAngle)
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
        private void HandleCaptureDeviceStreamNewFrame(object sender, NewFrameEventArgs eventArgs)
        {


          

            if (pictureBox1.Image != null)
            {
                pictureBox1.Image.Dispose();
            }
            Bitmap bitmap = eventArgs.Frame.Clone() as Bitmap;
            pictureBox1.Image = bitmap;

        }
        private void Cam_Review_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (videoCaptureDevice.IsRunning) 
            {
                videoCaptureDevice.SignalToStop(); 
            }
        }
        
        private void Take_photo_btn_Click(object sender, EventArgs e)
        {
            if (pictureBox2.Image != null) { pictureBox2.Image.Dispose(); }
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            Bitmap bmp = (Bitmap)RotateImage((Bitmap)pictureBox1.Image.Clone(), 45);
            pictureBox2.Image = bmp;
           // bmp.Dispose();
        }
    }
}
