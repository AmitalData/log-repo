using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.GlobalModel.EntityPMs;

namespace Logitude.BL.GlobalModel.Tools.DataMapping
{
    public class LogitudeLeadMapping
    {
        internal static void MappingLogitudeLead(LogitudeLeadPM itemPM, LogitudeLead itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
           
            }
 
            itemPoco.TenantNumber = itemPM.TenantNumber;
            itemPoco.Comments = itemPM.Comments;
            itemPoco.CompanyName = itemPM.CompanyName;
            itemPoco.ContactName = itemPM.ContactName;
            itemPoco.Email = itemPM.Email;
            itemPoco.IsEmailVerified = itemPM.IsEmailVerified;
            itemPoco.IsSentToCustomer = itemPM.IsSentToCustomer;
            itemPoco.PhoneNumber = itemPM.PhoneNumber;
            itemPoco.StatusCode = itemPM.StatusCode;
            itemPoco.RequestType = itemPM.RequestType;
            itemPoco.NumberOfBranches = itemPM.NumberOfBranches;
            itemPoco.NumberOfUsers = itemPM.NumberOfUsers;
            itemPoco.LastUpdateDate = itemPM.LastUpdateDate;
            itemPoco.CreateDate = itemPM.CreateDate;
            itemPoco.Country = itemPM.Country;

            itemPoco.CustomerId = itemPM.CustomerId;
            itemPoco.IsUserOpened = itemPM.IsUserOpened;
            itemPoco.OpportunityId = itemPM.OpportunityId;
            itemPoco.IATACode = itemPM.IATACode;
            itemPoco.CASSCode = itemPM.CASSCode;
            itemPoco.PackageCode = itemPM.PackageCode;
            itemPoco.LeadSource = itemPM.LeadSource;
            itemPoco.State = itemPM.State;
            itemPoco.City = itemPM.City;
            itemPoco.Street = itemPM.Street;
            itemPoco.ZipCode = itemPM.ZipCode;
            itemPoco.ClientId = itemPM.ClientId;
            itemPoco.LeadOrigin = itemPM.LeadOrigin;
            itemPoco.Campaign = itemPM.Campaign;
            BuildSearchFields(itemPM, itemPoco);     

        }

        private static void BuildSearchFields(LogitudeLeadPM entityPM, LogitudeLead lead)
        {
            string mySearchFields = "";

            if (!string.IsNullOrEmpty(entityPM.CompanyName))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.CompanyName : mySearchFields + "," + entityPM.CompanyName;
            }

            if (!string.IsNullOrEmpty(entityPM.Email))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Email : mySearchFields + "," + entityPM.Email;
            }

            if (!string.IsNullOrEmpty(entityPM.ContactName))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.ContactName : mySearchFields + "," + entityPM.ContactName;
            }

            if (!string.IsNullOrEmpty(entityPM.Comments))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Comments : mySearchFields + "," + entityPM.Comments;
            }

            if (!string.IsNullOrEmpty(entityPM.PhoneNumber))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.PhoneNumber : mySearchFields + "," + entityPM.PhoneNumber;
            }

            if (!string.IsNullOrEmpty(entityPM.StatusCode))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.StatusCode : mySearchFields + "," + entityPM.StatusCode;
            }

            if (!string.IsNullOrEmpty(entityPM.RequestType))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.RequestType : mySearchFields + "," + entityPM.RequestType;
            }

            if (!string.IsNullOrEmpty(entityPM.Country))
            {
                mySearchFields = string.IsNullOrEmpty(mySearchFields) ? entityPM.Country : mySearchFields + "," + entityPM.Country;
            }

            entityPM.SearchFields = mySearchFields;
            lead.SearchFields = mySearchFields;
        }
    }
}
