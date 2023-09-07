USE [Cards]
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'2317cd98-bb61-427b-8571-693169800dfd', N'admin', N'ADMIN', N'admin@example.com', N'ADMIN@EXAMPLE.COM', 0, N'AQAAAAEAACcQAAAAEJqcWlh4yNbLRZyMm4wfVBv4sL4YvWcvkbf9OChDUWh+8a5REpoMA4qNsy+C2LSi9w==', N'YPIITED6GCJH6MT62DTJBWQVXOABKLFY', N'18eb3236-dfc8-4f1a-a2d8-0db6965c8958', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetUsers] ([Id], [UserName], [NormalizedUserName], [Email], [NormalizedEmail], [EmailConfirmed], [PasswordHash], [SecurityStamp], [ConcurrencyStamp], [PhoneNumber], [PhoneNumberConfirmed], [TwoFactorEnabled], [LockoutEnd], [LockoutEnabled], [AccessFailedCount]) VALUES (N'6adf8e15-c39c-49ae-9082-1182814a20d2', N'user', N'USER', N'user@example.com', N'USER@EXAMPLE.COM', 0, N'AQAAAAEAACcQAAAAEO8hsxoRfM7qqzieQPQ+IfWQf2Z6X7oxS+AVUcwTEZrTGb5x+pBL3NquNzFtAlGwAw==', N'N5MUDKROPXMKTE2JUKQALW6RUZ3C6E73', N'07be7d00-e4fa-4251-b8ef-10a90c46ca3d', NULL, 0, 0, NULL, 1, 0)
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'642A2EBD-1B2F-4321-B294-028C3A27E0A2', N'User', N'USER', N'1FE5B345-A5E0-496E-8956-B48D21AA37CC')
GO
INSERT [dbo].[AspNetRoles] ([Id], [Name], [NormalizedName], [ConcurrencyStamp]) VALUES (N'D2A35AEC-402A-4BE6-9D7F-682C0B5D3FEF', N'Admin', N'ADMIN', N'BD480BB1-7D73-4393-AA09-51B11AAC949E')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'6adf8e15-c39c-49ae-9082-1182814a20d2', N'642A2EBD-1B2F-4321-B294-028C3A27E0A2')
GO
INSERT [dbo].[AspNetUserRoles] ([UserId], [RoleId]) VALUES (N'2317cd98-bb61-427b-8571-693169800dfd', N'D2A35AEC-402A-4BE6-9D7F-682C0B5D3FEF')
GO
