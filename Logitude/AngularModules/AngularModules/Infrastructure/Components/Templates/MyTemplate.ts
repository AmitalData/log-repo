declare var window: any;
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { ObjectTablePM } from '../../EntityPMs/ObjectTablePM';
import { ObjectFieldPM } from '../../EntityPMs/ObjectFieldPM';
import { ObjectsLocator } from '../../Locators/ObjectsLocator';
import { ChildDirective } from '../../Directives/ChildDirective';
import { SessionLocator } from '../../Utilities/SessionLocator';
import { AppTool } from '../../Tools';

@Component({
    selector: 'mytemplate',
    templateUrl: 'MyTemplate.html',
    inputs: ['ObjectTable', 'ObjectField', 'FieldName', 'Entity', 'IsHeaderScreenTemplate', 'IsListColumnCellTemplate', 'IsListColumnHeaderTemplate','FieldValue'],
})
export class MyTemplate implements OnInit {
    public Entity: any = null;
    public CustomField: any = null;
    public ObjectTable: ObjectTablePM = null;
    public ObjectField: ObjectFieldPM = null;
    public FieldName: string = null;
    public FieldValue: any = null;
    public HasTemplate: boolean = false;
    public DataTypeCode: string = null;
    public DigitsAfterPoints: string = "n0";
    public IsAutoFormat: boolean = false;
    public IsLookUp: boolean = false;
    public LookUpFieldValue: string = null;
    public IsHeaderScreenTemplate: boolean = false;
    public IsListColumnCellTemplate: boolean = false;
    public IsListColumnHeaderTemplate: boolean = false;
    public Direction: string = ObjectsLocator.GlobalSetting == undefined ? "ltr" : ObjectsLocator.GlobalSetting.LayoutDirection;
    public TextAlign = this.Direction == 'rtl' ? 'right' : 'left';
    public NumberFieldTextAlign: string = "right";
    public isRTL: boolean = false;
    private CurrentSession = SessionLocator.SelectedSession;
    //@ViewChild(ChildDirective) Child: ChildDirective;
    public ShowChildTemplate: boolean = false;

    constructor(private changeDetector: ChangeDetectorRef) {
    }

    ngOnInit() {
        //this.FieldValue = this.Entity[this.ObjectField.FieldName];
        this.changeDetector.detectChanges();
    }

}
