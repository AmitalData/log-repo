import {Component, ChangeDetectorRef, OnDestroy} from '@angular/core';
import {BaseComponent} from '../../../../Infrastructure/Components/LogitudeComponents/BaseComponent';
import { OpenFormatReportPM } from '../../../EntityPMs/OpenFormatReportPM';
import { EntityArgs } from '../../../../Infrastructure/DataContracts/EntityArgs';
import { ObjectsLocator } from '../../../../Infrastructure/Locators/ObjectsLocator';
import { SessionLocator } from '../../../../Infrastructure/Utilities/SessionLocator';
declare var window: any;
import { DocumentsFilingViewsExtService } from '../../../../Common/Services/ExtendedLists/DocumentsFilingViewsExtService';
import { ServiceResponse } from '../../../../Infrastructure/DataContracts/ServiceResponse';
import { DownloadManager } from '../../../../Infrastructure/Utilities/DownloadManager';




@Component({
    selector: 'OpenFormatReportLogTabComponent',
    
    templateUrl: './OpenFormatReportLogTabComponent.html',
})


export class OpenFormatReportLogTabComponent extends BaseComponent{
  public DataContext: any = this;
    ObjectTableName: string = "OpenFormatReport";
    entityPM: OpenFormatReportPM;
    isRTL: boolean = false;
    showLocals: boolean = false;
    _DocumentsFilingViewsExtService: DocumentsFilingViewsExtService = new DocumentsFilingViewsExtService();
    docFilingPM: any;

    constructor(private entityArgs: EntityArgs) {
        super();
        this.entityPM = entityArgs.EntityPM;
        if (ObjectsLocator.GlobalSetting) this.isRTL = (ObjectsLocator.GlobalSetting.LayoutDirection == "rtl");
        this.showLocals = !SessionLocator.LoggedUserPM.DontShowLocal;
        this.UIProperties.SetEnabled("ErrorMessage","OpenFormatReport",false)
    }

    get ErrorMessage() { return this.entityPM.ErrorMessage; }

    DownloadButtonClicked() {
        this.GetDocument();
    }


    GetDocument() {

        var objectTable = window.ObjectTables.filter(d => d.Name === this.ObjectTableName)[0];



        this._DocumentsFilingViewsExtService.GetLastDocumentsFilingPM(this.entityPM.Id, objectTable.Id).subscribe((myResult:any) => {
            console.log("[GetLastDocumentsFilingPM]", myResult);
            var mm: ServiceResponse = myResult;
            if (!mm.HasError) {
                this.docFilingPM = mm.Result;

                DownloadManager.DownloadPage(null, this.docFilingPM.SecurityId);
            }
        });


    }
}
