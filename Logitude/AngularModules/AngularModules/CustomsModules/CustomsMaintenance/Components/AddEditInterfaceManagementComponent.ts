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
import { InterfaceManagementPM } from '../../../Customs/EntityPMs/InterfaceManagementPM';
import { InterfaceManagementList } from '../../../Customs/EntityLists/InterfaceManagementList';


//import { InterfaceManagementPMService } from '../../../Customs/Services/StandardPMs/InterfaceManagementPMService';
import { InterfaceManagementPMExtendService } from '../../../Customs/Services/ExtendedPMs/InterfaceManagementPMExtendService';

import { InterfaceManagementListService } from '../../../Customs/Services/StandardLists/InterfaceManagementListService';
import { CodeNameClass } from '../../../Infrastructure/DataContracts/CodeNameClass';


@Component({
    
    templateUrl: './AddEditInterfaceManagementComponent.html',
})





export class AddEditInterfaceManagementComponent
    extends BaseComponent
    implements OnInit {

    public DataContext: AddEditInterfaceManagementComponent = this;
    public ObjectTableName: string = "Customs.InterfaceManagement";
    public columns: any[] = null;



    //private _InterfaceManagementPMService: InterfaceManagementPMService = new InterfaceManagementPMService();
    private _InterfaceManagementListService: InterfaceManagementListService = new InterfaceManagementListService();
    _InterfaceManagementPMExtendService: InterfaceManagementPMExtendService = new InterfaceManagementPMExtendService();
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    ///public ComponentRef: ComponentRef<AddEditInterfaceManagementComponent>;

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
    set SelectedInterfaceType(val) {  this.selectedInterfaceType = val; }
    SetWindowArgs(WinArg) {
        ;
        this._TenantInterfaceManagementList = WinArg.SelectedItem;
        this.CurrentSession.StartBusyIndicatorLoading();

        this._entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((response:any) => {
            this._entityResourceService.getEntityResourceByTableName("Customs.InterfaceTenantDefinition").subscribe((response:any) => {
                this._InterfaceManagementPMExtendService
                    .GetSingleInterfaceManagementwithDefinition
                    (this._TenantInterfaceManagementList.Code, SessionLocator.Tenant)
                    .subscribe((rsp:any) => {
                        this.entityPM = rsp.Result;
                        if (!AppTool.IsNullOrEmpty(this.entityPM.InterfaceType)) {
                            this.SelectedInterfaceType = this.InterfaceTypeList.filter(r => r.Code == this.entityPM.InterfaceType)[0];
                        }
                        this.ValidScreen()
                        this.CurrentSession.StopBusyIndicator();
                    });

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

    //IsUnifreightCertificateActivatedEnabled: boolean = true;

    _BolTest: boolean=false;
    get BolTest() {
        return this._BolTest;
    }
    set BolTest(value: boolean) {
        this._BolTest = value;
    }
    SetDcaRenameFileEnable(bol: boolean) {
        this.DcaRenameFileEnable = bol;
    }
    
    get DcaRenameFileEnable() { return this.entityPM != null ? this.entityPM.DcaRenameFileEnable : false; }
    set DcaRenameFileEnable(value: boolean) {
        
        this.entityPM.DcaRenameFileEnable = value;
        
        if (!this.entityPM.DcaRenameFileEnable) {
            this.DcaRenameFilePrefix = null;

            this.UIProperties.SetEnabled("DcaRenameFilePrefix", this.ObjectTableName, false);
        } else {
            this.UIProperties.SetEnabled("DcaRenameFilePrefix", this.ObjectTableName, true);
        }
    }
                                                                                        
    
    get DcaRenameFilePrefix() { return this.entityPM != null ? this.entityPM.DcaRenameFilePrefix: null; }
    set DcaRenameFilePrefix(value) { this.entityPM.DcaRenameFilePrefix= value; }
    
    get SignatureTypeCode() { return this.entityPM != null ? this.entityPM.SignatureTypeCode : null; }
    set SignatureTypeCode(value) { this.entityPM.SignatureTypeCode = value; }


    get Code() { return this.entityPM != null ? this.entityPM.Code : null; }
    set Code(value) { this.entityPM.Code= value; }
    

    get InOut() { return this.entityPM != null ? this.entityPM.InOut : null; }
    set InOut(value) { this.entityPM.InOut = value; }


    get DefaultPriority() { return this.entityPM != null ? this.entityPM.DefaultPriority : null; }
    set DefaultPriority(value) { this.entityPM.DefaultPriority = value; }





    get AllowRestore() { return this.entityPM != null ? this.entityPM.AllowRestore : false; }
    set AllowRestore(value) { this.entityPM.AllowRestore = value; }



    get Description() { return this.entityPM != null ? this.entityPM.Description : null; }
    set Description(value) { this.entityPM.Description = value; }



    get DefaultSendOptionsCode() { return this.entityPM != null ? this.entityPM.DefaultSendOptionsCode : null; }
    set DefaultSendOptionsCode(value) { this.entityPM.DefaultSendOptionsCode = value; }


                   


    get DcaPrefixName() { return this.entityPM != null ? this.entityPM.DcaPrefixName : null; }
    set DcaPrefixName(value) { this.entityPM.DcaPrefixName = value; }




    get DcaPrefixName2() { return this.entityPM != null ? this.entityPM.DcaPrefixName2 : null; }
    set DcaPrefixName2(value) { this.entityPM.DcaPrefixName2 = value; }





    get DcaPrefixName3() { return this.entityPM != null ? this.entityPM.DcaPrefixName3 : null; }
    set DcaPrefixName3(value) { this.entityPM.DcaPrefixName3 = value; }

    get DcaPrefixName4() { return this.entityPM != null ? this.entityPM.DcaPrefixName4 : null; }
    set DcaPrefixName4(value) { this.entityPM.DcaPrefixName4 = value; }



    //for edit 
    
    get TenantPriority() { return this.entityPM != null ? this.entityPM.TenantPriority : null; }
    set TenantPriority(value) { this.entityPM.TenantPriority = value; }

    get TenantSendOptionsCode() { return this.entityPM != null ? this.entityPM.TenantSendOptionsCode : null; }
    set TenantSendOptionsCode(value) { this.entityPM.TenantSendOptionsCode = value; }

    //#endregion


    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        let IsNew: boolean = false;//itzik : In  InterfaceManagementUpdateService  Insert/Update Tenant  !!!
        if (IsNew) {
            return;
        }
        
        this._InterfaceManagementPMExtendService.PutInterfaceManagementPM(this.entityPM)
            .subscribe((resp:any) => {
                if (resp.HasError) {
                    this.ValidationErrorsList = [];
                    this.ValidationErrorsList.push(resp.ErrorsArray[0]);
                    return;
                }
                this.CancelButtonClicked();

            });
    }
}
