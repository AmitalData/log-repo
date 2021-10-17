import {Component ,Output,EventEmitter} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {Validator} from '../../../Infrastructure/Validators/Validator';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {TenantPM} from '../../../Common/EntityPMs/TenantPM';
import {DateTool, AppTool} from '../../../Infrastructure/Tools';
import {ComputingPartnerPM} from '../../../Common/EntityPMs/ComputingPartnerPM';
import {ObservableCollection} from '../../../Infrastructure/Utilities/ObservableCollection';
import {ComputingPartnerTablePM} from '../../../Common/EntityPMs/ComputingPartnerTablePM';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import {ComputingPartnerPMService} from '../../../Common/Services/StandardPMs/ComputingPartnerPMService';
import {ComputingPartnerTranslationPM} from '../../../Common/EntityPMs/ComputingPartnerTranslationPM';
import {TranslationItem} from '../../../Common/Services/CommonDomainService';
import {ComputingPartnerTranslationPMService} from '../../../Common/Services/StandardPMs/ComputingPartnerTranslationPMService';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
declare var window: any

@Component({
    selector: 'EditTranslationComputingPartners',
    
    templateUrl: './EditTranslationComputingPartners.html',
})

export class EditTranslationComputingPartners extends BaseComponent {
    @Output() BackCompleted = new EventEmitter();
    public DataLoaded: boolean = false;
    public EntityPM: ComputingPartnerTranslationPM;
    public TranslatedEntity: TranslationItem;
    public DataContext: EditTranslationComputingPartners = this;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
        super();
        var service: EntityResourceService = new EntityResourceService();
        service.getEntityResourceByTableName("ComputingPartnerTranslation", 0).subscribe(p => {
        });
    }
    public ObjectTableName: string;

    SetWindowArgs(args: any) {
        this.TranslatedEntity = args.entityPM;
        this.ObjectTableName = this.TranslatedEntity.ObjectTableName;

        var service: ComputingPartnerTranslationPMService = new ComputingPartnerTranslationPMService();    
          var  Id = this.TranslatedEntity.Id;
        if (Id != null) {
            service.get(this.TranslatedEntity.Id).subscribe((p:any) => {
                if (!p.HasError) {
                    this.EntityPM = p.Result;
                    this.SetUiProperties();
                    this.DataLoaded = true;
                }

            });
        }
        else {
            this.EntityPM = new ComputingPartnerTranslationPM();
            this.EntityPM.ComputingPartnerId = this.TranslatedEntity.ComputingPartnerId;
            this.EntityPM.Tenant = SessionLocator.Tenant;
            this.EntityPM.CreateDate = DateTool.GetCurrentDateTimeAsUtc();
            this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
            this.EntityPM.UpdateDate = DateTool.GetCurrentDateTimeAsUtc();
            this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
            this.EntityPM.OurCode = this.TranslatedEntity.OurCode;
            this.EntityPM.PartnerCode = this.TranslatedEntity.PartnerCode;
            this.EntityPM.ComputingPartnerName = this.TranslatedEntity.ComputingPartnerName;
            this.EntityPM.ObjectTableName = this.TranslatedEntity.ObjectTableName;
            this.EntityPM.ObjectTableId = this.TranslatedEntity.ObjectTableId;
            this.FillParentObjectTableDetailsFromChildObjectTable();
            this.SetUiProperties();
            this.DataLoaded = true;
        }

    }

    private FillParentObjectTableDetailsFromChildObjectTable() {
        let childObjectTable = window.ObjectTables.filter(t => t.Name === this.EntityPM.ObjectTableName)[0];
        if (!childObjectTable) return;
        if (AppTool.IsNullOrEmpty(childObjectTable.ParentObjectTableName)) return;
        let parentObjectTable = window.ObjectTables.filter(t => t.Name === childObjectTable.ParentObjectTableName)[0];
        this.EntityPM.ObjectTableName = parentObjectTable?.Name;
        this.EntityPM.ObjectTableId = parentObjectTable?.Id;
    }

    SetUiProperties() {
        this.UIProperties.SetEnabled("OurCode", this.ObjectTableName, false);
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.CurrentSession.StartBusyIndicatorSaving();
        var service: ComputingPartnerTranslationPMService = new ComputingPartnerTranslationPMService();
        if (this.EntityPM.Id != null) {
            if (this.EntityPM.PartnerCode == null)
                this.EntityPM.PartnerCode = "";
            service.update(this.EntityPM).subscribe(p => {
                this.CurrentSession.StopBusyIndicator();
                this.BackCompleted.emit("event");
                this.CurrentSession.CloseCurrentWindow();
            });

        }
        else {
            service.insert(this.EntityPM).subscribe(p => {
                this.CurrentSession.StopBusyIndicator();
                this.BackCompleted.emit("event");
                this.CurrentSession.CloseCurrentWindow();
            });

        }
    }

    public get OurCode() { return this.EntityPM!=null?this.EntityPM.OurCode:null; }
    public get PartnerCode() { return this.EntityPM != null ? this.EntityPM.PartnerCode : null;  }
    public set PartnerCode(value: string) {
        if (value != this.EntityPM.PartnerCode)
            this.EntityPM.PartnerCode = value;
    }







}
