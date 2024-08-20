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

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllRooms]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllRooms
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllRooms]    Script Date: 20/08/2024 20:00:18 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 29/04/2024
-- Description:	Get all details of rooms
-- =============================================

/**************
    modifications

	001               Nitharsha           20/08/2024             Change this SP based on Rooms
***************/

CREATE PROCEDURE [dbo].[GetAllRooms]
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve all room details along with associated picture details
    SELECT 
	R.RoomId, 
	R.RoomNumber,
	R.RoomTypeId,
	R.Created,
	R.CreatedBy,
	R.Updated,
	R.UpdatedBy
    FROM Rooms R
END
GO

