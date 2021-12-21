IF NOT EXISTS (SELECT TOP 1 1 FROM SYS.ALL_OBJECTS WHERE [NAME] = 'User' AND [TYPE] = 'U') BEGIN
	CREATE TABLE [User](
		-- Entity properties
		Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY
		, InternalId UNIQUEIDENTIFIER NOT NULL
		, Deleted BIT NOT NULL
		
		, [Firstname] VARCHAR(100)
		, [Lastname] VARCHAR(100)
		
		, [Email] VARCHAR(250) UNIQUE
		, Username VARCHAR(100) UNIQUE
		, [Password] VARCHAR(254)

		, Active BIT DEFAULT(0)

	)

	-- DROP TABLE [User]
END
GO

IF NOT EXISTS (SELECT TOP 1 1 FROM [User] WHERE InternalId = 'D8E7CDDC-E581-49D1-BBD3-8226FF89BE44') BEGIN
	INSERT INTO [User](InternalId, Deleted, Firstname, Lastname, Email, Username, [Password], Active)
	SELECT 'D8E7CDDC-E581-49D1-BBD3-8226FF89BE44', 0, 'Mr. Root', '', 'dev@protoxos.com', 'root', '8a4d0c96230823eb48606a89da9ed348b4010348177d002d9d610d1c2c186c3c', 1
END
GO