import { UIProperties, UIProperty } from '../Components/LogitudeComponents/UIProperties';
import { ServiceHelper } from '../Utilities/ServiceHelper';
import { ServiceLocator } from '../../Infrastructure/Locators/ServiceLocator';
import { Output, EventEmitter } from '@angular/core';
import { PropertyChangedArgs } from '../EventEmitterArgs/PropertyChangedArgs';

export class TenantSettingPM {
	constructor() {
		this.IsDirty = false;
	}

	private id: string;
	public get Id() {
		return this.id;
	}
	public set Id(newValue: string) {
		if (this.id != newValue) {
			this.id = newValue;
			this.MarkAsDirty('Id');
		}
	}

	private tenant: number;
	public get Tenant() {
		return this.tenant;
	}
	public set Tenant(newValue: number) {
		if (this.tenant != newValue) {
			this.tenant = newValue;
			this.MarkAsDirty('Tenant');
		}
	}

	private size: number;
	public get Size() {
		return this.size;
	}
	public set Size(newValue: number) {
		if (this.size != newValue) {
			this.size = newValue;
			this.MarkAsDirty('Size');
		}
	}

	private prefix: string;
	public get Prefix() {
		return this.prefix;
	}
	public set Prefix(newValue: string) {
		if (this.prefix != newValue) {
			this.prefix = newValue;
			this.MarkAsDirty('Prefix');
		}
	}

	private settingCode: string;
	public get SettingCode() {
		return this.settingCode;
	}
	public set SettingCode(newValue: string) {
		if (this.settingCode != newValue) {
			this.settingCode = newValue;
			this.MarkAsDirty('SettingCode');
		}
	}

	private settingValue: string;
	public get SettingValue() {
		return this.settingValue;
	}
	public set SettingValue(newValue: string) {
		if (this.settingValue != newValue) {
			this.settingValue = newValue;
			this.MarkAsDirty('SettingValue');
		}
	}

	private objectTableId: string;
	public get ObjectTableId() {
		return this.objectTableId;
	}
	public set ObjectTableId(newValue: string) {
		if (this.objectTableId != newValue) {
			this.objectTableId = newValue;
			this.MarkAsDirty('ObjectTableId');
		}
	}

	private dontIncludeDirects: boolean;
	public get DontIncludeDirects() {
		return this.dontIncludeDirects;
	}
	public set DontIncludeDirects(newValue: boolean) {
		if (this.dontIncludeDirects != newValue) {
			this.dontIncludeDirects = newValue;
			this.MarkAsDirty('DontIncludeDirects');
		}
	}

	public OldEntityPM: TenantSettingPM;

	public IsDirty: boolean;
	MarkAsDirty(propertyName: string = null) {
		this.IsDirty = true;

		if (propertyName != null) {
			ServiceLocator.RulesValidator.ApplyEntityChangedRules(propertyName, this, 'TenantSetting');
		}
	}
}
