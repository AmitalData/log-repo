import { UserPM } from './UserPM';

export class UserPermittedProductPM {
	//
	constructor(entityParentPM: UserPM) {
		// this.UIProperties = new UIProperties();
		this.IsDirty = false;
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

	private userId: string;
	public get UserId() {
		return this.userId;
	}
	public set UserId(newValue: string) {
		this.userId = newValue;
		this.MarkAsDirty();
	}

	private productTypeCode: string;
	public get ProductTypeCode() {
		return this.productTypeCode;
	}
	public set ProductTypeCode(newValue: string) {
		this.productTypeCode = newValue;
		this.MarkAsDirty();
	}

	public entityParentPM: any;
	public get EntityParentPM() {
		return this.entityParentPM;
	}
	public set EntityParentPM(newValue: any) {
		this.entityParentPM = newValue;
	}

	public OldEntityPM: UserPermittedProductPM;

	public UniqueKey: string;

	public IsDirty: boolean;
	MarkAsDirty() {
		this.IsDirty = true;
		if (this.entityParentPM) {
			this.entityParentPM.MarkAsDirty();
		}
	}
}
