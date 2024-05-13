IF EXISTS (SELECT 1 FROM sysobjects WHERE id = object_id(N'[dbo].[AllAdmin]') AND OBJECTPROPERTY(id, N'IsProcedure') = 1)
BEGIN
	DROP PROCEDURE AllAdmin
END
GO
/****** Object:  StoredProcedure [dbo].[AllAdmin]    Script Date: 5/6/2024 5:48:30 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		Nitharsha
-- Create date: 15-02-2024
-- Description:	Get all admins list
-- =============================================


/*******************************

Modifications

001      Nitharsha         06/05/2024              get all details

********************************/
CREATE PROCEDURE [dbo].[AllAdmin]

AS	
BEGIN
	SET NOCOUNT ON;
	
		SELECT * FROM Admin
END
GO
