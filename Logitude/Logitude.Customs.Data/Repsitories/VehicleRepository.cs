 
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
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class VehicleRepository:IRepository<Vehicle>
   {
        
		public List<Vehicle> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public string GetVehicleIdByChassisNumber(string vehicleChassisNumber, int tenant)
        {
            if (string.IsNullOrEmpty(vehicleChassisNumber)) return "";
            return
                  (
                  from rec in context.Vehicles
                  where rec.VehicleChassisNumber == vehicleChassisNumber && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }

        public string GetVehicleIdByRichbitFileNumber(string richbitFileNumber, int tenant)
        {
            if (string.IsNullOrEmpty(richbitFileNumber)) return "";
            return
                  (
                  from rec in context.Vehicles
                  where rec.RichbitFileNumber == richbitFileNumber && rec.Tenant == tenant
                  select rec.Id
                  )
                  .FirstOrDefault();
        }
        public Vehicle GetSingleVehicleByVehicleChassisNumberOrRichbitFileNumber(string vehicleChassisNumber, string richbitFileNumber, int tenant)
        {
            Vehicle vehicle = null;

            if (!string.IsNullOrEmpty(vehicleChassisNumber) && vehicleChassisNumber != "null")
            {
               vehicle =    (from a in context.Vehicles.Include("VehicleStatus").Include("Declaration")
                        where (a.VehicleChassisNumber == vehicleChassisNumber) && a.Tenant == tenant
                        select a).FirstOrDefault();
             
            }
            else if (!string.IsNullOrEmpty(richbitFileNumber) && richbitFileNumber != "null")
            {
                
                    vehicle =  (from a in context.Vehicles.Include("VehicleStatus").Include("Declaration")
                            where (a.RichbitFileNumber == richbitFileNumber) && a.Tenant == tenant
                            select a).FirstOrDefault();

                
            }

            return vehicle;
        }

        public List<Vehicle> GetVehiclesForSelection(int tenant)
        {
            return (from a in context.Vehicles.Include("VehicleStatus").Include("Declaration").Include("VehiclePoolType").Include("Client").Include("VehicleManufacturer")
                    where a.DeclarationId == null && a.RichbitFileNumber != null && a.Tenant == tenant
                    select a).ToList();
        }

        public List<Vehicle> GetVehiclesByRichbitFileNumberAndDeclarationIds(string declarationId, string[] richbitFileNumbers, string[] chassissNumbers, int tenant)
        {

          IQueryable<Vehicle> vehicles = (from a in context.Vehicles.Include("VehicleStatus").Include("Declaration")
                           where ( (richbitFileNumbers.Contains(a.RichbitFileNumber) || chassissNumbers.Contains(a.VehicleChassisNumber)) && a.DeclarationId == declarationId && a.Tenant == tenant)
                           select a);


            return vehicles.ToList();
        }

        public List<Vehicle> GetVehiclesByRichbitFileNumbers(string[] richbitFileNumbers, int tenant)
        {

            IQueryable<Vehicle> vehicles = (from a in context.Vehicles.Include("VehicleStatus").Include("Declaration")
                                            where (richbitFileNumbers.Contains(a.RichbitFileNumber) && a.Tenant == tenant)
                                            select a);


            return vehicles.ToList();
        }

    }

}
   