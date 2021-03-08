import { JournalPM } from '../EntityPMs/JournalPM';
import { JournalLinePM } from '../EntityPMs/JournalLinePM';
import {AppTool, DateTool} from '../../Infrastructure/Tools';
import {SessionLocator} from '../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../Infrastructure/Utilities/TextCodeTranslator';

export class JournalValidator
{
    private static CurrentSession = SessionLocator.SelectedSession;
    private static FutureDateErrorsMessage: string;
    private static IsFutureDateErrorsExist: boolean = true;

    public static ValidateJournal(entityPM: any)
    {
        return [];
    }

    public static ValidateAccountingDate(entityPM: any) {


        return [];
    }


    public static ValidateJournalLines(line: any) {
        var errors = [];
        if (line) {
            if (line.ActionCode == null || line.ActionCode == undefined) {
                // you must choose action code
                errors.push(TextCodeTranslator.Translate("Accounting.General.O.chooseActionCode") + " " + line.Line  ); // + "You must choose action code for line "
            } else {
                // Credit Account
                if (line.ActionCode == '1' || line.ActionCode == '3' || line.ActionCode == '4') {
                    if (!line.CreditAccountId) {
                        //you must choose Credit account
                        errors.push(TextCodeTranslator.Translate("Accounting.General.O.chooseCreditAccount") + " " + line.Line  ); //+"You must choose Credit account for line "
                    }
                }

                // Debit Account
                if (line.ActionCode == '2' || line.ActionCode == '3' || line.ActionCode == '4') {
                    if (!line.DebitAccountId) {
                        //you must choose Debit account
                        errors.push(TextCodeTranslator.Translate("Accounting.General.O.chooseDebitAccount") + " " + line.Line  ); // +"You must choose Debit account for line "

                    }
                }

                // Currency
                if (!line.CurrencyId) {
                    //you must choose Currency
                    errors.push(TextCodeTranslator.Translate("Accounting.General.O.chooseCurrency") + " " + line.Line  ); //You must choose Currency for line

                }

                // Amount
                if (!line.LocalAmount || !line.ForeignAmount) {
                    //Amount is missing
                    errors.push(TextCodeTranslator.Translate("Accounting.General.O.AmountIsMissing") + " " + line.Line  ); //Amount is missing for line

                }

               // this.ValidateJournalLineForFutureDate(line);

                // Credit and Debit account (same currency)
                if (line.ActionCode == '3') {
                    if (line.CreditAccount == undefined || line.DebitAccount == undefined) return errors;
                    if (!line.CreditAccount.IsMultiCurrency && !line.DebitAccount.IsMultiCurrency) {
                        if (line.CreditAccount.CurrencyId != line.DebitAccount.CurrencyId) {
                            errors.push(TextCodeTranslator.Translate("Accounting.General.O.CreditDebitAccountMustSameCurrency")); //Credit and Debit account must be the same currency
                        }
                    }
                }

                
                // Ref. + Due Dates
                if (!line.DocumentDate) {
                    errors.push(TextCodeTranslator.Translate("Accounting.General.O.chooseRefDate") + " " + line.Line  ); //You should choose Ref. Date for line
                }
                if (!line.DueDate) {
                    errors.push(TextCodeTranslator.Translate("Accounting.General.O.chooseDueDate") + " " + line.Line  ); //You should choose Due Date for line
                }

                if (line.isValid != undefined) { // when this function called from editcomponent save button, the line does not have isvalid property
                    if (!line.isValid) {
                        errors.push(TextCodeTranslator.Translate("Journal.O.TheAccountingDayMustBeInRange"));
                    }
                }

            }
        }
        //this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
        SessionLocator.SelectedSession.CurrentEditComponent.ValidationErrorsList=errors;
        return errors;
    }


    private static ValidateJournalLineForFutureDate(line: any) {
        var journalLineHasFutureDate = this.CheckJournalLineForFutureDate(line)
       
        if (journalLineHasFutureDate) {
            this.SetFutureDateErrorsMessage(line);
        }
    }

    private static CheckJournalLineForFutureDate(line: any) {
        var currentDate: Date = new Date();
        return line.DocumentDate > DateTool.GetCurrentDateTimeAsUtc() || line.AccountingDate > DateTool.GetCurrentDateTimeAsUtc();
 
    }

     private static SetFutureDateErrorsMessage(line: any) {
        this.FutureDateErrorsMessage += this.IsFutureDateErrorsExist ? TextCodeTranslator.Translate("Journal.O.FutureDateIsNotAllowedInLine") + " " : ", ";
        this.FutureDateErrorsMessage += line.Line;
        this.IsFutureDateErrorsExist = false;
    }

