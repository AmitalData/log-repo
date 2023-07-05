declare var window: any;
import { Component, AfterViewInit, OnDestroy, ChangeDetectorRef, ViewChildren, QueryList, Output, Input, OnInit, ViewEncapsulation, ViewChild, HostListener, ElementRef } from '@angular/core';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { LocationDirective } from '../../../../Infrastructure/Utilities/LocationDirective';
import { ApiQueryFilters, FilterItem } from '../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { AppTool, ArrayTool, DateTool } from '../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import { BaseComponent } from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../Infrastructure/Utilities/ObservableCollection';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { LogitudeWindow } from '../../../../Controls/Windows/LogitudeWindow';
import { Validator } from '../../../../Infrastructure/Validators/Validator';
import { MessageWindow } from '../../../../Controls/Windows/MessageWindow';
import { ObjectTablePM } from '../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { ServiceHelper } from '../../../../Infrastructure/Utilities/ServiceHelper';
import { AmitalGatewayUtil, UnifreightMessageM } from '../../../../Infrastructure/Utilities/AmitalGatewayUtil';
//import * as cv from 'opencv4nodejs';
//import * as Tesseract from 'tesseract.js';
import { DeclarationPM } from '../../../../Customs/EntityPMs/DeclarationPM';
import { DocumentsFilingPM } from '../../../../Common/EntityPMs/DocumentsFilingPM';
import { RelatedDocumentViewModel } from '../../../CustomsDocuments/Components/RelatedDocumentViewModel';

// Services
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { CustomsSettingListService } from '../../../../Customs/Services/StandardLists/CustomsSettingListService';
//import {DeclarationWebService} from '../Services/WebServices/DeclarationWebService';
import { CustDocRelatedDocsWebService } from '../../../../Customs/Services/WebServices/CustDocRelatedDocsWebService';
import { ImageLibraryService } from '../../../../Common/Services/Others/ImageLibraryService';
import { CustomDocumentViewerService } from '../../../../Customs/Services/WebServices/CustomDocumentViewerService';
import { CustomsDocumentMetaDataValuePM } from '../../../../Customs/EntityPMs/CustomsDocumentMetaDataValuePM';
import { CustDocMetaDataValuesWebService } from '../../../../Customs/Services/WebServices/CustDocMetaDataValuesWebService';
import { DeclarationEventManager } from '../../../../Customs/Utilities/DeclarationEventManager';
import { ControlsIdCounter } from '../../../../Infrastructure/Utilities/ControlsIdCounter';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';
import { SupplierInvoiceItemPM } from '../../../../Customs/EntityPMs/SupplierInvoiceItemPM';
import { CustomsDocumentsDataProvider } from 'CustomsModules/CustomsDocuments/Components/CustomsDocumentsDataProvider';
import { NullTemplateVisitor } from '@angular/compiler';
@Component({

    templateUrl: './DeclarationSplitComponent.html',
})

export class DeclarationSplitComponent extends BaseComponent implements AfterViewInit, OnDestroy {
    public DataContext: any = this;
    public DeclarationPM: DeclarationPM;
    public ObjectTableName: string = "Customs.Declaration";
    IsDocsPanelVisible: boolean = false;
    public MetadataValues: CustomsDocumentMetaDataValuePM[];
    public RelatedDocuments: RelatedDocumentViewModel[];
    base64Image: string;
    pagesCount: number = 0;
    IsConnectedToUniFreight: boolean = false;
    timerToken: any;
    IsNoDocumentSelected: boolean = true;
    DocumentViewerImageId: string;
    selectedText = '';
    startX = 0;
    startY = 0;
    endX = 0;
    endY = 0;

    public customs: string = "עמילות";
    public forwarding: string = "שילוח";
    //Services
    private custDocRelatedDocsWebService: CustDocRelatedDocsWebService = new CustDocRelatedDocsWebService();
    private _ImageLibraryService: ImageLibraryService = new ImageLibraryService();
    private _CustomDocumentViewerService: CustomDocumentViewerService = new CustomDocumentViewerService();
    private custDocsMetadataWebService: CustDocMetaDataValuesWebService = new CustDocMetaDataValuesWebService();
    private customsSettingListService: CustomsSettingListService = new CustomsSettingListService;
    DeclarationSplitDocumentSelectionEVENT;
    DeclarationSplitDocumentItemSelectionEVENT;
    invoiceItem: any;
    constructor(private cd: ChangeDetectorRef, private elem: ElementRef) {
        super();
        var counter = ControlsIdCounter.GetNextControlIdCounter("DocumentViewerImage");
        this.DocumentViewerImageId = "DocumentViewerImage-" + counter;

    }
    @ViewChild('myImg', { static: true }) myImgVariable: ElementRef;

