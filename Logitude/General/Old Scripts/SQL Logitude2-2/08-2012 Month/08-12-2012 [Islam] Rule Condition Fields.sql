
-- Creating table 'RuleConditionFields'
CREATE TABLE [dbo].[RuleConditionFields] (
    [Id]varchar(15)   NOT NULL,
    [Tenant]int   NOT NULL,
    [Value]varchar(100)   NULL,
    [Operator]varchar(40)   NULL,
    [ObjectTableRuleId]varchar(15)   NOT NULL,
    [ObjectFieldId]varchar(15)   NOT NULL
);
GO


-- Creating primary key on [Id] in table 'RuleConditionFields'
ALTER TABLE [dbo].[RuleConditionFields]
ADD CONSTRAINT [PK_RuleConditionFields]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO
-- Creating foreign key on [ObjectTableRuleId] in table 'RuleConditionFields'
ALTER TABLE [dbo].[RuleConditionFields]
ADD CONSTRAINT [FK_RuleConditionFieldsObjectTableRule]
    FOREIGN KEY ([ObjectTableRuleId])
    REFERENCES [dbo].[ObjectTableRules]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RuleConditionFieldsObjectTableRule'
CREATE INDEX [IX_FK_RuleConditionFieldsObjectTableRule]
ON [dbo].[RuleConditionFields]
    ([ObjectTableRuleId]);
GO

-- Creating foreign key on [ObjectFieldId] in table 'RuleConditionFields'
ALTER TABLE [dbo].[RuleConditionFields]
ADD CONSTRAINT [FK_RuleConditionFieldsObjectField]
    FOREIGN KEY ([ObjectFieldId])
    REFERENCES [dbo].[ObjectFields]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RuleConditionFieldsObjectField'
CREATE INDEX [IX_FK_RuleConditionFieldsObjectField]
ON [dbo].[RuleConditionFields]
    ([ObjectFieldId]);
GO




alter table objecttablerules add [AdvancedCondition] bit not null default 'true'