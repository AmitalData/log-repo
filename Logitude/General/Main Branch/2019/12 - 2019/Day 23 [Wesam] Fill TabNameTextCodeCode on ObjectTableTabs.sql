

update ObjectTableTabs set TabNameTextCodeCode = (select TextCodes.Code from TextCodes where id = ObjectTableTabs.TabNameTextCodeId)