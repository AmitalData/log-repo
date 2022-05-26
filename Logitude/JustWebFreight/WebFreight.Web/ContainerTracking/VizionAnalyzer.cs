using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.ContainerTracking
{
    public class VizionAnalyzer
    {
        private ContainerUpdatedFields containerUpdatedFields;


        public VizionAnalyzer()
        {

        }

        public ContainerUpdatedFields Run()
        {
            return containerUpdatedFields;
        }
    }
}