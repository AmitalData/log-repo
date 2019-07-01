using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
namespace Logitude.Server.Tools.FTP
{
    public class FTPServiceMod
    {
        private string host = null;
        private string user = null;
        private string pass = null;
        private FtpWebRequest ftpRequest = null;
        private FtpWebResponse ftpResponse = null;
        private Stream ftpStream = null;
        private int bufferSize = 2048;

        /* Construct Object */
        public FTPServiceMod(string hostIP, string userName, string password)
        {
            if (!hostIP.Contains(@"ftp://"))
            {
                hostIP = @"ftp://" + hostIP;
            }

            host = hostIP; user = userName; pass = password;
        }
        /* Download File */
        public byte[] Download(string remoteFile, out string p_status, out string p_message)
        {
            byte[] result = null;
            p_status = "";
            p_message = "";
            try
            {

                p_message = FTPLogBuilder.BuildLogLine("Start downloading file " + host + "/" + remoteFile + " ");
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + remoteFile);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DownloadFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Get the FTP Server's Response Stream */
                ftpStream = ftpResponse.GetResponseStream();

                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);

                

                MemoryStream stream = new MemoryStream();
                /* Download the File by Writing the Buffered Data Until the Transfer is Complete */
                while (bytesRead > 0)
                {
                    stream.Write(byteBuffer, 0, bytesRead);
                    bytesRead = ftpStream.Read(byteBuffer, 0, bufferSize);
                }

                p_message += FTPLogBuilder.GetFileSizeString(stream.ToArray().Length);

                stream.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;

                result = stream.ToArray();

                p_message = FTPLogBuilder.AppendLogLine("File '" + remoteFile + "' was successfully downloaded", p_message);
                p_status = "1";
            }
            catch (WebException ex)
            {
                p_message = FTPLogBuilder.AppendLogLine(FTPLogBuilder.GetFTPErrorFromFtpStatusCode(ex, host, user, remoteFile, "download"), p_message);
                p_status = "-1";
            }
            catch (Exception ex)
            {
                p_message = FTPLogBuilder.AppendLogLine("Failed to download file '" + host + "/" + remoteFile + Environment.NewLine + ex.Message, p_message);
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + ex.InnerException.Message;
                p_status = "-1";
            }
            finally
            {
            }

