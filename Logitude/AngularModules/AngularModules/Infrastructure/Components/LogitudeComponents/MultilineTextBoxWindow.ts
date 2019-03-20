declare var window: any;
import {Component, Input, OnInit} from '@angular/core';
import {BaseComponent} from './BaseComponent';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {UIProperty, UIProperties, UIPropertyArgs} from './UIProperties';
import {AppTool} from '../../Tools';
import {ObjectsLocator} from '../../Locators/ObjectsLocator';

@Component({
    moduleId: module.id,

    selector: 'MultilineTextBoxWindow',
    templateUrl: "./MultilineTextBoxWindow.html",
})

export class MultilineTextBoxWindow implements OnInit {
    public DataContext: any;
    DisplayMode: boolean = false;

    constructor() {
    }

    SetWindowArgs(args){
        this.Text = args.TextValue;
        if (args.DisplayMode) {
            this.DisplayMode = args.DisplayMode;
        }
    }

    private text: string;
    public get Text() { return this.text; }
    public set Text(newValue: string) {
        if (this.text != newValue) {
            this.text = newValue;
        }
    }


    ngOnInit() {}

    OkButtonClicked(){
        SessionLocator.CurrentSession.CloseCurrentWindowEmit(this.text);
    }
    CancelButtonClicked(){
        SessionLocator.CurrentSession.CloseCurrentWindowEmit("<!#cancelled>");
    }
}
