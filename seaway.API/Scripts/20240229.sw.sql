IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[GetAllActivities]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE GetAllActivities
END
GO
/****** Object:  StoredProcedure [dbo].[InsertActivityWithPic]    Script Date: 2/29/2024 12:43:14 PM ******/
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

*/

CREATE PROCEDURE GetAllActivities
AS
BEGIN
    SET NOCOUNT ON;

	SELECT 
	   a.ActivityId,
	   a.Name,
	   a.Description,
	   a.IsActive,
	   p.Name AS PicName,
	   p.PicType,
	   p.Value AS PicValue
	FROM Activities a
	LEFT JOIN PicDocuments p  ON a.ActivityId = p.PicTypeId
	ORDER BY a.ActivityId

END
GO