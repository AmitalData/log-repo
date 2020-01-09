

update ObjectTableRules set TriggerFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = ObjectTableRules.TriggerFieldId)