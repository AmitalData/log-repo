import {Component, Output, EventEmitter, OnInit, AfterViewInit, ChangeDetectorRef} from '@angular/core';
import {BaseComponent} from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {LedgerTransactionPM} from '../../EntityPMs/LedgerTransactionPM';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import {EntityResourceService} from '../../../Infrastructure/Services/EntityResourceService';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import { LedgerTransactionPMService } from '../../Services/StandardPMs/LedgerTransactionPMService';

@Component({
    selector: 'LedgerTransactionInternalNotesComponent',
    moduleId: './Accounting/Components/Others/',
    templateUrl: 'LedgerTransactionInternalNotesComponent.html',
})

export class LedgerTransactionInternalNotesComponent extends BaseComponent {

     public ledgerTransactionPMService: LedgerTransactionPMService;

    private CurrentSession = SessionLocator.SelectedSession;
    public isRTL: boolean = false;
    public EntityPM: LedgerTransactionPM;
    public WarningErrorsList: string[] = [];
    public ValidationErrorsList: string[] = [];
    public ObjectTableName: string = "LedgerTransaction";
    public DataContext = this;
    public IsResourcesReady: boolean = false;
    public InternalNote: string = "";
    public ledgerTransactionPM: LedgerTransactionPM;


    constructor(private entityResourceService: EntityResourceService,private CD: ChangeDetectorRef) {
        super();
        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
        this.ledgerTransactionPMService = new LedgerTransactionPMService();
    }


    SetWindowArgs(args: any) {
       var loggedContact = SessionLocator.LoggedUserPM;
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.EntityPM = args['ledgerTransaction'];
            this.InternalNote=this.EntityPM.InternalNote;
            this.EntityPM.UpdateDateTime = (this.EntityPM.UpdateDateTime && (this.InternalNote!=null))?this.EntityPM.UpdateDateTime:new Date();
            this.EntityPM.UpdatedByUserName = this.EntityPM.UpdatedByUserName?this.EntityPM.UpdatedByUserName:(loggedContact.DontShowLocal ? loggedContact.EnglishName : (loggedContact.LocalName||loggedContact.EnglishName));
            this.IsResourcesReady = true;           
        });

    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        var loggedContact = SessionLocator.LoggedUserPM;
        this.CurrentSession.StartBusyIndicatorLoading();
        this.EntityPM.InternalNote = this.InternalNote;
        this.EntityPM.UpdateDateTime = new Date();
        this.EntityPM.UpdatedByUserName = loggedContact.DontShowLocal ? loggedContact.EnglishName : (loggedContact.LocalName||loggedContact.EnglishName);          
        this.ledgerTransactionPMService.update(this.EntityPM).subscribe((myResult: ServiceResponse) => {
            this.CurrentSession.StopBusyIndicator();
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.EntityPM = mm.Result;
            }
            this.CD.detectChanges();
            this.CurrentSession.CloseCurrentWindow();           
        });
    }
}
