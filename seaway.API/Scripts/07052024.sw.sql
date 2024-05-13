IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[NewAdmin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE NewAdmin
END
GO
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 07/05/2024
-- Description:	add new admin
-- =============================================
CREATE PROCEDURE NewAdmin 
	@username NVARCHAR(100),
	@password NVARCHAR(100)
AS
BEGIN

	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT 1 FROM Admin WHERE Username = @username)
	BEGIN
		INSERT INTO Admin(Username, Password) VALUES(@username, @password)
	END

END
GO

----------------------------------------------------------------------------------------------------------------------------------------------------------------------
----------------------------------------------------------------------------------------------------------------------------------------------------------------------

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[LoginLogoutTime]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE LoginLogoutTime
END
GO
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 07/05/2024
-- Description:	add lgin logout schedules
-- =============================================
CREATE PROCEDURE LoginLogoutTime 
	@username NVARCHAR(100) = NULL,
	@adminId INT = NULL,
	@loginId INT = NULL
AS
BEGIN

	SET NOCOUNT ON;

	IF (@loginId <> NULL)
	BEGIN
		UPDATE AdminLogin
		SET LogoutTime = GETDATE()
		WHERE LoginId = @loginId
	END

	ELSE IF(@username <> NULL)
	BEGIN 
		INSERT INTO AdminLogin(Username, AdminId, LoginTime, LogoutTime)
		VALUES(@username, @adminId, GETDATE(), '')
	END

END
GO

