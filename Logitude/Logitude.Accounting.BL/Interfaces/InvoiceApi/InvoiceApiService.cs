using CHAMP17;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;

namespace Logitude.Accounting.BL.Interfaces.Magaya
{
    public class InvoiceApiService
    {
     //   private CSSoapServiceClient _client;
        private int _accessKey;

        public bool OpenConnection(string user, string password)
        {
            try
            {
                //NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"OpenConnection Started: user: {user},password: {password} ");

                //_client = new CSSoapServiceClient();
                //int key;
                //api_session_error result = _client.StartSession(user, password, out key);
                //if (result == api_session_error.no_error)
                //{
                //    _accessKey = key;
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                return true;
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"OpenConnection Exception: {ex.Message}");
                return false;
            }
        }

        public bool EndSession()
        {
            try
            {
                //if (_client == null)
                //    throw new InvalidOperationException("Session not started.");

                //api_session_error result = _client.EndSession(_accessKey);
                //return result == api_session_error.no_error;
                return true;
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"EndSession Exception: {ex.Message}");
                return false;
            }
        }

        public (bool Success, string LogXml) QueryLog(
            string startDate,
            string endDate,
            int logEntryType,
            string transType,
            int flags)
        {
            try
            {
                //if (_client == null)
                //    throw new InvalidOperationException("Session not started.");

                //string transListXml;
                //api_session_error result = _client.QueryLog(
                //    _accessKey,
                //    startDate,
                //    endDate,
                //    logEntryType,
                //    transType,
                //    flags,
                //    out transListXml);

                //if (result == api_session_error.no_error)
                //{
                //    return (true, transListXml);
                //}
                //else
                //{
                //        return (false, null);
                //}
                return (false, null);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"QueryLog Exception: {ex.Message}");
                return (false, null);
            }
        }

        public (bool Success, string TransactionXml) GetTransaction(
    string type,
    int flags,
    string number)
        {
            try
            {
                //if (_client == null)
                //    throw new InvalidOperationException("Session not started.");

                //string transXml;
                //api_session_error result = _client.GetTransaction(
                //    _accessKey,
                //    type,
                //    flags,
                //    number,
                //    out transXml);

                //if (result == api_session_error.no_error)
                //{
                //    return (true, transXml);
                //}
                //else
                //{
                //    return (false, null);
                //}
                return (false, null);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"GetTransaction Exception: {ex.Message}");
                return (false, null);
            }
        }





    }
}
