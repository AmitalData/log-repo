

alter table Customs.PaymentOrderMethods drop PaymentOrderMethod_PaymentMethodType
go

DECLARE @SQL VARCHAR(4000)
SET @SQL = 'ALTER TABLE Customs.PaymentMethodTypes DROP CONSTRAINT |ConstraintName| '

SET @SQL = REPLACE(@SQL, '|ConstraintName|', ( SELECT   name
                                               FROM     sysobjects
                                               WHERE    xtype = 'PK'
                                                        AND parent_obj = OBJECT_ID('Customs.PaymentMethodTypes')
                                             ))

EXEC (@SQL)




alter table Customs.PaymentMethodTypes alter column code varchar(3) not null
go

alter table Customs.PaymentOrderMethods alter column TypeCode varchar(3) not null
go


ALTER TABLE Customs.PaymentMethodTypes
ADD PRIMARY KEY (Code)
go



alter table [Customs].[PaymentOrderMethods] add constraint [PaymentOrderMethod_PaymentMethodType] foreign key ([TypeCode]) references [Customs].[PaymentMethodTypes]([Code]);

