using Logitude.BL.Helpers;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Reflection;

namespace Logitude.BL.ShipmentsModel.Tools.Behaviours
{
    public class ContainersExternalDataBehaviour
    {
        private ContainersExternalData containersExternalData_New;
        private ContainersExternalData containersExternalData_DB;
        private ContainerPM containerPM;
        private IShipmentsContext shipmentsContext;
        private ContainersExternalDataRepository containersExternalDataRepository;
        private int tenant;
        private bool isNew;
        public ContainersExternalDataBehaviour(ContainerPM containerPM, IShipmentsContext context, ContainersExternal containersExternal)
        {
            this.containerPM = containerPM;
            this.shipmentsContext = context;
            this.tenant = containerPM.Tenant;
            this.containersExternalData_New = containersExternal.ContainersExternalData_New;
            this.containersExternalData_DB = containersExternal.ContainersExternalData_DB;
            this.isNew = containersExternal.IsNew;
            containersExternalDataRepository = new ContainersExternalDataRepository(context);
        }

        public void Handle()
        {
            MapEntity();
            SaveEntity();
        }

        private void MapEntity()
        {
            this.MapGateInField();
            this.MapGateOutField();
        }
        private void MapGateInField()
        {
            string propertyName = "GateIn";
            MapField(propertyName);
        }
        private void MapGateOutField()
        {
            string propertyName = "GateOut";
            MapField(propertyName);
        }
        private void MapField(string propertyName)
        {
            var containerValue = GetPropertyValue(containerPM, propertyName);
            var containersExternalDataDBValue = GetPropertyValue(containersExternalData_DB, propertyName);
            var containersExternalDataNewValue = GetPropertyValue(containersExternalData_New, propertyName);
            this.CheckOverwritingValue(containerValue, containersExternalDataDBValue, containersExternalDataNewValue, propertyName);
        }
        public object GetPropertyValue(object source, string propertyName)
        {
            return source.GetType().GetProperty(propertyName).GetValue(source, null);
        }
        private void CheckOverwritingValue(object conatinerValue, object containersExternalDataDBValue, object containersExternalDataNewValue, string propertyName)
        {
            if (conatinerValue == null)
            {
                SetValue(containerPM, propertyName, containersExternalDataNewValue);
                SetValue(containersExternalData_DB, propertyName, containersExternalDataNewValue);
            }
            else if (conatinerValue.Equals(containersExternalDataDBValue))
            {
                SetValue(containerPM, propertyName, containersExternalDataNewValue);
                SetValue(containersExternalData_DB, propertyName, containersExternalDataNewValue);
            }
            else
            {
                SetValue(containersExternalData_DB, propertyName, containersExternalDataNewValue);
            }
        }
        private void SetValue(object model, string field, object value)
        {
            PropertyInfo propertyInfo = model.GetType().GetProperty(field);
            propertyInfo.SetValue(model, value, null);
        }
        private void SaveEntity()
        {
            if (isNew)
            {
                containersExternalDataRepository.Add(containersExternalData_DB);
            }
            else
            {
                containersExternalDataRepository.Update(containersExternalData_DB);
            }
            containersExternalDataRepository.SubmitChanges();
        }
    }
}
