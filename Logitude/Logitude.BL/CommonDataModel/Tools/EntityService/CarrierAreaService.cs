using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class CarrierAreaService
    {
        bool isNewEntity;
        private int tenant;
        public CarrierArea Poco { get; set; }
        private CarrierAreaPM entityPM;
        private ICommonDataContext objectContext;
        private CarrierAreaRepository entityRepository;
        private CarrierAreasPortRepository CarrierAreasPortrepository;
        private Contact loggedContact;
        public CarrierAreaService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new CarrierAreaRepository(objectContext);
            this.CarrierAreasPortrepository = new CarrierAreasPortRepository(objectContext);

            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            ContactRepository contactRepository = new ContactRepository(objectContext);

            string email = HttpContext.Current.User.Identity.Name;
            this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
        }

        private List<CarrierAreasPortPM> CarrierAreasPortChangeSet;
        public void SetChangeSet(List<CarrierAreasPortPM> CarrierAreasPortChangeSet)
        {
            this.CarrierAreasPortChangeSet = CarrierAreasPortChangeSet;
        }

        public void Create(CarrierAreaPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            
            this.entityPM.Id = IdCounter.GetNumber("CarrierArea", tenant).ToString();
            this.Poco = new CarrierArea();
            this.Poco.Id = this.entityPM.Id;

            foreach (CarrierAreasPortPM item in entityPM.CarrierAreasPorts)
            {
                this.CreateCarrierAreasPort(item);
            }

            CarrierAreaTracing.Trace(entityPM, Poco, isNewEntity);
            CarrierAreaMapping.MapEntity(entityPM, Poco, isNewEntity, loggedContact.Id);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();           
        }

        public void Update(CarrierAreaPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleCarrierArea(entityPM.Id, entityPM.Tenant);
            
            if (mapComposition)
            {
                this.SetChangeSet(this.entityPM.CarrierAreasPorts);
            }

            this.UpdateCarrierAreasPortCollection();

            CarrierAreaTracing.Trace(entityPM, Poco, isNewEntity);
            CarrierAreaMapping.MapEntity(entityPM, Poco, isNewEntity, loggedContact.Id);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
        
        private void UpdateCarrierAreasPortCollection()
        {
            if (CarrierAreasPortChangeSet != null)
            {
                foreach (CarrierAreasPortPM itemPM in CarrierAreasPortChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateCarrierAreasPort(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateCarrierAreasPort(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteCarrierAreasPort(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void CreateCarrierAreasPort(CarrierAreasPortPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("CarrierAreasPort", tenant).ToString();
            itemPM.CarrierAreaId = this.entityPM.Id;
            itemPM.Tenant = tenant;
            itemPM.AddedByUserId = loggedContact.Id;

            CarrierAreasPort itemPoco = new CarrierAreasPort()
            {
                Id = itemPM.Id,
                CarrierAreaId = itemPM.CarrierAreaId,
                Tenant = tenant,
                Name = itemPM.Name,
                AddedDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                PortId = itemPM.PortId,
                AddedByUserId = itemPM.AddedByUserId,
            };

            CarrierAreasPortMapping.MapEntity(itemPM, itemPoco, true);
            CarrierAreasPortrepository.Add(itemPoco);
        }
        private void UpdateCarrierAreasPort(CarrierAreasPortPM itemPM)
        {
            CarrierAreasPort itemPoco = CarrierAreasPortrepository.GetSingleCarrierAreasPort(itemPM.Id, tenant);

            CarrierAreasPortMapping.MapEntity(itemPM, itemPoco, false);
            CarrierAreasPortrepository.Update(itemPoco);
        }

        private void DeleteCarrierAreasPort(CarrierAreasPortPM itemPM)
        {
            CarrierAreasPort itemPoco = CarrierAreasPortrepository.GetSingleCarrierAreasPort(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                CarrierAreasPortrepository.Remove(itemPoco);
            }
        }
    }
}
