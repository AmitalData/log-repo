
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Logitude.Customs.Data.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System.Data.Entity.Infrastructure;
using System.Data.Entity;

namespace Logitude.Customs.Data.Repsitories
{
    public partial class DeclarationRepository : IRepository<Declaration>
    {
        partial void onUpdate()
        {

        }



        public List<Declaration> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }

        public Declaration GetSingleDeclarationByNumber(string number, int tenant)
        {

            //SELECT * FROM AMINEt_MAIN.Declarations Extent1 WHERE((Extent1.DeclarationNumber = :p__linq__0) OR ((Extent1.DeclarationNumber IS NULL) AND(:p__linq__0 IS NULL))) AND(Extent1.Tenant = :p__linq__1)
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return (from a in context.Declarations
                    where a.DeclarationNumber == number && a.Tenant == tenant
                    select a).FirstOrDefault();
        }



        public Declaration GetDeclarationNotAmendmentDontDisplayInList(string id, string amendmentOriginalDeclartation, int tenant)
        {

            //SELECT * FROM AMINEt_MAIN.Declarations Extent1 WHERE((Extent1.DeclarationNumber = :p__linq__0) OR ((Extent1.DeclarationNumber IS NULL) AND(:p__linq__0 IS NULL))) AND(Extent1.Tenant = :p__linq__1)
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return (from a in context.Declarations
                    where ((a.Id == id && a.AmendmentDontDisplayInList == false) || (a.Id == amendmentOriginalDeclartation && a.AmendmentDontDisplayInList == false))
                    && a.Tenant == tenant
                    select a).FirstOrDefault();
        }
        public Declaration GetAcceptDeclarationAmendment(string id, int tenant)
        {


            //SELECT * FROM AMINEt_MAIN.Declarations Extent1 WHERE((Extent1.DeclarationNumber = :p__linq__0) OR ((Extent1.DeclarationNumber IS NULL) AND(:p__linq__0 IS NULL))) AND(Extent1.Tenant = :p__linq__1)
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 


            bool newBL = true;
            if (newBL)// TRING  FILENO=60255210
            {
                var qCustomFileNo = context.Declarations
    .Where(r => r.Id == id)
    .Where(r => r.Tenant == tenant)
    .Select(r => r.CustomFileNo);
                var qAllCustomFileNo =
                    (
                from c in qCustomFileNo
                join d in context.Declarations
                on c equals d.CustomFileNo
                select d
                    );

                //var qGetAcceptDeclarationAmendment =
                //    (
                //from dec in qAllCustomFileNo.Where(r => r.Tenant == tenant)
                //where
                //(
                //(dec.Id == id && dec.AmendmentDontDisplayInList == false && dec.DeclarationNumber != null) ||
                //(dec.AmendmentOriginalDeclartation == id && dec.DeclarationNumber != null && dec.AmendmentDontDisplayInList == false)
                //)
                //select dec
                //);

                var allDecSameFile = qAllCustomFileNo.ToList();
                var qGetAcceptDeclarationAmendment = (from a in allDecSameFile
                                                      where ((a.Id == id && a.AmendmentDontDisplayInList == false && a.DeclarationNumber != null) ||
                            (a.AmendmentOriginalDeclartation == id && a.DeclarationNumber != null && a.AmendmentDontDisplayInList == false))
                            && a.Tenant == tenant
                                                      select a
                                                );

                return qGetAcceptDeclarationAmendment.FirstOrDefault();
            }
            else
            {
                return (from a in context.Declarations
                        where ((a.Id == id && a.AmendmentDontDisplayInList == false && a.DeclarationNumber != null) ||
                        (a.AmendmentOriginalDeclartation == id && a.DeclarationNumber != null && a.AmendmentDontDisplayInList == false))
                        && a.Tenant == tenant
                        select a).FirstOrDefault();
            }




        }


