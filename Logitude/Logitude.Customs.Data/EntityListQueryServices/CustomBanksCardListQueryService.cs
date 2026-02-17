	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CustomBanksCardListQueryService
    {
	    private IQueryable<CustomBanksCardList> GetIqueryableList(IQueryable<CustomBanksCard> iQueryable)
        {
            IQueryable<CustomBanksCardList> query = (from a in iQueryable
                                                     select new CustomBanksCardList()
                                                    {
                                                       CardId = a.CardId,
                                                       CardName = a.Card != null? a.Card.LocalName : null,
                                                       CustomBankId = a.CustomBankId,
                                                       CustomsBankName = a.CustomBank != null? a.CustomBank.LocalName : null,
                                                       Id = a.Id,
                                                       Tenant = a.Tenant,


                                                    });
            return query;
		}

        private IQueryable<CustomBanksCard> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomBanksCard> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	