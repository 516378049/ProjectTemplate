
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/16/2020 16:05:43
-- Generated from EDMX file: D:\GitHub\netTemplateMVC\WebApplication1\Models\Model1.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [OrderMeal];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------


-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[foods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[foods];
GO
IF OBJECT_ID(N'[dbo].[goods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[goods];
GO
IF OBJECT_ID(N'[dbo].[ratings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ratings];
GO
IF OBJECT_ID(N'[dbo].[sellers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sellers];
GO
IF OBJECT_ID(N'[dbo].[supports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[supports];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'foods'
CREATE TABLE [dbo].[foods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(50)  NOT NULL,
    [price] decimal(18,2)  NOT NULL,
    [oldPrice] decimal(18,2)  NULL,
    [description] nvarchar(500)  NULL,
    [sellCount] int  NULL,
    [rating] int  NULL,
    [info] nvarchar(2000)  NULL,
    [icon] nvarchar(max)  NULL,
    [image] nvarchar(max)  NULL,
    [DelFlag] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpdateTime] datetime  NOT NULL
);
GO

-- Creating table 'goods'
CREATE TABLE [dbo].[goods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NULL,
    [type] int  NOT NULL,
    [DelFlag] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpdateTime] datetime  NOT NULL
);
GO

-- Creating table 'ratings'
CREATE TABLE [dbo].[ratings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [username] nvarchar(max)  NULL,
    [rateTime] int  NOT NULL,
    [deliveryTime] int  NOT NULL,
    [score] int  NOT NULL,
    [rateType] int  NOT NULL,
    [text] nvarchar(max)  NULL,
    [avatar] nvarchar(max)  NULL,
    [DelFlag] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpdateTime] datetime  NOT NULL
);
GO

-- Creating table 'sellers'
CREATE TABLE [dbo].[sellers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NULL,
    [description] nvarchar(2000)  NULL,
    [deliveryTime] int  NOT NULL,
    [score] int  NOT NULL,
    [serviceScore] int  NOT NULL,
    [foodScore] int  NOT NULL,
    [rankRate] int  NOT NULL,
    [minPrice] decimal(18,2)  NOT NULL,
    [deliveryPrice] decimal(18,2)  NOT NULL,
    [ratingCount] int  NOT NULL,
    [sellCount] int  NOT NULL,
    [bulletin] nvarchar(max)  NULL,
    [avatar] nvarchar(max)  NULL,
    [DelFlag] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpdateTime] datetime  NOT NULL
);
GO

-- Creating table 'supports'
CREATE TABLE [dbo].[supports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [type] int  NOT NULL,
    [description] nvarchar(max)  NULL,
    [DelFlag] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpdateTime] datetime  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Id] in table 'foods'
ALTER TABLE [dbo].[foods]
ADD CONSTRAINT [PK_foods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'goods'
ALTER TABLE [dbo].[goods]
ADD CONSTRAINT [PK_goods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ratings'
ALTER TABLE [dbo].[ratings]
ADD CONSTRAINT [PK_ratings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'sellers'
ALTER TABLE [dbo].[sellers]
ADD CONSTRAINT [PK_sellers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'supports'
ALTER TABLE [dbo].[supports]
ADD CONSTRAINT [PK_supports]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------