
import {UIProperties, UIProperty} from '../../../infrastructure/logitude-components/UIProperties';
export class IncotermPM {

      public UIProperties: UIProperties;
	  constructor() {
          this.UIProperties = new UIProperties; 
          this.IsDirty = false;
      }
 	 
    
    private _id: string;
    public get Id() { return this._id; }
    public set Id(newValue: string) { this._id = newValue; this.MarkAsDirty(); }
       
	 
    private _tenant: number;
    public get Tenant() { return this._tenant; }
    public set Tenant(newValue: number) { this._tenant = newValue; this.MarkAsDirty(); }
       
	 
    private _code: string;
    public get Code() { return this._code; }
    public set Code(newValue: string) { this._code = newValue; this.MarkAsDirty(); }
       
	 
    private _name: string;
    public get Name() { return this._name; }
    public set Name(newValue: string) { this._name = newValue; this.MarkAsDirty(); }
       
	 
    private _localName: string;
    public get LocalName() { return this._localName; }
    public set LocalName(newValue: string) { this._localName = newValue; this.MarkAsDirty(); }
       
	 
    private _computedLocalName: string;
    public get ComputedLocalName() { return this._computedLocalName; }
    public set ComputedLocalName(newValue: string) { this._computedLocalName = newValue; this.MarkAsDirty(); }
       
	 
    private _freight: string;
    public get Freight() { return this._freight; }
    public set Freight(newValue: string) { this._freight = newValue; this.MarkAsDirty(); }
       
	 
    private _otherCharges: string;
    public get OtherCharges() { return this._otherCharges; }
    public set OtherCharges(newValue: string) { this._otherCharges = newValue; this.MarkAsDirty(); }
       
	 
    private _addedManually: string;
    public get AddedManually() { return this._addedManually; }
    public set AddedManually(newValue: string) { this._addedManually = newValue; this.MarkAsDirty(); }
       
	 
    private _inActive: string;
    public get InActive() { return this._inActive; }
    public set InActive(newValue: string) { this._inActive = newValue; this.MarkAsDirty(); }
       
	 
    private _notes: string;
    public get Notes() { return this._notes; }
    public set Notes(newValue: string) { this._notes = newValue; this.MarkAsDirty(); }
       
	 
    private _searchFields: string;
    public get SearchFields() { return this._searchFields; }
    public set SearchFields(newValue: string) { this._searchFields = newValue; this.MarkAsDirty(); }
       
	 
    private _isSecured: string;
    public get IsSecured() { return this._isSecured; }
    public set IsSecured(newValue: string) { this._isSecured = newValue; this.MarkAsDirty(); }
       
	 
    private _isHybrid: string;
    public get IsHybrid() { return this._isHybrid; }
    public set IsHybrid(newValue: string) { this._isHybrid = newValue; this.MarkAsDirty(); }
       
	 

    public IsDirty: boolean;
    MarkAsDirty() {
        this.IsDirty = true;
		  	
    }
}
