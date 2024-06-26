IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[AllAdmin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE AllAdmin
END
GO

/****** Object:  StoredProcedure [dbo].[AllAdmin]    Script Date: 2/16/2024 7:49:02 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nitharsha
-- Create date: 16-02-2024
-- Description:	Get all admins list
-- =============================================

CREATE PROCEDURE AllAdmin

AS	
BEGIN
	SET NOCOUNT ON;
	
		SELECT Username, Password  FROM Admin
END
GO
