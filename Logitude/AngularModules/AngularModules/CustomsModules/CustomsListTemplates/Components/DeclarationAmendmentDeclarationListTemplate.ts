import {Component, ChangeDetectorRef} from '@angular/core';
import {ServiceArgs} from '../../../Infrastructure/DataContracts/ServiceArgs';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {AppTool} from '../../../Infrastructure/Tools';
import {LogitudeWindow} from '../../../Controls/Windows/LogitudeWindow';
import { CourierMasterPM } from '../../../Customs/EntityPMs/CourierMasterPM';
import { CourierMasterValidator } from '../../../Customs/Validators/CourierMasterValidator';
import { CustomsRequestsSheetPM } from '../../../Customs/EntityPMs/CustomsRequestsSheetPM';
import { DeclarationPM } from '../../../Customs/EntityPMs/DeclarationPM';
import { DeclarationList } from '../../../Customs/EntityLists/DeclarationList';

@Component({
    moduleId: module.id,
    templateUrl: './DeclarationAmendmentListTemplate.html',
})

export class DeclarationAmendmentListTemplate {

    public rowData: DeclarationList;
    public fieldName: any;
    fontcolor: string;
    entityPM: DeclarationPM;
    IsConnectedDeclarationChecked: boolean = true;
    IsNotConnectedDeclarationChecked: boolean = false;

    public IsDisplayOnly: boolean = false;
    public color: string;
    constructor(private CD: ChangeDetectorRef) {
        
    }

    setVariables(DeclarationListRecord: DeclarationList, fieldName: string, additionalData: any)
    {
        this.rowData = DeclarationListRecord;
        this.fieldName = fieldName;
        this.entityPM = SessionLocator.SelectedSession.CurrentEditComponent.EntityPM as DeclarationPM;
          this.CD.detectChanges();
    }
 




}
