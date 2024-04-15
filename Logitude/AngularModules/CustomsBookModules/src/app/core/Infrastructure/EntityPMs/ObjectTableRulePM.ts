import { RuleConditionFieldPM } from './RuleConditionFieldPM';

import { ServiceHelper } from '../../Infrastructure/Utilities/ServiceHelper';
export class ObjectTableRulePM {
	//
	constructor() {
		// this.UIProperties = new UIProperties();
		this.IsDirty = false;
	}

	public Id: string;
	public Tenant: number;
	public Condition: string;
	public SystemLevel: boolean;
	public RuleCode: string;
	public OutputMessage: string;
	public Name: string;
	public InActive: boolean;
	public RuleTypeCode: string;
	public ObjectTableId: string;
	public RuleTypeName: string;
	public RuleNotificationTypeCode: string;
	public TriggerTypeCode: string;
	public TriggerFieldId: string;
	public TriggerFieldCode: string;
	public ActiveForNew: boolean;
	public ActiveForUpdate: boolean;
	public Internal: boolean;
	public AdvancedCondition: boolean;
	public IsCreatedFromSystemRule: boolean;

	private ruleConditionFields: RuleConditionFieldPM[];
	get RuleConditionFields() {
		if (this.ruleConditionFields == null) {
			this.ruleConditionFields = [];
		}

		return this.ruleConditionFields;
	}
	set RuleConditionFields(newValue: RuleConditionFieldPM[]) {
		if (this.ruleConditionFields != newValue) {
			this.ruleConditionFields = newValue;
		}
	}
	public AddRuleConditionField(item: RuleConditionFieldPM) {
		if (item != null) {
			var index = this.RuleConditionFields.indexOf(item);
			if (index == -1) {
				item.EntityParentPM = this;
				this.RuleConditionFields.push(item);
				this.MarkAsDirty();
			}
		}
	}
	public RemoveRuleConditionField(item: RuleConditionFieldPM) {
		if (item != null) {
			var index = this.RuleConditionFields.indexOf(item);
			if (index > -1) {
				this.RuleConditionFields.splice(index, 1);
				this.MarkAsDirty();
			}
		}
	}

	public OldEntityPM: ObjectTableRulePM;

	public IsDirty: boolean;
	MarkAsDirty() {
		this.IsDirty = true;
	}

	private MyClone: ObjectTableRulePM;

	public CloneMe() {
		ServiceHelper.CloneEntityPM(this);
	}

	public RejectChanges() {
		ServiceHelper.RejectEntityPMChanges(this);
	}
}
