declare var window: any;
import { HostListener } from '@angular/core';
import {Directive, ElementRef, Input, Output, Component, OnInit, OnChanges, EventEmitter, AfterViewInit,Renderer2} from '@angular/core';  
import { ControlsIdCounter } from 'Infrastructure/Utilities/ControlsIdCounter';
import { SessionLocator } from 'Infrastructure/Utilities/SessionLocator';



@Component({
    

    selector: 'LogitudeHotKeysComponent',
    templateUrl: './LogitudeHotKeysComponent.html',
})

export class LogitudeHotKeysComponent{

    @Output() CTRL_S_HotKey: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() LeftArrowHotKey: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() RightArrowHotKey: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() ESCHotKey: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() CTRL_Shift_S_HotKey: EventEmitter<boolean> = new EventEmitter<boolean>();
    ComponentId:string;
    CounterId:number;
    public ComponentIndex:number;
    CurrentSession=SessionLocator.SelectedSession;

    @HostListener('document:keydown',['$event']) 
    hotKeySaveChanges(event:any){
        let saveKey=83;
        let rightKey=39;
        let leftKey=37;
        let escKey=27;
        let isCTRSSHotkey=(event.ctrlKey && event.which == saveKey);
        let isCTRLShiftSHotkey=(event.ctrlKey && event.shiftKey && event.which == saveKey);
        let isRightHotKey=(event.ctrlKey&&event.altKey&& event.which==rightKey);
        let isLeftHotKey=(event.ctrlKey&&event.altKey&& event.which==leftKey);
        let isEscHotkey=(event.which==escKey);
        if(isCTRLShiftSHotkey){
            this.EmitCtrlShiftSHotkey();
            return false;
        }
        else if(isCTRSSHotkey){
            this.EmitCtrlSHotkey();
           return false;
        }
        else if(isRightHotKey){
            this.EmitRightHotKey();
            return false;
        }
        else if(isLeftHotKey){
            this.EmitLeftHotKey();
            return false;
        }
        else if(isEscHotkey){
            this.EmitEscHotkey();
            return false;
        }
        
    }
    
   

    EmitCtrlShiftSHotkey(){
        if(this.ComponentId == SessionLocator.SelectedSession.CurrentLogitudeHotKeysComponent.ComponentId){
            console.log('shift + s  '+this.ComponentId);
            this.CTRL_Shift_S_HotKey.emit();
        }
    }

    EmitCtrlSHotkey(){
        if(this.ComponentId == SessionLocator.SelectedSession.CurrentLogitudeHotKeysComponent.ComponentId){
            console.log('Saving......from '+this.ComponentId);
            this.CTRL_S_HotKey.emit();
         }
    }

    EmitRightHotKey(){
        if(this.ComponentId == SessionLocator.SelectedSession.CurrentLogitudeHotKeysComponent.ComponentId){
            console.log('Next from '+this.ComponentId);
            this.RightArrowHotKey.emit();
        }
    }
    EmitLeftHotKey(){
        if(this.ComponentId == SessionLocator.SelectedSession.CurrentLogitudeHotKeysComponent.ComponentId){
            console.log('Previous from '+this.ComponentId);
            this.LeftArrowHotKey.emit();
            }
    }
    EmitEscHotkey(){
        if(this.ComponentId == SessionLocator.SelectedSession.CurrentLogitudeHotKeysComponent.ComponentId){
            console.log('Back button clicked method from '+this.ComponentId);
            this.ESCHotKey.emit();
            }
    }
    constructor(){

    }
    ngOnInit(){
        const baseIdCombination="logitude_hot_keys_";
        this.CounterId = ControlsIdCounter.GetNextControlIdCounter(baseIdCombination);
        this.ComponentId=baseIdCombination + this.CounterId;
        this.ComponentIndex=this.CurrentSession.GetNewLogitudeHotKeysComponentIndex();
        this.CurrentSession.AddLogitudeHotKeysComponent(this);
    }

    DestroyLogitudeHotKeysControl(){
        this.CurrentSession.RemoveLogitudeHotKeysComponent(this);
        this.CurrentSession=null;
        this.CTRL_S_HotKey=null;
        this.CTRL_Shift_S_HotKey=null;
        this.LeftArrowHotKey=null;
        this.RightArrowHotKey=null;
        this.ESCHotKey=null;
    }
    ngOnDestroy(){
        this.DestroyLogitudeHotKeysControl();
        
    }
    
}
