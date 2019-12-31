

update ObjectTableRuleFields set ObjectFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = ObjectTableRuleFields.ObjectFieldId)