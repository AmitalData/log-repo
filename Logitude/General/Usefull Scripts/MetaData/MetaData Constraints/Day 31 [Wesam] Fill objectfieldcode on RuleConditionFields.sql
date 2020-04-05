
update RuleConditionFields set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = RuleConditionFields.ObjectFieldId)
