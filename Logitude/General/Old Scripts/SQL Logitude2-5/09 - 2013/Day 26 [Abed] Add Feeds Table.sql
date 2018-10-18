

-------------------------------------> Don't Run it
-------------------------------------------------------------------------------------
--begin transaction
--begin

---- Creating table 'Feeds'
--IF OBJECT_ID(N'[dbo].[Feeds]', 'U') IS NOT NULL
--    DROP TABLE [dbo].[Feeds];

--CREATE TABLE [dbo].[Feeds] (
--     [PostId] [varchar](15) not null,
--	 [FolloweeId] [varchar](15) not null,
--	 [FollowerId] [varchar](15) not null,

--     [Tenant] [int] not null,
--	 [CreateDate] datetime NULL,
--	 [IsCancelled] bit NOT NULL,

--	 primary key ([PostId], [FolloweeId] , [FollowerId])
    
--);

-- --add foreign key from table [Posts] to [Feeds ] >>>>>>[PostId]
--ALTER TABLE [dbo].[Feeds]
--ADD CONSTRAINT [ FK_PostsFeeds]
--    FOREIGN KEY ([PostId] )
	
--    REFERENCES [dbo].[Posts]
--        ([Id])
--ON DELETE NO ACTION ON UPDATE NO ACTION;



-- --add foreign key from table [Users] to [Feeds ] >>>>>>[FolloweeId]
--ALTER TABLE [dbo].[Feeds]
--ADD CONSTRAINT [ FK_UsersFolloweeFeeds]
--    FOREIGN KEY ([FolloweeId] )
	
--    REFERENCES [dbo].[Users]
--        ([Id])
--ON DELETE NO ACTION ON UPDATE NO ACTION;



-- --add foreign key from table [Users] to [Feeds ] >>>>>>[FollowerId]

--ALTER TABLE [dbo].[Feeds]
--ADD CONSTRAINT [ FK_UsersFollowerFeeds]
--    FOREIGN KEY ([FollowerId] )
	
--    REFERENCES [dbo].[Users]
--        ([Id])
--ON DELETE NO ACTION ON UPDATE NO ACTION;

--END
--commit transaction

