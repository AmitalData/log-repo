using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace Logitude.Server.Tools.FTP
{
	public class FTPService
	{
		private string host = null;
		private string user = null;
		private string pass = null;
		private FtpWebRequest ftpRequest = null;
		private FtpWebResponse ftpResponse = null;
		private Stream ftpStream = null;
		private int bufferSize = 2048;

		/* Construct Object */
		public FTPService(string hostIP, string userName, string password)
		{
			if (!hostIP.Contains(@"ftp://") && !hostIP.Contains(@"ftps://"))
			{
				hostIP = @"ftp://" + hostIP;
			}

			host = hostIP; user = userName; pass = password;
		}
		/* Download File */
		public byte[] Download(string remoteFile, out string p_message)
		{
			byte[] result = null;
			p_message = "";
			try
			{

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



				stream.Close();
				ftpStream.Close();
				ftpResponse.Close();
				ftpRequest = null;

				result = stream.ToArray();

				p_message = "File '" + remoteFile + "' was successfully downloaded";
			}
			catch (WebException ex)
			{
               NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex);
				FTPServiceExceptionThrower.Throw(ex, user, host, remoteFile, "download");
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}

			return result;
		}
	

		/* Upload File */
		public bool Upload(string remoteFile, string folder, byte[] fileData, out string p_message, bool uploadAsTemp = false, bool deleteIfExists = false)
		{
			p_message = "";
			try
            {
				CreateFoldersIfNotExist(folder);

                //if (!string.IsNullOrEmpty(folder) && !this.DirectoryExists(folder))
                //            {
                //                this.CreateDirectory(folder);
                //            }

                /* Create an FTP Request */
                string uploadFileName = remoteFile;
                if (uploadAsTemp)
                {
                    uploadFileName = "tmp_" + uploadFileName + ".tmp";
                    DeleteFileIfExists(uploadFileName, folder);
                }
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
                    DeleteFileIfExists(remoteFile, folder);

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

                p_message = "File '" + remoteFile + "' was successfully uploaded to '" + (host + (!string.IsNullOrEmpty(folder) ? ("/" + folder) : ""));

                //else
                //{
                //    throw new Exception("The file already exists");
                //}
            }
            catch (WebException ex)
			{
				//string errorMessage = "";
				//var ftpResponse = ex.Response as FtpWebResponse;
				//switch (ftpResponse.StatusCode)
				//{
				//	case FtpStatusCode.NotLoggedIn:
				//		errorMessage += "Failed to perform 'Logon' to Host:'" + host + "' ,User:'" + user + Environment.NewLine + ftpResponse.StatusDescription;
				//		break;
				//	case FtpStatusCode.ActionNotTakenFileUnavailable:
				//		errorMessage += "Failed to upload " + remoteFile + " file" + Environment.NewLine + ftpResponse.StatusDescription;
				//		break;
				//	default:
				//		errorMessage += "Failed to upload " + remoteFile + " file" + Environment.NewLine + ftpResponse.StatusDescription;
				//		break;
				//}

				FTPServiceExceptionThrower.Throw(ex, user, host, remoteFile, "upload");
			}
			catch (Exception ex)
			{
				throw ex;
			}
			finally
			{
			}

			return true;
		}

		private void CreateFoldersIfNotExist(string folder)
		{
			if (string.IsNullOrEmpty(folder)) return;
			var folders = folder.Split('/');
			string folderPath = string.Empty;
			int folderscount = 0;
			while (folderscount < folders.Length)
			{
				folderPath = CreateFolderIfNotExist(folders[folderscount], folderPath);
				folderscount += 1;
			}
		}

		private string CreateFolderIfNotExist(string folderName, string folderPath)
		{
			if (string.IsNullOrEmpty(folderName)) return folderPath;
			if (!this.DirectoryExists((folderPath + "/" + folderName))) this.CreateDirectory((folderPath + "/" + folderName));
			return (folderPath + "/" + folderName);
		}

		private void DeleteFileIfExists(string remoteFile, string folder)
        {
            bool fileExists = CheckIfFileExists(remoteFile, folder);
            
            if (fileExists)
            {
                this.Delete(remoteFile, folder);
            }
        }

        /* Delete File */
        public void Delete(string deleteFile, string folder = null)
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
			try
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
			}
			catch (Exception exception)
            {

            }
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
		public string[] DirectoryListSimple(string directory)
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
