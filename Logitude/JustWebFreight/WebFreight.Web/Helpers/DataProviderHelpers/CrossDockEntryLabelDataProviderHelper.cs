using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.WarehouseLib.BL.EntityPMs;
using Logitude.WarehouseLib.BL.EntityQueryServices;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataContracts;
using WebFreight.Web.DataProviders;
namespace WebFreight.Web.Helpers.DataProviderHelpers
{
    public class CrossDockEntryLabelDataProviderHelper
    {
        public CrossDockEntryDataProviderHelper crossDockEntryDataProviderHelper;
        public byte[] LoadCrossDockEntryLabelDataProvider(string entityId, int tenant)
        {
            List<CrossDockEntryDataProvider> crossDocksEntryLabelsLists = GetCrossDockEntryLabelDataProvider(entityId, tenant);
            XmlSerializer serializer = new XmlSerializer(typeof(List<CrossDockEntryDataProvider>));
            using (MemoryStream memstream = new MemoryStream())
            {
                serializer.Serialize(memstream, crossDocksEntryLabelsLists);
                memstream.Seek(0, SeekOrigin.Begin);
                var reader = new StreamReader(memstream);
                string content = reader.ReadToEnd();
                byte[] bytearray = memstream.ToArray();
                return bytearray;
            }
        }

        private List<CrossDockEntryDataProvider> GetCrossDockEntryLabelDataProvider(string entityId, int tenant)
        {
            List<CrossDockEntryDataProvider> myResult = new List<CrossDockEntryDataProvider>();
            crossDockEntryDataProviderHelper = new CrossDockEntryDataProviderHelper();
            CrossDockEntryDataProvider crossDockEntryDataProvider = crossDockEntryDataProviderHelper.LoadCrossDockEntryDataProvider(entityId, tenant);
            if (!string.IsNullOrEmpty(crossDockEntryDataProvider.EntryNumber))
            {
                for (int counter = 1; counter <= crossDockEntryDataProvider.NumberOfPackages; counter++)
                {
                    CrossDockEntryDataProvider newlabel = crossDockEntryDataProvider.ShallowCopy();
                    newlabel.BarCode = crossDockEntryDataProvider.BarCode + String.Format("{0:00000}", counter);
                    myResult.Add(newlabel);
                }
            }
            return myResult;
        }

     
    }
}