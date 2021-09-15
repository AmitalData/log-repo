using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.QuoteModel.EntityPMs;
using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.QuoteModel.Repositories;
using System.Linq;

namespace Logitude.BL.QuoteModel.EntityQueries
{
    public class QuoteComputedFieldQuery
    {
        QuoteComputedFieldRepository repository;

        public QuoteComputedFieldQuery (int tenant)
        {
            repository = new QuoteComputedFieldRepository(tenant);
        }

        public QuoteComputedFieldQuery(QuoteComputedFieldRepository repository)
        {
            this.repository = repository;
        }

        public QuoteComputedFieldPM GetSinglePM(string id)
        {
            return (from quoteComputedField in repository.context.QuoteComputedField
                    where quoteComputedField.Id == id
                    select new QuoteComputedFieldPM()
                    {
                        Tenant = quoteComputedField.Tenant,
                        ConnectedToShipment = quoteComputedField.ConnectedToShipment,
                        ConnectedToTicket = quoteComputedField.ConnectedToTicket,
                        DeliveryTo = quoteComputedField.DeliveryTo,
                        PickupFrom = quoteComputedField.PickupFrom,
                        FromLocation = quoteComputedField.FromLocation ,
                        ToLocation = quoteComputedField.ToLocation,
                        EstimatedPayablesInSales = quoteComputedField.EstimatedPayablesInSales,
                        EstimatedPayablesInLocal = quoteComputedField.EstimatedPayablesInLocal,
                        EstimatedReceivablesInLocal = quoteComputedField.EstimatedReceivablesInLocal,
                        EstimatedReceivablesInSales = quoteComputedField.EstimatedReceivablesInSales,
                        MarkupPercentage = quoteComputedField.MarkupPercentage,
                    }).FirstOrDefault();
        }
        public IQueryable<QuoteComputedFieldList> GetIQueryableEntityList(IQueryable<QuoteComputedField> iQueryable)
        {
            IQueryable<QuoteComputedFieldList> result = (from quoteComputedField in iQueryable
                                                             select new QuoteComputedFieldList()
                                                             {
                                                                 Id = quoteComputedField.Id,
                                                                 Tenant = quoteComputedField.Tenant,
                                                                 ConnectedToShipment = quoteComputedField.ConnectedToShipment,
                                                                 ConnectedToTicket = quoteComputedField.ConnectedToTicket,
                                                                 DeliveryTo = quoteComputedField.DeliveryTo,
                                                                 PickupFrom = quoteComputedField.PickupFrom,
                                                                 FromLocation = quoteComputedField.FromLocation,
                                                                 ToLocation = quoteComputedField.ToLocation,
                                                                 EstimatedPayablesInSales = quoteComputedField.EstimatedPayablesInSales,
                                                                 EstimatedPayablesInLocal = quoteComputedField.EstimatedPayablesInLocal,
                                                                 EstimatedReceivablesInLocal = quoteComputedField.EstimatedReceivablesInLocal,
                                                                 EstimatedReceivablesInSales = quoteComputedField.EstimatedReceivablesInSales,
                                                                 MarkupPercentage = quoteComputedField.MarkupPercentage,
                                                             });
            return result;
        }


    }
}
