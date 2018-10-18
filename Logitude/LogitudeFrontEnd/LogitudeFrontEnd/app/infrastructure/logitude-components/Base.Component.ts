import {Component,OnInit} from 'angular2/core';
import {UIProperties, UIProperty} from './UIProperties';
@Component({

})

export abstract class BaseComponent 
{
    public EntityPM: any;
    public UIProperties: UIProperties;

    constructor() {
        this.UIProperties = new UIProperties; 
    }
    
    
}







