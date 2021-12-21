/*
DROP TABLE Vehicle
DROP TABLE VehicleDrivers
DROP TABLE VehicleLicencePlates
DROP TABLE VehicleParkedIn
DROP TABLE VehicleStolen
DROP TABLE VehicleSightings
DROP TABLE VehicleSightingsMedia
DROP TABLE VehicleOwner
DROP TABLE VehicleOwnerDocuments
*/

-- TYPE=TABLE
IF NOT EXISTS (SELECT TOP 1 1 FROM SYS.ALL_OBJECTS WHERE [NAME] = 'Vehicle' AND [TYPE] = 'U') BEGIN
	CREATE TABLE Vehicle(
		-- Base properties
		Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY
		, InternalId UNIQUEIDENTIFIER NOT NULL
		, Deleted BIT NOT NULL
		, UserCreatorId INT

		-- Entity properties
		, Alias VARCHAR(50) NOT NULL DEFAULT('')
		, Brand VARCHAR(50) NOT NULL DEFAULT('')
		, Model VARCHAR(50) NOT NULL DEFAULT('')
        , [Year] INT NOT NULL
		, Vin VARCHAR(32) NOT NULL
		, Color VARCHAR(20) NULL
		, CreationTime DATETIME NOT NULL
		
	)
END
GO
-- TYPE=TABLE
IF NOT EXISTS (SELECT TOP 1 1 FROM SYS.ALL_OBJECTS WHERE [NAME] = 'VehicleDrivers' AND [TYPE] = 'U') BEGIN
	CREATE TABLE VehicleDrivers(
		-- Base properties
		VehicleId INT
		, UserId INT
	)
END
GO
-- TYPE=TABLE
IF NOT EXISTS (SELECT TOP 1 1 FROM SYS.ALL_OBJECTS WHERE [NAME] = 'VehicleLicencePlates' AND [TYPE] = 'U') BEGIN
	CREATE TABLE VehicleLicencePlates(
		-- Base properties
		Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY
		, InternalId UNIQUEIDENTIFIER NOT NULL
		, Deleted BIT NOT NULL

		-- Entity properties
		, VehicleId INT
		, LicencePlate VARCHAR(15) NOT NULL DEFAULT('')
        , [Image] IMAGE NULL
		, ValidSince DATETIME NULL
		, ValidUntil DATETIME NULL
		, LastModify DATETIME NOT NULL
	)
END
GO

-- TYPE=TABLE
IF NOT EXISTS (SELECT TOP 1 1 FROM SYS.ALL_OBJECTS WHERE [NAME] = 'VehicleParkedIn' AND [TYPE] = 'U') BEGIN
	CREATE TABLE VehicleParkedIn(
		-- Entity properties
		Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY
		, VehicleId INT
		, Latitude INT
		, Longitude INT
		, [Message] VARCHAR(250) NULL

		, ParkinTime DATETIME NOT NULL
		, UnparkinTime DATETIME NULL
	)
END
GO
-- TYPE=TABLE
IF NOT EXISTS (SELECT TOP 1 1 FROM SYS.ALL_OBJECTS WHERE [NAME] = 'VehicleStolen' AND [TYPE] = 'U') BEGIN
	CREATE TABLE VehicleStolen(
		-- Entity properties
		Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY
		, VehicleId INT
		, Latitude INT
		, Longitude INT
		, StolenTime DATETIME NOT NULL
		, RecoveryTime DATETIME NULL
	)
END
GO
-- TYPE=TABLE
IF NOT EXISTS (SELECT TOP 1 1 FROM SYS.ALL_OBJECTS WHERE [NAME] = 'VehicleSightings' AND [TYPE] = 'U') BEGIN
	CREATE TABLE VehicleSightings(
		-- Entity properties
		StolenId INT
		, UserId INT
		, Latitude INT
		, Longitude INT
		, SightTime DATETIME NOT NULL
	)
END
GO
-- TYPE=TABLE
IF NOT EXISTS (SELECT TOP 1 1 FROM SYS.ALL_OBJECTS WHERE [NAME] = 'VehicleSightingsMedia' AND [TYPE] = 'U') BEGIN
	CREATE TABLE VehicleSightingsMedia(
		-- Entity properties
		SightId INT
		, Media BINARY
		, UploadTime DATETIME
	)
END
GO

-- TYPE=TABLE
IF NOT EXISTS (SELECT TOP 1 1 FROM SYS.ALL_OBJECTS WHERE [NAME] = 'VehicleOwner' AND [TYPE] = 'U') BEGIN
	CREATE TABLE VehicleOwner(
		-- Entity properties
		VehicleId INT
		, UserId INT
		, Verified BIT
		, LastOwner BIT
		, OwnDate DATETIME
	)
END
GO

-- TYPE=TABLE
IF NOT EXISTS (SELECT TOP 1 1 FROM SYS.ALL_OBJECTS WHERE [NAME] = 'VehicleOwnerDocuments' AND [TYPE] = 'U') BEGIN
	CREATE TABLE VehicleOwnerDocuments(
		-- Entity properties
		VehicleId INT
		, UserId INT

		, [Name] VARCHAR(100)
		, [Image] IMAGE

		, Valid BIT
		, UploadTime DATETIME
		, ValidationTime DATETIME
	)
END
GO