
declare @declarationConstraint table (id varchar(15),DeclarationStatusTypeCode varchar(2), hasConstraint bit, errorXml nvarchar(MAX)  )
declare @declarationWithoutConstraint table (id varchar(15) ,  hasConstraint bit)
declare @declarationStatus table (statusCode varchar(2))


insert into @declarationConstraint (id,DeclarationStatusTypeCode,errorXml)  select  Id,DeclarationStatusTypeCode,ErrosXml  from [Customs].Declarations where Id in( select DeclarationId from [Customs].DeclarationConstraints ) 
insert into @declarationWithoutConstraint (id) select   Id  from [Customs].Declarations where Id not in( select DeclarationId from [Customs].DeclarationConstraints ) 


 update @declarationWithoutConstraint set hasConstraint = 'false'
 update [customs].Declarations set HasConstraint = c.hasConstraint
from [Customs].Declarations as d,@declarationWithoutConstraint as c 
Where d.Id = c.id

update [Customs].Declarations set HasConstraint = 'false' where (DeclarationStatusTypeCode is null or DeclarationStatusTypeCode = '')

--select * from @declarationWithoutConstraint

--select * from  [customs].Declarations


 insert into @declarationStatus  values ('13'),('5'),('6'),('10'),('15')
 Declare @status varchar(2)
 declare @errorXml nvarchar(MAX)
 declare @id varchar(15)
 Declare StatusCursor cursor 
 for 
 select id, DeclarationStatusTypeCode , errorXml from @declarationConstraint
 	OPEN StatusCursor FETCH NEXT FROM StatusCursor INTO @id, @status, @errorXml
	WHILE @@FETCH_STATUS = 0
	BEGIN
	if(@status ='13' or @status='5' or @status='6' or @status ='10' or @status ='15')
	begin
	update @declarationConstraint set hasConstraint = 'true'

    update [customs].Declarations set HasConstraint = c.hasConstraint

    from [Customs].Declarations as d,@declarationConstraint as c , @declarationStatus as s
    Where d.Id = @id and d.DeclarationStatusTypeCode = s.statusCode
    end

	else if((@status ='12' or @status ='2') and(@errorXml is  null or  @errorXml = '' or @errorXml  not like '%<ListVersionID>1</ListVersionID>%'))
	BEGIN
	delete from @declarationStatus
   insert into @declarationStatus  values ('2'),('12')
       update @declarationConstraint set hasConstraint = 'true'

      update [customs].Declarations set HasConstraint = c.hasConstraint

    from [Customs].Declarations as d,@declarationConstraint as c , @declarationStatus as s
      Where d.Id =@id and d.DeclarationStatusTypeCode = s.statusCode


	END

	else if((@status ='12' or @status ='2') and (@errorXml   like '%<ListVersionID>1</ListVersionID>%'))
	BEGIN
	delete from @declarationStatus
   insert into @declarationStatus  values ('2'),('12')
       update @declarationConstraint set hasConstraint = 'false'

      update [customs].Declarations set HasConstraint = 'false'

    from [Customs].Declarations as d,@declarationConstraint as c 
      Where d.Id =@id and d.DeclarationStatusTypeCode = @status 


	END

FETCH NEXT FROM StatusCursor INTO @id, @status, @errorXml
	END
	CLOSE StatusCursor
	DEALLOCATE StatusCursor

	select * from @declarationConstraint


	