import {QuotePM} from './EntityPMs/QuotePM';
import {AppTool, ArrayTool} from '../Infrastructure/Tools';
import {QuoteStageList} from './EntityLists/QuoteStageList';
import {QuoteStageListService} from './Services/StandardLists/QuoteStageListService';
import { QuoteUtilities } from './Utilities/QuoteUtilities';
import { SessionLocator } from '../Infrastructure/Utilities/SessionLocator';

export class QuoteTool {
    public static IsQuoteEditEnabled(entityPM: QuotePM) {
        var myResult: boolean = true;

        var d = "ayman";
        if (entityPM != null) {
            if (entityPM.IsClosed) {
                myResult = false;
            }

            else if (entityPM.IsCancelled) {
                myResult = false;
            }

            else if (entityPM.IsQuoteDataExternal && entityPM.IsQuoteDocumentExternal) {
                myResult = false;
            }
            else {
                var myService: QuoteStageListService = new QuoteStageListService();
                myService.getAllFromCache().subscribe((resp: any) => {
                    if (!resp.HasError) {
                        var allStages = resp.Result;

                        var mySentStageId = "";
                        var mySentStage: QuoteStageList = allStages.filter(d => d.Code == "QTST")[0];

                        if (mySentStage != null) {
                            mySentStageId = mySentStage.Id;
                        }

                        if (entityPM.StageId == mySentStageId) {
                            myResult = false;
                        }
                    }
                });
            }
        }

        return myResult;
    }
    public static IsLCLQuote(entityPM: QuotePM) {
        var myResult = false;

        if (entityPM != null) {
            if (!AppTool.IsNullOrEmpty(entityPM.TransportModeId) && entityPM.TransportModeId.toUpperCase() == "A") {
                myResult = true;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.TransportModeId) && entityPM.TransportModeId.toUpperCase() == "O" && entityPM.ShipmentTypeId.toUpperCase() == "LCLD") {
                myResult = true;
            }

            else if (!AppTool.IsNullOrEmpty(entityPM.TransportModeId) && entityPM.TransportModeId.toUpperCase() == "I" && entityPM.ShipmentTypeId.toUpperCase() == "LTL") {
                myResult = true;
            }
        }

