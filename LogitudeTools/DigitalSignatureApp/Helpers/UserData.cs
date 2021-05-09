using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloud.Sign.App.Helpers
{
    public class UserData
    {
        public string UserName { get; set; }
        public string Id { get; set; }

        public string CardId { get; set; }
        public string CardType { get; set; }
        public int CurrentTenant { get; set; }
        //public string CompanyName { get; set; }
        public bool IsUser { get; set; }
        public string Technology { get; set; }
        public bool IsLocked { get; set; }
        public bool IpRestricted { get; set; }
        public bool InValidMailOrPassword { get; set; }
        public bool HasError { get; set; }
        public string ExceptionMessage { get; set; }
        public bool MustChangePassword { get; set; }

        public List<CompanyLogin> CompanyLogins { get; set; }
        public int ContactsCount { get; set; }
        public string HtmlVersion { get; set; }
        public string Token { get; set; }
        //#region IUser Members
        public string UserId { get; set; }
        public int Tenant { get; set; }
        public bool InvalidToken { get; set; }

        public bool InvalidMobileAccessPermission { get; set; }

        public CompanyLogin SelectedCompanyLogin { get; set; }

        public bool InActive { get; set; }

        public bool PrivateLablehasZeroTenant { get; set; }

        public bool IsBrandingEnabled { get; set; }

        public bool Unlicensed { get; set; }
        //[Key]
        //public string Name
        //{
        //    get
        //    ;
        //    set
        //   ;
        //}

        //public IEnumerable<string> Roles
        //{
        //    get
        //    ;
        //    set
        //  ;
        //}

        //#endregion
    }


    public class CompanyLogin
    {
        public int Tenant { get; set; }
        public string CompanyName { get; set; }
        public string CustomerName { get; set; }
        public bool IsUser { get; set; }
        public string CardId { get; set; }
        public string CardType { get; set; }
        public string Email { get; set; }
        public string ContactId { get; set; }
        public bool LicensedUser { get; set; }
        public bool InternetAccess { get; set; }
        public string URL { get; set; }
        public string Extension { get; set; }
        public string Id { get; set; }
        public string PrivateLabelId { get; set; }
        public bool HasLogboxAccess { get; set; }
    }

    public class ApiCredential
    {
        public int Tenant { get; set; }
        public bool CustomerCare { get; set; }
        public bool InValidKey { get; set; }
        public bool IpRestricted { get; set; }
        public bool HasError { get; set; }
        public string Token { get; set; }

    }
}
