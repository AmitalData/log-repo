using System;
using System.Collections.Generic;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Server.Tools.Counters;
using Simplog.Data.InfrastructureModel;
using Logitude.BL.InfrastructureModel.EntityQueries;

namespace Logitude.BL.InfrastructureModel.Tools.EntityService
{
    public class ObjectTableTabTextCodeService
    {
        private int tenant;
        private ObjectTableTabPM objectTableTab;
        private TextCodeService textCodeService;
        public ObjectTableTabTextCodeService(ObjectTableTabPM objectTableTab , IWebFreightContext webFreightContext, int tenant)
        {
            this.tenant = tenant;
            this.objectTableTab = objectTableTab;
            textCodeService = new TextCodeService(webFreightContext , tenant);
        }

        public void Create()
        {
            var textCode = GetNewInStanceFromTextCode(objectTableTab);
            textCodeService.Create(textCode);
            objectTableTab.TabNameTextCodeId = textCode.Id;
            objectTableTab.TabNameTextCodeCode = textCode.Code;
        }

        public void Update()
        {
            var textCodeQuery = new TextCodeQuery(tenant);
            var textCode = textCodeQuery.GetByCode(objectTableTab.TabNameTextCodeCode , objectTableTab.Tenant);
            if (textCode.DefaultText == objectTableTab.Name) return;
            textCode.DefaultText = objectTableTab.Name;
            textCodeService.Update(textCode);
        }

        private TextCodePM GetNewInStanceFromTextCode(ObjectTableTabPM tab)
        {
            return new TextCodePM()
            {
                Code = !string.IsNullOrEmpty(objectTableTab.TabNameTextCodeCode) ? objectTableTab.TabNameTextCodeCode : "ObjectTableTab.O." + tab.Code ,
                DefaultText = tab.Name,
                DefaultTextPlural = tab.Name,
                ObjectTableId = tab.ObjectTableId,
                Tenant = tab.Tenant,
                TextCodeTypeCode = !string.IsNullOrEmpty(objectTableTab.TabNameTextCodeType) ? objectTableTab.TabNameTextCodeType: "O",
                LocalDefaultText = tab.Name
            };
        }
    }

}