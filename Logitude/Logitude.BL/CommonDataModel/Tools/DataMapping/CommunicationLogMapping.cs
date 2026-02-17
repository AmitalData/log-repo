using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class CommunicationLogMapping
    {
        public static void MapEntity(CommunicationLogPM entityPM, CommunicationLog poco, bool isNewState)
        {
            ContactPM loggedContact = new ContactQuery(entityPM.Tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), entityPM.Tenant);

            if (isNewState)
            {
                poco.CreateDate = entityPM.CreateDate;
                poco.Tenant = entityPM.Tenant;
            }

            poco.CommunicationLogTypeCode = entityPM.CommunicationLogTypeCode;
            poco.CommunicationStatusTypeCode = entityPM.CommunicationStatusTypeCode;
            poco.InOut = entityPM.InOut;
            poco.Subject = entityPM.Subject;
            poco.DocumentOutId = entityPM.DocumentOutId;
            poco.DocumentsFilingId = entityPM.DocumentInId;
            poco.DocumentId = entityPM.DocumentId;
            poco.CreatedByUserId = entityPM.CreatedByUserId;
            poco.CC = entityPM.CC;
            poco.EntityId = entityPM.EntityId;
            poco.ObjectTableId = entityPM.ObjectTableId;
            poco.DoneDate = entityPM.DoneDate;
            poco.To = entityPM.To;
            poco.Retries = entityPM.Retries;
            poco.BCC = entityPM.BCC;
            poco.From = entityPM.From;
            poco.LastStatusDate = entityPM.LastStatusDate;
            poco.EntityReference = entityPM.EntityReference;
            poco.SearchFields = entityPM.BCC + "," + entityPM.CC + "," + entityPM.To + "," + entityPM.From + "," + entityPM.EntityReference + "," + entityPM.Subject;
            poco.NextTryDateTime = entityPM.NextTryDateTime;
            poco.NextTryDateTimeUTC = entityPM.NextTryDateTimeUTC;
            poco.CreateDateUTC = entityPM.CreateDateUTC;
            poco.DoneDateUTC = entityPM.DoneDateUTC;
            poco.LastStatusDateUTC = entityPM.LastStatusDateUTC;
            poco.AWBNumber = entityPM.AWBNumber;
            poco.ReplyToList = entityPM.ReplyToList;
            poco.ChildObjectTableId = entityPM.ChildObjectTableId;
            poco.ChildEntityId = entityPM.ChildEntityId;
            poco.LogSettings = entityPM.LogSettings;
            poco.IsSecured = entityPM.IsBodySecured;
            poco.EmailDeliveryError = entityPM.EmailDeliveryError;
            poco.ResponseDocumentId = entityPM.ResponseDocumentId;
        }
    }
}