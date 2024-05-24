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
