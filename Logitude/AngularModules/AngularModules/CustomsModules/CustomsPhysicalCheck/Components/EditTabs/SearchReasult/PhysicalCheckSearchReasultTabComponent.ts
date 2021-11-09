declare var window: any;
import { Component, Input, AfterContentInit, AfterViewInit, ViewChildren, QueryList } from '@angular/core';
import { EntityArgs } from '../../../../../Infrastructure/DataContracts/EntityArgs';
import { DateTool, AppTool, ArrayTool } from '../../../../../Infrastructure/Tools';
import { FeatureLocator } from '../../../../../Infrastructure/Utilities/FeatureLocator';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { LogTab } from '../../../../../Infrastructure/Components/LogitudeComponents/LogTabsComponent';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { PhysicalCheckPM } from '../../../../../Customs/EntityPMs/PhysicalCheckPM';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ObservableCollection } from '../../../../../Infrastructure/Utilities/ObservableCollection';
import { PaymentOrderConnectionTableExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/PaymentOrderConnectionTableExtendedPMService';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';
import { ClientList } from '../../../../../Customs/EntityLists/ClientList';
import { DeclarationWebService } from '../../../../../Customs/Services/WebServices/DeclarationWebService';
import { LogDatePickerComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/LogDatePickerComponent';
import { CustomSendOptionsArgs, RequestParamsBase } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ObjectTablePM } from '../../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { CH_NG_191_MSG2_ChangingTimeRequestParams } from    '../../../../../Customs/DataContract/RequestParams/CH_NG_191_MSG2_ChangingTimeRequestParams';
import { SendRequestVIA } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CH_NG_192_MSG3_ApproveChangeTimeResponseData } from '../../../../../Customs/DataContract/ResponseData/CH_NG_192_MSG3_ApproveChangeTimeResponseData';
import { IIGGeneralMessagesService } from '../../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';
import { PhysicalCheckPMService } from '../../../../../Customs/Services/StandardPMs/PhysicalCheckPMService';
import { GenericRequestParams } from '../../../../../Customs/DataContract/RequestParams/GenericRequestParams';
import { PhysicalCheckWebService } from '../../../../../Customs/Services/WebServices/PhysicalCheckWebService';
import { INF_MSG_GenericResponseData } from '../../../../../Customs/DataContract/ResponseData/INF_MSG_GenericResponseData';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';

@Component({
    selector:'PhysicalCheckSearchReasultTabComponent',
    
    templateUrl: './PhysicalCheckSearchReasultTabComponent.html',
})

