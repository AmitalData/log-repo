import {Component, OnInit} from '@angular/core';
import {UIProperties, UIProperty} from './UIProperties';


export abstract class BaseComponent {
    private _EntityPM: any;
    public get EntityPM(): any {
        return this._EntityPM;
    }
    public set EntityPM(value: any) {
        this._EntityPM = value;
    }
    public UIProperties: UIProperties;

    constructor() {
        this.UIProperties = new UIProperties;
    }
}
