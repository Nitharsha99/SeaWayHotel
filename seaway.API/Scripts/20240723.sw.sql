IF OBJECT_ID(N'AdminLogin', N'U') IS NOT NULL
BEGIN
    DROP TABLE AdminLogin
END
GO

IF OBJECT_ID(N'Admin', N'U') IS NOT NULL
BEGIN
    DROP TABLE Admin
END
GO

CREATE TABLE Admin(
  AdminId INT IDENTITY  PRIMARY KEY  NOT NULL,
  Username VARCHAR(100) NOT NULL,
  Password VARCHAR(200) NOT NULL,
  IsAdmin BIT NOT NULL,
  ProfilePic IMAGE NULL,
  CreatedBy VARCHAR(100) NOT NULL,
  Created DATETIME NOT NULL,
  UpdatedBy VARCHAR(100) NOT NULL,
  Updated DATETIME NOT NULL
);
GO

CREATE TABLE AdminLogin(
    LoginId INT IDENTITY NOT NULL PRIMARY KEY,
	Username VARCHAR(100) NOT NULL,
	AdminId INT NOT NULL,
	LoginTime DATETIME NOT NULL,
	LogoutTime DATETIME NOT NULL,
	CONSTRAINT FK_Admin FOREIGN KEY (AdminId) REFERENCES Admin(AdminId)
);
GO

IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[NewAdmin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE NewAdmin
END
GO

/****** Object:  StoredProcedure [dbo].[NewAdmin]    Script Date: 23/07/2024 23:27:21 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		Nitharsha
-- Create date: 07/05/2024
-- Description:	add new admin
-- =============================================

/*
	Modifications

	001          23/07/2024            Nitharsha             Add some variables and pic

*/
CREATE PROCEDURE [dbo].[NewAdmin] 
	@username NVARCHAR(100),
	@password NVARCHAR(200),
	@isAdmin BIT,                          --001
	@profilePic IMAGE = NULL,              --001
	@created NVARCHAR(100)                 --001
AS
BEGIN

	SET NOCOUNT ON;

	IF NOT EXISTS(SELECT 1 FROM Admin WHERE Username = @username)
	BEGIN
		INSERT INTO Admin(Username, Password, IsAdmin, ProfilePic, CreatedBy, Created, UpdatedBy, Updated) 
		VALUES(@username, @password, @isAdmin, @profilePic, @created, GETDATE(), @created, GETDATE())           --001
	END

END
GO