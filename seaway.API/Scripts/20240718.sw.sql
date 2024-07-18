IF OBJECT_ID(N'Activities', N'U') IS NOT NULL
BEGIN
    DROP TABLE Activities
END
GO

CREATE TABLE Activities(
    ActivityId INT IDENTITY NOT NULL PRIMARY KEY,
	Name VARCHAR(100) NOT NULL,
	Description VARCHAR(1000) NOT NULL,
	PictureId INT NULL,
	IsActive bit NOT NULL,
	CreatedBy VARCHAR(100) NOT NULL,
    Created DATETIME NOT NULL,
    UpdatedBy VARCHAR(100) NOT NULL,
    Updated DATETIME NOT NULL
);
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'BookingRooms' and column_name = 'updated')
BEGIN
EXEC sp_rename 'BookingRooms.updated', 'Updated', 'COLUMN'
END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Bookings' and column_name = 'InsertedTime')
BEGIN
ALTER TABLE Bookings
DROP COLUMN InsertedTime
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Bookings' and column_name = 'Created')
BEGIN
ALTER TABLE Bookings
ADD Created DATETIME NOT NULL
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Bookings' and column_name = 'Updated')
BEGIN
ALTER TABLE Bookings
ADD Updated DATETIME NOT NULL
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Customer' and column_name = 'Created')
BEGIN
ALTER TABLE Customer
ADD Created DATETIME NOT NULL
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Customer' and column_name = 'Updated')
BEGIN
ALTER TABLE Customer
ADD Updated DATETIME NOT NULL
END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Employee' and column_name = 'updated')
BEGIN
EXEC sp_rename 'Employee.updated', 'Updated', 'COLUMN'
END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Offers' and column_name = 'InsertedTime')
BEGIN
ALTER TABLE Offers
DROP COLUMN InsertedTime
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Offers' and column_name = 'CreatedBy')
BEGIN
ALTER TABLE Offers
ADD CreatedBy VARCHAR(100) NOT NULL
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Offers' and column_name = 'Created')
BEGIN
ALTER TABLE Offers
ADD Created DATETIME NOT NULL
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Offers' and column_name = 'UpdatedBy')
BEGIN
ALTER TABLE Offers
ADD UpdatedBy VARCHAR(100) NOT NULL
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Offers' and column_name = 'Updated')
BEGIN
ALTER TABLE Offers
ADD Updated DATETIME NOT NULL
END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'PicDocuments' and column_name = 'InsertedTime')
BEGIN
ALTER TABLE PicDocuments
DROP COLUMN InsertedTime
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'PicDocuments' and column_name = 'CreatedBy')
BEGIN
ALTER TABLE PicDocuments
ADD CreatedBy VARCHAR(100) NOT NULL
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'PicDocuments' and column_name = 'Created')
BEGIN
ALTER TABLE PicDocuments
ADD Created DATETIME NOT NULL
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'PicDocuments' and column_name = 'UpdatedBy')
BEGIN
ALTER TABLE PicDocuments
ADD UpdatedBy VARCHAR(100) NOT NULL
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'PicDocuments' and column_name = 'Updated')
BEGIN
ALTER TABLE PicDocuments
ADD Updated DATETIME NOT NULL
END
GO


