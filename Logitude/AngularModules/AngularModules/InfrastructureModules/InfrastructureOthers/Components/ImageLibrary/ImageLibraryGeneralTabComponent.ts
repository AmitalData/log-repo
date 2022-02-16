import { Component } from '@angular/core';
import { BaseComponent } from 'Infrastructure/Components/LogitudeComponents/BaseComponent';
import { ImageLibraryPM } from 'Infrastructure/EntityPMs/ImageLibraryPM';
import { TenantPM } from 'Common/EntityPMs/TenantPM';
import { ImageLibraryPMService } from 'Infrastructure/Services/StandardPMs/ImageLibraryPMService';
import { EntityArgs } from 'Infrastructure/DataContracts/EntityArgs';

@Component({
    templateUrl: './ImageLibraryGeneralTabComponent.html',
})

export class ImageLibraryGeneralTabComponent extends BaseComponent {
    
    public EntityPM: ImageLibraryPM;
    public DataContext: ImageLibraryGeneralTabComponent = this;
    public ObjectTableName: string = "ImageLibrary";
    public TenantPM: TenantPM;
    public ValidationErrorsList: string[] = [];
    public ImageLibraryPMService: ImageLibraryPMService;

    constructor(public entityArgs: EntityArgs) {
        super();
        this.EntityPM = this.entityArgs.EntityPM;
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

}