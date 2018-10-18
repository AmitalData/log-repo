using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.CustomsMessaging.Common.RequestParams
{

    /// <summary>
    /// if from \WebFreight.Web\CustomWebServices\DeclarationWebService.asmx  then  AppicationId ==  DeclarationIDDF_MSG10000_ImportDeclaration 
    /// </summary>
    public class GenericRequestParams:RequestParamsBase
    {
        public string AppicationId { get; set; }
        ///public string ServerMoreData { get; set; }
    }

}