            return result;
        }

        /* Upload File */
        public void Upload(string remoteFile, string folder, byte[] fileData, out string p_message, out string p_status, bool uploadAsTemp = false, bool deleteIfExists = false)
        {
            p_message = "";
            p_status = "";
            try
            {
                //string[] dirFiles = this.DirectoryListSimple(folder);
                //if (!string.IsNullOrEmpty(folder) && !this.DirectoryExists(folder))
                //{
                //    this.CreateDirectory(folder);
                //}

                /* Create an FTP Request */
                string urlPath = host + (!string.IsNullOrEmpty(folder) ? ("/" + folder) : "") + "/";
                p_message = FTPLogBuilder.BuildLogLine("Start uploading file " + remoteFile + " file to " + urlPath);

                string uploadFileName = remoteFile;
                if (uploadAsTemp)
                    uploadFileName = "tmp_" + Guid.NewGuid() + ".tmp";
                string url = host + (!string.IsNullOrEmpty(folder) ? ("/" + folder) : "") + "/" + uploadFileName;
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(url);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.UploadFile;
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpRequest.GetRequestStream();
                /* Open a File Stream to Read the File for Upload */
                MemoryStream localFileStream = new MemoryStream(fileData);
                /* Buffer for the Downloaded Data */
                byte[] byteBuffer = new byte[bufferSize];
                int bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                /* Upload the File by Sending the Buffered Data Until the Transfer is Complete */
                while (bytesSent != 0)
                {
                    ftpStream.Write(byteBuffer, 0, bytesSent);
                    bytesSent = localFileStream.Read(byteBuffer, 0, bufferSize);
                }


                localFileStream.Close();
                ftpStream.Flush();
                ftpStream.Close();

                FtpWebResponse response = (FtpWebResponse)ftpRequest.GetResponse();
                FtpStatusCode code = response.StatusCode;
                string description = response.StatusDescription;
                response.Close();
                ftpRequest = null;

                //if (uploadAsTemp)
                //    ftpRequest.RenameTo = remoteFile;
                ///* Establish Return Communication with the FTP Server */
                //ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ///* Resource Cleanup */
                //ftpResponse.Close();

                if (uploadAsTemp)
                {
                    bool fileExists = CheckIfFileExists(remoteFile, folder);
                    //if (!fileExists || (fileExists && deleteIfExists))
                    //{
                    string p1, p2;
                    if (fileExists)
                    {
                        this.Delete(remoteFile, out p1, out p2, folder);
                    }

                    string currentFileNameAndPath = (!string.IsNullOrEmpty(folder) ? ("/" + folder) : "") + "/" + uploadFileName;
                    this.Rename(currentFileNameAndPath, remoteFile);
                    // }
                    ////ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + currentFileNameAndPath);
                    //ftpRequest.Method = WebRequestMethods.Ftp.Rename;
                    ///* Rename the File */
                    //ftpRequest.RenameTo = remoteFile;
                    ///* Establish Return Communication with the FTP Server */
                    //ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                    ///* Resource Cleanup */
                }

                //p_message = "File '" + remoteFile + "' was successfully uploaded to '" + (host + (!string.IsNullOrEmpty(folder) ? ("/" + folder) : ""));


                p_message = FTPLogBuilder.AppendLogLine("File '" + remoteFile + "' was successfully uploaded to '" + (host + (!string.IsNullOrEmpty(folder) ? ("/" + folder) : "")), p_message);
                //else
                //{
                //    throw new Exception("The file already exists");
                //}
            }
            catch (WebException ex)
            {
                p_message = FTPLogBuilder.AppendLogLine(FTPLogBuilder.GetFTPErrorFromFtpStatusCode(ex, host, user, remoteFile, "upload"), p_message);
                p_status = "-1";
            }
            catch (Exception ex)
            {
                p_message = FTPLogBuilder.AppendLogLine("Failed to upload file '" + host + "/" + remoteFile + Environment.NewLine + ex.Message, p_message);
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + ex.InnerException.Message;
                p_status = "-1";
            }
            finally
            {
            }


        }

        /* Delete File */
        public void Delete(string deleteFile, out string p_message, out string p_status, string folder = null)
        {
            p_message = "";
            p_status = "";
            try
            {

                /* Create an FTP Request */
                string url = host + (!string.IsNullOrEmpty(folder) ? ("/" + folder) : "") + "/" + deleteFile;
                ftpRequest = (FtpWebRequest)WebRequest.Create(url);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.DeleteFile;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Resource Cleanup */
                ftpResponse.Close();
                ftpRequest = null;

                p_message = "File '" + deleteFile + "' was successfully deleted";
            }
            catch (WebException ex)
            {
                p_message = FTPLogBuilder.AppendLogLine(FTPLogBuilder.GetFTPErrorFromFtpStatusCode(ex, host, user, deleteFile, "upload"), p_message);
                p_status = "-1";
            }
            catch (Exception ex)
            {
                p_message = FTPLogBuilder.AppendLogLine("Failed to upload file '" + host + "/" + deleteFile + Environment.NewLine + ex.Message, p_message);
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + ex.InnerException.Message;
                p_status = "-1";
            }
            finally
            {
            }

            return;
        }

        /* Rename File */
        public void Rename(string currentFileNameAndPath, string newFileName)
        {

            /* Create an FTP Request */
            string url = host + "/" + currentFileNameAndPath.TrimStart('/');
            ftpRequest = (FtpWebRequest)WebRequest.Create(url);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.Rename;
            /* Rename the File */
            ftpRequest.RenameTo = newFileName;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Resource Cleanup */
            ftpResponse.Close();
            ftpRequest = null;

            return;
        }

        /* Create a New Directory on the FTP Server */
        public void CreateDirectory(string newDirectory)
        {

            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + newDirectory);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.MakeDirectory;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Resource Cleanup */
            ftpResponse.Close();
            ftpRequest = null;

            return;
        }

        public bool DirectoryExists(string directory)
        {

            bool directoryExists = false;
            try
            {
                ftpRequest = (FtpWebRequest)WebRequest.Create(host + "/" + directory);
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;

                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                ftpStream = ftpResponse.GetResponseStream();

                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try
                {
                    while (ftpReader.Peek() != -1)
                    {
                        directoryRaw += ftpReader.ReadLine() + "|";
                    }

                    directoryExists = true;
                }
                catch (Exception ex)
                {
                    if (ex.GetType() == typeof(WebException))
                    {
                        WebException webb = (WebException)ex;
                        if (webb.Response != null)
                        {
                            FtpWebResponse response = (FtpWebResponse)webb.Response;
                            if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                            {
                                // Directory not found. 
                                directoryExists = false;
                            }
                        }
                    }

                }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */

            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    FtpWebResponse response = (FtpWebResponse)ex.Response;
                    if (response.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)
                    {
                        // Directory not found. 
                        directoryExists = false;
                    }
                }
            }

            return directoryExists;
        }

        public bool CheckIfFileExists(string fileName, string folder)
        {

            /* Create an FTP Request */
            string url = host + (!string.IsNullOrEmpty(folder) ? ("/" + folder) : "") + "/" + fileName;
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest = (FtpWebRequest)WebRequest.Create(url);
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
            /* Establish Return Communication with the FTP Server */
            try
            {
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;


            }
            catch (WebException ex)
            {
                FtpWebResponse response = (FtpWebResponse)ex.Response;
                if (response.StatusCode ==
                    FtpStatusCode.ActionNotTakenFileUnavailable)
                {
                    return false;
                }


            }

            return true;
        }


        /* Get the Date/Time a File was Created */
        public string GetFileCreatedDateTime(string fileName)
        {

            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.GetDateTimestamp;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Establish Return Communication with the FTP Server */
            ftpStream = ftpResponse.GetResponseStream();
            /* Get the FTP Server's Response Stream */
            StreamReader ftpReader = new StreamReader(ftpStream);
            /* Store the Raw Response */
            string fileInfo = null;
            /* Read the Full Response Stream */
            fileInfo = ftpReader.ReadToEnd();

            /* Resource Cleanup */
            ftpReader.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
            /* Return File Created Date Time */
            return fileInfo;


        }

        /* Get the Size of a File */
        public string GetFileSize(string fileName)
        {

            /* Create an FTP Request */
            ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + fileName);
            /* Log in to the FTP Server with the User Name and Password Provided */
            ftpRequest.Credentials = new NetworkCredential(user, pass);
            /* When in doubt, use these options */
            ftpRequest.UseBinary = true;
            ftpRequest.UsePassive = true;
            ftpRequest.KeepAlive = true;
            /* Specify the Type of FTP Request */
            ftpRequest.Method = WebRequestMethods.Ftp.GetFileSize;
            /* Establish Return Communication with the FTP Server */
            ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
            /* Establish Return Communication with the FTP Server */
            ftpStream = ftpResponse.GetResponseStream();
            /* Get the FTP Server's Response Stream */
            StreamReader ftpReader = new StreamReader(ftpStream);
            /* Store the Raw Response */
            string fileInfo = null;
            /* Read the Full Response Stream */
            try { while (ftpReader.Peek() != -1) { fileInfo = ftpReader.ReadToEnd(); } }
            catch (Exception ex) { Console.WriteLine(ex.ToString()); }
            /* Resource Cleanup */
            ftpReader.Close();
            ftpStream.Close();
            ftpResponse.Close();
            ftpRequest = null;
            /* Return File Size */
            return fileInfo;

            /* Return an Empty string Array if an Exception Occurs */

        }

        /* List Directory Contents File/Folder Name Only */
        public string[] DirectoryListSimple(string directory, out string p_message, out string p_status, string p_pattern = "")
        {
            string[] directoryList = { };
            p_message = "";
            p_status = "";
            if (string.IsNullOrEmpty(p_pattern)) p_pattern = "*";
            string v_pattern = p_pattern;
            try
            {

                
                p_pattern = FTPLogBuilder.WildcardToRegex(p_pattern);
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectory;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */

                if (!string.IsNullOrEmpty(directoryRaw))
                {
                    directoryList = directoryRaw.Split("|".ToCharArray());
                }
                List<string> filterdFiles = new List<string>();
                if (!string.IsNullOrEmpty(p_pattern))
                {
                    foreach (string fileName in directoryList.Where(f => !string.IsNullOrWhiteSpace(f) && !string.IsNullOrWhiteSpace(Path.GetExtension(f))).ToList())
                    {
                        Match match = Regex.Match(fileName, p_pattern, RegexOptions.IgnoreCase);
                        // Here we check the Match instance.
                        if (match.Success)
                        {
                            filterdFiles.Add(fileName);
                        }

                    }

                    directoryList = filterdFiles.ToArray();

                    //if (!v_found)
                    //    p_message = "No files found with path/pattern '" + sftp.RemotePath + v_pattern + "'";
                    //else
                    //    p_message = v_count + " files found with path/pattern '" + sftp.RemotePath + v_pattern + "'";

                    if (directoryList.Count() == 0)
                        p_message = "No files found with path/pattern '" + host + "/" + directory + "/" + v_pattern + "/" + "'";
                    else
                        p_message = directoryList.Count() + " files found with path/pattern '" + host + "/" + directory  + v_pattern + "'";
                }
                else
                {
                    if (directoryList.Count() == 0)
                        p_message = "No files found with path/pattern '" + host + "/" + directory + "/" + v_pattern;
                    else
                        p_message = directoryList.Count() + " files found with path/pattern '" + host + "/" + directory + "/" + v_pattern;
                }
            }
            catch (WebException ex)
            {
                p_message = FTPLogBuilder.AppendLogLine(FTPLogBuilder.GetFTPErrorFromFtpStatusCode(ex, host, user, host + "/" + directory, "directory"), p_message);
                p_status = "-1";

            }
            catch (Exception ex)
            {
                p_message = "Failed to 'List Directory' ";
                p_message += "with path/pattern '" + host + "/" + directory + v_pattern + "'";
                p_message += Environment.NewLine + ex.Message;
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + ex.InnerException.Message;
                p_status = "-1";

            }

            return directoryList;


        }

        /* List Directory Contents in Detail (Name, Size, Created, etc.) */
        public string[] DirectoryListDetailed(string directory)
        {
            string[] directoryList = { };
            try
            {
                /* Create an FTP Request */
                ftpRequest = (FtpWebRequest)FtpWebRequest.Create(host + "/" + directory);
                /* Log in to the FTP Server with the User Name and Password Provided */
                ftpRequest.Credentials = new NetworkCredential(user, pass);
                /* When in doubt, use these options */
                ftpRequest.UseBinary = true;
                ftpRequest.UsePassive = true;
                ftpRequest.KeepAlive = true;
                /* Specify the Type of FTP Request */
                ftpRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
                /* Establish Return Communication with the FTP Server */
                ftpResponse = (FtpWebResponse)ftpRequest.GetResponse();
                /* Establish Return Communication with the FTP Server */
                ftpStream = ftpResponse.GetResponseStream();
                /* Get the FTP Server's Response Stream */
                StreamReader ftpReader = new StreamReader(ftpStream);
                /* Store the Raw Response */
                string directoryRaw = null;
                /* Read Each Line of the Response and Append a Pipe to Each Line for Easy Parsing */
                try { while (ftpReader.Peek() != -1) { directoryRaw += ftpReader.ReadLine() + "|"; } }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }
                /* Resource Cleanup */
                ftpReader.Close();
                ftpStream.Close();
                ftpResponse.Close();
                ftpRequest = null;
                /* Return the Directory Listing as a string Array by Parsing 'directoryRaw' with the Delimiter you Append (I use | in This Example) */

                if (!string.IsNullOrEmpty(directoryRaw))
                {
                    directoryList = directoryRaw.Split("|".ToCharArray());
                }
            }
            catch (WebException ex)
            {
                FTPServiceExceptionThrower.Throw(ex, user, host, "", "directory");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return directoryList;

        }
    }
}