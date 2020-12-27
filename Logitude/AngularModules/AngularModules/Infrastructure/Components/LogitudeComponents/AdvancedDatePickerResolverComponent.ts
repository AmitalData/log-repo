export class AdvancedDatePickerResolverComponent {
    constructor() {
    }

    public SetValidityBetweenTwoDateOptions(firstOption: any, secondOption: any) {
        if (firstOption != null && secondOption != null) {
            var firstDateValue = this.ResolveDateValue(firstOption);
            var secondDateValue = this.ResolveDateValue(secondOption);
            if (firstDateValue > secondDateValue) return false;
            else return true;
        }
        return false;
    }
    public ResolveDateValue(dateOption: any) {
        var date = new Date(); 
        var dateValue = new Date();
        var quarterNumber = (date.getMonth() - 1) / 3 + 1;
        quarterNumber = Math.trunc(quarterNumber);
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
                    dateValue = new Date(date.getFullYear(), 1);
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
                    dateValue = new Date(dateOption);
                    break;
            }
        }
        return dateValue;
    }

    public GetPreviousQuarterFirstDate() {
        var date = new Date(); 
        var firstDayOfQuarter: Date;
        var quarterNumber = (date.getMonth() - 1) / 3 + 1;
        quarterNumber = Math.trunc(quarterNumber);
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
        var quarterNumber = (date.getMonth() - 1) / 3 + 1;
        quarterNumber = Math.trunc(quarterNumber);
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
}
