using Logitude.Customs.Def.EntityPMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace WebFreight.Web.DataContracts
{


    public class SelectedNotifications
    {


        public List<string> SelectedIds { get; set; }

        public string Status { get; set; }

        public bool IsAllSelected { get; set; }

        public List<string> ExcludedIds;
        public string AssigneToNotificationTypeCode;
        public string IsClosedByAssignee;
        public string DueDate;
        public string AssigneToId;
        public string DepartmentId;
        public bool IsHandledByCustomOffice;
        public string DeclarationOfficeCode;
        public string SearchFields;
        public string CustomFileNo;
        public string DeclarationId;
        public int dataCount;
        public string SeenByAssigneeStatus;
        public string ObjectTableName;
    }
}