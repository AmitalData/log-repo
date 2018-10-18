using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebFreight.Web.Helpers
{
    public class ConnectionDetails : INotifyPropertyChanged
    {
        private string userName;
        [Required]
        public string UserName
        {
            get { return userName; }
            set
            {
                userName = value;
                if (string.IsNullOrEmpty(userName))
                {
                    throw new Exception("User name is required!");
                }
            }
        }
        private string password;
        [Required]
        public string Password
        {
            get { return password; }
            set
            {
                password = value;
                if (string.IsNullOrEmpty(password))
                {
                    throw new Exception("Password is required!");
                }
            }
        }

        private string server;
        [Required]
        public string Server
        {
            get { return server; }
            set
            {
                server = value;
                if (string.IsNullOrEmpty(server))
                {
                    throw new Exception("Server name is required!");
                }
            }
        }

        private string dataBase;
        [Required]
        public string DataBase
        {
            get { return dataBase; }
            set
            {
                dataBase = value;
                if (string.IsNullOrEmpty(BlobName))
                {
                    BlobName = !string.IsNullOrEmpty(dataBase) ? ("textcodes" + "_" + String.Format("{0:dd_MM_yyyy}", DateTime.Now)) : BlobName;
                }
                if (string.IsNullOrEmpty(dataBase))
                {
                    throw new Exception("Database name is required!");
                }
            }
        }

        //private string _DataBaseToRestore;

        //public string DataBaseToRestore
        //{
        //    get { return _DataBaseToRestore; }
        //    set
        //    {
        //        _DataBaseToRestore = value;
        //        if (string.IsNullOrEmpty(_DataBaseToRestore))
        //        {
        //            throw new Exception("Database name is required!");
        //        }
        //    }
        //}

        private string blobName;
        [Required]
        public string BlobName
        {
            get
            {
                return blobName;
            }
            set
            {
                blobName = value;
                OnPropertyChanged("BlobName");
                if (string.IsNullOrEmpty(blobName))
                {
                    throw new Exception("Blob name is required!");
                }
            }
        }

       
        public bool IsTextCodesExport { get; set; }
        public bool IsTextCodesImport { get; set; }
        //public string DataBase_To_Restore { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(property));
        } 
    }
}