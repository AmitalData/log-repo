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
        static readonly Dictionary<string, DateTime?> _HostLastDeleteAt;
        static readonly Dictionary<int, bool> _TenantFeatureExist;
        static readonly object _locker;

        private string _ftpHost;
        private int _tenant = -1;




        static SFTPDeleteTempFilesService()
        {
            _locker = new object();
            _HostLastDeleteAt = new Dictionary<string, DateTime?>();
            _TenantFeatureExist = new Dictionary<int, bool>();
        }

        public SFTPDeleteTempFilesService(int tenant, string ftpHost)
        {
            try
            {

                lock (_locker)
                {

                    _tenant = tenant;
                    _ftpHost = ftpHost;
                    if (!_TenantFeatureExist.ContainsKey(tenant))
                    {
                        bool exist = true;
                        _TenantFeatureExist[tenant] = exist;
                    }

                    if (!_HostLastDeleteAt.ContainsKey(_ftpHost))
                    {
                        _HostLastDeleteAt.Add(_ftpHost, DateTime.MinValue);
                    }
                }
            }
            catch (Exception e)
            {

                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
                //throw;
            }
        }
        /// <summary>
        /// // ENSURE ALL TEMP FILES DELETED - HAPPEN EVERY 10 MIN -IF uploadAsTemp && HasFeatureToggle[SFD]
        /// </summary>
        /// <param name="actionDeleteTempFiles"></param>
        /// <returns></returns>
        internal bool DeleteIfNeeded(bool uploadAsTemp ,Action actionDeleteTempFiles)
        {
            try
            {
                if (!uploadAsTemp)
                {
                    return false;
                }
                lock (_locker)
                {
                    bool tenantFeatureExist = false;
                    _TenantFeatureExist.TryGetValue(_tenant, out tenantFeatureExist);
                    if (!tenantFeatureExist)
                    {
                        Debug.WriteLine($"SFTPDeleteTempFilesService:does not  HasFeatureToggle -SFD");
                        return false;
                    }
                    ///Debug.WriteLine($"HasFeatureToggle -SFD ..");
                    DateTime? lastDeleteAt = null;
                    _HostLastDeleteAt.TryGetValue(_ftpHost, out lastDeleteAt);
                    lastDeleteAt = lastDeleteAt ?? DateTime.MinValue;
                    if (DateTime.Now.Subtract(lastDeleteAt.GetValueOrDefault()) > TimeSpan.FromMinutes(DELETE_EveryMin))
                    {
                        
                        _HostLastDeleteAt[_ftpHost] = DateTime.Now;
                    }
                    else
                    {
                        Debug.WriteLine($"SFTPDeleteTempFilesService:wait... (DELETE_EveryMin:{DELETE_EveryMin}) ");
                        return false;
                    }
                }
                Debug.WriteLine($"SFTPDeleteTempFilesService:actionDeleteTempFiles(DELETE_EveryMin:{DELETE_EveryMin}) ...");
                actionDeleteTempFiles?.Invoke();
                return true;
            }
            catch (Exception e)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(e);
                return false;
            }
        }

        
    }
}
