declare var window: any;
import { Component, Output, EventEmitter, OnInit, ComponentRef } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { FeatureLocator } from '../../../Infrastructure/Utilities/FeatureLocator';
import { AppTool } from '../../../Infrastructure/Tools';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { ListComponentArgs } from '../../../Infrastructure/Args';

import { ApiQueryFilters } from '../../../Infrastructure/DataContracts/ApiQueryFilters';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';





import { ObservableCollection } from '../../../Infrastructure/Utilities/ObservableCollection';
import { InterfaceManagementPM } from '../../../Customs/EntityPMs/InterfaceManagementPM';
import { InterfaceManagementList } from '../../../Customs/EntityLists/InterfaceManagementList';


//import { InterfaceManagementPMService } from '../../../Customs/Services/StandardPMs/InterfaceManagementPMService';
import { InterfaceManagementPMExtendService } from '../../../Customs/Services/ExtendedPMs/InterfaceManagementPMExtendService';

import { InterfaceManagementListService } from '../../../Customs/Services/StandardLists/InterfaceManagementListService';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';


@Component({

    templateUrl: './InterfaceTenantPriorityComponent.html',
})





export class InterfaceTenantPriorityComponent
    extends BaseComponent
    implements OnInit {

    public DataContext: InterfaceTenantPriorityComponent = this;
    public ObjectTableName: string = "Customs.InterfaceManagement";
    public columns: any[] = null;



    //private _InterfaceManagementPMService: InterfaceManagementPMService = new InterfaceManagementPMService();
    private _InterfaceManagementListService: InterfaceManagementListService = new InterfaceManagementListService();
    _InterfaceManagementPMExtendService: InterfaceManagementPMExtendService = new InterfaceManagementPMExtendService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    ///public ComponentRef: ComponentRef<InterfaceTenantPriorityComponent>;

    _TenantInterfaceManagementList: InterfaceManagementList;
    entityPM: InterfaceManagementPM = null;

    ValidationErrorsList: string[] = [];

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
    }
    Loaded: boolean = false;
    public InterfaceTypeList: CodeNameClass[];
    ngOnInit() {
        this.InterfaceTypeList = [];
        this.InterfaceTypeList.push(new CodeNameClass("", "הכל"));
        this.InterfaceTypeList.push(new CodeNameClass("C", "עמילות"));
        this.InterfaceTypeList.push(new CodeNameClass("B", "בלדרות"));
        this.SelectedInterfaceType = this.InterfaceTypeList[0];

        //ערכים NULL==הכל, C==רק עמילות, B==רק בלדרות
        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe(response => {
            this._entityResourceService.getEntityResourceByTableName("Customs.InterfaceTenantDefinition").subscribe((response: any) => {
            });


            //this.RefreshBtnClick()
        });

    }

    private selectedInterfaceType: CodeNameClass;
    get SelectedInterfaceType() { return this.selectedInterfaceType; }
    set SelectedInterfaceType(val) { this.selectedInterfaceType = val; }
    SetWindowArgs(WinArg) {
        ;
        this._TenantInterfaceManagementList = WinArg.SelectedItem;
        this.CurrentSession.StartBusyIndicatorLoading();

        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response: any) => {
            this._entityResourceService.getEntityResourceByTableName("Customs.InterfaceTenantDefinition").subscribe((response: any) => {
                this._InterfaceManagementPMExtendService
                    .GetSingleInterfaceManagementwithDefinition
                    (this._TenantInterfaceManagementList.Code, SessionLocator.Tenant)
                    .subscribe((rsp: any) => {
                        this.CurrentSession.StopBusyIndicator();
                        this.entityPM = rsp.Result;
                        if (!AppTool.IsNullOrEmpty(this.entityPM.InterfaceType)) {
                            this.SelectedInterfaceType = this.InterfaceTypeList.filter(r => r.Code == this.entityPM.InterfaceType)[0];
                        }
                        this.ValidScreen()
                        
                    },);

            });
        });

    }

    SignatureTypeVisibility: boolean = false;
    DcaRenameVisibility: boolean = false;
    ValidScreen() {
        if (this.entityPM.InOut == "O") {
            this.SignatureTypeVisibility = true;
        } else {
            this.DcaRenameVisibility = true;
        }
    }

    ///#region Properties

    get Code() { return this.entityPM != null ? this.entityPM.Code : null; }
    set Code(value) { /*this.entityPM.Code = value*/; }


    //for edit 

    get TenantPriority() { return this.entityPM != null ? this.entityPM.TenantPriority : null; }
    set TenantPriority(value) { this.entityPM.TenantPriority = value; }

    //#endregion


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        let IsNew: boolean = false;//itzik : In  InterfaceManagementUpdateService  Insert/Update Tenant  !!!
        if (IsNew) {
            return;
        }
        this.ValidateRegularUser();
        if (this.ValidationErrorsList.length > 0) {
            return;
        }

        this._InterfaceManagementPMExtendService.PutInterfaceManagementPM(this.entityPM)
            .subscribe((resp: any) => {
                if (resp.HasError) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                    return;
                }
                this.CancelButtonClicked();

            });
    }

    private ValidateRegularUser() {
        this.ValidationErrorsList = [];
        if (this.TenantPriority > 89) {
            this.ValidationErrorsList.push(" עדיפות מוגבלת ל 89")
        }
        if (this.TenantPriority < 11) {
            this.ValidationErrorsList.push(" עדיפות מוגבלת מ 11")
        }
    }
}
