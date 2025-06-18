using System;
using System.Web;

namespace AmitalCloud.Infrastructure.APITools.Helpers
{
    public class HttpContextUtil
    {
        public const string PathU2LUNIFREIGHTGATEWAYPage = "UNIFREIGHTGATEWAYSERVICE";
        public static bool IsCustomDomainService()
        {
            try
            {


                if (HttpContext.Current == null || HttpContext.Current.Request == null)
                {
                    return false;
                }

                if (HttpContext.Current.Request.Url.AbsolutePath
                    //.Contains("CustomDomainService")
                    .Contains("DomainService")
                    )
                {
                    return true;
                }
                if (HttpContext.Current.Request.Url.AbsolutePath.Contains(@"/api/"))//angular 
                {
                    if (!HttpContext.Current.Request.Url.AbsolutePath.ToLower().Contains("webservice"))
                    {

                        return true;
                    }


                }



                //"/api/declarations"
                return false;
            }
            catch (Exception)
            {

                return false;
            }
        }
        public static bool IsPath(string path)
        {
            try
            {
                if (HttpContext.Current == null || HttpContext.Current.Request == null || string.IsNullOrWhiteSpace(path))
                {
                    return false;
                }
                if (HttpContext.Current.Request.Url.AbsolutePath.Contains(path))
                {
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

}
