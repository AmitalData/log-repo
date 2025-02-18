declare var window: any;
import { Component, ChangeDetectorRef } from '@angular/core';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { TextCodeTranslator } from '../../Utilities/TextCodeTranslator';
import { ConfirmWindow } from '../../../Controls/Windows/ConfirmWindow';
import { EntityResourceService } from '../../Services/EntityResourceService';
import { GeneralLockService } from 'Infrastructure/Services/ExtendedLists/GeneralLockService';

@Component({
    
    templateUrl: './GeneralLockListTemplate.html',
})

export class GeneralLockListTemplate {
    _GeneralLockList: any;
    public fieldName: any;
    private CurrentSession = SessionLocator.SelectedSession;
    
    private _entityResourceService: EntityResourceService = new EntityResourceService();
    constructor(private CD: ChangeDetectorRef) {

    }

    setVariables(GeneralLockList: any, fieldName: string) {
        this._GeneralLockList = GeneralLockList;
        this.fieldName = fieldName;
        this.CD.detectChanges();
    }
   
    CancleGeneralLock(generalKey: string) {

        var confirmWindow = new ConfirmWindow();
        confirmWindow.Title = TextCodeTranslator.Translate('GeneralLock');
        confirmWindow.Width = 350;
        confirmWindow.Height = 200;
        confirmWindow.YesButtonText = TextCodeTranslator.Translate("Customs.General.B.OK");
        confirmWindow.ShowNoButton = false;
        confirmWindow.ShowCancelButton = true;
        confirmWindow.Show(TextCodeTranslator.Translate("GeneralLock.O.IsDeleteGeneralLock"))

        confirmWindow.WindowClosed.subscribe((event: any) => {
            if (confirmWindow.Yes == true) {
                this.CurrentSession.StartBusyIndicator("");
                var generalLockService = new GeneralLockService();
                generalLockService.DeleteGeneralLockByGeneralKey(generalKey).subscribe(myResult => {
                    this.CurrentSession.StopBusyIndicator();
                    if (!myResult.Result) {
                       
                        this.CurrentSession.FireEvent("IsGeneralLockChanged");
                    }
                    this.CD.detectChanges();
        
                }); 
            }   
        }); 
    }

}
