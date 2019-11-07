using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class AirlineAreaService
    {
        bool isNewEntity;
        private int tenant;
        public AirlineArea Poco { get; set; }
        private AirlineAreaPM entityPM;
        private ICommonDataContext objectContext;
        private AirlineAreaRepository entityRepository;
        private AirlineAreasPortRepository airlineAreasPortrepository;
        private Contact loggedContact;
        public AirlineAreaService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.objectContext = objectContext;
            this.entityRepository = new AirlineAreaRepository(objectContext);
            this.airlineAreasPortrepository = new AirlineAreasPortRepository(objectContext);

            this.GetLoggedContact();
        }

        private void GetLoggedContact()
        {
            ContactRepository contactRepository = new ContactRepository(objectContext);

            string email = HttpContext.Current.User.Identity.Name;
            this.loggedContact = contactRepository.GetSingleContactByEmail(email, tenant);
        }

        private List<AirlineAreasPortPM> airlineAreasPortChangeSet;
        public void SetChangeSet(List<AirlineAreasPortPM> airlineAreasPortChangeSet)
        {
            this.airlineAreasPortChangeSet = airlineAreasPortChangeSet;
        }

        public void Create(AirlineAreaPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPM = entityPM;
            
            this.entityPM.Id = IdCounter.GetNumber("AirlineArea", tenant).ToString();
            this.Poco = new AirlineArea();
            this.Poco.Id = this.entityPM.Id;

            foreach (AirlineAreasPortPM item in entityPM.AirlineAreasPorts)
            {
                this.CreateAirlineAreasPort(item);
            }

            AirlineAreaTracing.Trace(entityPM, Poco, isNewEntity);
            AirlineAreaMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();           
        }

        public void Update(AirlineAreaPM entityPM, bool mapComposition = false)
        {
            this.isNewEntity = false;
            this.entityPM = entityPM;
            this.Poco = entityRepository.GetSingleAirlineArea(entityPM.Id, entityPM.Tenant);
            
            if (mapComposition)
            {
                this.SetChangeSet(this.entityPM.AirlineAreasPorts);
            }

            this.UpdateAirlineAreasPortCollection();

            AirlineAreaTracing.Trace(entityPM, Poco, isNewEntity);
            AirlineAreaMapping.MapEntity(entityPM, Poco, isNewEntity);

            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }
        
        private void UpdateAirlineAreasPortCollection()
        {
            if (airlineAreasPortChangeSet != null)
            {
                foreach (AirlineAreasPortPM itemPM in airlineAreasPortChangeSet)
                {
                    switch (itemPM.ChangeSetOp)
                    {
                        case ChangeSetOperation.Insert:
                            {
                                this.CreateAirlineAreasPort(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Update:
                            {
                                this.UpdateAirlineAreasPort(itemPM);
                                break;
                            }

                        case ChangeSetOperation.Delete:
                            {
                                this.DeleteAirlineAreasPort(itemPM);
                                break;
                            }

                        default: { break; }
                    }
                }
            }
        }
        private void CreateAirlineAreasPort(AirlineAreasPortPM itemPM)
        {
            itemPM.Id = IdCounter.GetNumber("AirlineAreasPort", tenant).ToString();
            itemPM.AirlineAreaId = this.entityPM.Id;
            itemPM.Tenant = tenant;
            itemPM.AddedByUserId = loggedContact.Id;

            AirlineAreasPort itemPoco = new AirlineAreasPort()
            {
                Id = itemPM.Id,
                AirlineAreaId = itemPM.AirlineAreaId,
                Tenant = tenant,
                Name = itemPM.Name,
                AddedDate = itemPM.AddedDate,
                PortId = itemPM.PortId,
                AddedByUserId = itemPM.AddedByUserId,
            };

            AirlineAreasPortMapping.MapEntity(itemPM, itemPoco, true);
            airlineAreasPortrepository.Add(itemPoco);
        }
        private void UpdateAirlineAreasPort(AirlineAreasPortPM itemPM)
        {
            AirlineAreasPort itemPoco = airlineAreasPortrepository.GetSingleAirlineAreasPort(itemPM.Id, tenant);

            AirlineAreasPortMapping.MapEntity(itemPM, itemPoco, false);
            airlineAreasPortrepository.Update(itemPoco);
        }

        private void DeleteAirlineAreasPort(AirlineAreasPortPM itemPM)
        {
            AirlineAreasPort itemPoco = airlineAreasPortrepository.GetSingleAirlineAreasPort(itemPM.Id, tenant);
            if (itemPoco != null)
            {
                airlineAreasPortrepository.Remove(itemPoco);
            }
        }
    }
}