    ngAfterViewInit() {
        /*this.myImgVariable.nativeElement.onload = () => {
            this.recognizeText();
        }*/
        this.startRenderingImage();

    }
    ngOnDestroy() {
        AppTool.KillEventEmitter(this.DeclarationSplitDocumentSelectionEVENT);
        AppTool.KillEventEmitter(this.DeclarationSplitDocumentItemSelectionEVENT);

    }


    _DocumentFilingIdToSetWhileLoadDocument: string;
    SetComponentArgs(args: any) {
        debugger;
        if (!AppTool.IsNullOrEmpty(args)) {
            this.DeclarationPM = args.EntityPM;

            // 1- get customs settings
            this.customsSettingListService.getSingleFromCache(this.DeclarationPM.Tenant.toString()).subscribe((response: ServiceResponse) => {
                var list = response.Result;
                if (!AppTool.IsNullOrEmpty(list)) {
                    var customsSetting = list;
                    this.IsConnectedToUniFreight = customsSetting.IsConnectedToUniFreight;
                }
                if (this.DeclarationPM.Direction == 'E') {
                    this.customs = "תיק מכס";
                    this.forwarding = "תיק יצום";

                    this.DocumentFilterSelectedValue = "all";
                }
                //// 2- get metadata values then
                //SessionLocator.SelectedSession.StartBusyIndicatorLoading();
                //this.custDocsMetadataWebService.GetCustomsDocumentMetaDataValuesByCustomsDocumentFilingIds(customsDocTickets).subscribe((response2: ServiceResponse) => {
                //    this.MetadataValues = response2.Result;

                // 3- load documents(tickets)
                this.LoadDocuments();



                SessionLocator.SelectedSession.StopBusyIndicator();


                //    });
            });

            this.DeclarationSplitDocumentSelectionEVENT = DeclarationEventManager.DeclarationSplitDocumentSelection.subscribe((DocumentFilingId: any) => {
                console.log("-->> Loading document for supplier invoice: " + DocumentFilingId);
                if (AppTool.IsNullOrEmpty(this.RelatedDocuments)) {
                    //ClassifcationComponent Build B4 This Component finish Load Document !!!
                    this._DocumentFilingIdToSetWhileLoadDocument = DocumentFilingId;
                    return;
                }
                var document = this.RelatedDocuments.find(d => d.Id == DocumentFilingId);
                this.TicketItemClicked(document);

            });


            this.DeclarationSplitDocumentItemSelectionEVENT = DeclarationEventManager.DeclarationSplitDocumentItemSelection.subscribe((data: any) => {

                if (data == "remove") {

                    var elem = document.getElementById("rectangle-rectangle-1");
                    elem.parentNode.removeChild(elem);
                    return;
                }
                this.invoiceItem = data;
                if (AppTool.IsNullOrEmpty(this.RelatedDocuments)) {
                    //ClassifcationComponent Build B4 This Component finish Load Document !!!
                    this._DocumentFilingIdToSetWhileLoadDocument = this.invoiceItem.DocumentFilingId;
                    return;
                }
                var document1 = this.RelatedDocuments.find(d => d.Id == this.invoiceItem.DocumentFilingId);
                this.TicketItemClicked(document1, true);


            });
        }
    }





    //#region Document List
    SelectedTicket: RelatedDocumentViewModel;

    PageUp() {
        if (this.CurrentPageIndex > 0)
            this.CurrentPageIndex--;
        this.LoadDocumentPage();
    }
    PageDown() {
        if (this.CurrentPageIndex < this.pagesCount)
            this.CurrentPageIndex++;
        this.LoadDocumentPage();
    }

