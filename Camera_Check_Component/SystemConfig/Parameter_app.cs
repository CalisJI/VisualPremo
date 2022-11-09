using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Camera_Check_Component
{
    public class Parameter_app
    {
        
        public static string App_Folder = Application.StartupPath;
        
        public static string System_Config_File_Name = "SystemConfig.xml";
        public static string Code_File_Name = "Console.txt";
        public static string Console_Copy = "Console_Copy.txt";
        public static string Output_File_Name = "Output.txt";
        public static string TEMP_IMAGE_FOLDER_NAME ;
        public static string TEMP_IMAGE_FOLDER_PATH;
        public static string IMAGE_FOLDER_NAME = "IMAGE";
        public static string IMAGE_FOLDER_NAME_1 = "IMAGE_SHARE";
        public static string TXT_FOLDER_NAME = "TEXT_SHARE";
        public static string OK_IMAGE_TEMP_NAME;
        public static string ERROR_IMAGE_TEMP_NAME;
        public static void  TEMP(int day,int month,int year,string increase) 
        {
            //TEMP_IMAGE_FOLDER_NAME = "TEMP_IMG_1";
             TEMP_IMAGE_FOLDER_NAME = "TEMP_IMG-"+day.ToString()+"-"+month.ToString()+"-"+year.ToString()+"-"+increase+"";
             TEMP_IMAGE_FOLDER_PATH = App_Folder + "/" + IMAGE_FOLDER_NAME + "/" + TEMP_IMAGE_FOLDER_NAME;
        }
        public static void OK_TEMP(string date, string increase) 
        {
            OK_IMAGE_TEMP_NAME = "OK_IMG_" + date + "_" + increase + "";
            OK_IMAGE_FOLDER_PATH =  OK_IMAGE_FOLDER_NAME + "/" + OK_IMAGE_TEMP_NAME;
            
        }
        //public static void ERROR_TEMP(string date ,string increase)
        //{
        //    ERROR_IMAGE_TEMP_NAME = "NG_IMG_" + date + "_" + increase + "";
        //    ERROR_IMAGE_FOLDER_PATH =  ERROR_IMAGE_FOLDER_NAME + "/" + ERROR_IMAGE_TEMP_NAME;

        //}
        public static void ERROR_TEMP(string date)
        {
            ERROR_IMAGE_TEMP_NAME = "NG_IMG_" + date;
            ERROR_IMAGE_FOLDER_PATH = ERROR_IMAGE_FOLDER_NAME + "/" + ERROR_IMAGE_TEMP_NAME;

        }
        public static string OK_IMAGE_FOLDER_NAME = "OK_IMAGE";
        public static string ERROR_IMAGE_FOLDER_NAME = "NG_IMAGE";

        public static string System_Config_File_Path = App_Folder + "/" + System_Config_File_Name;
        public static string IMAGE_FOLDER_PATH =  IMAGE_FOLDER_NAME;
        public static string OK_IMAGE_FOLDER_PATH;
        public static string ERROR_IMAGE_FOLDER_PATH;
        
    }
}
