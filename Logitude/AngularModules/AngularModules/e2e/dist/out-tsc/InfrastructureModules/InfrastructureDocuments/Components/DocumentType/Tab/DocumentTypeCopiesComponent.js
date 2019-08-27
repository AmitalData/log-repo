"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = function (d, b) {
        extendStatics = Object.setPrototypeOf ||
            ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
            function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
        return extendStatics(d, b);
    }
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var __decorate = (this && this.__decorate) || function (decorators, target, key, desc) {
    var c = arguments.length, r = c < 3 ? target : desc === null ? desc = Object.getOwnPropertyDescriptor(target, key) : desc, d;
    if (typeof Reflect === "object" && typeof Reflect.decorate === "function") r = Reflect.decorate(decorators, target, key, desc);
    else for (var i = decorators.length - 1; i >= 0; i--) if (d = decorators[i]) r = (c < 3 ? d(r) : c > 3 ? d(target, key, r) : d(target, key)) || r;
    return c > 3 && r && Object.defineProperty(target, key, r), r;
};
var __metadata = (this && this.__metadata) || function (k, v) {
    if (typeof Reflect === "object" && typeof Reflect.metadata === "function") return Reflect.metadata(k, v);
};
Object.defineProperty(exports, "__esModule", { value: true });
var BaseComponent_1 = require("../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var core_1 = require("@angular/core");
var EntityArgs_1 = require("../../../../../Infrastructure/DataContracts/EntityArgs");
var DocumentCopiesViewModel_1 = require("../../DocumentComponent/DocsOut/ViewModel/DocumentCopiesViewModel");
var EntityResourceService_1 = require("../../../../../Infrastructure/Services/EntityResourceService");
var DocumentTypeCopiesComponent = /** @class */ (function (_super) {
    __extends(DocumentTypeCopiesComponent, _super);
    function DocumentTypeCopiesComponent(entityArgs) {
        var _this = _super.call(this) || this;
        _this.entityArgs = entityArgs;
        _this.IsVisibile = false;
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        return _this;
    }
    DocumentTypeCopiesComponent.prototype.ngOnInit = function () {
        var _this = this;
        this._entityResourceService.getEntityResourceByTableName("DocumentTypeCopy", 0).subscribe(function (response) {
            _this.IsVisibile = true;
            _this.EntityPM = _this.entityArgs.EntityPM;
            if (_this.EntityPM) {
                _this.Run();
            }
        });
    };
    DocumentTypeCopiesComponent.prototype.Run = function () {
        var _this = this;
        if (this.EntityPM.DocumentTypeCopies) {
            this.DocumentTypeCopiesLists = [];
            this.EntityPM.DocumentTypeCopies.forEach(function (copy) {
                _this.DocumentTypeCopiesLists.push(new DocumentCopiesViewModel_1.DocumentCopiesViewModel(copy, null, null, null, null, null, null, null));
            });
        }
    };
    DocumentTypeCopiesComponent.prototype.CheckboxIsSelectedByDefaultClick = function (selectedItem, value) {
        if (selectedItem != null) {
            selectedItem.IsSelectedByDefault = value;
            if (selectedItem.CurrentDocumentTypeCopy) {
                selectedItem.CurrentDocumentTypeCopy.IsSelectedByDefault = value;
            }
        }
        //selectedItem.IsSelectedByDefault = this.EntityPM.DocumentTypeCopies.filter(d=> d.Id == selectedItem.Id)[0].IsSelectedByDefault = !selectedItem.IsSelectedByDefault;
    };
    DocumentTypeCopiesComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            selector: 'DocumentTypeCopiesTab',
            templateUrl: './DocumentTypeCopiesComponent.html',
        }),
        __metadata("design:paramtypes", [EntityArgs_1.EntityArgs])
    ], DocumentTypeCopiesComponent);
    return DocumentTypeCopiesComponent;
}(BaseComponent_1.BaseComponent));
exports.DocumentTypeCopiesComponent = DocumentTypeCopiesComponent;
//# sourceMappingURL=DocumentTypeCopiesComponent.js.map