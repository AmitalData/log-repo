using System;
using System.Collections.Generic;

namespace RestClientApplication
{
	public class UserData
	{

		public string UserName { get; set; }

		public string Id { get; set; }

		public string CardId { get; set; }
		public string CardType { get; set; }
		public int CurrentTenant { get; set; }

		public bool IsUser { get; set; }

		public bool IsLocked { get; set; }
		public bool IpRestricted { get; set; }
		public bool InValidMailOrPassword { get; set; }
		public bool HasError { get; set; }

		public bool MustChangePassword { get; set; }

		public List<CompanyLogin> CompanyLogins { get; set; }
		public int ContactsCount { get; set; }

		public string Token {
			get;
			set;
		}

		public bool InvalidMobileAccessPermission { get; set; }

		public CompanyLogin SelectedCompanyLogin { get; set; }
		//#region IUser Members
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


    public  class CompanyLogin
	{
        public  int Tenant { get; set; }
        public  string CompanyName { get; set; }
        public   bool IsUser { get; set; }
        public  string CardId { get; set; }
        public   string CardType { get; set; }
        public  string Email { get; set; }
        public  string ContactId { get; set; }
        public  string URL { get; set; }
        public  bool InternetAccess { get; set; }
        public  string CustomerName { get; set; }



	}
}