        public Declaration GetAcceptDeclarationAmendmentByCustomsFile(string customFileNo, int tenant)
        {

            //SELECT * FROM AMINEt_MAIN.Declarations Extent1 WHERE((Extent1.DeclarationNumber = :p__linq__0) OR ((Extent1.DeclarationNumber IS NULL) AND(:p__linq__0 IS NULL))) AND(Extent1.Tenant = :p__linq__1)
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return (from a in context.Declarations
                    where (a.CustomFileNo == customFileNo && a.AmendmentDontDisplayInList == false && a.DeclarationNumber != null)
                    && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public int GetDeclarationMaxCancelRequestNumber(int tenant, string id)
        {
            // && a.Id==id

            var list = (from a in context.Declarations
                        where a.Tenant == tenant && a.CancelRequestNumber != null
                        select a.CancelRequestNumber).ToList();

            int? max = 0;

            if (list.Count() != 0)
                max = list.Max();

            return Convert.ToInt32(max);
        }

        public int GetDeclarationMaxAmendmentRequestNumber(int tenant)
        {
            //var x=  (from a in context.Declarations
            //         where a.Tenant == tenant
            //         select Convert.ToInt32(a.AmendmentRequestNumber)).Max();


            //  return (from a in context.Declarations
            //          where  a.Tenant == tenant
            //          select a).Max(rec => Convert.ToInt32( rec.AmendmentRequestNumber));

            var list = (from a in context.Declarations
                        where a.Tenant == tenant && (a.AmendmentRequestNumber != null || a.CancelRequestNumber != null)
                        select new { a.AmendmentRequestNumber, a.CancelRequestNumber }).ToList();

            int max = 0;

            if (list.Count() != 0) {
                var maxAmendmentRequestNumber = list.Select(r => (int.TryParse(r.AmendmentRequestNumber, out var a)) ?int.Parse(r.AmendmentRequestNumber):0).ToList().Max();
                var maxCancelRequestNumber = list.Select(r => ((r.CancelRequestNumber).HasValue)?r.CancelRequestNumber.Value:0).ToList().Max();
                max = (maxAmendmentRequestNumber > maxCancelRequestNumber) ? maxAmendmentRequestNumber : maxCancelRequestNumber;
            }

            return max;
        }


        public void GetDailyStatistic(int tenant,
            out int TotDec,
            out int TotDecPay,
            out int TotDecOpen2Date,

            out int TotLastMonthCreatedByUserId,
            out int TotLastMonthReferentUserId,



            out int LastMonthTotDecStandAlone,
            out int LastMonthTotdecIsConnectedToUnf,

            out DateTime LastPaidDeclarationDate,
            out int TotWithHATARA
            )
        {
            TotLastMonthCreatedByUserId =
                TotLastMonthReferentUserId =
            TotWithHATARA = TotDecOpen2Date = TotDecPay = TotDec = -1;
            LastMonthTotDecStandAlone = LastMonthTotdecIsConnectedToUnf = -1;
            LastPaidDeclarationDate = DateTime.MinValue;

            var weekAgo = DateTime.Now.Date;//.AddDays(-7);
            var monthAgo = DateTime.Now.Date;//.AddMonths(-1);

            var qOpenLastWeek =
                (from a in context.Declarations
                 where
                 a.Tenant == tenant &&
                 a.CreateDateTime >= weekAgo
                 select a);

            var qOpenLastMonth =
                (from a in context.Declarations
                 where
                 a.Tenant == tenant &&
                 a.CreateDateTime >= monthAgo
                 select a);

            //var qOpenFrom =
            //    (
            //    from a in qOpenLastMonth
            //    group a by a.IsConnectedToUnifreight into g
            //    select new { g.Key, tot = g.Count() }
            //    );


            var qLastMonthTotDecStandAlone =
                (
                from a in qOpenLastMonth
                where a.IsConnectedToUnifreight == false
                select a
                );
            var qLastMonthTotdecIsConnectedToUnf =
                (
                from a in qOpenLastMonth
                where a.IsConnectedToUnifreight == true
                select a
                );

            var qOpenLastMonthCreatedByUserId =
                (
                from a in qOpenLastMonth
                select a.CreatedByUserId
                ).Distinct();
            var qOpenLastMonthReferentUserId =
                (
                from a in qOpenLastMonth
                select a.ReferentUserId
                ).Distinct();
            //var qOpenLastMonthCreatedByUserIdCount = qOpenLastMonthCreatedByUserId.




            var qLastPaidDeclarationDate = (from a in qOpenLastMonth
                                            where a.PaymentDate.HasValue &&
                                            //!string.IsNullOrWhiteSpace(a.PaymentOrderNumber)
                                            (a.PaymentOrderNumber != null || a.PaymentOrderNumber.Trim() != string.Empty)

                                            orderby a.PaymentDate descending
                                            select a.PaymentDate
                     );
            var qAllDecWithHATARA =
                (
                from a in context.Declarations
                where a.HatraDate.HasValue
                select a.Id
                ).Distinct(); ;
            var qq = (
                 from a in qOpenLastWeek
                 group a by 1 into gOpenLastWeek
                 select new
                 {
                     Tot = 0,
                     TotHATARA = 0,
                     TotPayment = 0,
                     TotOpenLastWeek = gOpenLastWeek.Count(),

                     TotLastMonthCreatedByUserId = 0,
                     TotLastMonthReferentUserId = 0,
                     LastMonthTotDecStandAlone = 0,
                     LastMonthTotdecIsConnectedToUnf = 0,
                     LastPaidDeclarationDate = DateTime.MinValue,
                 }
                 ).Concat(

                 from b in context.Declarations
                .Where(r => r.Tenant == tenant)
                .Where(r => r.PaymentDate.HasValue)
                 group b by 1 into gPaymentDate
                 select new //TotM()
                 {
                     Tot = 0,
                     TotHATARA = 0,
                     TotPayment = gPaymentDate.Count(),
                     TotOpenLastWeek = 0,
                     TotLastMonthCreatedByUserId = 0,
                     TotLastMonthReferentUserId = 0,
                     LastMonthTotDecStandAlone = 0,
                     LastMonthTotdecIsConnectedToUnf = 0,
                     LastPaidDeclarationDate = DateTime.MinValue,
                 }
                 ).Concat(
                 from c in context.Declarations
                 group c by 1 into gTot
                 select new //TotM()
                 {
                     Tot = gTot.Count(),
                     TotHATARA = qAllDecWithHATARA.Count(),
                     TotPayment = 0,
                     TotOpenLastWeek = 0,
                     TotLastMonthCreatedByUserId = qOpenLastMonthCreatedByUserId.Count(),
                     TotLastMonthReferentUserId = qOpenLastMonthReferentUserId.Count(),

                     LastMonthTotDecStandAlone = qLastMonthTotDecStandAlone.Count(),
                     LastMonthTotdecIsConnectedToUnf = qLastMonthTotdecIsConnectedToUnf.Count(),
                     LastPaidDeclarationDate = DateTime.MinValue,
                 }
                 )

                 ;

            var list = qq.ToList();
            if (list.Count > 0)
            {
                TotDec = list.Max(a => a.Tot);
                TotWithHATARA = list.Max(a => a.TotHATARA);
                TotDecPay = list.Max(a => a.TotPayment);
                TotDecOpen2Date = list.Max(a => a.TotOpenLastWeek);
                TotLastMonthCreatedByUserId = list.Max(a => a.TotLastMonthCreatedByUserId);
                TotLastMonthReferentUserId = list.Max(a => a.TotLastMonthReferentUserId);
                LastMonthTotDecStandAlone = list.Max(a => a.LastMonthTotDecStandAlone);
                LastMonthTotdecIsConnectedToUnf = list.Max(a => a.LastMonthTotdecIsConnectedToUnf);
                LastPaidDeclarationDate = qLastPaidDeclarationDate.FirstOrDefault().GetValueOrDefault();//  list.Max(A => A.LastPaidDeclarationDate); 
            }

            //return null;
        }
        //<--- Yuval Chalup 19.11.2015 TASK-17450
        public IQueryable<Declaration> GetSingleDeclarationPMByNumber(string number, int tenant)
        {
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return (from a in context.Declarations
                    where a.DeclarationNumber == number && a.Tenant == tenant
                    select a);
        }
        //Yuval Chalup 19.11.2015 TASK-17450 --->

        public IQueryable<Declaration> GetQSingle(EntityKeyFields entityKeys)
        {
            DeclarationKeys keys = entityKeys as DeclarationKeys;
            return (from a in context.Declarations
                    where a.Id == keys.Id
                    select a);
        }
        public Declaration GetByCustomFileNo(string customFileNo, int tenant)
        {

            if (String.IsNullOrWhiteSpace(customFileNo)) return null;
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.CustomFileNo == customFileNo && rec.Tenant == tenant
                  select rec
                  )
                  .FirstOrDefault();
        }
        public string GetIdByCustomFileNo(string customFileNo, int tenant)
        {

            if (String.IsNullOrWhiteSpace(customFileNo)) return "";
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.CustomFileNo == customFileNo && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }
        public List<string> GetListByCourierHAWB(string CourierHAWB, int tenant)
        {
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 



            var q =
                  (
                  from rec in context.Declarations
                  where rec.CourierHAWB == CourierHAWB && rec.Tenant == tenant
                  select rec.Id
                  );
            return q.ToList(); ;
        }
        public string GetConcurrencyGUIDByCustomFileNo(string customFileNo, int tenant)
        {

            if (String.IsNullOrWhiteSpace(customFileNo)) return "";
            return
                  (
                  from rec in context.Declarations
                  where rec.CustomFileNo == customFileNo && rec.Tenant == tenant
                  select rec.ConcurrencyGUID
                  )
                  .FirstOrDefault();
        }


        public string GetIdByExternalDeclarationNumber(string externalDeclarationNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(externalDeclarationNumber)) return "";
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.ExternalDeclarationNumber == externalDeclarationNumber && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public string GetIdByDeclarationNumber(string declarationNumber, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationNumber)) return "";
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.DeclarationNumber == declarationNumber && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public List<Declaration> GetDeclarationsById(List<string> declarationIds)
        {
            List<Declaration> declarations = (from a in context.Declarations
                                              where declarationIds.Contains(a.Id)
                                              select a).ToList();

            return declarations;

        }

