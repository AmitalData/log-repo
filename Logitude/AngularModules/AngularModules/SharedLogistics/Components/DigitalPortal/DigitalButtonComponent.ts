declare var window: any;
import { Component, ChangeDetectorRef } from '@angular/core';
import { TenantPM } from '../../../Common/EntityPMs/TenantPM';
import { InfraSettings } from '../../../Infrastructure/Utilities/InfraSettings';
import { SessionLocator } from '../../../Infrastructure/Utilities/SessionLocator';
import { EntityListService } from '../../../Infrastructure/Services/EntityListService';
import { DigitalCustomizationService, AddCustomFieldRequest } from '../../../Infrastructure/Services/WebServices/DigitalCustomizationService';
import { AppTool } from '../../../Infrastructure/Tools';

@Component({
    selector: 'DigitalButtonComponent',
    templateUrl: './DigitalButtonComponent.html',
})

export class DigitalButtonComponent {

    public rowData: any;
    public fieldName: any;
    public TenantPM: TenantPM;
    public InUseVisibile: boolean = true;
    private CurrentSession = SessionLocator.SelectedSession;
    private digitalCustomizationService: DigitalCustomizationService;
    private objectTableId: string;
    private parentObjectTableId: string;
    private profileId: string;
    private profileCode: string;
     
    constructor(private CD: ChangeDetectorRef, private _entityListService: EntityListService) {
        this.TenantPM = InfraSettings.TenantPM;
        this.digitalCustomizationService = new DigitalCustomizationService();
    }

    setVariables(rowData: any, fieldName: string) {
        this.rowData = rowData;
        this.fieldName = fieldName;
        this.fieldName = fieldName.split(",");
        this.objectTableId = this.fieldName[0];
        this.parentObjectTableId = this.fieldName[1];
        this.profileId = this.fieldName[2];
        this.profileCode = this.fieldName[3];

        this.InUseVisibile = this.rowData.InUse;
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
    }

    AddObjectFieldClicked() {
        this.StartBusyIndicator("Adding field to your list");
        this.AddObjectField();
    }


    RefreshDateUpdated() {
        this.InUseVisibile = true;
        var isDestroyed: boolean = this.CD['destroyed'];
        if (!isDestroyed) {
            this.CD.detectChanges();
        }
        this.CurrentSession.SessionEvent.emit({ Name: "ReloadDigitalPortalPermissions" });
    }

    public FireEvent(eventArgs: any) {
        this.CurrentSession.SessionEvent.emit(eventArgs);
    }

    AddObjectField() {
        var newField = new AddCustomFieldRequest();
        newField.ObjectTableId = this.objectTableId;
        newField.ParentObjectTableId = this.parentObjectTableId;
        newField.ProfileId = this.profileId;
        newField.ProfileCode = this.profileCode;
        newField.FieldCode = this.rowData.FieldCode;
        newField.TextCode = this.rowData.FullNameTextCodeCode;
        newField.DefaultText = this.rowData.FullNameTextCodeDefaultText;
        newField.DisplayText = this.rowData.FullNameTextCodeDefaultText;
        newField.CreatedBy = SessionLocator.LoggedUserPM.EnglishName;
        newField.ModifiedBy = SessionLocator.LoggedUserPM.EnglishName;
        newField.IsList = this.rowData.DisplayInList;
        newField.IsPm = !AppTool.IsNullOrEmpty(this.rowData.PMPropertyPath);
        this.digitalCustomizationService.AddCustomField(newField).subscribe((myResult) => {
            this.RefreshDateUpdated();
            this.StopBusyIndicator();
        });
    }

    private StartBusyIndicator(message: string) {
        this.CurrentSession.StartBusyIndicator(message);
    }

    private StopBusyIndicator() {
        this.CurrentSession.StopBusyIndicator();
    }
}
