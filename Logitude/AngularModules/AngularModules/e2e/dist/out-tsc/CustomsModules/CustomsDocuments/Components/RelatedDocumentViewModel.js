"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var CustomDocumentTypeMetaDataListService_1 = require("../../../Customs/Services/StandardLists/CustomDocumentTypeMetaDataListService");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var RelatedDocumentViewModel = /** @class */ (function () {
    //******************************************//
    function RelatedDocumentViewModel(documentsFilingPM, customsDocumentMetaDataValuePMs, isDisplayOnly) {
        this.documentsFilingPM = documentsFilingPM;
        this.customsDocumentMetaDataValuePMs = customsDocumentMetaDataValuePMs;
        this.isDisplayOnly = isDisplayOnly;
        //**************Properties******************//
        this.IsConnected = false;
        this.EntityResourceService = new EntityResourceService_1.EntityResourceService();
        this.SetStatusImages();
        this.SetCustomDocumentMetaData();
        //this.DocumentTypeName = documentsFilingPM.CustomsDocumentTypeName;
    }
    Object.defineProperty(RelatedDocumentViewModel.prototype, "Id", {
        get: function () { return this.documentsFilingPM.Id; },
        set: function (value) {
            if (this.documentsFilingPM.Id != value) {
                this.documentsFilingPM.Id = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RelatedDocumentViewModel.prototype, "Name", {
        get: function () { return this.documentsFilingPM.Name; },
        set: function (value) {
            if (this.documentsFilingPM.Name != value) {
                this.documentsFilingPM.Name = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RelatedDocumentViewModel.prototype, "ExternalAttachmentId", {
        get: function () { return this.documentsFilingPM.ExternalAttachmentId; },
        set: function (value) {
            if (this.documentsFilingPM.ExternalAttachmentId != value) {
                this.documentsFilingPM.ExternalAttachmentId = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RelatedDocumentViewModel.prototype, "ExternalAttachmentIdVisibility", {
        get: function () {
            if (this.ExternalAttachmentId) {
                return true;
            }
            else {
                return false;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RelatedDocumentViewModel.prototype, "Extension", {
        get: function () { return this.documentsFilingPM.FileExtension; },
        set: function (value) {
            if (this.documentsFilingPM.FileExtension != value) {
                this.documentsFilingPM.FileExtension = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RelatedDocumentViewModel.prototype, "Code", {
        get: function () { return this.documentsFilingPM.Code; },
        set: function (value) {
            if (this.documentsFilingPM.Code != value) {
                this.documentsFilingPM.Code = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RelatedDocumentViewModel.prototype, "CodeVisibility", {
        get: function () {
            if (this.ExternalAttachmentId) {
                return false;
            }
            else {
                return true;
            }
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(RelatedDocumentViewModel.prototype, "FileSize", {
        get: function () { return this.documentsFilingPM.FileSize; },
        set: function (value) {
            if (this.documentsFilingPM.FileSize != value) {
                this.documentsFilingPM.FileSize = value;
            }
        },
        enumerable: true,
        configurable: true
    });
    RelatedDocumentViewModel.prototype.SetStatusImages = function () {
        if (this.documentsFilingPM.CustomsDocumentStatusCode == "1" || this.documentsFilingPM.CustomsDocumentStatusCode == "2" || this.documentsFilingPM.CustomsDocumentStatusCode == "7") {
            this.Status1ImageGreen = true;
            this.Status1ImageGray = false;
        }
        else {
            this.Status1ImageGreen = false;
            this.Status1ImageGray = true;
        }
        if (this.documentsFilingPM.CustomsDocumentStatusCode == "2") {
            this.Status2ImageGreen = false;
            this.Status2ImageGray = false;
            this.Status2ErrorImage = true;
        }
        else if (this.documentsFilingPM.CustomsDocumentStatusCode == "1") {
            this.Status2ImageGreen = true;
            this.Status2ImageGray = false;
            this.Status2ErrorImage = false;
        }
        else {
            this.Status2ImageGreen = false;
            this.Status2ImageGray = true;
            this.Status2ErrorImage = false;
        }
    };
    RelatedDocumentViewModel.prototype.SetCustomDocumentMetaData = function () {
        var _this = this;
        var customDocumentTypeMetaDataListService = new CustomDocumentTypeMetaDataListService_1.CustomDocumentTypeMetaDataListService();
        customDocumentTypeMetaDataListService.getAllFromCache().subscribe(function (res) {
            _this.customDocumentTypeMetaDataLists = res.Result;
            _this.EntityResourceService.getEntityResourceByTableName("Customs.CustomDocumentTypeMetaData").subscribe(function (response) {
                _this.customDocumentTypeMetaDataLists = _this.customDocumentTypeMetaDataLists.filter(function (d) { return d.DocumentTypeCode === _this.documentsFilingPM.CustomsDocumentTypeCode; });
                _this.DocumentTypeName = _this.documentsFilingPM.Description;
                if (_this.customsDocumentMetaDataValuePMs && _this.customDocumentTypeMetaDataLists) {
                    var leading = _this.customDocumentTypeMetaDataLists.filter(function (d) { return d.IsLeading && d.DocumentTypeCode == _this.documentsFilingPM.CustomsDocumentTypeCode; })[0];
                    if (leading) {
                        var leadingValue = _this.customsDocumentMetaDataValuePMs.filter(function (d) { return d.MetaDataTypeCode == leading.MetaDataTypeCode; })[0];
                        _this.LeadingMetaDataValue = leadingValue != null ? (leadingValue.MetaDataValue == "True" ? "כן" : (leadingValue.MetaDataValue == "False" ? "לא" : leadingValue.MetaDataValue)) : null;
                        _this.LeadingMetaDataName = leading.MetaDataTypeName;
                        if (_this.LeadingMetaDataValue) {
                            _this.DocumentTypeName = _this.documentsFilingPM.CustomsDocumentTypeName;
                        }
                        else {
                            _this.DocumentTypeName = _this.documentsFilingPM.Description;
                        }
                    }
                }
            });
        });
    };
    RelatedDocumentViewModel.prototype.ConnectDocumentToTicket = function (params) {
        console.log(params);
    };
    RelatedDocumentViewModel.prototype.OnDragStart = function (event) {
        //if (this.isDisplayOnly) {
        //    event.preventDefault();
        //}
        //else {
        event.dataTransfer.setData("Id", this.documentsFilingPM.Id); //.setData("text", event.target.id);
        //}
    };
    return RelatedDocumentViewModel;
}());
exports.RelatedDocumentViewModel = RelatedDocumentViewModel;
//# sourceMappingURL=RelatedDocumentViewModel.js.map