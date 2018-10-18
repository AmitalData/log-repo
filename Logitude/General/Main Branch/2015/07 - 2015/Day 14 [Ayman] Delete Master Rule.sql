

delete from ObjectTableRuleFields where ObjectTableRuleId = (select Id from ObjectTableRules where RuleCode = 'MAWB')
go

delete from RuleConditionFields where ObjectTableRuleId = (select Id from ObjectTableRules where RuleCode = 'MAWB')
go

delete from ObjectTableRules where RuleCode = 'MAWB'