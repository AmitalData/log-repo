-- Creating foreign key on [RuleNotificationTypeCode] in table 'ObjectTableRules'
ALTER TABLE [dbo].[ObjectTableRules]
drop CONSTRAINT [FK_ObjectTableRuleRuleNotificationType]
   

-- Creating non-clustered index for FOREIGN KEY 'FK_ObjectTableRuleRuleNotificationType'
  
 
DROP INDEX [IX_FK_ObjectTableRuleRuleNotificationType] ON  [dbo].[ObjectTableRules]

-- Creating foreign key on [RuleNotificationTypeCode] in table 'ObjectTableRules'
ALTER TABLE [dbo].[ObjectTableRules]
ADD CONSTRAINT [FK_ObjectTableRuleRuleNotificationType]
    FOREIGN KEY ([RuleNotificationTypeCode])
    REFERENCES [dbo].[RuleNotificationTypes]
        ([Code])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ObjectTableRuleRuleNotificationType'
CREATE INDEX [IX_FK_ObjectTableRuleRuleNotificationType]
ON [dbo].[ObjectTableRules]
    ([RuleNotificationTypeCode]);
GO

alter table [dbo].[ObjectTableRules] ALTER COLUMN  [RuleNotificationTypeCode] varchar(4)   NULL