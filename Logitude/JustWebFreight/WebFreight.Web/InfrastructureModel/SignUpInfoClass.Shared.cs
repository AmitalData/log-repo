

using Simplog.Data.CommonDataModel.EntityPOCOs;
namespace WebFreight.Web.InfrastructureModel
{
	public class SignUpInfoClass
	{
        public string Email { get; set; }
        //public string Password { get; set; }
        public string Company { get; set; }
        public string Phone { get; set; }
        //public string EmployeeNumber { get; set; }
        public string Name { get; set; }
        public string CustomerId { get; set; }
        public bool IsCrmTenant { get; set; }
        public int Tenant { get; set; }
        public string PackageCode { get; set; }
        public string CountryName { get; set; }
        public string CountryCode { get; set; }
        public string ObjecttableName { get; set; }

    }
}