CREATE TABLE [dbo].[user](
	[ID] [int] IDENTITY(0,1) NOT NULL,
	[name] [nvarchar](20) NOT NULL,
	[patronymic] [nvarchar](15) NOT NULL,
	[lastname] [nvarchar](20) NOT NULL,
	[email] [nvarchar](20) NOT NULL
) ON [PRIMARY]