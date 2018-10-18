using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Server.Tools.Helpers
{
    public class SystemLogoHelper
    {
        public static string GetEnvironmentLogoName(bool small)
        {
            string myResult = "";

            string myLogoCode = LogitudeSettings.LogoCode;
            switch (myLogoCode)
            {
                
                case "U.N.I":
                    {
                        myResult = "unifreightlogo";
                        break;
                    }

                default:
                    {
                        myResult = "logo0";
                        break;
                    }
            }

            if (small)
            {
                myResult = "small" + myResult;
            }

            return myResult;
        }

        //public static string GetEnvironmentSmallLogoName()
        //{
        //    string myResult = "";

        //    string myLogoCode = LogitudeSettings.LogoCode;
        //    switch (myLogoCode)
        //    {
        //        case "C.R.M":
        //            {
        //                myResult = "SmallCRMLogo.jpg";
        //                break;
        //            }

        //        case "U.N.I":
        //            {
        //                myResult = "SmallUnifreightLogo.jpg";
        //                break;
        //            }

        //        default:
        //            {
        //                myResult = "smalllogo0.jpg";
        //                break;
        //            }
        //    }

        //    return myResult;
        //}
    }
}
