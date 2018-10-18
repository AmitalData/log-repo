Create PROCEDURE [dbo].[usp_DeleteContactsUnseenEntitiesByContactId] 
(
  @pContactId   varchar(15),
  @pObjectTableId   varchar(15),
  @pTenant    int
)
as 

BEGIN; 

Delete  From ContactsUnseenEntities where [ContactId] =  @pContactId  and [Tenant] =  @pTenant and  [ObjectTableId] =  @pObjectTableId;


 End;