//
import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
export class RuleConditionFieldPM {
	//
	// constructor(_entityParentPM: any) {
	//     this.EntityParentPM = _entityParentPM;
	//     this.UIProperties = new UIProperties;
	//     this.IsDirty = false;
	// }

	/*[Key]*/
	public Id: string;
	public Tenant: number;
	public ObjectTableRuleId: string;
	public ObjectFieldId: string;
	public ObjectFieldCode: string;
	public Value: string;
	public Operator: string;
	public ObjectFieldName: string;
	public OldEntityPM: RuleConditionFieldPM;

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

	public UniqueKey: string;

	public IsDirty: boolean;
	MarkAsDirty() {
		this.IsDirty = true;
		if (this.EntityParentPM) {
			this.EntityParentPM.MarkAsDirty();
		}
	}

	private MyClone: RuleConditionFieldPM;

	public CloneMe() {
		ServiceHelper.CloneEntityPM(this);
	}

	public RejectChanges() {
		ServiceHelper.RejectEntityPMChanges(this);
	}
}
