
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 07/03/2020 18:51:21
-- Generated from EDMX file: D:\GitHub\netTemplateMVC\Model\EF\Model1.edmx
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
IF OBJECT_ID(N'[dbo].[RatingsSellers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RatingsSellers];
GO
IF OBJECT_ID(N'[dbo].[sellers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[sellers];
GO
IF OBJECT_ID(N'[dbo].[supports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[supports];
GO
IF OBJECT_ID(N'[dbo].[Token]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Token];
GO
IF OBJECT_ID(N'[dbo].[UserInfo]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserInfo];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'foods'
CREATE TABLE [dbo].[foods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [goodId] int  NULL,
    [name] nvarchar(50)  NULL,
    [price] decimal(18,2)  NULL,
    [oldPrice] decimal(18,2)  NULL,
    [description] nvarchar(500)  NULL,
    [sellCount] int  NULL,
    [rating] int  NULL,
    [info] nvarchar(2000)  NULL,
    [icon] nvarchar(2000)  NULL,
    [image] nvarchar(2000)  NULL,
    [DelFlag] int  NULL,
    [CreateTime] datetime  NULL,
    [UpdateTime] datetime  NULL
);
GO

-- Creating table 'goods'
CREATE TABLE [dbo].[goods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [sellerId] int  NULL,
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
    [foodId] int  NULL,
    [username] nvarchar(200)  NULL,
    [rateTime] nvarchar(100)  NULL,
    [rateType] int  NULL,
    [text] nvarchar(1000)  NULL,
    [avatar] nvarchar(1000)  NULL,
    [DelFlag] int  NULL,
    [CreateTime] datetime  NULL,
    [UpdateTime] datetime  NULL
);
GO

-- Creating table 'RatingsSellers'
CREATE TABLE [dbo].[RatingsSellers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [sellerId] int  NULL,
    [username] nvarchar(200)  NULL,
    [rateTime] nvarchar(100)  NULL,
    [deliveryTime] nvarchar(100)  NULL,
    [score] decimal(18,2)  NULL,
    [rateType] int  NULL,
    [text] nvarchar(1000)  NULL,
    [avatar] nvarchar(1000)  NULL,
    [DelFlag] int  NULL,
    [CreateTime] datetime  NULL,
    [UpdateTime] datetime  NULL
);
GO

-- Creating table 'sellers'
CREATE TABLE [dbo].[sellers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [name] nvarchar(max)  NULL,
    [description] nvarchar(2000)  NULL,
    [deliveryTime] int  NULL,
    [score] decimal(18,6)  NULL,
    [serviceScore] decimal(18,6)  NULL,
    [foodScore] decimal(18,6)  NULL,
    [rankRate] decimal(18,6)  NULL,
    [minPrice] decimal(18,6)  NULL,
    [deliveryPrice] decimal(18,6)  NULL,
    [ratingCount] int  NULL,
    [sellCount] int  NULL,
    [bulletin] nvarchar(max)  NULL,
    [avatar] nvarchar(max)  NULL,
    [DelFlag] int  NULL,
    [CreateTime] datetime  NULL,
    [UpdateTime] datetime  NULL
);
GO

-- Creating table 'supports'
CREATE TABLE [dbo].[supports] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [sellerId] int  NULL,
    [type] int  NULL,
    [description] nvarchar(max)  NULL,
    [DelFlag] int  NULL,
    [CreateTime] datetime  NULL,
    [UpdateTime] datetime  NULL
);
GO

-- Creating table 'Token'
CREATE TABLE [dbo].[Token] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [userId] int  NULL,
    [access_token] nvarchar(100)  NULL,
    [expires_in] nvarchar(50)  NULL,
    [DelFlag] int  NOT NULL,
    [CreateTime] datetime  NOT NULL,
    [UpdateTime] datetime  NOT NULL
);
GO

-- Creating table 'UserInfo'
CREATE TABLE [dbo].[UserInfo] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [openid] nvarchar(50)  NULL,
    [nickname] nvarchar(50)  NULL,
    [sex] int  NULL,
    [province] nvarchar(100)  NULL,
    [city] nvarchar(100)  NULL,
    [country] nvarchar(100)  NULL,
    [headimgurl] nvarchar(100)  NULL,
    [privilege] nvarchar(100)  NULL,
    [unionid] nvarchar(200)  NULL,
    [access_token] nchar(10)  NULL,
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

-- Creating primary key on [Id] in table 'RatingsSellers'
ALTER TABLE [dbo].[RatingsSellers]
ADD CONSTRAINT [PK_RatingsSellers]
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

-- Creating primary key on [Id] in table 'Token'
ALTER TABLE [dbo].[Token]
ADD CONSTRAINT [PK_Token]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserInfo'
ALTER TABLE [dbo].[UserInfo]
ADD CONSTRAINT [PK_UserInfo]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------