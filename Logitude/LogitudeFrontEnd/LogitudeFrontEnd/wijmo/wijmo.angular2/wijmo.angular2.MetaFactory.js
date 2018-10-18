var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
var wj;
(function (wj) {
    var angular2;
    (function (angular2) {
        'use strict';
        var MetaFactory = (function (_super) {
            __extends(MetaFactory, _super);
            function MetaFactory() {
                _super.apply(this, arguments);
            }
            // Override to return wijmo.knockout.PropDesc
            MetaFactory.CreateProp = function (propertyName, propertyType, changeEvent, enumType, isNativeControlProperty, priority) {
                return new PropDesc(propertyName, propertyType, changeEvent, enumType, isNativeControlProperty, priority);
            };
            // Override to return wijmo.knockout.EventDesc
            MetaFactory.CreateEvent = function (eventName, isPropChanged) {
                return new EventDesc(eventName, isPropChanged);
            };
            // Override to return wijmo.knockout.ComplexPropDesc
            MetaFactory.CreateComplexProp = function (propertyName, isArray, ownsObject) {
                return new ComplexPropDesc(propertyName, isArray, ownsObject);
            };
            // Typecasted override.
            MetaFactory.findProp = function (propName, props) {
                return wijmo.interop.ControlMetaFactory.findProp(propName, props);
            };
            // Typecasted override.
            MetaFactory.findEvent = function (eventName, events) {
                return wijmo.interop.ControlMetaFactory.findEvent(eventName, events);
            };
            // Typecasted override.
            MetaFactory.findComplexProp = function (propName, props) {
                return wijmo.interop.ControlMetaFactory.findComplexProp(propName, props);
            };
            return MetaFactory;
        })(wijmo.interop.ControlMetaFactory);
        angular2.MetaFactory = MetaFactory;
        var PropDesc = (function (_super) {
            __extends(PropDesc, _super);
            function PropDesc() {
                _super.apply(this, arguments);
            }
            return PropDesc;
        })(wijmo.interop.PropDescBase);
        angular2.PropDesc = PropDesc;
        // Describes a scope event
        var EventDesc = (function (_super) {
            __extends(EventDesc, _super);
            function EventDesc() {
                _super.apply(this, arguments);
            }
            return EventDesc;
        })(wijmo.interop.EventDescBase);
        angular2.EventDesc = EventDesc;
        // Describe property info for nested directives.
        var ComplexPropDesc = (function (_super) {
            __extends(ComplexPropDesc, _super);
            function ComplexPropDesc() {
                _super.apply(this, arguments);
            }
            return ComplexPropDesc;
        })(wijmo.interop.ComplexPropDescBase);
        angular2.ComplexPropDesc = ComplexPropDesc;
    })(angular2 = wj.angular2 || (wj.angular2 = {}));
})(wj = exports.wj || (exports.wj = {}));
exports.wjNg2Meta = wj.angular2;
//# sourceMappingURL=wijmo.angular2.MetaFactory.js.map