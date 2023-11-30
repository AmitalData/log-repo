using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.Server.Tools.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebFreight.Web.Helpers.APIHelpers
{
    public class ComputeHelper
    {
        public static double? ComputeVolume(ShipmentPackagePM package, ShipmentPM entityPM)
        {
            double? myResult = null;
            myResult = ComputePackageVolume(package.Quantity, package.Width, package.Height, package.Length, package.Weight, entityPM.Ratio, entityPM.DimensionsUnitCode, entityPM.VolumeUnitCode, entityPM.GrossWeightUnitCode);

            return myResult;
        }

        public static double? ComputeInsideVolume(InsideShipmentPackagePM package, ShipmentPM entityPM)
        {
            double? myResult = null;
            myResult = ComputeInsidePackageVolume(package.Quantity, package.Width, package.Height, package.Length, package.Volume, package.Weight, entityPM.Ratio, entityPM.DimensionsUnitCode, entityPM.VolumeUnitCode, entityPM.GrossWeightUnitCode);

            return myResult;
        }

        public static double? ComputeInsidePackageVolume(double? quantity, double? width, double? height, double? length, double? volume, double? weight, double? ratio, string dimentionCode, string volumeCode, string fromWeightCode)
        {
            double? myWidth = null;
            double? myHeight = null;
            double? myLength = null;
            double? myWeight = null;
            double? myRatio = null;
            double? myQuantity = null;
            double? myVolume = null;

            if (width != null)
            {
                myWidth = Convert.ToDouble(width);
            }

            if (height != null)
            {
                myHeight = Convert.ToDouble(height);
            }

            if (length != null)
            {
                myLength = Convert.ToDouble(length);
            }

            if (weight != null)
            {
                myWeight = Convert.ToDouble(weight);
            }

            if (ratio != null)
            {
                myRatio = Convert.ToDouble(ratio);
            }

            if (quantity != null)
            {
                myQuantity = Convert.ToDouble(quantity);
            }

            if (volume != null)
            {
                myVolume = Convert.ToDouble(volume);
            }

            double? myResult = null;


            if (myWidth == null || myHeight == null || myLength == null || myQuantity == null)
            {
                if (myVolume != null)
                {
                    myResult = myVolume;// GetWeightFromVolume(volumeCode, fromWeightCode, myVolume, myRatio);
                }

                else if (myWeight != null)
                {
                    myResult = GetVolumeFromWeight(fromWeightCode, volumeCode, myWeight, myRatio);
                }
            }

            else
            {
                myResult = GetVolumeFromDimentions(dimentionCode, volumeCode, myWidth, myHeight, myLength, myQuantity);
            }

            if (myResult != null)
            {
                myResult = Round(myResult.Value, 3);
            }

            return myResult;
        }

        public static double? ComputePackageVolume(double? quantity, double? width, double? height, double? length, double? weight, double? ratio, string dimentionCode, string volumeCode, string fromWeightCode)
        {
            double? myWidth = null;
            double? myHeight = null;
            double? myLength = null;
            double? myWeight = null;
            double? myRatio = null;
            double? myQuantity = null;

            if (width != null)
            {
                myWidth = Convert.ToDouble(width);
            }

            if (height != null)
            {
                myHeight = Convert.ToDouble(height);
            }

            if (length != null)
            {
                myLength = Convert.ToDouble(length);
            }

            if (weight != null)
            {
                myWeight = Convert.ToDouble(weight);
            }

            if (ratio != null)
            {
                myRatio = Convert.ToDouble(ratio);
            }

            if (quantity != null)
            {
                myQuantity = Convert.ToDouble(quantity);
            }

            double? myResult = null;


            if (myWidth == null || myHeight == null || myLength == null || myQuantity == null)
            {
                if (myWeight != null)
                {
                    myResult = GetVolumeFromWeight(fromWeightCode, volumeCode, myWeight, myRatio);
                }
            }

            else
            {
                myResult = GetVolumeFromDimentions(dimentionCode, volumeCode, myWidth, myHeight, myLength, myQuantity);
            }

            if (myResult != null)
            {
                myResult = Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetVolumeFromWeight(string weightCode, string volumeCode, double? weight, double? ratio)
        {
            double? myWeight = null;
            double? myRatio = null;

            if (weight != null)
            {
                myWeight = Convert.ToDouble(weight);
            }

            if (ratio != null)
            {
                myRatio = Convert.ToDouble(ratio);
            }

            double? myResult = null;
            double? factorOfConvert = 1;

            if (myWeight != null)
            {
                if (string.IsNullOrEmpty(weightCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                double? weightInKilograms = weightInKilograms = myWeight * factorOfConvert;
                double? volumeInCBM = (weightInKilograms * myRatio) / 1000;

                if (string.IsNullOrEmpty(volumeCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (volumeCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                        case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                    }
                }

                myResult = volumeInCBM * factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetVolumeFromDimentions(string dimentionCode, string volumeCode, double? width, double? height, double? length, double? quantity)
        {
            double? myWidth = null;
            double? myHeight = null;
            double? myLength = null;
            double? myQuantity = null;

            if (width != null)
            {
                myWidth = Convert.ToDouble(width);
            }

            if (height != null)
            {
                myHeight = Convert.ToDouble(height);
            }

            if (length != null)
            {
                myLength = Convert.ToDouble(length);
            }

            if (quantity != null)
            {
                myQuantity = Convert.ToDouble(quantity);
            }

            double? myResult = null;
            double factorOfConvert = 1;

            if (string.IsNullOrEmpty(dimentionCode))
            {
                dimentionCode = "CM";
            }

            if (string.IsNullOrEmpty(volumeCode))
            {
                volumeCode = "CBM";
            }

            if (dimentionCode.ToUpper() == "INC" && volumeCode.ToUpper() == "CBI")
            {
                myResult = myQuantity * (myWidth * myHeight * myLength);
            }

            else if (dimentionCode.ToUpper() == "FT" && volumeCode.ToUpper() == "CBF")
            {
                myResult = myQuantity * (myWidth * myHeight * myLength);
            }

            else
            {
                switch (dimentionCode.ToUpper())
                {
                    case "CM": { factorOfConvert = 1; break; }
                    case "INC": { factorOfConvert = 2.54; break; }      // 1 inch = 2.54 centimeters
                    case "FT": { factorOfConvert = 30.48; break; }      // 1ft = 30.48 centimeters
                }

                double? volumeInCentimeters = myQuantity * (myWidth * myHeight * myLength) * Math.Pow(factorOfConvert, 3);
                double? volumeInCBM = volumeInCentimeters / Math.Pow(100, 3);

                switch (volumeCode.ToUpper())
                {
                    case "CBM": { factorOfConvert = 1; break; }
                    case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                    case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                }

                myResult = volumeInCBM * factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = Round(myResult.Value, 3);
            }

            return myResult;
        }

        public static double? ComputeVolumetricWeight(ShipmentPackagePM package, ShipmentPM entityPM)
        {
            double? myResult = null;
            myResult = ComputePackageVolumetricWeight(package.Quantity, package.Width, package.Height, package.Length, package.Volume, package.Weight, entityPM.Ratio, entityPM.DimensionsUnitCode, entityPM.VolumeUnitCode, entityPM.GrossWeightUnitCode, entityPM.ChargeableWeightUnitCode);

            return myResult;
        }

        public static double? ComputeInsideVolumetricWeight(InsideShipmentPackagePM package, ShipmentPM entityPM)
        {
            double? myResult = null;
            myResult = ComputePackageVolumetricWeight(package.Quantity, package.Width, package.Height, package.Length, package.Volume, package.Weight, entityPM.Ratio, entityPM.DimensionsUnitCode, entityPM.VolumeUnitCode, entityPM.GrossWeightUnitCode, entityPM.ChargeableWeightUnitCode);

            return myResult;
        }

        public static double? ComputePackageVolumetricWeight(double? quantity, double? width, double? height, double? length, double? volume, double? weight, double? ratio, string dimentionCode, string volumeCode, string grossWeightCode, string chargeableWeightCode)
        {
            double? myWidth = null;
            double? myHeight = null;
            double? myLength = null;
            double? myWeight = null;
            double? myRatio = null;
            double? myQuantity = null;
            double? myVolume = null;

            if (width != null)
            {
                myWidth = Convert.ToDouble(width);
            }

            if (height != null)
            {
                myHeight = Convert.ToDouble(height);
            }

            if (length != null)
            {
                myLength = Convert.ToDouble(length);
            }

            if (weight != null)
            {
                myWeight = Convert.ToDouble(weight);
            }

            if (ratio != null)
            {
                myRatio = Convert.ToDouble(ratio);
            }

            if (quantity != null)
            {
                myQuantity = Convert.ToDouble(quantity);
            }

            if (volume != null)
            {
                myVolume = Convert.ToDouble(volume);
            }

            double? myResult = null;

            if (myWidth == null || myHeight == null || myLength == null || myQuantity == null)
            {
                if (myVolume != null)
                {
                    myResult = GetWeightFromVolume(volumeCode, chargeableWeightCode, myVolume, myRatio);
                }

                else if (myWeight != null)
                {
                    myResult = GetWeightFromWeight(grossWeightCode, chargeableWeightCode, myWeight);
                }
            }

            else
            {
                myResult = GetWeightFromDimentions(myWidth, myHeight, myLength, myQuantity, dimentionCode, chargeableWeightCode, myRatio);
            }

            if (myResult != null)
            {
                myResult = Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetWeightFromVolume(string volumeCode, string weightCode, double? volume, double? ratio)
        {
            double? myVolume = null;
            double? myRatio = null;

            if (volume != null)
            {
                myVolume = Convert.ToDouble(volume);
            }

            if (ratio != null)
            {
                myRatio = Convert.ToDouble(ratio);
            }

            double? myResult = null;
            double? factorOfConvert = 1;

            if (volume != null)
            {
                double? volumeInCBM = null;
                double? weightInKilograms = null;

                if (string.IsNullOrEmpty(volumeCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (volumeCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                        case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                    }
                }

                volumeInCBM = myVolume / factorOfConvert;
                weightInKilograms = (volumeInCBM * 1000) / myRatio;

                if (string.IsNullOrEmpty(weightCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                myResult = weightInKilograms / factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetWeightFromWeight(string fromWeightCode, string toWeightCode, double? weight)
        {
            double? myWeight = null;

            if (weight != null)
            {
                myWeight = Convert.ToDouble(weight);
            }

            double? myResult = null;
            double? factorOfConvert = 1;

            if (weight != null)
            {
                if (string.IsNullOrEmpty(fromWeightCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (fromWeightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                double? weightInKilograms = myWeight * factorOfConvert;

                if (string.IsNullOrEmpty(toWeightCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (toWeightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                myResult = weightInKilograms / factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetWeightFromDimentions(double? width, double? height, double? length, double? quantity, string dimentionCode, string weightCode, double? ratio)
        {
            double? myWidth = null;
            double? myHeight = null;
            double? myLength = null;
            double? myRatio = null;
            double? myQuantity = null;

            if (width != null)
            {
                myWidth = Convert.ToDouble(width);
            }

            if (height != null)
            {
                myHeight = Convert.ToDouble(height);
            }

            if (length != null)
            {
                myLength = Convert.ToDouble(length);
            }

            if (ratio != null)
            {
                myRatio = Convert.ToDouble(ratio);
            }

            if (quantity != null)
            {
                myQuantity = Convert.ToDouble(quantity);
            }

            double? myResult = null;
            double factorOfConvert = 1;

            if (string.IsNullOrEmpty(dimentionCode))
            {
                factorOfConvert = 1;
            }

            else
            {
                switch (dimentionCode.ToUpper())
                {
                    case "CM": { factorOfConvert = 1; break; }
                    case "INC": { factorOfConvert = 2.54; break; }      // 1 inch = 2.54 centimeters
                    case "FT": { factorOfConvert = 30.48; break; }      // 1ft = 30.48 centimeters
                }
            }

            double? volumeInCentimeters = myQuantity * (myWidth * myHeight * myLength) * Math.Pow(factorOfConvert, 3);
            double? volumeInCBM = volumeInCentimeters / Math.Pow(100, 3);

            double? weightInKilograms = (volumeInCBM * 1000) / myRatio;

            if (string.IsNullOrEmpty(weightCode))
            {
                factorOfConvert = 1;
            }

            else
            {
                switch (weightCode.ToUpper())
                {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                    case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                }
            }

            myResult = weightInKilograms / factorOfConvert;

            if (myResult != null)
            {
                myResult = Round(myResult.Value, 3);
            }

            return myResult;
        }

        public static void ComputeTotals(ShipmentPM entityPM)
        {
            List<ShipmentPackagePM> notDeletedPackages = entityPM.ShipmentPackages.Where(d => d.ChangeSetOp != Simplog.Server.Infrastructure.ChangeSetOperation.Delete).ToList();
            if (notDeletedPackages.Count == 0)
            {
                entityPM.NumberOfPackages = null;
                entityPM.NumberOfContainers = null;
                entityPM.GrossWeight = null;
                entityPM.Volume = null;
                entityPM.VolumetricWeight = null;
                entityPM.ChargeableWeight = null;
                entityPM.GrossWeightInKG = null;
                entityPM.GrossWeightPerTon = null;
                entityPM.ChargeableWeightInKG = null;
            }

            else
            {
                if (MethodHelper.IsLCLEntity(entityPM.TransportModeId, entityPM.ShipmentTypeId))
                {
                    entityPM.NumberOfPackages = notDeletedPackages.Sum(s => s.Quantity);
                }

                else
                {
                    entityPM.NumberOfContainers = notDeletedPackages.Sum(s => s.Quantity);
                }

                entityPM.GrossWeight = Round(notDeletedPackages.Sum(s => s.Weight), 3);
                entityPM.Volume = Round(notDeletedPackages.Sum(s => s.Volume), 3);
                entityPM.VolumetricWeight = Round(notDeletedPackages.Sum(s => s.VolumetricWeight), 3);
                entityPM.ChargeableWeight = CalculateChargeableWeight(entityPM.GrossWeight, entityPM.VolumetricWeight, entityPM.GrossWeightUnitCode, entityPM.ChargeableWeightUnitCode, entityPM.DirectionId, entityPM.TransportModeId);
                ComputeGrossWeigh_Kg_Ton(entityPM);
                ComputeChargeableWeight_Kg(entityPM);
            }
        }
        public static double? CalculateChargeableWeight(double? grossWeight, double? volumetricWeight, string grossWeightUnitCode, string chargeableWeightUnitCode, string directionId, string transportModeId)
        {
            double? myGrossWeight = null;
            double? myVolumetricWeight = null;

            if (grossWeight != null)
            {
                myGrossWeight = Convert.ToDouble(grossWeight);
            }

            if (volumetricWeight != null)
            {
                myVolumetricWeight = Convert.ToDouble(volumetricWeight);
            }

            double? myResult = null;

            if (myGrossWeight != null || myVolumetricWeight != null)
            {

                double? grossWeightInVolumetricUnit = GetWeightFromWeight(grossWeightUnitCode, chargeableWeightUnitCode, myGrossWeight);

                if (grossWeightInVolumetricUnit != null && myVolumetricWeight == null)
                {
                    myResult = grossWeightInVolumetricUnit;
                }

                if (grossWeightInVolumetricUnit == null && myVolumetricWeight != null)
                {
                    myResult = myVolumetricWeight;
                }

                if (grossWeightInVolumetricUnit != null && myVolumetricWeight != null)
                {
                    myResult = grossWeightInVolumetricUnit > myVolumetricWeight ? grossWeightInVolumetricUnit : myVolumetricWeight;
                }
            }

            if (myResult != null)
            {
                myResult = RoundChargeableWeight(myResult, chargeableWeightUnitCode, directionId, transportModeId);
            }

            return myResult;
        }
        public static double? GetWeightFromWeight(string fromWeightCode, string toWeightCode, object weight)
        {
            double? myWeight = null;

            if (weight != null)
            {
                myWeight = Convert.ToDouble(weight);
            }

            double? myResult = null;
            double? factorOfConvert = 1;

            if (weight != null)
            {
                if (string.IsNullOrEmpty(fromWeightCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (fromWeightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                double? weightInKilograms = myWeight * factorOfConvert;

                if (string.IsNullOrEmpty(toWeightCode))
                {
                    factorOfConvert = 1;
                }

                else
                {
                    switch (toWeightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }     // 1 LB = 0.45359237 KG
                        case "MT": { factorOfConvert = 1000; break; }           // 1 mt = 1000 KG
                    }
                }

                myResult = weightInKilograms / factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? RoundChargeableWeight(object args, string chargeableWeightUnitCode, string directionId, string transportModeId)
        {
            double? myArgs = null;

            if (args != null)
            {
                myArgs = Convert.ToDouble(args);
            }

            double? result = myArgs;

            if (chargeableWeightUnitCode != "MT")
            {
                if (directionId == "E" && transportModeId == "A")
                {
                    if (result != null)
                    {
                        string toString = result.ToString();
                        string[] r = toString.Split('.');

                        if (r.Length > 1)
                        {
                            string strDigits = "0." + r[1];
                            double? digits = Convert.ToDouble(strDigits);
                            double? integer = Convert.ToDouble(r[0]);

                            if (digits <= 0.5)
                            {
                                result = integer + 0.5;
                            }

                            else
                            {
                                result = integer + 1;
                            }
                        }
                    }
                }
            }

            return result;
        }
        public static double? Round(double? value, int digits)
        {
            double? myValue = null;

            if (value != null)
            {
                myValue = Convert.ToDouble(value);
            }

            double? myResult = myValue;

            if (myValue != null && digits >= 1 && digits <= 15)
            {
                string mySTR = String.Format("{0:N" + digits + "}", myValue);

                myResult = Convert.ToDouble(mySTR);
            }

            return myResult;
        }
        private static void ComputeGrossWeigh_Kg_Ton(ShipmentPM entityPM)
        {
            double? weigh_Kg = null;
            double? weigh_Ton = null;

            if (entityPM.GrossWeight != null)
            {
                double factorOfConvert = 1;

                if (!string.IsNullOrEmpty(entityPM.GrossWeightUnitCode))
                {
                    switch (entityPM.GrossWeightUnitCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                weigh_Kg = entityPM.GrossWeight * factorOfConvert;
            }

            if (weigh_Kg != null)
            {
                weigh_Kg = Round(weigh_Kg, 3);

                weigh_Ton = weigh_Kg / 1000;
            }

            if (weigh_Ton != null)
            {
                weigh_Ton = Round(weigh_Ton, 3);
            }

            entityPM.GrossWeightInKG = weigh_Kg;
            entityPM.GrossWeightPerTon = weigh_Ton;
        }
        private static void ComputeChargeableWeight_Kg(ShipmentPM entityPM)
        {
            if (entityPM.ChargeableWeight == null) return;

            double factorOfConvert = GetChargeableWeightConvertFactor(entityPM.ChargeableWeightUnitCode);
            double? weigh_Kg = entityPM.ChargeableWeight * factorOfConvert;
            
            if (weigh_Kg != null) 
                weigh_Kg = Round(weigh_Kg, 3);

            entityPM.ChargeableWeightInKG = weigh_Kg;
        }
        private static double GetChargeableWeightConvertFactor(string chargeableWeightUnitCode)
        {
            double factorOfConvert = 1;

            if (!string.IsNullOrEmpty(chargeableWeightUnitCode))
            {
                switch (chargeableWeightUnitCode.ToUpper())
                {
                    case "KG": { factorOfConvert = 1; break; }
                    case "LB": { factorOfConvert = 0.45359237; break; }
                    case "MT": { factorOfConvert = 1000; break; }
                }
            }
            return factorOfConvert;
        }
    }
}