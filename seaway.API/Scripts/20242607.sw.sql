IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetActivityById]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetActivityById
END
GO

/****** Object:  StoredProcedure [dbo].[GetActivityById]    Script Date: 26/07/2024 11:34:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 24/05/2024
-- Description:	Get all details of activity with those pics by id
-- =============================================

/**************
    modifications

	0001         26/07/2024           Nitharsha           Change PicType in enum 

***************/

CREATE PROCEDURE [dbo].[GetActivityById]
    @activityId INT
AS
BEGIN
    SET NOCOUNT ON;

	IF EXISTS(SELECT 1 FROM Activities WHERE ActivityId = @activityId)
	BEGIN
	       SELECT 
		    A.ActivityId,
			A.Name AS ActivityName,
			A.Description,
			A.IsActive,
			PD.PicType,
			PD.Name AS PicName,
			PD.Value AS PicValue,
			PD.PicTypeId,
			PD.CloudinaryPublicId
			FROM Activities A
			LEFT JOIN PicDocuments PD ON A.ActivityId = PD.PicTypeID AND PD.PicType = 2                          --0001
			WHERE (@activityId = A.ActivityId)                                                  
	END
END
GO
