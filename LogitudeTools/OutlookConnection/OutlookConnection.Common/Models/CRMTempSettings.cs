using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;


using OutlookConnection.Common.Contracts;
using OutlookConnection.Common.LoginWcfServiceReference;
//using OutlookConnection.Common.Contracts;

namespace OutlookConnection.Common.Models
{



    public class CRMTempSettings : INotifyPropertyChanged, ICRMTempSettings
    {
       

        string _user;
        string _serverURL;
        string _password;
        string _token;
        TenantInfo _MyTenantInfo;
        string _accountID;
        bool _enableAddin;
        int _enableNotification;
        int _enableAutoSync;
        int _syncNumberOfDays;
        string _StoreID;






        public string User
        {
            get { return (_user ?? "").ToLower(); }
            set
            {
                var lower = (value ?? "").ToLower();
                if (lower == _user) return;
                _user = lower;
                OnPropertyChanged("User");
            }
        }



        public string StoreID
        {
            get { return _StoreID; }
            set
            {

                //check Online
                if (value == _StoreID)
                    return;

                _StoreID = value;
                OnPropertyChanged("StoreID");

            }
        }




        public string Password
        {
            get { return _password; }
            set
            {
                // check password is OK
                if (value == _password)
                    return;

                _password = value;
                OnPropertyChanged("Password");
            }
        }

        public string ServerURL
        {
            get { return _serverURL; }
            set
            {

                //check Online
                if (value == _serverURL)
                    return;

                _serverURL = value;
                OnPropertyChanged("ServerURL");

            }
        }

        public string Token
        {
            get { return _token; }
            set
            {
                //save  incypt Token
                if (value == _token)
                    return;

                _token = value;
                OnPropertyChanged("Token");

            }
        }

        public TenantInfo MyTenantInfo
        {
            get { return _MyTenantInfo = _MyTenantInfo ?? new TenantInfo(); }
            set
            {
                //List of tenant?
                if (value == _MyTenantInfo)
                    return;

                _MyTenantInfo = value;
                OnPropertyChanged("MyTenantInfo");

            }
        }



        public string AccountID
        {
            get { return _accountID; }
            set
            {
                //List of tenant?
                if (value == _accountID)
                    return;

                _accountID = value;
                OnPropertyChanged("AccountID");

            }
        }


        public bool EnableAddin
        {
            get { return _enableAddin; }
            set
            {
                //List of tenant?
                if (value == _enableAddin)
                    return;

                _enableAddin = value;
                OnPropertyChanged("EnableAddin");

            }
        }

        public int EnableNotification
        {
            get { return _enableNotification; }
            set
            {
                //List of tenant?
                if (value == _enableNotification)
                    return;

                _enableNotification = value;
                OnPropertyChanged("EnableNotification");

            }
        }

        public int EnableAutoSync
        {
            get { return _enableAutoSync; }
            set
            {
                //List of tenant?
                if (value == _enableAutoSync)
                    return;

                _enableAutoSync = value;
                OnPropertyChanged("EnableAutoSync");

            }
        }

        public int SyncNumberOfDays
        {
            get { return _syncNumberOfDays; }
            set
            {
                //List of tenant?
                if (value == _syncNumberOfDays)
                    return;

                _syncNumberOfDays = value;
                OnPropertyChanged("SyncNumberOfDays");

            }
        }





        void CheckUP()
        {

        }


        public event PropertyChangedEventHandler PropertyChanged;

        void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = this.PropertyChanged;
            if (handler != null)
                handler(this, new PropertyChangedEventArgs(propertyName));
        }


    }



}
