using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

using WebFreight.Web.Helpers;
using WebFreight.Web.MetaDataUpdate.DetailClasses;
using Logitude.Server.Tools.Counters;

namespace WebFreight.Web.MetaDataUpdate.AddClasses
{
    public class AddMeasurements
    {
        public static void AddMeasurement(MeasurementDetails measurementDetails, MeasurementRepository measurementRepository, Dictionary<string, Measurement> tenantMeasurements)
        {
            if (tenantMeasurements.Keys.Contains(measurementDetails.Code))
            {
                Measurement measurement = tenantMeasurements[measurementDetails.Code];
                measurement.InActive = measurementDetails.InActive;
                measurement.IsContainer = measurementDetails.IsContainer;
                measurement.IsContainerMeasurement = measurementDetails.IsContainerMeasurement;
                measurement.Name = measurementDetails.Name;
                measurement.ShortName = measurementDetails.ShortName;
                measurement.Tenant = measurementDetails.Tenant;
                measurement.SearchFields = measurementDetails.Code + "," + measurementDetails.Name + "," + measurementDetails.ShortName + "," + measurementDetails.WeightUnitCode;
                measurementRepository.Update(measurement);
            }
            else
            {
                Measurement newMeasurement = new Measurement()
                {
                    Tenant = measurementDetails.Tenant,
                    ShortName = measurementDetails.ShortName,
                    Name = measurementDetails.Name,
                    IsContainerMeasurement = measurementDetails.IsContainerMeasurement,
                    IsContainer = measurementDetails.IsContainer,
                    InActive = measurementDetails.InActive,
                    Code = measurementDetails.Code,
                    SearchFields = measurementDetails.Code + "," + measurementDetails.Name + "," + measurementDetails.ShortName + "," + measurementDetails.WeightUnitCode,
                    Id = IdCounter.GetNumber("Measurement", measurementDetails.Tenant).ToString(),

                };

                measurementRepository.Add(newMeasurement);
            }
        }
    }
}