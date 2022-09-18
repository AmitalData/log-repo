using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Messaging.Customs;
using Logitude.Customs.BL.TraceEvents;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Def.EntityQueryServicesExt;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.MessagingServices;
using Logitude.Server.Tools.Contracts;
using Logitude.Server.Tools.Helpers;
using Logitude.ServerHealthService.Models;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using ClosedXML.Excel;
namespace Logitude.Customs.CustomsMessaging.Tasks
{
    public class SendReportExel : ICustomsSendReportExel
    {
        public void StartRun(string taskId, int seedDefaultTenant)
        {

            var customsSettingQueryService = new CustomsSettingQueryService(seedDefaultTenant);
            var allCustomsSetting = customsSettingQueryService.GetAll();
            allCustomsSetting.ForEach(t => RunPerTenant(t));




        }
        private void RunPerTenant(CustomsSettingPM t)
        {
            getData(t.Tenant);

            string subject = " דוחות " + DateTime.Now.ToString();
            string body = "מייל זה נשלח אוטומטי נא לא להשיב למייל זה";
            SendEmails(subject, body);

        }

        public void getData(int tenent)
        {
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(tenent);
            List<Declaration> Declarations = declarationQueryService.GetDeclaration100(tenent);
            DataSet ds = ToDataSet(Declarations);
            ExportDataSetToExcel(ds);
        }





        public DataSet ToDataSet(List<Declaration> list)
        {
            Type elementType = typeof(Declaration);
            DataSet ds = new DataSet();
            DataTable t = new DataTable();
            ds.Tables.Add(t);

            //add a column to table for each public property on T
            foreach (var propInfo in elementType.GetProperties())
            {
                Type ColType = Nullable.GetUnderlyingType(propInfo.PropertyType) ?? propInfo.PropertyType;

                t.Columns.Add(propInfo.Name, ColType);
            }

            //go through each property on T and add each value to the table
            foreach (Declaration item in list)
            {
                DataRow row = t.NewRow();

                foreach (var propInfo in elementType.GetProperties())
                {
                    row[propInfo.Name] = propInfo.GetValue(item, null) ?? DBNull.Value;
                }

                t.Rows.Add(row);
            }

            return ds;
        }
        private void ExportDataSetToExcel(DataSet ds)
        {
            string AppLocation = "";
            AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
            AppLocation = AppLocation.Replace("file:\\", "");
            string file = AppLocation + "\\ExcelFiles\\DataFile.xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                wb.Worksheets.Add(ds.Tables[0]);
                wb.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                wb.Style.Font.Bold = true;
                wb.SaveAs(file);
            }
        }

        protected void SendEmails(string subject, string body)
        {
            try
            {
                string AppLocation = "";
                AppLocation = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().CodeBase);
                AppLocation = AppLocation.Replace("file:\\", "");
                string file = AppLocation + "\\ExcelFiles\\DataFile.xlsx";
                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("amitarltest123@gmail.com");
                MailMessage mailMessage = new MailMessage();

                mailMessage.From = new MailAddress("amitarltest123@gmail.com");
                //foreach (var emailAddress in Settings.EmailSettings.To.Addresses.Split(',').ToList())
                //{
                //    mailMessage.To.Add(emailAddress);
                //}
                mailMessage.To.Add("sh254256@gmail.com");
                System.Net.Mail.Attachment attachment;
                attachment = new System.Net.Mail.Attachment(file); //Attaching File to Mail  
                mailMessage.Attachments.Add(attachment);
                mailMessage.BodyEncoding = Encoding.UTF8;
                mailMessage.Subject = subject;
                mailMessage.Body = body;
                smtpClient.Port = 587;
                smtpClient.Credentials = new NetworkCredential("amitarltest123@gmail.com", "amital123");
                smtpClient.Send(mailMessage);


            }
            catch (Exception exception)
            {

            }
        }



        //public void getData1()
        //{
        //    DeclarationQueryService declarationQueryService = new DeclarationQueryService(0);
        //    List<Declaration> Declarations = declarationQueryService.GetDeclaration100(1);

        //    DataSet ds = null;
        //    using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["Local"].ConnectionString))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("GetSalesDetails", con);
        //            cmd.CommandType = CommandType.StoredProcedure;
        //            SqlDataAdapter da = new SqlDataAdapter();
        //            da.SelectCommand = cmd;
        //            ds = new DataSet();
        //            da.Fill(ds);

        //            ExportDataSetToExcel(ds);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //        finally
        //        {
        //            ds.Dispose();
        //        }
        //    }
        //}




    }
}
