USE [Seaway_db]
GO
SET ANSI_NULLS ON
GO
- =============================================
-- Author:        Sadakshini
-- Create date:   14/03/2025
-- Description:   Create new package
-- =============================================

SET QUOTED_IDENTIFIER ON
GO

CREATE OR ALTER   procedure [dbo].[CreatePackage]
	@PackageName varchar(200),
    @Description varchar(1000),
    @PackageDurationType int,
    @Price decimal(18, 2),
    @UserType int,
    @IsActive bit,
    @CreatedBy varchar(100),
    @Created datetime,
	@UpdatedBy varchar(100),
    @Updated datetime,
	@PackageId INT OUTPUT

As
Begin
	Set NoCOunt on;

	insert into packages (PackageName,Description,PackageDurationType,Price,UserType,IsActive,CreatedBy,Created,UpdatedBy,Updated)
	values (@PackageName,@Description,@PackageDurationType,@Price,@UserType,@IsActive,@CreatedBy,GETDATE(),@UpdatedBy,GETDATE());

	SET @PackageId=SCOPE_IDENTITY();

	RETURN 1;
End
GO

