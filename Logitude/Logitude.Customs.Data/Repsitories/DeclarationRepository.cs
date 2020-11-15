
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



        public Declaration GetDeclarationNotAmendmentDontDisplayInList(string id,string amendmentOriginalDeclartation,  int tenant)
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

            return (from a in context.Declarations
                    where ((a.DeclarationNumber == id && a.AmendmentDontDisplayInList == false) || (a.AmendmentOriginalDeclartation == id && a.AmendmentDontDisplayInList == false))
                    && a.Tenant == tenant
                    select a).FirstOrDefault();
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
                         where a.Tenant == tenant && a.AmendmentRequestNumber!= null
                        select a.AmendmentRequestNumber).ToList();

            int max = 0;

            if (list.Count() != 0)
                max = list.Select(int.Parse).ToList().Max();

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
        public List<Declaration> GetDeclarationsByIdAndClientID(List<string> declarationIds,string clientID)
        {
            DateTime month3ago = DateTime.Now.AddDays(-90);
            List<Declaration> declarations = (from a in context.Declarations
                                              where a.CustomerId==clientID && a.CreateDateTime > month3ago && declarationIds.Contains(a.Id)
                                              select a).ToList();

            return declarations;

        }

        public Declaration GetDeclarationByFunctionalReferenceID( string functionalReferenceID)
        {
            //Declaration declarationParent = (from a in context.Declarations
            //                           where declarationNumber == a.DeclarationNumber
            //                           select a).FirstOrDefault();


            Declaration declaration = (from a in context.Declarations
                                              where functionalReferenceID ==a.AmendmentRequestNumber
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
        public List<string> GetIdsThatIsChanged(int tenant,List<string> DecIds)
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


        public Declaration GetLastDeclarationByDeclarationId(string id, int tenant)
        {
            if (String.IsNullOrWhiteSpace(id)) return null;
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            return
                  (
                  from rec in context.Declarations
                  where rec.AmendmentOriginalDeclartation == id && rec.Tenant == tenant
                  select rec
                  ).OrderByDescending(x=>x.CreateDateTime)
                  .FirstOrDefault();
        }

        public List<Declaration> GetDeclarationAmendmentsById(int tenant, string id)
        {

            if (String.IsNullOrWhiteSpace(id)) return null;
            (context as IObjectContextAdapter).ObjectContext.ContextOptions.UseCSharpNullComparisonBehavior = false; //Pasted from <http://stackoverflow.com/questions/682429/how-can-i-query-for-null-values-in-entity-framework?lq=1> 

            Declaration declaration = GetSingleDeclarationById( id  , tenant);
           
            if (declaration.IsAmendment== true)
            {      Declaration declarationOrg = GetSingleDeclarationById(declaration.AmendmentOriginalDeclartation, tenant);

                var myQ = (from a in context.Declarations
                           where (a.AmendmentOriginalDeclartation == declarationOrg.Id || a.Id== declarationOrg.Id ) && a.Id != id
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


    }
    //class TotM {

}
  

