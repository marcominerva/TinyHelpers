CREATE TABLE [dbo].[Posts](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](80) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Authors] [nvarchar](max) NULL,
	[Date] [date] NULL,
	[Time] [time](7) NULL,
	[Reviews] [nvarchar](max) NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
))
GO