IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateAdmin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE UpdateAdmin
END
GO
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 06/08/2024
-- Description:	Update admin and manager details
-- =============================================
CREATE PROCEDURE UpdateAdmin
	@adminId INT,
	@username NVARCHAR(100) = NULL,
	@isAdmin BIT = NULL,
	@profilePic IMAGE = NULL,
	@updatedBy NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	IF EXISTS(SELECT 1 FROM Admin WHERE AdminId = @adminId)
	BEGIN
		UPDATE Admin
		SET Username = ISNULL(@username, Username),
			IsAdmin = ISNULL(@isAdmin, IsAdmin),
			ProfilePic = ISNULL(@profilePic, ProfilePic),
			Updated = GETDATE(),
			UpdatedBy = @updatedBy
		WHERE AdminId = @adminId
	END
END
GO
