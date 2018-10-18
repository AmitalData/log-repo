
IF OBJECT_ID(N'[dbo].[ContactDoneMethods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContactDoneMethods];
go

create table [ContactDoneMethods] (
    [Code] [varchar](2) not null,
    [Name] [varchar](40) not null,
    [SearchFields] [nvarchar](1000) null,
    primary key ([Code])
);
go

begin transaction
begin

alter table Contacts add [ContactDoneMethodCode] varchar(2) null

alter table [Contacts] add constraint [Contact_ContactDoneMethod] foreign key ([ContactDoneMethodCode]) references [ContactDoneMethods]([Code]);

END
commit transaction
