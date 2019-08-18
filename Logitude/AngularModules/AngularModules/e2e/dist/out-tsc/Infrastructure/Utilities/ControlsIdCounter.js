"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ControlsIdCounter = /** @class */ (function () {
    function ControlsIdCounter() {
    }
    ControlsIdCounter.GetNextIdCounter = function () {
        this.ControlsId = this.ControlsId + 1;
        return this.ControlsId;
    };
    ControlsIdCounter.GetNextControlIdCounter = function (keyName) {
        var counter = this.CountersList.filter(function (d) { return d.KeyName == keyName; })[0];
        if (counter == null || counter == undefined) {
            counter = new CounterRecord(keyName, 0);
            this.CountersList.push(counter);
        }
        counter.Counter = counter.Counter + 1;
        return counter.Counter;
        //this.LOVCounterId = this.LOVCounterId + 1;
        //return this.LOVCounterId;
    };
    ControlsIdCounter.ControlsId = 0;
    ControlsIdCounter.LOVCounterId = 0;
    ControlsIdCounter.TextBoxCounterId = 0;
    ControlsIdCounter.CheckBoxCounterId = 0;
    ControlsIdCounter.DatePickerCounterId = 0;
    ControlsIdCounter.CountersList = new Array();
    return ControlsIdCounter;
}());
exports.ControlsIdCounter = ControlsIdCounter;
var CounterRecord = /** @class */ (function () {
    function CounterRecord(KeyName, Counter) {
        this.KeyName = KeyName;
        this.Counter = Counter;
    }
    return CounterRecord;
}());
//# sourceMappingURL=ControlsIdCounter.js.map