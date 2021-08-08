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
import { CustomSendOptionsArgs } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { ObjectTablePM } from '../../../../../Infrastructure/EntityPMs/ObjectTablePM';
import { CH_NG_191_MSG2_ChangingTimeRequestParams } from    '../../../../../Customs/DataContract/RequestParams/CH_NG_191_MSG2_ChangingTimeRequestParams';
import { SendRequestVIA } from '../../../../../Customs/DataContract/RequestParams/RequestParamsBase';
import { CH_NG_192_MSG3_ApproveChangeTimeResponseData } from '../../../../../Customs/DataContract/ResponseData/CH_NG_192_MSG3_ApproveChangeTimeResponseData';
import { IIGGeneralMessagesService } from '../../../../../Customs/Services/WebServices/IIGGeneralMessagesService';
import { CustomMessageProgressComponent } from '../../../../../CustomsModules/CustomsControls/Components/CustomMessageProgressComponent';
import {EntityResourceService} from '../../../../../Infrastructure/Services/EntityResourceService';

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
 
    _IIGGeneralMessagesService: IIGGeneralMessagesService = new IIGGeneralMessagesService();

    

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


    get CheckAnwserStatus() { return this.EntityPM.CheckAnwserStatus; }
    set CheckAnwserStatus(value: number) {
        if (this.EntityPM.CheckAnwserStatus != value) {
            this.EntityPM.CheckAnwserStatus = value;
        }
    }

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
              
                               
            });
        });

    }

    Init() {
        if (this.entityArgs == null || (this.entityArgs != null && this.entityArgs.EntityPM == null)) return;
        this.EntityPM = this.entityArgs.EntityPM;
        this.ObjectTableName = this.entityArgs.ObjectTableName;
        this.Listen();
    }

    ngAfterViewInit() {
   
    }
    ngAfterContentInit()
    {
  
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
