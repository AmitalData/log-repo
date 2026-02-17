import {Component} from '@angular/core';
import {CRMTool}from '../../Tools';
import {AppTool} from '../../../Infrastructure/Tools'; 

@Component({
    moduleId: module.id,
    templateUrl: './FieldTemplateComponent.html',
})

export class FieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    public SpotlightDataTemplate: string = null;
    public IsSpotLightTemplate: boolean = false;
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

            if (this.ObjectTableName == "Ticket") {
                if (this.FieldName == "RankCode") {
                    this.SetRanksSource();
                }

                else if (this.FieldName == "LastCompletedActivityTypeCode") {
                    if (!AppTool.IsNullOrEmpty(this.Entity.LastCompletedActivityTypeCode)) {
                        this.ImageSrc = CRMTool.GetActivityImageSrc(this.Entity.LastCompletedActivityTypeCode);
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

    // Rank
    public RankSource1: string = null;
    public RankSource2: string = null;
    public RankSource3: string = null;
    private SetRanksSource() {
        var RankCode = this.Entity['RankCode'];

        switch (RankCode) {
            case "1": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarGray.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
                break;
            }

            case "2": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarOrange.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
                break;
            }

            case "3": {
                this.RankSource1 = "./Images/Icons/StarOrange.png";
                this.RankSource2 = "./Images/Icons/StarOrange.png";
                this.RankSource3 = "./Images/Icons/StarOrange.png";
                break;
            }

            default: {
                this.RankSource1 = "./Images/Icons/StarGray.png";
                this.RankSource2 = "./Images/Icons/StarGray.png";
                this.RankSource3 = "./Images/Icons/StarGray.png";
            }
        }
    }
}
