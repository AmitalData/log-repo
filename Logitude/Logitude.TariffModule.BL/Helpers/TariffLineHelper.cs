using Logitude.TariffModule.Data.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.Helpers
{
    public class TariffLineHelper
    {
        public static string ComputeTariffLineErrorText(TariffLine tariffLine)
        {
            string errorText = null;

            if (!string.IsNullOrEmpty(tariffLine.OriginPortText) && string.IsNullOrEmpty(tariffLine.OriginPortId))
            {
                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Port with code " + tariffLine.OriginPortText + " not found";
                }

                else
                {
                    errorText = errorText + ", Port with code " + tariffLine.OriginPortText + " not found";
                }
            }
            else if (string.IsNullOrEmpty(tariffLine.OriginPortText) && string.IsNullOrEmpty(tariffLine.OriginPortId))
            {
                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Missing Origin Port";
                }

                else
                {
                    errorText = errorText + ", Missing Origin Port";
                }
            }

            if (!string.IsNullOrEmpty(tariffLine.DestinationPortText) && string.IsNullOrEmpty(tariffLine.DestinationPortId))
            {
                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Port with code " + tariffLine.DestinationPortText + " not found";
                }

                else
                {
                    errorText = errorText + ", Port with code " + tariffLine.DestinationPortText + " not found";
                }
            }
            else if (string.IsNullOrEmpty(tariffLine.DestinationPortText) && string.IsNullOrEmpty(tariffLine.DestinationPortId))
            {
                if (string.IsNullOrEmpty(errorText))
                {
                    errorText = "Missing Destination Port";
                }

                else
                {
                    errorText = errorText + ", Missing Destination Port";
                }
            }

            if (!string.IsNullOrEmpty(tariffLine.MinPriceText) && tariffLine.MinPrice == null)
            {
                if(IsNumber(tariffLine.MinPriceText))
                {
                    if (IsMinus(tariffLine.MinPriceText))
                    {
                        if (string.IsNullOrEmpty(errorText))
                        {
                            errorText = "Min price can't be minus";
                        }

                        else
                        {
                            errorText = errorText + ", Min price can't be minus";
                        }
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Min price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Min price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(tariffLine.Step1PriceText) && tariffLine.Step1Price == null)
            {
                if (IsNumber(tariffLine.Step1PriceText))
                {
                    if (IsMinus(tariffLine.Step1PriceText))
                    {
                        if (string.IsNullOrEmpty(errorText))
                        {
                            errorText = "Step 1 price can't be minus";
                        }

                        else
                        {
                            errorText = errorText + ", Step 1 price can't be minus";
                        }
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 1 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 1 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(tariffLine.Step2PriceText) && tariffLine.Step2Price == null)
            {
                if (IsNumber(tariffLine.Step2PriceText))
                {
                    if (IsMinus(tariffLine.Step2PriceText))
                    {
                        if (string.IsNullOrEmpty(errorText))
                        {
                            errorText = "Step 2 price can't be minus";
                        }

                        else
                        {
                            errorText = errorText + ", Step 2 price can't be minus";
                        }
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 2 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 2 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(tariffLine.Step3PriceText) && tariffLine.Step3Price == null)
            {
                if (IsNumber(tariffLine.Step3PriceText))
                {
                    if (IsMinus(tariffLine.Step3PriceText))
                    {
                        if (string.IsNullOrEmpty(errorText))
                        {
                            errorText = "Step 3 price can't be minus";
                        }

                        else
                        {
                            errorText = errorText + ", Step 3 price can't be minus";
                        }
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 3 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 3 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(tariffLine.Step4PriceText) && tariffLine.Step4Price == null)
            {
                if (IsNumber(tariffLine.Step4PriceText))
                {
                    if (IsMinus(tariffLine.Step4PriceText))
                    {
                        if (string.IsNullOrEmpty(errorText))
                        {
                            errorText = "Step 4 price can't be minus";
                        }

                        else
                        {
                            errorText = errorText + ", Step 4 price can't be minus";
                        }
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 4 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 4 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(tariffLine.Step5PriceText) && tariffLine.Step5Price == null)
            {
                if (IsNumber(tariffLine.Step5PriceText))
                {
                    if (IsMinus(tariffLine.Step5PriceText))
                    {
                        if (string.IsNullOrEmpty(errorText))
                        {
                            errorText = "Step 5 price can't be minus";
                        }

                        else
                        {
                            errorText = errorText + ", Step 5 price can't be minus";
                        }
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 5 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 5 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(tariffLine.Step6PriceText) && tariffLine.Step6Price == null)
            {
                if (IsNumber(tariffLine.Step6PriceText))
                {
                    if (IsMinus(tariffLine.Step6PriceText))
                    {
                        if (string.IsNullOrEmpty(errorText))
                        {
                            errorText = "Step 6 price can't be minus";
                        }

                        else
                        {
                            errorText = errorText + ", Step 6 price can't be minus";
                        }
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 6 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 6 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(tariffLine.Step7PriceText) && tariffLine.Step7Price == null)
            {
                if (IsNumber(tariffLine.Step7PriceText))
                {
                    if (IsMinus(tariffLine.Step7PriceText))
                    {
                        if (string.IsNullOrEmpty(errorText))
                        {
                            errorText = "Step 7 price can't be minus";
                        }

                        else
                        {
                            errorText = errorText + ", Step 7 price can't be minus";
                        }
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 7 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 7 price format is invalid";
                    }
                }
            }

            if (!string.IsNullOrEmpty(tariffLine.Step8PriceText) && tariffLine.Step8Price == null)
            {
                if (IsNumber(tariffLine.Step8PriceText))
                {
                    if (IsMinus(tariffLine.Step8PriceText))
                    {
                        if (string.IsNullOrEmpty(errorText))
                        {
                            errorText = "Step 8 price can't be minus";
                        }

                        else
                        {
                            errorText = errorText + ", Step 8 price can't be minus";
                        }
                    }
                }

                else
                {
                    if (string.IsNullOrEmpty(errorText))
                    {
                        errorText = "Step 8 price format is invalid";
                    }

                    else
                    {
                        errorText = errorText + ", Step 8 price format is invalid";
                    }
                }
            }

            return errorText;
        }
        private static bool IsMinus(string myNumberString)
        {
            bool isMinus = false;

            decimal myNumber = Convert.ToDecimal(myNumberString);
            if (myNumber < 0)
            {
                isMinus = true;
            }

            return isMinus;
        }
        private static bool IsNumber(string myNumberString)
        {
            bool isNumber = false;

            if (!string.IsNullOrEmpty(myNumberString))
            {
                decimal value;
                if (Decimal.TryParse(myNumberString, out value))
                {
                    isNumber = true;
                }
            }

            return isNumber;
        }
    }    
}
