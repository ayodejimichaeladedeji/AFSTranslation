-- CREATE TABLE Users (
--     Id INT PRIMARY KEY IDENTITY,
--     Username NVARCHAR(100) NOT NULL UNIQUE,
--     PasswordHash NVARCHAR(256) NOT NULL
-- );

-- CREATE PROCEDURE sp_RegisterUser
--     @Username NVARCHAR(100),
--     @PasswordHash NVARCHAR(256)
-- AS
-- BEGIN
--     INSERT INTO Users (Username, PasswordHash)
--     VALUES (@Username, @PasswordHash)
-- END;

-- CREATE PROCEDURE sp_GetUserByUsername
--     @Username NVARCHAR(100)
-- AS
-- BEGIN
--     SELECT Id, Username, PasswordHash FROM Users
--     WHERE Username = @Username
-- END

-- IF OBJECT_ID('dbo.TranslationLogs', 'U') IS NOT NULL
--     DROP TABLE dbo.TranslationLogs;

-- CREATE TABLE TranslationLogs (
--     Id INT IDENTITY(1,1) PRIMARY KEY,
--     OriginalText NVARCHAR(MAX) NOT NULL,
--     TranslatedText NVARCHAR(MAX) NOT NULL,
--     Mode NVARCHAR(50) NOT NULL,
--     CreatedAt DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
--     UserId INT NOT NULL,

--     FOREIGN KEY (UserId) REFERENCES Users(Id)
-- );

-- CREATE OR ALTER PROCEDURE spInsertTranslationLog
--     @OriginalText NVARCHAR(MAX),
--     @TranslatedText NVARCHAR(MAX),
--     @Mode NVARCHAR(50),
--     @UserId INT
-- AS
-- BEGIN
--     SET NOCOUNT ON;

--     INSERT INTO TranslationLogs (OriginalText, TranslatedText, Mode, CreatedAt, UserId)
--     VALUES (@OriginalText, @TranslatedText, @Mode, GETUTCDATE(), @UserId);
-- END;

-- CREATE PROCEDURE spGetTranslationLogsByUser
--     @UserId INT
-- AS
-- BEGIN
--     SET NOCOUNT ON;

--     SELECT 
--         Id, OriginalText, TranslatedText, Mode, CreatedAt
--     FROM 
--         TranslationLogs
--     WHERE 
--         UserId = @UserId
--     ORDER BY 
--         CreatedAt DESC;
-- END;

SELECT * FROM [dbo].TranslationLogs;
SELECT * FROM [dbo].Users;
