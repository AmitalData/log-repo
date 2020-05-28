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
var Tools_1 = require("../../../../Infrastructure/Tools");
var SessionLocator_1 = require("../../../../Infrastructure/Utilities/SessionLocator");
var BaseComponent_1 = require("../../../../Infrastructure/Components/LogitudeComponents/BaseComponent");
var ServiceHelper_1 = require("../../../../Infrastructure/Utilities/ServiceHelper");
var AmitalGatewayUtil_1 = require("../../../../Infrastructure/Utilities/AmitalGatewayUtil");
var RelatedDocumentViewModel_1 = require("../../../CustomsDocuments/Components/RelatedDocumentViewModel");
var CustomsSettingListService_1 = require("../../../../Customs/Services/StandardLists/CustomsSettingListService");
//import {DeclarationWebService} from '../Services/WebServices/DeclarationWebService';
var CustDocRelatedDocsWebService_1 = require("../../../../Customs/Services/WebServices/CustDocRelatedDocsWebService");
var ImageLibraryService_1 = require("../../../../Common/Services/Others/ImageLibraryService");
var CustomDocumentViewerService_1 = require("../../../../Customs/Services/WebServices/CustomDocumentViewerService");
var CustDocMetaDataValuesWebService_1 = require("../../../../Customs/Services/WebServices/CustDocMetaDataValuesWebService");
var DeclarationEventManager_1 = require("../../../../Customs/Utilities/DeclarationEventManager");
var ControlsIdCounter_1 = require("../../../../Infrastructure/Utilities/ControlsIdCounter");
var DownloadManager_1 = require("../../../../Infrastructure/Utilities/DownloadManager");
var DeclarationSplitComponent = /** @class */ (function (_super) {
    __extends(DeclarationSplitComponent, _super);
    function DeclarationSplitComponent(cd) {
        var _this = _super.call(this) || this;
        _this.cd = cd;
        _this.DataContext = _this;
        _this.ObjectTableName = "Customs.Declaration";
        _this.IsDocsPanelVisible = true;
        _this.pagesCount = 0;
        _this.IsConnectedToUniFreight = false;
        _this.IsNoDocumentSelected = true;
        //Services
        _this.custDocRelatedDocsWebService = new CustDocRelatedDocsWebService_1.CustDocRelatedDocsWebService();
        _this._ImageLibraryService = new ImageLibraryService_1.ImageLibraryService();
        _this._CustomDocumentViewerService = new CustomDocumentViewerService_1.CustomDocumentViewerService();
        _this.custDocsMetadataWebService = new CustDocMetaDataValuesWebService_1.CustDocMetaDataValuesWebService();
        _this.customsSettingListService = new CustomsSettingListService_1.CustomsSettingListService;
        _this.CurrentSession = SessionLocator_1.SessionLocator.SelectedSession;
        _this.IsMouseOverDownload = false;
        //#region split indicator
        _this.showSplitIndicator = false;
        _this.splitIndicatorText = "";
        _this.currentPageIndex = 0;
        //#endregion
        //#region Zooming
        _this.trackBarValue = 1;
        _this.TrackBarStep = 0.2;
        _this.ImgScaleValue = "scale(1)";
        //- rotate image to convas - JS Code 
        _this.img = new Image;
        _this.angles = [0 * Math.PI, 0.5 * Math.PI, Math.PI, 1.5 * Math.PI]; // store angles (0, 90, 180, 270) in an array
        _this.angleIndex = 0;
        _this.lastOffsetX = 0;
        //#endregion
        _this.DocumentFilterSelectedValue = "customs";
        var counter = ControlsIdCounter_1.ControlsIdCounter.GetNextControlIdCounter("DocumentViewerImage");
        _this.DocumentViewerImageId = "DocumentViewerImage-" + counter;
        return _this;
    }
    DeclarationSplitComponent.prototype.ngAfterViewInit = function () {
        this.startRenderingImage();
    };
    DeclarationSplitComponent.prototype.ngOnDestroy = function () {
        Tools_1.AppTool.KillEventEmitter(this.DeclarationSplitDocumentSelectionEVENT);
    };
    DeclarationSplitComponent.prototype.SetComponentArgs = function (args) {
        var _this = this;
        if (!Tools_1.AppTool.IsNullOrEmpty(args)) {
            this.DeclarationPM = args.EntityPM;
            // 1- get customs settings
            this.customsSettingListService.getSingleFromCache(this.DeclarationPM.Tenant.toString()).subscribe(function (response) {
                var list = response.Result;
                if (!Tools_1.AppTool.IsNullOrEmpty(list)) {
                    var customsSetting = list;
                    _this.IsConnectedToUniFreight = customsSetting.IsConnectedToUniFreight;
                }
                //// 2- get metadata values then 
                //this.CurrentSession.StartBusyIndicatorLoading();
                //this.custDocsMetadataWebService.GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(customsDocTickets).subscribe((response2: ServiceResponse) => {
                //    this.MetadataValues = response2.Result;
                // 3- load documents(tickets)
                _this.LoadDocuments();
                _this.CurrentSession.StopBusyIndicator();
                //    });
            });
            this.DeclarationSplitDocumentSelectionEVENT = DeclarationEventManager_1.DeclarationEventManager.DeclarationSplitDocumentSelection.subscribe(function (DocumentFilingId) {
                console.log("-->> Loading document for supplier invoice: " + DocumentFilingId);
                if (Tools_1.AppTool.IsNullOrEmpty(_this.RelatedDocuments)) {
                    //ClassifcationComponent Build B4 This Component finish Load Document !!!
                    _this._DocumentFilingIdToSetWhileLoadDocument = DocumentFilingId;
                    return;
                }
                var document = _this.RelatedDocuments.find(function (d) { return d.Id == DocumentFilingId; });
                _this.TicketItemClicked(document);
            });
        }
    };
    DeclarationSplitComponent.prototype.PageUp = function () {
        if (this.CurrentPageIndex > 0)
            this.CurrentPageIndex--;
        this.LoadDocumentPage();
    };
    DeclarationSplitComponent.prototype.PageDown = function () {
        if (this.CurrentPageIndex < this.pagesCount)
            this.CurrentPageIndex++;
        this.LoadDocumentPage();
    };
    DeclarationSplitComponent.prototype.TicketItemClicked = function (document) {
        if (this.IsMouseOverDownload)
            return;
        this.IsDocsPanelVisible = false;
        //reset counters
        this.pagesCount = 1;
        this.CurrentPageIndex = 1;
        this.SelectedTicket = document;
        this.IsNoDocumentSelected = false;
        this.angleIndex = 0;
        this.ImgScaleValue = "scale(1)";
        this.TrackBarValue = 1;
        this.LoadDocumentPage();
        //this.LoadDocumentPage(); // need to check it again, it cannot draw image at first call 
    };
    DeclarationSplitComponent.prototype.RefreshButtonClicked = function () {
        if (!Tools_1.AppTool.IsNullOrEmpty(this.CurrentPageIndex))
            this.LoadDocumentPage();
        //this.renderImage();
    };
    DeclarationSplitComponent.prototype.LoadDocumentPage = function (pageIndex) {
        var _this = this;
        if (pageIndex === void 0) { pageIndex = null; }
        if (this.SelectedTicket) {
            this.StartBusyIndicator("Loading page...");
            var index = pageIndex ? pageIndex : this.CurrentPageIndex;
            if (index == 0)
                index = 1;
            console.log("Load Page: ", index);
            //this.CurrentSession.StartBusyIndicatorLoading();
            //if (this.IsConnectedToUniFreight)
            //    var index = this.CurrentPageIndex;
            //else
            //    var index = this.CurrentPageIndex - 1;
            this._CustomDocumentViewerService.GetDocumentPage(this.SelectedTicket.documentsFilingPM.DocumentId, index - 1, this.IsConnectedToUniFreight).subscribe(function (myResponse) {
                var result = myResponse.Result;
                console.log("[Response] GetDocumentPage", result);
                if (result) {
                    //reset rotation
                    //this.ImgTransformOriginValue = "right top";
                    //this.ImgRotationValue = "rotate(0deg)";
                    //this.RotationAngle = 0;
                    _this.pagesCount = result.Count;
                    if (!Tools_1.AppTool.IsNullOrEmpty(result.Page)) {
                        //this.CurrentSession.StopBusyIndicator();
                        _this.base64Image = "data:image/png;base64," + result.Page;
                        console.log(_this.base64Image);
                        _this.img.src = _this.base64Image;
                        _this.renderImage();
                        var t = setTimeout(function () { _this.renderImage(); }, 20);
                    }
                    else {
                        _this.CurrentPageIndex = 0;
                        _this.base64Image = null;
                        _this.img.src = _this.base64Image;
                        _this.renderImage();
                        var t = setTimeout(function () { _this.renderImage(); }, 20);
                        return;
                    }
                }
                else {
                    _this.CurrentPageIndex = 0;
                    //this.CurrentSession.StopBusyIndicator();
                    _this.base64Image = null;
                    _this.img.src = _this.base64Image;
                    _this.renderImage();
                    var t = setTimeout(function () { _this.renderImage(); }, 20);
                    return;
                }
                //this.CurrentSession.StopBusyIndicator();
                _this.CurrentPageIndex = index;
            });
        }
    };
    DeclarationSplitComponent.prototype.StartBusyIndicator = function (text) {
        if (text === void 0) { text = "Loading..."; }
        this.splitIndicatorText = text;
        this.showSplitIndicator = true;
    };
    DeclarationSplitComponent.prototype.StopBusyIndicator = function () {
        this.showSplitIndicator = false;
    };
    //#endregion
    DeclarationSplitComponent.prototype.LoadDocuments = function () {
        var _this = this;
        this.CurrentSession.StartBusyIndicatorLoading();
        var objecttable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.Declaration"; })[0];
        this.custDocRelatedDocsWebService.GetDocumentsFilingsForRelatedDocuments(this.DeclarationPM.Id, null, objecttable.Id, "I", this.DeclarationPM.CustomFileNo, this.DocumentFilterSelectedValue)
            .subscribe(function (response) {
            console.log("[response] GetDocumentsFilingsForRelatedDocuments:", response);
            _this.CurrentSession.StopBusyIndicator();
            if (!Tools_1.AppTool.IsNullOrEmpty(response)) {
                _this.RelatedDocuments = [];
                var relatedDocs;
                relatedDocs = response.Result;
                for (var i = 0; i < relatedDocs.length; i++) {
                    //var ticket = this.CustomsDocumentsTickets.filter(d => d.DocumentsFilingId == relatedDocs[i].Id)[0];
                    //var values: CustomsDocumentMetaDataValuePM[] = this.MetadataValues.filter(d => d.CustomsDocumentId == relatedDocs[i].Id);
                    var values = null;
                    //if (!ticket) {
                    var relatedDocViewModel = new RelatedDocumentViewModel_1.RelatedDocumentViewModel(relatedDocs[i], values, true);
                    _this.RelatedDocuments.push(relatedDocViewModel);
                    //}
                }
                //Load first document
                //this.TicketItemClicked(this.RelatedDocuments[0]);
                //this.IsDocsPanelVisible = true;
                //ClassifcationComponent Build B4 This Component finish Load Document !!!
                if (!Tools_1.AppTool.IsNullOrEmpty(_this.RelatedDocuments) &&
                    Tools_1.AppTool.IsNullOrEmpty(_this._DocumentFilingIdToSetWhileLoadDocument)) {
                    var document = _this.RelatedDocuments.find(function (d) { return d.Id == _this._DocumentFilingIdToSetWhileLoadDocument; });
                    _this._DocumentFilingIdToSetWhileLoadDocument = null;
                    _this.TicketItemClicked(document);
                }
            }
        });
    };
    DeclarationSplitComponent.prototype.DownloadDocumentFile = function (documentsFilingId) {
        var _this = this;
        this.custDocRelatedDocsWebService.GetSingleDocumentsFilingPM(documentsFilingId).subscribe(function (resp) {
            var documentFiling = resp.Result;
            _this._ImageLibraryService.DownloadFile(documentFiling.DocumentId, documentFiling.Extension, documentFiling.Folder, SessionLocator_1.SessionLocator.Tenant).subscribe(function (res) {
                var documentName = documentFiling.DocumentId;
                var token = ServiceHelper_1.ServiceHelper.GetLDocumentDownloadToken();
                var uri = ServiceHelper_1.ServiceHelper.GetLogitudeURL() + "WebPages/Downloadpage.aspx?id=" + documentName + "&tempId=" + token;
                if (AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                    AmitalGatewayUtil_1.AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(uri);
                    return;
                }
                DownloadManager_1.DownloadManager.DownloadPage(documentName);
            });
        });
    };
    Object.defineProperty(DeclarationSplitComponent.prototype, "CurrentPageIndex", {
        get: function () { return this.currentPageIndex; },
        set: function (value) {
            this.currentPageIndex = value;
        },
        enumerable: true,
        configurable: true
    });
    DeclarationSplitComponent.prototype.TextChanged = function (valueString) {
        var _this = this;
        var value = Number(valueString);
        this.timerToken = setTimeout(function () {
            if (value > 0 && value <= _this.pagesCount) {
                _this.LoadDocumentPage(value);
            }
        }, 1000);
    };
    Object.defineProperty(DeclarationSplitComponent.prototype, "TrackBarValue", {
        get: function () {
            return this.trackBarValue;
        },
        set: function (value) {
            this.trackBarValue = value;
            this.CalculateScaleValue();
        },
        enumerable: true,
        configurable: true
    });
    DeclarationSplitComponent.prototype.ZoomInButton = function () {
        if (this.TrackBarValue >= 3)
            return;
        this.TrackBarValue += +this.TrackBarStep;
        this.CalculateScaleValue();
    };
    DeclarationSplitComponent.prototype.ZoomOutButton = function () {
        if (this.TrackBarValue <= 1)
            return;
        this.TrackBarValue -= +this.TrackBarStep;
        this.CalculateScaleValue();
    };
    DeclarationSplitComponent.prototype.CalculateScaleValue = function () {
        //var scaleValue = this.trackBarValue / 100 + 1;
        var scaleValue = this.trackBarValue;
        this.ImgScaleValue = "scale(" + scaleValue + ")";
    };
    //#endregion
    //#region Rotation
    //ImgTransformOriginValue: string = "right top";
    //ImgRotationValue: string = "rotate(0deg)";
    //RotationAngle: number = 0;
    //ToggleTransformOrigin() {
    //    if (this.RotationAngle == 0) {
    //        this.ImgTransformOriginValue = "right top";
    //    }
    //    else if (this.RotationAngle == 90) {
    //        this.ImgTransformOriginValue = "left top";
    //    }
    //    else if (this.RotationAngle == 180) {
    //        this.ImgTransformOriginValue = "left bottom";
    //    }
    //    else if (this.RotationAngle == 270) {
    //        this.ImgTransformOriginValue = "right bottom";
    //    }
    //    else if (this.RotationAngle == 360) {
    //        this.ImgTransformOriginValue = "right top";
    //    }
    //}
    DeclarationSplitComponent.prototype.RotateRightButton = function () {
        if (this.IsNoDocumentSelected)
            return;
        //Rotate
        //if (this.RotationAngle >= 360)
        //    this.RotationAngle = 90;
        //else
        //    this.RotationAngle += 90;
        //this.ImgRotationValue = "rotate(" + this.RotationAngle + "deg)";
        //origin position
        //this.ToggleTransformOrigin();
        this.rotateCW();
    };
    DeclarationSplitComponent.prototype.RotateLeftButton = function () {
        if (this.IsNoDocumentSelected)
            return;
        //if (this.RotationAngle <= 0)
        //    this.RotationAngle = 270;
        //else
        //    this.RotationAngle -= 90;
        //this.ImgRotationValue = "rotate(" + this.RotationAngle + "deg)";
        //origin position
        //this.ToggleTransformOrigin();
        this.rotateCCW();
    };
    DeclarationSplitComponent.prototype.startRenderingImage = function () {
        //this.img.src = './Images/Split/testimage.png'; // http://i.imgur.com/sAyE5ZE.png
        //this.img.src = this.base64Image;
        this.canvas = document.getElementById('canvas');
        this.ctx = this.canvas.getContext('2d');
        this.renderImage();
    };
    DeclarationSplitComponent.prototype.renderImage = function () {
        this.StartBusyIndicator("Rendering...");
        /// use index to set canvas size
        switch (this.angleIndex) {
            case 0:
            case 2:
                /// for 0 and 180 degrees size = image
                this.canvas.width = this.img.width;
                this.canvas.height = this.img.height;
                break;
            case 1:
            case 3:
                /// for 90 and 270 canvas width = img height etc.
                this.canvas.width = this.img.height;
                this.canvas.height = this.img.width;
                break;
        }
        /// get stored angle and center of canvas    
        var angle = this.angles[this.angleIndex], cw = this.canvas.width * 0.5, ch = this.canvas.height * 0.5;
        /// rotate context
        this.ctx.translate(cw, ch);
        this.ctx.rotate(angle);
        this.ctx.translate(-this.img.width * 0.5, -this.img.height * 0.5);
        /// draw image and reset transform
        this.ctx.drawImage(this.img, 0, 0);
        this.ctx.setTransform(1, 0, 0, 1, 0, 0);
        this.img.src = this.base64Image;
        this.StopBusyIndicator();
        this.cd.detectChanges();
    };
    DeclarationSplitComponent.prototype.rotateCW = function () {
        this.angleIndex++; /// increment index of array
        if (this.angleIndex >= this.angles.length)
            this.angleIndex = 0;
        this.renderImage();
    };
    DeclarationSplitComponent.prototype.rotateCCW = function () {
        this.angleIndex--; /// decrement index of array
        if (this.angleIndex < 0)
            this.angleIndex = this.angles.length - 1;
        this.renderImage();
    };
    DeclarationSplitComponent.prototype.OnMouseWheel = function (event) {
        console.log("[EVENT] MouseWheel, ", event);
        if (event) {
            if (event.altKey) {
                event.preventDefault();
                var delta = event.deltaY / 100;
                if (delta < 0)
                    this.ZoomInButton();
                else
                    this.ZoomOutButton();
            }
            if (event.ctrlKey) {
                event.preventDefault();
                var delta = event.deltaY / 100;
                if (delta < 0)
                    this.ZoomInButton();
                else
                    this.ZoomOutButton();
            }
        }
    };
    DeclarationSplitComponent.prototype.OnMouseMove = function (event) {
        if (event) {
            if (event.which == 1) {
                var element = document.getElementById(this.DocumentViewerImageId);
                //console.log("[EVENT] MouseMove, ", event);
                //console.log("[crd] scrollTop, ", element.scrollTop);
                //console.log("[crd] scrollLeft, ", element.scrollLeft);
                var deltaY;
                var deltaX;
                //if (this.RotationAngle == 180 ) {
                //    deltaY = (this.lastOffsetY - event.offsetY) * -1 ;
                //    deltaX = (this.lastOffsetX - event.offsetX) * -1 ;
                //} else if (this.RotationAngle == 90 ) {
                //    deltaX = (this.lastOffsetY - event.offsetY) * -1;
                //    deltaY = (this.lastOffsetX - event.offsetX);
                //} else if (this.RotationAngle == 270) {
                //    deltaX = (this.lastOffsetY - event.offsetY);
                //    deltaY = (this.lastOffsetX - event.offsetX) * -1;
                //} else {
                deltaY = this.lastOffsetY - event.offsetY;
                deltaX = this.lastOffsetX - event.offsetX;
                //}
                element.scrollTop += deltaY;
                element.scrollLeft += deltaX;
                console.log("--------------------------");
                console.log("[scrollTop] Y: " + this.lastOffsetY + "-" + event.offsetY + "=" + deltaY);
                console.log("[scrollLeft] X: " + this.lastOffsetX + "-" + event.offsetX + "=" + deltaX);
            }
        }
    };
    DeclarationSplitComponent.prototype.OnMouseUp = function (event) {
    };
    DeclarationSplitComponent.prototype.OnMouseDown = function (event) {
        if (event) {
            this.lastOffsetX = event.offsetX;
            this.lastOffsetY = event.offsetY;
        }
    };
    DeclarationSplitComponent.prototype.DocumentFilterItemClicked = function (value) {
        this.DocumentFilterSelectedValue = value;
        this.LoadDocuments();
        //this.GetRelatedDocuments();
    };
    DeclarationSplitComponent = __decorate([
        core_1.Component({
            moduleId: module.id,
            templateUrl: './DeclarationSplitComponent.html',
        }),
        __metadata("design:paramtypes", [core_1.ChangeDetectorRef])
    ], DeclarationSplitComponent);
    return DeclarationSplitComponent;
}(BaseComponent_1.BaseComponent));
exports.DeclarationSplitComponent = DeclarationSplitComponent;
//# sourceMappingURL=DeclarationSplitComponent.js.map