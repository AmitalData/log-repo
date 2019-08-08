"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var Cloner = /** @class */ (function () {
    function Cloner(object) {
        this.Object = null;
        this.Fields = [];
        this.Entities = [];
        this.Object = object;
    }
    Cloner.prototype.AddField = function (fieldName) {
        if (fieldName != null) {
            if (this.Object != null) {
                var existsField = this.Fields.filter(function (f) { return f.FieldName == fieldName; })[0];
                if (existsField) {
                    existsField.FieldValue = this.Object[fieldName];
                }
                else {
                    this.Fields.push(new ClonerField(fieldName, this.Object[fieldName]));
                }
            }
        }
    };
    Cloner.prototype.AddEntity = function (entity) {
        if (entity != null) {
            if (this.Object != null) {
                this.Entities.push(new ClonerEntity(entity));
            }
        }
    };
    Cloner.prototype.RejectChanges = function () {
        var _this = this;
        if (this.Object != null) {
            this.Fields.forEach(function (item) {
                var currentValue = _this.Object[item.FieldName];
                if (currentValue != item.FieldValue) {
                    _this.Object[item.FieldName] = item.FieldValue;
                }
            });
            this.Entities.forEach(function (item) {
                if (item != null) {
                    var currentValue = item.Entity['IsDirty'];
                    if (currentValue != item.IsDirty) {
                        item.Entity['IsDirty'] = item.IsDirty;
                    }
                }
            });
        }
    };
    return Cloner;
}());
exports.Cloner = Cloner;
var ClonerField = /** @class */ (function () {
    function ClonerField(name, value) {
        this.FieldName = name;
        this.FieldValue = value;
    }
    return ClonerField;
}());
var ClonerEntity = /** @class */ (function () {
    function ClonerEntity(entity) {
        this.Entity = null;
        this.IsDirty = false;
        this.Entity = entity;
        this.IsDirty = entity['IsDirty'];
    }
    return ClonerEntity;
}());
//# sourceMappingURL=Cloner.js.map