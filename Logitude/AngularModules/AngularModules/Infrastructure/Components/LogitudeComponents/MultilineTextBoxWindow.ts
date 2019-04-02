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
    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    SetWindowArgs(args){
        this.Text = args.TextValue;
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
        this.CurrentSession.CloseCurrentWindowEmit(this.text);
    }
    CancelButtonClicked(){
        this.CurrentSession.CloseCurrentWindowEmit("<!#cancelled>");
    }
}
