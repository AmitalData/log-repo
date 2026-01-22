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
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure.Helpers;

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
            ClientsTapagQueryService clientsTapagQueryService = new ClientsTapagQueryService(context);
            ClientIndicationQueryService clientIndicationQueryService = new ClientIndicationQueryService(context);

            entityPM.ClientAddresses = clientAddressQueryService.GetMulti(clientKeys, true);
            entityPM.ClientDrivingLicenses = clientDrivingLicenseQueryService.GetMulti(clientKeys, true);
            entityPM.ClientPoas = clientsPoaQueryService.GetMulti(clientKeys, true);
            entityPM.ClientsTapags = clientsTapagQueryService.GetMulti(clientKeys, true);
            entityPM.ClientIndications = clientIndicationQueryService.GetMulti(clientKeys, true);

        }
        public string GetIdByCode(string code, int tenant, bool insertIfNotFount = false)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }

            // fix the teudat zeut length
            code = code.PadLeft(9, '0');

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

        public string GetIdByCodeOrPassport(string code, string passport, int tenant, bool insertIfNotFount = false)
        {
            //if (string.IsNullOrEmpty(code))
            //{
            //    return null;
            //}
            string id = repository.GetIdByCodeOrPassport(tenant, code, passport);

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

        public string GetIdByPassportNumberOrCountry(string passportNumber, string passportCountryCode, int tenant)
        {
            if (string.IsNullOrEmpty(passportNumber) && string.IsNullOrEmpty(passportCountryCode))
            {
                return null;
            }
            string id = repository.GetIdByPassportNumberOrCountry(tenant, passportNumber, passportCountryCode);
            return id;
        }

        public ClientPM GetClientByCode_Cache(string code, int tenant)
        {
            string entityKeyString = $"GetClientByCode_Cache({code}, {tenant})";
            var res = CacheManager
                .GetOrInsertNewObject<ClientPM>(entityKeyString,
                () => { return this.GetClientByCode(code, tenant); });
            return res;

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
        public Dictionary<string, string> GetClientIdsByCodes(IEnumerable<string> codes, int tenant)
        {
            var list = codes?
                .Where(c => !string.IsNullOrWhiteSpace(c))
                .Select(c => c.Trim())
                .Distinct()
                .ToList();

            if (list == null || list.Count == 0)
                return new Dictionary<string, string>();

            var clients = repository.GetClientsByCodes(list, tenant);

            return clients
                .Where(c => !string.IsNullOrEmpty(c.Code))
                .GroupBy(c => c.Code)
                .ToDictionary(g => g.Key, g => g.First().Id);
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

        public List<ClientPM> GetAllLocalClientsIsConcurrencyGUID(int tenant)
        {
            List<ClientPM> clientsPMList = new List<ClientPM>();
            List<Client> clientsList = repository.GetAllLocalClientsIsConcurrencyGUID(tenant);

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


        public List<ClientPM> GetAllClientsPOAExpire(int tenant) 
        {
            //LogMessagingUtil.Instance.AppendLine(context.GetConnection().ConnectionString);
            List<ClientPM> clientsPMList = new List<ClientPM>();
            List<Client> clientsList = repository.GetAllClientsPOAExpire(tenant);

            if (clientsList != null && clientsList.Count > 0)
            {
                foreach (Client client in clientsList)
                {
                    /*ClientPM clientPM = new ClientPM()
                    {
                        Code = client.Code,
                        PassportNumber = client.PassportNumber,
                        PassportCountryCode = client.PassportCountryCode,
                        PassportTypeCode = client.PassportTypeCode,
                        Tenant = client.Tenant,
                        ClientTypeSpecificCode = client.ClientTypeSpecificCode,
                        Id = client.Id,
                    };*/
                    var clientPM = this.GetEntityPM(client, true, new ClientKeys() { Id = client.Id });
                    clientsPMList.Add(clientPM);
                }
            }

            return clientsPMList;
        }
    }
}
