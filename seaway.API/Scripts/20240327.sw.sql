IF EXISTS(SELECT * from INFORMATION_SCHEMA.columns where table_name = 'PicDocuments' and column_name = 'Value')
BEGIN
ALTER TABLE PicDocuments
ALTER COLUMN Value Image NOT NULL
END
GO