using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.IO;
using WebFreight.Web.WebServices;

namespace Logitude.DeploymentUtilities.Models
{
    public class DeploymentUtilitiesTool
    {
        public void RunTool()
        {
            if (ToolArguments.IsArgumentProvided(Arguments.ROLES))
            {
                UploadRoles();
            }

            if (ToolArguments.IsArgumentProvided(Arguments.FEATURES))
            {
                UploadFeatures();
            }

            if (ToolArguments.IsArgumentProvided(Arguments.HTMLVERSION))
            {
                UpdateHtmlVersion();
            }
        }
        
        protected void UploadRoles()
        {
            Console.Write("\n");

            string rolesCSVFilePath = ToolArguments.GetArgumentValue(Arguments.ROLES);
            byte[] rolesData = GetByteArrayFromCSVFile(rolesCSVFilePath);

            if(rolesData == null)
            {
                ExitTool("Error: Cannot Get Byte Array From CSV File " + rolesCSVFilePath);
            }

            try
            {
                Console.WriteLine("Uploading Roles From File " + rolesCSVFilePath + " ...");
                ExcelExportService excelExportService = new ExcelExportService();
                string result = excelExportService.ImportRoleFeatures(rolesData);
                if (String.IsNullOrEmpty(result))
                {
                    Console.WriteLine("Roles Was Uploaded Successfully");
                }
                else
                {
                    Console.WriteLine("Uploading Roles Finished With Errors:\n" + result);
                }
            }
            catch(Exception exception)
            {
                ExitTool("Error: " + exception.ToString());
            }
        }

        protected void UploadFeatures()
        {
            Console.Write("\n");

            string featuresCSVFilePath = ToolArguments.GetArgumentValue(Arguments.FEATURES);
            byte[] featuresData = GetByteArrayFromCSVFile(featuresCSVFilePath);

            if (featuresData == null)
            {
                ExitTool("Error: Cannot Get Byte Array From CSV File " + featuresCSVFilePath);
            }

            try
            {
                Console.WriteLine("Uploading Features From File " + featuresCSVFilePath + " ...");
                ExcelExportService excelExportService = new ExcelExportService();
                string result = excelExportService.ImportFeaturePackages(featuresData);
                if (String.IsNullOrEmpty(result))
                {
                    Console.WriteLine("Features Was Uploaded Successfully");
                }
                else
                {
                    Console.WriteLine("Uploading Features Finished With Errors:\n" + result);
                }
            }
            catch (Exception exception)
            {
                ExitTool("Error: " + exception.ToString());
            }
        }

        protected void UpdateHtmlVersion()
        {
            Console.Write("\n");

            string htmlVersion = ToolArguments.GetArgumentValue(Arguments.HTMLVERSION);

            if (String.IsNullOrEmpty(htmlVersion))
            {
                ExitTool("Error: Cannot Find The New HTML Version");
            }

            try
            {
                Console.WriteLine("Updating HTML Version To " + htmlVersion + " ...");
                SettingRepository settingRepository = new SettingRepository();
                Setting setting = settingRepository.GetSingleSetting("1");
                setting.HtmlVersion = htmlVersion;
                settingRepository.SubmitChanges();
                Console.WriteLine("HTML Version Was Updated Successfully");
            }
            catch (Exception exception)
            {
                ExitTool("Error: " + exception.ToString());
            }
        }

        protected byte[] GetByteArrayFromCSVFile(string csvFilePath)
        {
            if (!IsFileExists(csvFilePath))
            {
                ExitTool("Error: Cannot Find File " + csvFilePath);
            }

            if (!IsCSVFile(csvFilePath))
            {
                ExitTool("Error: Invalid CSV File " + csvFilePath);
            }

            FileStream fileStream = null;
            BinaryReader binaryReader = null;

            try
            {
                fileStream = new FileStream(csvFilePath, FileMode.Open, FileAccess.Read);
                binaryReader = new BinaryReader(fileStream);
                long bytes = new FileInfo(csvFilePath).Length;
                byte[] byteArray = binaryReader.ReadBytes((int)bytes);
                fileStream.Close();
                binaryReader.Close();
                return byteArray;
            }
            catch(Exception)
            {
                if(fileStream != null)
                {
                    fileStream.Close();
                }
                if(binaryReader != null)
                {
                    binaryReader.Close();
                }
                return null;
            }
        }

        protected bool IsFileExists(string filePath)
        {
            if (File.Exists(filePath))
            {
                return true;
            }
            return false;
        }

        protected bool IsCSVFile(string filePath)
        {
            string extension = Path.GetExtension(filePath);
            if(extension?.ToLower() == ".csv")
            {
                return true;
            }
            return false;
        }

        protected void ExitTool(string message)
        {
            Console.WriteLine(message);
            Environment.Exit(1);
        }
    }
}