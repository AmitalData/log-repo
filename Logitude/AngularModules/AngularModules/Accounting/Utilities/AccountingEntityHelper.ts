
export class AccountingEntityHelper {

    static getEntityIcon(_sourceTypeCode: string){
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
        }
        return iconTxt;
    }

    static getEntityObjectTableName(_sourceTypeCode: string){
        var tableName = "Journal";

        switch (_sourceTypeCode) {
            // 1-Journal
            case "1": {
                tableName = "Journal";
                break;
            }

            // 2-ARInvoice
            case "2": {
                tableName = "ARInvoice";
                break;
            }

            // 3-ARPayment
            case "3": {
                tableName = "ARPayment";

                break;
            }

            // 4-APInvoice
            case "4": {
                tableName = "APInvoice";

                break;
            }

            // 5-APPayment
            case "5": {
                tableName = "APPayment";

                break;
            }

            // 6-Cheque Deposit
            case "6": {
                tableName = "BankDeposit";

                break;
            }

            // 7-Cash Deposit
            case "7": {
                tableName = "BankDeposit";

                break;
            }

            // 8-Revaluation
            case "8": {
                tableName = "Revaluation";

                break;
            }

            // 9-PaymentCheque
            case "9": {
                tableName = "PaymentCheque";

                break;
            }

            // 10-Adjustment
            case "10": {
                tableName = "Adjustment";

                break;
            }

        }
        return tableName;

    }

}
