"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var ComponentArgs = /** @class */ (function () {
    function ComponentArgs() {
    }
    ComponentArgs.AddComponent = function (component) {
        if (ComponentArgs.ComponentLists == null) {
            ComponentArgs.ComponentLists = new Array();
        }
        var item = ComponentArgs.ComponentLists.filter(function (d) { return d.key == component.key; })[0];
        if (!item) {
            ComponentArgs.ComponentLists.push(component);
        }
        else {
            item.Component = component.Component;
        }
    };
    return ComponentArgs;
}());
exports.ComponentArgs = ComponentArgs;
//# sourceMappingURL=ComponentArgs.js.map