IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Admin' and column_name = 'UserRole')
BEGIN
ALTER TABLE Admin
DROP COLUMN UserRole
END
GO

IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Admin' and column_name = 'IsAdmin')
BEGIN
ALTER TABLE Admin
ADD IsAdmin bit not null
END
GO
