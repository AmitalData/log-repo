import {Component, OnInit,ChangeDetectorRef}  from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import {SessionLocator} from '../../../../Infrastructure/Utilities/SessionLocator';
import {AppTool, DateTool} from '../../../../Infrastructure/Tools';
import {ObjectsLocator} from '../../../../Infrastructure/Locators/ObjectsLocator';
import {LogitudeWindow} from '../../../../Controls/Windows/LogitudeWindow';
import {GLAccountListService} from '../../../Services/StandardLists/GLAccountListService'
import { JournalLinePM } from '../../../EntityPMs/JournalLinePM';
import { EntityResourceService } from '../../../../Infrastructure/Services/EntityResourceService';
import { JournalPM } from '../../../EntityPMs/JournalPM';
import { TextCodeTranslator } from '../../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({

    templateUrl: './UpdateJournalLineNoteComponent.html',
})

export class UpdateJournalLineNoteComponent extends BaseComponent{

    private CurrentSession = SessionLocator.SelectedSession;
    public isRTL: boolean = false;
    public EntityPM: JournalLinePM;
    public WarningErrorsList: string[] = [];
    public ValidationErrorsList: string[] = [];
    public ObjectTableName = "JournalLine";
    public DataContext = this;
    public IsResourcesReady: boolean = false;
    public Notes: string = "";
    public line: number;
    public journalLine: JournalLinePM;

    constructor(private entityResourceService: EntityResourceService) {
        super();
        if (ObjectsLocator.GlobalSetting) {
            this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        }
    }

    SetWindowArgs(args: any) {
        this.entityResourceService.getEntityResourceByTableName(this.ObjectTableName).subscribe((res: any) => {
            this.EntityPM = args['journalLine'];
            this.Notes = this.EntityPM.Notes;
            this.IsResourcesReady = true;
        });
    }

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        this.EntityPM.Notes = this.Notes;
        this.CurrentSession.CloseCurrentWindow();
    }


}
