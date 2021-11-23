using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Dca
{
    class RemoveOldOrphanedFilesFromBackupService
    {
        internal void RemoveOldFiles(string backupPath)
        {
            var dir = new DirectoryInfo(backupPath);
            var allFileSystemInfos = dir.GetFiles("*", SearchOption.TopDirectoryOnly);
            foreach (FileInfo FileSystemInfo in allFileSystemInfos)
            {
                if (DateTime.Now.Subtract(FileSystemInfo.CreationTime) > TimeSpan.FromDays(7))
                {
                    FileSystemInfo.Delete();
                }
            }
        }
    }
}
