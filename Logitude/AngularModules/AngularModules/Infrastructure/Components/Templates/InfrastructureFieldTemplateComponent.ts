import {Component, ChangeDetectorRef} from '@angular/core';

@Component({
    moduleId: module.id,
    templateUrl: './InfrastructureFieldTemplateComponent.html',
})

export class InfrastructureFieldTemplateComponent {
    public Entity: any = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public ObjectTableName: string = null;
    public SpotlightDataTemplate: string = null;
    public IsSpotLightTemplate: boolean = false;
    constructor(private cd: ChangeDetectorRef) {

    }

    public Run(args: any) {
        this.Entity = args['Entity'];
        this.FieldName = args['FieldName'];
        this.ObjectTableName = args['ObjectTableName'];
        this.IsSpotLightTemplate = args['IsSpotLightTemplate'];
        this.SpotlightDataTemplate = args['SpotlightDataTemplate'];

        if (this.Entity != null && this.FieldName != null) {
            this.FieldValue = this.Entity[this.FieldName];

            if (this.cd) {
                var isDestroyed: boolean = this.cd['destroyed'];
                if (!isDestroyed) {
                    this.cd.detectChanges();
                }
            }
        }
    }
}
