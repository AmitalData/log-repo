using System;
using System.Collections.Generic;
using System.Linq;
using Logitude.Server.Tools.FTP;

namespace Logitude.Server.Tools.ExternalServices
{
    public interface IDcaManagerShim
    {
        List<string> FileListing(string searchPattern, string appendFolder, ref string moreParams,
                                 out bool error, out string log);
        string GetContentsBASE64OfDownloadIncomeFile(string filename, string appendFolder, ref string moreParams,
                                 out bool error, out string log);
        void DeleteIncomeFile(string filename, string appendFolder, ref string moreParams,
                                 out bool error, out string log);
        void RenameIncomeFile(string filename, string newFilename, string appendFolder, ref string moreParams,
                                 out bool error, out string log);
        void MoveIncomeFileToDir(string filename, string renameFilename, string appendFolder, string targetDir,
                                 ref string moreParams, out bool error, out string log);
        void PurgeOldFiles(int keepDays = 14);
    }

    public sealed class LegacyDcaManagerShim : IDcaManagerShim
    {
        private readonly DcaManager _inner;

        public LegacyDcaManagerShim(DcaManager inner) => _inner = inner;

        public List<string> FileListing(string searchPattern, string appendFolder, ref string moreParams,
            out bool error, out string log)
        {
            return _inner.FileListing(searchPattern, appendFolder, ref moreParams, out error, out log);
        }

        public string GetContentsBASE64OfDownloadIncomeFile(string filename, string appendFolder,
            ref string moreParams, out bool error, out string log)
        {
            return _inner.GetContentsBASE64OfDownloadIncomeFile(filename, appendFolder, ref moreParams, out error, out log);
        }

        public void DeleteIncomeFile(string filename, string appendFolder, ref string moreParams,
            out bool error, out string log)
        {
            _inner.DeleteIncomeFile(filename, appendFolder, ref moreParams, out error, out log);
        }

        public void RenameIncomeFile(string filename, string newFilename, string appendFolder,
            ref string moreParams, out bool error, out string log)
        {
            _inner.RenameIncomeFile(filename, newFilename, appendFolder, ref moreParams, out error, out log);
        }

        public void MoveIncomeFileToDir(string filename, string renameFilename, string appendFolder,
            string targetDir, ref string moreParams, out bool error, out string log)
        {
            _inner.MoveIncomeFileToDir(filename, renameFilename, appendFolder, targetDir, ref moreParams, out error, out log);
        }

        public void PurgeOldFiles(int keepDays = 14) {  }

    }

    public sealed class SftpDcaManagerShim : IDcaManagerShim
    {
        private readonly PartnerSftpConfig _config;
        private readonly int _tenant;
        private readonly SFTPDeleteTempFilesService _sftpDeleteTempFiles;
        private const string HistoryDir = "History";
        private const string OrphanDir = "Orphan";


        public SftpDcaManagerShim(PartnerSftpConfig config, int tenant)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
            _tenant = tenant;
            _sftpDeleteTempFiles = new SFTPDeleteTempFilesService(_tenant, _config.Host);
        }

        private bool EnsureDirExists(SFTPService sftp, string sub)
        {
            var path = CombinePath(_config.RemotePath ?? "/", sub);
            if (TryChangeDir(sftp, path)) return true;
            return TryMakeDir(sftp, path) && TryChangeDir(sftp, path);
        }
        private void SafeRename(SFTPService sftp, string src, string dst, ref string more,
                        out string status, out string message)
        {
            sftp.Rename(src, dst, "1", ref more, out status, out message);
        }

        private static DateTime GetTimeStamp(string file)
        {
            // matches yyyy-MM-dd_HH-mm-ss inside name
            var m = System.Text.RegularExpressions.Regex.Match(
                file, @"\d{4}-\d{2}-\d{2}_\d{2}-\d{2}-\d{2}");
            DateTime dt;
            return m.Success && DateTime.TryParseExact(
                       m.Value, "yyyy-MM-dd_HH-mm-ss",
                       System.Globalization.CultureInfo.InvariantCulture,
                       System.Globalization.DateTimeStyles.AssumeUniversal,
                       out dt)
                   ? dt : DateTime.MaxValue;
        }

