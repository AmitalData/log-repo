using System;
using System.Web;
using System.Linq;
using System.Collections.Generic;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.DataMapping
{
    public class TermsofUseSignatureMapping
    {
        public static void MapEntity(TermsofUseSignaturePM termsofUseSignaturePm, TermsofUseSignature termsofUseSignature, bool isNewState)
        {
            termsofUseSignature.Tenant = termsofUseSignaturePm.Tenant;
            termsofUseSignature.SignedDatetime = termsofUseSignaturePm.SignedDatetime;
            termsofUseSignature.ContactId = termsofUseSignaturePm.ContactId;
            termsofUseSignature.TermsofUseId = termsofUseSignaturePm.TermsofUseId;
        }
    }
}