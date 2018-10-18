using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers
{
    public class GetXapFileData
    {

        public  XapFileInfo GetXapFile(string fileName , bool isStaging)
        {

            XapFileInfo xapFileInfo = null;
            string strConnString = ConfigurationManager.ConnectionStrings["Globalstr"].ConnectionString;

            using (SqlConnection cn = new SqlConnection(strConnString))
            {
                SqlParameter pfileName = new SqlParameter("@pFileName", SqlDbType.VarChar, 60);
                SqlParameter pfileData = new SqlParameter("@pFileData", SqlDbType.Binary);
                SqlParameter pisStaging = new SqlParameter("@pIsStaging", SqlDbType.Bit);


                pfileName.Direction = ParameterDirection.Input;
                pisStaging.Direction = ParameterDirection.Input; 


                pfileData.Direction = ParameterDirection.Output;

         

                pfileName.Value = fileName;
                pisStaging.Value = isStaging;


                SqlCommand cmd = new SqlCommand("usp_GetXapFileData", cn);
                cmd.CommandType = CommandType.StoredProcedure;


                cmd.Parameters.Add(pfileName);
                cmd.Parameters.Add(pfileData);
                cmd.Parameters.Add(pisStaging);
                cn.Open();
                cmd.ExecuteNonQuery();
                cn.Close();


              //  bool istaging = (bool)cmd.Parameters["@pIsStaging"].Value;
                byte[] filedata = (byte[])cmd.Parameters["@pFileData"].Value;

                xapFileInfo = new XapFileInfo()
                {
                    FileName = fileName,
                    FileData = filedata,
                    IsStaging = isStaging,
                };

            }

            return xapFileInfo;
        }


    }
}