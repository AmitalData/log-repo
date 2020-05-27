declare var window: any;
declare var System: any;
import {Component, Output, EventEmitter, Input, OnInit, ViewChild, ViewChildren, ViewContainerRef,AfterViewInit} from '@angular/core';
import {ObjectTablePM} from '../../../EntityPMs/ObjectTablePM'
import {MenuButtonPM} from '../../../EntityPMs/MenuButtonPM'
import {MenuButtonGroupPM} from '../../../EntityPMs/MenuButtonGroupPM'
import {EntityArgs} from '../../../DataContracts/EntityArgs';
import {SessionLocator} from '../../../Utilities/SessionLocator'
import {ServiceHelper} from '../../../Utilities/ServiceHelper'
import {AppTool} from '../../../Tools'


@Component({
       
    selector: 'MenuButtonsComponentLoader',
    templateUrl: "./MenuButtonsComponentLoader.html", 
})

export class MenuButtonsComponentLoader implements OnInit,AfterViewInit {

    //-----------------------------inputs---------------------------------------//
    @Input() MenuButton: MenuButtonPM;
    @Input() ObjectTable: ObjectTablePM;
    @Input() EntityPM: any;
    //----------------------------------------------------------------------------//
    @ViewChild('MenuButtonComponent', { read: ViewContainerRef, static: false }) ComponentViewContainerRef: ViewContainerRef; 

    constructor() {
    }

    ngOnInit() {
        
    }
    ngAfterViewInit(){
        if (this.MenuButton.HtmlComponentPath) {
            SessionLocator.DynamicLoader.Load(this.MenuButton.HtmlComponentPath, this.ComponentViewContainerRef).then(cmpRef => {
                cmpRef.instance.Run({ EntityPM: this.EntityPM, ObjectTable: this.ObjectTable });
            });
        }
    }
}