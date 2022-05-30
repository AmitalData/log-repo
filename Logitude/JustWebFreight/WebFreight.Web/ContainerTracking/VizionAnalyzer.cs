using Logitude.BL.DataContracts;
using Simplog.Data.ShipmentsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.ContainerTracking
{
    public class VizionAnalyzer
    {
        private ContainerUpdatedFields containerUpdatedFields;


        public VizionAnalyzer(VisionContainerStatus visionContainerStatus)
        {

            var shipmentsContext = ShipmentsContext.GetContext(0);
            


        }

        public ContainerUpdatedFields Run()
        {
            return containerUpdatedFields;
        }
    }
}