    public static ValidateTotals(entityPM: JournalPM) {

        var errors = [];

        if (entityPM.StatusCode == "1" || entityPM.StatusCode == "2" || entityPM.StatusCode == "3")
        {

            var cSum: number = 0;
            var dSum: number = 0;

            for (let line of entityPM.JournalLines) {
                var lineAccountingDate = new Date(line.AccountingDate.toString());
                var lineDay = lineAccountingDate.getDate();
                var lineMonth = lineAccountingDate.getMonth() + 1;

                var headerAccountingDate = new Date(entityPM.AccountingDate.toString());
                var headerMonth = headerAccountingDate.getMonth() + 1;

                if (lineMonth != headerMonth) {
                    line.AccountingDate = headerAccountingDate;
                    line.AccountingDate.setDate(lineDay);
                }

                if (!AppTool.IsNullOrEmpty(line.LocalAmount)) {
                    if (line.ActionCode == "1")
                        cSum += line.LocalAmount;
                    else if (line.ActionCode == "2")
                        dSum += line.LocalAmount;
                    else if (line.ActionCode == "3") {
                        cSum += line.LocalAmount;
                        dSum += line.LocalAmount;
                    }
                    else if (line.ActionCode == "4") {
                        cSum += line.LocalAmount;
                        dSum += line.LocalAmount;
                    }
                }

            }
            if (cSum.toFixed(2) != dSum.toFixed(2)) {
                errors.push(TextCodeTranslator.Translate("Accounting.General.O.TotalDebitMustEqualTotalCredit") + ": " + JournalValidator.Abs(dSum - cSum).toFixed(2)); //Total debit amount must be equal to total credit amount, There is a difference of
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = errors;
            } else {
                this.CurrentSession.CurrentEditComponent.ValidationErrorsList = [];
            }
        }
        return errors;
    }
    FillErrorList(result:string[]) {
        if (result.length > 0) {
            for (var error in result) {
                this.errorList.push(result[error]);
            }
        }
      }
    errorList: string[];
    public Validate(entityPM: JournalPM) {

        JournalValidator.CurrentSession = SessionLocator.SelectedSession;

        this.errorList = [];
        var result = [];
        JournalValidator.FutureDateErrorsMessage = "";
        JournalValidator.IsFutureDateErrorsExist = true;
        // Validate last row of journal lines
        //if (!AppTool.IsNullOrEmpty(entityPM.JournalLines)) {
        //    var lastRow = entityPM.JournalLines[entityPM.JournalLines.length - 1];
        //}
       
        for (var line in entityPM.JournalLines) {
            var journalLine = entityPM.JournalLines[line];
            result = JournalValidator.ValidateJournalLines(journalLine);
            this.FillErrorList(result); 
        }

        this.SetFutureDateErrorsIfExist(entityPM);

        // Validate Totals
        result = JournalValidator.ValidateTotals(entityPM)
        if (result.length > 0) {
            this.FillErrorList(result); 
            return this.errorList;
        }

        return this.errorList ;
    }

    SetFutureDateErrorsIfExist(entityPM: JournalPM) {
        const statusCode_JournalApproved : string = "2";
        var isIsFutureDateErrorsExistAndJournalApproved = !JournalValidator.IsFutureDateErrorsExist && entityPM.StatusCode == statusCode_JournalApproved;
        if (isIsFutureDateErrorsExistAndJournalApproved)
            this.errorList.push(JournalValidator.FutureDateErrorsMessage);
    }

    public static Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }


    public static IsAccDayValid(date: Date, day: number) {
        if (day > 0 && day < 32) {
            var lastDayOfMonth = this.lastDay(date.getFullYear(), date.getMonth());
            if (day > lastDayOfMonth) {
                //error
                //this.UIProperties.SetValidity("AccDay", this.ObjectTableName, false, this.accountingDayMustBeInRange);
                return false;
                //var t = setTimeout(() => {
                //    this.AccDay = value;
                //});
            } else {
                //this.UIProperties.SetValidity("AccDay", this.ObjectTableName, true, "valid");
                //this.AccountingDate = new Date(date.setDate(day));
                return true;

            }
        } else {
            //error
            //this.UIProperties.SetValidity("AccDay", this.ObjectTableName, false, this.accountingDayMustBeInRange);
            //this.isValid = false;
            //var t = setTimeout(() => {
            //    this.AccDay = value;
            //});
            return false;
        }

    }


    public static lastDay(year, month) {
        return new Date(year, month + 1, 0).getDate();
    }


}

