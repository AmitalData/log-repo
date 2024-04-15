export class ObjectFieldValidationPM {
	//
	constructor(entityParentPM: any) {
		this.EntityParentPM = entityParentPM;
		// this.UIProperties = new UIProperties();
		this.IsDirty = false;
	}

	private id: string;
	public get Id() {
		return this.id;
	}
	public set Id(newValue: string) {
		this.id = newValue;
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

	private searchFields: string;
	public get SearchFields() {
		return this.searchFields;
	}
	public set SearchFields(newValue: string) {
		this.searchFields = newValue;
		this.MarkAsDirty();
	}

	private changeSetOp: string;
	public get ChangeSetOp() {
		return this.changeSetOp;
	}
	public set ChangeSetOp(newValue: string) {
		this.changeSetOp = newValue;
		this.MarkAsDirty();
	}

	public OldEntityPM: ObjectFieldValidationPM;

	public UniqueKey: string;

	private entityParentPM: any;
	public get EntityParentPM() {
		return this.entityParentPM;
	}
	public set EntityParentPM(newValue: any) {
		this.entityParentPM = newValue;
	}

	public IsDirty: boolean;
	public DisableMarkAsDirty: boolean = false;
	MarkAsDirty() {
		if (!this.DisableMarkAsDirty) {
			this.IsDirty = true;
			if (this.entityParentPM) {
				this.entityParentPM.MarkAsDirty();
			}
		}
	}
}
