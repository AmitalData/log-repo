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
            string status, message;
            var markerRelPath = CombinePath(sub ?? "", ".keep");   // e.g. "History/.keep" or "History/a/b/.keep"
            sftp.Upload(markerRelPath, Array.Empty<byte>(), /*createDirs*/ true, /*overwrite*/ true, out status, out message);
            return status == "0" || status == "1";

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

                    if (status != "0")
                    {
                        error = true;
                        log = message;
                    }

                    files = files.Where(f => !f.EndsWith(" <dir>")).ToList();
                    files = files
                        .Where(n => !string.IsNullOrWhiteSpace(n))
                        .Select(n => n.Trim())
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                        .OrderBy(n => n, StringComparer.OrdinalIgnoreCase)
                        .ToList();

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


        public string GetContentsBASE64OfDownloadIncomeFile(
    string filename, string appendFolder, ref string moreParams,
    out bool error, out string log)
        {
            error = false; log = ""; string base64 = "";

            try
            {
                using (var sftp = new SFTPService(_sftpDeleteTempFiles))
                {
                    string status, message;
                    var remotePath = CombinePath(_config.RemotePath, appendFolder);

                    SftpLogin(sftp, remotePath, out status, out message);
                    if (status != "0") { error = true; log = message; return base64; }

                    // Download (DownloadFile returns "1" on success in this lib)
                    var fileBytes = sftp.DownloadFile(filename, out status, out message);
                    if (status == "1" && fileBytes != null)
                    {
                        fileBytes = NormalizeToUtf8NoBom(fileBytes); // <-- keep!
                        base64 = Convert.ToBase64String(fileBytes);
                    }
                    else
                    {
                        error = true; log = message;
                    }

                    sftp.Logoff(ref moreParams, out status, out message);
                }
            }
            catch (Exception ex)
            {
                error = true; log = $"SFTP download error: {ex.Message}";
            }
            return base64;
        }


        public void PurgeOldFiles(int keepDays = 14)
        {
            DateTime threshold = DateTime.UtcNow.AddDays(-keepDays);

            string dummyMore = ""; bool err; string log;

            var hist = FileListing("*", HistoryDir, ref dummyMore, out err, out log);
            Purge(hist, HistoryDir, threshold);

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

        public void DeleteIncomeFile(string filename, string appendFolder, ref string moreParams,
    out bool error, out string log)
        {
            error = false; log = "";

            try
            {
                using (var sftp = new SFTPService(_sftpDeleteTempFiles))
                {
                    string status, message;

                    // Work from this base
                    var remoteBase = CombinePath(_config.RemotePath, appendFolder);
                    SftpLogin(sftp, remoteBase, out status, out message);
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(
                        $"SFTP DeleteIncomeFile: login status={status}, msg='{message}', base='{remoteBase}'");

                    if (status != "0") { error = true; log = message; return; }

                    SplitRemotePath(filename, out var srcDir, out var srcFile);
                    var targetRelDir = string.IsNullOrEmpty(srcDir) ? HistoryDir : CombinePath(HistoryDir, srcDir);
                    var targetRelPath = CombinePath(targetRelDir, srcFile);

                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(
                        $"SFTP MOVE to History (no mkdir): base='{remoteBase}', from='{filename}', to='{targetRelPath}'");

                    sftp.Rename(filename, targetRelPath, "T", ref moreParams, out status, out message);

                    if (status != "0" && status != "1")
                    {
                        error = true;
                        log = $"Move to History failed (likely missing folder). " +
                              $"Status={status}, msg='{message}'. " +
                              $"Please create '{CombinePath(remoteBase, HistoryDir)}' on the SFTP server.";
                        NetCommonHelper.Logger.DevLog.Instance.WriteError(log);
                    }
                    else
                    {
                        log = message;
                    }

                    sftp.Logoff(ref moreParams, out status, out message);
                }
            }
            catch (Exception ex)
            {
                error = true; log = $"SFTP delete/move error: {ex.Message}";
            }
        }




        public void RenameIncomeFile(string filename, string newFilename, string appendFolder,
    ref string moreParams, out bool error, out string log)
        {
            error = false; log = "";

            try
            {
                using (var sftp = new SFTPService(_sftpDeleteTempFiles))
                {
                    string status, message;
                    var remotePath = CombinePath(_config.RemotePath, appendFolder);

                    SftpLogin(sftp, remotePath, out status, out message);
                    if (status != "0") { error = true; log = message; return; }

                    sftp.Rename(filename, newFilename, "T", ref moreParams, out status, out message);
                    if (status != "0") error = true;
                    log = message;

                    sftp.Logoff(ref moreParams, out status, out message);
                }
            }
            catch (Exception ex)
            {
                error = true; log = $"SFTP rename error: {ex.Message}";
            }
        }


        public void MoveIncomeFileToDir(string filename, string renameFilename, string appendFolder,
    string targetDir, ref string moreParams, out bool error, out string log)
        {
            error = false; log = "";

            try
            {
                using (var sftp = new SFTPService(_sftpDeleteTempFiles))
                {
                    string status, message;
                    var remoteBase = CombinePath(_config.RemotePath, appendFolder);

                    SftpLogin(sftp, remoteBase, out status, out message);
                    if (status != "0") { error = true; log = message; return; }

                    var toName = string.IsNullOrEmpty(renameFilename) ? filename : renameFilename;
                    var targetRelPath = CombinePath(targetDir, toName);   // RELATIVE target!

                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug(
                        $"SFTP MOVE to '{targetDir}' (no mkdir): base='{remoteBase}', from='{filename}', to='{targetRelPath}'");

                    sftp.Rename(filename, targetRelPath, "T", ref moreParams, out status, out message);

                    if (status != "0" && status != "1")
                    {
                        error = true;
                        log = $"Move to '{targetDir}' failed (likely missing folder). " +
                              $"Status={status}, msg='{message}'. " +
                              $"Create '{CombinePath(remoteBase, targetDir)}' on the SFTP server.";
                        NetCommonHelper.Logger.DevLog.Instance.WriteError(log);
                    }
                    else
                    {
                        log = message;
                    }

                    sftp.Logoff(ref moreParams, out status, out message);
                }
            }
            catch (Exception ex)
            {
                error = true; log = $"SFTP move error: {ex.Message}";
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

        private static void SplitRemotePath(string relativePath, out string dirPart, out string filePart)
        {
            var p = (relativePath ?? "").Replace('\\', '/').Trim('/');
            var idx = p.LastIndexOf('/');
            if (idx < 0) { dirPart = ""; filePart = p; }
            else { dirPart = p.Substring(0, idx); filePart = p.Substring(idx + 1); }
        }


        private static bool HasUtf8Bom(byte[] d) =>
    d != null && d.Length >= 3 && d[0] == 0xEF && d[1] == 0xBB && d[2] == 0xBF;

        private static byte[] StripUtf8Bom(byte[] data)
        {
            if (!HasUtf8Bom(data)) return data ?? Array.Empty<byte>();
            var trimmed = new byte[data.Length - 3];
            Buffer.BlockCopy(data, 3, trimmed, 0, trimmed.Length);
            return trimmed;
        }

        private static bool HasUtf16LeBom(byte[] d) =>
            d != null && d.Length >= 2 && d[0] == 0xFF && d[1] == 0xFE;

        private static bool HasUtf16BeBom(byte[] d) =>
            d != null && d.Length >= 2 && d[0] == 0xFE && d[1] == 0xFF;

        private static byte[] NormalizeToUtf8NoBom(byte[] data)
        {
            if (data == null) return Array.Empty<byte>();
            if (HasUtf8Bom(data)) return StripUtf8Bom(data);
            if (HasUtf16LeBom(data))
                return System.Text.Encoding.Convert(
                    System.Text.Encoding.Unicode, System.Text.Encoding.UTF8, data, 2, data.Length - 2);
            if (HasUtf16BeBom(data))
                return System.Text.Encoding.Convert(
                    System.Text.Encoding.BigEndianUnicode, System.Text.Encoding.UTF8, data, 2, data.Length - 2);
            return data; 
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