using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using TimeTableManagementSystem.Models.TTMS;
using System.Data.SqlClient;
using System.Configuration;

namespace TimeTableManagementSystem.Controllers.BosinessLayer
{
    public class BLTTMS
    {

        public SqlConnection MyConn = null;
        public string StringConnection = "";

        private void OpenConnection()
        {
            if (MyConn == null)
            {
                StringConnection = @"server=" + ConfigurationSettings.AppSettings["server"].ToString()
                    + "; database=" + ConfigurationSettings.AppSettings["database"].ToString()
                    + "; uid=" + ConfigurationSettings.AppSettings["username"].ToString()
                    + "; password=" + "cyborg@^$camp%!" + "; Connect timeout=0";

                MyConn = new SqlConnection(Convert.ToString(StringConnection));
            }
            if (MyConn.State == ConnectionState.Closed || MyConn.State == ConnectionState.Broken)
            {
                MyConn.Open();
            }
        }

        private SqlCommand CreateCmd(string procName, params object[] ps)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = procName;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Connection = MyConn;
            SqlParameter[] sqlpa = null;
            if (ps != null)
            {
                SqlCommandBuilder.DeriveParameters(cmd);
                cmd.Parameters.RemoveAt(0);
                sqlpa = new SqlParameter[cmd.Parameters.Count];
                cmd.Parameters.CopyTo(sqlpa, 0);
                for (int i = 0; i < sqlpa.Length; ++i)
                {
                    sqlpa[i].Value = ps[i];
                }
            }
            return cmd;

        }

        public DataTable ExecuteDataSetBySelect(string procName, params object[] ps)
        {
            OpenConnection();
            using (SqlCommand cmd = CreateCmd(procName, ps))
            {
                using (SqlDataAdapter sqlad = new SqlDataAdapter())
                {
                    using (DataSet ds = new DataSet())
                    {
                        try
                        {
                            sqlad.SelectCommand = cmd;
                            sqlad.SelectCommand.CommandTimeout = 0;
                            sqlad.Fill(ds);
                            MyConn.Close();
                        }
                        catch (Exception ex)
                        {
                            MyConn.Close();
                            return new DataTable();
                        }
                        return ds.Tables[0];
                    }
                }
            }
        }

        public DataTable SubmitTotalHoursForTimeTable(TTMSModels TTMS)
        {
            try
            {
                return ExecuteDataSetBySelect("SP_SubmitTotalHoursForTimeTable", TTMS.WorkingDayPerWeek, TTMS.SubjectPerday, TTMS.TotalSubjects);
            }
            catch (Exception Ex)
            {
                return new DataTable();
            }
        }

        public DataTable GenerateTimeTable(TTMSModels TTMS)
        {
            try
            {
                string[] AllSubjectsNames = TTMS.AllSubjectsName.Split(',');
                string[] AllSubjectsHours = TTMS.AllSubjectsHour.Split(',');
                DataTable DT = new DataTable();
                for (int i=0; i< AllSubjectsNames.Length; i++)
                {
                    DT = ExecuteDataSetBySelect("SP_GenerateTimeTable", Convert.ToString(AllSubjectsNames[i]), Convert.ToInt32(AllSubjectsHours[i]));
                }

                DataTable TruncateTable = ExecuteDataSetBySelect("SP_TruncateTimeTable", 0);

                return DT;
            }
            catch (Exception Ex)
            {
                return new DataTable();
            }
        }
    }
}