        public List<string> FileListing(string searchPattern, string appendFolder, ref string moreParams,
            out bool error, out string log)
        {
            error = false;
            log = "";
            var files = new List<string>();

            try
            {
                using (var sftp = new SFTPService(_sftpDeleteTempFiles))
                {
                    string status, message;
                    var remotePath = CombinePath(_config.RemotePath, appendFolder);

                    SftpLogin(sftp, remotePath, out status, out message);
                    if (status != "0")
                    {
                        error = true;
                        log = message;
                        return files;
                    }

                    files = sftp.DirList(searchPattern, true, false, out status, out message);

                    if (status != "0" && string.IsNullOrWhiteSpace(status))
                    {
                        error = true;
                        log = message;
                    }

                    files = files.Where(f => !f.EndsWith(" <dir>")).ToList();

                    // Logout
                    sftp.Logoff(ref moreParams, out status, out message);
                }
            }
            catch (Exception ex)
            {
                error = true;
                log = $"SFTP FileListing error: {ex.Message}";
            }

            return files;
        }

        public string GetContentsBASE64OfDownloadIncomeFile(string filename, string appendFolder,
            ref string moreParams, out bool error, out string log)
        {
            error = false;
            log = "";
            string base64Content = "";

            try
            {
                using (var sftp = new SFTPService(_sftpDeleteTempFiles))
                {
                    string status, message;
                    var remotePath = CombinePath(_config.RemotePath, appendFolder);

                    SftpLogin(sftp, remotePath, out status, out message);

                    if (status != "0")
                    {
                        error = true;
                        log = message;
                        return base64Content;
                    }

                    var fileData = sftp.DownloadFile(filename, out status, out message);

                    if (status == "1" && fileData != null)
                    {
                        base64Content = Convert.ToBase64String(fileData);
                    }
                    else
                    {
                        error = true;
                        log = message;
                    }
                    string msg;   
                    EnsureDirExists(sftp, HistoryDir);
                    var historyPath = CombinePath(HistoryDir, filename);
                    SafeRename(sftp, filename, historyPath, ref moreParams, out status, out msg);

                    // Logout
                    sftp.Logoff(ref moreParams, out status, out message);
                }
            }
            catch (Exception ex)
            {
                error = true;
                log = $"SFTP download error: {ex.Message}";
            }

            return base64Content;
        }
        public void PurgeOldFiles(int keepDays = 14)
        {
            DateTime threshold = DateTime.UtcNow.AddDays(-keepDays);

            string dummyMore = ""; bool err; string log;

            // History
            var hist = FileListing("*", HistoryDir, ref dummyMore, out err, out log);
            Purge(hist, HistoryDir, threshold);

            // Orphan
            var orph = FileListing("*", OrphanDir, ref dummyMore, out err, out log);
            Purge(orph, OrphanDir, threshold);
        }

        private void Purge(IEnumerable<string> list, string subDir, DateTime threshold)
        {
            int deleted = 0;
            foreach (var file in list)
            {
                if (GetTimeStamp(file) < threshold)
                {
                    bool err;
                    string log;
                    string mp = "";
                    DeleteIncomeFile(file, subDir, ref mp, out err, out log);
                    if (!err) deleted++;
                }
            }
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(
                $"Purged {deleted} files from {subDir} older than {threshold}");
        }

        private bool TryChangeDir(SFTPService sftp, string path)
        {
            string st, msg, more = "";
            sftp.ChangeDir(path, ref more, out st, out msg);
            return st == "0";
        }

