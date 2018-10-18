using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Xml.Serialization;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data.EntityListQueryServices;
using Simplog.Server.Infrastructure;
using System.ServiceModel.DomainServices.Server;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace WebFreight.Web.CustomModel.DomainServices
{
    public partial class CustomDomainService
    {
        public ClientPM GetSingleClientPM(string id, int tenant)
        {
            customContext = CustomContext.GetContext(tenant);
            clientQuery = new ClientQueryService(customContext);
            ClientPM Client = clientQuery.GetSingle(id, true, false);
            return Client;
        }

        public ClientList GetSingleClientList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Client", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ClientListQueryService listService = new ClientListQueryService(customContext);
            return listService.GetSingle(id);
        }

        public ClientList GetSingleClientListByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Client", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ClientListQueryService listService = new ClientListQueryService(customContext);
            return listService.GetSingleClientListByCode(code,tenant);
        }

        public ClientList GetSingleClientListByPassportNumber(string passportNumber, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
          

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            ClientListQueryService listService = new ClientListQueryService(customContext);
            return listService.GetSingleClientListByPassportNumber(passportNumber, tenant);
        }

        public ClientPM GetSingleClientPMByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Client", "READ", tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            clientQuery = new ClientQueryService(customContext);
            ClientPM client = clientQuery.GetClientByCode(code,tenant);
            return client;
        }

        public List<ClientList> GetClientLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Client", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ClientListQueryService listService = new ClientListQueryService(customContext);
            return listService.GetList(tenant);
         
        }

        public List<ClientList> GetClientFilters(byte[] xmlFilters, int tenant)
        {
           

            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Client", "READ", tenant);

            customContext = CustomContext.GetContext(tenant);
            ClientListQueryService listService = new ClientListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return listService.GetList(queryOperations, tenant);

        }

        public bool DoesClientCodeExist(string code, int tenant)
        {
            clientRepository = new ClientRepository(tenant);
            return (clientRepository.GetAll(tenant).Where(d => d.Code == code && d.Tenant == tenant)).Any();
        }


        public int GetClientFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            //SecurityUtility.CheckContactFeature("Customs.Client", "READ", tenant);
            customContext = CustomContext.GetContext(tenant);
            ClientListQueryService queryService = new ClientListQueryService(customContext);
            QueryOperations queryOperations = EntityListFilter.GetQueryOperations(xmlFilters);
            return queryService.GetListCount(queryOperations, tenant);

        }

        public void InsertClient(ClientPM entityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.Client", "NEW", entityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(entityPm.Tenant);
            }

            ClientUpdateService service = new ClientUpdateService(customContext, new Dictionary<string, IContext>(), entityPm.Tenant);
            entityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            foreach (ClientAddressPM clientAddress in entityPm.ClientAddresses)
            {
                clientAddress.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
            }
             service.Update(entityPm, true);
         

          
        }

        public void UpdateClient(ClientPM currententityPm)
        {
            //SecurityUtility.CheckContactFeature("Customs.Client", "UPDATE", currententityPm.Tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(currententityPm.Tenant);
            }

            ClientUpdateService service = new ClientUpdateService(customContext, new Dictionary<string, IContext>(), currententityPm.Tenant);
            currententityPm.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            SetClientAddressChangeSet(currententityPm);
            service.Update(currententityPm, true);

        }

        private void SetClientAddressChangeSet(ClientPM currententityPm)
        {
            List<ClientAddressPM> clientAddresschangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.ClientAddresses).Cast<ClientAddressPM>().ToList();
            foreach (ClientAddressPM itemPM in clientAddresschangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ClientAddressPM currentItemPM = currententityPm.ClientAddresses.Where(d => d.ClientId == itemPM.ClientId && d.AddressId == itemPM.AddressId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;
                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            ClientAddressPM currentItemPM = currententityPm.ClientAddresses.Where(d => d.ClientId == itemPM.ClientId && d.AddressId == itemPM.AddressId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;                          
                            SetClientAddressCommunicationTypeChangeSet(currentItemPM);
                         
                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            ClientAddressPM currentItemPM = new ClientAddressPM() { ChangeSetOp = ChangeSetOperation.Delete, ClientId = itemPM.ClientId, AddressId = itemPM.AddressId };



                            List<ClientsAddressCommTypePM> ClientAddressCommunicationTypeChangeset = ChangeSet.GetAssociatedChanges(itemPM, d => d.ClientsAddressCommTypes).Cast<ClientsAddressCommTypePM>().ToList();
                            foreach (ClientsAddressCommTypePM item in ClientAddressCommunicationTypeChangeset)
                            {
                                ClientsAddressCommTypePM deletedItem = new ClientsAddressCommTypePM()
                                {
                                   ClientId = item.ClientId,
                                   AddressId = item.AddressId,
                                   Line = item.Line,
                                  ChangeSetOp = ChangeSetOperation.Delete,
                                };
                                currentItemPM.DeletedClientsAddressCommTypes.Add(deletedItem);
                            }

                            currententityPm.DeletedClientAddresses.Add(currentItemPM);

                            break;
                        }
                    default:
                        {
                            ClientAddressPM currentItemPM = currententityPm.ClientAddresses.Where(d => d.ClientId == itemPM.ClientId && d.AddressId == itemPM.AddressId).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }

        private void SetClientAddressCommunicationTypeChangeSet(ClientAddressPM currententityPm)
        {
            List<ClientsAddressCommTypePM> clientsAddressCommunicationTypeChangeset = ChangeSet.GetAssociatedChanges(currententityPm, d => d.ClientsAddressCommTypes).Cast<ClientsAddressCommTypePM>().ToList();
            foreach (ClientsAddressCommTypePM itemPM in clientsAddressCommunicationTypeChangeset)
            {
                switch (ChangeSet.GetChangeOperation(itemPM))
                {
                    case ChangeOperation.Insert:
                        {
                            ClientsAddressCommTypePM currentItemPM = currententityPm.ClientsAddressCommTypes.Where(d => d.ClientId == itemPM.ClientId && d.AddressId == itemPM.AddressId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Insert;

                            break;
                        }
                    case ChangeOperation.Update:
                        {
                            ClientsAddressCommTypePM currentItemPM = currententityPm.ClientsAddressCommTypes.Where(d => d.ClientId == itemPM.ClientId && d.AddressId == itemPM.AddressId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.Update;


                            break;
                        }
                    case ChangeOperation.Delete:
                        {
                            ClientsAddressCommTypePM currentItemPM = new ClientsAddressCommTypePM() { ChangeSetOp = ChangeSetOperation.Delete, ClientId = itemPM.ClientId, AddressId = itemPM.AddressId, Line = itemPM.Line };
                            currententityPm.DeletedClientsAddressCommTypes.Add(currentItemPM);

                            break;
                        }
                    default:
                        {
                            ClientsAddressCommTypePM currentItemPM = currententityPm.ClientsAddressCommTypes.Where(d => d.ClientId == itemPM.ClientId && d.AddressId == itemPM.AddressId && d.Line == itemPM.Line).FirstOrDefault();
                            currentItemPM.ChangeSetOp = ChangeSetOperation.None;
                            break;
                        }
                }
            }
        }
        public void UpdateClientList(ClientList list)
        {

        }


        public ClientPM GetSingleClientPMByCardId(string CardId, int Tenant)
        {
            ClientPM client = null;

            //if (customContext == null)
            //{
            //    customContext = CustomContext.GetContext(Tenant);
            //}

            //clientQuery = new ClientQueryService(customContext);

            CardRepository cardRepository = new CardRepository(Tenant);
            Card card = cardRepository.GetSingleCard(CardId, Tenant);
            if (card != null)
            {
                client = GetSingleClientPMByCode(card.VatNumber, Tenant);
            }

            return client;
        }

        public ClientPM GetSingleClientPMIncludeAllByCode(string code, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            if (customContext == null)
            {
                customContext = CustomContext.GetContext(tenant);
            }
            customContext = CustomContext.GetContext(tenant);
            clientQuery = new ClientQueryService(customContext);
            ClientPM client = clientQuery.GetClientByCode(code, tenant);
            if (client != null)
            {
                client = clientQuery.GetSingle(client.Id, true, false);
            }
            return client;
        }

        public bool CheckIfCorporationNameExists(int tenant)
        {
            bool exists = false;
            var setting = CustomsSettingQueryService.GetSettingByTenant(tenant);
            string claimSubmiterNumber = "";

            if (setting != null)
            {
                claimSubmiterNumber = setting.CustomsAgentId.Length <= 9 ? setting.CustomsAgentId : null;
            }

            if (!string.IsNullOrEmpty(claimSubmiterNumber))
            {
                ClientQueryService clientQueryService = new ClientQueryService(tenant);
                ClientPM clientPM = clientQueryService.GetClientByCode(claimSubmiterNumber, tenant);
                if (clientPM != null && !string.IsNullOrWhiteSpace(clientPM.LocalCorporationName))
                {
                    exists = true;
                }
            }
            return exists;
        }
  
    }
}