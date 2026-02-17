import {Component,ChangeDetectorRef} from '@angular/core'; 
import {WebFreightDomainService} from '../../../Infrastructure/Services/WebFreightDomainService';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {OnInit, Output, EventEmitter, ComponentRef, QueryList} from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
//import {JournalExtendedListService} from '../../Services/ExtendedLists/JournalExtendedListService';
import {ServiceResponse} from '../../../Infrastructure/DataContracts/ServiceResponse';
import {AppTool} from '../../../Infrastructure/Tools';
import {ReconcileEventManager} from '../../Utilities/ReconcileEventManager';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { EntityArgs } from '../../../Infrastructure/DataContracts/EntityArgs';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';


@Component({
    moduleId: module.id,
    templateUrl: './ReconcileExternalPageListTemplate.html',
})

export class ReconcileExternalPageListTemplate {

    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;

   public CurrentSession = SessionLocator.SelectedSession;
    public isRTL: boolean = false;


    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
    }

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;

        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }
    ViewEvents(line: any) {
       

        var entityPM = this.rowData;
        var windowArgs: EntityArgs = new EntityArgs();
        windowArgs.ObjectTableName = "ReconcileExternalPage";
        windowArgs.EntityPM = entityPM;
      
        var logWindow = new LogitudeWindow();
        logWindow.Width = 950;
        logWindow.Height = 600;
        logWindow.Title = TextCodeTranslator.Translate("ReconcileExternalPage");
        logWindow.WindowArgs = windowArgs;
       
        this.CurrentSession.SessionEvent.emit("noselect");
        logWindow.Show('./Accounting/Components/EditTabs/BankAccount/BankPageEventsComponent');
    }

    Abs(number: number) {
        return number < 0 ? number * -1 : number;
    }

}