        public List<Declaration> GetDeclarationsByExportContainerizationId(string exportContainerizationID)
        {
            List<Declaration> declarations = (from a in context.Declarations
                                              where a.ExportContainerizationID == exportContainerizationID
                                              select a).ToList();

            return declarations;

        }
        public List<Declaration> GetDeclarationsByIdAndClientID(List<string> declarationIds, string clientID)
        {
            DateTime month3ago = DateTime.Now.AddDays(-90);
            List<Declaration> declarations = (from a in context.Declarations
                                              where a.CustomerId == clientID && a.CreateDateTime > month3ago && declarationIds.Contains(a.Id)
                                              select a).ToList();

            return declarations;

        }
        public IQueryable<Declaration> GetByExportContainerizationID(string exportContainerizationID, int tenant)
        {

            return (from a in context.Declarations
                    where a.ExportContainerizationID == exportContainerizationID && a.Tenant == tenant
                    select a);
        }



        public Declaration GetDeclarationByFunctionalReferenceID(string functionalReferenceID, int tenant)
        {
            //Declaration declarationParent = (from a in context.Declarations
            //                           where declarationNumber == a.DeclarationNumber
            //                           select a).FirstOrDefault();


            Declaration declaration = (from a in context.Declarations
                                       where functionalReferenceID == a.AmendmentRequestNumber && a.Tenant == tenant
                                       select a).FirstOrDefault();

            return declaration;

        }


