using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Logitude.CustomsMessaging.Common.ResponseData;
using Logitude.CustomsMessaging.ResponseServices.DeclarationErrorPointer;
using Logitude.Server.Tools.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnifreightIIG.Common.CargoQueryMessageServiceReference;
using UnifreightIIG.Common.ImportDeclarationSubmitRequestServiceReference;

namespace Logitude.CustomsMessaging.ResponseServices
{

    public class MN_NG_8241_Cargo_MessageResponseService
         :
         ResponseServiceBase<
         INF_MSG_GenericResponseData,
         MN_NG_8241_Cargo_Message,
         GenericRequestParams>
    {
        DeclarationPM _MyDeclarationPM;

        public override void Update(MN_NG_8241_Cargo_Message customResponse, GenericRequestParams requestParams)
        {
            if (requestParams.AppicationId == "0" || String.IsNullOrWhiteSpace(requestParams.AppicationId)) // || customResponse.ResponseContentHeader.Exception == null) 
            {
                if (String.IsNullOrWhiteSpace(customResponse.ResponseContentHeader.Remark))
                {
                    LogMessagingUtil.Instance.AppendLine("No Declaration details in the Response " + requestParams.AppicationId);
                }
                else
                {
                    LogMessagingUtil.Instance.AppendLine("No Declaration details in the Response " + customResponse.ResponseContentHeader.Remark + requestParams.AppicationId);
                }
                return;
            }

            var responseName = requestParams.ResponseName;
            var context = CustomContext.GetContext(requestParams.Tenant);
            var myQueryService = new DeclarationQueryService(context);
            var myDeclarationUpdateService = new DeclarationUpdateService(context, new Dictionary<string, IContext>(), requestParams.Tenant);
            DeclarationErrorPointerService mydDclarationErrorPointerService = new DeclarationErrorPointerService();

            this._MyDeclarationPM = myQueryService.GetSingle(requestParams.AppicationId, true, false);

            if (this._MyDeclarationPM == null)
            {
                LogMessagingUtil.Instance.AppendLine("Can not found declaration" + requestParams.AppicationId);
                return;
            }

            if (customResponse.ResponseContentHeader.Exception != null)
            {
                //Declaration declaration = new Declaration();
                //_MyDeclarationPM.ErrosXml = mydDclarationErrorPointerService.AnalyzeDeclarationException(customResponse.ResponseContentHeader.Exception, declaration);
            }
            else
            {
                LogMessagingUtil.Instance.AppendLine("Analyze Manifest response" + requestParams.AppicationId);

                if (_MyDeclarationPM.Consignments != null && _MyDeclarationPM.Consignments.Count() > 0)
                {
                    if (String.IsNullOrWhiteSpace(_MyDeclarationPM.Consignments[0].UnloadPortCode))
                    {

                        _MyDeclarationPM.Consignments[0].UnloadPortCode = customResponse.Cargo.CargoAdditionalData.First().unloadingLocationID;

                    }
                    if (String.IsNullOrWhiteSpace(_MyDeclarationPM.Consignments[0].StorageSiteCode))
                    {

                        _MyDeclarationPM.Consignments[0].StorageSiteCode = customResponse.Cargo.CargoAdditionalData.First().goodsReceiptPlaceSiteID;

                    }
                    if (_MyDeclarationPM.Consignments[0].ConsignmentPackages == null || _MyDeclarationPM.Consignments[0].ConsignmentPackages.Count() == 0)
                    {
                        _MyDeclarationPM.Consignments[0].ConsignmentPackages = GetDeclarationConsignmentsPackagesPM(customResponse, _MyDeclarationPM.Consignments[0]);
                    }
                    else
                    {
                        if (_MyDeclarationPM.Consignments[0].ConsignmentInternalTransitions != null && _MyDeclarationPM.Consignments[0].ConsignmentInternalTransitions.Count > 0)
                        {
                            _MyDeclarationPM.Consignments[0].ConsignmentPackages = GetDeclarationConsignmentsPackagesPMForInternalTransitions(customResponse, _MyDeclarationPM.Consignments[0]);
                        }
                    }
                }
            }
            _MyDeclarationPM.ChangeSetOp = ChangeSetOperation.Update;
            myDeclarationUpdateService.Update(_MyDeclarationPM, true);

            this.MyResponseData = new INF_MSG_GenericResponseData();
            MyResponseData.ApplicationID = requestParams.AppicationId;
            MyResponseData.Succeeded = true;
        }

        public override INF_MSG_GenericResponseData GetResponse(MN_NG_8241_Cargo_Message customResponse, GenericRequestParams requestParams)
        {
            return this.MyResponseData;
        }

        private List<ConsignmentPackagePM> GetDeclarationConsignmentsPackagesPM(MN_NG_8241_Cargo_Message customResponse, ConsignmentPM Consignment)
        {
            var declarationConsignmentsPackagesPMList = new List<ConsignmentPackagePM>();

            if (customResponse.CargoItem == null)
            {
                return declarationConsignmentsPackagesPMList;
            }

            int count = 0;
            string packtype = null;
            decimal weight = 0;
            int quntity = 0;
            customResponse.CargoItem.OrderBy(ci => ci.PackingType);
            //Array.Sort(customResponse.CargoItem);
            foreach (var package in customResponse.CargoItem)
            {
                if (package.PackingType != packtype && packtype != null)
                {
                    count++;
                    var declarationConsignmentPackage = new ConsignmentPackagePM();
                    declarationConsignmentPackage.SequenceNumeric = count;
                    declarationConsignmentPackage.ChangeSetOp = ChangeSetOperation.Insert;
                    declarationConsignmentPackage.ConsignmentNumber = Consignment.ConsignmentNumber;
                    declarationConsignmentPackage.DeclarationId = Consignment.DeclarationId;
                    declarationConsignmentPackage.LineNumber = count;
                    declarationConsignmentPackage.Tenant = Consignment.Tenant;

                    if(Consignment.ConsignmentInternalTransitions != null  && Consignment.ConsignmentInternalTransitions.Count > 0)
                    {
                        declarationConsignmentPackage.PackageMeasureQualifierCode = "3";
                    }
                    else
                    {
                        declarationConsignmentPackage.PackageMeasureQualifierCode = "2";
                    }
                    declarationConsignmentPackage.PackageTypeCode = package.PackingType;
                    if (quntity > 0)
                    {
                        declarationConsignmentPackage.PackageQuantity = quntity;
                    }

                    if (weight > 0)
                    {
                        declarationConsignmentPackage.GrossMassMeasure = weight;
                    }
                    weight = 0;
                    quntity = 0;
                    declarationConsignmentsPackagesPMList.Add(declarationConsignmentPackage);
                }
                else
                {
                    if (package.grossMassMeasureWeight.HasValue)
                    {
                        weight = weight + package.grossMassMeasureWeight.Value;
                    }
                    quntity = quntity + package.Quantity;
                }
                packtype = package.PackingType;
            }
            var lastPackage = customResponse.CargoItem.Last();
            if (weight > 0 || quntity > 0)
            {
                count++;
                var declarationConsignmentPackage = new ConsignmentPackagePM();
                declarationConsignmentPackage.SequenceNumeric = count;
                declarationConsignmentPackage.ChangeSetOp = ChangeSetOperation.Insert;
                declarationConsignmentPackage.ConsignmentNumber = Consignment.ConsignmentNumber;
                declarationConsignmentPackage.DeclarationId = Consignment.DeclarationId;
                declarationConsignmentPackage.LineNumber = count;
                declarationConsignmentPackage.Tenant = Consignment.Tenant;

                if (Consignment.ConsignmentInternalTransitions != null && Consignment.ConsignmentInternalTransitions.Count > 0)
                {
                    declarationConsignmentPackage.PackageMeasureQualifierCode = "3";
                }
                else
                {
                    declarationConsignmentPackage.PackageMeasureQualifierCode = "2";
                }
                declarationConsignmentPackage.PackageTypeCode = lastPackage.PackingType;
                if (quntity > 0)
                {
                    declarationConsignmentPackage.PackageQuantity = quntity;
                }

                if (weight > 0)
                {
                    declarationConsignmentPackage.GrossMassMeasure = weight;
                }
                declarationConsignmentsPackagesPMList.Add(declarationConsignmentPackage);
            }

            return declarationConsignmentsPackagesPMList;
        }

        private List<ConsignmentPackagePM> GetDeclarationConsignmentsPackagesPMForInternalTransitions(MN_NG_8241_Cargo_Message customResponse, ConsignmentPM Consignment)
        {
            var declarationConsignmentsPackagesPMList = new List<ConsignmentPackagePM>();

            if (customResponse.CargoItem == null)
            {
                return declarationConsignmentsPackagesPMList;
            }

            ConsignmentPackagePM internalConsignmentPackagePM = null;
            foreach (var consignmentPackageItem in Consignment.ConsignmentPackages)
            {
                if(consignmentPackageItem.PackageMeasureQualifierCode == "3")
                {
                    internalConsignmentPackagePM = consignmentPackageItem;
                }
            }

            int count = 0;
            string packtype = null;
            decimal weight = 0;
            int quntity = 0;
            customResponse.CargoItem.OrderBy(ci => ci.PackingType);
            //Array.Sort(customResponse.CargoItem);
            foreach (var package in customResponse.CargoItem)
            {
                if (package.PackingType != packtype && packtype != null)
                {
                    if (internalConsignmentPackagePM == null)
                    {
                        count++;
                        var declarationConsignmentPackage = new ConsignmentPackagePM();
                        declarationConsignmentPackage.SequenceNumeric = count;
                        declarationConsignmentPackage.ChangeSetOp = ChangeSetOperation.Insert;
                        declarationConsignmentPackage.ConsignmentNumber = Consignment.ConsignmentNumber;
                        declarationConsignmentPackage.DeclarationId = Consignment.DeclarationId;
                        declarationConsignmentPackage.LineNumber = count;
                        declarationConsignmentPackage.Tenant = Consignment.Tenant;
                        declarationConsignmentPackage.PackageMeasureQualifierCode = "3";
                        declarationConsignmentPackage.PackageTypeCode = package.PackingType;
                        if (quntity > 0)
                        {
                            declarationConsignmentPackage.PackageQuantity = quntity;
                        }

                        if (weight > 0)
                        {
                            declarationConsignmentPackage.GrossMassMeasure = weight;
                        }
                        weight = 0;
                        quntity = 0;
                        declarationConsignmentsPackagesPMList.Add(declarationConsignmentPackage);
                    }
                    else
                    {
                        internalConsignmentPackagePM.ChangeSetOp = ChangeSetOperation.Insert;
                        internalConsignmentPackagePM.PackageQuantity = quntity;
                        internalConsignmentPackagePM.GrossMassMeasure = weight;
                        declarationConsignmentsPackagesPMList.Add(internalConsignmentPackagePM);
                    }
                }
                else
                {
                    if (package.grossMassMeasureWeight.HasValue)
                    {
                        weight = weight + package.grossMassMeasureWeight.Value;
                    }
                    quntity = quntity + package.Quantity;
                }
                packtype = package.PackingType;
            }
            var lastPackage = customResponse.CargoItem.Last();
            if (weight > 0 || quntity > 0)
            {
                if (internalConsignmentPackagePM == null)
                {
                    count++;
                    var declarationConsignmentPackage = new ConsignmentPackagePM();
                    declarationConsignmentPackage.SequenceNumeric = count;
                    declarationConsignmentPackage.ChangeSetOp = ChangeSetOperation.Insert;
                    declarationConsignmentPackage.ConsignmentNumber = Consignment.ConsignmentNumber;
                    declarationConsignmentPackage.DeclarationId = Consignment.DeclarationId;
                    declarationConsignmentPackage.LineNumber = count;
                    declarationConsignmentPackage.Tenant = Consignment.Tenant;
                    declarationConsignmentPackage.PackageMeasureQualifierCode = "3";
                    declarationConsignmentPackage.PackageTypeCode = lastPackage.PackingType;
                    if (quntity > 0)
                    {
                        declarationConsignmentPackage.PackageQuantity = quntity;
                    }

                    if (weight > 0)
                    {
                        declarationConsignmentPackage.GrossMassMeasure = weight;
                    }
                    declarationConsignmentsPackagesPMList.Add(declarationConsignmentPackage);
                }
                else
                {
                    internalConsignmentPackagePM.ChangeSetOp = ChangeSetOperation.Insert;
                    internalConsignmentPackagePM.PackageQuantity = quntity;
                    internalConsignmentPackagePM.GrossMassMeasure = weight;
                    declarationConsignmentsPackagesPMList.Add(internalConsignmentPackagePM);
                }
            }
            return declarationConsignmentsPackagesPMList;
        }

        T SetCodeTypeValue<T>(string val)
              where T : CodeType, new()
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return null;
            }
            return new T()
            {
                listID = "",
                listAgencyName = "",
                listName = "",
                listVersionID = "",
                name = "",
                listURI = "",
                listSchemeURI = "",
                Value = val
            };

        }


        T SetQuantityTypeValue<T>(string measurementUnit, decimal val)
               where T : QuantityType, new()
        {
            var measurementUnitRealString = "";
            if (String.IsNullOrWhiteSpace(measurementUnit)) // hard coded
            {
                measurementUnitRealString = MeasurementUnitCommonCodeContentType.EA.ToString();
            }
            else
            {
                var list = Enum.GetNames(typeof(MeasurementUnitCommonCodeContentType)).ToList();
                if (list.Exists(unit => unit == measurementUnit))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit);
                }
                else if (list.Exists(unit => unit == measurementUnit.Reverse()))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit.Reverse());
                }
            }
            if (String.IsNullOrWhiteSpace(measurementUnitRealString))
            {
                return null;
            }

            MeasurementUnitCommonCodeContentType measurementCommonUnit;
            var success = Enum.TryParse(measurementUnitRealString, out measurementCommonUnit);

            if (!success)
            {
                ///throw new System.Exception("measurementUnit is not valid " + measurementUnit);  
            }
            ;

            return new T()
            {
                unitCode = measurementCommonUnit,
                unitCodeSpecified = success,
                Value = val
            };

        }

        T SetMeasureTypeValue<T>(string measurementUnit, decimal val)
               where T : MeasureType, new()
        {

            var measurementUnitRealString = "";
            if (String.IsNullOrWhiteSpace(measurementUnit)) // hard coded
            {
                measurementUnitRealString = MeasurementUnitCommonCodeContentType.KGM.ToString();
            }
            else
            {
                var list = Enum.GetNames(typeof(MeasurementUnitCommonCodeContentType)).ToList();
                if (list.Exists(unit => unit == measurementUnit))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit);
                }
                else if (list.Exists(unit => unit == measurementUnit.Reverse()))
                {
                    measurementUnitRealString = list.FirstOrDefault(unit => unit == measurementUnit.Reverse());
                }
            }
            if (String.IsNullOrWhiteSpace(measurementUnitRealString))
            {
                return null;
            }

            MeasurementUnitCommonCodeContentType measurementCommonUnit;
            var success = Enum.TryParse(measurementUnitRealString, out measurementCommonUnit);

            if (!success)
            {
                ///throw new System.Exception("measurementUnit is not valid " + measurementUnit);  
            }
            ;

            return new T()
            {
                unitCode = measurementCommonUnit,
                unitCodeSpecified = success,
                Value = val
            };
        }

    }
}
