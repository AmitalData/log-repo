using Logitude.Accounting.BL.MagayaRef;
using Logitude.Accounting.BL.Utils;
using Logitude.Accounting.Data.Repositories;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace Logitude.Accounting.BL.Interfaces.Magaya
{
    public class InvoiceApiService
    {
        private CSSoapService _client;
        private int _accessKey;

        public bool OpenConnection()
        {
            try
            {
                var user = ConfigurationManager.AppSettings["MagayaUser"];
                var password = ConfigurationManager.AppSettings["MagayaPassword"];
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo($"OpenConnection Started: user: {user},password: {password} ");
                if(string.IsNullOrEmpty(user) ||  string.IsNullOrEmpty(password))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError("OpenConnection Failed: User or Password is not configured.");
                    return false;
                }
                _client = new CSSoapService();
                int key;
                api_session_error result = _client.StartSession(user, password, out key);
                if (result == api_session_error.no_error)
                {
                    _accessKey = key;
                    return true;
                }
                else
                {
                    return false;
                }
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
                if (_client == null)
                    throw new InvalidOperationException("Session not started.");

                api_session_error result = _client.EndSession(_accessKey);
                return result == api_session_error.no_error;
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
                if (_client == null)
                    throw new InvalidOperationException("Session not started.");
                string trans_list_xml;
                api_session_error result = _client.QueryLog(
                    _accessKey,
                    startDate,
                    endDate,
                    logEntryType,
                    transType,
                    flags,
                    out trans_list_xml);

                if (result == api_session_error.no_error)
                {
                    return (true, trans_list_xml);
                }
                else
                {
                    return (false, null);
                }
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
                if (_client == null)
                    throw new InvalidOperationException("Session not started.");

                string transXml;
                api_session_error result = _client.GetTransaction(
                    _accessKey,
                    type,
                    flags,
                    number,
                    out transXml);

                if (result == api_session_error.no_error)
                {
                    return (true, transXml);
                }
                else
                {
                    return (false, null);
                }
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"GetTransaction Exception: {ex.Message}");
                return (false, null);
            }
        }


        public bool ReSendQueue(string  id,int tenant)
        {
            InvoiceApiQueryBatch invoiceApiQueryBatch = new InvoiceApiQueryBatch();
            try
            {
                InvoiceApiCommunicationLogRepository invoiceApiCommunicationLogRepository = new InvoiceApiCommunicationLogRepository(tenant);
               var communicationLog= invoiceApiCommunicationLogRepository.GetSingle(id, tenant);
                if(!string.IsNullOrEmpty(communicationLog?.Exception))
                {
                    var dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(communicationLog?.Exception);
                    if (dict != null && dict.ContainsKey("MessageValues"))
                    {
                        Dictionary<string, string> messageValues = null;
                        if (dict["MessageValues"] is JObject jObject)
                        {
                            messageValues = jObject.ToObject<Dictionary<string, string>>();
                        }
                        else if (dict["MessageValues"] is Dictionary<string, string> directDict)
                        {
                            messageValues = directDict;
                        }
                        if (messageValues != null)
                        {
                            invoiceApiQueryBatch.SaveInvoiceApiInvoiceInQueue(messageValues, tenant);
                            return true;
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"ReSendQueue Exception: {ex.Message}");
                return false;
            }
        }



    }
}
