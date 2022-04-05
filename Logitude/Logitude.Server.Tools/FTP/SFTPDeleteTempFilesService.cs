using Logitude.Server.Tools.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.FTP
{
    public class SFTPDeleteTempFilesService
    {

        //INSERT INTO "TOGGLES" (CODE, NAME, SEARCHFIELDS) VALUES ('SFD', 'SFTPDeleteTempFiles', 'SFTPDeleteTempFiles')
        //INSERT INTO "FEATURETOGGLES"(ID, TENANT, CREATEDATE, CREATEDBYUSERID, UPDATEDATE, UPDATEDBYUSERID, SEARCHFIELDS, TENANTNUMBER, INACTIVE, TOGGLECODE) VALUES('SFD_T1', '1', TO_TIMESTAMP('2022-04-05 14:19:28.729000000', 'YYYY-MM-DD HH24:MI:SS.FF'), '1-9', TO_TIMESTAMP('2022-03-06 14:19:46.456000000', 'YYYY-MM-DD HH24:MI:SS.FF'), '1-9', 'SFD', '1', '0', 'SFD')

        //TENANT=3
        //INSERT INTO "FEATURETOGGLES"(ID, TENANT, CREATEDATE, CREATEDBYUSERID, UPDATEDATE, UPDATEDBYUSERID, SEARCHFIELDS, TENANTNUMBER, INACTIVE, TOGGLECODE) 
        //VALUES('SFD_T3', '3', TO_TIMESTAMP('2022-04-05 14:19:28.729000000', 'YYYY-MM-DD HH24:MI:SS.FF'), '1-9', TO_TIMESTAMP('2022-03-06 14:19:46.456000000', 'YYYY-MM-DD HH24:MI:SS.FF'), '1-9', 'SFD', '3', '0', 'SFD')


        const int DELETE_EveryMin = 10;//change to 20
        static readonly Dictionary<string, DateTime?> _HostLastDelete;
        static readonly Dictionary<int, bool> _FeatureExist;
        static readonly object _locker;

        private string _ftpHost;
        private int _tenant = -1;




        static SFTPDeleteTempFilesService()
        {
            _locker = new object();
            _HostLastDelete = new Dictionary<string, DateTime?>();
            _FeatureExist = new Dictionary<int, bool>();
        }

        public SFTPDeleteTempFilesService(int tenant, string ftpHost)
        {
            try
            {

                lock (_locker)
                {

                    _tenant = tenant;
                    _ftpHost = ftpHost;
                    if (!_FeatureExist.ContainsKey(tenant))
                    {
                        bool exist = Server.Tools.Helpers.FeatureToggleHelper.HasFeatureToggle("SFD", tenant);
                        Debug.WriteLine($"HasFeatureToggle(t)[SFD]={exist }");
                        _FeatureExist[tenant] = exist;
                    }

                    if (!_HostLastDelete.ContainsKey(_ftpHost))
                    {
                        _HostLastDelete.Add(_ftpHost, DateTime.MinValue);
                    }
                }
            }
            catch (Exception e)
            {

                Logger.LogMe(e.ToString(), true, "SFTP");
                //throw;
            }
        }

        internal bool DeleteIfNeeded(Action actionDeleteTempFiles)
        {
            try
            {
                lock (_locker)
                {

                    if (_FeatureExist[_tenant])
                    {
                        Debug.WriteLine($"no HasFeatureToggle -SFD");
                        return false;
                    }

                    DateTime last = _HostLastDelete[_ftpHost] ?? DateTime.MinValue;
                    if (DateTime.Now.Subtract(last) > TimeSpan.FromMinutes(DELETE_EveryMin))
                    {
                        
                        _HostLastDelete[_ftpHost] = DateTime.Now;
                    }
                    else
                    {
                        return false;
                    }
                }
                Debug.WriteLine($"actionDeleteTempFiles(DELETE_EveryMin:{DELETE_EveryMin}) ...");
                actionDeleteTempFiles?.Invoke();
                return true;
            }
            catch (Exception e)
            {
                Logger.LogMe(e.ToString(), true, "SFTP");
                return false;
            }
        }

        
    }
}
