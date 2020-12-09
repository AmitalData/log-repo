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

    @Output() SaveHotKey: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() LeftArrowHotKey: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() RightArrowHotKey: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() ESCHotKey: EventEmitter<boolean> = new EventEmitter<boolean>();
    @Output() ShiftSHotKey: EventEmitter<boolean> = new EventEmitter<boolean>();
    ComponentId:string;
    CounterId:number;
    public ComponentIndex:number;
    CurrentSession=SessionLocator.SelectedSession;

    @HostListener('document:keydown',['$event']) 
    hotKeySaveChanges(event:any){
        let saveKey=83;
        let isCTRSSHotkey=(event.ctrlKey && event.which == saveKey);
        let isCTRLShiftSHotkey=(event.ctrlKey && event.shiftKey && event.which == saveKey);

        if(isCTRLShiftSHotkey){
            if(this.ComponentId == SessionLocator.SelectedSession.CurrentLogitudeHotKeysComponent.ComponentId){
                console.log('shift + s  '+this.ComponentId);
                this.ShiftSHotKey.emit();
            }
            return false;
        }
        else if(isCTRSSHotkey){
            if(this.ComponentId == SessionLocator.SelectedSession.CurrentLogitudeHotKeysComponent.ComponentId){
                console.log('Saving......from '+this.ComponentId);
                this.SaveHotKey.emit();
             }
           return false;
        }
        
    }
    
    @HostListener('document:keydown.arrowright') hotKeyNext(){
        
        if(this.ComponentId == SessionLocator.SelectedSession.CurrentLogitudeHotKeysComponent.ComponentId){
        console.log('Next from '+this.ComponentId);
        this.RightArrowHotKey.emit();
        }
        
        return false;
    }
    
    @HostListener('document:keydown.arrowleft') hotKeyPrevious(){
      
        if(this.ComponentId == SessionLocator.SelectedSession.CurrentLogitudeHotKeysComponent.ComponentId){
        console.log('Previous from '+this.ComponentId);
        this.LeftArrowHotKey.emit();
        }
        
        return false;
    }
    @HostListener('document:keydown.escape') hotKeyBack(){
        
        if(this.ComponentId == SessionLocator.SelectedSession.CurrentLogitudeHotKeysComponent.ComponentId){
        console.log('Back button clicked method from '+this.ComponentId);
        this.ESCHotKey.emit();
        }
        
        return false;
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
        this.SaveHotKey=null;
        this.ShiftSHotKey=null;
        this.LeftArrowHotKey=null;
        this.RightArrowHotKey=null;
        this.ESCHotKey=null;
    }
    ngOnDestroy(){
        this.DestroyLogitudeHotKeysControl();
        
    }
    
}
