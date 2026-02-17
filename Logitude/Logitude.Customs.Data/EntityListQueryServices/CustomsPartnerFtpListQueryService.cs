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

            var all = new List<KeyValuePair<string, string>>() {
                new KeyValuePair<string, string>("SUBMANIFEST", "תת מצהר לממן"),
                new KeyValuePair<string, string>("ECTHR","ש.מ.ב לממן"),
            };
            IQueryable<CustomsPartnerFtpList> query = (from a in iQueryable
                                                           //join keyVal in all on a.InterfaceName equals keyVal.Key
                                                       select new CustomsPartnerFtpList()
                                                       {
                                                           Id = a.Id,
                                                           InterfaceName = a.InterfaceName,
                                                           PartnerCode = a.PartnerCode,
                                                           FileName = a.FileName,
                                                           FtpDetailsId = a.FtpDetailsId,
                                                           Tenant = a.Tenant,
                                                           TypeCode = a.TypeCode,

                                                           InterfaceCodeName = a.InterfaceName, // move 2 client
                                                           //all.FirstOrDefault( r=>r.Key== a.InterfaceName).Value,
                                                           //keyVal.Value,


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
	