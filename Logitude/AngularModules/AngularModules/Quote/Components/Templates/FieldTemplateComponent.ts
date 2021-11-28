

import { Component } from '@angular/core';
import { AppTool } from '../../../Infrastructure/Tools';
import { CRMTool } from '../../../CRM/Tools';

@Component({
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    public SpotlightDataTemplate: string = null;
    public IsSpotLightTemplate: boolean = false;
    public IsInlandDomestic: boolean = false;
    public ImageSrc: string = null;
    constructor() {

    }

    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];

        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];

            if (this.Entity.DirectionId == 'D' && this.Entity.TransportModeId == "I") {
                this.IsInlandDomestic = true;
            }

            else if (this.FieldName == "LastActivityTypeCode") {
                if (!AppTool.IsNullOrEmpty(this.Entity.LastActivityTypeCode)) {
                    this.ImageSrc = CRMTool.GetActivityImageSrc(this.Entity.LastActivityTypeCode);
                }
            }

            else if (this.FieldName == "NextActivityTypeCode") {
                if (!AppTool.IsNullOrEmpty(this.Entity.NextActivityTypeCode)) {
                    this.ImageSrc = CRMTool.GetActivityImageSrc(this.Entity.NextActivityTypeCode);
                }
            }
        }
    }
}
