
delete from ObjectTableRuleFields where ObjectTableRuleId=(select id from ObjectTableRules where RuleCode='User_Block_Fields' and ObjectTableId=(select id from ObjectTables where name='User'))
delete from ObjectTableRules where RuleCode='User_Block_Fields' and ObjectTableId=(select id from ObjectTables where name='User')
delete from ObjectTableRuleFields where ObjectTableRuleId=(select id from ObjectTableRules where RuleCode='Contact_Block_Fields' and ObjectTableId=(select id from ObjectTables where name='contact'))
delete from ObjectTableRules where RuleCode='Contact_Block_Fields' and ObjectTableId=(select id from ObjectTables where name='contact')