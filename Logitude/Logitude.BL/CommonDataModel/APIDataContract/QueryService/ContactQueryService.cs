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
        public List<Contact> ContactCustomDataMapping(CardPM EntityPm, List<ContactPM> MyEntityPMs, int Tenant)
        {
            return this.ContactMapping(MyEntityPMs, Tenant);           
        }

        public List<ContactPM> ContactCustomDataMappingAndValidatin(Customer MainEntity, List<Contact> MyEntities, int Tenant, string ComputingPartnerName = "")
        {
            return this.ContactMappingAndValidating(MyEntities, Tenant, ComputingPartnerName);
        }

        public List<Contact> ContactMapping(List<ContactPM> MyEntityPM, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var MyList = new List<Contact>();
                foreach (var item in MyEntityPM)
                {
                    var temp = new Contact();
                    temp.Id = item.Id;
                    temp.EnglishName = item.EnglishName;
                    temp.LocalName = item.LocalName;
                    temp.Code = item.ExternalId;
                    temp.Email = item.Email;
                    MyList.Add(temp);
                }

                return MyList;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<ContactPM> ContactMappingAndValidating(List<Contact> MyEntity, int Tenant, string ComputingPartnerName = "")
        {
            try
            {
                var MyList = new List<ContactPM>();
                foreach (var item in MyEntity)
                {
                    var temp = new ContactPM();
                    if (!string.IsNullOrEmpty(item.Id))
                    {
                        temp = query.GetSinglePM(item.Id, Tenant);
                    }

                    if (temp == null)
                    {
                        throw new ApplicationException("Contact with Id " + item.Id + " doesn't exist");
                    }

                    if (string.IsNullOrEmpty(temp.Id))
                    {
                        temp.Id = item.Id;
                    }

                    temp.EnglishName = item.EnglishName;
                    temp.LocalName = item.LocalName;
                    temp.ExternalId = item.Code;
                    temp.Email = item.Email;
                    MyList.Add(temp);
                }

                return MyList;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
