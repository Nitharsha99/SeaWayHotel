IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllRoomsWithPicDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllRoomsWithPicDetails
END
GO

/****** Object:  StoredProcedure [dbo].[GetAllRoomsWithPicDetails]    Script Date: 19/05/2024 19:35:41 ******/
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

001            Nitharsha           19/05/2024                 Change Sp based on isActive

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
			WHERE (@roomId IS NULL OR @roomId = r.RoomId)                                                  --001
	END
END
GO
