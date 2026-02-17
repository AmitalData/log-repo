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

    public partial class DeficitConnFileParagraphTypeListQueryService
    {
	    private IQueryable<DeficitConnFileParagraphTypeList> GetIqueryableList(IQueryable<DeficitConnFileParagraphType> iQueryable)
        {
            IQueryable<DeficitConnFileParagraphTypeList> query = (from a in iQueryable.Include("ParagraphType")
                                                                       select new DeficitConnFileParagraphTypeList()
                                                    {
                                                        DeclarationId = a.DeclarationId,
                                                        Amount = a.Amount,
                                                        DeficitId = a.DeficitId,
                                                        ParagraphTypeCode = a.ParagraphTypeCode,
                                                        Tenant = a.Tenant,
                                                        ParagraphTypeName = a.ParagraphType != null? a.ParagraphType.LocalName :null,
                                                    });
            return query;
		}

        private IQueryable<DeficitConnFileParagraphType> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<DeficitConnFileParagraphType> iQueryable, int tenant)
        {
            return iQueryable;
        }
	}


}
	