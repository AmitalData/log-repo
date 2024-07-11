import { ChangeDetectorRef, Component, ViewChild } from "@angular/core";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { LogTexBoxFormComponent, TextBoxField, ValueChange } from "InfrastructureModules/InfrastructureOthers/AmitalAPI/components/LogTexBoxFormComponent";
import { Subject } from "rxjs";
import { filter } from "rxjs/operators";

@Component({
    selector: 'LogDefaultAndConfigurationDetailsTab',
    template: `
        <log-text-box-form #form [fields]='fields' [DataContext]='DataContext' [dir]="'ltr'" (changeEvent)='$setKeyChange.next($event)'></log-text-box-form>
    `,
    styles: [` 
            :host ::ng-deep .form-data-field-field { 
                flex: 0 0 410px !important;
            }
            
            :host ::ng-deep .form-data-field-field LogLabel {
                flex: 0 0 100px !important;
            }    
        `],
})
export class DefaultAndConfigurationDetailsTabComponent {
    @ViewChild('form') form!: LogTexBoxFormComponent;

    constructor(private readonly cd: ChangeDetectorRef) {}

    ngOnInit() {    }

    $setKeyChange: Subject<ValueChange>;
    DataContext = { UIProperties: new UIProperties() };
    fields: TextBoxField[] = [
        { name: 'Is_Active', label: 'Is Active', type: 'boolean', value: true },
        { name: 'StoreInCache', label: 'Store In Cache', type: 'boolean', value: true },
        { name: 'SetKey', label: 'Set Key', required: true, type: 'logLov', params: { LookUpTableName: 'DefaultAndConfigurationKey' } },
        { name: 'AdditionalKey', label: 'Additional Key', required: true },
        { name: 'SortOrder', label: 'Sort Order', type: 'number', required: true },
        { name: 'AllowInheritance', label: 'Allow Inheritance', type: 'boolean' },
    ];

    SetTabArgs({ EntityPM }) {
        this.DataContext = EntityPM.dataContext;
        this.$setKeyChange = EntityPM.$setKeyChange;
        this.$setKeyChange.pipe(filter(vc => vc.field === '')).subscribe((valueChange: ValueChange) => this.DataContext = valueChange.value);
        this.cd.detectChanges();
        EntityPM.forms.form = this.form;
    }
}