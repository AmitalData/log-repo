import { Component } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';
import { ImageLibraryPM } from 'Infrastructure/EntityPMs/ImageLibraryPM';
import { DateTool } from 'Infrastructure/Tools';
import { TenantPM } from 'Common/EntityPMs/TenantPM';
import { ImageLibraryPMService } from 'Infrastructure/Services/StandardPMs/ImageLibraryPMService';
import { Validator } from 'Infrastructure/Validators/Validator';
import { ServiceResponse } from 'Infrastructure/DataContracts/ServiceResponse';

@Component({
    templateUrl: './NewImageLibraryComponent.html',
})


export class NewImageLibraryComponent extends BaseComponent {
    public EntityPM: ImageLibraryPM;
    public DataContext: NewImageLibraryComponent = this;
    public ObjectTableName: string = "ImageLibrary";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    public ImageLibraryPMService: ImageLibraryPMService;
    private CurrentSession = SessionLocator.SelectedSession;

    constructor() {
        super();
        this.ImageLibraryPMService = new ImageLibraryPMService();
        this.SetEntityDefaultValues();
    }

    SetEntityDefaultValues() {
        this.EntityPM = new ImageLibraryPM();
        this.EntityPM.Tenant = SessionLocator.Tenant;
        this.EntityPM.CreateDate = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.CreatedByUserId = SessionLocator.LoggedUserId;
        this.EntityPM.UpdateDate = DateTool.GetCurrentDateAsUtc();
        this.EntityPM.UpdatedByUserId = SessionLocator.LoggedUserId;
    }

    get Name() { return this.EntityPM.Name; }
    set Name(value: string) {
        if (this.EntityPM.Name != value) {
            this.EntityPM.Name = value;
        }
    }

    get ImageId() { return this.EntityPM.ImageDetailId; }
    set ImageId(value: string) {
        if (this.EntityPM.ImageDetailId != value) {
            this.EntityPM.ImageDetailId = value;
        }
    }

    
    ImageUploadedCompleted(imageId) {
        this.ImageId = imageId;
    } 

    CancelButtonClicked() {
        this.CurrentSession.CloseCurrentWindow();
    }

    OkButtonClicked() {
        if (!this.IsEntityValid()) return;
        this.CurrentSession.StartBusyIndicatorSaving();
        this.ImageLibraryPMService.insert(this.EntityPM).subscribe((myResponse: ServiceResponse) => {
            this.OnInsertFinish(myResponse);
        });
    }

    private IsEntityValid(): boolean {
        var errors: string[] = [];
        Validator.TryValidateObject(this.EntityPM, this.ObjectTableName, errors);

        this.ValidationErrorsList = errors;
        return this.ValidationErrorsList.length == 0;
    }

    private OnInsertFinish(myResponse: ServiceResponse) {
        this.CurrentSession.StopBusyIndicator();
        if (myResponse.HasError) {
            this.ValidationErrorsList = myResponse.ErrorsArray;
            return;
        }
        this.CurrentSession.CloseCurrentWindow();
    }
}