IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Rooms' and column_name = 'IsBooked')
BEGIN
ALTER TABLE Rooms
DROP COLUMN IsBooked
END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Rooms' and column_name = 'BookedFrom')
BEGIN
ALTER TABLE Rooms
DROP COLUMN BookedFrom
END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Rooms' and column_name = 'BookedTo')
BEGIN
ALTER TABLE Rooms
DROP COLUMN BookedTo
END
GO

IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Rooms' and column_name = 'RoomType')
BEGIN
ALTER TABLE Rooms
DROP COLUMN RoomType
END
GO

