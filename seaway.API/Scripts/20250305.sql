USE [Seaway_db]
GO

/****** Object:  StoredProcedure [dbo].[GetPackageById]    Script Date: 3/5/2025 8:37:37 PM ******/
SET ANSI_NULLS ON
GO

-- =============================================
-- Author:		Sadakshini
-- Create date: 05/03/2025
-- Description:	Get all details of packages by its id
-- =============================================

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   procedure [dbo].[GetPackageById]
	@packageId INT
As
Begin
	SET Nocount on;
	if exists(select 1 from packages where PackageId=@packageId)
	Begin
		Select
		PackageId,
		PackageName,
		Description,
		PackageDurationType,
		Price,
		UserType,
		IsActive,
		CreatedBy,
		Created,
		UpdatedBy,
		Updated

		from Packages
		where (@packageId=PackageId)
	End
End
GO

