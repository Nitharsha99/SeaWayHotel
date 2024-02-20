/*
   
   Creator: Nitharsha
   Purpose: drop unwanted tables and change some changes in exist table
   Date   : 20/02/2024

*/

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------drop Unwanted tables ----------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF OBJECT_ID(N'OfferPicture', N'U') IS NOT NULL
BEGIN
    DROP TABLE OfferPicture
END
GO

IF OBJECT_ID(N'ActivityPicture', N'U') IS NOT NULL
BEGIN
    DROP TABLE ActivityPicture
END
GO

IF OBJECT_ID(N'RoomPicture', N'U') IS NOT NULL
BEGIN
    DROP TABLE RoomPicture
END
GO

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------drop Unwanted column PictureId from tables -----------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------


IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Activities' and column_name = 'PictureId')
BEGIN
	ALTER TABLE Activities
	DROP CONSTRAINT FK_ActivityPic

	-- Drop the column
	ALTER TABLE Activities
	DROP COLUMN PictureId
END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Room' and column_name = 'PictureId')
BEGIN
	ALTER TABLE Room
	DROP CONSTRAINT FK_RoomPic

	-- Drop the column
	ALTER TABLE Room
	DROP COLUMN PictureId
END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Offers' and column_name = 'PictureId')
BEGIN
	ALTER TABLE Offers
	DROP CONSTRAINT FK_OfferPic

	-- Drop the column
	ALTER TABLE Offers
	DROP COLUMN PictureId
END
GO

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------drop PicDocument table -----------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF OBJECT_ID(N'PicDocuments', N'U') IS NOT NULL
BEGIN
    DROP TABLE PicDocuments
END
GO

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------Create new PicDocument Table -----------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

CREATE TABLE PicDocuments(
    PicDocumentId INT IDENTITY NOT NULL PRIMARY KEY,
	PicTypeId INT NOT NULL,
	PicType VARCHAR(20) NOT NULL,
	Name VARCHAR(50) NOT NULL,
	Value VARBINARY(MAX) NOT NULL,
	IsActive BIT NOT NULL,
	InsertedTime DATETIME NOT NULL
)
GO

