import { UserPM } from './UserPM';

export class UserRolesPM {
	//
	constructor(_entityParentPM: any) {
		this.EntityParentPM = _entityParentPM;
		// this.UIProperties = new UIProperties();
		this.IsDirty = false;
	}

	private entityParentPM: any;
	public get EntityParentPM() {
		return this.entityParentPM;
	}
	public set EntityParentPM(newValue: any) {
		this.entityParentPM = newValue;
	}

	private changeSetOp: string;
	public get ChangeSetOp() {
		return this.changeSetOp;
	}
	public set ChangeSetOp(newValue: string) {
		this.changeSetOp = newValue;
		this.MarkAsDirty();
	}

	private id: string;
	public get Id() {
		return this.id;
	}
	public set Id(newValue: string) {
		this.id = newValue;
		this.MarkAsDirty();
	}

	private tenant: number;
	public get Tenant() {
		return this.tenant;
	}
	public set Tenant(newValue: number) {
		this.tenant = newValue;
		this.MarkAsDirty();
	}

	private name: string;
	public get Name() {
		return this.name;
	}
	public set Name(newValue: string) {
		this.name = newValue;
		this.MarkAsDirty();
	}

	private exists: boolean;
	public get Exists() {
		return this.exists;
	}
	public set Exists(newValue: boolean) {
		this.exists = newValue;
		this.MarkAsDirty();
	}

	private added: boolean;
	public get Added() {
		return this.added;
	}
	public set Added(newValue: boolean) {
		this.added = newValue;
		this.MarkAsDirty();
	}

	private removed: boolean;
	public get Removed() {
		return this.removed;
	}
	public set Removed(newValue: boolean) {
		this.removed = newValue;
		this.MarkAsDirty();
	}

	public OldEntityPM: UserRolesPM;

	public UniqueKey: string;

	public IsDirty: boolean;
	MarkAsDirty() {
		this.IsDirty = true;
		if (this.EntityParentPM) {
			this.EntityParentPM.MarkAsDirty();
		}
	}
}