    IsMouseOverDownload: boolean = false;
    TicketItemClicked(document: RelatedDocumentViewModel, selectItem: boolean = false) {

        if (this.IsMouseOverDownload) return;
        if (this.DeclarationPM.Direction != "E")
            this.IsDocsPanelVisible = false;
        //reset counters
        this.pagesCount = 1;
        this.CurrentPageIndex = 1;

        this.SelectedTicket = document;
        this.IsNoDocumentSelected = false;

        this.angleIndex = 0;
        this.ImgScaleValue = "scale(1)";
        this.TrackBarValue = 1;

        this.LoadDocumentPage(null, selectItem);
        //this.LoadDocumentPage(); // need to check it again, it cannot draw image at first call

    }
    RefreshButtonClicked() {
        this.resample_single(this.canvas, this.canvas.width, this.canvas.height, true);

        // if (!AppTool.IsNullOrEmpty(this.CurrentPageIndex))
        //     this.LoadDocumentPage();
        // //this.renderImage();
    }
    OpenInWindowButtonClicked() {



        //var documentFiling = resp.Result;
        this._ImageLibraryService.DownloadFile(this.SelectedTicket.documentsFilingPM.DocumentId, this.SelectedTicket.documentsFilingPM.FileExtension, this.SelectedTicket.documentsFilingPM.Folder, SessionLocator.Tenant).subscribe((res: any) => {


            var documentName = SessionLocator.Tenant + "_" + this.SelectedTicket.documentsFilingPM.DocumentId;


            DownloadManager.DownloadPage(documentName);

        });



    }

    composedPath(el) {
        let path = [];
        while (el) {
            path.push(el);
            if (el.tagName === 'HTML') {
                path.push(document);
                path.push(window);
                return path;
            }
            el = el.parentElement;
        }
    }




    LoadDocumentPage(pageIndex: number = null, selectItem: boolean = false) {
        debugger;
        if (this.SelectedTicket) {

            this.StartBusyIndicator("Loading page...");

            var index = pageIndex ? pageIndex : this.CurrentPageIndex;

            if (index == 0) index = 1;

            if (this.invoiceItem != null && !AppTool.IsNullOrEmpty(this.invoiceItem.OcrPageNumber) && this.invoiceItem.OcrPageNumber != 0 && selectItem) {
                index = this.invoiceItem.OcrPageNumber;
            }

            console.log("Load Page: ", index);

            //SessionLocator.SelectedSession.StartBusyIndicatorLoading();

            //if (this.IsConnectedToUniFreight)
            //    var index = this.CurrentPageIndex;
            //else
            //    var index = this.CurrentPageIndex - 1;

            //let path = this.composedPath(event.target);

            this._CustomDocumentViewerService.GetDocumentPage(this.SelectedTicket.documentsFilingPM.DocumentId, index - 1, this.IsConnectedToUniFreight, this.RotationAngle).subscribe((myResponse: ServiceResponse) => {
                var result = myResponse.Result;
                console.log("[Response] GetDocumentPage", result);
                this.StopBusyIndicator();
                if (result) {

                    //reset rotation
                    //this.ImgTransformOriginValue = "right top";
                    //this.ImgRotationValue = "rotate(0deg)";
                    //this.RotationAngle = 0;

                    this.pagesCount = result.Count;

                    if (!AppTool.IsNullOrEmpty(result.Page)) {
                        //SessionLocator.SelectedSession.StopBusyIndicator();

                        this.base64Image = "data:image/png;base64," + result.Page;

                        //document.getElementsByClassName("div-grabbable")[0].removeChild(document.getElementsByClassName("rectangle")[0]);
                        var elements = document.getElementsByClassName("rectangle");
                        while (elements.length > 0) {
                            elements[0].parentNode.removeChild(elements[0]);
                        }

                        if (this.invoiceItem != null && this.invoiceItem.OcrTop != 0 && this.invoiceItem.OcrTop != undefined && this.invoiceItem.OcrHeight != 0 && this.invoiceItem.OcrHeight != undefined && selectItem) {
                            var elem = document.getElementsByClassName("grabbable")[0] as HTMLImageElement;;

                            let rect = document.createElement('div');
                            rect.className = 'rectangle';
                            rect.id = 'rectangle-' + "rectangle-1";
                            rect.style.position = 'absolute';
                            rect.style.border = '1px solid #ed1c31';
                            rect.style.borderRadius = '3px';
                            rect.style.left = 0 + 'px';
                            var percent = (elem.height / elem.naturalHeight);
                            rect.style.top = (this.invoiceItem.OcrTop * percent) - 1 + 'px';
                            rect.style.width = '100%';
                            rect.style.height = (this.invoiceItem.OcrHeight * percent) + 2 + 'px';
                            document.getElementsByClassName("div-grabbable")[0].appendChild(rect);

                            console.log(this.base64Image);
                            this.CurrentPageIndex = this.invoiceItem.OcrPageNumber;
                        }
                        else {
                            this.CurrentPageIndex = index;

                        }



                        // this.img.src = this.base64Image;
                        // this.renderImage();
                        // var t = setTimeout(() => { this.renderImage(); }, 20);

                    } else {
                        this.CurrentPageIndex = 0;
                        this.base64Image = null;
                        // this.img.src = this.base64Image;
                        // this.renderImage();
                        // var t = setTimeout(() => { this.renderImage(); },20);
                        return;
                    }

                } else {
                    this.CurrentPageIndex = 0;
                    //SessionLocator.SelectedSession.StopBusyIndicator();
                    this.base64Image = null;
                    // this.img.src = this.base64Image;
                    // this.renderImage();
                    // var t = setTimeout(() => { this.renderImage(); },20);
                    return;
                }
                //SessionLocator.SelectedSession.StopBusyIndicator();



            });
        }
    }

