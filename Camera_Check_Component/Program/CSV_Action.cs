using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Camera_Check_Component
{
    public class CSV_Action
    {
        private static string NameFile = "VisualData.csv";
        private string path;
        public CSV_Action(string path)
        {
            this.path = path;
            try
            {
                if (!File.Exists(path + @"\" + NameFile))
                {
                    File.Create(path + @"\" + NameFile).Close();
                    using (StreamWriter sw = new StreamWriter(path + @"/" + NameFile))
                    {
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}", "Month", "Date", "Time", "ID Bath", "Result","NG Image Number"));
                    }
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
        }
        public void Add_Data(ObjectCSVData data)
        {
            try
            {

                using (StreamWriter sw = File.AppendText(path + @"/" + NameFile))
                {
                    sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5}",data.Month,data.Date,data.Time,data.ID_Bath,data.Result,data.Number_NG));
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);

            }
        }

    }
    public class ObjectCSVData
    {
        public ObjectCSVData()
        {

        }
        public string Month { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
        public int ID_Bath { get; set; }
        public int Result { get; set; }
        public string Number_NG { get; set; }

    }
}
