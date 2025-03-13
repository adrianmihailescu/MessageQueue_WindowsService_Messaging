SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[angajati]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[angajati](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[nume] [nvarchar](max) NOT NULL,
	[prenume] [nvarchar](max) NOT NULL,
	[data_angajarii] [datetime] NOT NULL,
 CONSTRAINT [PK_angajati] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (IGNORE_DUP_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[sp_Angajati_Insert]') AND type in (N'P', N'PC'))
BEGIN
EXEC dbo.sp_executesql @statement = N'
CREATE PROCEDURE [dbo].[sp_Angajati_Insert]
	(
	@ERC INT OUT
	, @MESSAGE NVARCHAR(4000) OUT
	, @nume nvarchar(max)
	, @prenume nvarchar(max)
	, @data_angajarii datetime
	)
AS
  
-- Variabile locale
DECLARE @LOCAL_TRAN BIT

-- Initializari
SET @ERC = 0
SET @MESSAGE = ''''

-- Bloc tranzactie
BEGIN TRY
	-- Daca nu exista inca o tranzactie, o deschid acum
	IF @@TRANCOUNT = 0
	BEGIN
		SET @LOCAL_TRAN = 1
		BEGIN TRAN
	END
	
	IF @nume IS NULL OR @nume = ''''
			raiserror(''Completati numele angajatului !'', 11, 1)
			
			IF @prenume IS NULL OR @prenume = ''''
			raiserror(''Completati prenumelenumele angajatului !'', 11, 1)
			
		IF @data_angajarii IS NULL OR @data_angajarii = ''''
			raiserror(''Completati data angajarii !'', 11, 1)

		IF EXISTS(
		SELECT
			nume, prenume
		FROM angajati
		WHERE
		nume = @nume
		AND prenume = @prenume
		)
		-- daca exista deja un angajat cu acelasi nume si acelasi prenume, il updatez
		BEGIN
			update angajati
			SET
				nume = @nume
				, prenume = @prenume,
				data_angajarii = @data_angajarii
			WHERE 
			nume = @nume and prenume = @prenume
		END
		
		ELSE
		BEGIN
			-- ALTFEL IL INSEREZ
			INSERT INTO angajati
			(
				nume
				, prenume
				, data_angajarii
		   )
			
			VALUES
			(
				@nume
				, @prenume
				, @data_angajarii
			  )
		END
			
		IF @@ROWCOUNT = 0
			raiserror(''Eroare de adaugare a angajatului!'', 11, 1)
			
	-- Inchid tranzactia
	IF @LOCAL_TRAN = 1
		COMMIT TRAN
END TRY
BEGIN CATCH
	-- Salvez eroarea in parametrii de iesire
	SET @ERC = CASE ERROR_NUMBER()
		WHEN 50000
		THEN -ERROR_STATE()
		ELSE ERROR_NUMBER()
		END

	SET @MESSAGE = ISNULL(ERROR_PROCEDURE(), ''Unknown'') + '':'' + 
		CAST(ISNULL(ERROR_LINE(), -1) AS NVARCHAR) + '' - '' + 
		LEFT(ISNULL(ERROR_MESSAGE(), ''-''), 3850)

	-- Inchid tranzactia si loghez mesajul de eroare
	IF @LOCAL_TRAN = 1
	BEGIN
		ROLLBACK TRAN
	END
END CATCH

' 
END
