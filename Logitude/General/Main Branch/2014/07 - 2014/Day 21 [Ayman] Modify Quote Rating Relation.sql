
-- After migration
IF OBJECT_ID (N'QuoteRatings', N'U') IS NOT NULL
begin;
	
	if not exists (select * from QuoteRatings where Code = 'C')
	insert into QuoteRatings(Code, Name, IndexOrder, SearchFields)
	values('C','Cold',1,'C,Cold')

	if not exists (select * from QuoteRatings where Code = 'N')
	insert into QuoteRatings(Code, Name, IndexOrder, SearchFields)
	values('N','Neutral',2,'N,Neutral')

	if not exists (select * from QuoteRatings where Code = 'W')
	insert into QuoteRatings(Code, Name, IndexOrder, SearchFields)
	values('W','Warm',3,'W,Warm')

	if not exists (select * from QuoteRatings where Code = 'H')
	insert into QuoteRatings(Code, Name, IndexOrder, SearchFields)
	values('H','Hot',4,'H,Hot')

	if COLUMNPROPERTY(OBJECT_ID(N'Quotes'), 'RatingCode', 'ColumnId') is null
	alter table Quotes add RatingCode varchar(1)
	
	update Quotes set RatingCode = 'N' where RatingCode is null

	if not exists (select * from sys.objects o where o.object_id = object_id(N'FK_QuoteRating') AND OBJECTPROPERTY(o.object_id, N'IsForeignKey') = 1)
	BEGIN
	ALTER TABLE Quotes ADD CONSTRAINT FK_QuoteRating
		FOREIGN KEY (RatingCode)
		REFERENCES QuoteRatings(Code)
		ON DELETE NO ACTION ON UPDATE NO ACTION;
	END

	if not exists (select * from sys.indexes where name = 'IX_FK_QuoteRating')
	CREATE INDEX [IX_FK_QuoteRating] ON Quotes (RatingCode);									

end