export class AdvancedDatePickerResolverComponent {
    constructor() {
    }

    public SetValidityBetweenTwoDateOptions(firstOption: any, secondOption: any) {
        if (firstOption != null && secondOption != null) {
            var firstDateValue = this.ResolveDateValue(firstOption, true);
            var secondDateValue = this.ResolveDateValue(secondOption, false);
            if (firstDateValue > secondDateValue) return false;
            else return true;
        }
        return false;
    }
    public ResolveDateValue(dateOption: any, fromDate: boolean) {
        var date = new Date(); 
        var dateValue = new Date();
        var quarterNumber = this.GetQuarterNumber(date);
        if (dateOption != null) {
            switch (dateOption) {
                case 'TOD':
                    dateValue = date;
                    break;
                case 'YES':
                    dateValue = new Date(date.getFullYear(), date.getMonth(), date.getDate() - 1)
                    break;
                case 'BTM':
                    dateValue = new Date(date.getFullYear(), date.getMonth(), 1);
                    break;
                case 'BLM':
                    dateValue = new Date(date.getFullYear(), date.getMonth() - 1, 1);
                    break;
                case 'BTQ':
                    dateValue = new Date(date.getFullYear(), (quarterNumber - 1) * 3, 1);
                    break;
                case 'BLQ':
                    dateValue = this.GetPreviousQuarterFirstDate();
                    break;
                case 'BTY':
                    dateValue = new Date(date.getFullYear(), 0);
                    break;
                case 'BLY':
                    dateValue = new Date(date.getFullYear() - 1, 0);
                    break;
                case 'ETM':
                    dateValue = new Date(date.getFullYear(), date.getMonth() + 1, 0); 
                    break;
                case 'ELM':
                    dateValue = new Date(date.getFullYear(), date.getMonth(), 0);
                    break;
                case 'ETQ':
                    dateValue = new Date(date.getFullYear(), (quarterNumber - 1) * 3 + 3, 0);
                    break;
                case 'ELQ':
                    dateValue = this.GetPreviousQuarterLastDate();
                    break;
                case 'ETY':
                    dateValue = new Date(date.getFullYear() + 1, 0, 0);
                    break;
                case 'ELY':
                    dateValue = new Date(date.getFullYear(), 0, 0);
                    break;
                default:
                    try {
                        if (typeof dateOption === "string" && dateOption?.startsWith("PER_")) {
                            const parts = dateOption.split("_");
                            const value = parseInt(parts[1], 10);
                            const unit = parts[2].toLowerCase(); // days / months / years
                            const sign = fromDate ? -1 : 1;

                            switch (unit) {
                                case "days":
                                    dateValue.setDate(date.getDate() + sign * value);
                                    break;
                                case "months":
                                    dateValue.setMonth(date.getMonth() + sign * value);
                                    break;
                                case "years":
                                    dateValue.setFullYear(date.getFullYear() + sign * value);
                                    break;
                                default:
                                    throw new Error("Unsupported period unit: " + unit);
                            }
                        }
                        else {
                            dateValue = new Date(dateOption);
                        }
                    } catch(error) {
                        console.error("Unexpected error", error);
                        throw new Error("Unexpected error when validating");
                    }
                    break;
            }
        }
        return dateValue;
    }

    public GetPreviousQuarterFirstDate() {
        var date = new Date(); 
        var firstDayOfQuarter: Date;
        var quarterNumber = this.GetQuarterNumber(date);
        if (quarterNumber == 1) {
            quarterNumber = 4;
            firstDayOfQuarter = new Date(date.getFullYear() - 1, (quarterNumber - 1) * 3, 1);
        }
        else {
            quarterNumber--;
            firstDayOfQuarter = new Date(date.getFullYear(), (quarterNumber - 1) * 3, 1);
        }
        return firstDayOfQuarter;
    }

    public GetPreviousQuarterLastDate() {
        var date = new Date();
        var lastDayOfQuarter: Date;
        var quarterNumber = this.GetQuarterNumber(date);
        if (quarterNumber == 1) {
            quarterNumber = 4;
            lastDayOfQuarter = new Date(date.getFullYear() - 1, (quarterNumber) * 3, 0);
        }
        else {
            quarterNumber--;
            lastDayOfQuarter = new Date(date.getFullYear(), (quarterNumber) * 3, 0);
        }
        return lastDayOfQuarter;
    }

    private GetQuarterNumber(date: Date) {
        var quarterNumber = (date.getMonth() - 1) / 3 + 1;
        quarterNumber = Math.trunc(quarterNumber);
        quarterNumber = quarterNumber == 0 ? 1 : quarterNumber;
        return quarterNumber;
    }
}