    //#region split indicator
    showSplitIndicator: boolean = false;
    splitIndicatorText: string = "";
    StartBusyIndicator(text: string = "Loading...") {
        this.splitIndicatorText = text;
        this.showSplitIndicator = true;
    }
    StopBusyIndicator() {
        this.showSplitIndicator = false;
    }
    //#endregion
    private customsDocumentsDataProvider: CustomsDocumentsDataProvider;

    LoadDocuments() {
        this.customsDocumentsDataProvider = new CustomsDocumentsDataProvider(this.ObjectTableName, this.DeclarationPM, null, null, null);

        SessionLocator.SelectedSession.StartBusyIndicatorLoading();
        var objecttable = window.ObjectTables.filter(x => x.Name === "Customs.Declaration")[0];

        this.customsDocumentsDataProvider.GetCustomsDocumentsRelatedDocuments(this.DocumentFilterSelectedValue)
            .subscribe((response: ServiceResponse) => {
                console.log("[response] GetDocumentsFilingsForRelatedDocuments:", response);
                SessionLocator.SelectedSession.StopBusyIndicator();

                if (!AppTool.IsNullOrEmpty(response)) {

                    this.RelatedDocuments = [];
                    var relatedDocs: DocumentsFilingPM[];
                    relatedDocs = response.Result;
                    for (var i = 0; i < relatedDocs.length; i++) {
                        //var ticket = this.CustomsDocumentsTickets.filter(d => d.DocumentsFilingId == relatedDocs[i].Id)[0];
                        //var values: CustomsDocumentMetaDataValuePM[] = this.MetadataValues.filter(d => d.CustomsDocumentId == relatedDocs[i].Id);
                        var values = null;

                        //if (!ticket) {
                        var relatedDocViewModel = new RelatedDocumentViewModel(relatedDocs[i], values, true);
                        this.RelatedDocuments.push(relatedDocViewModel);
                        //}
                    }
                    //Load first document
                    //this.TicketItemClicked(this.RelatedDocuments[0]);
                    //this.IsDocsPanelVisible = true;



                    //ClassifcationComponent Build B4 This Component finish Load Document !!!
                    if (
                        this.RelatedDocuments.length != 0 &&
                        AppTool.IsNullOrEmpty(this._DocumentFilingIdToSetWhileLoadDocument)) {
                        var document = this.RelatedDocuments.find(d => d.Id == this._DocumentFilingIdToSetWhileLoadDocument);
                        this._DocumentFilingIdToSetWhileLoadDocument = null;
                        this.TicketItemClicked(document);
                    }
                }
            });
    }




    DownloadDocumentFile(documentsFilingId: string) {

        this.custDocRelatedDocsWebService.GetSingleDocumentsFilingPM(documentsFilingId).subscribe((resp: ServiceResponse) => {
            var documentFiling = resp.Result;
            this._ImageLibraryService.DownloadFile(documentFiling.DocumentId, documentFiling.Extension, documentFiling.Folder, SessionLocator.Tenant).subscribe((res: any) => {


                var documentName = documentFiling.DocumentId;
                //var token = ServiceHelper.GetLDocumentDownloadToken();
                //let uri = ServiceHelper.GetLogitudeURL() + "WebPages/Downloadpage.aspx?id=" + documentName + "&tempId=" + token;
                //if (AmitalGatewayUtil.Instance.AmitalBrowserInUse) {
                //    AmitalGatewayUtil.Instance.DeclarationMessaging.RaiseOpenNewBrowser(uri);
                //    return;
                //}
                DownloadManager.DownloadPage(documentName);


            });
        });

    }


