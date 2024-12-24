import { Component, Input, QueryList, ViewChildren } from "@angular/core";
import { UIProperties } from "Infrastructure/Components/LogitudeComponents/UIProperties";
import { WrapperLogFieldComponent } from "InfrastructureModules/InfrastructureOthers/AmitalAPI/components/WrapperLogFieldComponent";
import { fieldsError } from "InfrastructureModules/InfrastructureOthers/AmitalAPI/WindowsComponent/AmitalAPIAddWindowService";

@Component({
    selector: 'app-field-by-type',
    template: `
        <ng-container *ngIf='!isArray'>
            <wrapper-log-field #field [dir]='dir' [type]='_type' [name]='name' [DataContext]='_DataContext' [required]='required'></wrapper-log-field>                           
        </ng-container>

        <ng-container *ngIf='isArray'>
            <img src='Images/Buttons/Add.png' (click)='arr.push(null)' style='cursor: pointer; margin: 0 5px 0 0;'>
            <div class='values-container'>
                <div *ngFor='let item of arr; let i = index' style='display: flex'>
                    <img src='Images/RedX.png' (click)='arr.splice(i, 1)' style='cursor: pointer; padding: 4px;'>
                    <wrapper-log-field #field [dir]='dir' [type]='_type' [value]='item' [required]='required'></wrapper-log-field>
                </div>
            </div>
        </ng-container>
    `,
    styles: [``],
})
export class FieldByTypeComponent {
    @ViewChildren('field') fields!: QueryList<WrapperLogFieldComponent>;
    _type: string = 'System.String';
    isArray: boolean = false;
    arr: any[] = [];
    @Input() name: string;
    @Input() dir: string = 'rtl';
    @Input() required: boolean = false; 
    @Input() set type(t: string) {
        this.arr = Array(1);
        this.isArray = t.endsWith('[]');
        const type = this.isArray ? t.replace('[]', '') : t;
        this._type = this.convertType(type);    
    }
    _DataContext: any = { UIProperties: new UIProperties() };
    @Input() set DataContext(d: any) {
        this._DataContext = d || { UIProperties: new UIProperties() };

        if (d[this.name] && this.isArray)
            this.arr = d[this.name].split(';');
    }

    public get value(): any {
        return this.isArray ? this.fields.toArray().map(x => x.Value).join(';') : this.fields.first.Value;
    }

    public get valid(): boolean {
        return this.isArray ? this.fields.toArray().every(x => x.valid) : this.fields.first.valid;
    }

    public get errors(): fieldsError[] {
        let result: fieldsError[] = [];

        if(this.isArray)  
            this.fields.toArray().forEach(x => result = result.concat(x.errors));
        else
            result = this.fields.first.errors;

        return result;
    }

    convertType(type: string): string {
        switch (type) {
            case 'System.String':
                return 'text';
            case 'System.Int':
            case 'System.Int32':
            case 'System.Double':
            case 'System.Double32':
                return 'number';                
            case 'System.Boolean':
                return 'boolean';
            case 'System.DateTime':
                return 'date';
            default:
                return 'text';
        }
    }
}