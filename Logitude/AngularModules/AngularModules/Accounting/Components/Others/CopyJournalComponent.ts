import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { JournalPM } from '../../EntityPMs/JournalPM';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';


@Component({
    selector: 'CopyJournalComponent',

    templateUrl: './CopyJournalComponent.html',
})

export class CopyJournalComponent extends BaseComponent {
    public EntityPM: JournalPM;
    public DataContext: any = this;
    public ObjectTableName: string = "Journal";
    public ValidationErrorsList: string[] = [];
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();

        this._entityResourceService.getEntityResourceByTableName("Journal").subscribe((response: any) => { });

    }

    SetUIProperties() {

    }

    SetWindowArgs(args: any) {
        if (args != null) {
            this.EntityPM = args.JournalPM;
        }
    }
    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }
 
    OkButtonClicked() {
      
        var windowTitle = TextCodeTranslator.Translate("Accounting.General.O.NewJournal");


        var entityPM: JournalPM = new JournalPM();
        entityPM.IsNew = true;
        entityPM.JournalLines = this.EntityPM.JournalLines;
        entityPM.TypeCode = "0"; // Manual
        entityPM.AccountingEntityCode = "1"; // Journal

        entityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        entityPM.Tenant = SessionLocator.Tenant;
        SessionLocator.DynamicLoader.Load('./Infrastructure/Components/EditComponent/EditComponent', this.CurrentSession.SessionLocation.viewContainerRef)
            .then(cmpRef => {
                cmpRef.instance.ComponentRef = cmpRef;
                cmpRef.instance.Run({
                    EntityPM: entityPM, ObjectTableName: 'Journal', BackButtonLabel: TextCodeTranslator.Translate("Accounting.General.O.Main")
                   
                });
                this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
                cmpRef.instance.BackCompleted.subscribe(bk => {
                    //this.LoadAllScreenData();
                    ////this.isWindowOpened = false;
                });
            });
    }

}
