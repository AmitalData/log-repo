-- not needed
declare @PotentialCustomerId as varchar(15)
declare @ActiveCustomerId as varchar (15)
declare @ExternalId as varchar(25)
declare @ActivePartnerTypeId as char(2)
declare @Tenant as int

BEGIN
              DECLARE CustomersCursor CURSOR READ_ONLY
              FOR
              SELECT Id, AccountingCard, Tenant
              FROM Cards
              WHERE PartnerTypeId = 'PO' and AccountingCard is not null
              OPEN CustomersCursor FETCH NEXT FROM CustomersCursor INTO @PotentialCustomerId, @ExternalId, @Tenant              
              WHILE @@FETCH_STATUS = 0
                     BEGIN
                     
                     set @ActiveCustomerId = (select Id from Cards where Code = @ExternalId and Tenant = @Tenant)
                     set @ActivePartnerTypeId = (select PartnerTypeId  from Cards where Code = @ExternalId and Tenant = @Tenant)

                     if (@ActivePartnerTypeId is not null)
                     begin

                           if (@ActivePartnerTypeId = 'CS')
                           begin

                                  if (@ActiveCustomerId is not null)
                                  Begin
                           
                                         update Cards 
                                         set PrimaryContactId = null 
                                         where Id = @PotentialCustomerId

                                         update Activities
                                         set CustomerId = @ActiveCustomerId
                                         where CustomerId = @PotentialCustomerId

                                         update Opportunities
                                         set CustomerId = @ActiveCustomerId
                                         where CustomerId = @PotentialCustomerId

                                         update Quotes
                                         set ConsigneeId = @ActiveCustomerId
                                         where ConsigneeId = @PotentialCustomerId

                                         update Quotes
                                         set ShipperId = @ActiveCustomerId
                                         where ShipperId = @PotentialCustomerId

                                         update Quotes
                                         set CustomerId = @ActiveCustomerId
                                         where CustomerId = @PotentialCustomerId

                                         update CustomerAdditionalServices
                                         set CustomerId = @ActiveCustomerId
                                         where CustomerId = @PotentialCustomerId

                                         update CustomerProducts
                                         set CustomerId = @ActiveCustomerId
                                         where CustomerId = @PotentialCustomerId

                                         update CustomerCompetitors
                                         set CustomerId = @ActiveCustomerId
                                         where CustomerId = @PotentialCustomerId

                                         update CustomerSalesNote
                                         set CustomerId = @ActiveCustomerId
                                         where CustomerId = @PotentialCustomerId

                                         declare @ContactId as varchar(15)
                                         BEGIN
                                                DECLARE ContactsCursor CURSOR READ_ONLY
                                                FOR
                                                SELECT ContactId
                                                FROM CardContacts
                                                WHERE CardId = @PotentialCustomerId
                                                OPEN ContactsCursor FETCH NEXT FROM ContactsCursor INTO @ContactId            
                                                WHILE @@FETCH_STATUS = 0
                                                       BEGIN

                                                       if not exists ( select * from Activities where CallWithId = @ContactId)
                                                       begin
                                                              
                                                              delete from CardContacts 
                                                              where CardId = @PotentialCustomerId and ContactId = @ContactId

                                                              delete from ContactTenants
                                                              where ContactId = @ContactId

                                                              delete from ContactTenantRoleSet
                                                              where ContactTenantId = @ContactId

                                                              delete from Contacts
                                                              where Id = @ContactId

                                                       end

                                                       else
                                                       begin

                                                              update CardContacts
                                                              set CardId = @ActiveCustomerId
                                                              where CardId = @PotentialCustomerId

                                                       end                                                                                             

                                                              FETCH NEXT FROM ContactsCursor INTO @ContactId    
                                                       END
                                                CLOSE ContactsCursor
                                                DEALLOCATE ContactsCursor
                                         END
                           
                                         delete from Addresses where CardId = @PotentialCustomerId
                                         delete from Customers where Id = @PotentialCustomerId
                                         delete from Cards where Id = @PotentialCustomerId
                         
                                  End

                           end

                     end
                     
                           FETCH NEXT FROM CustomersCursor INTO @PotentialCustomerId, @ExternalId, @Tenant       
                     END
              CLOSE CustomersCursor
              DEALLOCATE CustomersCursor
END