    currentPageIndex: number = 0;
    public get CurrentPageIndex() { return this.currentPageIndex }
    public set CurrentPageIndex(value: number) {
        this.currentPageIndex = value;
    }

    TextChanged(valueString: string) {
        var value = Number(valueString);

        this.timerToken = setTimeout(() => {

            if (value > 0 && value <= this.pagesCount) {
                this.LoadDocumentPage(value);
            }

        }, 1000);
    }

    //#endregion

    //#region Zooming
    trackBarValue: number = 1;
    get TrackBarValue() {
        return this.trackBarValue;
    }
    set TrackBarValue(value: number) {
        this.trackBarValue = value;
        this.CalculateScaleValue();
        //this.resample_single(this.canvas, this.canvas.width, this.canvas.height, true);


    }

    TrackBarStep: number = 0.2;
    ImgScaleValue: string = "scale(1)";
    ImgHeight: string = "";

    ZoomInButton() {
        if (this.TrackBarValue >= 3) return;
        this.TrackBarValue += +this.TrackBarStep;
        this.CalculateScaleValue();

        if (this.TrackBarValue == 1)
            this.resample_single(this.canvas, this.canvas.width, this.canvas.height, true);


    }
    ZoomOutButton() {
        if (this.TrackBarValue <= 1) return;
        this.TrackBarValue -= +this.TrackBarStep;
        this.CalculateScaleValue();

    }
    CalculateScaleValue() {
        //var scaleValue = this.trackBarValue / 100 + 1;
        var scaleValue = this.trackBarValue;
        this.ImgScaleValue = "scale(" + scaleValue + ")";
        this.ImgHeight = (scaleValue * 100).toString() + "%";
    }
    //#endregion

    //#region Rotation
    ImgTransformOriginValue: string = "right top";
    ImgRotationValue: string = "rotate(0deg)";
    RotationAngle: number = 0;

    ToggleTransformOrigin() {

        if (this.RotationAngle == 0) {
            this.ImgTransformOriginValue = "right top";
        }
        else if (this.RotationAngle == 90) {
            this.ImgTransformOriginValue = "left top";
        }
        else if (this.RotationAngle == 180) {
            this.ImgTransformOriginValue = "left bottom";
        }
        else if (this.RotationAngle == 270) {
            this.ImgTransformOriginValue = "right bottom";
        }
        else if (this.RotationAngle == 360) {
            this.ImgTransformOriginValue = "right top";
        }
    }


    public get transformValue(): string {
        return this.ImgScaleValue + ' ' + this.ImgRotationValue;
    }



    RotateRightButton() {
        if (this.IsNoDocumentSelected) return;

        // Rotate
        if (this.RotationAngle >= 360)
            this.RotationAngle = 90;
        else
            this.RotationAngle += 90;

        this.ImgRotationValue = "rotate(" + this.RotationAngle + "deg)";

        // origin position
        this.ToggleTransformOrigin();

        this.LoadDocumentPage(this.CurrentPageIndex);

        // this.rotateCW();

    }
    RotateLeftButton() {
        if (this.IsNoDocumentSelected) return;

        if (this.RotationAngle <= 0)
            this.RotationAngle = 270;
        else
            this.RotationAngle -= 90;

        this.ImgRotationValue = "rotate(" + this.RotationAngle + "deg)";

        //origin position
        this.ToggleTransformOrigin();

        this.LoadDocumentPage(this.CurrentPageIndex);
        // this.rotateCCW();

    }

    //- rotate image to convas - JS Code
    img = new Image;
    canvas;
    ctx;
    angles = [0 * Math.PI, 0.5 * Math.PI, Math.PI, 1.5 * Math.PI];  // store angles (0, 90, 180, 270) in an array
    angleIndex = 0;

