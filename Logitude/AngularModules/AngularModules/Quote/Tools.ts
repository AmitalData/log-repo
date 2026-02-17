import {QuotePM} from './EntityPMs/QuotePM';
import {SessionLocator} from '../Infrastructure/Utilities/SessionLocator';
import {AppTool, ArrayTool} from '../Infrastructure/Tools';
import {QuoteStageList} from './EntityLists/QuoteStageList';
import {QuoteStageListService} from './Services/StandardLists/QuoteStageListService';

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
                                case "QTY": { myCostQuantity = entityPM.NumberOfPackages; break; }
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
}