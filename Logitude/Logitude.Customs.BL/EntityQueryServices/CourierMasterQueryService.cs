using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityQueryServices
{
    public partial  class CourierMasterQueryService 
    {
        public CourierMasterPM GetByDeclarationId(string declarationId, int tenant)
        {
            var courierDeclarationQueryService = new CourierDeclarationQueryService(tenant);
            var courierDeclarationPM =courierDeclarationQueryService.GetCourierDeclarationByDeclarationId(declarationId, tenant);
            if (courierDeclarationPM == null)
            {
                return null;
            }
            var entityPM=this.GetSingle(courierDeclarationPM.CourierMasterId, false, false);
            return entityPM;
        }
        public CourierMasterPM GetSingleCourier(string airlineId, string MAWB, string HAWB, int tenant)
        {
            CourierMaster poco = repository.GetSingleCourier(airlineId, HAWB, MAWB, tenant);
            CourierMasterPM entityPM = null;
            if (poco != null)
            {
                entityPM = new CourierMasterPM()
                {
                    Id = poco.Id,
                    AirlineId = poco.AirlineId,
                    HAWB= poco.HAWB,
                    MAWB = poco.MAWB,
                    Tenant = poco.Tenant,
                };
            }
            return entityPM;
        }

        public IQueryable<DeclarationPM> GetCourierConnectedDeclarations(QueryOperations queryOperations, int tenant)
        {
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            string CourierMasterId = null;
            string CourierSearchField = null;
            int skippedItems = queryOperations.PageIndex;
            QueryFilterItem CourierMasterIdFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CourierMasterId").FirstOrDefault();
            QueryFilterItem CourierSearchFieldFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CourierSearchFields").FirstOrDefault();
            if (CourierMasterIdFilter != null)
            {
                CourierMasterId = CourierMasterIdFilter.FieldValue.ToString();
            }
            if (CourierSearchFieldFilter != null)
            {
                CourierSearchField = CourierSearchFieldFilter.FieldValue.ToString();
            }
            DeclarationRepository declarationRep = new DeclarationRepository(tenant);
            IQueryable<Declaration> declarations = declarationRep.GetCourierConnectedDeclaratins(CourierMasterId, tenant);

            IQueryable<DeclarationPM> declarationPMs;
            if (string.IsNullOrWhiteSpace(CourierSearchField))
            {
                declarationPMs = (from a in declarations
                                  select new DeclarationPM()
                                   {
                                       DeclarationNumber = a.DeclarationNumber,
                                       CustomFileNo = a.CustomFileNo,
                                       Id = a.Id,
                                       Tenant = a.Tenant,
                                       DeclarationStatusTypeName = a.DeclarationStatusType != null ? a.DeclarationStatusType.LocalName : null,
                                       CustomerName = a.CustomerCard != null ? a.CustomerCard.LocalName : null,
                                       ManifestNumber = a.CourierHAWB,
                                       CourierHAWB = a.CourierHAWB
                                   });
            }
            else
            {
                declarationPMs = (from a in declarations
                                  where a.CourierSearchFields.Contains(CourierSearchField)
                                  select new DeclarationPM()
                                   {
                                       DeclarationNumber = a.DeclarationNumber,
                                       CustomFileNo = a.CustomFileNo,
                                       Id = a.Id,
                                       Tenant = a.Tenant,
                                       DeclarationStatusTypeName = a.DeclarationStatusType != null ? a.DeclarationStatusType.LocalName : null,
                                       CustomerName = a.CustomerCard != null ? a.CustomerCard.LocalName : null,
                                       ManifestNumber = a.CourierHAWB,
                                       CourierHAWB = a.CourierHAWB
                                   });
            }


            return declarationPMs;
        }

        public void GetStatistic(string courierMasterId, int tenant, out List<KeyValuePair<string, int>> keyValuePairList)
        {
            keyValuePairList = new List<KeyValuePair<string, int>>();

            var repositoryCourierDeclarations = new CourierDeclarationRepository(MainContext as ICustomContext);
            var declarationCourierStatusRepository = new DeclarationCourierStatusRepository(MainContext as ICustomContext);
            //var declarationRepository = new DeclarationRepository(MainContext as ICustomContext);

            var q =
                (from cd in repositoryCourierDeclarations.GetAll(tenant).Where(r => r.CourierMasterId == courierMasterId)
                 join dStatus in declarationCourierStatusRepository.GetAll(tenant)
                 on cd.DeclarationId equals dStatus.DeclarationId
                 select dStatus
                 //join declaration in declarationRepository.GetAll(tenant)
                 //on dStatus.DeclarationId equals declaration.Id
                 //select new { dStatus, declaration ,tooltip="" }
             );

            int HOLD = 0;
            int ALL = 0;
            int DOC = 0;
            int DOC_U = 0;
            int DOC_C = 0;
            int DOC_I = 0;
            int DOC_V = 0;
            int SVG = 0;
            int MNF = 0;
            int MNF_C = 0;
            int MNF_W = 0;
            int MNF_I = 0;
            int MNF_V = 0;
            int DEC = 0;
            int DEC_C = 0;
            int DEC_W = 0;
            int DEC_I = 0;
            int DEC_V = 0;
            int PAY = 0;
            int MNFR = 0;
            int DECR = 0;
            int PAYReadyNotFastindividual = 0;
            int ACC = 0;
            int ACC_W = 0;
            int ACC_WS = 0;
            int MNFR_RV = 0;
            int DECR_RV = 0;
            int PAY_C = 0;
            int PAY_R = 0;
            int PAY_I = 0;

            var totQ =
            (from dStatus in q
             group dStatus by 1 into g
             select new
             {
                 aLL = g.Count(),
                 dOC = g.Count(r => r.DocumentStatusCode == "M" || r.DocumentStatusCode == "X"),
                 DOC_U = g.Count(r => r.DocumentStatusCode == "X"),
                 DOC_C = g.Count(r => r.DocumentStatusCode == "M"),
                 DOC_I = g.Count(r => r.DocumentStatusCode == "I"),
                 DOC_V = g.Count(r => r.DocumentStatusCode == "V"),
                 sVG = g.Count(r => (r.IsCourierMissingClassification == true)),
                 MNF = g.Count(r => (r.CourierManifestStatusCode == "M" || r.CourierManifestStatusCode == "X")),
                 MNF_C = g.Count(r => (r.CourierManifestStatusCode == "M")),
                 MNF_W = g.Count(r => (r.CourierManifestStatusCode == "X")),
                 MNF_I = g.Count(r => (r.CourierManifestStatusCode == "I")),
                 MNF_V = g.Count(r => (r.CourierManifestStatusCode == "V")),
                 dEC = g.Count(r => (r.CourierDeclarationStatusCode == "M" || r.CourierDeclarationStatusCode == "X")),
                 dEC_C = g.Count(r => (r.CourierDeclarationStatusCode == "M")),
                 dEC_W = g.Count(r => (r.CourierDeclarationStatusCode == "X")),
                 dEC_I = g.Count(r => (r.CourierDeclarationStatusCode == "I")),
                 dEC_V = g.Count(r => (r.CourierDeclarationStatusCode == "V")),
                 pAY = g.Count(r => (r.CourierPaymentStatusCode == "R")),
                 pAY_I = g.Count(r => (r.CourierPaymentStatusCode == "I")),
                 pAY_C = g.Count(r => (r.CourierPaymentStatusCode == "R") || (r.CourierPaymentStatusCode == "P")),
                 //PAY_RL = g.Count(r => (r.CourierPaymentStatusCode == "R" && r.HighLowValue=="L")),
                 PAYReadyNotFastindividual = g.Count(r => (r.CourierPaymentStatusCode == "R" && r.FastIndividualProcessCode == "F")),//Task 47220: שינוי לוגיקת תשלום מרוכז 
                 MNFR = g.Count(r => (r.CourierManifestStatusCode == "R")),
                 MNFR_RV = g.Count(r => (r.CourierManifestStatusCode == "R" || r.CourierManifestStatusCode == "V") && r.CourierPaymentStatusCode != "P"),
                 DECR = g.Count(r => (r.CourierDeclarationStatusCode == "R")),
                 DECR_RV = g.Count(r => (r.CourierDeclarationStatusCode == "R" || r.CourierDeclarationStatusCode == "V")),
                 //HOLD = g.Count(r => (r.CourierPendingReasonCode != null)),
                 HOLD = g.Count(r => (r.CourierPendingReasonList != null)),
                 ACC = g.Count(r => (r.StorageSiteStatusCode == "2" || r.SpecialActionStatus == "X")),
                 ACC_W = g.Count(r => (r.StorageSiteStatusCode == "2")),
                 ACC_WS = g.Count(r => (r.SpecialActionStatus == "X")),
             });

            var tot =totQ.FirstOrDefault();
            if (tot != null)
            {
                HOLD = tot.HOLD;
                ALL = tot.aLL;
                DOC = tot.dOC;
                DOC_U = tot.DOC_U;
                DOC_C = tot.DOC_C;
                DOC_I = tot.DOC_I;
                DOC_V = tot.DOC_V;
                SVG = tot.sVG;
                MNF = tot.MNF;
                MNF_C = tot.MNF_C;
                MNF_W = tot.MNF_W;
                MNF_I = tot.MNF_I;
                MNF_V = tot.MNF_V;
                DEC = tot.dEC;
                DEC_C = tot.dEC_C;
                DEC_W = tot.dEC_W;
                DEC_I = tot.dEC_I;
                DEC_V = tot.dEC_V;
                PAY = tot.pAY;
                MNFR = tot.MNFR;
                DECR = tot.DECR;
                MNFR_RV = tot.MNFR_RV;
                DECR_RV = tot.DECR_RV;
                PAYReadyNotFastindividual = tot.PAYReadyNotFastindividual;
                ACC = tot.ACC;
                ACC_W = tot.ACC_W;
                ACC_WS = tot.ACC_WS;
                PAY_I = tot.pAY_I;
                PAY_C = tot.pAY_C;
            }

            keyValuePairList.Add(new KeyValuePair<string, int>("ALL", ALL));
            keyValuePairList.Add(new KeyValuePair<string, int>("DOC", DOC));
            keyValuePairList.Add(new KeyValuePair<string, int>("DOC_U", DOC_U));
            keyValuePairList.Add(new KeyValuePair<string, int>("DOC_C", DOC_C));
            keyValuePairList.Add(new KeyValuePair<string, int>("DOC_I", DOC_I));
            keyValuePairList.Add(new KeyValuePair<string, int>("DOC_V", DOC_V));
            keyValuePairList.Add(new KeyValuePair<string, int>("SVG", SVG));
            keyValuePairList.Add(new KeyValuePair<string, int>("MNF", MNF));
            keyValuePairList.Add(new KeyValuePair<string, int>("MNF_C", MNF_C));
            keyValuePairList.Add(new KeyValuePair<string, int>("MNF_W", MNF_W));
            keyValuePairList.Add(new KeyValuePair<string, int>("MNF_I", MNF_I));
            keyValuePairList.Add(new KeyValuePair<string, int>("MNF_V", MNF_V));
            keyValuePairList.Add(new KeyValuePair<string, int>("DEC", DEC));
            keyValuePairList.Add(new KeyValuePair<string, int>("DEC_C", DEC_C));
            keyValuePairList.Add(new KeyValuePair<string, int>("DEC_W", DEC_W));
            keyValuePairList.Add(new KeyValuePair<string, int>("DEC_I", DEC_I));
            keyValuePairList.Add(new KeyValuePair<string, int>("DEC_V", DEC_V));
            keyValuePairList.Add(new KeyValuePair<string, int>("PAY", PAY));
            keyValuePairList.Add(new KeyValuePair<string, int>("PAY_I", PAY_I));
            keyValuePairList.Add(new KeyValuePair<string, int>("PAY_C", PAY_C));
            keyValuePairList.Add(new KeyValuePair<string, int>("HOLD", HOLD));
            keyValuePairList.Add(new KeyValuePair<string, int>("DECR", DECR));
            keyValuePairList.Add(new KeyValuePair<string, int>("MNFR", MNFR));
            keyValuePairList.Add(new KeyValuePair<string, int>("DECR_RV", DECR_RV));
            keyValuePairList.Add(new KeyValuePair<string, int>("MNFR_RV", MNFR_RV));
            keyValuePairList.Add(new KeyValuePair<string, int>("PAYReadyNotFastindividual", PAYReadyNotFastindividual));
            keyValuePairList.Add(new KeyValuePair<string, int>("ACC", ACC));
            keyValuePairList.Add(new KeyValuePair<string, int>("ACC_W", ACC_W));
            keyValuePairList.Add(new KeyValuePair<string, int>("ACC_WS", ACC_WS));
        }

        public IQueryable<DeclarationPM> GetNotConnectedDeclaratins(QueryOperations queryOperations, int tenant)
        {
            string CourierSearchField = null;
            QueryFilterItem CourierSearchFieldFilter = queryOperations.QueryFilterItems.Where(d => d.FieldName == "CourierSearchFields").FirstOrDefault();
            if (CourierSearchFieldFilter != null)
            {
                CourierSearchField = CourierSearchFieldFilter.FieldValue.ToString();
            }

            DeclarationRepository declarationRep = new DeclarationRepository(tenant);
            IQueryable<Declaration> declarations = declarationRep.GetNotConnectedDeclaratins( tenant);

            IQueryable<DeclarationPM> declarationPMs;
            if (string.IsNullOrWhiteSpace(CourierSearchField))
            {
                declarationPMs = (from a in declarations
                                  select new DeclarationPM()
                                  {
                                      DeclarationNumber = a.DeclarationNumber,
                                      CustomFileNo = a.CustomFileNo,
                                      Id = a.Id,
                                      Tenant = a.Tenant,
                                      DeclarationStatusTypeName = a.DeclarationStatusType != null ? a.DeclarationStatusType.LocalName : null,
                                      CustomerName = a.CustomerCard != null ? a.CustomerCard.LocalName : null,
                                      ManifestNumber = a.CourierHAWB,
                                      CourierHAWB = a.CourierHAWB,
                                      SearchFields = a.SearchFields
                                  });
            }
            else
            {
                declarationPMs = (from a in declarations
                                  where a.CourierSearchFields.Contains(CourierSearchField)
                                  select new DeclarationPM()
                                    {
                                        DeclarationNumber = a.DeclarationNumber,
                                        CustomFileNo = a.CustomFileNo,
                                        Id = a.Id,
                                        Tenant = a.Tenant,
                                        DeclarationStatusTypeName = a.DeclarationStatusType != null ? a.DeclarationStatusType.LocalName : null,
                                        CustomerName = a.CustomerCard != null ? a.CustomerCard.LocalName : null,
                                        ManifestNumber = a.CourierHAWB,
                                        CourierHAWB = a.CourierHAWB,
                                        SearchFields = a.SearchFields
                                    });
            }
            return declarationPMs;
        }

        public CourierMasterPM GetSingleByAirlineAWBs(string airlineId, string hawb, string mawb, int tenant)
        {
            CourierMasterRepository courierMasterRepository = new CourierMasterRepository(context);
            CourierMasterPM courierMasterPM = null;
            var poco = courierMasterRepository.GetCourierMaster(airlineId, hawb, mawb, tenant);
            if (poco != null)
            {
                courierMasterPM = this.GetEntityPM(poco, false, null);
            }
            return courierMasterPM;
        }

    }
}
