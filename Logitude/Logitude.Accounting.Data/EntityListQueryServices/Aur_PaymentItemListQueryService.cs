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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class Aur_PaymentItemListQueryService
    {
	    private IQueryable<Aur_PaymentItemList> GetIqueryableList(IQueryable<Aur_PaymentItem> iQueryable)
        {
		IQueryable<Aur_PaymentItemList> query = (from a in iQueryable
                                            select new Aur_PaymentItemList()
											{
                     
					                          PaymentId = a.PaymentId,
					
					                          Line = a.Line,
					
					                          PaymentSequence = a.PaymentSequence,
					
					                          QuoteId = a.QuoteId,
					
					                          Project = a.Project,
					
					                          ProjectNumber = a.ProjectNumber,
					
					                          SectionType = a.SectionType,
					
					                          BaseAmount = a.BaseAmount,
					
		                    	            });
            return query;
		}

		private IQueryable<Aur_PaymentItem> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Aur_PaymentItem> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<Aur_PaymentItem> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Aur_PaymentItem> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	