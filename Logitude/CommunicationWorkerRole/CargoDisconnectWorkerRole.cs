using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.CargoTracking.Data;
using Logitude.Server.Tools.KafkaConfigurations;
using Logitude.Server.Tools.Messages;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading;
using System.Linq;
using System.Collections.Generic;
using Logitude.CargoTracking.Data.EntityPOCOs;
using System.Data.Entity;
using Logitude.CargoTracking.BL.CargoTrackingServices;
using System.Data;
using System.Data.SqlClient;
using Simplog.Data.ShipmentsModel;
using System.Linq.Expressions;

namespace CommunicationWorkerRole
{
    /// <summary>
    /// this worker role to disconnect shipments, return all disconnected shipment as it was before connect
    /// In incremental process > any shipment that dose not contains the CustomfileId or any order dose not contains the shipmentId will add to queue 
    /// this worker role get all records from Queue and check if the shipments were connected in cargo or no
    /// and update just (shipments that were connected) on the logitude database
    /// the incremental process will get all updated shipments and re-create the shipments as it was before connect
    /// </summary>
    public class CargoDisconnectWorkerRole : WorkerEntryPoint
    {

        const int MaxShepmentsNumberPerTime = 100;
        private ICargoTrackingContext cargoContext;
        private IShipmentsContext shipmentsContext;
        const int MaximumNumberOfConcurrentConnections = 12;
        public override bool OnStart()
        {
            ServicePointManager.DefaultConnectionLimit = MaximumNumberOfConcurrentConnections;
            ThreadId = Guid.NewGuid().ToString();
            BatchServiceCode = "CargoDisconnec";
            cargoContext = CargoTrackingContext.GetContext(0);
            shipmentsContext = ShipmentsContext.GetContext(0);
            return base.OnStart();
        }
        public override void Run()
        {
            while (IsRunning)
            {
                Do();
            }
        }

        private void Do()
        {
            if (General.IsUpdating())
            {
                Thread.Sleep(60000);
                return;
            }
            try
            {
                DisconnectShepments();
                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Thread.Sleep(10000);
            }

        }

        private void DisconnectShepments()
        {
            var disconnectShipmentQueue = GetDisconnectQueue();
            if (disconnectShipmentQueue.Count <= 0)
                return;
            var connectedShipmentsIds = GetConnectedShipmentsIds(disconnectShipmentQueue);
            if (connectedShipmentsIds.Count > 0)
                UpdateShipmetsByIds(connectedShipmentsIds);
            RemoveQueueRecords(disconnectShipmentQueue);
        }
        private List<CargoDisconnectQueue> GetDisconnectQueue()
        {
            return cargoContext.CargoDisconnectQueues.Take(MaxShepmentsNumberPerTime).ToList();
        }



        private List<string> GetConnectedShipmentsIds(List<CargoDisconnectQueue> disconnectShipmentQueue)
        {
            var tenantsIds = disconnectShipmentQueue.GroupBy(e => e.Tenant);
            var shipmentsConnectedIds = GetShipmentsConnectedIds(tenantsIds);
            shipmentsConnectedIds = shipmentsConnectedIds.GroupBy(e => e).Select(e => e.Key).ToList();
            return shipmentsConnectedIds;

        }
        private void UpdateShipmetsByIds(List<string> connectedShipmentsIds)
        {
            var ids = GetIdsAsString(connectedShipmentsIds);
            var query = $"update Shipments set AutomaticLastUpdateDate = GETDATE() where id in ({ids}) ";
            shipmentsContext.GetActiveDbContext().Database.ExecuteSqlCommand(query);
        }
        private void RemoveQueueRecords(List<CargoDisconnectQueue> orderShipmentQueue)
        {
            var ids = GetIdsAsString(orderShipmentQueue.Select(e => e.Id + "").ToList());
            var query = $"delete from CargoDisconnectQueues where id in ({ids}) ";
            cargoContext.GetActiveDbContext().Database.ExecuteSqlCommand(query);
        }
        private List<CargoDisconnectShipmentData> GetShipmentsConnectedToOrder(List<string> orderIds)
        {
            var data = cargoContext.CargoTrackingShipments.Where(e => orderIds.Contains(e.ForwardingShipmentHeaderId))
                .Select(e => new { e.EntityId, e.EntityType, e.ForwardingShipmentHeaderId, e.CustomsShipmentHeaderId })
                .ToList();
            return data.Select(e => new CargoDisconnectShipmentData()
            {
                CustomsShipmentHeaderId = e.CustomsShipmentHeaderId,
                EntityId = e.EntityId,
                EntityType = e.EntityType,
                ForwardingShipmentHeaderId = e.ForwardingShipmentHeaderId
            }).ToList();

        }
        private List<string> GetShipmentsConnectedIds(IEnumerable<IGrouping<int, CargoDisconnectQueue>> tenantsIds)
        {
            var query = BuildGetQueryByIdAndTenant(tenantsIds);
            var data = query.Select(e => e.ShipmentId)
                .ToList();
            return data;
        }

