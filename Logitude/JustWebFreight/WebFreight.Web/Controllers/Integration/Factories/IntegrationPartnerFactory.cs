using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Simplog.Data.CommonDataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Controllers.Integration.Factories
{
    public class IntegrationPartnerFactory
    {
        private int tenant;
        private string typeCode;
        private string typeName;
        private string cardName;
        private string countryId;
        private IntegrationPartner partner;
        private ICommonDataContext context;
        public IntegrationPartnerFactory(int tenant, string countryId, ICommonDataContext context)
        {
            this.tenant = tenant;
            this.context = context;
            this.countryId = countryId;
        }

        public IntegrationPartner PrepairePartner(string typeCode, string typeName)
        {
            this.typeCode = typeCode;
            this.typeName = typeName;
            this.cardName = "Integration " + typeName;

            this.GetPartner();

            if(partner == null)
            {
                this.CreatePartner();
            }

            partner.TypeCode = typeCode;
            partner.TypeName = typeName;

            return partner;
        }

        private void GetPartner()
        {
            partner = (from d in context.Cards
                       where
                       d.Tenant == tenant
                       && d.PartnerTypeId == typeCode
                       && d.EnglishName == cardName
                       select new IntegrationPartner()
                       {
                           Id = d.Id
                       }).FirstOrDefault();

            if (partner != null)
            {
                this.GetMainAddress();

                this.GetOtherVariables();
            }
        }

        private void CreatePartner()
        {
            switch (typeCode)
            {
                case "Shipper":
                case "Consignee":
                    {
                        CustomerPM entityPM = new CustomerPM()
                        {
                            Tenant = tenant,
                            PartnerTypeId = typeCode,
                            EnglishName = cardName,
                        };

                        entityPM.Addresses.Add(CreateMainAddress());

                        CustomerService service = new CustomerService(context, entityPM);
                        service.Create();

                        partner = new IntegrationPartner()
                        {
                            Id = entityPM.Id,
                            MainAddressId = entityPM.Addresses.FirstOrDefault().Id,
                        };

                        break;
                    }

                case "Agent":
                    {
                        AgentPM entityPM = new AgentPM()
                        {
                            Tenant = tenant,
                            PartnerTypeId = typeCode,
                            EnglishName = cardName,
                        };

                        entityPM.Addresses.Add(CreateMainAddress());

                        AgentService service = new AgentService(context,tenant);
                        service.Create(entityPM);

                        partner = new IntegrationPartner()
                        {
                            Id = entityPM.Id,
                            MainAddressId = entityPM.Addresses.FirstOrDefault().Id,
                        };

                        break;
                    }
            }
        }

        private void GetMainAddress()
        {
            partner.MainAddressId = (from d in context.Addresses where d.Tenant == tenant && d.CardId == partner.Id && d.AddressTypeId == "M" select d.Id).FirstOrDefault();

            if (partner.MainAddressId == null)
            {
                AddressPM entityPM = this.CreateMainAddress();
                entityPM.CardId = partner.Id;

                AddressService service = new AddressService(context, tenant);
                service.Create(entityPM);

                partner.MainAddressId = entityPM.Id;
            }
        }

        private AddressPM CreateMainAddress()
        {
            AddressPM entityPM = new AddressPM()
            {
                Tenant = tenant,
                AddressTypeId = "M",
                Description = "Main Address",
                Name = cardName,
                City = "Integration City",
                CountryId = countryId,
            };

            return entityPM;
        }

        private void GetOtherVariables()
        {
            switch (typeCode)
            {
                case "Shipper":
                    {
                        var output = (from d in context.Customers
                                      where d.Id == partner.Id
                                      select new
                                      {
                                          d1 = d.SalesmanUserId,
                                          d2 = d.AccountManagerUserId,
                                      }).FirstOrDefault();
                        break;
                    }
            }
        }
    }

    public class IntegrationPartner
    {
        public string Id { get; set; }
        public string TypeCode { get; set; }
        public string TypeName { get; set; }
        public string MainAddressId { get; set; }
    }
}