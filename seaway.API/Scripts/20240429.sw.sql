IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllRooms]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllRooms
END
GO

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

***************/

CREATE PROCEDURE GetAllRooms
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve all room details along with associated picture details
    SELECT 
	R.RoomId, 
	R.RoomName,
	R.CountOfMaxGuest,
	R.Price,
	R.DiscountPercent,
	R.DiscountPrice
    FROM Room R
END
GO

----------------------------------------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllRoomsWithPicDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllRoomsWithPicDetails
END
GO

/****** Object:  StoredProcedure [dbo].[GetAllRoomsWithPicDetails]    Script Date: 4/29/2024 7:23:00 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 04/04/2024
-- Description:	Get all details of rooms with those pics
-- =============================================

/**************
    modifications

***************/

CREATE PROCEDURE [dbo].[GetAllRoomsWithPicDetails]
    @roomId INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve all room details along with associated picture details
	IF EXISTS(SELECT 1 FROM Room WHERE RoomId = @roomId)
	BEGIN
	       SELECT 
			R.RoomId, 
			R.RoomName,
			R.CountOfMaxGuest,
			R.Price,
			R.DiscountPercent,
			R.DiscountPrice,
			PD.PicType,
			PD.Name AS PicName,
			PD.Value AS PicValue,
			PD.PicTypeId,
			PD.CloudinaryPublicId
			FROM Room R
			LEFT JOIN PicDocuments PD ON R.RoomID = PD.PicTypeID AND PD.PicType = 'Room'
			WHERE (R.IsActive = 1)
			AND (@roomId IS NULL OR @roomId = r.RoomId)
	END
END
GO
