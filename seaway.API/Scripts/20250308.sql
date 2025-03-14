USE [Seaway_db]
GO

/****** Object:  StoredProcedure [dbo].[DeletePackage]    Script Date: 3/8/2025 12:24:03 PM ******/
SET ANSI_NULLS ON
GO

-- =============================================
-- Author:        Sadakshini
-- Create date:   08/03/2025
-- Description:   Deletes a package by its ID
-- =============================================

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   procedure [dbo].[DeletePackage]
	@packageId INT
AS
Begin
	SET NOCOUNT ON;

	If Exists (SELECt 1 from packages where PackageId = @packageId)
	Begin
		Delete From packages where PackageId=@packageId;
		Return 1;
	End
	Else
	Begin 
		return 0;
	End
End
GO

