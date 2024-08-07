IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteActivity]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE DeleteActivity
END
GO

/****** Object:  StoredProcedure [dbo].[DeleteActivity]    Script Date: 01/08/2024 20:25:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Kavishan
-- Create date: 24/05/2024
-- Description:	Delete Activity
-- =============================================

/*
	Modificattions

	001          Nitharsha           01/08/2024                change Activity based on picType
*/
CREATE PROCEDURE [dbo].[DeleteActivity]
	@activityId INT
AS
BEGIN
	SET NOCOUNT ON;
	IF EXISTS(SELECT 1 FROM Activities WHERE ActivityId = @activityId)
	BEGIN
		DELETE FROM Activities WHERE ActivityId = @activityId
		IF EXISTS(SELECT 1 FROM PicDocuments WHERE PicTypeId = @activityId AND PicType = 2)          --001
			BEGIN
				DELETE FROM PicDocuments WHERE PicTypeId = @activityId AND PicType = 2             --001
			END
	END
END
GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteRoomCategoryWithPics]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE DeleteRoomCategoryWithPics
END
GO
/****** Object:  StoredProcedure [dbo].[DeleteRoomCategoryWithPics]    Script Date: 01/08/2024 20:27:57 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 11/07/2024
-- Description:	Delete RoomCategory and related Pics of Category
-- =============================================

/*
	Modifications

	001       Nitharsha          01/08/2024               change picType based on PicType enum
*/
CREATE PROCEDURE [dbo].[DeleteRoomCategoryWithPics]
	@categoryId INT
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @isCategoryRemove BIT = 0;

	IF EXISTS(SELECT 1 FROM RoomCategory WHERE CategoryId = @categoryId)
	BEGIN
		DELETE FROM RoomCategory WHERE CategoryId = @categoryId

		SET @isCategoryRemove = 1

		IF(@isCategoryRemove = 1)
		BEGIN
			IF EXISTS(SELECT 1 FROM PicDocuments WHERE PicTypeId = @categoryId AND PicType = 1)          --001
			BEGIN
				DELETE FROM PicDocuments WHERE PicTypeId = @categoryId AND PicType = 1           --001
			END
		END
	END
END
GO

