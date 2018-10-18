
export class ControlsIdCounter {
    private static ControlsId: number = 0;
    private static LOVCounterId: number = 0;
    private static TextBoxCounterId: number = 0;
    private static CheckBoxCounterId: number = 0;
    private static DatePickerCounterId: number = 0;
    private static CountersList: CounterRecord[] = new Array<CounterRecord>();
    public static GetNextIdCounter() {
        this.ControlsId = this.ControlsId + 1;
        return this.ControlsId;
    }

    public static GetNextControlIdCounter(keyName: string) {
        var counter = this.CountersList.filter(d=> d.KeyName == keyName)[0];
        if (counter == null || counter == undefined) {
            counter = new CounterRecord(keyName, 0);
            this.CountersList.push(counter);
        }
        counter.Counter = counter.Counter + 1;
        return counter.Counter;
        //this.LOVCounterId = this.LOVCounterId + 1;
        //return this.LOVCounterId;
    }

    
}

class CounterRecord {
    constructor(public KeyName: string, public Counter: number) {

    }
}