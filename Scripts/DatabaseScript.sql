USE [master];
GO

IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'FruitAppDB') --Check if DB already exists
BEGIN
	CREATE DATABASE [FruitAppDB] --Create database
	ON PRIMARY
	( NAME = FruitAppDB_dat,
		FILENAME = 'C:\MSSQL\Data\fruitappdbdat.mdf',
		SIZE = 10MB,
		MAXSIZE = UNLIMITED,
		FILEGROWTH = 64MB )
	LOG ON
	( NAME = FruitAppDB_log,
		FILENAME = 'C:\MSSQL\Data\fruitappdb_log.ldf',
		SIZE = 10MB,
		MAXSIZE = UNLIMITED,
		FILEGROWTH = 64MB ) ;
END
GO

USE [FruitAppDB]

SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

BEGIN TRY --Error Handling

  BEGIN TRANSACTION CREATEPRODUCTS -- Transaction to Rollback
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Products]') AND type in (N'U')) --Check if Products table already exists
	BEGIN
		CREATE TABLE [dbo].[Products]( --Create Products Table
			[ProductId] INT IDENTITY(1,1) NOT NULL,
			[ProductCode] VARCHAR(100) NOT NULL,
			[Name] VARCHAR(100) NOT NULL,
			[Description] VARCHAR(200) NULL,
			[CategoryName] VARCHAR(100) NOT NULL,
			[Price] DECIMAL(19, 4) NOT NULL,
			[CategoryId] INT NOT NULL,
			[CreatedDate] DATETIME2(7) NOT NULL,
			[CreatedBy] VARCHAR(150) NULL,
			[ImageUrl] VARBINARY(MAX) NULL,

		CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
		(
			[ProductId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] --Set Primary Key
		) ON [PRIMARY]
END
 COMMIT TRANSACTION CREATEPRODUCTS
    PRINT 'Products table created successfully.' 
END TRY

BEGIN CATCH  --In the Event of Errors
  IF (@@TRANCOUNT > 0)
   BEGIN
      ROLLBACK TRANSACTION CREATEPRODUCTS
      PRINT 'Error detected, all changes reversed'
   END 
    SELECT
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() AS ErrorState,
        ERROR_PROCEDURE() AS ErrorProcedure,
        ERROR_LINE() AS ErrorLine,
        ERROR_MESSAGE() AS ErrorMessage
END CATCH

BEGIN TRY
  BEGIN TRANSACTION CREATECATEGORIES --Transaction to Rollback
	IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Categories]') AND type in (N'U')) --Check if Categories Table Exists
	BEGIN
		CREATE TABLE [dbo].[Categories](
			[CategoryId] INT IDENTITY(1,1) NOT NULL,
			[Name] VARCHAR(100) NULL,
			[CategoryCode] VARCHAR(50) NULL,
			[IsActive] BIT NOT NULL,

		CONSTRAINT [PK_Categories] PRIMARY KEY CLUSTERED 
		(
			[CategoryId] ASC
		)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY] --Add Primary Key
		) ON [PRIMARY]

		IF NOT EXISTS(SELECT 1
		FROM sys.columns
		WHERE Name = N'CategoryId'
			AND Object_ID = Object_ID(N'dbo.Categories'))
		BEGIN
			ALTER TABLE [dbo].[Categories]  WITH CHECK ADD CONSTRAINT [Categories_Product_CategoryId_fk] FOREIGN KEY([CategoryId]) --Set up Relation Between Products and Categories
			REFERENCES [dbo].[Products] ([CategoryId])
		END
END
 COMMIT TRANSACTION CREATECATEGORIES --Commit Transaction
    PRINT 'Categories table created successfully.' 
END TRY

BEGIN CATCH --If Errors
  IF (@@TRANCOUNT > 0)
   BEGIN
      ROLLBACK TRANSACTION CREATECATEGORIES
      PRINT 'Error detected, all changes reversed'
   END 
    SELECT
        ERROR_NUMBER() AS ErrorNumber,
        ERROR_SEVERITY() AS ErrorSeverity,
        ERROR_STATE() AS ErrorState,
        ERROR_PROCEDURE() AS ErrorProcedure,
        ERROR_LINE() AS ErrorLine,
        ERROR_MESSAGE() AS ErrorMessage
END CATCH