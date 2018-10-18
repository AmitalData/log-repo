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

    public partial class DecCargoSplitConsItemListQueryService
    {
	    private IQueryable<DecCargoSplitConsItemList> GetIqueryableList(IQueryable<DecCargoSplitConsItem> iQueryable)
        {
		IQueryable<DecCargoSplitConsItemList> query = (from a in iQueryable.Include("SplitOrMergeReason")
                                                       select new DecCargoSplitConsItemList()
											{
                                                DeclarationCargoSplitId = a.DeclarationCargoSplitId,
                                                DecCargoSplitConsLineNo = a.DecCargoSplitConsLineNo,
                                                ItemLine = a.ItemLine,
                                                CargoDescription = a.CargoDescription,
                                                GrossMassMeasure = a.GrossMassMeasure,
                                                ParentCargoConsinmentItem = a.ParentCargoConsinmentItem,
                                                RequestReasonCode = a.RequestReasonCode,
                                                RequestReasonName = a.SplitOrMergeReason.LocalName,
                                            });
            return query;
		}

		private IQueryable<DecCargoSplitConsItem> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DecCargoSplitConsItem> iQueryable, int tenant)
        {
            return iQueryable;
        }
			}


}
	