    startRenderingImage() {

        //this.img.src = './Images/Split/testimage.png'; // http://i.imgur.com/sAyE5ZE.png
        //this.img.src = this.base64Image;
        this.canvas = document.getElementById('canvas');
        this.ctx = this.canvas.getContext('2d');


        this.renderImage();


    }
    renderImage() {
        if (this.base64Image) {
            this.StartBusyIndicator("Rendering...");

            /// use index to set canvas size
            // switch (this.angleIndex) {
            //     case 0:
            //     case 2:
            //         /// for 0 and 180 degrees size = image
            //         this.canvas.width = this.img.width;
            //         this.canvas.height = this.img.height;
            //         break;
            //     case 1:
            //     case 3:
            //         /// for 90 and 270 canvas width = img height etc.
            //         this.canvas.width = this.img.height;
            //         this.canvas.height = this.img.width;
            //         break;
            // }

            // /// get stored angle and center of canvas
            // var angle = this.angles[this.angleIndex],
            //     cw = this.canvas.width * 0.5,
            //     ch = this.canvas.height * 0.5;

            // /// rotate context
            // this.ctx.translate(cw, ch);
            // this.ctx.rotate(angle);
            // this.ctx.translate(-this.img.width * 0.5, -this.img.height * 0.5);

            // /// draw image and reset transform
            // this.ctx.drawImage(this.img, 0, 0);
            // this.ctx.setTransform(1, 0, 0, 1, 0, 0);
            // this.img.src = this.base64Image;




            this.StopBusyIndicator();
            this.cd.detectChanges();


            // this.resample_single(this.canvas, this.canvas.width, this.canvas.height, true);

            // setTimeout(() => {
            //     this.resample_single(this.canvas, this.canvas.width, this.canvas.height, true);
            // }, 500);

        } else {
            this.ctx.clearRect(0, 0, this.canvas.width, this.canvas.height);

            this.StopBusyIndicator();
            this.cd.detectChanges();
        }

    }
    resample_single(canvas, width, height, resize_canvas) {

        // console.log("RESAMPLE: start");


        // var width_source = canvas.width;
        // var height_source = canvas.height;
        // width = Math.round(width);
        // height = Math.round(height);

        // width_source = width_source == 0 ? 1 : width_source;
        // height_source = height_source == 0 ? 1 : height_source;
        // width = width == 0 ? 1 : width;
        // height = height == 0 ? 1 : height;

        // var ratio_w = width_source / width;
        // var ratio_h = height_source / height;
        // var ratio_w_half = Math.ceil(ratio_w / 2);
        // var ratio_h_half = Math.ceil(ratio_h / 2);

        // var ctx = canvas.getContext("2d");
        // var img = ctx.getImageData(0, 0, width_source, height_source);
        // var img2 = ctx.createImageData(width, height);
        // var data = img.data;
        // var data2 = img2.data;

        // for (var j = 0; j < height; j++) {
        //     for (var i = 0; i < width; i++) {
        //         var x2 = (i + j * width) * 4;
        //         var weight = 0;
        //         var weights = 0;
        //         var weights_alpha = 0;
        //         var gx_r = 0;
        //         var gx_g = 0;
        //         var gx_b = 0;
        //         var gx_a = 0;
        //         var center_y = (j + 0.5) * ratio_h;
        //         var yy_start = Math.floor(j * ratio_h);
        //         var yy_stop = Math.ceil((j + 1) * ratio_h);
        //         for (var yy = yy_start; yy < yy_stop; yy++) {
        //             var dy = Math.abs(center_y - (yy + 0.5)) / ratio_h_half;
        //             var center_x = (i + 0.5) * ratio_w;
        //             var w0 = dy * dy; //pre-calc part of w
        //             var xx_start = Math.floor(i * ratio_w);
        //             var xx_stop = Math.ceil((i + 1) * ratio_w);
        //             for (var xx = xx_start; xx < xx_stop; xx++) {
        //                 var dx = Math.abs(center_x - (xx + 0.5)) / ratio_w_half;
        //                 var w = Math.sqrt(w0 + dx * dx);
        //                 if (w >= 1) {
        //                     //pixel too far
        //                     continue;
        //                 }
        //                 //hermite filter
        //                 weight = 2 * w * w * w - 3 * w * w + 1;
        //                 var pos_x = 4 * (xx + yy * width_source);
        //                 //alpha
        //                 gx_a += weight * data[pos_x + 3];
        //                 weights_alpha += weight;
        //                 //colors
        //                 if (data[pos_x + 3] < 255)
        //                     weight = weight * data[pos_x + 3] / 250;
        //                 gx_r += weight * data[pos_x];
        //                 gx_g += weight * data[pos_x + 1];
        //                 gx_b += weight * data[pos_x + 2];
        //                 weights += weight;
        //             }
        //         }
        //         data2[x2] = gx_r / weights;
        //         data2[x2 + 1] = gx_g / weights;
        //         data2[x2 + 2] = gx_b / weights;
        //         data2[x2 + 3] = gx_a / weights_alpha;
        //     }
        // }

        // //clear and resize canvas
        // if (resize_canvas === true) {
        //     canvas.width = width;
        //     canvas.height = height;
        // } else {
        //     ctx.clearRect(0, 0, width_source, height_source);
        // }

        // //draw
        // ctx.putImageData(img2, 0, 0);

        // console.log("RESAMPLE: done");

    }

