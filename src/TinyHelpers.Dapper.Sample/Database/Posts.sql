CREATE TABLE [dbo].[Posts](
	[Id] [uniqueidentifier] NOT NULL,
	[Title] [nvarchar](80) NOT NULL,
	[Content] [nvarchar](max) NOT NULL,
	[Authors] [nvarchar](max) NOT NULL,
	[Date] [smalldatetime] NOT NULL,
	[Reviews] [nvarchar](max) NULL,
 CONSTRAINT [PK_Posts] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO