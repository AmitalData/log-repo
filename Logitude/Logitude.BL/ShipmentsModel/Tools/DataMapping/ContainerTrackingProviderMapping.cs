using Simplog.Data.ShipmentsModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;

namespace Logitude.BL.ShipmentsModel.Tools.DataMapping
{
    public partial class ContainerTrackingProviderMapping
    {
        public static void Map(ContainerTrackingProviderPM itemPM, ContainerTrackingProvider itemPoco, bool isNewEntity)
        {
            if (isNewEntity)
            {
                itemPoco.Id = itemPM.Id;
                itemPoco.Tenant = itemPM.Tenant;
                
            }

            itemPoco.LogitudeToken = itemPM.LogitudeToken; 
            itemPoco.Name = itemPM.Name;
            itemPoco.ProviderURL = itemPM.ProviderURL;
            itemPoco.SourceCode = itemPM.SourceCode;
            itemPoco.APIKey = itemPM.APIKey;
            itemPoco.CallbackURL = itemPM.CallbackURL;
            BuildSearchField(itemPM, itemPoco);
        }
        public static void BuildSearchField(ContainerTrackingProviderPM itemPM, ContainerTrackingProvider containerTrackingProvider)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, containerTrackingProvider.Name);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerTrackingProvider.ProviderURL);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerTrackingProvider.SourceCode);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerTrackingProvider.APIKey);
            MethodHelper.AddToSearchFields(ref mySearchFields, containerTrackingProvider.CallbackURL);

            itemPM.SearchFields = mySearchFields;
            containerTrackingProvider.SearchFields = mySearchFields;
        }
    }
}