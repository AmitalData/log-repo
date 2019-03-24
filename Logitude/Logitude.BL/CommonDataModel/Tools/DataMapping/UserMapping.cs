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
    public class UserMapping
    {
        public static void MapEntity(UserPM userPm, User user, bool isNewState)
        {
            if (isNewState)
            {
                user.Tenant = userPm.Tenant;
                user.CreateDate = userPm.CreateDate;
            }

            user.BranchId = userPm.BranchId;
            user.DepartmentId = userPm.DepartmentId;
            user.Notes = userPm.Notes;            
            user.IsBranchRestricted = userPm.IsBranchRestricted;
            user.Code = userPm.Code;
            user.IsSalesman = userPm.IsSalesman;
            user.IsFreelancer = userPm.IsFreelancer;
            user.FreelancerId = userPm.FreelancerId;
            user.BusinessUnitId = userPm.BusinessUnitId;
            
            user.ExpirationDate = userPm.ExpirationDate;
            user.LicencedUser = userPm.LicencedUser;
            user.IsProductRestricted = userPm.IsProductRestricted;
            user.ProductTypeCode = userPm.ProductTypeCode;
            user.IsDistributor = userPm.IsDistributor;
            user.DistributorCode = userPm.DistributorCode;
            user.IsShowContactDetailsInTheMobileApp = userPm.IsShowContactDetailsInTheMobileApp;
            user.PersonalId = userPm.PersonalId;
            user.Technology = userPm.Technology;
            user.SetAngularAsDefault = userPm.SetAngularAsDefault;
            user.IsTwoFactorAuthenticationEnabled = userPm.IsTwoFactorAuthenticationEnabled;
            user.DocumentFilingInbox = userPm.DocumentFilingInbox;
            user.ShowLogBoxToolTip = userPm.ShowLogBoxToolTip;
            user.ShowInboxToolTip = userPm.ShowInboxToolTip;
            user.ShowLocalNameInLOV = userPm.ShowLocalNameInLOV;
            user.UserRoles = userPm.UserRoles;
            user.ShowNewReleaseToolTip = userPm.ShowNewReleaseToolTip;
            BuildSearchFields(userPm, user);
        }

        private static void BuildSearchFields(UserPM entityPM, User entityPOCO)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Email);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.EnglishName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.LocalName);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.PersonalId);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Code);

            if (mySearchFields.Length > 1000)
            {
                mySearchFields = mySearchFields.Substring(0, 1000);
            }

            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