        private IQueryable<CargoTrackingShipmentSearch> BuildGetQueryByIdAndTenant(IEnumerable<IGrouping<int, CargoDisconnectQueue>> tenantsIds)
        {
            var query = cargoContext.CargoTrackingShipmentSearches.AsQueryable();
            var searchCriteria = new List<Expression<Func<CargoTrackingShipmentSearch, bool>>>();
            foreach (var tenantGroup in tenantsIds)
            {
                foreach (var shipment in tenantGroup)
                {
                    searchCriteria.Add(e => e.ShipmentId == shipment.ShipmentId && e.Tenant == tenantGroup.Key);
                }
            }
            var joinedSearchCriteria = Join(Expression.Or, searchCriteria);
            query = query.Where(joinedSearchCriteria);
            return query;
        }

        private string GetIdsAsString(List<string> Ids)
        {
            var ids = "";
            foreach (var item in Ids)
            {
                ids += $"'{item}',";
            }
            if (Ids.Count > 0)
                ids = ids.Substring(0, ids.Length - 1);
            return ids;

        }

        public static Expression<Func<T, TReturn>> Join<T, TReturn>(Func<Expression, Expression, BinaryExpression> joiner, IReadOnlyCollection<Expression<Func<T, TReturn>>> expressions)
        {
            if (!expressions.Any())
            {
                throw new ArgumentException("No expressions were provided");
            }
            var firstExpression = expressions.First();
            var otherExpressions = expressions.Skip(1);
            var firstParameter = firstExpression.Parameters.Single();
            var otherExpressionsWithParameterReplaced = otherExpressions.Select(e => ReplaceParameter(e.Body, e.Parameters.Single(), firstParameter));
            var bodies = new[] { firstExpression.Body }.Concat(otherExpressionsWithParameterReplaced);
            var joinedBodies = bodies.Aggregate(joiner);
            return Expression.Lambda<Func<T, TReturn>>(joinedBodies, firstParameter);
        }
        public static T ReplaceParameter<T>(T expr, ParameterExpression toReplace, ParameterExpression replacement) where T : Expression
        {
            var replacer = new ExpressionReplacer(e => e == toReplace ? replacement : e);
            return (T)replacer.Visit(expr);
        }
    }
    public class CargoDisconnectShipmentData
    {
        public string CustomsShipmentHeaderId { get; internal set; }
        public string EntityId { get; internal set; }
        public string EntityType { get; internal set; }
        public string ForwardingShipmentHeaderId { get; internal set; }
    }
    public class ExpressionReplacer : ExpressionVisitor
    {
        private readonly Func<Expression, Expression> replacer;

        public ExpressionReplacer(Func<Expression, Expression> replacer)
        {
            this.replacer = replacer;
        }

        public override Expression Visit(Expression node)
        {
            return base.Visit(replacer(node));
        }
    }
}
