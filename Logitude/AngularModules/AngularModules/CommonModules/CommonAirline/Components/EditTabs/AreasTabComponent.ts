import {Component, OnInit, ChangeDetectorRef} from '@angular/core';
import {AirlinePM} from '../../../../Common/EntityPMs/AirlinePM';
import {EntityArgs} from '../../../../Infrastructure/DataContracts/EntityArgs';
import {PartnersDomainService} from '../../../../Common/Services/PartnersDomainService';
import {TarrifHeaderPM} from '../../../../Common/EntityPMs/TarrifHeaderPM';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {TenantPM} from '../../../../Common/EntityPMs/TenantPM';
import {InfraSettings} from '../../../../Infrastructure/Utilities/InfraSettings';
import {TextCodeTranslator} from '../../../../Infrastructure/Utilities/TextCodeTranslator';
import {UIProperty, UIProperties}  from '../../../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {ServiceResponse} from '../../../../Infrastructure/DataContracts/ServiceResponse';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {EntityResourceService} from '../../../../Infrastructure/Services/EntityResourceService';
import {DateTimeToDatePipe} from '../../../../Controls/Pipes/DateTimeToDatePipe';
import {TarrifFromToTypePM} from '../../../../Common/EntityPMs/TarrifFromToTypePM';
import {TextCodeTranslationPipe} from '../../../../Controls/Pipes/TextCodeTranslationPipe';
import {AppTool, DateTool, FontTool} from '../../../../Infrastructure/Tools';
import {TarrifChargePM} from '../../../../Common/EntityPMs/TarrifChargePM';
import {Cloner} from '../../../../Infrastructure/Utilities/Cloner';
import { AirlineAreaPM } from '../../../../Common/EntityPMs/AirlineAreaPM';
import { ConfirmWindow } from '../../../../Controls/Windows/ConfirmWindow';
@Component({
    moduleId: module.id,
    templateUrl: './AreasTabComponent.html',
})

export class AreasTabComponent extends BaseComponent implements OnInit {
    public EntityPM: AirlinePM;
    public ObjectTableName: string = "Airline";
    public TenantPM: TenantPM;
    public ResourcesReady: boolean = false;
    private _entityResourceService: EntityResourceService = new EntityResourceService();  
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(public entityArgs: EntityArgs) {
        super();
        this._entityResourceService.getEntityResourceByTableName("AirlineArea", 0).subscribe(response => {
            this.ResourcesReady = true;
            this.EntityPM = entityArgs.EntityPM;
            this.TenantPM = SessionLocator.TenantPM;         
        });
    }
    ngOnInit() {
       
    }    

    public AddEditAirlineAreaClicked(EditedEntity: AirlineAreaPM=null) {
        this._entityResourceService.getEntityResourceByTableName("AirlineAreasPort", 0).subscribe(response => {
            var logitudeWindow = new LogitudeWindow();
            if (EditedEntity == null) {
                logitudeWindow.Title = "New Area";
            }
            else {
                logitudeWindow.Title = "Edit Area";
            }
            logitudeWindow.Width = 700;
            logitudeWindow.Height = 650;
            var isNew: boolean = false;
            if (EditedEntity==null) {
                isNew = true;
            }   
            else {
                isNew = false;
            }

            logitudeWindow.WindowArgs = { EntityPM: this.EntityPM, IsNew: isNew, Entity: EditedEntity}
                logitudeWindow.Show("./CommonModules/CommonAirline/Components/AddEdit/AddEditAirlineAreaComponent");
        });
      
    }

    public DeleteAirlineAreaClicked(EditedEntity: AirlineAreaPM = null) {
        if (EditedEntity) {
            var window: ConfirmWindow = new ConfirmWindow();
            window.Show("Are you sure you want to delete this area?");
            window.WindowClosed.subscribe((event: any) => {
                if (window.Yes) {
                    this.EntityPM.RemoveAirlineAreaPM(EditedEntity);
                }
            });
            
        }
    }

}
