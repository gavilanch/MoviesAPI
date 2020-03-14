CREATE TABLE [AspNetRoles] (
    [Id] nvarchar(450) NOT NULL,
    [Name] nvarchar(256) NULL,
    [NormalizedName] nvarchar(256) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [AspNetUsers] (
    [Id] nvarchar(450) NOT NULL,
    [UserName] nvarchar(256) NULL,
    [NormalizedUserName] nvarchar(256) NULL,
    [Email] nvarchar(256) NULL,
    [NormalizedEmail] nvarchar(256) NULL,
    [EmailConfirmed] bit NOT NULL,
    [PasswordHash] nvarchar(max) NULL,
    [SecurityStamp] nvarchar(max) NULL,
    [ConcurrencyStamp] nvarchar(max) NULL,
    [PhoneNumber] nvarchar(max) NULL,
    [PhoneNumberConfirmed] bit NOT NULL,
    [TwoFactorEnabled] bit NOT NULL,
    [LockoutEnd] datetimeoffset NULL,
    [LockoutEnabled] bit NOT NULL,
    [AccessFailedCount] int NOT NULL,
    CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Genres] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(40) NOT NULL,
    CONSTRAINT [PK_Genres] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [Movies] (
    [Id] int NOT NULL IDENTITY,
    [Title] nvarchar(300) NOT NULL,
    [Summary] nvarchar(max) NULL,
    [InTheaters] bit NOT NULL,
    [ReleaseDate] datetime2 NOT NULL,
    [Poster] nvarchar(max) NULL,
    CONSTRAINT [PK_Movies] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [MovieTheaters] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NOT NULL,
    [Location] geography NULL,
    CONSTRAINT [PK_MovieTheaters] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [People] (
    [Id] int NOT NULL IDENTITY,
    [Name] nvarchar(120) NOT NULL,
    [Biography] nvarchar(max) NULL,
    [DateOfBirth] datetime2 NOT NULL,
    [Picture] nvarchar(max) NULL,
    CONSTRAINT [PK_People] PRIMARY KEY ([Id])
);
GO


CREATE TABLE [AspNetRoleClaims] (
    [Id] int NOT NULL IDENTITY,
    [RoleId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserClaims] (
    [Id] int NOT NULL IDENTITY,
    [UserId] nvarchar(450) NOT NULL,
    [ClaimType] nvarchar(max) NULL,
    [ClaimValue] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserLogins] (
    [LoginProvider] nvarchar(450) NOT NULL,
    [ProviderKey] nvarchar(450) NOT NULL,
    [ProviderDisplayName] nvarchar(max) NULL,
    [UserId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
    CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserRoles] (
    [UserId] nvarchar(450) NOT NULL,
    [RoleId] nvarchar(450) NOT NULL,
    CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
    CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [AspNetUserTokens] (
    [UserId] nvarchar(450) NOT NULL,
    [LoginProvider] nvarchar(450) NOT NULL,
    [Name] nvarchar(450) NOT NULL,
    [Value] nvarchar(max) NULL,
    CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
    CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [MoviesGenres] (
    [MovieId] int NOT NULL,
    [GenreId] int NOT NULL,
    CONSTRAINT [PK_MoviesGenres] PRIMARY KEY ([GenreId], [MovieId]),
    CONSTRAINT [FK_MoviesGenres_Genres_GenreId] FOREIGN KEY ([GenreId]) REFERENCES [Genres] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MoviesGenres_Movies_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movies] ([Id]) ON DELETE CASCADE
);
GO


CREATE TABLE [MoviesActors] (
    [PersonId] int NOT NULL,
    [MovieId] int NOT NULL,
    [Character] nvarchar(max) NULL,
    [Order] int NOT NULL,
    CONSTRAINT [PK_MoviesActors] PRIMARY KEY ([PersonId], [MovieId]),
    CONSTRAINT [FK_MoviesActors_Movies_MovieId] FOREIGN KEY ([MovieId]) REFERENCES [Movies] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_MoviesActors_People_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [People] ([Id]) ON DELETE CASCADE
);
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Genres]'))
    SET IDENTITY_INSERT [Genres] ON;
INSERT INTO [Genres] ([Id], [Name])
VALUES (4, N'Adventure'),
(5, N'Animation'),
(6, N'Drama'),
(7, N'Romance');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Name') AND [object_id] = OBJECT_ID(N'[Genres]'))
    SET IDENTITY_INSERT [Genres] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Location', N'Name') AND [object_id] = OBJECT_ID(N'[MovieTheaters]'))
    SET IDENTITY_INSERT [MovieTheaters] ON;
INSERT INTO [MovieTheaters] ([Id], [Location], [Name])
VALUES (1, geography::Parse('POINT (-69.9388777 18.4839233)'), N'Agora'),
(2, geography::Parse('POINT (-69.9118804 18.4826214)'), N'Sambil'),
(3, geography::Parse('POINT (-69.856427 18.506934)'), N'Megacentro'),
(4, geography::Parse('POINT (-73.986227 40.730898)'), N'Village East Cinema');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Location', N'Name') AND [object_id] = OBJECT_ID(N'[MovieTheaters]'))
    SET IDENTITY_INSERT [MovieTheaters] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'InTheaters', N'Poster', N'ReleaseDate', N'Summary', N'Title') AND [object_id] = OBJECT_ID(N'[Movies]'))
    SET IDENTITY_INSERT [Movies] ON;
INSERT INTO [Movies] ([Id], [InTheaters], [Poster], [ReleaseDate], [Summary], [Title])
VALUES (2, CAST(1 AS bit), NULL, '2019-04-26T00:00:00.0000000', NULL, N'Avengers: Endgame'),
(3, CAST(0 AS bit), NULL, '2019-04-26T00:00:00.0000000', NULL, N'Avengers: Infinity Wars'),
(4, CAST(0 AS bit), NULL, '2020-02-28T00:00:00.0000000', NULL, N'Sonic the Hedgehog'),
(5, CAST(0 AS bit), NULL, '2020-02-21T00:00:00.0000000', NULL, N'Emma'),
(6, CAST(0 AS bit), NULL, '2020-02-21T00:00:00.0000000', NULL, N'Greed');
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'InTheaters', N'Poster', N'ReleaseDate', N'Summary', N'Title') AND [object_id] = OBJECT_ID(N'[Movies]'))
    SET IDENTITY_INSERT [Movies] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Biography', N'DateOfBirth', N'Name', N'Picture') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] ON;
