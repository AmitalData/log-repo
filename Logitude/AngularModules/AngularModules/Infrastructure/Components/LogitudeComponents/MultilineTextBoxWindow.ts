declare var window: any;
import {Component, Input, OnInit} from '@angular/core';
import {BaseComponent} from './BaseComponent';
import {SessionLocator} from '../../Utilities/SessionLocator';
import {TextCodeTranslator} from '../../Utilities/TextCodeTranslator';
import {UIProperty, UIProperties, UIPropertyArgs} from './UIProperties';
import {AppTool} from '../../Tools';
import {ObjectsLocator} from '../../Locators/ObjectsLocator';

@Component({
    

    selector: 'MultilineTextBoxWindow',
    templateUrl: "./MultilineTextBoxWindow.html",
})

export class MultilineTextBoxWindow implements OnInit {
    public DataContext: any;
    DisplayMode: boolean = false;
    PreventNewLine: boolean = false;
    IsTextBoxRTL: boolean = false;
    RowsCount: number;
    EnableKeyDown: boolean = false;

    private CurrentSession = SessionLocator.SelectedSession;
    constructor() {
    }

    SetWindowArgs(args){
        this.Text = args.TextValue;
        if (args.DisplayMode) {
            this.DisplayMode = args.DisplayMode;
        }
        this.RowsCount = args.RowsCount;
        this.IsTextBoxRTL = args.IsTextBoxRTL;

        this.PreventNewLine = this.RowsCount == 1;

        this.EnableKeyDown = args.EnableKeyDown;
    }

    private text: string;
    public get Text() { return this.text; }
    public set Text(newValue: string) {
        if (this.text != newValue) {
            this.text = newValue;
        }
    }

    OnKeyDown(event) {

        var ENTER = 13;
        var key = event.keyCode;
        var keyChar = event.key;

        if (key == ENTER && this.PreventNewLine && !this.EnableKeyDown) {
            event.preventDefault();
            return;
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