        private bool TryMakeDir(SFTPService sftp, string path)
        {
            try
            {
                var save = sftp; 
                sftp.Upload("dummy.tmp", new byte[0], true, false, out var st, out var msg); 
                return st == "0" || st == "-2";
            }
            catch { return false; }
        }
        public void DeleteIncomeFile(string filename, string appendFolder, ref string moreParams,
            out bool error, out string log)
        {
            error = false;
            log = "";

            try
            {
                using (var sftp = new SFTPService(_sftpDeleteTempFiles))
                {
                    string status, message;
                    var remotePath = CombinePath(_config.RemotePath, appendFolder);

                    SftpLogin(sftp, remotePath, out status, out message);

                    if (status != "0")
                    {
                        error = true;
                        log = message;
                        return;
                    }

                    // Delete file using your existing method
                    sftp.DeleteFile(filename, out status, out message);

                    if (status != "0")
                    {
                        error = true;
                    }
                    log = message;

                    // Logout
                    sftp.Logoff(ref moreParams, out status, out message);
                }
            }
            catch (Exception ex)
            {
                error = true;
                log = $"SFTP delete error: {ex.Message}\n{ex.StackTrace}";
            }
        }

        public void RenameIncomeFile(string filename, string newFilename, string appendFolder,
            ref string moreParams, out bool error, out string log)
        {
            error = false;
            log = "";

            try
            {
                using (var sftp = new SFTPService(_sftpDeleteTempFiles))
                {
                    string status, message;
                    var remotePath = CombinePath(_config.RemotePath, appendFolder);

                    SftpLogin(sftp, remotePath, out status, out message);

                    if (status != "0")
                    {
                        error = true;
                        log = message;
                        return;
                    }

                    // Rename file using your existing method
                    sftp.Rename(filename, newFilename, "T", ref moreParams, out status, out message);

                    if (status != "0")
                    {
                        error = true;
                    }
                    log = message;

                    // Logout
                    sftp.Logoff(ref moreParams, out status, out message);
                }
            }
            catch (Exception ex)
            {
                error = true;
                log = $"SFTP rename error: {ex.Message}";
            }
        }

        public void MoveIncomeFileToDir(string filename, string renameFilename, string appendFolder,
            string targetDir, ref string moreParams, out bool error, out string log)
        {
            error = false;
            log = "";

            try
            {
                using (var sftp = new SFTPService(_sftpDeleteTempFiles))
                {
                    string status, message;
                    var remotePath = CombinePath(_config.RemotePath, appendFolder);

                    SftpLogin(sftp, remotePath, out status, out message);

                    if (status != "0")
                    {
                        error = true;
                        log = message;
                        return;
                    }

                    // Move file using rename
                    var targetPath = CombinePath(targetDir, string.IsNullOrEmpty(renameFilename) ? filename : renameFilename);
                    sftp.Rename(filename, targetPath, "T", ref moreParams, out status, out message);

                    if (status != "0")
                    {
                        error = true;
                    }
                    log = message;

                    // Logout
                    sftp.Logoff(ref moreParams, out status, out message);
                }
            }
            catch (Exception ex)
            {
                error = true;
                log = $"SFTP move error: {ex.Message}";
            }
        }

        private string CombinePath(string path1, string path2)
        {
            if (string.IsNullOrWhiteSpace(path2))
                return path1;

            var separator = "/";
            path1 = string.IsNullOrWhiteSpace(path1)
                ? "/"
                : path1.TrimEnd('/', '\\');

            path2 = path2?.TrimStart('/', '\\') ?? "";

            return string.IsNullOrWhiteSpace(path1)
                ? path2
                : $"{path1}{separator}{path2}";
        }


       private void SftpLogin(SFTPService sftp, string remotePath, out string status, out string message)
        {
            if (_config.UsePrivateKey)
            {
                sftp.LogonWithKey(_config.Host, _config.Username, _config.PrivateKeyPath,
                                  _config.Port.ToString(), remotePath, out status, out message);
            }
            else
            {
                sftp.Logon(_config.Host, _config.Username, _config.Password,
                           _config.Port.ToString(), remotePath, out status, out message);
            }
        }
    }
    public class PartnerSftpConfig
    {
        public string Host { get; set; }
        public int Port { get; set; } = 22;
        public string Username { get; set; }
        public string Password { get; set; }
        public string PrivateKeyPath { get; set; }
        public bool UsePrivateKey { get; set; }
        public string RemotePath { get; set; } = "/";
    }
}