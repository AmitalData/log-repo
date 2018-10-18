using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs;

namespace Logitude.Server.Tools.Helpers
{
    public class General
    {
        public static string GetAddress(Address address)
        {
            string resultAddress = "";

            if (address != null)
            {
                resultAddress = address.Address1 != null ? address.Address1 : "";

                if (!string.IsNullOrEmpty(address.Address2))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.Address2;
                }

                if (!string.IsNullOrEmpty(address.City))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.City;
                }

                if (address.State != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + " " + (address.State.LocalName != null ? address.State.LocalName : "");
                    }

                    else
                    {
                        resultAddress = resultAddress + " " + (address.State.EnglishName != null ? address.State.EnglishName : "");
                    }
                }

                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    resultAddress = resultAddress + " " + address.ZipCode;
                }

                if (address.Country != null)
                {
                    if (address.IsLocalLanguage)
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.LocalName;
                    }

                    else
                    {
                        resultAddress = resultAddress + Environment.NewLine + address.Country.EnglishName;
                    }
                }
            }

            return resultAddress;
        }
    }
}
