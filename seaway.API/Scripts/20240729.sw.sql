IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllRoomCategoryWithPicDetails]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllRoomCategoryWithPicDetails
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllRoomCategoryWithPicDetails]    Script Date: 29/07/2024 20:33:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 11/07/2024
-- Description:	Get all details of room category with those pics
-- =============================================

/*
	Modifications

	001           Nitharsha              29/07/2024                     Change PicType type
*/
CREATE PROCEDURE [dbo].[GetAllRoomCategoryWithPicDetails]
    @categoryId INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Retrieve all room details along with associated picture details
	IF EXISTS(SELECT 1 FROM RoomCategory WHERE CategoryId = @categoryId)
	BEGIN
	       SELECT 
			R.CategoryId, 
			R.RoomName,
			R.CountOfMaxGuest,
			R.Price,
			R.DiscountPercent,
			R.DiscountPrice,
			R.Created,
			R.CreatedBy,
			R.Updated,
			R.UpdatedBy,
			PD.PicType,
			PD.Name AS PicName,
			PD.Value AS PicValue,
			PD.PicTypeId,
			PD.CloudinaryPublicId
			FROM RoomCategory R
			LEFT JOIN PicDocuments PD ON R.CategoryId = PD.PicTypeID AND PD.PicType = 1                 --001
			WHERE @categoryId = r.CategoryId                                                  
	END
END
GO
