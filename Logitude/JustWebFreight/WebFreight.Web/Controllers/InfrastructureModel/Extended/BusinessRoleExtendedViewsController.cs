using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityQueryServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.Repsitories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;

namespace WebFreight.Web.Controllers.InfrastructureModel.Extended
{
    public class BusinessRoleExtendedViewsController : ApiController 
    {
        public HttpResponseMessage GetToggleBusinessRoles(string memberId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                IInfrastructureContext myContext = InfrastructureContext.GetContext(authToken.Tenant); ;

                LBPTeamMemberRepository memberRep = new LBPTeamMemberRepository(authToken.Tenant);
                TeamMemberBusinessRoleRepository memberBusinessRoleRep = new TeamMemberBusinessRoleRepository(authToken.Tenant);

                LBPTeamMember teamMemberPM = memberRep.GetSingle(memberId, authToken.Tenant);
                string businessRoleId = teamMemberPM.MemberTeamId;

                IQueryable<TeamMemberBusinessRole> iQueryable = (from d in myContext.TeamMemberBusinessRoles
                                                                 where d.Tenant == authToken.Tenant && d.BusinessRoleId == businessRoleId && d.TeamMemberId == memberId
                                                                 select d);

                IQueryable<TeamMemberBusinessRole> retsultQuery;
                //if (iQueryable != null && iQueryable.Count() > 0)
                //{
                //    retsultQuery = iQueryable.Where(item => !iQueryable.Contains(item));
                //}
                //else
                //{
                //    retsultQuery = iQueryable;
                //}

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
    }
}