using Logitude.Server.Tools;
using Microsoft.WindowsAzure.Storage.Queue;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Azure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using WebFreight.Web.InfrastructureModel;

namespace WebFreight.Web.Helpers
{
    public class SignUpHelper
    {
        public void SendMessageToQueue(byte[] msg, SignUpInfoClass signupInfo)
        {
            Setting setting;
            using (TransactionScope setScope = TransactionFactory.GetNewTransaction(new TimeSpan(2, 0, 0)))//new TransactionScope(TransactionScopeOption.RequiresNew, new TimeSpan(2, 0, 0)))
            {
                SettingRepository settingRepository = new SettingRepository();
                setting = settingRepository.GetSingleSetting("1");
                setScope.Complete();
            }
            try
            {
                SignUpClass.StartSignUp(signupInfo);
            }
            catch (Exception ex)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteFatal(ex, $"Failed creating new tenant: {signupInfo?.Name}, error: {ex.Message}");
                throw;
            }
        }
    }
}