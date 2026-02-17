using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Update.SandBox
{
    class UpdateCustomsTest
    {
        public void Test()
        {
            var updateClass = new WebFreight.Web.MetaDataUpdate.UpdateClasses.CustomUpdate();
            updateClass.FillCustomsRequestsSheetStatusTable();
            updateClass.FillCustomsNotificationDefinitions();

            updateClass.FillCustomsInterfaceManagements();
            updateClass.FillCustomsInterfaceSendOptions();
        }
        void FillCustomsRequestsSheetStatusTable()
        {
            var myCustomUpdate = new WebFreight.Web.MetaDataUpdate.UpdateClasses.CustomUpdate();
            myCustomUpdate.FillCustomsRequestsSheetStatusTable();
        }

        void FillCustomsNotificationDefinitions()
        {
            var myCustomUpdate = new WebFreight.Web.MetaDataUpdate.UpdateClasses.CustomUpdate();
            myCustomUpdate.FillCustomsNotificationDefinitions();
        }

        void FillCustomsInterfaceSendOptions()
        {
            var myCustomUpdate = new WebFreight.Web.MetaDataUpdate.UpdateClasses.CustomUpdate();
            myCustomUpdate.FillCustomsInterfaceSendOptions();
        }
    }
}
