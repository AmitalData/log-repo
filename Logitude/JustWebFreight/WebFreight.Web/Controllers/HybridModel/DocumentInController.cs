using Logitude.BL.CommonDataModel.EntityPMs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebFreight.Web.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;

using System.Reflection;
using WebFreight.Web.Security;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using WebFreight.Web.DataContracts;
using Logitude.CargoTracking.Data;
using Logitude.CargoTracking.Data.EntityLists;
using Logitude.CargoTracking.Data.EntityListQueryServices;
using Simplog.Server.Infrastructure;
using System.Data.Common;
using System.Data.Entity;
using System.Configuration;
using Logitude.BL.InfrastructureModel.EntityLists;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.Services;
using System.Linq.Expressions;
using WebFreight.Web.Controllers.DigitalPortal.Models;
using Newtonsoft.Json;
using Microsoft.TeamFoundation.Common;
using WebFreight.Web.WcfApi;
using Logitude.Server.Tools;




namespace WebFreight.Web.Controllers.HybridModel
{
    public class DocumentInController : ApiController
    {
  
        public Response Upsert(DocumentsFilingPM documentDataPM, bool batch)
        {
          
               DocumentInWcfService DocumentInWcfService= new DocumentInWcfService();
                Response response = DocumentInWcfService.Upsert(documentDataPM, batch);
                return response;
                     
        }
    }
}