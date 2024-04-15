import { ServiceLocator } from '../../Infrastructure/Locators/ServiceLocator';
import { Output, EventEmitter } from '@angular/core';
import { PropertyChangedArgs } from '../../Infrastructure/EventEmitterArgs/PropertyChangedArgs';
// import { UIProperties } from '../../Infrastructure/Components/LogitudeComponents/UIProperties';

export class SharedUserQueryPM {
	//
	//
	// constructor(_entityParentPM: any) {
	//     this.EntityParentPM = _entityParentPM;
	//
	//     this.IsDirty = false;
	// }

	private id: string;
	public get Id() {
		return this.id;
	}
	public set Id(newValue: string) {
		this.id = newValue;
	}

	private tenant: number;
	public get Tenant() {
		return this.tenant;
	}
	public set Tenant(newValue: number) {
		this.tenant = newValue;
	}

	private userId: string;
	public get UserId() {
		return this.userId;
	}
	public set UserId(newValue: string) {
		this.userId = newValue;
	}

	private queryId: string;
	public get QueryId() {
		return this.queryId;
	}
	public set QueryId(newValue: string) {
		this.queryId = newValue;
	}

	private queryCode: string;
	public get QueryCode() {
		return this.queryCode;
	}
	public set QueryCode(newValue: string) {
		this.queryCode = newValue;
	}

	private entityParentPM: any;
	public get EntityParentPM() {
		return this.entityParentPM;
	}
	public set EntityParentPM(newValue: any) {
		this.entityParentPM = newValue;
	}

	public UniqueKey: string;

	private changeSetOp: string;
	public get ChangeSetOp() {
		return this.changeSetOp;
	}
	public set ChangeSetOp(newValue: string) {
		if (this.changeSetOp != newValue) {
			this.changeSetOp = newValue;
			this.MarkAsDirty('ChangeSetOp');
		}
	}

	public OldEntityPM: SharedUserQueryPM;

	public IsDirty: boolean;
	MarkAsDirty(propertyName: string = null) {
		this.IsDirty = true;
		if (this.EntityParentPM) {
			this.EntityParentPM.MarkAsDirty();
		}

		if (propertyName != null) {
			//
			ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, 'SharedUserQuery');
		}
	}
}
