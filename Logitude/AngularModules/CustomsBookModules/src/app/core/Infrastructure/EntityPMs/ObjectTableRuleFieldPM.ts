import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
export class ObjectTableRuleFieldPM {
	//
	constructor() {
		// this.UIProperties = new UIProperties();
		this.IsDirty = false;
	}

	/*[Key]*/
	public Id: string;
	public Tenant: number;
	public SystemLevel: boolean;
	public ObjectFieldId: string;
	public ObjectTableRuleId: string;
	public ObjectFieldName: string;
	public Expression: string;
	public RuleNotificationTypeCode: string;
	public ObjectTableRuleCode: string;
	public ObjectTableRuleTypeCode: string;
	public ObjectFieldCode: string;
	public OldEntityPM: ObjectTableRuleFieldPM;
	private changeSetOp: string;
	public get ChangeSetOp() {
		return this.changeSetOp;
	}
	public set ChangeSetOp(newValue: string) {
		this.changeSetOp = newValue;
		this.MarkAsDirty();
	}
	public IsDirty: boolean;
	MarkAsDirty() {
		this.IsDirty = true;
	}
}
