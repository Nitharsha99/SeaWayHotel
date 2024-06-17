

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllRoomsWithPicDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllRoomsWithPicDetails
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllRoomsWithPicDetails]    Script Date: 4/18/2024 7:34:27 PM ******/
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

	001      Nitharsha           19/04/2024                     add roomId to find one record
***************/

CREATE PROCEDURE [dbo].[GetAllRoomsWithPicDetails]
    @roomId INT = NULL                        --001
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
	WHERE (R.IsActive = 1) AND (PD.PicType = 'Room')
	AND (@roomId IS NULL OR @roomId = r.RoomId)                                --001
END
GO