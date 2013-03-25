USE [TildeContents]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Folders]') AND type in (N'U'))
DROP TABLE [Folders]

CREATE TABLE [Folders](
	[FolderId] [uniqueidentifier] NOT NULL,
	[ParentId] [uniqueidentifier] NULL,
	[Name] [varchar](100) NOT NULL,
CONSTRAINT [PK_Folders] PRIMARY KEY CLUSTERED ([FolderId] ASC)) ON [PRIMARY]

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[Files]') AND type in (N'U'))
DROP TABLE [Files]

CREATE TABLE [Files](
	[FileId] [uniqueidentifier] NOT NULL,
	[FolderId] [uniqueidentifier] NOT NULL,
	[Name] [varchar](100) NOT NULL,
	[Text] [varchar](MAX) NULL,
CONSTRAINT [PK_Files] PRIMARY KEY CLUSTERED ([FileId] ASC)) ON [PRIMARY]