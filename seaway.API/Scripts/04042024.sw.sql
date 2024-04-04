IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllRoomsWithPicDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllRoomsWithPicDetails
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 04/04/2024
-- Description:	Get all details of rooms with those pics
-- =============================================

CREATE PROCEDURE GetAllRoomsWithPicDetails
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
	R.DiscountPrice,
	PD.PicType,
	PD.Name AS PicName,
	PD.Value AS PicValue,
	PD.PicTypeId,
	PD.CloudinaryPublicId
    FROM Room R
    LEFT JOIN PicDocuments PD ON R.RoomID = PD.PicTypeID 
	WHERE R.IsActive = 1
END
GO

-------------------------------------------------------------------------------------------------------------------------------------------------------------------
-------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[SelectRoomWithPics]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE SelectRoomWithPics
END
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 04/04/2024
-- Description:	Select particular room with pics with id
-- =============================================
CREATE PROCEDURE SelectRoomWithPics
   @roomId int
AS
BEGIN
   SET NOCOUNT ON;

   select * from Room where RoomId = @roomId

   select * from PicDocuments where PicTypeId = @roomId
END
GO