import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { BaseComponent } from '../../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from '../../../../../Infrastructure/Utilities/SessionLocator';
import { DeclarationList } from '../../../../../Customs/EntityLists/DeclarationList';
import { ServiceResponse } from '../../../../../Infrastructure/DataContracts/ServiceResponse';
import { AppTool } from '../../../../../Infrastructure/Tools';
import { ApiQueryFilters } from '../../../../../Infrastructure/DataContracts/ApiQueryFilters';
import { MessageWindow } from '../../../../../Controls/Windows/MessageWindow';
import { NewEntityArgs } from '../../../../../Infrastructure/Args';
import { LogitudeWindow } from '../../../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../../../Infrastructure/Utilities/TextCodeTranslator';
import { DeclarationPM } from '../../../../../Customs/EntityPMs/DeclarationPM';
import { ConsignmentPM } from '../../../../../Customs/EntityPMs/ConsignmentPM';
import { DeclarationPMService } from '../../../../../Customs/Services/StandardPMs/DeclarationPMService';
import { DeclarationExtendedListService } from '../../../../../Customs/Services/ExtendedLists/DeclarationExtendedListService';
import { CustomsHouseTypeExtendedPMService } from '../../../../../Customs/Services/ExtendedPMs/CustomsHouseTypeExtendedPMService';
import { EntityResourceService } from '../../../../../Infrastructure/Services/EntityResourceService';
import { CustomsSettingListService } from '../../../../../Customs/Services/StandardLists/CustomsSettingListService';

@Component({
    selector: 'NewExportDeclarationComponent',
    
    templateUrl: './NewExportDeclarationComponent.html',
})

export class NewExportDeclarationComponent extends BaseComponent implements OnInit {
    DirectionFilter_E: string;
    public EntityPM: DeclarationPM;
    public DataContext: NewExportDeclarationComponent = this;
    public ObjectTableName: string = "Customs.Declaration";
    public ValidationErrorsList: string[] = [];
    QueryNameText: string = "";
     
    IsTransportModeMatch: boolean = true;

    private declarationPMService: DeclarationPMService = new DeclarationPMService();
    private declarationExtendedListService: DeclarationExtendedListService = new DeclarationExtendedListService();
    private customsHouseTypeExtendedPMService: CustomsHouseTypeExtendedPMService = new CustomsHouseTypeExtendedPMService;
    _CustomsSettingListService: CustomsSettingListService = new CustomsSettingListService();


    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private EntityResourceService: EntityResourceService) {
        super();
        this.EntityPM = new DeclarationPM();
        this.SetDefaultValues();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityResourceService.getEntityResourceByTableName("Customs.CustomsTransportMode").subscribe(response => { });

    }
    SetDefaultValues() {
        this.EntityPM.Direction = "E";
        if (this._CustomsSettingListService == null) {
            this._CustomsSettingListService = new CustomsSettingListService();
        }
        this._CustomsSettingListService.getSingleFromCache(SessionLocator.Tenant.toString())
            .subscribe((customsSettingList: ServiceResponse) => {
                if (customsSettingList != null) {
                    this.EntityPM.AgentId = customsSettingList.Result ? customsSettingList.Result.CustomsAgentId : null;
                }
            });
        this.EntityPM.DeclarationTypeCode = "2";
        this.EntityPM.AgentRoleCode = "A";
    }
    SetWindowArgs(args: any) {
        
    }

    ngOnInit() {

    }

    //#region Properties
    get ExportFile() { return this.EntityPM.ExportFile; }
    set ExportFile(value: string) {
        if (this.EntityPM.ExportFile != value) {
            this.EntityPM.ExportFile = value;
        }
    }
    get CustomerId() { return this.EntityPM.CustomerId; }
    set CustomerId(value: string) {
        if (this.EntityPM.CustomerId != value) {
            this.EntityPM.CustomerId = value;
        }
    }
    get TransportMode() { return this.EntityPM.TransportModeId }
    
    TransportModeClicked(value: string) {
        if (this.EntityPM.TransportModeId != value) {
            this.EntityPM.TransportModeId = value;
        }
    }

    isOkButtonClicked: boolean = false;
    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicator("");
        this.isOkButtonClicked = true;
          var idIndex = this.CurrentSession.GetNewId("RadioButton");
        var errors: string[] = [];
        this.ValidationErrorsList = [];

        if (!this.ExportFile) {
            errors.push("עליך להזין מספר תיק יצוא");
        }

        if (!this.CustomerId) {
            errors.push("עליך להזין לקוח");
        }

        if (!this.TransportMode) {
            errors.push("סוג הובלה - שדה חובה ");
        }
       
        if (errors.length > 0) {
            this.ValidationErrorsList = errors;
            this.CurrentSession.StopBusyIndicator();
            return;
        }

        this.SubmitChanges();
        
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    SubmitChanges() {
        this.CurrentSession.CloseCurrentWindowEmit("ok");
        this.CurrentSession.StopBusyIndicator();
        var Consignment  = new ConsignmentPM(this.EntityPM);

        Consignment.ConsignmentType = 'E';

        if (this.TransportMode == 'A') Consignment.CargoTypeCode = "16";

        this.EntityPM.AddConsignment(Consignment); 

        this.declarationPMService.insert(this.EntityPM).subscribe(myResult => {

            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                var entity = mm.Result;
                SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent',
                    this.CurrentSession.SessionLocation.viewContainerRef)
                    .then(cmpRef => {
                        cmpRef.instance.ComponentRef = cmpRef;
                        cmpRef.instance.Run({ EntityId: entity.Id, ObjectTableName: this.ObjectTableName, BackButtonLabel: this.QueryNameText });
                        cmpRef.instance.BackCompleted.subscribe(($event: any) => {
                            this.CancelButtonClicked();
                        });
                    });
            }
            else {
                this.ValidationErrorsList = mm.ErrorsArray;
                this.CurrentSession.StopBusyIndicator();
            }
        });
    }
}
