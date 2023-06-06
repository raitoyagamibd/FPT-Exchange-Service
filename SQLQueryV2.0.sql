CREATE DATABASE FPT_Exchange_DB
GO
USE FPT_Exchange_DB
GO

CREATE TABLE [User] (
  Id uniqueidentifier primary key NOT NULL,
  [Name] nvarchar(256) NOT NULL,
  [Avatar] nvarchar(max),
  [Email] nvarchar(256) NOT NULL,
  [Password] nvarchar(256) NOT NULL,
  [AccessToken] nvarchar(256),
  [RefreshToken] nvarchar(256),
  [RoleID] uniqueidentifier foreign key references [Role](Id) NOT NULL,
  [StationID] uniqueidentifier foreign key references [Stations](Id),
  [WalletID] uniqueidentifier foreign key references [Wallet](Id),
  Status nvarchar(256) NOT NULL,
  [CreateAt] Datetime not null default getdate(),
  );
  GO

CREATE TABLE [Stations] (
  Id uniqueidentifier primary key NOT NULL,
  [Name] nvarchar(50),
  [Address] nvarchar(50),
);
GO

INSERT INTO [Stations] (Id, [Name], [Address])
VALUES 
  ('e1a45e68-3a83-4f94-83f2-4562b8a79f21', 'Station A', '123 Main Street'),
  ('f8b12c34-5d67-6e89-7a90-1b2c3d4e5f67', 'Station B', '456 Elm Street');

CREATE TABLE [Wallet] (
  Id uniqueidentifier primary key NOT NULL,
  [Score] int,
);
GO




CREATE TABLE [Role] (
  Id uniqueidentifier primary key NOT NULL,
  [Name] nvarchar(50) NOT NULL,
);
GO

INSERT INTO [Role] (Id, [Name])
VALUES
  ('3D2C1859-4E4F-4CFA-9EFB-AFBCD6A7A17D', 'Admin'),
  ('A530E7C2-9D44-4CE7-82F7-3EDD17B57734', 'Staff'),
  ('61B99726-8E73-4A0C-BE55-C7855BB0A0C3', 'Customer');

CREATE TABLE [Status] (
  Id uniqueidentifier primary key NOT NULL,
  [Name] nvarchar(50),
);
GO

CREATE TABLE [Category] (
  Id uniqueidentifier primary key NOT NULL,
  [Name] nvarchar(50),
);
GO

CREATE TABLE [Product] (
  Id uniqueidentifier primary key NOT NULL,
  [Name] nvarchar(50),
  [Description] nvarchar(256),
  [Price] int,
  [CategoryID] uniqueidentifier foreign key references [Category](Id) NOT NULL,
  [Status_ID] uniqueidentifier foreign key references [Status](Id) NOT NULL,
  [StationID] uniqueidentifier foreign key references [Stations](Id) NOT NULL,
  [AddByID] uniqueidentifier foreign key references [User](Id) NOT NULL,
  [SellerID] uniqueidentifier foreign key references [User](Id) NOT NULL,
  [BuyerID] uniqueidentifier foreign key references [User](Id) NOT NULL,
  [CreatedAt] Datetime not null default getdate(),
  
);
GO

CREATE TABLE [ProductActivy] (
  Id uniqueidentifier primary key NOT NULL,
  [ActionType] nvarchar(50),
  [UserID] uniqueidentifier foreign key references [User](Id) NOT NULL,
  [ProductID] uniqueidentifier foreign key references [Product](Id) NOT NULL,
  [Stations_ID] uniqueidentifier foreign key references [Stations](Id) NOT NULL,
  [OldStatus] uniqueidentifier foreign key references [Status](Id) NOT NULL,
  [NewStatus] uniqueidentifier foreign key references [Status](Id),
  [CreatedDate] Datetime not null default getdate(),
  
);
GO



CREATE TABLE [ProductTransfer] (
  Id uniqueidentifier primary key NOT NULL,
  [StationIDForm] uniqueidentifier foreign key references [Stations](Id) NOT NULL,
  [StationIDTo] uniqueidentifier foreign key references [Stations](Id) NOT NULL,
  [UserID] uniqueidentifier foreign key references [User](Id) NOT NULL,
  [DateTime] datetime not null default getdate(),
  
);
GO

CREATE TABLE [Transaction] (
  Id uniqueidentifier primary key NOT NULL,
  [ProductID] uniqueidentifier foreign key references [Product](Id) NOT NULL,
  [WalletID] uniqueidentifier foreign key references [Wallet](Id) NOT NULL,
  [Amount] int,
  [Fee] int,
  [Receive] int,
  [CreatedDate] Datetime not null default getdate(),
  
);
GO

CREATE TABLE [ImageProduct] (
  Id uniqueidentifier primary key NOT NULL,
  [Product_ID] uniqueidentifier foreign key references [Product](Id) NOT NULL,
  Url nvarchar(256),
);
GO

CREATE TABLE [Notification] (
  Id uniqueidentifier primary key NOT NULL,
  [Description] nvarchar(max),
  [CreateAt] DateTime not null default getdate(),
  [SendTo] uniqueidentifier foreign key references [User](Id) NOT NULL,
  
);
GO

CREATE TABLE [ProductTransfer_Item] (
  Id uniqueidentifier primary key NOT NULL,
  [ProductID] uniqueidentifier foreign key references [Product](Id) NOT NULL,
  [ProductTransferID] uniqueidentifier foreign key references [ProductTransfer](Id) NOT NULL,
  [Status] nvarchar(50) NOT NULL,
  
);
GO

