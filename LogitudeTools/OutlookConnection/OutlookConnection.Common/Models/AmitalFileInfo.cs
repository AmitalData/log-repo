using OutlookConnection.Common.Utils;
using System.IO;

namespace OutlookConnection.Common.Models
{
    public class AmitalFileInfo
    {
        public string LocalPath { get; set; }
        public string RepositoryPath { get; set; }
        public AmitalFileStatus Status { get; set; }
        public string RepositoryURL { get; set; }


        public bool IsLocked { get; set; }

        public int Tenant { get; set; }

        public string Key { get; set; }

        public static AmitalFileInfo GetFromInfoFile(string LocalPath)
        {


            if (!File.Exists(GetMetadataFile(LocalPath)))
            {
                return null;
            }

            var xml = File.ReadAllText(GetMetadataFile(LocalPath));

            var amitalFileInfo = XmlGenericUtil<AmitalFileInfo>.DeserilazeObject(xml);
            return amitalFileInfo;
        }

        public void SetInfoFile()
        {
            var xml = XmlGenericUtil<AmitalFileInfo>.SerilazeObject(this);

            File.WriteAllText(GetMetadataFile(this.LocalPath), xml);
        }

        private static string GetMetadataFile(string LocalPath)
        {
            var metadataFile = LocalPath + ".AmitalFileInfo";
            return metadataFile;
        }

        //public static string GetLocalFile(string selectedFile)
        // {
        //    return Path.Combine(Application.UserAppDataPath,"AmitalBOS",Path.GetFileName(selectedFile));
        //}
    }
    public enum AmitalFileStatus
    {
        None = 0,
        Ok = 1
    }
}
