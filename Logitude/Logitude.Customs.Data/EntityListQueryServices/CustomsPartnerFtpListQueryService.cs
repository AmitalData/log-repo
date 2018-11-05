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

    public partial class CustomsPartnerFtpListQueryService
    {
        private IQueryable<CustomsPartnerFtpList> GetIqueryableList(IQueryable<CustomsPartnerFtp> iQueryable)
        {
            IQueryable<CustomsPartnerFtpList> query = (from a in iQueryable
                                                       select new CustomsPartnerFtpList()
                                                       {
                                                           Id = a.Id,
                                                           InterfaceName = a.InterfaceName,
                                                           PartnerCode = a.PartnerCode,
                                                           FileName = a.FileName,
                                                           FtpDetailsId = a.FtpDetailsId,
                                                           Tenant = a.Tenant,
                                                           TypeCode = a.TypeCode,


                                                           FileExt = a.FileExt,

                                                       });
            return query;
        }

        private IQueryable<CustomsPartnerFtp> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<CustomsPartnerFtp> iQueryable, int tenant)
        {
            //throw new NotImplementedException();
            return iQueryable;
        }
    }


}
	