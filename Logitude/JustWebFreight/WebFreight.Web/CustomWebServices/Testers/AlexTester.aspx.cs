using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Logitude.Accounting.BL;
using System.Transactions;
using Logitude.Accounting.Data;
using Simplog.Server.Infrastructure;
using Logitude.Accounting.BL.EntityUpdateServices;
using Logitude.Accounting.BL.Utils;

namespace WebFreight.Web.CustomWebServices.Testers
{
    public partial class AlexTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RevaluationBatch revaluationBatch = new RevaluationBatch();
            revaluationBatch.RunOneRevaluation("1-2", 1);
        }
    }
}