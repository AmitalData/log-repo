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
    public partial class ClientsPoaQueryService : EntityQueryService<ClientsPoa, ClientsPoaKeys, ClientsPoaPM, ClientPM, ClientKeys>
    {
        
        //public List<ClientsPoaPM> GetPoas(string authorizerExternalId, string authorizerPassportNumber,string poaID, int tenant)
        //{
        //    if (string.IsNullOrWhiteSpace(authorizerExternalId) && string.IsNullOrWhiteSpace(authorizerPassportNumber))
        //        return null;

        //    List<ClientsPoaPM> clientsPoaPMList = new List<ClientsPoaPM>();
        //    List<ClientsPoa> clientsPoaList = repository.GetPoas(authorizerExternalId, authorizerPassportNumber, poaID, tenant);

        //    if (clientsPoaList != null && clientsPoaList.Count > 0)
        //    {
        //        foreach (ClientsPoa poa in clientsPoaList)
        //        {
        //            ClientsPoaPM clientPoaPM = new ClientsPoaPM()
        //            {
        //                PoaID = poa.PoaID,
        //                PoaAuthorizationType = poa.PoaAuthorizationType,
        //                AuthorizerPassportType = poa.AuthorizerPassportType,
        //                AuthorizerPassportNumber = poa.AuthorizerPassportNumber,
        //                AuthorizerExternalId = poa.AuthorizerExternalId,
        //                AuthorizedExternalId = poa.AuthorizedExternalId,
        //                AuthorizerPassportCountry = poa.AuthorizerPassportCountry,
        //                ClientId = poa.ClientId,
        //                EndDate = poa.EndDate,
        //                PoaStatus = poa.PoaStatus,
        //                StartDate = poa.StartDate,
        //                Tenant = poa.Tenant,
        //                Id = poa.Id
        //            };
        //            clientsPoaPMList.Add(clientPoaPM);
        //        }
        //    }

        //    return clientsPoaPMList;

        //}

       
    }
}
