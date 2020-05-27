

update AirlineMessagingRules set RuleFieldCode = (select ObjectFields.FieldCode from ObjectFields where id = AirlineMessagingRules.RuleFieldId)