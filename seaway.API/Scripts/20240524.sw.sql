IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllActivities]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllActivities
END
GO
/****** Object:  StoredProcedure [dbo].[GetAllActivities]    Script Date: 24/05/2024 13:01:16 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nitharsha
-- Create date: 29-02-2024
-- Description:	get all activities with all details
-- =============================================

/*
  Modifications

  001           Nitharsha                24/05/2024                change get only activity details without pics

*/

CREATE PROCEDURE [dbo].[GetAllActivities]
AS
BEGIN
    SET NOCOUNT ON;

	SELECT 
	   a.ActivityId,
	   a.Name,
	   a.Description,
	   a.IsActive                                        --001
	FROM Activities a
	ORDER BY a.ActivityId

END
GO

------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-----------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetActivityById]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetActivityById
END
GO


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
			LEFT JOIN PicDocuments PD ON A.ActivityId = PD.PicTypeID AND PD.PicType = 'Activity'
			WHERE (@activityId = A.ActivityId)                                                  
	END
END
GO
