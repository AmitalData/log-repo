using Simplog.Data.QuoteModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Logitude.BL.QuoteModel.CustomFilters
{
    public class DigitalPortalCustomFilter
    {
        public static IQueryable<Quote> ApplySearchFilter(QueryFilterItem item, IQueryable<Quote> queryableData)
        {
            string digitalPortalSearchFields = item.FieldValue as string;
            digitalPortalSearchFields = digitalPortalSearchFields.ToLower().Trim();
            queryableData = queryableData.Where(d =>
                  d.QuoteNumber.Contains(digitalPortalSearchFields)
           );

            return queryableData;
        }

        public static IQueryable<Quote> ApplyTransportModeFilter(QueryFilterItem item, IQueryable<Quote> queryableData, int tenant)
        {
            var transportModesString = item.FieldValue as string;
            var shipmentTypesString = item.FieldValue2 as string;
            var shipmentSubTypesString = item.FieldValue3 as string;

            List<string> transportModes = !string.IsNullOrWhiteSpace(transportModesString)
                                          ? transportModesString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                .ToList()
                                          : new List<string>();

            List<string> shipmentTypes = !string.IsNullOrWhiteSpace(shipmentTypesString)
                                         ? shipmentTypesString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                              .ToList()
                                         : new List<string>();

            List<string> shipmentSubTypes = !string.IsNullOrWhiteSpace(shipmentSubTypesString)
                                            ? shipmentSubTypesString.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                                                    .ToList()
                                            : new List<string>();
            var values = new List<string>();
            var subTypesList = new List<ShipmentSubType>();
            if (shipmentSubTypes.Any())
            {
                subTypesList = GetShipmentTypes(tenant);
            }

            foreach (var tm in transportModes)
            {
                if (tm.Equals("a", StringComparison.InvariantCultureIgnoreCase))
                {
                    var airCodes = new List<string> { "Air" };

                    if (shipmentSubTypes.Any())
                    {
                        var airSubTypesIds = subTypesList.Where(a => airCodes
                                                               .Contains(a.ShipmentTypeCode, StringComparer.InvariantCultureIgnoreCase))
                                                         .Select(a => a.Id)
                                                         .ToList();

                        var airSubTypes = shipmentSubTypes.Where(a => airSubTypesIds
                                                                     .Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                         .ToList();
                        if (airSubTypes.Any())
                        {
                            airSubTypes.ForEach(a => values.Add($"A:Air:{a}"));
                        }
                        else
                        {
                            values.Add($"A:Air");
                        }
                    }
                    else
                    {
                        values.Add($"A:Air");
                    }
                }
                else if (tm.Equals("o", StringComparison.InvariantCultureIgnoreCase))
                {
                    var oceanCodes = new List<string> { "FCL", "FCLD", "LCL", "LCLD", "MyGO" };
                    var orderedOccen = shipmentTypes.Where(a => oceanCodes
                                                                .Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                    .ToList();

                    if (shipmentSubTypes.Any())
                    {
                        var oceanSubTypesIds = subTypesList.Where(a => oceanCodes
                                                                    .Contains(a.ShipmentTypeCode, StringComparer.InvariantCultureIgnoreCase))
                                                        .Select(a => a.Id)
                                                        .ToList();

                        var oceanSubTypes = shipmentSubTypes.Where(a => oceanSubTypesIds
                                                                     .Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                         .ToList();

                        if (oceanSubTypes.Any())
                        {
                            foreach (var oceanSubType in oceanSubTypes)
                            {
                                orderedOccen.ForEach(a => values.Add($"O:{a}:{oceanSubType}"));
                            }
                        }
                        else
                        {
                            orderedOccen.ForEach(a => values.Add($"O:{a}"));
                        }
                    }
                    else
                    {
                        if (orderedOccen.Any())
                        {
                            orderedOccen.ForEach(a => values.Add($"O:{a}"));
                        }
                        else
                        {
                            oceanCodes.ForEach(a => values.Add($"O:{a}"));
                        }
                    }
                }
                else if (tm.Equals("i", StringComparison.InvariantCultureIgnoreCase))
                {
                    var inlandCodes = new List<string> { "FTL", "LTL", "MyGI" };
                    var orderedInlnad = shipmentTypes.Where(a => inlandCodes
                                                                 .Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                     .ToList();

                    if (shipmentSubTypes.Any())
                    {
                        var inlandSubTypesIds = subTypesList.Where(a => inlandCodes
                                                                  .Contains(a.ShipmentTypeCode, StringComparer.InvariantCultureIgnoreCase))
                                                      .Select(a => a.Id)
                                                      .ToList();

                        var inlandSubTypes = shipmentSubTypes.Where(a => inlandSubTypesIds
                                                                     .Contains(a, StringComparer.InvariantCultureIgnoreCase))
                                                         .ToList();
                        if (inlandSubTypes.Any())
                        {
                            foreach (var inlandSubtype in inlandSubTypes)
                            {
                                orderedInlnad.ForEach(a => values.Add($"I:{a}:{inlandSubtype}"));
                            }
                        }
                        else
                        {
                            orderedInlnad.ForEach(a => values.Add($"I:{a}"));
                        }

                    }
                    else
                    {
                        if (orderedInlnad.Any())
                        {
                            orderedInlnad.ForEach(a => values.Add($"I:{a}"));
                        }
                        else
                        {
                            inlandCodes.ForEach(a => values.Add($"I:{a}"));
                        }
                    }
                }
            }

            queryableData = queryableData.Where(a => values.Any(v => (a.TransportModeId
                                                                       + ":"
                                                                       + a.ShipmentTypeId
                                                                       + ":"
                                                                       + a.ShipmentSubTypeId)
                                                                      .Contains(v)));

            return queryableData;
        }


        public static IQueryable<Quote> ApplyStatusFilter(QueryFilterItem item, IQueryable<Quote> queryableData)
        {
            var values = item.FieldValue?.ToString().Split(',').ToList();
            if (values == null || !values.Any()) return queryableData;

            List<Expression<Func<Quote, bool>>> predicate = new List<Expression<Func<Quote, bool>>>();

            foreach (var value in values)
            {
                switch (value)
                {
                    case "WaitingForApproval":
                        predicate.Add(x => x.Stage.Code == "QTST");
                        break;

                    case "Accepted":
                        predicate.Add(x => x.Stage.Code == "QTAC");
                        break;

                    case "Rejected":
                        predicate.Add(x => x.Stage.Code == "QTDC");
                        break;

                    case "Expired":
                        predicate.Add(x => x.ExpirationDate < DateTime.Now && !x.IsCancelled && !x.IsClosed);
                        break;
                }
            }
            queryableData = Any<Quote>(queryableData, predicate.ToArray());

            return queryableData;
        }

        public static IQueryable<T> Any<T>(IQueryable<T> q, params Expression<Func<T, bool>>[] preds)
        {
            var par = Expression.Parameter(typeof(T), "[Extent1]");

            Expression<Func<T, bool>> expr1 = x => false;

            foreach (var expr2 in preds)
            {
                var secondBody = new ReplaceVisitor(expr2.Parameters[0], expr1.Parameters[0]).Visit(expr2.Body);
                expr1 = Expression.Lambda<Func<T, bool>>(Expression.OrElse(expr1.Body, secondBody), expr1.Parameters);
            }

            return q.Where(expr1);
        }

        private static List<ShipmentSubType> GetShipmentTypes(int tenant)
        {
            IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            ShipmentSubTypeRepository shipmentSubTypeRepository = new ShipmentSubTypeRepository(context);
            IQueryable<ShipmentSubType> shipmentSubTypes = shipmentSubTypeRepository.GetShipmentSubTypes(tenant);
            return shipmentSubTypes?.ToList();
        }

        public static IQueryable<Quote> ApplyFromToFilter(QueryFilterItem item, IQueryable<Quote> queryableData, bool isToCountry = false)
        {
            string value = item.FieldValue as string;
            if (isToCountry) return queryableData.Where(x => x.ToPort != null && x.ToPort.CountryId == value);
            return queryableData.Where(x => x.FromPort != null && x.FromPort.CountryId == value);
        }
    }

    internal class ReplaceVisitor : ExpressionVisitor
    {
        private readonly Expression from, to;

        public ReplaceVisitor(Expression from, Expression to)
        {
            this.from = from;
            this.to = to;
        }

        public override Expression Visit(Expression node)
        {
            return node == from ? to : base.Visit(node);
        }
    }
}
