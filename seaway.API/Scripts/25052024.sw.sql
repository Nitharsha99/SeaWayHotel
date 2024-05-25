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
		IF EXISTS(SELECT 1 FROM PicDocuments WHERE PicTypeId = @activityId AND PicType = 'Activity')
			BEGIN
				DELETE FROM PicDocuments WHERE PicTypeId = @activityId AND PicType = 'Activity'
			END
	END
END
GO






USE [seaway]
GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateActivity]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UpdateActivity
END
GO


SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kavishan
-- Create date: 25/05/2024
-- Description:	Update Activity details
-- =============================================


CREATE PROCEDURE [dbo].[UpdateActivity] 
	@ActivityId	INT,
	@Name NVARCHAR(100) = NULL,
	@Description NVARCHAR(1000) = NULL,
	@IsActive BIT = NULL
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT 1 FROM Activities WHERE ActivityId = @ActivityId)
	BEGIN
		UPDATE Activities
		SET Name = ISNULL(@Name, Name),
            Description = ISNULL(@Description, Description),
            IsActive = ISNULL(@IsActive, IsActive)
        WHERE ActivityId = @ActivityId
	END
END
GO








