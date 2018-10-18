

ALTER TABLE Quotes ADD UpdatedByUserId varchar(15)
GO

update Quotes set UpdatedByUserId = CreatedByUserId
GO

ALTER TABLE Quotes alter column UpdatedByUserId varchar(15) not null
GO

	ALTER TABLE Quotes ADD CONSTRAINT FK_QuoteUpdatedByUser
		FOREIGN KEY (UpdatedByUserId)
		REFERENCES Users(Id)
		ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

CREATE INDEX [IX_FK_QuoteUpdatedByUser] ON Quotes (UpdatedByUserId);



