
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Def.EntityPMs; 
using Amital.QuoteOPM.Data;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;

namespace Amital.QuoteOPM.BL.EntityDataMappings
{
   
   public partial class QuoteOPDataMapping: IMapping<QuoteOPPM, QuoteOP>
   {
        

        public void CustomPMToPOCO(QuoteOPPM entityPM, QuoteOP entityPoco)
        {
            //throw new NotImplementedException();\
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.QuoteNumber);
            entityPoco.QuoteNumber = entityPM.QuoteNumber;
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.CreatedByUserId);
            entityPoco.CreatedByUserId = entityPM.CreatedByUserId;
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.OpenDate);
            entityPoco.OpenDate = entityPM.OpenDate;
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);
            entityPoco.Tenant = entityPM.Tenant;
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.DirectionId);
            entityPoco.DirectionId = entityPM.DirectionId;
            this.CustomMappedPOCOProperties.Add(POCOPropertyNames.ProductCode);
            entityPoco.ProductCode = entityPM.ProductCode;



            entityPoco.GrossWeightInKG = entityPM.GrossWeightInKG = GetWeightInKG(entityPM.GrossWeightUnitCode, entityPM.GrossWeight);
            entityPoco.GrossWeightPerTon = entityPM.GrossWeightPerTon = GetWeightInTon(entityPM.GrossWeightInKG);
            entityPoco.ChargeableWeightInKG = entityPM.ChargeableWeightInKG = GetChargeableWeightInKG(entityPM.ChargeableWeightUnitCode, entityPM.ChargeableWeight);


            BuildSearchField(entityPM, entityPoco);
        }
        public static double? GetWeightInKG(string weightCode, double? weight)
        {
            double? myResult = null;

            if (weight != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(weightCode))
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                myResult = weight * factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetWeightInTon(double? weightInKG)
        {
            double? myResult = null;

            if (weightInKG != null)
            {
                myResult = weightInKG / 1000;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }

        public static double? GetChargeableWeightInKG(string weightCode, double? weight)
        {
            double? myResult = null;

            if (weight != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(weightCode))
                {
                    switch (weightCode.ToUpper())
                    {
                        case "KG": { factorOfConvert = 1; break; }
                        case "LB": { factorOfConvert = 0.45359237; break; }
                        case "MT": { factorOfConvert = 1000; break; }
                    }
                }

                myResult = weight * factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        public static double? GetVolumeInCBM(string volumeCode, double? volume)
        {
            double? myResult = null;

            if (volume != null)
            {
                double? factorOfConvert = 1;

                if (!string.IsNullOrEmpty(volumeCode))
                {
                    switch (volumeCode.ToUpper())
                    {
                        case "CBM": { factorOfConvert = 1; break; }
                        case "CBI": { factorOfConvert = 61024; break; }      // 1m³ = 61024in³
                        case "CBF": { factorOfConvert = 35.315; break; }     // 1m³ = 35.315ft³
                    }
                }

                myResult = volume / factorOfConvert;
            }

            if (myResult != null)
            {
                myResult = MethodHelper.Round(myResult.Value, 3);
            }

            return myResult;
        }
        private static void BuildSearchField(QuoteOPPM entityPM, QuoteOP entityPoco)
        {
            string mySearchFields = "";

            int tenant = entityPM.Tenant;

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.QuoteNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Subject);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.Notes);

            #region Ports
            Logitude.BL.Helpers.QueryHelper.AddPortToSearchFields(ref mySearchFields, tenant, entityPM.FromPortId);
            QueryHelper.AddPortToSearchFields(ref mySearchFields, tenant, entityPM.ToPortId);
            #endregion

            #region Partners

            if (!string.IsNullOrEmpty(entityPM.ShipperId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ShipperId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipperReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ShipperReference2);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.ConsigneeId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.ConsigneeId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsigneeReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ConsigneeReference2);
                }
            }

            if (!string.IsNullOrEmpty(entityPM.CustomerId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.CustomerId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerReference1);
                    MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CustomerReference2);
                }

                if (!string.IsNullOrEmpty(entityPM.CustomerContactId))
                {
                    Contact myContact = ContactRepository.GetSingleContact(entityPM.CustomerContactId, tenant, true);
                    if (myContact != null)
                    {
                        MethodHelper.AddToSearchFields(ref mySearchFields, myContact.Email);
                    }
                }
            }

            if (!string.IsNullOrEmpty(entityPM.NotifyId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.NotifyId, tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }
            #endregion

            #region Carrier
            if (!string.IsNullOrEmpty(entityPM.MainCarriageCarrierId))
            {
                Card myCard = CardRepository.GetSingleCard(entityPM.MainCarriageCarrierId, entityPM.Tenant, true);
                if (myCard != null)
                {
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.Code);
                    MethodHelper.AddToSearchFields(ref mySearchFields, myCard.EnglishName);
                }
            }
            #endregion

            if (mySearchFields.Length > 1500)
            {
                mySearchFields = mySearchFields.Substring(0, 1500);
            }

            entityPM.SearchFields = mySearchFields;
            entityPoco.SearchFields = mySearchFields;
        }
        public void CustomPOCOToPM(QuoteOPPM entityPM, QuoteOP entityPOCO)
        {
            //throw new NotImplementedException();
        }
   }


}
   