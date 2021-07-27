using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityUpdateServices;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class ClientQueryService : EntityQueryService<Client, ClientKeys, ClientPM, object, ClientKeys>
    {
        public override void GetComposition(EntityKeyFields entityKeys, ClientPM entityPM)
        {
            ICustomContext context = MainContext as CustomContext;
            ClientKeys clientKeys = entityKeys as ClientKeys;
            ClientAddressQueryService clientAddressQueryService = new ClientAddressQueryService(context);
            ClientDrivingLicenseQueryService clientDrivingLicenseQueryService = new ClientDrivingLicenseQueryService(context);
            ClientsPoaQueryService clientsPoaQueryService = new ClientsPoaQueryService(context);

            entityPM.ClientAddresses = clientAddressQueryService.GetMulti(clientKeys, true);
            entityPM.ClientDrivingLicenses = clientDrivingLicenseQueryService.GetMulti(clientKeys, true);
            entityPM.ClientPoas = clientsPoaQueryService.GetMulti(clientKeys, true);
        }
        public string GetIdByCode(string code, int tenant,bool insertIfNotFount = false)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            string id = repository.GetIdByCode(tenant, code);

            if (string.IsNullOrWhiteSpace(id) && insertIfNotFount == true)
            {
                ICustomContext dbContext = CustomContext.GetContext(tenant);
                var clientUpdateService = new ClientUpdateService(dbContext, new Dictionary<string, IContext>(), tenant);

                ClientPM newClientPM = new ClientPM();
                newClientPM.Tenant = tenant;
                newClientPM.Code = code;
                clientUpdateService.InsertNewClientOnlyByCode(newClientPM, true);
                id = newClientPM.Id;
            }
            return id;
        }

        public string GetIdByPassportNumberOrCountry(string passportNumber,string passportCountryCode, int tenant)
        {
            if (string.IsNullOrEmpty(passportNumber) && string.IsNullOrEmpty(passportCountryCode))
            {
                return null;
            }
            string id = repository.GetIdByPassportNumberOrCountry(tenant, passportNumber, passportCountryCode);
            return id;
        }

        public ClientPM GetClientByCode(string code, int tenant)
        {
            Client client = repository.GetSingleClientByCode(code, tenant);
            ClientPM clientPM = null;
            if (client != null)
            {
                 clientPM = new ClientPM()
                {
                    Code = client.Code,
                    PassportNumber = client.PassportNumber,
                    PassportCountryCode = client.PassportCountryCode,
                    PassportTypeCode = client.PassportTypeCode,
                    Tenant = client.Tenant,
                    BirthDate = client.BirthDate,
                    ClientTypeSpecificCode = client.ClientTypeSpecificCode,
                    Id = client.Id,
                    FullName = client.FullName,
                    LocalCorporationName = client.LocalCorporationName,
                    FacilitationTypeCode = client.FacilitationTypeCode
                };
            }
            return clientPM;
        }

        public List<ClientPM> GetAllLocalClients(int tenant)
        {
            List<ClientPM> clientsPMList = new List<ClientPM>();
            List<Client> clientsList = repository.GetAllLocalClients(tenant);

            if (clientsList != null && clientsList.Count > 0)
            {
                foreach (Client client in clientsList)
                {
                    ClientPM clientPM = new ClientPM()
                    {
                        Code = client.Code,
                        PassportNumber = client.PassportNumber,
                        PassportCountryCode = client.PassportCountryCode,
                        PassportTypeCode = client.PassportTypeCode,
                        Tenant = client.Tenant,
                        ClientTypeSpecificCode = client.ClientTypeSpecificCode,
                        Id = client.Id,
                    };
                    clientsPMList.Add(clientPM);
                }
            }

            return clientsPMList;
        }
    }
}
