
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 10/22/2020 20:24:05
-- Generated from EDMX file: C:\CurrentProjects\CharsooServer\CharsooWebAPI\Models\CharsooDataModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [charsoog_DBEntities];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CategoryParent]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Categories] DROP CONSTRAINT [FK_CategoryParent];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryPrerequisite]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Categories] DROP CONSTRAINT [FK_CategoryPrerequisite];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryPuzzle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Puzzles] DROP CONSTRAINT [FK_CategoryPuzzle];
GO
IF OBJECT_ID(N'[dbo].[FK_LogIn_PlayerInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[LogIns] DROP CONSTRAINT [FK_LogIn_PlayerInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_Purchases_PlayerInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Purchases] DROP CONSTRAINT [FK_Purchases_PlayerInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_CategoryUserPuzzle]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[UserPuzzles] DROP CONSTRAINT [FK_CategoryUserPuzzle];
GO
IF OBJECT_ID(N'[dbo].[FK_PlayerID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PuzzleRates] DROP CONSTRAINT [FK_PlayerID];
GO
IF OBJECT_ID(N'[dbo].[FK_PuzzleID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PuzzleRates] DROP CONSTRAINT [FK_PuzzleID];
GO
IF OBJECT_ID(N'[dbo].[FK_PushIDs_PlayerInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PushIDs] DROP CONSTRAINT [FK_PushIDs_PlayerInfo];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Puzzles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Puzzles];
GO
IF OBJECT_ID(N'[dbo].[PlayerInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayerInfoes];
GO
IF OBJECT_ID(N'[dbo].[LogIns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LogIns];
GO
IF OBJECT_ID(N'[dbo].[Purchases]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Purchases];
GO
IF OBJECT_ID(N'[dbo].[UserPuzzles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserPuzzles];
GO
IF OBJECT_ID(N'[dbo].[PlayPuzzles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PlayPuzzles];
GO
IF OBJECT_ID(N'[dbo].[PuzzleRates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PuzzleRates];
GO
IF OBJECT_ID(N'[dbo].[PushIDs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PushIDs];
GO
IF OBJECT_ID(N'[dbo].[Rewards]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Rewards];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Icon] nvarchar(20)  NOT NULL,
    [Price] int  NOT NULL,
    [Visit] bit  NOT NULL,
    [Row] int  NOT NULL,
    [LastUpdate] datetime  NOT NULL,
    [ParentID] int  NULL,
    [PrerequisiteID] int  NULL
);
GO

-- Creating table 'Puzzles'
CREATE TABLE [dbo].[Puzzles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Clue] nvarchar(50)  NOT NULL,
    [Content] nvarchar(1500)  NOT NULL,
    [Row] int  NOT NULL,
    [Solved] bit  NOT NULL,
    [Paid] bit  NOT NULL,
    [CategoryID] int  NULL,
    [LastUpdate] datetime  NOT NULL,
    [CreatorID] int  NULL,
    [Rate] int  NULL,
    [PlayCount] int  NULL
);
GO

-- Creating table 'PlayerInfoes'
CREATE TABLE [dbo].[PlayerInfoes] (
    [PlayerID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(50)  NOT NULL,
    [Avatar] nvarchar(500)  NOT NULL,
    [Telephone] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [CoinCount] int  NOT NULL,
    [Dirty] bit  NOT NULL,
    [HasDubler] bit  NOT NULL
);
GO

-- Creating table 'LogIns'
CREATE TABLE [dbo].[LogIns] (
    [PlayerID] int  NOT NULL,
    [DeviceID] varchar(150)  NOT NULL,
    [LoginTime] datetime  NOT NULL,
    [Latitude] float  NOT NULL,
    [Longitude] float  NOT NULL
);
GO

-- Creating table 'Purchases'
CREATE TABLE [dbo].[Purchases] (
    [PlayerID] int  NOT NULL,
    [PurchaseID] nchar(15)  NOT NULL,
    [LastUpdate] datetime  NOT NULL,
    [Dirty] bit  NOT NULL
);
GO

-- Creating table 'UserPuzzles'
CREATE TABLE [dbo].[UserPuzzles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ClientID] int  NOT NULL,
    [CreatorID] int  NOT NULL,
    [Clue] nvarchar(50)  NOT NULL,
    [Content] nvarchar(1500)  NOT NULL,
    [CategoryID] int  NULL,
    [LastUpdate] datetime  NOT NULL,
    [PlayCount] int  NULL
);
GO

-- Creating table 'PlayPuzzles'
CREATE TABLE [dbo].[PlayPuzzles] (
    [PlayerID] int  NOT NULL,
    [PuzzleID] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [MoveCount] int  NOT NULL,
    [HintCount1] int  NOT NULL,
    [HintCount2] int  NOT NULL,
    [HintCount3] int  NOT NULL,
    [Success] bit  NOT NULL,
    [Duration] int  NOT NULL,
    [Dirty] bit  NOT NULL
);
GO

-- Creating table 'PuzzleRates'
CREATE TABLE [dbo].[PuzzleRates] (
    [PuzzleID] int  NOT NULL,
    [PlayerID] int  NOT NULL,
    [Rate] int  NOT NULL
);
GO

-- Creating table 'PushIDs'
CREATE TABLE [dbo].[PushIDs] (
    [PlayerID] int  NOT NULL,
    [PID] nvarchar(50)  NOT NULL
);
GO

-- Creating table 'Rewards'
CREATE TABLE [dbo].[Rewards] (
    [RewardID] int  NOT NULL,
    [PlayerID] int  NOT NULL,
    [RewardAmount] int  NOT NULL,
    [RewardCouse] nvarchar(50)  NOT NULL,
    [LastUpdate] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Puzzles'
ALTER TABLE [dbo].[Puzzles]
ADD CONSTRAINT [PK_Puzzles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [PlayerID] in table 'PlayerInfoes'
ALTER TABLE [dbo].[PlayerInfoes]
ADD CONSTRAINT [PK_PlayerInfoes]
    PRIMARY KEY CLUSTERED ([PlayerID] ASC);
GO

-- Creating primary key on [PlayerID], [DeviceID], [LoginTime] in table 'LogIns'
ALTER TABLE [dbo].[LogIns]
ADD CONSTRAINT [PK_LogIns]
    PRIMARY KEY CLUSTERED ([PlayerID], [DeviceID], [LoginTime] ASC);
GO

-- Creating primary key on [PlayerID], [PurchaseID] in table 'Purchases'
ALTER TABLE [dbo].[Purchases]
ADD CONSTRAINT [PK_Purchases]
    PRIMARY KEY CLUSTERED ([PlayerID], [PurchaseID] ASC);
GO

-- Creating primary key on [ID] in table 'UserPuzzles'
ALTER TABLE [dbo].[UserPuzzles]
ADD CONSTRAINT [PK_UserPuzzles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [PlayerID], [PuzzleID], [Time] in table 'PlayPuzzles'
ALTER TABLE [dbo].[PlayPuzzles]
ADD CONSTRAINT [PK_PlayPuzzles]
    PRIMARY KEY CLUSTERED ([PlayerID], [PuzzleID], [Time] ASC);
GO

-- Creating primary key on [PuzzleID], [PlayerID] in table 'PuzzleRates'
ALTER TABLE [dbo].[PuzzleRates]
ADD CONSTRAINT [PK_PuzzleRates]
    PRIMARY KEY CLUSTERED ([PuzzleID], [PlayerID] ASC);
GO

-- Creating primary key on [PID] in table 'PushIDs'
ALTER TABLE [dbo].[PushIDs]
ADD CONSTRAINT [PK_PushIDs]
    PRIMARY KEY CLUSTERED ([PID] ASC);
GO

-- Creating primary key on [RewardID] in table 'Rewards'
ALTER TABLE [dbo].[Rewards]
ADD CONSTRAINT [PK_Rewards]
    PRIMARY KEY CLUSTERED ([RewardID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [ParentID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [FK_CategoryParent]
    FOREIGN KEY ([ParentID])
    REFERENCES [dbo].[Categories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryParent'
CREATE INDEX [IX_FK_CategoryParent]
ON [dbo].[Categories]
    ([ParentID]);
GO

-- Creating foreign key on [PrerequisiteID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [FK_CategoryPrerequisite]
    FOREIGN KEY ([PrerequisiteID])
    REFERENCES [dbo].[Categories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryPrerequisite'
CREATE INDEX [IX_FK_CategoryPrerequisite]
ON [dbo].[Categories]
    ([PrerequisiteID]);
GO

-- Creating foreign key on [CategoryID] in table 'Puzzles'
ALTER TABLE [dbo].[Puzzles]
ADD CONSTRAINT [FK_CategoryPuzzle]
    FOREIGN KEY ([CategoryID])
    REFERENCES [dbo].[Categories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryPuzzle'
CREATE INDEX [IX_FK_CategoryPuzzle]
ON [dbo].[Puzzles]
    ([CategoryID]);
GO

-- Creating foreign key on [PlayerID] in table 'LogIns'
ALTER TABLE [dbo].[LogIns]
ADD CONSTRAINT [FK_LogIn_PlayerInfo]
    FOREIGN KEY ([PlayerID])
    REFERENCES [dbo].[PlayerInfoes]
        ([PlayerID])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PlayerID] in table 'Purchases'
ALTER TABLE [dbo].[Purchases]
ADD CONSTRAINT [FK_Purchases_PlayerInfo]
    FOREIGN KEY ([PlayerID])
    REFERENCES [dbo].[PlayerInfoes]
        ([PlayerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [CategoryID] in table 'UserPuzzles'
ALTER TABLE [dbo].[UserPuzzles]
ADD CONSTRAINT [FK_CategoryUserPuzzle]
    FOREIGN KEY ([CategoryID])
    REFERENCES [dbo].[Categories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CategoryUserPuzzle'
CREATE INDEX [IX_FK_CategoryUserPuzzle]
ON [dbo].[UserPuzzles]
    ([CategoryID]);
GO

-- Creating foreign key on [PlayerID] in table 'PuzzleRates'
ALTER TABLE [dbo].[PuzzleRates]
ADD CONSTRAINT [FK_PlayerID]
    FOREIGN KEY ([PlayerID])
    REFERENCES [dbo].[PlayerInfoes]
        ([PlayerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PlayerID'
CREATE INDEX [IX_FK_PlayerID]
ON [dbo].[PuzzleRates]
    ([PlayerID]);
GO

-- Creating foreign key on [PuzzleID] in table 'PuzzleRates'
ALTER TABLE [dbo].[PuzzleRates]
ADD CONSTRAINT [FK_PuzzleID]
    FOREIGN KEY ([PuzzleID])
    REFERENCES [dbo].[UserPuzzles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [PlayerID] in table 'PushIDs'
ALTER TABLE [dbo].[PushIDs]
ADD CONSTRAINT [FK_PushIDs_PlayerInfo]
    FOREIGN KEY ([PlayerID])
    REFERENCES [dbo].[PlayerInfoes]
        ([PlayerID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PushIDs_PlayerInfo'
CREATE INDEX [IX_FK_PushIDs_PlayerInfo]
ON [dbo].[PushIDs]
    ([PlayerID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------