        public List<Declaration> GetDeclarationsThatCanResend(int tenant, int take)
        {
            DateTime month2Ago = DateTime.Now.AddDays(-60);
            var statuss = new List<string>() { "12", "13" };
            var myQ = (from a in context.Declarations
                       where a.CreateDateTime > month2Ago
                       where statuss.Contains(a.DeclarationStatusTypeCode)
                       select a);
            myQ = myQ.Take(take);
            var list = myQ.ToList();
            return list;
        }
        public Dictionary<string, string> GetCustomersByDeclarationIds(List<string> declarationIds, int tenant)
        {
            Dictionary<string, string> declarationCustomers = new Dictionary<string, string>();
            Dictionary<string, string> DeclarationCustomers = new Dictionary<string, string>();

            Dictionary<string, string> result = (from a in context.Declarations
                                                 where declarationIds.Contains(a.Id) && a.Tenant == tenant
                                                 select a).ToDictionary(d => d.Id, f => f.CustomerId);


            CardRepository cardRep = new CardRepository(tenant);
            declarationCustomers = cardRep.GetCustomerNamesById(result.Values.ToList());




            foreach (var item in result)
            {
                if (item.Value != null)
                {
                    string customerName = declarationCustomers[item.Value];
                    DeclarationCustomers.Add(item.Key, customerName);
                }

            }

            //declarationCustomers = result.Select(t => new { t.Id, t.CustomerId })
            //        .ToDictionary(t => t.Id, t => t.CustomerId);

            //foreach (var item in declarationCustomers)
            // {
            //  Card customerCard = CardRepository.GetSingleCard(item.Value, tenant, true);
            //  DeclarationCustomers.Add(item.Key,customerCard.LocalName);

            // }

            return DeclarationCustomers;

        }


