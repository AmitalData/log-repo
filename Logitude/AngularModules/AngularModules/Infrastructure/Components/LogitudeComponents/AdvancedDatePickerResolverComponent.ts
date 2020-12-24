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
                    dateValue = dateOption.Name;
                    break;
                case 'BLQ':
                    dateValue = dateOption.Name;
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
                    dateValue = dateOption.Name;
                    break;
                case 'ELQ':
                    dateValue = dateOption.Name;
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
}
