import {Component, OnInit} from '@angular/core';
import {UIProperties, UIProperty} from './UIProperties';


export abstract class BaseComponent {
    public EntityPM: any;
    public UIProperties: UIProperties;

    constructor() {
        this.UIProperties = new UIProperties;
    }
}
