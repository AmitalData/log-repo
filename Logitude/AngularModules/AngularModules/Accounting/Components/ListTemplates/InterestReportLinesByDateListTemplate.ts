import { AccountingEntityHelper } from './../../Utilities/AccountingEntityHelper';
import { SessionLocator } from './../../../Infrastructure/Utilities/SessionLocator';
import {Component,ChangeDetectorRef} from '@angular/core';
import {AppTool} from '../../../Infrastructure/Tools';

import {ObjectsLocator} from '../../../Infrastructure/Locators/ObjectsLocator';
import { InterestReportLinesByDatePM } from '../../EntityPMs/InterestReportLinesByDatePM';
import { LogitudeWindow } from '../../../Controls/Windows/LogitudeWindow';
import { TextCodeTranslator } from '../../../Infrastructure/Utilities/TextCodeTranslator';
@Component({
    
    templateUrl: "./InterestReportLinesByDateListTemplate.html"
})
export class InterestReportLinesByDateListTemplate {
    public rowData: any;
    public fieldName: any;
    public AdditionalData: any;
    public Source: any;
    public IconCode: string;
    public ColorCode: string;

 
    public isRTL: boolean = false;
    public showLocal: boolean = !SessionLocator.LoggedUserPM.DontShowLocal;
    private CurrentSession = SessionLocator.SelectedSession;
    constructor(private CD: ChangeDetectorRef) {
        if (ObjectsLocator.GlobalSetting)
            this.isRTL = ObjectsLocator.GlobalSetting.LayoutDirection == "rtl";
    }

  

    setVariables(rowData: any, fieldName: string, MyAdditionalData: any) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.AdditionalData = MyAdditionalData;

        //#region Set Icons

        this.IconCode = AccountingEntityHelper.getEntityIcon(this.rowData.SourceTypeCode);

        //#endregion

        var isDestroyed: boolean = this.CD["destroyed"];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    LogWindowShow() {
        var logWindow = new LogitudeWindow();
        logWindow.Title = TextCodeTranslator.Translate("InterestReportLinesByDate.O.InterestDetails");
        var myPath = "./Accounting/Components/Packages/EditTabs/InterestReport/GeneralTab/InterestReportLineByDateDetails/InterestReportLineByDateDetailsComponent";
        logWindow.Width = 950;
        logWindow.Height = 600;
        logWindow.DataContext = this.rowData ;
        logWindow.Show(myPath);
    }

 

    
}
