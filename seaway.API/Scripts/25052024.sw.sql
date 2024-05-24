IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteActivity]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE DeleteActivity
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kavishan
-- Create date: 24/05/2024
-- Description:	Delete Activity
-- =============================================
CREATE PROCEDURE [dbo].DeleteActivity
	@activityId INT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT 1 FROM Activities WHERE ActivityId = @activityId)
	BEGIN
		DELETE FROM Activities WHERE ActivityId = @activityId
	END
END
GO


