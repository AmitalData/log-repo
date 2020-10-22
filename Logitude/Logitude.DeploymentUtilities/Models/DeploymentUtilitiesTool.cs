using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using WebFreight.Web.WebServices;

namespace Logitude.DeploymentUtilities.Models
{
    public class DeploymentUtilitiesTool
    {
        public void RunTool()
        {
            List<string> mainArguments = new List<string>() { Arguments.IMPORT, Arguments.EXPORT, Arguments.HTMLVERSION };
            if (mainArguments.Where(a => ToolArguments.IsArgumentProvided(a)).Count() > 1)
            {
                ExitTool("Error: Cannot Use More Than One Of Main Arguments: Import, Export, And HTMLVersion");
            }

            bool isImportArgumentProvided = ToolArguments.IsArgumentProvided(Arguments.IMPORT);
            bool isExportArgumentProvided = ToolArguments.IsArgumentProvided(Arguments.EXPORT);
            bool isHtmlVersionArgumentProvided = ToolArguments.IsArgumentProvided(Arguments.HTMLVERSION);
            bool isRolesArgumentProvided = ToolArguments.IsArgumentProvided(Arguments.ROLES);
            bool isPackagesArgumentProvided = ToolArguments.IsArgumentProvided(Arguments.PACKAGES);

            if (isImportArgumentProvided)
            {
                if (isRolesArgumentProvided)
                {
                    ImportRoles();
                }

                if (isPackagesArgumentProvided)
                {
                    ImportPackages();
                }
            }
            else if (isExportArgumentProvided)
            {
                if (isRolesArgumentProvided)
                {
                    ExportRoles();
                }

                if (isPackagesArgumentProvided)
                {
                    ExportPackages();
                }
            }
            else if (isHtmlVersionArgumentProvided)
            {
                UpdateHtmlVersion();
            }
            else
            {
                Console.WriteLine("Cannot Find One Of Main Arguments: Import, Export, And HTMLVersion");
            }
        }
        
        protected void ImportRoles()
        {
            try
            {
                Console.Write("\n");

                string rolesCSVFilePath = ToolArguments.GetArgumentValue(Arguments.ROLES);
                byte[] rolesData = GetByteArrayFromCSVFile(rolesCSVFilePath);

                if (rolesData == null)
                {
                    ExitTool("Error: Cannot Get Byte Array From CSV File " + rolesCSVFilePath);
                }

                Console.WriteLine("Importing Roles From File " + rolesCSVFilePath + " ...");
                ExcelExportService excelExportService = new ExcelExportService();
                string result = excelExportService.ImportRoleFeatures(rolesData);
                if (String.IsNullOrEmpty(result))
                {
                    Console.WriteLine("Roles Was Imported Successfully");
                }
                else
                {
                    Console.WriteLine("Importing Roles Finished With Errors:\n" + result);
                }
            }
            catch(Exception exception)
            {
                ExitTool("Error: " + exception.ToString());
            }
        }

        protected void ExportRoles()
        {
            try
            {
                Console.Write("\n");

                string rolesCSVFilePath = ToolArguments.GetArgumentValue(Arguments.ROLES);
                string rolesCSVDirectoryPath = Path.GetDirectoryName(rolesCSVFilePath);

                if (!IsDirectoryExists(rolesCSVDirectoryPath))
                {
                    ExitTool("Error: Cannot Find Path " + rolesCSVDirectoryPath);
                }

                if (!IsCSVFile(rolesCSVFilePath))
                {
                    ExitTool("Error: Invalid CSV File " + rolesCSVFilePath);
                }

                Console.WriteLine("Exporting Roles To CSV File " + rolesCSVFilePath + " ...");
                string sourceDBConnectionString = ConfigurationManager.ConnectionStrings["SourceDatabaseStr"].ToString();
                ExcelExportService excelExportService = new ExcelExportService();
                byte[] byteArray = excelExportService.ExportRoleFeaturesFromSourceDB(sourceDBConnectionString);
                string result = ConvertByteArrayToCSVFile(byteArray, rolesCSVFilePath);
                if(result == null)
                {
                    Console.WriteLine("Roles Was Exported Successfully");
                }
                else
                {
                    Console.WriteLine("Error: Cannot Convert Byte Array To CSV File");
                }
            }
            catch (Exception exception)
            {
                ExitTool("Error: " + exception.ToString());
            }
        }

        protected void ImportPackages()
        {
            try
            {
                Console.Write("\n");

                string packagesCSVFilePath = ToolArguments.GetArgumentValue(Arguments.PACKAGES);
                byte[] packagesData = GetByteArrayFromCSVFile(packagesCSVFilePath);

                if (packagesData == null)
                {
                    ExitTool("Error: Cannot Get Byte Array From CSV File " + packagesCSVFilePath);
                }

                Console.WriteLine("Importing Packages From File " + packagesCSVFilePath + " ...");
                ExcelExportService excelExportService = new ExcelExportService();
                string result = excelExportService.ImportFeaturePackages(packagesData);
                if (String.IsNullOrEmpty(result))
                {
                    Console.WriteLine("Packages Was Imported Successfully");
                }
                else
                {
                    Console.WriteLine("Importing Packages Finished With Errors:\n" + result);
                }
            }
            catch (Exception exception)
            {
                ExitTool("Error: " + exception.ToString());
            }
        }

        protected void ExportPackages()
        {
            try
            {
                Console.Write("\n");

                string packagesCSVFilePath = ToolArguments.GetArgumentValue(Arguments.PACKAGES);
                string packagesCSVDirectoryPath = Path.GetDirectoryName(packagesCSVFilePath);

                if (!IsDirectoryExists(packagesCSVDirectoryPath))
                {
                    ExitTool("Error: Cannot Find Path " + packagesCSVDirectoryPath);
                }

                if (!IsCSVFile(packagesCSVFilePath))
                {
                    ExitTool("Error: Invalid CSV File " + packagesCSVFilePath);
                }

                Console.WriteLine("Exporting Packages To CSV File " + packagesCSVFilePath + " ...");
                string sourceDBConnectionString = ConfigurationManager.ConnectionStrings["SourceDatabaseStr"].ToString();
                ExcelExportService excelExportService = new ExcelExportService();
                byte[] byteArray = excelExportService.ExportPackagesFeaturesFromSourceDB(sourceDBConnectionString);
                string result = ConvertByteArrayToCSVFile(byteArray, packagesCSVFilePath);
                if (result == null)
                {
                    Console.WriteLine("Packages Was Exported Successfully");
                }
                else
                {
                    Console.WriteLine("Error: Cannot Convert Byte Array To CSV File");
                }
            }
            catch (Exception exception)
            {
                ExitTool("Error: " + exception.ToString());
            }
        }

        protected void UpdateHtmlVersion()
        {
            try
            {
                Console.Write("\n");

                string htmlVersion = ToolArguments.GetArgumentValue(Arguments.HTMLVERSION);

                if (String.IsNullOrEmpty(htmlVersion))
                {
                    ExitTool("Error: Cannot Find The New HTML Version");
                }

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

        protected string ConvertByteArrayToCSVFile(byte[] byteArray, string csvFilePath)
        {
            try
            {
                FileStream fileStream = new FileStream(csvFilePath, FileMode.Create, FileAccess.Write);
                fileStream.Write(byteArray, 0, byteArray.Length);
                fileStream.Close();
                return null;
            }
            catch (Exception exception)
            {
                return exception.ToString();
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

        protected bool IsDirectoryExists(string directoryPath)
        {
            if (Directory.Exists(directoryPath))
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