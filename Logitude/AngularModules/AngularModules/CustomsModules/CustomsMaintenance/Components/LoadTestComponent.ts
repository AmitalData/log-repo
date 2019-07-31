

declare var window: any;
import { Component, Output, EventEmitter, OnInit, ComponentRef } from '@angular/core';
import { BaseComponent } from       '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from  '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ListComponentArgs } from '../../../Infrastructure/Args';

import { ApiQueryFilters } from  '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from  '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from   '../../../Infrastructure/Services/EntityListService';


 


import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';



import { DeclarationListService} from '../../../Customs/Services/StandardLists/DeclarationListService';

import {DeclarationWebService} from '../../../Customs/Services/WebServices/DeclarationWebService';
import {DeclarationPMService} from '../../../Customs/Services/StandardPMs/DeclarationPMService';
import {LoadTestService} from '../../../Customs/Services/WebServices/LoadTestService';
import {DeclarationExtendedListService} from '../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import {DeclarationList} from '../../../Customs/EntityLists/DeclarationList';
import {DeclarationPM} from '../../../Customs/EntityPMs/DeclarationPM';
import {DocumentsFilingPM} from '../../../Common/EntityPMs/DocumentsFilingPM';
import {CustomsDocumentPM} from '../../../Customs/EntityPMs/CustomsDocumentPM';
import {CustomsDocumentMetaDataValuePM} from '../../../Customs/EntityPMs/CustomsDocumentMetaDataValuePM';


import {CustDocRelatedDocsWebService} from '../../../Customs/Services/WebServices/CustDocRelatedDocsWebService';

import {GenericRequestParams} from '../../../Customs/DataContract/RequestParams/GenericRequestParams';
import {PrintRequestRequestParams} from '../../../Customs/DataContract/RequestParams/PrintRequestRequestParams';

import { DeclarationMessagesService } from '../../../Customs/Services/WebServices/DeclarationMessagesService';
//import { PrintRequestRequestParams } from '../../../Customs/DataContract/RequestParams/PrintRequestRequestParams';
//import { PrintRequestResponseData, PrintRequestResultList } from '../../../DataContract/ResponseData/PrintRequestResponseData';

import {CustomsDocumentPMService} from '../../../Customs/Services/StandardPMs/CustomsDocumentPMService';

@Component({
    moduleId: module.id,
    templateUrl: './LoadTestComponent.html',
})





export class LoadTestComponent
    extends BaseComponent
    implements OnInit {

    public DataContext: LoadTestComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    public columns: any[] = null;



    private _DeclarationPMService: DeclarationPMService = new DeclarationPMService();
    private _DeclarationListService: DeclarationListService = new DeclarationListService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _LoadTestService: LoadTestService = new LoadTestService();
    private _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    private _custDocRelatedDocsWebService: CustDocRelatedDocsWebService = new CustDocRelatedDocsWebService();
    ///public ComponentRef: ComponentRef<LoadTestComponent>;

    //entityPM: CustomsSettingPM;

    ValidationErrorsList: string[] = [];


    public _GetNewCustomFileList: number[] = [];
    public _OpenDecFiligList: number[] = [];
    public _SendDecList: number[] = [];
    public _SaveDecList: number[] = [];
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    Loaded: boolean = false;
    ngOnInit() {
        this.Loaded = true;
    }

    ValidScreen() {

    }

    ///#region Properties

    //IsUnifreightCertificateActivatedEnabled: boolean = true;




    _CustomerId: string = "10009065";
    get CustomerId() { return this._CustomerId; }
    set CustomerId(value: string) { this._CustomerId = value; }


    _Consignee: string = "1000";
    get Consignee() { return this._Consignee; }
    set Consignee(value) { this._Consignee = value; }

    _CopyFromDecId: string = "1-104502";
    get CopyFromDecId() { return this._CopyFromDecId; }
    set CopyFromDecId(value) { this._CopyFromDecId = value; }


    _CopyFromDecId1: string = "1-104502";
    get CopyFromDecId1() { return this._CopyFromDecId1; }
    set CopyFromDecId1(value) { this._CopyFromDecId1 = value; }


    _CopyFromDecId2: string = "1-104502";
    get CopyFromDecId2() { return this._CopyFromDecId2; }
    set CopyFromDecId2(value) { this._CopyFromDecId2 = value; }
    //#endregion
    _COM_ID = "hdefwqjlpk6z_6cwtkessa00000000";///"pkajungqyegfkg6hhqjizw00000000";
    get COM_ID() { return this._COM_ID; }
    set COM_ID(value) { this._COM_ID= value; }

    _Max: number = 30;
    get Max() { return this._Max; }
    set Max(value) { this._Max= value; }

    _current: number = 0;
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
    _LogProccess: string;
    get LogProccess() {
        return this._LogProccess ;
    }
    set LogProccess(value) { this._LogProccess = value; }

    _CustomLoadTest: CustomLoadTest;
    OkButtonClicked() {
        this._current = 0;
        this.DoIt();
    }
    CanStartAgain: boolean=true;
    DoIt() {
        this._current = this._current + 1;
        this.CanStartAgain = false;
        if (this._current < this.Max) {
            this._CustomLoadTest = new CustomLoadTest(this.Consignee, this.CustomerId, this.CopyFromDecId, this.COM_ID ,this);
            
            this._CustomLoadTest.OnLogChange
                .subscribe((logIt) => {
                    this.LogProccess = logIt;
                });
            this._CustomLoadTest.OnFinish
                .subscribe(() => {
                    this.DoIt();
                });
            this._CustomLoadTest.Start();
        } else {
            this.CanStartAgain = true;
            this.LogProccess =("Finish !!!!!!!!!!!!!!!!!!!!!!!");
            this._current = 0;
        }
    }
  
}

