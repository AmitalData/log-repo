namespace Logitude.Base.Models.PartnersPreparation
{
    public class Contact
    {
        public int Tenant { get; set; }
        public string EnglishName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Fax { get; set; }
        public bool IsCreatedWithPartner { get; set; }
        public bool SetAsPrimaryForCard { get; set; }
    }
}