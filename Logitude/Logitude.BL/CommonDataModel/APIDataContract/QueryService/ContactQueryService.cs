using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.CommonDataModel.APIDataContract.ApiV1
{
    public partial class ContactQueryService
    {
        public List<Contact> ContactCustomDataMapping(CardPM EntityPm, List<ContactPM> MyEntityPMs, int Tenant, string ComputingPartnerName = "")
        {
            return this.ContactMapping(MyEntityPMs, Tenant,ComputingPartnerName);           
        }

        public List<ContactPM> ContactCustomDataMappingAndValidatin(Customer MainEntity, List<Contact> MyEntities, int Tenant, string ComputingPartnerName = "", bool IsUpdate = false)
        {
            return this.ContactMappingAndValidating(MyEntities, Tenant, ComputingPartnerName);
        }

        public List<Contact> ContactMapping(List<ContactPM> MyEntityPM, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var contacts = new List<Contact>();
                foreach (var item in MyEntityPM)
                {
                    var temp = new Contact();
                    temp.Id = item.Id;
                    temp.EnglishName = item.EnglishName;
                    temp.LocalName = item.LocalName;
                    temp.Code = item.ExternalId;
                    temp.Email = item.Email;
                    temp.Position = item.Position;
                    temp.Mobile = item.Mobile;
                    temp.BusinessPhone = item.BusinessPhone;
                    temp.Code = item.ExternalId;
                    contacts.Add(temp);
                }

                return contacts;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ContactPM> ContactMappingAndValidating(List<Contact> MyEntity, int tenant, string ComputingPartnerName = "")
        {
            try
            {
                var contacts = new List<ContactPM>();
                foreach (var item in MyEntity)
                {
                    var contactPM = new ContactPM();
                    if (!string.IsNullOrEmpty(item.Id))
                    {
                        contactPM = query.GetSinglePM(item.Id, tenant);

                        if (contactPM == null)
                            throw new ApplicationException("Contact with Id " + item.Id + " doesn't exist");
                    }

                    else if (!string.IsNullOrEmpty(item.Email))
                    {
                        contactPM = new ContactPM();
                        contactPM.IsAPIContact = true;
                        contactPM.IsCreatedWithPartner = true;                       
                    }
                   
                    contactPM.Id = item.Id;
                    contactPM.Tenant = tenant;
                    contactPM.EnglishName = item.EnglishName;
                    contactPM.LocalName = item.LocalName;
                    contactPM.ExternalId = item.Code;
                    contactPM.Email = item.Email;
                    contactPM.Position = item.Position;
                    contactPM.Mobile = item.Mobile;
                    contactPM.BusinessPhone = item.BusinessPhone;
                    contactPM.ExternalId = item.Code;
                    contactPM.SetAsPrimaryForCard = item.IsPrimaryContact;
                    contacts.Add(contactPM);
                }

                return contacts;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