export class PhysicalCheckSearchReasultTabComponent
    extends BaseComponent
    implements AfterViewInit, AfterContentInit{
    public DataContext: PhysicalCheckSearchReasultTabComponent = this;
    public EntityPM: PhysicalCheckPM;
    public ObjectTableName: string = "Customs.PhysicalCheck";

    private currentEditComponentId: string;
 
 
    physicalCheckWebService: PhysicalCheckWebService = new PhysicalCheckWebService();
    physicalCheckPMService: PhysicalCheckPMService  = new PhysicalCheckPMService();
    ResponseData: INF_MSG_GenericResponseData;
    ValidationErrors: string[];


    ValidationErrorsList: string[] = [];
    public get ErrorsList() { return this.ValidationErrorsList; }
    public set ErrorsList(val: string[]) {
        this.ValidationErrorsList = val;
    }

    

    get SearchResult() { return this.EntityPM.SearchResult; }
    set SearchResult(value: string) {
        if (this.EntityPM.SearchResult != value) {
            this.EntityPM.SearchResult = value;
        }

        if (value) {
            this.UIProperties.SetWarning("SearchResult", this.ObjectTableName, false);
        }
        else {
            this.UIProperties.SetWarning("SearchResult", this.ObjectTableName, true);
        }

    }


    get SealNumber() { return this.EntityPM.SealNumber; }
    set SealNumber(value: string) {
        if (this.EntityPM.SealNumber != value) {
            this.EntityPM.SealNumber = value;
        }
    }

    get CheckAuthorityAttenderTypeID() { return this.EntityPM.CheckAuthorityAttenderTypeID; }
    set CheckAuthorityAttenderTypeID(value: string) {
        if (this.EntityPM.CheckAuthorityAttenderTypeID != value) {
            this.EntityPM.CheckAuthorityAttenderTypeID = value;
        }
    }

    get CheckAuthorityAttenderTypeName() { return this.EntityPM.CheckAuthorityAttenderTypeName; }
    set CheckAuthorityAttenderTypeName(value: string) {
        if (this.EntityPM.CheckAuthorityAttenderTypeName != value) {
            this.EntityPM.CheckAuthorityAttenderTypeName = value;
        }
    }


    get CheckAnwserStatus() { return this.EntityPM.CheckAnwserStatus == 1 ? "תוצאות בדיקה התקבלו במכס" : ""; }
    

    _InputParam: EntityArgs;
    @Input()
    set PhysicalCheckParam(val: EntityArgs) {
        this.entityArgs = this._InputParam = val;
        this.Init();
    }
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs, private EntityResourceService: EntityResourceService) {
        super();

        this.EntityResourceService.getEntityResourceByTableName("Customs.PhysicalCheck").subscribe((response:any) => {
            this.EntityResourceService.getEntityResourceByTableName("Customs.PaymentOrderLine").subscribe((response:any) => {
                this.Init();
                this.UIProperties.SetEnabled("CheckAnwserStatus", this.ObjectTableName, false);
                               
            });
        });

    }

    Init() {
        if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null)) return;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        if (AppTool.IsNullOrEmpty(this.EntityPM.SearchResult))
            this.UIProperties.SetWarning("SearchResult", this.ObjectTableName, true);

        this.Listen();
    }

    ngAfterViewInit() {
   
    }
    ngAfterContentInit()
    {
  
    }

    OnAddEditWindowClosed(event) {
        this.ValidationErrors = [];
    }
    OnCustomSendOptionsButtonClick(event) {
        this.ValidationErrors = [];
       // this.ValidationErrorsList = [];
         this.CurrentSession.StartBusyIndicator("");
        this.physicalCheckPMService.update(this.EntityPM).subscribe((response: any) => {
            if (AppTool.IsNullOrEmpty(this.EntityPM.SearchResult)) {
                 this.ValidationErrors.push("תוצאת הבדיקה שדה חובה");

                var windowArgs: any = {};
                windowArgs.Errors = this.ValidationErrors;
                windowArgs.ComponentHeight = '328px';  
                var windowTitle = "Error";

                var logWindow = new LogitudeWindow();
                logWindow.Width = 600;
                logWindow.Height = 400;
                logWindow.Title = windowTitle;
                logWindow.ShowCloseButton = false;
                logWindow.WindowArgs = windowArgs;
                logWindow.WindowClosed.subscribe(($event: any) => this.OnAddEditWindowClosed($event));

                logWindow.Show('./CustomsModules/CustomsControls/Components/CustomsErrorsComponent');

                this.CurrentSession.StopBusyIndicator();
                return;
            }
            this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
            this.CurrentSession.CurrentEditComponent.LoadCompleted.emit(true);
            var params: GenericRequestParams = new GenericRequestParams();
            params.Tenant = SessionLocator.Tenant;
            params.RequestVIA = event.RequestVIA;
            params.ForcePersonalSign = event.ForcePersonalSign;
            params.LoggingEnabled = true;
            params.LoggingEntityId = this.EntityPM.Id;
            params.LoggingEntityId2 = this.EntityPM.DeclarationId;
            params.AppicationId = this.EntityPM.Id;
            params.LoggingUserId = SessionLocator.LoggedUserId;
            params.LoggingObjectTableId = window.ObjectTables.filter(d => d.Name === 'Customs.PhysicalCheck')[0].Id;
            params.RequestName = "מענה לבדיקה פיזית";
            params.ResponseName = "מענה לבדיקה פיזית - תשובה"
            CustomMessageProgressComponent
                .ShowProgressBar(params.PBId,
                    "שליחת תוצאות בדיקה", false)
                .then((res) => {
                    this.ResponseData = res;
                }
                ).catch((err) => {
                    this.ValidationErrors.push(err);
                 });
            this.physicalCheckWebService.SendSearchResults(params)
                .subscribe((myServiceResponse: ServiceResponse) => {
                    this.CurrentSession.CurrentEditComponent.ReloadEntityPM();

                });

        });
    }



    private Listen() {
        if (this.CurrentSession.CurrentEditComponent != null) {

            this.currentEditComponentId = this.CurrentSession.CurrentEditComponent.ComponentId;

            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
                    if (isSaveSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
                    if (isLoadSuccess) {
                        this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        //this.BuildAccountingCustomFilesList();
                    }
                })
            );
            this.CurrentSession.CurrentEditComponent.SubscriptionAdd(
                this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
                    if (this.currentEditComponentId == this.CurrentSession.CurrentEditComponent.ComponentId) {
                        if (tabCode == "ANPC") {
                            //this.DisplayOnlyCheck();
                        }
                    }
                })
            );
        }
    }

    // log tab
    selectedTab: LogTab;
    public get SelectedTab() { return this.selectedTab; }
    public set SelectedTab(tab: LogTab) {
        this.selectedTab = tab;
    }
    SetWindowArgs(winArg) {
        this.EntityPM = winArg.EntityPM;
        //this.EntityPM = winArg.declarationPM;
    }
    public SetTabArgs(args: any, valdationErrorList: any[] = null) {
        this.EntityPM = args.EntityPM;
        console.log("EntityPM", this.EntityPM);
    }

     


}
