import { CounterDefinitionPM } from "../../../Common/EntityPMs/CounterDefinitionPM";
import { MessageWindow } from "../../../Controls/Windows/MessageWindow";
import { GroupByPipe } from "../../../Infrastructure/Pipes/GroupByPipe";
import { AppTool } from "../../../Infrastructure/Tools";
import { Validator } from "../../../Infrastructure/Validators/Validator";

export class CustomizedARInvoiceCounterValidatingService {
    private ValidationErrorsList: Array<string> = [];
    private counterDefinitions: CounterDefinitionPM[] = [];
    private objectTableName: string = "CounterDefinition";
    constructor() {

    }
    Validate(counterDefinitions: CounterDefinitionPM[]) {

        this.ValidationErrorsList = [];
        this.counterDefinitions = counterDefinitions.filter(def => !def.InActive);

        if (this.counterDefinitions == null || this.counterDefinitions.length < 7) {
            this.ValidationErrorsList.push("Please assign a definition for each invoice type");
            return this.ValidationErrorsList;
        }

        let isValidGreaterStartNumber = this.ValidateGreaterStartNumber();

        if (!isValidGreaterStartNumber) {
            this.ShowMessageWindow("The new start number must be greater than current start number!");
            return;
        }

        let isValidUniquePrefix: boolean = true;

        if (this.counterDefinitions[0]?.UniquePerPrefix) {
            isValidUniquePrefix = this.ValidateUniquePerPrefix();
        }

        if (!isValidUniquePrefix) {
            this.ShowMessageWindow("Some Prefix values are invalid (Prefix should be unique)");
            return;
        }

        let errors: string[] = [];

        if (this.HasEmptyCounterSize()) {
            errors.push("Size field is mandatory!");
        }

        if (this.counterDefinitions[0]?.UniquePerPrefix) {
            this.ValidateIfUniquePerPrefix(counterDefinitions, errors);
        }
        else {
            this.ValidateIfNotUniquePerPrefix(counterDefinitions, errors);
        }

        this.ValidationErrorsList = errors;

        return this.ValidationErrorsList;
    }



    private ValidateIfNotUniquePerPrefix(counterDefinitions: CounterDefinitionPM[], errors: string[]) {
        let startNumberLength = (this.counterDefinitions[0]?.StartNumber) ? this.counterDefinitions[0].StartNumber.toString().length : 0;
        if (startNumberLength + AppTool.GetCounterPrefixLength(this.counterDefinitions[0]?.Prefix) + AppTool.GetCounterPrefixLength(this.counterDefinitions[0]?.Suffix) > 20) {
            errors.push("Maximum length allowed for [Prefix + StartNumber + Suffix] is 20");
        }

        counterDefinitions.forEach(item => {
            if (item.CounterSize > 20) {
                errors.push("Maximum size allowed for counter is 20");
            }
            Validator.TryValidateObject(item, this.objectTableName, errors);
        });
    }

    private ValidateIfUniquePerPrefix(counterDefinitions: CounterDefinitionPM[], errors: string[]) {
        counterDefinitions.forEach(item => {
            if (item.CounterSize > 20) {
                errors.push("Maximum size allowed for counter is 20");
            }
            Validator.TryValidateObject(item, this.objectTableName, errors);

            if (item.UniquePerPrefix && !AppTool.IsNullOrEmpty(item.Prefix) && !AppTool.IsNullOrEmpty(item.StartNumber)) {
                this.ValidateFormatLength(item, errors);
            }
        });
    }

    private ValidateFormatLength(item: CounterDefinitionPM, errors: string[]) {
        if ((item.StartNumber).toString().length + AppTool.GetCounterPrefixLength(item.Prefix) + AppTool.GetCounterPrefixLength(item.Suffix) <= 20) return;
        errors.push("Maximum length allowed for [Prefix + StartNumber + Suffix] is 20");
    }

    private ValidateGreaterStartNumber() {
        this.counterDefinitions.forEach(item => {
            if (AppTool.IsNullOrEmpty(item.StartNumber))
                return;
            if (item.StartNumber < item.StartNumber_Old) {
                return false;
            }
        });
        return true;
    }

    private ValidateUniquePerPrefix() {
        let myPipe = new GroupByPipe();
        let groupedByParameter1Count = myPipe.transform(this.counterDefinitions, "Parameter1").length;
        let counterdefinitionsWithoutNullPrefix = this.counterDefinitions.filter(def => !AppTool.IsNullOrEmpty(def.Prefix));
        let groupbyPrefixCount: number = myPipe.transform(counterdefinitionsWithoutNullPrefix, "Prefix").length;
        if (groupedByParameter1Count != groupbyPrefixCount) {
            return false;
        }
        return true;
    }

    private HasEmptyCounterSize() {
        return this.counterDefinitions.some(item => AppTool.IsNullOrEmpty(item.CounterSize) && !item.InActive);
    }

    private ShowMessageWindow(message: string) {
        let messageWindow = new MessageWindow();
        messageWindow.Show(message);
    }
}
