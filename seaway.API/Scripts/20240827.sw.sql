IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Rooms' and column_name = 'RoomNumber')
BEGIN
ALTER TABLE Rooms
ALTER COLUMN RoomNumber VARCHAR(50) NOT NULL
END
GO

----------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[InsertNewRoom]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE InsertNewRoom
END
GO
/****** Object:  StoredProcedure [dbo].[InsertNewRoom]    Script Date: 27/08/2024 12:44:41 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nitharsha
-- Create date: 14-03-2024
-- Description:	Insert new room
-- =============================================

/*
   Modifications

*/

CREATE PROCEDURE [dbo].[InsertNewRoom]
	@roomNumber NVARCHAR(50),
	@roomTypeId INT,
	@createdBy NVARCHAR(100)
AS
BEGIN

	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT 1 FROM Rooms WHERE RoomNumber = @roomNumber)
	BEGIN
		INSERT INTO Rooms(RoomNumber, RoomTypeId, Created, CreatedBy, Updated, UpdatedBy)
		VALUES(@roomNumber, @roomTypeId, GETDATE(), @createdBy, GETDATE(), @createdBy)
	END

END
GO

------------------------------------------------------------------------------------------------------------------------------------------------------
------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateRoom]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UpdateRoom
END
GO
/****** Object:  StoredProcedure [dbo].[UpdateRoom]    Script Date: 27/08/2024 13:46:02 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 13/05/2024
-- Description:	Update room details
-- =============================================

/******************************************************************

    MODIFICATIONS

******************************************************************/
CREATE PROCEDURE [dbo].[UpdateRoom] 
	@roomId	INT,
	@roomNumber NVARCHAR(50) = NULL,
	@roomTypeId INT = NULL,
	@updatedBy NVARCHAR(100)
AS
BEGIN

	SET NOCOUNT ON;

	IF EXISTS(SELECT 1 FROM Rooms WHERE RoomId = @roomId)
	BEGIN

		UPDATE Rooms
		SET RoomNumber = ISNULL(@roomNumber, RoomNumber),
            RoomTypeId = CASE WHEN @roomTypeId = 0 THEN RoomTypeId ELSE @roomTypeId END,
			Updated = GETDATE(),
			UpdatedBy = @updatedBy
        WHERE RoomId = @roomId
	END
END
GO

