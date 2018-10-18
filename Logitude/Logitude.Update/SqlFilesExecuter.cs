using Simplog.Data.CommonDataModel;
using Simplog.Global.Data.GlobalModel;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Logitude.Update
{
    public class SqlFilesExecuter
    {
        static string connectionString;
        private static string ConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(connectionString))
                {
                    CommonDataContext Context = CommonDataContext.GetContextByDBId("0");
                    connectionString = Context.GetConnection().ConnectionString;
                }

                return connectionString;
            }
        }

        static string globalConnectionString;
        private static string GlobalConnectionString
        {
            get
            {
                if (string.IsNullOrEmpty(globalConnectionString))
                {
                    GlobalContext Context = new GlobalContext();
                    globalConnectionString = Context.GetCurrentConnection();
                }

                return globalConnectionString;
            }
        }

        static string successFiles = "";
        static string failedFiles = "";
        static List<string> totalFiles = new List<string>();

        public static void ExecuteAllSqlFiles(string rootDirecPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(rootDirecPath);
            List<DirectoryInfo> subDirectories = dirInfo.GetDirectories().ToList();
            if (subDirectories.Count > 0)
            {

            }

            string errors = "";
            string successFiles = "";
            string directoryPath = rootDirecPath + @"\";
            string[] allFiles = dirInfo.GetFiles("*.sql").Select(f => f.Name.Replace(f.Extension, "")).ToArray();
            foreach (string file in allFiles)
            {
                try
                {
                    string filePath = directoryPath + file + ".sql";
                    string script = File.ReadAllText(filePath);
                    CommonDataContext Context = CommonDataContext.GetContextByDBId("0");
                    SqlConnection sqlConnection1 = new SqlConnection(Context.GetConnection().ConnectionString);
                    SqlCommand cmd = new SqlCommand();
                    SqlDataReader reader;
                    cmd.CommandText = script;
                    cmd.Connection = sqlConnection1;
                    sqlConnection1.Open();
                    reader = cmd.ExecuteReader();

                    successFiles += "-" + file + " executed successfully" + Environment.NewLine;
                }
                catch (Exception ex)
                {
                    errors += "-" + file + " not executed: " + ex.Message + Environment.NewLine;
                }

            }

            if (!string.IsNullOrEmpty(errors))
            {
                string message = "Successful Files:" + Environment.NewLine + successFiles;
                message += Environment.NewLine + "=========================================";
                message += Environment.NewLine + "Failed Files:" + Environment.NewLine + errors;
                FlexibleMessageBox.Show(message, "Script Files");
                //MessageBox.Show(message);
            }
            else
            {
                if (allFiles.Length == 0)
                    MessageBox.Show("No sql files were found in the selected directory!");
                else
                    MessageBox.Show("All files executed successfully!");
            }
        }

        public static void ExecuteAllSqlFiles2(string rootDirecPath, bool subDirCall = false)
        {
            if (!subDirCall)
            {
                totalFiles = new List<string>();
                successFiles = "";
                failedFiles = "";
            }
            DirectoryInfo dirInfo = new DirectoryInfo(rootDirecPath);
            List<DirectoryInfo> subDirectories = dirInfo.GetDirectories().ToList();
            if (subDirectories.Count > 0)
            {
                foreach (DirectoryInfo subDir in subDirectories)
                {
                    ExecuteAllSqlFiles2(subDir.FullName, true);
                }
            }
            else
            {
                string directoryPath = rootDirecPath + @"\";
                string[] allDirFiles = dirInfo.GetFiles("*.sql").Select(f => f.Name.Replace(f.Extension, "")).ToArray();
                //totalFiles = totalFiles.Concat(allDirFiles).ToList();
                foreach (string file in allDirFiles)
                {
                    totalFiles.Add(file);
                    try
                    {
                        string filePath = directoryPath + file + ".sql";
                        string script = File.ReadAllText(filePath);

                        SqlConnection sqlConnection1 = new SqlConnection(ConnectionString);
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = script;
                        cmd.Connection = sqlConnection1;
                        sqlConnection1.Open();
                        SqlDataReader reader = cmd.ExecuteReader();
                        sqlConnection1.Close();

                        successFiles += "-" + dirInfo.Name + "/" + file + " executed successfully" + Environment.NewLine;
                    }
                    catch (Exception ex)
                    {
                        if (ex.Message.Contains("Invalid object name"))
                        {
                            try
                            {
                                string filePath = directoryPath + file + ".sql";
                                string script = File.ReadAllText(filePath);

                                SqlConnection sqlConnection1 = new SqlConnection(GlobalConnectionString);
                                SqlCommand cmd = new SqlCommand();
                                cmd.CommandText = script;
                                cmd.Connection = sqlConnection1;
                                sqlConnection1.Open();
                                SqlDataReader reader = cmd.ExecuteReader();
                                sqlConnection1.Close();

                                successFiles += "-" + dirInfo.Name + "/" + file + " executed successfully" + Environment.NewLine;
                            }
                            catch (Exception gex)
                            {
                                failedFiles += "-" + dirInfo.Name + "/" + file + " not executed: " + gex.Message + Environment.NewLine;
                            }

                        }
                        else
                            failedFiles += "-" + dirInfo.Name + "/" + file + " not executed: " + ex.Message + Environment.NewLine;
                    }

                }

            }

            if (!subDirCall)
            {
                if (!string.IsNullOrEmpty(failedFiles))
                {
                    string message = "Successful Files:" + Environment.NewLine + successFiles;
                    message += Environment.NewLine + "=========================================";
                    message += Environment.NewLine + "Failed Files:" + Environment.NewLine + failedFiles;
                    FlexibleMessageBox.Show(message, "Script Files");
                    //MessageBox.Show(message);
                }
                else
                {
                    if (totalFiles.Count == 0)
                        MessageBox.Show("No sql files were found in the selected directory!");
                    else
                        MessageBox.Show("All files executed successfully!");
                }
            }
        }
    }
}
