IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[LoginLogoutTime]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE LoginLogoutTime
END
GO
/****** Object:  StoredProcedure [dbo].[LoginLogoutTime]    Script Date: 28/07/2024 00:06:01 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 07/05/2024
-- Description:	add lgin logout schedules
-- =============================================

/*
	Modifications

	001       Nitharsha          28/07/2024                    change null code in correct manner
*/
CREATE PROCEDURE [dbo].[LoginLogoutTime] 
	@username NVARCHAR(100) = NULL,
	@adminId INT = NULL,
	@loginId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	IF (@loginId IS NOT NULL)                           --001
	BEGIN
		UPDATE AdminLogin
		SET LogoutTime = GETDATE()
		WHERE LoginId = @loginId
	END

	ELSE IF(@username IS NOT NULL)                        --001
	BEGIN 
		INSERT INTO AdminLogin(Username, AdminId, LoginTime, LogoutTime)
		VALUES(@username, @adminId, GETDATE(), '')
	END

END
GO
