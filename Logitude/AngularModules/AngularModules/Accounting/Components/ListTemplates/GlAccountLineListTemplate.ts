import { SessionLocator } from './../../../Infrastructure/Utilities/SessionLocator';
import {Component,ChangeDetectorRef} from '@angular/core';
import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { AccountingNotePMService } from './../../Services/StandardPMs/AccountingNotePMService';
import { AccountingNotePM } from './../../EntityPMs/AccountingNotePM';
import { AccountingNoteExtendedListService } from './../../Services/ExtendedLists/AccountingNoteExtendedListService';
import { AccountingNoteList } from './../../EntityLists/AccountingNoteList';
import { ServiceResponse } from '../../../Infrastructure/DataContracts/ServiceResponse';
import {GLAccountPM} from '../../EntityPMs/GLAccountPM';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
import { MessageWindow } from './../../../Controls/Windows/MessageWindow';
import { DateTool } from './../../../Infrastructure/Tools';
import {EntityArgs} from '../../../Infrastructure/DataContracts/EntityArgs';

@Component({

    templateUrl: "./GlAccountLineListTemplate.html"
})
export class GlAccountLineListTemplate {

    public isRTL: boolean = false;
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    private CurrentSession = SessionLocator.SelectedSession;
    public EntityPM: GLAccountPM = null;
    public isUsedOutside: boolean = false; // when view tab inside customer ..

    _AccountingNotePMService: AccountingNotePMService = new AccountingNotePMService();
    _AccountingNoteExtendedListService: AccountingNoteExtendedListService = new AccountingNoteExtendedListService();

    constructor(private entityArgs: EntityArgs,private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting)
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";

        // Set Entity
        if(entityArgs && entityArgs.ObjectTableName == "GLAccount")
        {
            this.EntityPM = entityArgs.EntityPM;
            
        }
        else
        {
            this.isUsedOutside = true;
        }
        //this.Listen();  
    }

    // private SaveCompletedEvent: any = null;
    // private LoadCompletedEvent: any = null;
    // private TabSelectedEvent: any = null;
    // Listen() {
    //     if (this.CurrentSession.CurrentEditComponent != null) {
    //         if (this.SaveCompletedEvent == null) {
    //             this.SaveCompletedEvent = this.CurrentSession.CurrentEditComponent.SaveCompleted.subscribe((isSaveSuccess: boolean) => {
    //                 if (isSaveSuccess) {
    //                     this.CurrentSession.CurrentEditComponent.ReloadEntityPM();
    //                     this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        
    //                 }
    //             });
    //         }

    //         if (this.LoadCompletedEvent == null) {
    //             this.LoadCompletedEvent = this.CurrentSession.CurrentEditComponent.LoadCompleted.subscribe((isLoadSuccess: boolean) => {
    //                 if (isLoadSuccess) {
    //                     this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
    //                     console.log("Entity Reloaded");
                      
    //                 }
    //             });
    //         }


           
    //         if (this.TabSelectedEvent == null) {
    //             this.TabSelectedEvent = this.CurrentSession.CurrentEditComponent.TabSelected.subscribe((tabCode: string) => {
    //                 if (tabCode == "GAOV") {
    //                     this.EntityPM = this.CurrentSession.CurrentEditComponent.EntityPM;
                        
    //                 }
    //             });
    //         }
    //     }
    // }
   
   

    //#region Accounting Notes
    accountingNotesList: AccountingNoteList[] = [];
    isNotesLoading:boolean = false;

    GetAccountingNotes(){
        if(this.EntityPM.CardId){
            this.accountingNotesList = [];
            this.isNotesLoading = true;
            // setTimeout(() => {

            this._AccountingNoteExtendedListService.GetNotesByCard(this.EntityPM.CardId)
                .subscribe((res:ServiceResponse) =>
                {
                        this.isNotesLoading = false;


                    if(res.HasError){
                        var msg = new MessageWindow();
                        msg.Show("Get Accounting Note error: " + res.ErrorsArray[0]);
                    }else{
                        var notesList = res.Result;
                        this.accountingNotesList = notesList;
                        
                    }
                });
            // }, 2000);

        }

    }
    OpenAccountingNote(notePM: AccountingNotePM){

        var windowArgs: any = {};
        windowArgs.EntityPM = this.EntityPM;
        windowArgs.AccountingNotePM = notePM;

        var logitudeWindow = new LogitudeWindow();
        logitudeWindow.Width = 450;
        logitudeWindow.Height = 350;
        logitudeWindow.Title = notePM ? '' : TextCodeTranslator.Translate("Accounting.O.NewAccountingNote");

        logitudeWindow.WindowArgs = windowArgs;
        logitudeWindow.Show('./Accounting/Components/Others/AccountingNoteComponent');
        logitudeWindow.WindowClosed.subscribe(($event: any) => {
            this.GetAccountingNotes();
        });

    }
    // ItemEditButton(_noteList: AccountingNoteList){

    //     this.CurrentSession.StartBusyIndicatorLoading();

    //     this._AccountingNotePMService.get(_noteList.Id)
    //         .subscribe((myResult:any) => {
    //             var mm: ServiceResponse = myResult;
    //             if (!mm.HasError) {
    //                 var _notePM = mm.Result;
    //                 this.OpenAccountingNote(_notePM);
    //                 this.CurrentSession.StopBusyIndicator();


    //             }
    //             else {
    //                 this.CurrentSession.StopBusyIndicator();
    //             }
    //         });


    // }
    
    // txt_updatedBy: string = TextCodeTranslator.Translate("AccountingNote.F.UpdatedByUserName");
    // GetNoteTitle(note:AccountingNoteList){
    //     var result = "";
    //     if(note){
    //         var myFormats = DateTool.GetDateFormats(note.UpdateDate);
    //         var formatedDate = myFormats.DateString + " " + myFormats.ShortTimeString;
    //         result = this.txt_updatedBy + ' ' + note.UpdatedByUserName + ' (' + formatedDate + ') ';
    //     }
    //     return result;
    // }
    //#endregion

}
