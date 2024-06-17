IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[DeleteRoomWithPics]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE DeleteRoomWithPics
END
GO

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 22/05/2024
-- Description:	Delete Room and related Pics of Rooms
-- =============================================
CREATE PROCEDURE DeleteRoomWithPics
	@roomId INT
AS
BEGIN

	SET NOCOUNT ON;
	DECLARE @isRoomRemove BIT = 0;

	IF EXISTS(SELECT 1 FROM Room WHERE RoomId = @roomId)
	BEGIN
		DELETE FROM Room WHERE RoomId = @roomId

		SET @isRoomRemove = 1

		IF(@isRoomRemove = 1)
		BEGIN
			IF EXISTS(SELECT 1 FROM PicDocuments WHERE PicTypeId = @roomId AND PicType = 'Room')
			BEGIN
				DELETE FROM PicDocuments WHERE PicTypeId = @roomId AND PicType = 'Room'
			END
		END
	END
END
GO