    resample_light(canvas, width, height, resize_canvas) {
        console.log("RESAMPLE: start");


        var width_source = canvas.width;
        var height_source = canvas.height;
        width = Math.round(width);
        height = Math.round(height);

        width_source = width_source == 0 ? 1 : width_source;
        height_source = height_source == 0 ? 1 : height_source;
        width = width == 0 ? 1 : width;
        height = height == 0 ? 1 : height;

        var ratio_w = width_source / width;
        var ratio_h = height_source / height;
        var ratio_w_half = Math.ceil(ratio_w / 2);
        var ratio_h_half = Math.ceil(ratio_h / 2);

        var ctx = canvas.getContext("2d");
        var img = ctx.getImageData(0, 0, width_source, height_source);
        var img2 = ctx.createImageData(width, height);
        var data = img.data;
        var data2 = img2.data;


        //draw
        ctx.putImageData(img2, 0, 0);

        console.log("RESAMPLE: done");

    }



    rotateCW() {
        this.angleIndex++;     /// increment index of array
        if (this.angleIndex >= this.angles.length) this.angleIndex = 0;
        this.renderImage();
    }
    rotateCCW() {
        this.angleIndex--;      /// decrement index of array
        if (this.angleIndex < 0) this.angleIndex = this.angles.length - 1;
        this.renderImage();
    }

    //-

    //#endregion

    //#region Mouse Events
    lastOffsetY: number;
    lastOffsetX: number = 0;

    OnMouseWheel(event) {
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
    }
    OnMouseMove(event) {
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


    }
    /*@HostListener('contextmenu', ['$event'])
    onContextMenu(event: MouseEvent) {
        if (this.selectedText) {
            event.preventDefault();
            window.open(`https://www.google.com/search?q=${this.selectedText}`);
        }
    }*/




    /*recognizeText() {
        // Calculate the coordinates of the selected area
        const x = Math.min(this.startX, this.endX);
        const y = Math.min(this.startY, this.endY);
        const width = Math.abs(this.startX - this.endX);
        const height = Math.abs(this.startY - this.endY);

        // Use the createElement function to create a new canvas element
        var canvas = document.createElement("canvas");
        var ctx = canvas.getContext("2d");

        // Use the drawImage function to draw the selected area of the image on the canvas
        ctx.drawImage(this.myImgVariable.nativeElement, x, y, width, height, 0, 0, width, height);

        // Use the toDataURL function to get the data of the selected area
        var imgData = canvas.toDataURL();
        //const image = cv.imread(this.myImgVariable.nativeElement.src);

        // Use the Tesseract.recognize function to recognize the text in the selected area
        Tesseract.recognize(imgData)
            .then(result => {
                this.selectedText = result.data.text;
                //this.drawRectangle(image, imgData, new cv.Vec(0, 0, 255));
            })
            .catch(err => console.error(err));

    }*/

    /*drawRectangle(img, rect, color, thickness = 2) {
        img.drawRectangle(
            rect,
            color,
            thickness,
            cv.LINE_8
        );
    }*/
    @HostListener('mouseup', ['$event'])
    OnMouseUp(event) {
        /*  if (event) {
              this.endX = event.clientX;
              this.endY = event.clientY;
              this.recognizeText();
          }*/
    }

    @HostListener('mousedown', ['$event'])
    OnMouseDown(event) {
        if (event) {
            this.lastOffsetX = event.offsetX;
            this.lastOffsetY = event.offsetY;

            this.startX = event.clientX;
            this.startY = event.clientY;
        }



    }
    //#endregion

    DocumentFilterSelectedValue: string = "customs";
    DocumentFilterItemClicked(value: string) {
        this.DocumentFilterSelectedValue = value;
        this.LoadDocuments();
        //this.GetRelatedDocuments();
    }
}
