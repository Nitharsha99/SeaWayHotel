IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[InsertPackage]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE InsertPackage
END
GO
-- ================================================
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:	Nitharsha
-- Create date: 15/03/2025
-- Description:	Create a new package
-- =============================================
CREATE PROCEDURE InsertPackage
	@name NVARCHAR(100),
	@description NVARCHAR(500),
	@durationType INT,
	@price DECIMAL(18, 2),
	@userType INT,
	@createdBy NVARCHAR(100)
AS
BEGIN
	SET NOCOUNT ON;

	DECLARE @newPackageId INT;

	IF NOT EXISTS(SELECT 1 FROM Packages WHERE PackageName = @name and IsActive = 1)
	BEGIN
		    INSERT INTO Packages(PackageName, Description, IsActive, PackageDurationType, Price, UserType, Created, CreatedBy, Updated, UpdatedBy)
			VALUES (@name, @description, 1, @durationType, @price, @userType, GETDATE(), @createdBy, GETDATE(), @createdBy);          

			SET @newPackageId = SCOPE_IDENTITY(); -- Get the ID of the newly inserted activity

			-- Return the IDs of the inserted records
			SELECT @newPackageId AS packageId;
	END
END
GO
