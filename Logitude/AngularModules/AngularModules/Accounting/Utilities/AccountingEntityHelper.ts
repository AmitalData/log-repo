import { AppTool } from "Infrastructure/Tools";
import { SessionLocator } from "Infrastructure/Utilities/SessionLocator";

export class AccountingEntityHelper {

    static getEntityIcon(_sourceTypeCode: string) {
        var iconTxt = "";
        switch (_sourceTypeCode) {
            // 1-Journal
            case "1": {
                iconTxt = "JR";
                break;
            }

            // 2-ARInvoice
            case "2": {
                iconTxt = "IN";
                break;
            }

            // 3-ARPayment
            case "3": {
                iconTxt = "PY";
                break;
            }

            // 4-APInvoice
            case "4": {
                iconTxt = "IN";
                break;
            }

            // 5-APPayment
            case "5": {
                iconTxt = "PY";

                break;
            }

            // 6-Cheque Deposit
            case "6": {
                iconTxt = "DP";

                break;
            }

            // 7-Cash Deposit
            case "7": {
                iconTxt = "DP";

                break;
            }

            // 8-Revaluation
            case "8": {
                iconTxt = "RV";

                break;
            }

            // 9-PaymentCheque
            case "9": {
                iconTxt = "CH";

                break;
            }

            // 10-Adjustment
            case "10": {
                iconTxt = "AJ";

                break;
            }
            case AccountingEntityValues.TaxReport: {
                iconTxt = "TR";
                break;
            }
        }
        return iconTxt;
    }

    static GetPartnerTypeObjectTableName(partnerTypeId: string){
        var objectTableName;
        switch (partnerTypeId) {
            case 'AG': { objectTableName = 'Agent'; break; }
            case 'AL': { objectTableName = 'Airline'; break; }
            case 'CG': { objectTableName = 'CustomAgent'; break; }
            case 'CH': { objectTableName = 'CustomsShipper'; break; }
            case 'CS': { objectTableName = 'Customer'; break; }
            case 'PO': { objectTableName = 'Customer'; break; }
            case 'PT': { objectTableName = 'Participant'; break; }
            case 'SG': { objectTableName = 'ShippingAgent'; break; }
            case 'SL': { objectTableName = 'ShippingLine'; break; }
            case 'TR': { objectTableName = 'Trucker'; break; }
            case 'VD': { objectTableName = 'Vendor'; break; }
            case 'WH': { objectTableName = 'Warehouse'; break; }
            case 'AC': { objectTableName = 'AccountingPartner'; break; }

            case 'CC': { objectTableName = 'Custom Clearance'; break; } // not found
            case 'CO': { objectTableName = 'Coloader'; break; } // not found
            case 'FL': { objectTableName = 'Freelancer'; break; } // not found
            case 'OT': { objectTableName = 'Others'; break; } // not found
        }
        return objectTableName;
    }

    static OpenCard(connectedCardId: string, partnerTypeName: string, selectedTabCode: string)
    {
        if (!AppTool.IsNullOrEmpty(connectedCardId)) {
            SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', SessionLocator.SelectedSession.SessionLocation.viewContainerRef)
                .then(cmpRef =>
                {
                    cmpRef.instance.ComponentRef = cmpRef;
                    cmpRef.instance.Run({ EntityId: connectedCardId, ObjectTableName: partnerTypeName == "Others" || partnerTypeName == "Coloader" ? "Vendor" : partnerTypeName, SelectedTabCode: selectedTabCode });
                    cmpRef.instance.BackCompleted.subscribe(bk =>
                    {
                    });
                });
        }
    }
    
    static getEntityObjectTableName(sourceTypeCode: string) {
        let tableName = 'Journal';

        switch (sourceTypeCode) {
            // 1-Journal
            case '1': {
                tableName = 'Journal';
                break;
            }

            // 2-ARInvoice
            case '2': {
                tableName = 'ARInvoice';
                break;
            }

            // 3-ARPayment
            case '3': {
                tableName = 'ARPayment';

                break;
            }

            // 4-APInvoice
            case '4': {
                tableName = 'APInvoice';

                break;
            }

            // 5-APPayment
            case '5': {
                tableName = 'APPayment';

                break;
            }

            // 6-Cheque Deposit
            case '6': {
                tableName = 'BankDeposit';

                break;
            }

            // 7-Cash Deposit
            case '7': {
                tableName = 'BankDeposit';

                break;
            }

            // 8-Revaluation
            case '8': {
                tableName = 'Revaluation';

                break;
            }

            // 9-PaymentCheque
            case '9': {
                tableName = 'PaymentCheque';

                break;
            }

            // 10-Adjustment
            case '10': {
                tableName = 'Reconciliation';

                break;
            }

            // 11-InterestReport
            case '11': {
                tableName = 'InterestReport';

                break;
            }
            case '12': {
               tableName = 'ExternalReconciliation';

               break;
            }
            case AccountingEntityValues.TaxReport: {
                tableName = 'TaxReport';
                break;
            }

        }
        return tableName;

    }


}

export class AccountingEntityValues
{
   public static Journal: string = "1";
   public static ARInvoice: string = "2";
   public static ARPayment: string = "3";
   public static APInvoice: string = "4";
   public static APPayment: string = "5";
   public static ChequeDeposit: string = "6";
   public static CashDeposit: string = "7";
   public static Revaluation: string = "8";
   public static PaymentCheque: string = "9";
   public static Adjustment: string = "10";
   public static YearTransfer: string = "11";
   public static BankAdjustment: string = "12";
   public static TaxReport: string = "13";
}
