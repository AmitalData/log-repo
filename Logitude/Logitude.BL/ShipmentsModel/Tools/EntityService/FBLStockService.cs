using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
namespace Logitude.BL.ShipmentsModel.Tools.EntityService
{
    public class FBLStockService
    {
        bool isNewEntity;
        private int tenant;
        public FBLStock Poco { get; set; }

        public IShipmentsContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }
        private IShipmentsContext objectContext;

        private FBLStockPM entityPm;

        private FBLStockRepository entityRepository;
        public FBLStockService(IShipmentsContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new FBLStockRepository(objectContext);
        }

        public void Create(FBLStockPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;

            bool exist = entityRepository.GetSingleFBLStockByNumber(entityPM.Number, entityPM.Tenant) != null ? true : false;

            if (!exist)
            {
                this.Poco = new FBLStock();

                this.Poco.Id = IdCounter.GetNumber("FBLStock", entityPM.Tenant).ToString();
                this.Poco.Id = this.entityPm.Id;
                FBLStockMapping.MapEntity(entityPM, Poco, isNewEntity);
                //FBLStockTracing.Trace(entityPM, Poco, isNewEntity);
                //FBLStockValidating.Validate(entityPM);

                entityRepository.Add(Poco);
                entityRepository.SubmitChanges();
            }

            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "Stack Number");
                throw new Exception(msg);
            }

        }

        public void Update(FBLStockPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleFBLStock(entityPM.Id, entityPm.Tenant);

            bool exist = entityRepository.GetSingleFBLStockByNumber(entityPM.Number, entityPM.Tenant) != null ? true : false;

            if (!exist)
            {
                //FBLStockValidating.Validate(entityPM);
                //FBLStockTracing.Trace(entityPM, Poco, isNewEntity);
                FBLStockMapping.MapEntity(entityPM, Poco, isNewEntity);
                entityRepository.Update(Poco);
                entityRepository.SubmitChanges();
            }
            else
            {
                string msg = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", entityPM.Tenant);
                msg = msg.Replace("%Entity", "Stack Number");
                throw new Exception(msg);
            }
        }

        public void DeleteFBLStocksOperation(string stackId, bool isSeriesDelete, string loggedUserId)
        {
            FBLStock entity = entityRepository.GetSingleFBLStock(stackId, tenant);

            if (!isSeriesDelete)
            {
                entityRepository.Remove(entity);

                if (loggedUserId != null)
                {
                    //EventTracer.CreateTraceEvent(new EventTracerArgs()
                    //{
                    //    Tenant = tenant,
                    //    EventTypeCode = "AWBR",
                    //    UserId = loggedUserId,
                    //    EntityId = airlineId,
                    //    ObjectTableName = "Airline",
                    //    Notes = "FBL  number [" + entity.Number + "] removed",
                    //});
                }
            }

            else
            {
                List<FBLStock> stacksList = entityRepository.GetFBLStocksByInsertionDate(tenant, entity.InsertionDate);

                foreach (FBLStock stack in stacksList)
                {
                    entityRepository.Remove(stack);
                }

                if (loggedUserId != null)
                {
                    //EventTracer.CreateTraceEvent(new EventTracerArgs()
                    //{
                    //    Tenant = tenant,
                    //    EventTypeCode = "AWBD",
                    //    UserId = loggedUserId,
                    //    EntityId = airlineId,
                    //    ObjectTableName = "Airline",
                    //    Notes = "AWB stack inserted on [" + entity.InsertionDate.ToShortDateString() + "] at [" + entity.InsertionDate.ToShortTimeString() + "] removed",
                    //});
                }
            }

            //TableLastUpdateClass.UpdateTableHistory(entity.Tenant, "FBLStock");
            entityRepository.SubmitChanges();
        }

        public void CreateFBLStocks(int startNumber, int endNumber, string loggedUserId)
        {
            ValidateStockNumbers(startNumber, endNumber);
            DateTime insertionDate = TenantServerConfigration.GetCurrentDateTime(tenant);
            for (int i = startNumber; i <= endNumber; i++)
            {
                CreateFBLStock(insertionDate, i);
            }
            entityRepository.SubmitChanges();
        }

        private void ValidateStockNumbers(int startNumber, int endNumber)
        {
            List<int> existStocks = new FBLStockRepository(tenant)
                .GetExistStocksInRange(tenant, startNumber, endNumber).Select(a => a.Number).ToList();

            if (!existStocks.Any()) return;

            string message = TranslateTextsClass.Translate("General.M.EntityAlreadyExists", tenant).Replace("%Entity", "Stock Number") + " ";
            message += "[" + string.Join(",", existStocks) + "]";
            throw new Exception(message);
        }

        private void CreateFBLStock(DateTime insertionDate, int number)
        {
            FBLStock fBLStock = new FBLStock()
            {
                Id = IdCounter.GetNumber("FBLStock", tenant).ToString(),
                Tenant = tenant,
                Number = number,
                InsertionDate = insertionDate,
            };
            entityRepository.Add(fBLStock);
        }
    }
}
