using Logitude.Customs.BL.EntityDataMappings;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial class TapagQueryService
    {
        public TapagPM GetSingleTapagByLeadingFileNumber(string leadingFileNumber, int tenant)
        {
            ICustomContext context=MainContext as CustomContext;
            TapagRepository tapagRep=new TapagRepository(context);
            Tapag tapag = tapagRep.GetSingleTapagByLeadingFileNumber(leadingFileNumber, tenant);
            if (tapag == null || string.IsNullOrWhiteSpace(tapag.Id))
            {
                return null;
            }

            TapagPM tapagPM = new TapagPM();
            TapagDataMapping tapagMapping = new TapagDataMapping();
            tapagMapping.CustomPOCOToPM(tapagPM, tapag);
            tapagMapping.POCOToPM(tapagPM, tapag);
            return tapagPM;
        }

        public TapagList GetSingleTapagListByLeadingFileNumber(string leadingFileNumber, int tenant)
        {
            ICustomContext context = MainContext as CustomContext;
            TapagRepository tapagRep = new TapagRepository(context);
            Tapag tapag = tapagRep.GetSingleTapagByLeadingFileNumber(leadingFileNumber, tenant);
            if (tapag == null || string.IsNullOrWhiteSpace(tapag.Id))
            {
                return null;
            }

            TapagList tapagList = new TapagList()
            {
                Id = tapag.Id,
                Tenant = tapag.Tenant,
                CreateDate = tapag.CreateDate,
                CustomerId = tapag.CustomerId,
                TapagTypeCode = tapag.TapagTypeCode,
                LeadingFileNumber = tapag.LeadingFileNumber,
                ValidityDate = tapag.ValidityDate,
                IsClosed = tapag.IsClosed,

                TapagTypeName = tapag.TapagType != null ? tapag.TapagType.LocalName : null,
                ImporterId = tapag.ImporterId,
                ProfessionUnitTypeCode = tapag.ProfessionUnitTypeCode,
                CustomsBranchCode = tapag.CustomsBranchCode,
                ImporterName = tapag.Importer != null ? tapag.Importer.LocalFirstName : null,
                CustomsBranchName = tapag.CustomsBranch != null ? tapag.CustomsBranch.LocalName : null,
                ProfessionUnitTypeName = tapag.ProfessionUnitType != null ? tapag.ProfessionUnitType.LocalName : null,
                ReferantId = tapag.ReferantId,

            };

         
            return tapagList;
            
        }
       

        public List<TapagList> GetDeclarationTapags(string declarationId, int tenant)
        {
            ICustomContext context = MainContext as CustomContext;
            TapagConnectionTableRepository connectionRep = new TapagConnectionTableRepository(context);
            List<TapagConnectionTable> connections = connectionRep.GetDearationTapagConnectionTables(declarationId,null, tenant);

            List<string> tapagIds = (from a in connections select a.TapagId).ToList();


            List<Tapag> tapags = repository.GetTapagsByListOfIds(tapagIds);

            List<TapagList> tapagLists = new List<TapagList>();
            foreach (Tapag item in tapags)
            {
                   TapagConnectionTable connection = (from a in connections
                                                   where a.TapagId == item.Id
                                                   select a).FirstOrDefault();

                TapagList tapagList = new TapagList()
                {
                    Id = item.Id,
                    Tenant = item.Tenant,
                    CreateDate = item.CreateDate,
                    CustomerId = item.CustomerId,
                    TapagTypeCode = item.TapagTypeCode,
                    LeadingFileNumber = item.LeadingFileNumber,
                    ValidityDate = item.ValidityDate,
                    IsClosed = item.IsClosed,
                    CustomsTapagFile = connection.CustomsTapagFile,
                    CustomsNumeral = connection.CustomsNumeral,
                    RequestFileNumber = connection.RequestFileNumber,
                    TapagTypeName = item.TapagType != null? item.TapagType.LocalName : null,
                    ImporterId = item.ImporterId,
                    ProfessionUnitTypeCode = item.ProfessionUnitTypeCode,
                    CustomsBranchCode = item.CustomsBranchCode,
                    ImporterName = item.Importer != null ? item.Importer.LocalFirstName : null,
                    CustomsBranchName = item.CustomsBranch != null? item.CustomsBranch.LocalName : null,
                    ProfessionUnitTypeName = item.ProfessionUnitType != null ? item.ProfessionUnitType.LocalName : null,
                    ReferantId = item.ReferantId,

                };

                tapagLists.Add(tapagList);
            }

            return tapagLists;

          






        }
    }
}
