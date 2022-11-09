using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using System.Data.Sql;
using System.Data.SqlTypes;
using System.Configuration;


namespace Camera_Check_Component
{
    
    class SQL_action
    {
        private System_config system_config;
        public string Error_Mes;
        SqlConnection conn = new SqlConnection();
        public static DataTable GetSQL_SeverList() 
        {
            //DataTable serversTable = SqlDataSourceEnumerator.Instance.GetDataSources();
            DataTable serversTable = SqlDataSourceEnumerator.Instance.GetDataSources();
            return serversTable;
        }
       
        public static DataTable GetSQL_DatabaseList(string servername) 
        {
            SqlConnectionStringBuilder connection = new SqlConnectionStringBuilder();
            connection.DataSource = servername;
            //connection.UserID = UserID;
            //connection.Password = Password;
            connection.IntegratedSecurity = true;
            
            string Con = connection.ToString();          
            SqlConnection StrCon = new SqlConnection(Con);
            StrCon.Open();
            DataTable dt = StrCon.GetSchema("Databases");
            StrCon.Close();
            return dt;
        }
       
        public String GetSource() 
        {
            system_config = Program_Configuration.GetSystem_Config();

          string source = "Data Source= " + system_config.SQL_server + ";Initial Catalog=" + system_config.Database + ";Integrated Security=True";
            return source;
        }
        private void connec_SQL() 
        {
            try
            {
                conn = new SqlConnection(GetSource());
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();
                }
                Error_Mes = string.Empty;
            }
            catch (Exception e)
            {
                Error_Mes = e.Message;
            }
            
        }
        private void close_SQL() 
        {
            if(conn.State == ConnectionState.Open) 
            {
                conn.Close();
            }
        }
        public void OP() 
        {
            connec_SQL();
        }
        public void CL()
        {
            close_SQL();
        }
        public Boolean excute_data(string cmd)
        {
            connec_SQL();
            bool check = false;
            try
            {              
                SqlCommand cmds = new SqlCommand(cmd, conn);
                cmds.ExecuteNonQuery();
                check = true;
                Error_Mes = string.Empty;
            }
            catch (Exception e)
            {
                check = false;
                Error_Mes = e.Message;
            }
            close_SQL();
            return check;
        }
        public  DataTable result_tbl(string tbl)
        {
            DataTable dt = new DataTable();
            try
            {
                connec_SQL();
                SqlCommand cmd = new SqlCommand("SELECT * FROM " + tbl + "", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
                close_SQL();
                Error_Mes = string.Empty;
            }
            catch (Exception e)
            {
                Error_Mes = e.Message;
            }
            return dt;
        }
        public  string getID_user(string userid,string pass)   
        {
            string ID = "";
            connec_SQL();
            try 
            {
               
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_user_ID WHERE [user] = '" + userid + "' AND pass = '"+ pass +"'", conn);              
                SqlDataAdapter da = new SqlDataAdapter(cmd);               
                DataTable dt = new DataTable();
                da.Fill(dt);
                if(dt!= null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        ID = dr["ID_user"].ToString();
                    }
                }
            }
            catch (Exception e)
            {
                Error_Mes = e.Message;
            }
            // StrCon.Close();
            close_SQL();

            return ID;
        }
        public string getID_per_group(string id_per) 
        {
            string per_group = "";
            connec_SQL();
            try
            {
             
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_per_rel WHERE ID_user_rel = '" + id_per + "'", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt != null)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        per_group = dr["ID_per_rel"].ToString();
                    }
                }
            }
            catch
            {

            }
           
            close_SQL();
            return per_group;
        }
        public string get_exist_account(string userid) 
        {
            string user = "";
            connec_SQL();
            try
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_user_ID WHERE [user] = '" + userid + "'", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (da != null) 
                {
                    foreach(DataRow dr in dt.Rows) 
                    {
                        user = dr["user"].ToString();
                    }
                }
            }
            catch (Exception)
            {
            
            }
            close_SQL();
            return user;
        }
        public List<string> RuleList(string per_group) 
        {
            List<string> ruleList = new List<string>();
            connec_SQL();
            try
            {
                
                SqlCommand cmd = new SqlCommand("SELECT * FROM tbl_detail_per WHERE id_per='" + per_group + "'", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt != null) 
                {
                    foreach(DataRow dr in dt.Rows) 
                    {
                        ruleList.Add(dr["rule_action"].ToString());
                    }
                }
            }
            catch (Exception)
            {

                
            }
            close_SQL();
            return ruleList;
        }
    }
}
