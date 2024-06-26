using Logitude.Server.Tools.Utils;
using nsoftware.IPWorksSSH;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.FTP
{
    public class SFTPService
    {
        private nsoftware.IPWorksSSH.Sftp sftp;
        private int _currIdxFile;
        private string _start_path = "";
        private string _pattern = "";
        private List<FileParam> _files;
        private readonly SFTPDeleteTempFilesService _SFTPDeleteTempFiles;

        public SFTPService(SFTPDeleteTempFilesService sFTPDeleteTempFilesService=null)
        {
            _SFTPDeleteTempFiles = sFTPDeleteTempFilesService;
            Initialize();
        }
        private void Initialize()
        {
            sftp = null;
            _currIdxFile = 0;
        }
        public void Logon(string p_host, string p_user, string p_password, string p_port, string p_directory, out string p_status, out string p_message)
        {
       
            MyStart();
            if (string.IsNullOrEmpty(p_port))
                p_port = "22";
            p_status = "";
            p_message = "";
            int v_port = 80;
            int v_sshport = 0;
            int.TryParse(p_port, out v_sshport);
            Initialize();
            try
            {
                if (string.IsNullOrEmpty(p_port)) p_port = "80";
                int.TryParse(p_port, out v_port);
                if (string.IsNullOrWhiteSpace(p_host))
                {
                    p_message = "Parameter 'Server Host' is missing";
                    p_status = "-1";
                    return;
                }
                if (string.IsNullOrWhiteSpace(p_user))
                {
                    p_message = "Parameter 'User Name' is missing";
                    p_status = "-1";
                    return;
                }
                sftp = new nsoftware.IPWorksSSH.Sftp();
                //sftp.RuntimeLicense = "2121";
                sftp.Firewall.Port = v_port;
                sftp.RemotePath = "././././";
                sftp.OnSSHServerAuthentication += new nsoftware.IPWorksSSH.Sftp.OnSSHServerAuthenticationHandler(sftp_OnSSHServerAuthentication);
                sftp.OnSSHStatus += new nsoftware.IPWorksSSH.Sftp.OnSSHStatusHandler(sftp_OnSSHStatus);
                //this.sftp1.OnDirList += new nsoftware.IPWorksSSH.Sftp.OnDirListHandler(this.sftp1_OnDirList);
                sftp.SSHAuthMode = nsoftware.IPWorksSSH.SftpSSHAuthModes.amPassword;
                sftp.SSHHost = p_host;
                sftp.SSHUser = p_user;
                sftp.SSHPassword = p_password;
                sftp.RemotePath = p_directory;
                sftp.SSHAuthMode = SftpSSHAuthModes.amPublicKey;
                sftp.RuntimeLicense = "31484E42414431535542323031393130323552413153554241544A353234353800000000000000003135554732304250000058415852315432434D5233410000";
                //string projectPath = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
                //DirectoryInfo solutionDir = System.IO.Directory.GetParent(projectPath);
                //string solutionDirectory = solutionDir.FullName;
                //string cerfFilePath = solutionDirectory + @"\Logitude.Server.Tools\FTP\private.pem";

               // string cerfFilePath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"FTP\private.pem");
                //sftp.SSHCert = new Certificate(CertStoreTypes.cstPEMKeyFile, cerfFilePath, "test", "*");//@"C:\temp\private.pem"
                


                //sftp.Connected = true;
                sftp.SSHLogon(p_host, v_sshport);
                //sftp.SSHLogoff();
                p_message = "Successfully connected to Host:'" + p_host + "' ,User:'" + p_user;
                p_message += "', directory:'" + sftp.RemotePath + "'";

                p_message = FTPLogBuilder.BuildLogLine(p_message);
                p_status = "0";
            }
            catch (Exception ex)
            {
                p_status = "-1";
                p_message = "Failed to perform 'Logon' to Host:'" + p_host + "' ,User:'" + p_user;
                p_message += "', directory:'" + sftp.RemotePath + "'";
                p_message += Environment.NewLine + ex.Message;
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + p_message;
            }
            finally
            {
                MyFinally();
            }
        }
        public void Logoff(ref string p_more, out string p_status, out string p_message)
        {
            MyStart();
            p_status = "";
            p_message = "";
            if (sftp == null)
            {
                p_status = "0";
                p_message = "Application is not connected. There was no need to perform 'Logoff'";
                return;
            }
            try
            {

                sftp.SSHLogoff();
            }
            catch (Exception ex)
            {
                p_status = "-1";
                p_message = "Performing 'Logoff' failed'" + Environment.NewLine + ex.Message;
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + ex.InnerException.Message;
            }
            finally
            {
                sftp = null;
                _currIdxFile = 0;
                MyFinally();
            }

        }
        public void DownloadAllDirectoryFiles(string p_pattern, out string p_download_filename, out string p_status, out string p_message)
        {
            MyStart();
            bool v_contiue;
            p_status = "";
            p_message = "";
            bool v_downloaded = false;
            DirEntryList v_dirList = null;
            p_download_filename = "";
            if (string.IsNullOrEmpty(p_pattern)) p_pattern = "*";
            string v_pattern = p_pattern;
            StringBuilder sb = new StringBuilder();
            try
            {
                if (sftp == null)
                {
                    throw new Exception("The 'Sftp' object is null" +
                        Environment.NewLine + "Probably 'Login' was not performed");
                }
                if (v_pattern != _pattern)
                    _files = null;
                sftp.RemoteFile = "";
                p_pattern = WildcardToRegex(p_pattern);
                if (_files == null || v_pattern != _pattern)
                {
                    _files = new List<FileParam>();
                    sftp.ListDirectory();
                    v_dirList = sftp.DirList;
                    foreach (DirEntry de in v_dirList)
                    {
                        _files.Add(new FileParam(de.FileName, de.IsDir, de.FileSize, de.FileTime));
                    }
                    _pattern = v_pattern;
                    _currIdxFile = 0;
                }
                else
                {
                    _currIdxFile++;
                }
                if (_files == null)
                {
                    p_message = "No more files for pattern '" + v_pattern + "'";
                    if (sftp.RemotePath != "") p_message += " in directory '" + sftp.RemotePath + "'";
                    p_status = "0";
                    return;
                }
                if (_currIdxFile >= _files.Count)
                {
                    p_message = "No more files for pattern '" + v_pattern + "'";
                    if (sftp.RemotePath != "") p_message += " in directory '" + sftp.RemotePath + "'";
                    p_status = "0";
                    return;
                }
                for (; _currIdxFile < _files.Count; _currIdxFile++)
                {
                    FileParam fp = _files[_currIdxFile];
                    if (!fp.IsDir)
                    {
                        Match match = Regex.Match(fp.Filename, p_pattern,
        RegexOptions.IgnoreCase);

                        // Here we check the Match instance.
                        if (match.Success)
                        {
                            // Finally, we get the Group value and display it.
                            p_download_filename = "";
                            Console.WriteLine(match.ToString());
                            sb.AppendLine(match.ToString());
                            byte[] fileData = DownloadFileInternally(fp.Filename, out v_contiue, out p_status, out p_message);
                            if (!v_contiue)
                            {
                                v_downloaded = true;
                                //p_download_filename = Path.GetDirectoryName(p_localpath + @"\") + @"\" + Path.GetFileName(fp.Filename);
                                //p_download_filename += Environment.NewLine + "#" + _currIdxFile.ToString();
                                break;
                            }
                        }
                    }
                }
                if (p_status == "-1") return;
                //p_message = sb.ToString();
                if (!v_downloaded)
                {
                    p_message = "No more files for pattern '" + v_pattern + "'";
                    if (sftp.RemotePath != "") p_message += " in directory '" + sftp.RemotePath + "'";
                }
                p_status = "0";
            }
            catch (Exception ex)
            {
                p_status = "-1";
                p_message = "Failed to 'Download' file "; //+ Environment.NewLine + ex.Message;
                if (sftp != null) p_message += "'" + sftp.RemotePath + v_pattern + "'";
                p_message += ex.Message;
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + p_message;
            }
            finally
            {
                MyFinally();
            }
        }

        public void DeleteFile(string p_filename, out string p_status, out string p_message, bool toDir = true)
        {
            MyStart();
            p_status = "";
            p_message = "";
            try
            {
                if (string.IsNullOrEmpty(p_filename) || string.IsNullOrWhiteSpace(p_filename))
                {
                    p_message = "Parameter 'FileName' is missing";
                    p_status = "-1";
                    return;
                }
                if (sftp == null)
                {
                    throw new Exception("The 'Sftp' object is null" +
                        Environment.NewLine + "Probably 'Login' was not performed");
                }
                sftp.RemoteFile = p_filename;
                //if (!sftp.FileExists)
                if (toDir && !IsRemoteFileExist(p_filename))
                {
                    p_message = "File '" + p_filename + "' not exist";
                    if (sftp.RemotePath != "") p_message += " in directory '" + sftp.RemotePath + "'";
                    p_status = "0";
                    return;
                }
                sftp.DeleteFile(p_filename);
                p_message = "File '" + p_filename + "' was successfully deleted";
                if (sftp.RemotePath != "") p_message += " in directory '" + sftp.RemotePath + "'";
                p_status = "0";

            }
            catch (Exception ex)
            {
                p_message = "Failed to delete file ";
                if (sftp != null) p_message += "'" + sftp.RemotePath + p_filename + "'";
                p_message += Environment.NewLine + ex.Message;
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + ex.InnerException.Message;
                p_status = "-1";
            }
            finally
            {
                MyFinally();
            }

            p_message = FTPLogBuilder.BuildLogLine(p_message);
        }
   
        private bool IsRemoteFileExist(string p_filename)
        {

            bool v_exist = false;
            string v_filename = "";
            try
            {
                v_filename = sftp.RemoteFile;
                sftp.RemoteFile = p_filename;
                sftp.ListDirectory();
                DirEntryList _dirList = sftp.DirList;
                foreach (DirEntry dirEntry in _dirList)
                {
                    if (dirEntry.FileName != "")
                    {
                        if (dirEntry.FileName.ToUpper() == p_filename.ToUpper())
                        {
                            v_exist = true;
                            break;
                        }
                    }
                }
                return (v_exist);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (sftp != null)
                {
                    sftp.RemoteFile = v_filename;
                }
            }
        }


        private void CreateFoldersIfNotExist(nsoftware.IPWorksSSH.Sftp sftp)
        {
            if (string.IsNullOrEmpty(sftp.RemotePath)) return;
            var folders = sftp.RemotePath.Split('/');
            string folderPath = string.Empty;
            int folderscount = 0;
            while (folderscount < folders.Length)
            {
                folderPath = CreateFolderIfNotExist(folders[folderscount], folderPath, sftp);
                folderscount += 1;
            }
        }

        private string CreateFolderIfNotExist(string folderName, string folderPath, nsoftware.IPWorksSSH.Sftp sftp)
        {
            if (string.IsNullOrEmpty(folderName)) return folderPath;

            try
            {
                sftp.MakeDirectory((folderPath + "/" + folderName));
            }
            catch (Exception exception) { }

            return (folderPath + "/" + folderName);
        }

        public void Upload(string p_filename, byte[] filedata, bool deleteIfExist, bool uploadAsTemp, out string p_status, out string p_message)
        {
            MyStart();
            p_status = "";
            p_message = "";
            //if (!string.IsNullOrEmpty(p_folderName) && sftp.RemotePath == @"/")
            //    sftp.RemotePath = p_folderName;
            string v_filename = Path.GetFileName(p_filename);
            string v_temp_filename = "", v_temp_filename_full = "";
            try
            {
                p_message = FTPLogBuilder.BuildLogLine("Start uploading file " + p_filename + " file to " +sftp.SSHHost + sftp.RemotePath);
                if (sftp == null)
                {
                    throw new Exception("The 'Sftp' object is null" +
                        Environment.NewLine + "Probably 'Login' was not performed");
                }
                if (string.IsNullOrEmpty(v_filename) || string.IsNullOrWhiteSpace(v_filename))
                {
                    //p_message = "Parameter 'FileName' is missing";

                    p_message = FTPLogBuilder.AppendLogLine("Parameter 'FileName' is missing", p_message);
                    p_status = "-1";
                    return;
                }

                CreateFoldersIfNotExist(sftp);


                if (deleteIfExist)
                    sftp.Overwrite = true;
                else
                    sftp.Overwrite = false;
                sftp.RemoteFile = v_filename;
                if (IsRemoteFileExist(v_filename))
                {
                    if (deleteIfExist)
                    {
                        sftp.DeleteFile(v_filename);
                    }
                    else
                        throw new Exception("The file '" + sftp.RemotePath + v_filename + "' already exist." +
                            Environment.NewLine + "Please delete it or use paramer 'Del New Name' to 'auto delete' it");
                }
                if (uploadAsTemp)
                {
                    v_temp_filename = "tmp_" + p_filename + ".tmp";
                    v_temp_filename_full = Path.GetDirectoryName(p_filename) + "\\" + v_temp_filename;
                    //File.Copy(p_filename, v_temp_filename_full);
                    Upload(v_temp_filename_full, filedata, true, false, out p_status, out p_message);
                    if (p_status == "-1") return;
                    sftp.RemoteFile = v_temp_filename;
                    sftp.RenameFile(v_filename);
                    sftp.RemoteFile = v_filename;

                }
                else
                {
                    // sftp.LocalFile = p_filename;
                    //

                    sftp.SetUploadStream(new MemoryStream(filedata));
                    sftp.Upload();

                }
                string v_1 = "";
                //if (uploadAsTemp) v_1 = " with 'Upload As Temp File' property ";
                //p_message = "File '" + p_filename + "' was successfully uploaded to '" + sftp.SSHHost + sftp.RemotePath + sftp.RemoteFile + v_1;

                p_message = FTPLogBuilder.AppendLogLine("File '" + p_filename + "' was successfully uploaded to '" + sftp.SSHHost + sftp.RemotePath + sftp.RemoteFile + v_1, p_message);
                p_status = "0";
            }
            catch (Exception ex)
            {
                string v_1 = "";
                //if (uploadAsTemp) v_1 = " with 'Upload As Temp File' property ";
                string error_p_message = "Failed to upload" + v_1 + "file ";
                if (sftp != null) error_p_message += "'" + p_filename + "' to '" + sftp.RemotePath + sftp.RemoteFile + "'";
                error_p_message += Environment.NewLine + ex.Message;
                if (ex.InnerException != null)
                    error_p_message += Environment.NewLine + ex.InnerException.Message;
                p_status = "-1";
                p_message = FTPLogBuilder.AppendLogLine("Parameter 'FileName' is missing", p_message);
            }
            finally
            {
                if (File.Exists(v_temp_filename_full))
                    File.Delete(v_temp_filename_full);

                try
                {
                    if (_SFTPDeleteTempFiles?.DeleteIfNeeded(uploadAsTemp, (Action)(this.DeleteTempFiles)) == true)
                    {
                        p_message += Environment.NewLine + $" SFTPDeleteTempFilesService - delete temp file  ";
                    }
                    
                }
                catch (Exception e)
                {

                    NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e, "SFTP");
                }

                MyFinally();
            }

            
        }

        private void DeleteTempFiles()
        {

            string p_status_1;
            string p_message_1;

           NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"DirList('tmp_ *.tmp') ..");
            var directoryFiles = DirList("tmp_*.tmp", true, false, out p_status_1, out p_message_1).ToList();
           NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"Temp file {directoryFiles.Count()}");


            foreach (var fileName in directoryFiles)
            {
               NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"delete Temp file {fileName}");
                DeleteFile(fileName, out p_status_1, out p_message_1);
            }

        }

        

        public void Rename(string p_curr_name, string p_new_name, string p_del_new_name, ref string p_more, out string p_status, out string p_message)
        {
            MyStart();
            p_status = "";
            p_message = "";
            bool v_del_new_name = CnBool(p_del_new_name);
            try
            {
                if (sftp == null)
                {
                    throw new Exception("The 'Sftp' object is null" +
                        Environment.NewLine + "Probably 'Login' was not performed");
                }
                if (string.IsNullOrEmpty(p_curr_name))
                {
                    p_message = "Parameter 'Current File Name' is missing.";
                    p_status = "-1";
                    return;
                }
                if (string.IsNullOrEmpty(p_new_name))
                {
                    p_message = "Parameter 'New File Name' is missing.";
                    p_status = "-1";
                    return;
                }
                sftp.RemoteFile = p_curr_name;
                //if (!sftp.FileExists)
                if (!IsRemoteFileExist(p_curr_name))
                {
                    throw new Exception("The current file '" + sftp.RemotePath + p_curr_name + "' not exist");
                }
                sftp.RemoteFile = p_new_name;
                //if (sftp.FileExists)
                if (IsRemoteFileExist(p_new_name))
                {
                    if (v_del_new_name)
                    {
                        sftp.DeleteFile(p_new_name);
                    }
                    else
                        throw new Exception("The 'renamed to' file '" + sftp.RemotePath + p_new_name + "' already exist." +
                            Environment.NewLine + "Please delete it or use paramer 'Del New Name' to 'auto delete' it");
                }
                sftp.RemoteFile = p_curr_name;
                sftp.RenameFile(p_new_name);
                p_message = "File '" + sftp.RemotePath + p_curr_name + "' was sucessfully renamed to '" + sftp.RemotePath + p_new_name + "'";
                p_status = "0";
            }
            catch (Exception ex)
            {
                if (sftp != null) p_message +=
                p_message = "Failed to rename file ";
                if (sftp != null) p_message += "'" + sftp.RemotePath + p_curr_name + "' to '" + sftp.RemotePath + p_new_name;
                p_message += Environment.NewLine + ex.Message;
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + ex.InnerException.Message;
                p_status = "-1";
            }
            finally
            {
                MyFinally();
            }

        }

        public void ChangeDir(string p_remote_dir, ref string p_more, out string p_status, out string p_message)
        {
            MyStart();
            p_status = "";
            p_message = "";
            _files = null;
            try
            {
                if (sftp == null)
                {
                    throw new Exception("The 'Sftp' object is null" +
                        Environment.NewLine + "Probably 'Login' was not performed");
                }
                if (string.IsNullOrEmpty(p_remote_dir))
                {
                    p_message = "Parameter 'Remote Dir' is null.";
                    p_status = "0";
                    return;
                }
                p_remote_dir = p_remote_dir.Replace("\\", "/");
                sftp.RemotePath = p_remote_dir;
            }
            catch (Exception ex)
            {
                p_message = "Failed to change directory '" + p_remote_dir + "'" +
    Environment.NewLine + ex.Message;
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + ex.InnerException.Message;
                p_status = "-1";
            }
            finally
            {
                MyFinally();
            }
        }

        public List<string> DirList(string p_pattern, bool p_onlyfiles, bool p_onlydirs, out string p_status, out string p_message)
        {
            List<string> filesList = new List<string>();
            MyStart();
            p_status = "";
            p_message = "";
             
            bool v_onlyfiles = p_onlyfiles;
            bool v_onlydirs = p_onlydirs;
            bool v_found = false;
            int v_count = 0;
            if (string.IsNullOrEmpty(p_pattern)) p_pattern = "*";
            string v_pattern = p_pattern;
            StringBuilder sb = new StringBuilder();
            try
            {
                if (sftp == null)
                {
                    throw new Exception("The 'Sftp' object is null" +
                        Environment.NewLine + "Probably 'Login' was not performed");
                }
                sftp.RemoteFile = "";
                p_pattern = WildcardToRegex(p_pattern);
                sftp.ListDirectory();
                DirEntryList _dirList = sftp.DirList;
                foreach (DirEntry dirEntry in _dirList)
                {
                    Match match = Regex.Match(dirEntry.FileName, p_pattern,
    RegexOptions.IgnoreCase);
                    // Here we check the Match instance.
                    if (match.Success)
                    {
                        // Finally, we get the Group value and display it.
                        v_found = true;
                        if (v_onlyfiles)
                        {
                            if (dirEntry.FileName != ".." && !dirEntry.IsDir) v_count++;
                        }
                        else if (p_onlydirs)
                        {
                            if (dirEntry.FileName != ".." && dirEntry.IsDir) v_count++;
                        }
                        else if (dirEntry.FileName != "..") v_count++;
                        if (dirEntry.IsDir && !v_onlyfiles)
                            filesList.Add(dirEntry.FileName + " <dir>");//sb.AppendLine(dirEntry.FileName + " <dir>");
                        if (!dirEntry.IsDir && !v_onlydirs)
                            filesList.Add(dirEntry.FileName);//sb.AppendLine(dirEntry.FileName);
                        Console.WriteLine(match.ToString());
                      
                    }
                }
                //p_filelist = sb.ToString();
                if (!v_found)
                    p_message = "No files found with path/pattern '" + sftp.RemotePath + v_pattern + "'";
                else
                    p_message = v_count + " files found with path/pattern '" + sftp.RemotePath + v_pattern + "'";

            }
            catch (Exception ex)
            {
                p_message = "Failed to 'List Directory' ";
                if (sftp != null) p_message += "with path/pattern '" + sftp.RemotePath + v_pattern + "'";
                p_message += Environment.NewLine + ex.Message;
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + ex.InnerException.Message;
                p_status = "-1";

            }
            finally
            {
                MyFinally();
            }

            p_message = FTPLogBuilder.BuildLogLine(p_message);

            return filesList;
        }
      

        public byte[] DownloadFile(string p_filename, out string p_status, out string p_message,bool toDir=true)
        {
            MemoryStream downloadStream = new MemoryStream();
            //p_continue = false;
            p_status = "";
            p_message = "";
            try
            {
                sftp.RemoteFile = p_filename;

                p_message = FTPLogBuilder.BuildLogLine("Start downloading file " + sftp.RemotePath + p_filename + " " + FTPLogBuilder.GetFileSizeStringInBytes(sftp.FileAttributes.Size));
                 
                sftp.SetDownloadStream(downloadStream);
                //sftp.LocalFile = p_localpath + @"\" + p_filename;
                sftp.Overwrite = true;
                //if (!sftp.FileExists)
                if (toDir &&  !IsRemoteFileExist(p_filename))
                {
                    // p_continue = true;
                    p_status = "-2";
                    //p_message = "Failed to download file '" + sftp.RemotePath + p_filename + ", file doesn't exist";
                    p_message = FTPLogBuilder.AppendLogLine("Failed to download file '" + sftp.RemotePath + p_filename + ", file doesn't exist", p_message);
                    return null;
                }
                else
                {
                    //sftp.do
                    sftp.Download();
                    // p_message = "File '" + p_filename + "' was successfully downloaded";// to '" + sftp.LocalFile + "'";
                    p_message = FTPLogBuilder.AppendLogLine("File '" + p_filename + "' was successfully downloaded", p_message);
                    if (sftp.RemotePath != "") p_message += " in directory '" + sftp.RemotePath + "'";
                    p_status = "1";
                    // p_continue = false;
                }

            }
            catch (Exception ex)
            {
                //p_message = "Failed to download file '" + sftp.RemotePath + p_filename;//"' to local path '" + sftp.LocalFile + "'";
                //p_message += Environment.NewLine + ex.Message;

                p_message = FTPLogBuilder.AppendLogLine("Failed to download file '" + sftp.RemotePath + p_filename + Environment.NewLine + ex.Message, p_message);
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + ex.InnerException.Message;
                p_status = "-1";
                //p_continue = false;
            }

            return downloadStream.ToArray();
        }
        #region Infrastructure
        private byte[] DownloadFileInternally(string p_filename, out bool p_continue, out string p_status, out string p_message)
        {
            MemoryStream downloadStream = new MemoryStream();
            p_continue = false;
            p_status = "";
            p_message = "";
            try
            {
                sftp.RemoteFile = p_filename;
                sftp.SetDownloadStream(downloadStream);
                //sftp.LocalFile = p_localpath + @"\" + p_filename;
                sftp.Overwrite = true;
                //if (!sftp.FileExists)
                if (!IsRemoteFileExist(p_filename))
                {
                    p_continue = true;
                    return null;
                }
                else
                {
                    //sftp.do
                    sftp.Download();
                    p_message = "File '" + p_filename + "' was successfully downloaded";// to '" + sftp.LocalFile + "'";
                    if (sftp.RemotePath != "") p_message += " in directory '" + sftp.RemotePath + "'";
                    p_continue = false;
                }

            }
            catch (Exception ex)
            {
                p_message = "Failed to download file '" + sftp.RemotePath + p_filename;// + "' to local path '" + sftp.LocalFile + "'";
                p_message += Environment.NewLine + ex.Message;
                if (ex.InnerException != null)
                    p_message += Environment.NewLine + ex.InnerException.Message;
                p_status = "-1";
                p_continue = false;
            }

            return downloadStream.ToArray();
        }

        private void sftp_OnSSHServerAuthentication(object sender, nsoftware.IPWorksSSH.SftpSSHServerAuthenticationEventArgs e)
        {
            e.Accept = true;
        }

        private void sftp_OnSSHStatus(object sender, nsoftware.IPWorksSSH.SftpSSHStatusEventArgs e)
        {
            //txtSSHStatus.AppendText(e.Message + "\r\n");
            Console.WriteLine(e.Message);
        }

        public static bool CnBool(object p_obj)
        {
            bool v_bool = false;
            string v_str = "";
            if (p_obj == null) return (v_bool);
            v_str = (string)p_obj;
            if (v_str == "1" || v_str == "T" || v_str == "t" || v_str == "y" || v_str == "y") return (true);
            bool.TryParse(v_str, out v_bool);
            return (v_bool);
        }
        private void MyStart()
        {
            if (sftp != null)
                sftp.RemoteFile = "";
            _start_path = Directory.GetCurrentDirectory();
        }

        private string WildcardToRegex(string p_pattern)
        {
            try
            {
                if (string.IsNullOrEmpty(p_pattern)) p_pattern = "*";
                p_pattern = Wildcard.WildcardToRegex(p_pattern);
                return (p_pattern);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void MyFinally()
        {
            if (sftp != null)
            {
                sftp.RemoteFile = "";
            }
            if (_start_path != "")
                Directory.SetCurrentDirectory(_start_path);

        }
        # endregion



    }
    public class FileParam
    {
        public string Filename;
        public bool IsDir;
        long Size;
        string CreateDate;
        public FileParam(string filename, bool isDir, long size, string createDate)
        {
            Filename = filename;
            IsDir = isDir;
            Size = size;
            CreateDate = createDate;
        }
    }
}