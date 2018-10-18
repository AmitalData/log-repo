
update Queries set EditWizardName = null where ObjectTableId = (select id from ObjectTables where Name ='Agent' and EditWizardName = 'SharedLogistics.Views.InviteAgentsControl')
