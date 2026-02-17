import {Component, AfterViewInit, ChangeDetectorRef, OnInit, Input, Output, EventEmitter}  from '@angular/core';
import {SessionLocator} from '../../../Infrastructure/Utilities/SessionLocator';
import {TextCodeTranslator} from '../../../Infrastructure/Utilities/TextCodeTranslator';
import {AppTool} from '../../../Infrastructure/Tools';
import {ControlsIdCounter} from '../../Utilities/ControlsIdCounter';


@Component({
    moduleId: module.id,

    selector: 'LogBoolean',
    templateUrl: './LogBooleanComponent.html',
})

export class LogBooleanComponent implements OnInit {
    public FilterSelectedValue: string;
    public ControlId: string = null;

    @Input() ObjectFieldName: string = null;
    @Input() ObjectTableName: string = null;
    @Input() DataContext: any;
    @Input() Width: number;
    counterId: number;
    CheckIfExists(IdCom: string) {
        var element = document.getElementById(IdCom);
        if (element != null && element != undefined) {
            return true;
        }
        return false;
    }

    SetControlIds(baseIdCombination: string) {
        this.ControlId = baseIdCombination;
    }

    FilterItemClicked(key: string) {
        if (this.FilterSelectedValue == key) {
            this.FilterSelectedValue = null;
            this.DataContext[this.ObjectFieldName] = null;
        }
        else {
            this.FilterSelectedValue = key;
            if (this.FilterSelectedValue == 'yes') {
                this.DataContext[this.ObjectFieldName] = "True";
            }
            else if (this.FilterSelectedValue == 'no') {
                this.DataContext[this.ObjectFieldName] = "False";
            }
        }
        
        
    }

    ngOnInit() {
        this.counterId = null;
        var baseIdCombination = null;
        if (this.ObjectTableName) {
            baseIdCombination = this.ObjectTableName + "_" + this.ObjectFieldName;
        }
        else {
            baseIdCombination = this.ObjectFieldName;
        }
        if (this.CheckIfExists(baseIdCombination)) {
            this.counterId = ControlsIdCounter.GetNextControlIdCounter(baseIdCombination);
        }

        if (this.counterId != null) {
            baseIdCombination = baseIdCombination + '_' + this.counterId.toString();
        }

        this.SetControlIds(baseIdCombination);

        if (this.DataContext[this.ObjectFieldName] == "True") {
            this.FilterSelectedValue = 'yes';
        }
        else if (this.DataContext[this.ObjectFieldName] == "False") {
            this.FilterSelectedValue = 'no';
        }
        else {
            this.FilterSelectedValue = null;
        }

    }
}