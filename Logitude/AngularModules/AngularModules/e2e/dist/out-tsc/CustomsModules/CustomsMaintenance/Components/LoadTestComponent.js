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
var core_1 = require("@angular/core");
var BaseComponent_1 = require("../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var EntityResourceService_1 = require("../../../Infrastructure/Services/EntityResourceService");
var Tools_1 = require("../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../Infrastructure/Utilities/SessionLocator");
var DeclarationListService_1 = require("../../../Customs/Services/StandardLists/DeclarationListService");
var DeclarationWebService_1 = require("../../../Customs/Services/WebServices/DeclarationWebService");
var DeclarationPMService_1 = require("../../../Customs/Services/StandardPMs/DeclarationPMService");
var LoadTestService_1 = require("../../../Customs/Services/WebServices/LoadTestService");
var DeclarationExtendedListService_1 = require("../../../Customs/Services/ExtendedLists/DeclarationExtendedListService");
var CustomsDocumentMetaDataValuePM_1 = require("../../../Customs/EntityPMs/CustomsDocumentMetaDataValuePM");
var CustDocRelatedDocsWebService_1 = require("../../../Customs/Services/WebServices/CustDocRelatedDocsWebService");
var GenericRequestParams_1 = require("../../../Customs/DataContract/RequestParams/GenericRequestParams");
var PrintRequestRequestParams_1 = require("../../../Customs/DataContract/RequestParams/PrintRequestRequestParams");
var DeclarationMessagesService_1 = require("../../../Customs/Services/WebServices/DeclarationMessagesService");
//import { PrintRequestRequestParams } from '../../../Customs/DataContract/RequestParams/PrintRequestRequestParams';
//import { PrintRequestResponseData, PrintRequestResultList } from '../../../DataContract/ResponseData/PrintRequestResponseData';
var CustomsDocumentPMService_1 = require("../../../Customs/Services/StandardPMs/CustomsDocumentPMService");
var LoadTestComponent = /** @class */ (function (_super) {
    __extends(LoadTestComponent, _super);
    function LoadTestComponent() {
        var _this = _super.call(this) || this;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this.columns = null;
        _this._DeclarationPMService = new DeclarationPMService_1.DeclarationPMService();
        _this._DeclarationListService = new DeclarationListService_1.DeclarationListService();
        _this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        _this._LoadTestService = new LoadTestService_1.LoadTestService();
        _this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        _this._custDocRelatedDocsWebService = new CustDocRelatedDocsWebService_1.CustDocRelatedDocsWebService();
        ///public ComponentRef: ComponentRef<LoadTestComponent>;
        //entityPM: CustomsSettingPM;
        _this.ValidationErrorsList = [];
        _this._GetNewCustomFileList = [];
        _this._OpenDecFiligList = [];
        _this._SendDecList = [];
        _this._SaveDecList = [];
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.Loaded = false;
        ///#region Properties
        //IsUnifreightCertificateActivatedEnabled: boolean = true;
        _this._CustomerId = "10009065";
        _this._Consignee = "1000";
        _this._CopyFromDecId = "1-104502";
        _this._CopyFromDecId1 = "1-104502";
        _this._CopyFromDecId2 = "1-104502";
        //#endregion
        _this._COM_ID = "hdefwqjlpk6z_6cwtkessa00000000"; ///"pkajungqyegfkg6hhqjizw00000000";
        _this._Max = 30;
        _this._current = 0;
        _this.CanStartAgain = true;
        return _this;
    }
    LoadTestComponent.prototype.ngOnInit = function () {
        this.Loaded = true;
    };
    LoadTestComponent.prototype.ValidScreen = function () {
    };
    Object.defineProperty(LoadTestComponent.prototype, "CustomerId", {
        get: function () { return this._CustomerId; },
        set: function (value) { this._CustomerId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LoadTestComponent.prototype, "Consignee", {
        get: function () { return this._Consignee; },
        set: function (value) { this._Consignee = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LoadTestComponent.prototype, "CopyFromDecId", {
        get: function () { return this._CopyFromDecId; },
        set: function (value) { this._CopyFromDecId = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LoadTestComponent.prototype, "CopyFromDecId1", {
        get: function () { return this._CopyFromDecId1; },
        set: function (value) { this._CopyFromDecId1 = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LoadTestComponent.prototype, "CopyFromDecId2", {
        get: function () { return this._CopyFromDecId2; },
        set: function (value) { this._CopyFromDecId2 = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LoadTestComponent.prototype, "COM_ID", {
        get: function () { return this._COM_ID; },
        set: function (value) { this._COM_ID = value; },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(LoadTestComponent.prototype, "Max", {
        get: function () { return this._Max; },
        set: function (value) { this._Max = value; },
        enumerable: true,
        configurable: true
    });
    LoadTestComponent.prototype.CancelButtonClicked = function () {
        this.CurrentSession.CloseCurrentWindow();
    };
    Object.defineProperty(LoadTestComponent.prototype, "LogProccess", {
        get: function () {
            return this._LogProccess;
        },
        set: function (value) { this._LogProccess = value; },
        enumerable: true,
        configurable: true
    });
    LoadTestComponent.prototype.OkButtonClicked = function () {
        this._current = 0;
        this.DoIt();
    };
    LoadTestComponent.prototype.DoIt = function () {
        var _this = this;
        this._current = this._current + 1;
        this.CanStartAgain = false;
        if (this._current < this.Max) {
            this._CustomLoadTest = new CustomLoadTest(this.Consignee, this.CustomerId, this.CopyFromDecId, this.COM_ID, this);
            this._CustomLoadTest.OnLogChange
                .subscribe(function (logIt) {
                _this.LogProccess = logIt;
            });
            this._CustomLoadTest.OnFinish
                .subscribe(function () {
                _this.DoIt();
            });
            this._CustomLoadTest.Start();
        }
        else {
            this.CanStartAgain = true;
            this.LogProccess = ("Finish !!!!!!!!!!!!!!!!!!!!!!!");
            this._current = 0;
        }
    };
    LoadTestComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './LoadTestComponent.html',
        }),
        __metadata("design:paramtypes", [])
    ], LoadTestComponent);
    return LoadTestComponent;
}(BaseComponent_1.BaseComponent));
exports.LoadTestComponent = LoadTestComponent;
var TestStartes;
(function (TestStartes) {
    TestStartes[TestStartes["start"] = 1] = "start";
    TestStartes[TestStartes["GetNewCustomFile"] = 2] = "GetNewCustomFile";
    TestStartes[TestStartes["GetDeclarationFromFileNo"] = 3] = "GetDeclarationFromFileNo";
    TestStartes[TestStartes["GetSingleDeclarationByCustomFileNo"] = 4] = "GetSingleDeclarationByCustomFileNo";
    TestStartes[TestStartes["PutCopyDeclaration"] = 5] = "PutCopyDeclaration";
    TestStartes[TestStartes["SendDeclarationT1"] = 6] = "SendDeclarationT1";
    TestStartes[TestStartes["HaveDoc"] = 7] = "HaveDoc";
})(TestStartes || (TestStartes = {}));
var CustomLoadTest = /** @class */ (function () {
    function CustomLoadTest(Consignee, CustomerId, CopyFromDecId, FilingCopy, parentLoadTestComponent) {
        this.Consignee = Consignee;
        this.CustomerId = CustomerId;
        this.CopyFromDecId = CopyFromDecId;
        this.FilingCopy = FilingCopy;
        this.OnFinish = new core_1.EventEmitter();
        this.OnLogChange = new core_1.EventEmitter();
        this._DeclarationPMService = new DeclarationPMService_1.DeclarationPMService();
        this._DeclarationListService = new DeclarationListService_1.DeclarationListService();
        this._entityResourceService = new EntityResourceService_1.EntityResourceService();
        this._LoadTestService = new LoadTestService_1.LoadTestService();
        this._DeclarationExtendedListService = new DeclarationExtendedListService_1.DeclarationExtendedListService();
        this._custDocRelatedDocsWebService = new CustDocRelatedDocsWebService_1.CustDocRelatedDocsWebService();
        this._DeclarationWebService = new DeclarationWebService_1.DeclarationWebService();
        this._DeclarationMessagesService = new DeclarationMessagesService_1.DeclarationMessagesService();
        this._CustomsDocumentPMService = new CustomsDocumentPMService_1.CustomsDocumentPMService();
        this._HaveTicket = false;
        this._LastError = "";
        this._SendDeclarationCounter = 0;
        this.Objecttable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.Declaration"; })[0];
        this._parentLoadTestComponent = parentLoadTestComponent;
    }
    CustomLoadTest.prototype.Start = function () {
        var _this = this;
        this._SendDeclarationCounter = 0;
        this.LogMe("strat");
        this._LoadTestService.GetNewCustomFile(SessionLocator_1.SessionLocator.Tenant, this.Consignee, this.CustomerId)
            .subscribe(function (rspNewCustomFile) {
            if (rspNewCustomFile == null || rspNewCustomFile.Result == null || rspNewCustomFile.Result.newFileNo == null) {
                _this._LastError = "GetNewCustomFile Failed " + Date.now().toLocaleString();
                _this.Start();
                return;
            }
            var newFileNo = rspNewCustomFile.Result.newFileNo;
            _this._FileNo = newFileNo;
            _this.LogMe("new file this._FileNo =" + _this._FileNo);
            _this._LoadTestService.GetDeclarationFromFileNo(SessionLocator_1.SessionLocator.Tenant, newFileNo, _this.FilingCopy)
                .subscribe(function (rspDeclarationFromFileNo) {
                if (rspDeclarationFromFileNo == null || rspDeclarationFromFileNo.Result == null || rspDeclarationFromFileNo.Result.returnFileNo == null) {
                    _this._LastError = "GetDeclarationFromFileNo Failed " + Date.now().toLocaleString();
                    _this.Start();
                    return;
                }
                var returnFileNo = rspDeclarationFromFileNo.Result.returnFileNo;
                _this.LogMe("Get GetDeclaration done" + _this._FileNo);
                //this._DeclarationListService.get
                _this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(returnFileNo)
                    .subscribe(function (rspDeclarationByCustomFileNo) {
                    if (rspDeclarationByCustomFileNo == null || rspDeclarationByCustomFileNo.Result == null || rspDeclarationByCustomFileNo.Result.Id == null) {
                        _this._LastError = "GetSingleDeclarationByCustomFileNo Failed " + Date.now().toLocaleString();
                        _this.Start();
                        return;
                    }
                    var declarationList = rspDeclarationByCustomFileNo.Result;
                    _this._DeclarationPMService.get(declarationList.Id)
                        .subscribe(function (rsptPMget) {
                        var declarationPM = rsptPMget.Result;
                        _this._DeclarationPM = declarationPM;
                        _this.LogMe("updating GetDeclaration " + _this._DeclarationPM.Id);
                        _this._DeclarationPM.Consignments[0].ConsignmentPackages[0].GrossMassMeasure = 321;
                        _this._DeclarationPM.Consignments[0].ConsignmentPackages[0].PackageQuantity = 321;
                        _this._DeclarationPMService.update(declarationPM)
                            .subscribe(function (rsptPMupdate) {
                            _this._DeclarationPM = rsptPMupdate.Result;
                            _this.LogMe("Start copy  GetDeclaration from " + _this.CopyFromDecId);
                            _this._DeclarationExtendedListService.PutCopyDeclaration(_this.CopyFromDecId, declarationList.Id, SessionLocator_1.SessionLocator.Tenant)
                                .subscribe(function (rsptCopyDeclaration) {
                                _this.LogMe(rsptCopyDeclaration.Result);
                                _this.HybridUpdateDocFiling();
                            });
                        });
                        //});
                    });
                });
            });
        });
    };
    CustomLoadTest.prototype.HybridUpdateDocFiling = function () {
        var _this = this;
        this.LogMe("Check HybridUpdateDocFiling ");
        this._custDocRelatedDocsWebService.GetDocumentsFilingsForRelatedDocuments(this._DeclarationPM.Id, null, this.Objecttable.Id, "I", this._DeclarationPM.CustomFileNo, "")
            .subscribe(function (rspHaveHybridDoc) {
            var documentsFilingPM = rspHaveHybridDoc.Result;
            _this._ArrayOfDocumentsFilingPM = documentsFilingPM;
            _this.SendDeclaration();
        });
    };
    CustomLoadTest.prototype.SendDocumentsFiling = function () {
        var _this = this;
        this.LogMe("SendDocumentsFiling");
        //http://localhost:9996/api/customsdocuments/getsingle?documentsfilingid=xpi82i%2Bwv0c9xhqyrpxjha00000000
        var documentsfilingid = this._ArrayOfDocumentsFilingPM[0].Id;
        this._CustomsDocumentPMService.get(encodeURIComponent(documentsfilingid))
            .subscribe(function (rsp) {
            _this._CustomsDocumentPM = rsp.Result;
            _this._CustomsDocumentPM.DeclarationId = _this._DeclarationPM.Id;
            _this._CustomsDocumentPM.IsSendToQueue = true;
            _this._CustomsDocumentPM.DocumentRemarks = "WhileAnalayzeCostomResponseSendDEC";
            var newValue = new CustomsDocumentMetaDataValuePM_1.CustomsDocumentMetaDataValuePM(_this._CustomsDocumentPM);
            newValue.MetaDataTypeCode = "87";
            newValue.CustomsDocumentId = _this._CustomsDocumentPM.DocumentsFilingId;
            newValue.MetaDataValue = "True";
            newValue.Tenant = SessionLocator_1.SessionLocator.Tenant;
            _this._CustomsDocumentPM.AddCustomsDocumentMetaDataValue(newValue);
            //var newValue: CustomsDocumentMetaDataValuePM = new CustomsDocumentMetaDataValuePM(this._CustomsDocumentPM);
            //newValue.MetaDataTypeCode = "3"
            //newValue.CustomsDocumentId = this._CustomsDocumentPM.DocumentsFilingId;
            //newValue.MetaDataValue = "321";
            //newValue.Tenant = SessionLocator.Tenant;
            //this._CustomsDocumentPM.AddCustomsDocumentMetaDataValue(newValue);
            _this._CustomsDocumentPMService.update(_this._CustomsDocumentPM)
                .subscribe(function (rspU) {
                _this.OnFinish.emit();
            });
        });
        ;
    };
    CustomLoadTest.prototype.SendDeclaration = function () {
        var _this = this;
        var startAt = new Date();
        this.LogMe("SendDeclaration");
        var searchParams = new GenericRequestParams_1.GenericRequestParams();
        searchParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
        searchParams.AppicationId = this._DeclarationPM.Id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this._DeclarationPM.Id;
        searchParams.LoggingEntityReference = this._DeclarationPM.DeclarationNumber;
        searchParams.LoggingObjectTableId = this.Objecttable.Id;
        searchParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
        searchParams.RequestName = "Declaration Request";
        searchParams.ResponseName = "Declaration Response";
        //searchParams.RequestVIA = this.RequestVIA;
        //searchParams.ForcePersonalSign = this.ForcePersonalSign;
        if (false) {
            //CustomMessageProgressComponent
            //    .ShowProgressBar(searchParams.PBId,
            //    "שליחת הצהרת יבוא", false)
            //    .then((res) => {
            //        this.ResponseData = res;
            //        this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            //    }
            //    ).catch((err) => {
            //        this.CurrentSession.CurrentEditComponent.StopBusyIndicator();
            //        this.ValidationErrors.push(err);
            //        this.FillValidationErrors("Errors");
            //    });
        }
        this._DeclarationWebService.PostSendDeclaration(searchParams)
            .subscribe(function (response) {
            _this._SendDeclarationCounter = _this._SendDeclarationCounter + 1;
            var endAt = new Date();
            var t = endAt.getTime() - startAt.getTime();
            t = t / 1000;
            var myDuration = Number(t.toPrecision(2));
            ;
            _this._parentLoadTestComponent._SendDecList.push(myDuration);
            //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            if (_this._ArrayOfDocumentsFilingPM.length > 0) {
                if (_this._SendDeclarationCounter > 1) {
                    if (!_this._HaveTicket) {
                        _this.ConnectTicket();
                    }
                    else {
                        //if (this._HaveTicket)
                        _this.SendPrintRequest();
                    }
                }
                else {
                    _this.LogMe("Update&SendDeclaration till 4");
                    _this._DeclarationPMService.get(_this._DeclarationPM.Id)
                        .subscribe(function (rsptPMget) {
                        var declarationPM = rsptPMget.Result;
                        _this._DeclarationPM = declarationPM;
                        _this._DeclarationPM.Consignments[0].CargoDescription = _this._DeclarationPM.Consignments[0].CargoDescription + _this._SendDeclarationCounter.toString();
                        _this._DeclarationPMService.update(_this._DeclarationPM)
                            .subscribe(function (rsptPMupdate) {
                            _this._DeclarationPM = rsptPMupdate.Result;
                            _this.SendDeclaration();
                        });
                    });
                }
            }
            else {
                _this.HybridUpdateDocFiling();
            }
        });
    };
    CustomLoadTest.prototype.ConnectTicket = function () {
        var _this = this;
        this.LogMe("ConnectTicket");
        this._LoadTestService.GetTicket(SessionLocator_1.SessionLocator.Tenant, this._DeclarationPM.Id)
            .subscribe(function (response) {
            //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            if (response.Result == "Ok") {
                _this._HaveTicket = true;
            }
            else {
                _this.LogMe(response.Result);
            }
            _this.SendPrintRequest();
        });
    };
    CustomLoadTest.prototype.SendPrintRequest = function () {
        var _this = this;
        this.LogMe("SendPrintRequest");
        this._DeclarationPMService.get(this._DeclarationPM.Id)
            .subscribe(function (rsptPMget) {
            var declarationPM = rsptPMget.Result;
            _this._DeclarationPM = declarationPM;
            var currRequestParams = new PrintRequestRequestParams_1.PrintRequestRequestParams();
            currRequestParams.LoggingEnabled = true;
            currRequestParams.LoggingUserId = SessionLocator_1.SessionLocator.LoggedUserId;
            //currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
            //currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
            currRequestParams.Tenant = SessionLocator_1.SessionLocator.Tenant;
            currRequestParams.IsSearchByDeclarationRadio = true;
            currRequestParams.IsSearchByCargoRadio = false;
            currRequestParams.DeclarationNumber = [];
            currRequestParams.DeclarationNumber.push(_this._DeclarationPM.DeclarationNumber);
            //CustomMessageProgressComponent
            //    .ShowProgressBar(currRequestParams.PBId,
            //    "שליחת שאילתא להדפסת הצהרה", true)
            //    .then((res) => {
            //        this.ResponseData = res;
            //        this.OnMassageDisplayMethod();
            //    }
            //    ).catch((err) => {
            //        this.ValidationErrorsList.push(err);
            //    });
            _this._DeclarationMessagesService.PostPrintRequestRequest(currRequestParams)
                .subscribe(function (myServiceResponse) {
                _this.LogMe("PostPrintRequestRequest");
                _this.SendDocumentsFiling();
            });
        });
    };
    CustomLoadTest.prototype.LogMe = function (logIt) {
        var sendStat = this.GetStaticArray(this._parentLoadTestComponent._SendDecList);
        var myTemplateString = "Send :  " + sendStat + " \n" + logIt + "\n";
        console.log(myTemplateString);
        this.LogText = myTemplateString;
        this.OnLogChange.emit(myTemplateString);
    };
    CustomLoadTest.prototype.GetStaticArray = function (ary) {
        var avrSend = 0;
        var maxSend = 0;
        var last = 0;
        if (!Tools_1.AppTool.IsNullOrEmpty(ary) && ary.length > 0) {
            avrSend = ary.reduce(function (sum, a) { return sum + a; }, 0) / (ary.length || 1);
            maxSend = ary.reduce(function (a, b) {
                return Math.max(a, b);
            });
            last = ary[ary.length - 1];
            var myTemplateString = " AVR:  " + avrSend + " MAX: " + maxSend + " LAST: " + last;
            return myTemplateString;
        }
        else {
            return "None";
        }
    };
    return CustomLoadTest;
}());
exports.CustomLoadTest = CustomLoadTest;
//# sourceMappingURL=LoadTestComponent.js.map