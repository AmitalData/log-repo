using Logitude.Accounting.BL.CloseTables;
using Logitude.Accounting.BL.MagayaRef;
using Logitude.Accounting.BL.Utils;
using Logitude.Accounting.Data.Repositories;
using Logitude.BL.Helpers;
using System;
using System.Collections.Generic;

namespace Logitude.Accounting.BL.Interfaces.Magaya
{
    public class InvoiceApiService
    {
        private CSSoapService helper;
        private int _accessKey;

        public void OpenConnection(int tenant)
        {
            try
            {
                var user = DefaultService.Instance.Get(tenant, "Magaya", "Magaya")?.Value1;
                var password = DefaultService.Instance.Get(tenant, "Magaya", "Magaya")?.Value2;
                NetCommonHelper.Logger.DevLog.Instance.WriteInfo("OpenConnection Started");
                if(string.IsNullOrEmpty(user) ||  string.IsNullOrEmpty(password))
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteError("OpenConnection Failed: User or Password is not configured.");
                    throw new InvalidOperationException("OpenConnection Failed: User or Password is not configured");
                }
                helper = new CSSoapService();
                int key;
                api_session_error result = helper.StartSession(user, password, out key);
                if (result == api_session_error.no_error)
                {
                    _accessKey = key;
                   
                }
                else
                {
                    throw new InvalidOperationException($"OpenConnection Failed: {result}");
                }
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"OpenConnection Exception: {ex.Message}");
                throw ex;
            }
        }

        public void EndSession()
        {
            try
            {
                EnsureSessionStarted();

                api_session_error result = helper.EndSession(_accessKey);
                if(!(result == api_session_error.no_error))
                {
                    throw new InvalidOperationException($"EndSession Failed: {result}");
                }
              
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"EndSession Exception: {ex.Message}");
                throw ex;
            }
        }

        public  string  QueryLog(
            string startDate,
            string endDate,
            int logEntryType,
            string transType,
            int flags)
        {
            try
            {
                EnsureSessionStarted();
                string trans_list_xml;
                api_session_error result = helper.QueryLog(
                    _accessKey,
                    startDate,
                    endDate,
                    logEntryType,
                    transType,
                    flags,
                    out trans_list_xml);

                if (result == api_session_error.no_error)
                {
                    return trans_list_xml;
                }
                else
                {
                    throw new InvalidOperationException($"QueryLog Failed: {result}");
                }
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"QueryLog Exception: {ex.Message}");
                throw ex;
            }
        }

        public  string  GetTransaction(
    string type,
    int flags,
    string number)
        {
            try
            {
                EnsureSessionStarted();
                string transXml;
                api_session_error result = helper.GetTransaction(
                    _accessKey,
                    type,
                    flags,
                    number,
                    out transXml);

                if (result == api_session_error.no_error)
                {
                    return transXml;
                }
                else
                {
                    throw new InvalidOperationException($"GetTransaction Failed: {result}");
                }
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"GetTransaction Exception: {ex.Message}");
                throw ex;
            }
        }


        public void ReSendQueue(string  id,int tenant)
        {
            InvoiceApiQueryBatch invoiceApiQueryBatch = new InvoiceApiQueryBatch();
            try
            {
                InvoiceApiCommunicationLogRepository invoiceApiCommunicationLogRepository = new InvoiceApiCommunicationLogRepository(tenant);
               var communicationLog= invoiceApiCommunicationLogRepository.GetSingle(id, tenant);
                communicationLog.StatusCode  = InvoiceApiStatusEnum.Pending;
                        var messageBody = new Dictionary<string, string>
                       {
                        { "Guid", communicationLog?.ExternalID },
                        { "InvoiceApiId", communicationLog?.Id},
                        };
                      invoiceApiQueryBatch.SaveInvoiceApiInvoiceInQueue(messageBody, tenant);
                      
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteError($"ReSendQueue Exception: {ex.Message}");
                throw ex;
            }
        }

        private void EnsureSessionStarted()
        {
            if (helper == null)
                throw new InvalidOperationException("Session not started.");
        }



    }
}
