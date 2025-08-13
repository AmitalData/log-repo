
import {JournalPM} from './JournalPM';

import {UIProperties} from '../../Infrastructure/Components/LogitudeComponents/UIProperties';
import {ServiceHelper} from '../../Infrastructure/Utilities/ServiceHelper';
import {ServiceLocator} from '../../Infrastructure/Locators/ServiceLocator';
import {Output, EventEmitter}  from '@angular/core';
import {PropertyChangedArgs} from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';


export class JournalAnalyseResult {
      
      @Output() PropertyChanged: EventEmitter<PropertyChangedArgs> = new EventEmitter<PropertyChangedArgs>();
      public UIProperties: UIProperties;
	  constructor() {
                    this.UIProperties = new UIProperties(this); 
          this.IsDirty = false;
      }
 	 
    
    private journalPM: JournalPM;
    public get JournalPM() { 
        return this.journalPM; 
    }
    public set JournalPM(newValue: JournalPM) { if (this.journalPM !== newValue) { this.journalPM = newValue; this.MarkAsDirty("JournalPM"); } }
       
	 
    private duplicatesSkipped: number;
    public get DuplicatesSkipped() { return this.duplicatesSkipped; }
    public set DuplicatesSkipped(newValue: number) { if (this.duplicatesSkipped !== newValue) { this.duplicatesSkipped = newValue; this.MarkAsDirty("DuplicatesSkipped"); } }
       


    public OldEntityPM: JournalAnalyseResult;
		
    public IsDirty: boolean;
    public DisableMarkAsDirty: boolean = false;
    MarkAsDirty(propertyName:string = null) {
       if(!this.DisableMarkAsDirty)
       {
        this.IsDirty = true;
		  	
        if (propertyName != null) {
            this.PropertyChanged.emit(new PropertyChangedArgs(propertyName,this));
            ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, "JournalAnalyseResult");
           
        }
       }
    }


    public CloneMe() {
        ServiceHelper.CloneEntityPM(this);
    }

    public RejectChanges() {
        ServiceHelper.RejectEntityPMChanges(this);
    }

}
