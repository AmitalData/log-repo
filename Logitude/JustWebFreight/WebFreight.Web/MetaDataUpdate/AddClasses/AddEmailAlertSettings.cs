using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddEmailAlertSettings
    {
        public static void AddEmailAlertSetting(EmailAlertSettingDetails alertDetails, EmailAlertSettingRepository alertRepository, Dictionary<string, EmailAlertSetting> tenantAlerts)
        {
            if (tenantAlerts.Keys.Contains(alertDetails.Code))
            {
                EmailAlertSetting alert = tenantAlerts[alertDetails.Code];
                alert.ObjectTableId = alertDetails.ObjectTableId;
                alert.Tenant = alertDetails.Tenant;
                alert.SettingLevelCode = alertDetails.SettingLevelCode;
                alert.To = alertDetails.To;
                alert.InActive = alertDetails.InActive;
                alert.Description = alertDetails.Description;
                alert.IndexOrder = alertDetails.IndexOrder;
                alertRepository.Update(alert);
            }
            else
            {
                EmailAlertSetting newAlert = new EmailAlertSetting()
                {
                    Tenant = alertDetails.Tenant,
                    ObjectTableId = alertDetails.ObjectTableId,
                    Code = alertDetails.Code,
                    Id = IdCounter.GetNumber("EmailAlertSetting", alertDetails.Tenant).ToString(),
                    SettingLevelCode = alertDetails.SettingLevelCode,
                    To = alertDetails.To,
                    InActive = alertDetails.InActive,
                    Description = alertDetails.Description,
                    IndexOrder = alertDetails.IndexOrder,
                };

                alertRepository.Add(newAlert);
            }
        }
    }
}