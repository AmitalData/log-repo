import { Component, ChangeDetectorRef, OnInit } from '@angular/core';
import { BaseComponent } from '../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { JournalPM } from '../../EntityPMs/JournalPM';
import { EntityResourceService } from '../../../Infrastructure/Services/EntityResourceService';


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

    constructor() {
        super();

        this._entityResourceService.getEntityResourceByTableName("Journal").subscribe((response: any) => { });

    }

    SetUIProperties() {

    }

    SetWindowArgs(args: any) {
        if (args != null) {

        }
    }
}