        return myResult;
    }
    public static GetFromPortTextCode(TransportModeId: string) {
        var myResult: string = null;

        switch (TransportModeId) {
            case "A": { myResult = "Quote.S.NewQuote.Gateway"; break; }
            case "O": { myResult = "Quote.S.NewQuote.LoadingPort"; break; }
            case "I": { myResult = "Quote.S.NewQuote.From"; break; }
            default: { myResult = "Quote.S.NewQuote.From"; break; }
        }

        return myResult;
    }
    public static GetToPortTextCode(TransportModeId: string) {
        var myResult: string = null;

        switch (TransportModeId) {
            case "A": { myResult = "Quote.S.NewQuote.Destination"; break; }
            case "O": { myResult = "Quote.S.NewQuote.DischargePort"; break; }
            case "I": { myResult = "Quote.S.NewQuote.To"; break; }
            default: { myResult = "Quote.S.NewQuote.To"; break; }
        }

        return myResult;
    }
    public static OnQuoteQuantitiesChanged(entityPM: QuotePM) {
        if (entityPM) {
            if (entityPM.QuoteTypeCode == "A") {
                var isLCL = QuoteTool.IsLCLQuote(entityPM);
                if (isLCL) {
                    if (entityPM.QuoteCharges.filter(d => d.SaleUnitPrice != null || d.CostUnitPrice != null).length == 0) {
                        entityPM.QuoteCharges.forEach(item => {

                            var myCostQuantity: number = null;
                            switch (item.CostMeasurementCode) {
                                case "GRWT": { myCostQuantity = entityPM.GrossWeight; break; }
                                case "CHWT": { myCostQuantity = entityPM.ChargeableWeight; break; }
                                case "VOLU": { myCostQuantity = entityPM.Volume; break; }
                                case "BTEU": { myCostQuantity = entityPM.TEU; break; }
                                case "FIXD": { myCostQuantity = 1; break; }
                                case "GWTN": { myCostQuantity = entityPM.GrossWeightPerTon; break; }
                                case "PRVL": { myCostQuantity = entityPM.ValueOfGoods; break; }
                                case "PRFR": { myCostQuantity = ArrayTool.Sum(entityPM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT"), "CostTotalAmount"); break; }
                                case "QTY": { myCostQuantity = entityPM.NumberOfPackages; break; }
                                case "CWKG": { myCostQuantity = entityPM.ChargeableWeightInKG; break; }
                                case "GWKG": { myCostQuantity = entityPM.GrossWeightInKG; break; }
                                case "VCBM": { myCostQuantity = entityPM.VolumeInCBM; break; }
                                case "PDCW": { myCostQuantity = entityPM.PickupDeliveryChargeableWeight; break; }
                                case "PFCL": { myCostQuantity = ArrayTool.Sum(entityPM.QuoteCharges.filter(d => d.CostCurrencyId != SessionLocator.LocalCurrencyId), "CostTotalAmountLocal"); break; }
                                default: { break; }
                            }

                            var mySaleQuantity = null;
                            switch (item.SaleMeasurementCode) {
                                case "GRWT": { mySaleQuantity = entityPM.GrossWeight; break; }
                                case "CHWT": { mySaleQuantity = entityPM.ChargeableWeight; break; }
                                case "VOLU": { mySaleQuantity = entityPM.Volume; break; }
                                case "BTEU": { mySaleQuantity = entityPM.TEU; break; }
                                case "FIXD": { mySaleQuantity = 1; break; }
                                case "GWTN": { mySaleQuantity = entityPM.GrossWeightPerTon; break; }
                                case "PRVL": { mySaleQuantity = entityPM.ValueOfGoods; break; }
                                case "PRFR": { mySaleQuantity = ArrayTool.Sum(entityPM.QuoteCharges.filter(d => d.ChargesGroupCode == "FRT"), "SaleTotalAmount"); break; }
                                case "QTY": { mySaleQuantity = entityPM.NumberOfPackages; break; }
                                case "CWKG": { mySaleQuantity = entityPM.ChargeableWeightInKG; break; }
                                case "GWKG": { mySaleQuantity = entityPM.GrossWeightInKG; break; }
                                case "VCBM": { mySaleQuantity = entityPM.VolumeInCBM; break; }
                                case "PDCW": { mySaleQuantity = entityPM.PickupDeliveryChargeableWeight; break; }
                                case "PFCL": { mySaleQuantity = ArrayTool.Sum(entityPM.QuoteCharges.filter(d => d.SaleCurrencyId != SessionLocator.LocalCurrencyId), "SaleTotalAmountLocal"); break; }
                                default: { break; }
                            }

                            if (item.CostQuantity != myCostQuantity) {
                                item.CostQuantity = AppTool.Round(myCostQuantity, 2);
                            }

                            if (item.SaleQuantity != mySaleQuantity) {
                                item.SaleQuantity = AppTool.Round(mySaleQuantity, 2);
                            }

                            // Ayman
                            // No need to Compute anything else (Amounts = Quantity * UnitPrice)
                            // this Code is applied only when there is NO unit price
                        });
                    }
                }

                else {

                }
            }
        }
    }
    public static IsQuoteStageDraft(entityPM: QuotePM) {
        var myResult: boolean = true;

        if (entityPM != null) {
            var myService: QuoteStageListService = new QuoteStageListService();
            myService.getAllFromCache().subscribe((resp: any) => {
                if (!resp.HasError) {
                    var allStages = resp.Result;

                    var draftStageId = "";
                    var draftStage: QuoteStageList = allStages.filter(d => d.Code == "QTDR")[0];

                    if (draftStage != null) {
                        draftStageId = draftStage.Id;
                    }

                    if (entityPM.StageId == draftStageId) {
                        myResult = true;
                    }
                }
            });
        }

        return myResult;
    }

    public static CheckUpdateQuantities(entityPM: QuotePM) {
        var myResult: boolean = false;

        var isLCL = QuoteUtilities.IsLCLQuote(entityPM);

        if (isLCL) {
            myResult = this.CheckUpdateQuantities_LCL(entityPM);
        }

        else {
            myResult = this.CheckUpdateQuantities_FCL(entityPM);
        }

        return myResult;
    }
    private static CheckUpdateQuantities_LCL(entityPM: QuotePM) {
        var myResult: boolean = false;

        if (entityPM.QuoteTypeCode == "A") {
            if (entityPM.QuoteCharges.filter(d => d.SaleUnitPrice != null || d.CostUnitPrice != null).length > 0) {
                var entityQuantity: number = null;

                //"PFCL"
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PFCL" || f.SaleMeasurementCode == "PFCL").length > 0) {
                    myResult = this.CheckUpdateMessageforPFCL(entityPM);
                }

                //"GRWT"
                entityQuantity = entityPM.GrossWeight;
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "GRWT" && f.CostQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }
                else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "GRWT" && f.SaleQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }

                //"GWTN"
                entityQuantity = entityPM.GrossWeightPerTon;
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "GWTN" && f.CostQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }
                else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "GWTN" && f.SaleQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }

                //"CHWT"
                entityQuantity = entityPM.ChargeableWeight;
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "CHWT" && f.CostQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }
                else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "CHWT" && f.SaleQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }

                //CWKG
                entityQuantity = entityPM.ChargeableWeightInKG;
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "CWKG" && f.CostQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }
                else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "CWKG" && f.SaleQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }

                //GWKG
                entityQuantity = entityPM.GrossWeightInKG;
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "GWKG" && f.CostQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }
                else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "GWKG" && f.SaleQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }

                //"VOLU"
                entityQuantity = entityPM.Volume;
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "VOLU" && f.CostQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }
                else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "VOLU" && f.SaleQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }

                //"VCBM"
                entityQuantity = entityPM.VolumeInCBM;
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "VCBM" && f.CostQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }
                else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "VCBM" && f.SaleQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }


                // PDCW
                entityQuantity = entityPM.PickupDeliveryChargeableWeight;
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PDCW" && f.CostQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }
                else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "PDCW" && f.SaleQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }

                //"BTEU"
                entityQuantity = entityPM.TEU;
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "BTEU" && f.CostQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }
                else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "BTEU" && f.SaleQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }

                //"QTY"
                entityQuantity = entityPM.NumberOfPackages;
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "QTY" && f.CostQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }
                else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "QTY" && f.SaleQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }

                //"PRVL"
                entityQuantity = entityPM.ValueOfGoods;
                if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PRVL" && f.CostQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }
                else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "PRVL" && f.SaleQuantity != entityQuantity).length > 0) {
                    myResult = true;
                }

                //"PRFR"
                if (entityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT").length > 0) {
                    if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PRFR" || f.SaleMeasurementCode == "PRFR").length > 0) {

                        var FRT_CostQuantity = entityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT")[0].CostTotalAmount;
                        var FRT_SaleQuantity = entityPM.QuoteCharges.filter(f => f.ChargesGroupCode == "FRT")[0].SaleTotalAmount;

                        if (AppTool.IsNullOrZero(FRT_CostQuantity)) {
                            FRT_CostQuantity = 0;
                        }

                        if (AppTool.IsNullOrZero(FRT_SaleQuantity)) {
                            FRT_SaleQuantity = 0;
                        }

                        if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PRFR" && f.CostQuantity != null && f.CostQuantity != 0 && f.CostQuantity != FRT_CostQuantity).length > 0) {
                            myResult = true;
                        }

                        if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "PRFR" && f.SaleQuantity != null && f.SaleQuantity != 0 && f.SaleQuantity != FRT_SaleQuantity).length > 0) {
                            myResult = true;
                        }
                    }
                }
            }
        }

        return myResult;
    }

    private static CheckUpdateMessageforPFCL(entityPM: QuotePM): boolean {
        var isDifferentOrders = false;
        var PFCL_CostQuantity = ArrayTool.Sum(entityPM.QuoteCharges.filter(d => d.CostCurrencyId != SessionLocator.LocalCurrencyId), "CostTotalAmountLocal");
        var PFCL_SaleQuantity = ArrayTool.Sum(entityPM.QuoteCharges.filter(d => d.SaleCurrencyId != SessionLocator.LocalCurrencyId), "SaleTotalAmountLocal");;

        if (AppTool.IsNullOrZero(PFCL_CostQuantity)) {
            PFCL_CostQuantity = 0;
        }

        if (AppTool.IsNullOrZero(PFCL_SaleQuantity)) {
            PFCL_SaleQuantity = 0;
        }

        if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "PFCL" && f.CostQuantity != null && f.CostQuantity != 0 && f.CostQuantity != PFCL_CostQuantity).length > 0) {
            isDifferentOrders = true;
        }

        if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "PFCL" && f.SaleQuantity != null && f.SaleQuantity != 0 && f.SaleQuantity != PFCL_SaleQuantity).length > 0) {
            isDifferentOrders = true;
        }
        return isDifferentOrders;
    }
    private static CheckUpdateQuantities_FCL(entityPM: QuotePM) {
        var myResult: boolean = false;

        if (entityPM.QuoteTypeCode == "A") {
            var entityQuantity: number = null;

            entityQuantity = entityPM.TEU;
            if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "BTEU" && f.CostQuantity != entityQuantity).length > 0) {
                myResult = true;
            }
            else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "BTEU" && f.SaleQuantity != entityQuantity).length > 0) {
                myResult = true;
            }

            if (entityPM.QuoteCharges.filter(f => f.CostMeasurementCode == "QTY" && f.CostQuantity != entityPM.NumberOfContainers).length > 0) {
                myResult = true;
            }
            else if (entityPM.QuoteCharges.filter(f => f.SaleMeasurementCode == "QTY" && f.SaleQuantity != entityPM.NumberOfContainers).length > 0) {
                myResult = true;
            }

            if (entityPM.QuoteCharges.filter(d => d.SaleUnitPrice != null || d.CostUnitPrice != null).length > 0) {
                if (entityPM.QuoteCharges.filter(d => (d.CostMeasurementCode == "PRVL" && d.CostQuantity != entityPM.ValueOfGoods) || (d.CostMeasurementCode == "PRVL" && d.CostQuantity != entityPM.ValueOfGoods)).length > 0) {
                    myResult = true;
                }

                else if (entityPM.QuoteCharges.filter(d => d.SaleMeasurementCode == "PRVL" && d.SaleQuantity != entityPM.ValueOfGoods).length > 0) {
                    myResult = true;
                }
            }
        }

        return myResult;
    }
}
