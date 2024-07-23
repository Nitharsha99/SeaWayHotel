IF NOT EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'Admin' and column_name = 'ProfilePic')
BEGIN
ALTER TABLE Admin
ADD ProfilePic IMAGE null
END
GO