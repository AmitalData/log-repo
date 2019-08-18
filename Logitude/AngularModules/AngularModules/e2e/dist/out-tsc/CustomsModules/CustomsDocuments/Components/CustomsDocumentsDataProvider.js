"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var DeclarationCustomsDocumentsController_1 = require("../../../CustomsModules/CustomsDeclarationModules/DeclarationOthers/Components/Documents/DeclarationCustomsDocumentsController");
var CollateralCustomsDocumentsController_1 = require("../../../CustomsModules/CustomsCollateral/Components/Documents/CollateralCustomsDocumentsController");
var ClaimCustomsDocumentsController_1 = require("../../../CustomsModules/CustomsClaim/Components/Documents/ClaimCustomsDocumentsController");
var CustDocRelatedDocsWebService_1 = require("../../../Customs/Services/WebServices/CustDocRelatedDocsWebService");
var CustomsDocumentsDataProvider = /** @class */ (function () {
    function CustomsDocumentsDataProvider(objectTableName, entityPM, childEntity1Id, childEntity1Name) {
        if (childEntity1Id === void 0) { childEntity1Id = null; }
        if (childEntity1Name === void 0) { childEntity1Name = null; }
        var _this = this;
        this.objectTableName = objectTableName;
        this.entityPM = entityPM;
        this.childEntity1Id = childEntity1Id;
        this.childEntity1Name = childEntity1Name;
        this.custDocRelatedDocsWebService = new CustDocRelatedDocsWebService_1.CustDocRelatedDocsWebService();
        var objectTable = window.ObjectTables.filter(function (d) { return d.Name === _this.objectTableName; })[0];
        this.ObjectTableId = objectTable.Id;
        switch (this.objectTableName) {
            case 'Customs.Declaration': {
                this.declarationCustomsDocumentsController = new DeclarationCustomsDocumentsController_1.DeclarationCustomsDocumentsController(entityPM, childEntity1Id, childEntity1Name);
                break;
            }
            case 'Customs.CustomsCollateral': {
                this.collateralCustomsDocumentsController = new CollateralCustomsDocumentsController_1.CollateralCustomsDocumentsController(entityPM, childEntity1Id, childEntity1Name);
                break;
            }
            case 'Customs.Claim': {
                this.claimCustomsDocumentsController = new ClaimCustomsDocumentsController_1.ClaimCustomsDocumentsController(entityPM, childEntity1Id, childEntity1Name);
                break;
            }
        }
    }
    CustomsDocumentsDataProvider.prototype.GetCustomsDocumentsController = function () {
        switch (this.objectTableName) {
            case 'Customs.Declaration': {
                return this.declarationCustomsDocumentsController;
            }
            case 'Customs.CustomsCollateral': {
                return this.collateralCustomsDocumentsController;
            }
            case 'Customs.Claim': {
                return this.claimCustomsDocumentsController;
            }
        }
    };
    CustomsDocumentsDataProvider.prototype.GetCustomsDocumentsRelatedDocuments = function (filterValue) {
        switch (this.objectTableName) {
            case 'Customs.Declaration': {
                return this.custDocRelatedDocsWebService.GetDocumentsFilingsForRelatedDocuments(this.entityPM.Id, null, this.ObjectTableId, 'I', this.entityPM.CustomFileNo, filterValue);
            }
            case 'Customs.Claim': {
                return this.custDocRelatedDocsWebService.GetDocumentsFilingsForRelatedDocuments(this.entityPM.Id, null, this.ObjectTableId, 'I', null, null);
            }
        }
    };
    return CustomsDocumentsDataProvider;
}());
exports.CustomsDocumentsDataProvider = CustomsDocumentsDataProvider;
//# sourceMappingURL=CustomsDocumentsDataProvider.js.map