INSERT INTO [People] ([Id], [Biography], [DateOfBirth], [Name], [Picture])
VALUES (5, NULL, '1962-01-17T00:00:00.0000000', N'Jim Carrey', NULL),
(6, NULL, '1965-04-04T00:00:00.0000000', N'Robert Downey Jr.', NULL),
(7, NULL, '1981-06-13T00:00:00.0000000', N'Chris Evans', NULL);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'Id', N'Biography', N'DateOfBirth', N'Name', N'Picture') AND [object_id] = OBJECT_ID(N'[People]'))
    SET IDENTITY_INSERT [People] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PersonId', N'MovieId', N'Character', N'Order') AND [object_id] = OBJECT_ID(N'[MoviesActors]'))
    SET IDENTITY_INSERT [MoviesActors] ON;
INSERT INTO [MoviesActors] ([PersonId], [MovieId], [Character], [Order])
VALUES (5, 4, N'Dr. Ivo Robotnik', 1),
(6, 2, N'Tony Stark', 1),
(6, 3, N'Tony Stark', 1),
(7, 2, N'Steve Rogers', 2),
(7, 3, N'Steve Rogers', 2);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'PersonId', N'MovieId', N'Character', N'Order') AND [object_id] = OBJECT_ID(N'[MoviesActors]'))
    SET IDENTITY_INSERT [MoviesActors] OFF;
GO


IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'GenreId', N'MovieId') AND [object_id] = OBJECT_ID(N'[MoviesGenres]'))
    SET IDENTITY_INSERT [MoviesGenres] ON;
INSERT INTO [MoviesGenres] ([GenreId], [MovieId])
VALUES (6, 2),
(4, 2),
(6, 3),
(4, 3),
(4, 4),
(6, 5),
(7, 5),
(6, 6),
(7, 6);
IF EXISTS (SELECT * FROM [sys].[identity_columns] WHERE [name] IN (N'GenreId', N'MovieId') AND [object_id] = OBJECT_ID(N'[MoviesGenres]'))
    SET IDENTITY_INSERT [MoviesGenres] OFF;
GO


CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
GO


CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
GO


CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
GO


CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
GO


CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
GO


CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
GO


CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
GO


CREATE INDEX [IX_MoviesActors_MovieId] ON [MoviesActors] ([MovieId]);
GO


CREATE INDEX [IX_MoviesGenres_MovieId] ON [MoviesGenres] ([MovieId]);
GO


