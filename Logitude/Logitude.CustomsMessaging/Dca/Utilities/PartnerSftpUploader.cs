using Logitude.Customs.BL.CloseTables;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools.FTP;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Dca.Utilities
{
    public class PartnerSftpUploader
    {
        private readonly FTPDetail _ftpDetail;
        public PartnerSftpUploader(FTPDetail ftpDetail)
        {
            _ftpDetail = ftpDetail ?? throw new ArgumentNullException(nameof(ftpDetail));
        }
        public void UploadFile(string localFilePath, string remoteFileName, bool deleteIfExist = true, bool uploadAsTemp = false)
        {
            if (string.IsNullOrWhiteSpace(localFilePath))
                throw new ArgumentException("localFilePath cannot be null or empty");
            if (!File.Exists(localFilePath))
                throw new FileNotFoundException($"File not found: {localFilePath}");

            byte[] fileData = File.ReadAllBytes(localFilePath);
            string fileName = Path.GetFileName(localFilePath);
            UploadBytes(fileName, fileData, deleteIfExist, uploadAsTemp);
        }

        public void UploadBytes(string fileName, byte[] fileData, bool deleteIfExist = true, bool uploadAsTemp = false)
        {
            if (fileData == null || fileData.Length == 0)
                throw new ArgumentException("fileData cannot be null or empty");
            if (string.IsNullOrWhiteSpace(fileName))
                throw new ArgumentException("fileName cannot be null or empty");

            string status, message;
            string port = "22";
            string remotePath = _ftpDetail.Folder;

            using (var sftp = new SFTPService())
            {
                // Logon
                sftp.Logon(_ftpDetail.Host, _ftpDetail.UserName, _ftpDetail.Password, port, remotePath, out status, out message);
                if (status != "0")
                    throw new Exception($"SFTP logon failed: {message}");

                // Upload
                sftp.Upload(fileName, fileData, deleteIfExist, uploadAsTemp, out status, out message);
                if (status != "0")
                    throw new Exception($"SFTP upload failed: {message}");

                // Logoff
                string more = "";
                sftp.Logoff(ref more, out status, out message);
            }
        }
    }
}