        public List<SupplierInvoice> GetSupplierInvoicesByDeclaration(string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoices
                    where a.DeclarationId == declarationId && a.Tenant == tenant && a.InvoiceCurrencyTypeCode != null
                    select a).ToList();
        }

        public string GetCustomFileNoByDeclarationId(string declarationId, int tenant)
        {
            if (String.IsNullOrWhiteSpace(declarationId)) return "";
            return
                  (
                  from rec in context.Declarations
                  where rec.Id == declarationId && rec.Tenant == tenant
                  select rec.CustomFileNo
                  )
                  .FirstOrDefault();
        }

        public List<string> GetCustomsFileNumbersByDeclaraionIds(List<string> declarationIds, int tenant)
        {
            List<string> declarations = (from a in context.Declarations
                                         where declarationIds.Contains(a.Id) && a.Tenant == tenant
                                         select a.CustomFileNo).ToList();

            return declarations;
        }

        public void FastDeleteMulti(DeclarationKeys entityKeyFields)
        {

            (context as DbContextBase)
                .DeleteWhere<Declaration>(rec => rec.Id == entityKeyFields.Id);
        }

        public Declaration GetSingleDeclarationById(string id, int tenant)
        {
            Declaration declaration = (from a in context.Declarations
                                       where a.Id == id && a.Tenant == tenant
                                       select a).FirstOrDefault();

            return declaration;
        }

        public int GetInvoiceItemsWithTradeAgreementCount(string declarationId, int tenant)
        {
            return (from a in context.SupplierInvoiceItems
                    where a.DeclarationId == declarationId && a.Tenant == tenant && a.TradeAgreementCode != null
                    select a).Count();
        }
        public List<string> GetIdsThatIsChanged(int tenant, List<string> DecIds)
        {

            var declarations = (from a in context.Declarations
                                where a.Tenant == tenant
                                where DecIds.Contains(a.Id)
                                where a.IsChanged
                                select a.Id
                                                    );

            return declarations.ToList();
        }

        public IQueryable<Declaration> GetCourierConnectedDeclaratins(string CourierMasterId, int tenant)
        {
            var /*List<string>*/ courierDeclarations = (from a in context.CourierDeclarations
                                                        where a.CourierMasterId == CourierMasterId && a.Tenant == tenant
                                                        select a.DeclarationId)/*.ToList()*/;

            IQueryable<Declaration> declarations = (from a in context.Declarations
                                                    where courierDeclarations.Contains(a.Id)
                                                    select a);

            return declarations;
        }

        public IQueryable<Declaration> GetNotConnectedDeclarations(int tenant)
        {
            bool joinIt = false;

#if (false)
            {
                List<string> courierDeclarations = (from a in context.CourierDeclarations
                                                    where a.CourierMasterId != null && a.Tenant == tenant
                                                    select a.DeclarationId).ToList();

                IQueryable<Declaration> declarations = (from a in context.Declarations
                                                        where !courierDeclarations.Contains(a.Id) && a.IsCourierDeclaration == true
                                                        select a);
            }
#endif
            IQueryable<Declaration> declarations = null;
            if (!joinIt)
            {
                var courierDeclarations = (from a in context.CourierDeclarations
                                           where a.CourierMasterId != null && a.Tenant == tenant
                                           select a.DeclarationId);

                declarations = (from a in context.Declarations
                                where !courierDeclarations.Contains(a.Id) && a.IsCourierDeclaration == true
                                select a);
            }
            else
            {
                declarations = (from a in context.Declarations

                                where /*!courierDeclarations.Contains(a.Id) */
                                !context.CourierDeclarations.Any(cd => cd.DeclarationId == a.Id)
                                && a.IsCourierDeclaration == true
                                select a
                 );

            }
            bool testIt = false;
            if (testIt)
            {
                var res = declarations.ToList();
            }
            return declarations;
        }


        public void SetIsChangedAndSubmitChanges(Declaration declaration)
        {
            if (!declaration.PaymentDate.HasValue)
            {
                declaration.IsChanged = true;
                this.Update(declaration);
                this.SubmitChanges();
            }
        }

        public bool GetIsValueForCustomsOnlyFromDeclaration(string declarationId, int tenant)
        {
            bool? IsValueForCustomsOnly = (from a in context.Declarations
                                           where a.Id == declarationId && a.Tenant == tenant
                                           select a.IsValueForCustomsOnly).FirstOrDefault();
            return IsValueForCustomsOnly ?? IsValueForCustomsOnly.Value;
        }

        public string GetCusomFileNoForDeclaration(string declarationId, int tenant)
        {
            Declaration dec = (from a in context.Declarations
                               where a.Id == declarationId && a.Tenant == tenant
                               select a).FirstOrDefault();

            return dec != null ? dec.CustomFileNo : null;
        }

        public Declaration GetDeclarationByDecNoAndVersion(string decNo, string version, int tenant)
        {
            if (String.IsNullOrWhiteSpace(decNo)) return null;
            if (String.IsNullOrWhiteSpace(version)) return null;

            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.DeclarationNumber == decNo && rec.VersionId == version && rec.Tenant == tenant
                  select rec
                  )
                  .FirstOrDefault();
        }

        public Declaration GetDeclarationByCustomFileNo(string customFileNo, int tenant)
        {
            if (String.IsNullOrWhiteSpace(customFileNo)) return null;
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.CustomFileNo == customFileNo && rec.Tenant == tenant
                  select rec
                  )
                  .FirstOrDefault();
        }
        public Declaration GetLastDeclarationByDeclarationId(string id, int tenant)
        {
            if (String.IsNullOrWhiteSpace(id)) return null;
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.AmendmentOriginalDeclartation == id && rec.Tenant == tenant
                  select rec
                  ).OrderByDescending(x => x.CreateDateTime)
                  .FirstOrDefault();
        }

        public List<Declaration> GetDeclarationAmendmentsById(int tenant, string id)
        {

            if (String.IsNullOrWhiteSpace(id)) return null;
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            Declaration declaration = GetSingleDeclarationById(id, tenant);

            if (declaration.IsAmendment == true)
            {
                Declaration declarationOrg = GetSingleDeclarationById(declaration.AmendmentOriginalDeclartation, tenant);

                var myQ = (from a in context.Declarations
                           where (a.AmendmentOriginalDeclartation == declarationOrg.Id || a.Id == declarationOrg.Id) && a.Id != id
                           select a);
                return myQ.ToList();
            }

            else
            {
                var myQ = (from a in context.Declarations
                           where a.AmendmentOriginalDeclartation == id
                           select a);
                return myQ.ToList();
            }
        }

        public List<DeclarationPendingBulkFeed> GetforPendingBulkFeed(string courierMasterId, string goodsDescription, string weightFrom, string weightTo, string incotermCode, string SearchFilter, string totalInvoice, string fastIndividualProcess, int? skip = null, int? take = null, string sortingCol = null, string sortingDir = null)
        {
            int weightFromInt = 0;
            int weightToInt = 0;

            var q1 = (from cd in context.CourierDeclarations.Where(cd => cd.CourierMasterId == courierMasterId)

                      join d in context.Declarations on cd.DeclarationId equals d.Id

                      join dcs in context.DeclarationCourierStatuses on d.Id equals dcs.DeclarationId

                      join cp in context.ConsignmentPackages on d.Id equals cp.DeclarationId into cpjoin
                      from cpj in cpjoin.Where(cp => cp.PackageMeasureQualifierCode == "2").DefaultIfEmpty()

                      join c in context.Clients on d.ImporterId equals c.Id into cjoin
                      from cj in cjoin.DefaultIfEmpty()

                      join s in (from temp in context.SupplierInvoices
                                 group temp by temp.DeclarationId into temp2
                                 where temp2.Count() > 0
                                 select new { DeclarationId = temp2.Key, SupplierInvoices = temp2.FirstOrDefault() })
                               on d.Id equals s.DeclarationId into sjoin
                      from sj in sjoin.DefaultIfEmpty()

                          //join s in context.SupplierInvoices on d.Id equals s.DeclarationId into sjoin
                          //let sj = context.SupplierInvoices.Where(s=> d.Id == s.DeclarationId).FirstOrDefault()
                      orderby d.Id

                      select new
                      {
                          DeclarationId = d.Id,
                          CourierHAWB = d.CourierHAWB,
                          Importername = d.ImporterName,
                          Cargodescription = d.CargoDescription,
                          Casualimporteraddress1 = d.CasualImporterAddress1,
                          Casualimporteraddress2 = d.CasualImporterAddress2,
                          Casualimportercity = d.CasualImporterCity,
                          TotalInvoiceAmountInUSD = dcs.TotalInvoiceAmountInUSD,
                          PackageMeasureQualifierCode1 = cpj != null ? cpj.PackageMeasureQualifierCode : "0",// t.AsEnumerable().Sum(cpj => int.Parse(cpj.PackageMeasureQualifierCode)),
                          Code = cj != null ? cj.Code : d.ImporterCode,
                          IncotermCode = sj != null ? sj.SupplierInvoices.IncotermCode : "",
                          CourierSearchFields = d.CourierSearchFields,
                          FastIndividualProcessCode = dcs.FastIndividualProcessCode
                      });

            if (skip.HasValue)
                q1 = q1.OrderBy(x => x.CourierHAWB).Skip(skip.Value);

            if (take.HasValue)
                q1 = q1.Take(take.Value);

            if (!string.IsNullOrEmpty(incotermCode))
                q1 = q1.Where(s => s.IncotermCode == incotermCode);

            if (!string.IsNullOrEmpty(goodsDescription))
                q1 = q1.Where(s => s.Cargodescription.Contains(goodsDescription));

            if (!string.IsNullOrEmpty(SearchFilter))
                q1 = q1.Where(s => s.CourierSearchFields.Contains(SearchFilter));


            switch (totalInvoice)
            {
                case "75":
                    {
                        q1 = q1.Where(r => r.TotalInvoiceAmountInUSD <= 75);

                        break;
                    }
                case "500":
                    {
                        q1 = q1.Where(r => r.TotalInvoiceAmountInUSD > 75 && r.TotalInvoiceAmountInUSD <= 500);
                        break;
                    }
                case "1000":
                    {
                        q1 = q1.Where(r => r.TotalInvoiceAmountInUSD > 500 && r.TotalInvoiceAmountInUSD <= 1000);

                        break;
                    }
            }

            switch (fastIndividualProcess)
            {
                case "F":
                case "I":
                    {
                        q1 = q1.Where(r => r.FastIndividualProcessCode == fastIndividualProcess);
                        break;
                    }
            }


            var res = q1.ToList();


            var q2 = res.GroupBy(d =>
                new
                {
                    d.DeclarationId,
                    d.CourierHAWB,
                    d.Importername,
                    d.Cargodescription,
                    d.Casualimporteraddress1,
                    d.Casualimporteraddress2,                    
                    d.Casualimportercity,
                    d.TotalInvoiceAmountInUSD,
                    d.PackageMeasureQualifierCode1,
                    d.Code,
                    d.IncotermCode
                }).Select(t => new DeclarationPendingBulkFeed()
                {
                    DeclarationId = t.Key.DeclarationId,
                    CourierHAWB = t.Key.CourierHAWB,
                    Importername = t.Key.Importername,
                    Cargodescription = t.Key.Cargodescription,
                    Casualimporteraddress = t.Key.Casualimporteraddress1 + " " + t.Key.Casualimporteraddress2,
                    Casualimportercity = t.Key.Casualimportercity,
                    TotalInvoiceAmountInUSD = t.Key.TotalInvoiceAmountInUSD,
                    PackageMeasureQualifierCode = t.Sum(cpj => Convert.ToInt32(cpj.PackageMeasureQualifierCode1)),
                    Code = t.Key.Code,
                    IncotermCode = t.Key.IncotermCode
                });

            if (int.TryParse(weightFrom, out weightFromInt))
                q2 = q2.Where(s => s.PackageMeasureQualifierCode >= weightFromInt);

            if (int.TryParse(weightTo, out weightToInt))
                q2 = q2.Where(s => s.PackageMeasureQualifierCode <= weightToInt);

            if (!string.IsNullOrEmpty(sortingCol))
            {
                var sortBy = new Dictionary<string, Func<IEnumerable<DeclarationPendingBulkFeed>, IEnumerable<DeclarationPendingBulkFeed>>>()
                {
                    { "CourierHAWB", lus => lus.OrderBy(lu => lu.CourierHAWB) },
                    { "Importername", lus => lus.OrderBy(lu => lu.Importername) },
                    { "Code", lus => lus.OrderBy(lu => lu.Code) },
                    { "Cargodescription", lus => lus.OrderBy(lu => lu.Cargodescription) },
                    { "IncotermCode", lus => lus.OrderBy(lu => lu.IncotermCode) },
                    { "TotalInvoiceAmountInUSD", lus => lus.OrderBy(lu => lu.TotalInvoiceAmountInUSD) },
                    { "PackageMeasureQualifierCode", lus => lus.OrderBy(lu => lu.PackageMeasureQualifierCode) },
                    { "Casualimportercity", lus => lus.OrderBy(lu => lu.Casualimportercity) },
                };
                q2 = sortBy[sortingCol](q2);

                if (sortingDir == "Descending")
                    q2 = q2.Reverse();
            };

            var res2 = q2.ToList();

            return res2;
        }

        public string GetHatraDateForDecId(string decId, int tenant)
        {
            var HatraDateQuery = (from a in context.Declarations
                                  where a.Id == decId && a.Tenant == tenant
                                  select a.HatraDate);
            return HatraDateQuery.FirstOrDefault().ToString();
        }


        public object GetDeclarationConsignment(string exportFile)
        {
            var myQ = (from d in context.Declarations
                       join c in context.Consignments on d.Id equals c.DeclarationId into cjoin
                       from cj in cjoin.DefaultIfEmpty()

                       where d.ExportFile == exportFile
                       select new { Consignment = cj , d}
                       );
            var res = myQ.Take(1).ToList().FirstOrDefault();
            return res;
        }

        public DeclarationId GetDeclarationId(string exportFileNo, string exporterNumber, string transportmodeId, string cargoIdentifierType, string cargoIdentifierKey1, string cargoIdentifierKey2, string cargoIdentifierKey3)
        {
            var myQ = (from d in context.Declarations
                       join c in context.Consignments on d.Id equals c.DeclarationId into cjoin
                       from cj in cjoin.DefaultIfEmpty()

                       where d.ExportFile == exportFileNo
                       && d.ImporterCode == exporterNumber
                       && d.TransportModeId == transportmodeId
                       && cj.CargoTypeCode== cargoIdentifierType
                       && cj.ManifestNumber == cargoIdentifierKey1
                       && cj.SecondCargoID == cargoIdentifierKey2
                       && cj.ThirdCargoID == cargoIdentifierKey3
                       select new DeclarationId { Id = d.Id }
                       );
            DeclarationId res = myQ.Take(1).ToList().FirstOrDefault();
            return res;
        }
    }


    public class DeclarationId
    {
        public string Id { get; set; }
    }


    public class DeclarationPendingBulkFeed
    {
        public string DeclarationId { get; set; }
        public string CourierHAWB { get; set; }
        public string Importername { get; set; }
        public string Cargodescription { get; set; }
        public string Casualimporteraddress { get; set; }
        
        public string Casualimportercity { get; set; }
        public decimal? TotalInvoiceAmountInUSD { get; set; }
        public int PackageMeasureQualifierCode { get; set; }
        public string Code { get; set; }
        public string IncotermCode { get; set; }

        public DeclarationPendingBulkFeed() { }

        public DeclarationPendingBulkFeed(string declarationId, string courierHAWB, string importername, string cargodescription, string casualimporteraddress, string casualimportercity, decimal? totalInvoiceAmountInUSD, int packageMeasureQualifierCode, string code, string incotermCode)
        {
            DeclarationId = declarationId;
            CourierHAWB = courierHAWB;
            Importername = importername;
            Cargodescription = cargodescription;
            Casualimporteraddress = casualimporteraddress;            
            Casualimportercity = casualimportercity;
            TotalInvoiceAmountInUSD = totalInvoiceAmountInUSD;
            PackageMeasureQualifierCode = packageMeasureQualifierCode;
            Code = code;
            IncotermCode = incotermCode;
        }
    }

    public class TempBulkFeedPending
    {
        public string Id { get; set; }
        public string Importername { get; set; }
        public string Cargodescription { get; set; }
        public string Casualimporteraddress1 { get; set; }
        public string Casualimporteraddress2 { get; set; }
        public string Casualimportercity { get; set; }
        public decimal? Totalinvoiceamountinus { get; set; }
        public string PackageMeasureQualifierCode1 { get; set; }
        public string Code { get; set; }
        public string IncotermCode { get; set; }

        public TempBulkFeedPending() { }

        public TempBulkFeedPending(string id, string importername, string cargodescription, string casualimporteraddress1, string casualimporteraddress2, string casualimportercity, decimal? totalinvoiceamountinus, string packageMeasureQualifierCode1, string code, string incotermCode)
        {
            Id = id;
            Importername = importername;
            Cargodescription = cargodescription;
            Casualimporteraddress1 = casualimporteraddress1;
            Casualimporteraddress2 = casualimporteraddress2;
            Casualimportercity = casualimportercity;
            Totalinvoiceamountinus = totalinvoiceamountinus;
            PackageMeasureQualifierCode1 = packageMeasureQualifierCode1;
            Code = code;
            IncotermCode = incotermCode;
        }
    }
    //class TotM {

}