enum TestStartes {
    start = 1,
    GetNewCustomFile,
    GetDeclarationFromFileNo,
    GetSingleDeclarationByCustomFileNo,
    PutCopyDeclaration,
    SendDeclarationT1,
    HaveDoc,
}
export class CustomLoadTest {
    _State: TestStartes;

    public OnFinish: EventEmitter<void> = new EventEmitter<void>();
    public OnLogChange: EventEmitter<string> = new EventEmitter<string>();

    private _DeclarationPMService: DeclarationPMService = new DeclarationPMService();
    private _DeclarationListService: DeclarationListService = new DeclarationListService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private _LoadTestService: LoadTestService = new LoadTestService();
    private _DeclarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    private _custDocRelatedDocsWebService: CustDocRelatedDocsWebService = new CustDocRelatedDocsWebService();
    private _DeclarationWebService: DeclarationWebService = new DeclarationWebService();
    private _DeclarationMessagesService: DeclarationMessagesService = new DeclarationMessagesService();

    private _CustomsDocumentPMService: CustomsDocumentPMService = new CustomsDocumentPMService();
    _FileNo: string;
    _DeclarationPM: DeclarationPM;
    _ArrayOfDocumentsFilingPM: Array<DocumentsFilingPM>;
    _CustomsDocumentPM: CustomsDocumentPM;
    Objecttable: any;
    _HaveTicket: boolean = false;
    _parentLoadTestComponent: LoadTestComponent;
    constructor(public Consignee: string, public CustomerId: string, public CopyFromDecId: string, public FilingCopy: string, parentLoadTestComponent: LoadTestComponent ) {
            this.Objecttable = window.ObjectTables.filter(function (x) { return x.Name === "Customs.Declaration"; })[0];
            this._parentLoadTestComponent = parentLoadTestComponent;
    }

    
    _LastError :string= "";
    public Start() {
        this._SendDeclarationCounter = 0;
        this.LogMe("strat");
        this._LoadTestService.GetNewCustomFile(SessionLocator.Tenant, this.Consignee, this.CustomerId)
            .subscribe(rspNewCustomFile => {

                if (rspNewCustomFile == null || rspNewCustomFile.Result == null || rspNewCustomFile.Result.newFileNo==null) {
                    this._LastError = "GetNewCustomFile Failed " + Date.now().toLocaleString();
                    this.Start();
                    return
                }
                let newFileNo = rspNewCustomFile.Result.newFileNo;
                this._FileNo = newFileNo;
                this.LogMe("new file this._FileNo =" + this._FileNo);
                this._LoadTestService.GetDeclarationFromFileNo(SessionLocator.Tenant, newFileNo, this.FilingCopy)
                    .subscribe(rspDeclarationFromFileNo => {

                        if (rspDeclarationFromFileNo == null || rspDeclarationFromFileNo.Result == null || rspDeclarationFromFileNo.Result.returnFileNo==null) {
                            this._LastError = "GetDeclarationFromFileNo Failed " + Date.now().toLocaleString();
                            this.Start();
                            return
                        }
                        let returnFileNo = rspDeclarationFromFileNo.Result.returnFileNo;
                        this.LogMe("Get GetDeclaration done" + this._FileNo);
                        //this._DeclarationListService.get
                        this._DeclarationExtendedListService.GetSingleDeclarationByCustomFileNo(returnFileNo)
                            .subscribe(rspDeclarationByCustomFileNo => {
                                if (rspDeclarationByCustomFileNo == null || rspDeclarationByCustomFileNo.Result == null || rspDeclarationByCustomFileNo.Result.Id==null) {
                                    this._LastError = "GetSingleDeclarationByCustomFileNo Failed " + Date.now().toLocaleString();
                                    this.Start();
                                    return
                                }
                                let declarationList: DeclarationList = rspDeclarationByCustomFileNo.Result;

                                this._DeclarationPMService.get(declarationList.Id)
                                    .subscribe(rsptPMget => {
                                        let declarationPM: DeclarationPM = rsptPMget.Result;
                                        this._DeclarationPM = declarationPM;
                                        this.LogMe("updating GetDeclaration " + this._DeclarationPM.Id);
                                        this._DeclarationPM.Consignments[0].ConsignmentPackages[0].GrossMassMeasure = 321;
                                        this._DeclarationPM.Consignments[0].ConsignmentPackages[0].PackageQuantity = 321;
                                        this._DeclarationPMService.update(declarationPM)
                                            .subscribe(rsptPMupdate => {
                                                this._DeclarationPM = rsptPMupdate.Result;
                                                this.LogMe("Start copy  GetDeclaration from " + this.CopyFromDecId);
                                                this._DeclarationExtendedListService.PutCopyDeclaration(this.CopyFromDecId, declarationList.Id, SessionLocator.Tenant)
                                                    .subscribe(rsptCopyDeclaration => {
                                                        this.LogMe(rsptCopyDeclaration.Result);


                                                        this.HybridUpdateDocFiling();


                                                    });
                                            });
                                        //});
                                    });

                            });


                    });

            });
    }
    HybridUpdateDocFiling() {

        this.LogMe("Check HybridUpdateDocFiling ");
        this._custDocRelatedDocsWebService.GetDocumentsFilingsForRelatedDocuments(this._DeclarationPM.Id, null, this.Objecttable.Id, "I", this._DeclarationPM.CustomFileNo,"")
            .subscribe(rspHaveHybridDoc => {
                var documentsFilingPM: Array<DocumentsFilingPM> = rspHaveHybridDoc.Result;
                this._ArrayOfDocumentsFilingPM = documentsFilingPM;
                this.SendDeclaration();
            });
    }
    _SendDeclarationCounter: number = 0;
    SendDocumentsFiling() {
        this.LogMe("SendDocumentsFiling");
        //http://localhost:9996/api/customsdocuments/getsingle?documentsfilingid=xpi82i%2Bwv0c9xhqyrpxjha00000000
        let documentsfilingid = this._ArrayOfDocumentsFilingPM[0].Id;
        this._CustomsDocumentPMService.get(encodeURIComponent(documentsfilingid))
            .subscribe(rsp => {
                this._CustomsDocumentPM = rsp.Result;

                this._CustomsDocumentPM.DeclarationId = this._DeclarationPM.Id;
                this._CustomsDocumentPM.IsSendToQueue = true;
                this._CustomsDocumentPM.DocumentRemarks = "WhileAnalayzeCostomResponseSendDEC";

                var newValue: CustomsDocumentMetaDataValuePM = new CustomsDocumentMetaDataValuePM(this._CustomsDocumentPM);
                newValue.MetaDataTypeCode = "87"
                newValue.CustomsDocumentId = this._CustomsDocumentPM.DocumentsFilingId;
                newValue.MetaDataValue = "True";
                newValue.Tenant = SessionLocator.Tenant;
                this._CustomsDocumentPM.AddCustomsDocumentMetaDataValue(newValue);
                //var newValue: CustomsDocumentMetaDataValuePM = new CustomsDocumentMetaDataValuePM(this._CustomsDocumentPM);
                //newValue.MetaDataTypeCode = "3"
                //newValue.CustomsDocumentId = this._CustomsDocumentPM.DocumentsFilingId;
                //newValue.MetaDataValue = "321";
                //newValue.Tenant = SessionLocator.Tenant;
                //this._CustomsDocumentPM.AddCustomsDocumentMetaDataValue(newValue);
                this._CustomsDocumentPMService.update(this._CustomsDocumentPM)
                    .subscribe(rspU => {

                        this.OnFinish.emit();
                    });


            });
        ;

    }
    SendDeclaration() {
        let startAt = new Date();
        this.LogMe("SendDeclaration");
        var searchParams: GenericRequestParams = new GenericRequestParams();
        searchParams.Tenant = SessionLocator.Tenant;
        searchParams.AppicationId = this._DeclarationPM.Id;
        searchParams.LoggingEnabled = true;
        searchParams.LoggingEntityId = this._DeclarationPM.Id;
        searchParams.LoggingEntityReference = this._DeclarationPM.DeclarationNumber;
        searchParams.LoggingObjectTableId = this.Objecttable.Id;
        searchParams.LoggingUserId = SessionLocator.LoggedUserId;
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
            .subscribe((response: ServiceResponse) => {
                this._SendDeclarationCounter = this._SendDeclarationCounter + 1;
                let endAt = new Date();



                var t = endAt.getTime() - startAt.getTime();
                t = t / 1000;

                var myDuration = Number(t.toPrecision(2));;
                this._parentLoadTestComponent._SendDecList.push(myDuration);

                //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                if (this._ArrayOfDocumentsFilingPM.length > 0) {
                    if (this._SendDeclarationCounter > 1) {
                        if (!this._HaveTicket) {
                            this.ConnectTicket();
                        } else {
                            //if (this._HaveTicket)
                            this.SendPrintRequest();
                        }
                    } else {

                        this.LogMe("Update&SendDeclaration till 4");
                        this._DeclarationPMService.get(this._DeclarationPM.Id)
                            .subscribe(rsptPMget => {
                                let declarationPM: DeclarationPM = rsptPMget.Result;
                                this._DeclarationPM = declarationPM;

                                this._DeclarationPM.Consignments[0].CargoDescription = this._DeclarationPM.Consignments[0].CargoDescription + this._SendDeclarationCounter.toString();
                                this._DeclarationPMService.update(this._DeclarationPM)
                                    .subscribe(rsptPMupdate => {
                                        this._DeclarationPM = rsptPMupdate.Result;
                                        this.SendDeclaration();
                                    });
                            });
                    }
                } else {
                    this.HybridUpdateDocFiling();
                }
            });
    }
    ConnectTicket() {
        this.LogMe("ConnectTicket");
        this._LoadTestService.GetTicket(SessionLocator.Tenant, this._DeclarationPM.Id)
            .subscribe((response: ServiceResponse) => {
                //this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                if (response.Result == "Ok") {
                    this._HaveTicket = true;
                } else {
                    this.LogMe(response.Result);
                }

                this.SendPrintRequest();

            });
    }
    SendPrintRequest() {
        this.LogMe("SendPrintRequest");
        this._DeclarationPMService.get(this._DeclarationPM.Id)
            .subscribe(rsptPMget => {
                let declarationPM: DeclarationPM = rsptPMget.Result;
                this._DeclarationPM = declarationPM;


                var currRequestParams = new PrintRequestRequestParams();
                currRequestParams.LoggingEnabled = true;
                currRequestParams.LoggingUserId = SessionLocator.LoggedUserId;
                //currRequestParams.RequestVIA = customSendOptionsArgs.RequestVIA;
                //currRequestParams.ForcePersonalSign = customSendOptionsArgs.ForcePersonalSign;
                currRequestParams.Tenant = SessionLocator.Tenant;
                currRequestParams.IsSearchByDeclarationRadio = true;
                currRequestParams.IsSearchByCargoRadio = false;
                currRequestParams.DeclarationNumber = [];

                currRequestParams.DeclarationNumber.push(this._DeclarationPM.DeclarationNumber);


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


                this._DeclarationMessagesService.PostPrintRequestRequest(currRequestParams)
                    .subscribe((myServiceResponse: ServiceResponse) => {
                        this.LogMe("PostPrintRequestRequest");
                        this.SendDocumentsFiling();
                    });
            });
    }
    public LogText: string;
    LogMe(logIt: any) {
        let sendStat = this.GetStaticArray(this._parentLoadTestComponent._SendDecList);
        
        var myTemplateString = `Send :  ${sendStat} 
${logIt}
`;
        console.log(myTemplateString);
        this.LogText = myTemplateString;
        this.OnLogChange.emit(myTemplateString);
    }

    GetStaticArray(ary: number[]) {
        let avrSend = 0;
        let maxSend = 0;
        let last = 0;
        if (!AppTool.IsNullOrEmpty(ary) && ary.length > 0) {
            avrSend = ary.reduce(function (sum, a) { return sum + a }, 0) / (ary.length || 1);
            maxSend = ary.reduce(function (a, b) {
                return Math.max(a, b);
            });
            last = ary[ary.length-1];

            var myTemplateString = ` AVR:  ${avrSend} MAX: ${maxSend} LAST: ${last}`;
            return myTemplateString;
        } else {
            return "None";
        }
    }
}
