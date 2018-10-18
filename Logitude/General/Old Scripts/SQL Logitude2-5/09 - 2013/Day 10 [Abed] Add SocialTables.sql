

-- Creating table 'Posts'
IF OBJECT_ID(N'[dbo].[Posts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Posts];


CREATE TABLE [dbo].[Posts] (
     [Id] [varchar](15) not null,
	 [Tenant] [int] not null,
	 [CreatedById] [varchar](15) not null,
     [GroupId] [varchar](15)  null,
     [BodyText] [varchar](6000)not null,
	 [CreateDate]datetime NULL,
	 [ParentPostId] [varchar](15)  null,
	 [NumberOfLikes] [int]  null,
	 [IsPrivate] bit NOT NULL,
	 [IsCancelled] bit NOT NULL,
	 [ObjectTableId] [varchar](15) null,
	 [EntityId] [varchar](15)  null,
	
     primary key ([Id])
);

go



-- Creating table 'Feeds'
IF OBJECT_ID(N'[dbo].[Feeds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Feeds];

CREATE TABLE [dbo].[Feeds] (
     [PostId] [varchar](15) not null,
	 [UserId] [varchar](15) not null,
     [Tenant] [int] not null,
	 [PostDate] datetime NULL,
	 primary key ([PostId], [UserId])
    
);

go


-- Creating table 'Groups'
IF OBJECT_ID(N'[dbo].[Groups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Groups];

CREATE TABLE [dbo].[Groups] (
      [Id] [varchar](15) not null,
	  [Tenant] [int] not null,
	  [Name] [varchar](100) not null,
      [Description] [varchar](100) not null,
	  [IsPrivate] bit NOT NULL,
	  [OwnerId] [varchar](15) not null,
	  [CreateDate] datetime NULL,
	  primary key ([Id])
    
);

go


 ---- Creating table 'Followers'
 IF OBJECT_ID(N'[dbo].[Followers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Followers];

      CREATE TABLE [dbo].[Followers] (
      [FolloweeUserId] [varchar](15) not null,
	  [FollowerUserId] [varchar](15) not null,
	  [Tenant] [int] not null,
      [CreateDate]datetime NULL,
	  [IsCancelled] bit NOT NULL,
	  [CancelledDate]datetime NULL,
	  primary key ([FolloweeUserId], [FollowerUserId])
    
);

go


 ---- Creating table 'PostLikes'
 
IF OBJECT_ID(N'[dbo].[PostLikes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PostLikes];

      CREATE TABLE [dbo].[PostLikes] (
      [PostId] [varchar](15) not null,
	  [UserId] [varchar](15) not null,
	  [Tenant] [int] not null,
	  [IsCancelled] bit NOT NULL,
	  primary key ([PostId], [UserId])
    
);

go

 ---- Creating table 'FollowEntities'
 IF OBJECT_ID(N'[dbo].[FollowEntities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FollowEntities];

      CREATE TABLE [dbo].[FollowEntities] (
	  [Id] [varchar](15) not null,
	  [Tenant] [int] not null,
	  [ObjectTableId] [varchar](15) not null,
	  [EntityId] [varchar](15) not null,
	  [FollowerUserId] [varchar](15) not null,
	  [CreateDate] datetime NULL,
	  [IsCancelled] bit NOT NULL,
	  [CancelledDate] datetime NULL,

	    primary key ([Id])
    
);

go


 ---- Creating table 'GroupMembers'
IF OBJECT_ID(N'[dbo].[GroupMembers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GroupMembers];

      CREATE TABLE [dbo].[GroupMembers] (
	  [GroupId] [varchar](15) not null,
	  [UserId] [varchar](15) not null,
	  [Tenant] [int] not null,
	  [CreateDate]datetime NULL,
	   [IsCancelled] bit NOT NULL,
	  [CancelledDate]datetime NULL,

	     primary key ([GroupId], [UserId])
    
);

go



--[Posts ]
 --add foreign key from table [Users] to [Posts ] >>>>>>[CreatedById]
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [ FK_UserPosts]
    FOREIGN KEY ([CreatedById])
    REFERENCES [dbo].[Users]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;


 --add foreign key from table [ObjectTables] to [Posts] >>>>>>[ObjectTableId]
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [ FK_ObjectTablesPosts]
    FOREIGN KEY ([ObjectTableId] )
	
    REFERENCES [dbo].[ObjectTables]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;


 --add foreign key from table [Groups] to [Posts ] >>>>>>[GroupId]
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [ FK_GroupsPosts]
    FOREIGN KEY ([GroupId])
    REFERENCES [dbo].[Groups]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;




 --add foreign key from table [Posts] to [Posts ] >>>>>>[ParentPostId]
ALTER TABLE [dbo].[Posts]
ADD CONSTRAINT [ FK_ParentPostPosts]
    FOREIGN KEY ([ParentPostId])
    REFERENCES [dbo].[Posts]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;


--[Feeds]

 --add foreign key from table [Posts] to [Feeds ] >>>>>>[PostId]
ALTER TABLE [dbo].[Feeds]
ADD CONSTRAINT [ FK_PostsFeeds]
    FOREIGN KEY ([PostId] )
	
    REFERENCES [dbo].[Posts]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;


 --add foreign key from table [Users] to [Feeds ] >>>>>>[UserId]
ALTER TABLE [dbo].[Feeds]
ADD CONSTRAINT [ FK_UsersFeeds]
    FOREIGN KEY ([UserId] )
	
    REFERENCES [dbo].[Users]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;




--[Followers]
 --add foreign key from table [Users] to [Followers ] >>>>>>[FolloweeUserId ]
ALTER TABLE [dbo].[Followers]
ADD CONSTRAINT [ FK_UsersFollowees]
    FOREIGN KEY ([FolloweeUserId])
	
    REFERENCES [dbo].[Users]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;


 --add foreign key from table [Users] to [[Followers] ] >>>>>>[FollowerUserId]
ALTER TABLE [dbo].[Followers]
ADD CONSTRAINT [ FK_UsersFollowers]
    FOREIGN KEY ([FollowerUserId] )
	
    REFERENCES [dbo].[Users]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;




--[PostLikes ]
 --add foreign key from table [Posts] to [PostLikes ] >>>>>>[PostId]
ALTER TABLE [dbo].[PostLikes]
ADD CONSTRAINT [ FK_PostsPostLikes]
    FOREIGN KEY ([PostId] )
	
    REFERENCES [dbo].[Posts]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;



 --add foreign key from table [Users] to [ PostLikes] >>>>>>[UserId]
ALTER TABLE [dbo].[PostLikes]
ADD CONSTRAINT [ FK_UsersPostLikes]
    FOREIGN KEY ([UserId] )
	
    REFERENCES [dbo].[Users]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;



--[Groups]
 --add foreign key from table [Users] to [Groups] >>>>>>[OwnerId]
ALTER TABLE [dbo].[Groups]
ADD CONSTRAINT [ FK_UsersGroups]
    FOREIGN KEY ([OwnerId] )
	
    REFERENCES [dbo].[Users]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;





--[FollowEntities]

 --add foreign key from table [Users] to [FollowEntities] >>>>>>[FollowerUserId]
ALTER TABLE [dbo].[FollowEntities]
ADD CONSTRAINT [ FK_UsersFollowEntities]
    FOREIGN KEY ([FollowerUserId] )
	
    REFERENCES [dbo].[Users]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;


 --add foreign key from table [ObjectTables] to [FollowEntities] >>>>>>[ObjectTableId]
ALTER TABLE [dbo].[FollowEntities]
ADD CONSTRAINT [ FK_ObjectTablesFollowEntities]
    FOREIGN KEY ([ObjectTableId] )
	
    REFERENCES [dbo].[ObjectTables]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;






--[GroupMembers]

 --add foreign key from table [Groups] to [GroupMembers] >>>>>>[GroupId]
ALTER TABLE [dbo].[GroupMembers]
ADD CONSTRAINT [ FK_GroupsGroupMembers]
    FOREIGN KEY ([GroupId] )
	
    REFERENCES [dbo].[Groups]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;



 --add foreign key from table [Users] to [GroupMembers] >>>>>>[GroupId]
ALTER TABLE [dbo].[GroupMembers]
ADD CONSTRAINT [ FK_UsersGroupMembers]
    FOREIGN KEY ([UserId] )
	
    REFERENCES [dbo].[Users]
        ([Id])
ON DELETE NO ACTION ON UPDATE NO ACTION;


