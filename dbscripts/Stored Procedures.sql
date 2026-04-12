ALTER PROCEDURE [dbo].[sp_GetDoctors]
(
	@Search   NVARCHAR(100) = NULL,
    @Status   INT           = NULL,
    @Page     INT           = 1,
    @PageSize INT           = 10
)
AS
BEGIN
    SET NOCOUNT ON;
	
    DECLARE @Offset INT = (@Page - 1) * @PageSize;

    -- Result set 1: paged doctors
    SELECT
        Id, FullName, Email, Specialization,
        LicenseNumber, LicenseExpiryDate, LicenseStatus
    FROM Doctor
    WHERE
        (@Search IS NULL OR FullName LIKE '%' + @Search + '%'
                         OR LicenseNumber LIKE '%' + @Search + '%')
        AND
        (@Status IS NULL OR LicenseStatus = @Status)
    ORDER BY FullName
    OFFSET @Offset ROWS
    FETCH NEXT @PageSize ROWS ONLY;

    -- Result set 2: total count
    SELECT COUNT(*)
    FROM Doctor
    WHERE
        (@Search IS NULL OR FullName LIKE '%' + @Search + '%'
                         OR LicenseNumber LIKE '%' + @Search + '%')
        AND
        (@Status IS NULL OR LicenseStatus = @Status);
END
go

CREATE PROCEDURE [dbo].[sp_GetDoctorById]
(
	@Id  INT    
)
AS
BEGIN 
    SELECT
        Id, FullName, Email, Specialization,
        LicenseNumber, LicenseExpiryDate, LicenseStatus
    FROM Doctor
    WHERE
        Id = @Id
    ORDER BY FullName
END
go
------------------------------
ALTER PROCEDURE sp_CreateDoctor
    @FullName NVARCHAR(150),
    @Email NVARCHAR(150),
    @Specialization NVARCHAR(100),
    @LicenseNumber NVARCHAR(100),
    @LicenseExpiryDate DATE,
    @LicenseStatus INT,
	@Message NVARCHAR(200) OUTPUT
AS
BEGIN
     -- ✅ Check duplicate Email
    IF EXISTS (SELECT 1 FROM Doctor WHERE Email = @Email)
    BEGIN
			set @Message = 'Email already exists';
        RETURN;
    END

    -- ✅ Check duplicate LicenseNumber
    IF EXISTS (SELECT 1 FROM Doctor WHERE LicenseNumber = @LicenseNumber)
    BEGIN
		set @Message = 'License number already exists';
        RETURN;
    END

    INSERT INTO Doctor
    (
        FullName,
        Email,
        Specialization,
        LicenseNumber,
        LicenseExpiryDate,
        LicenseStatus,
        CreatedDate
    )
    VALUES
    (
        @FullName,
        @Email,
        @Specialization,
        @LicenseNumber,
        @LicenseExpiryDate,
        @LicenseStatus,
        GETDATE()
    );
	set @Message = 'Doctor created successfully'
    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
go

---------------------
ALTER PROCEDURE sp_UpdateDoctor
    @Id INT,
    @FullName NVARCHAR(150),
    @Email NVARCHAR(150),
    @Specialization NVARCHAR(100),
    @LicenseNumber NVARCHAR(100),
    @LicenseExpiryDate DATE,
    @LicenseStatus INT,
	@Message NVARCHAR(200) OUTPUT
AS
BEGIN
   -- ✅ Check duplicate Email
    IF EXISTS (SELECT 1 FROM Doctor WHERE Email = @Email)
    BEGIN
			set @Message = 'Email already exists';
        RETURN;
    END

    -- ✅ Check duplicate LicenseNumber
    IF EXISTS (SELECT 1 FROM Doctor WHERE LicenseNumber = @LicenseNumber)
    BEGIN
		set @Message = 'License number already exists';
        RETURN;
    END

    UPDATE Doctor
    SET 
        FullName = @FullName,
        Email = @Email,
        Specialization = @Specialization,
        LicenseNumber = @LicenseNumber,
        LicenseExpiryDate = @LicenseExpiryDate,
        LicenseStatus = @LicenseStatus,
		UpdatedDate = getdate()
    WHERE Id = @Id;

	-- ✅ Check if any row was updated
    IF @@ROWCOUNT = 0
    BEGIN
        --SELECT 
           -- CAST(0 AS BIT) AS Success,
           set @Message= 'Doctor not found';
    END
    ELSE
    BEGIN
      set @Message= 'Doctor updated successfully';
    END
END
go

----------
ALTER PROCEDURE sp_DeleteDoctor
    @Id INT,
    @Message NVARCHAR(200) OUTPUT
AS
BEGIN
    

    BEGIN TRY
        BEGIN TRAN;

        IF NOT EXISTS (SELECT 1 FROM Doctor WHERE Id = @Id)
        BEGIN
            ROLLBACK TRAN;
            SET @Message = 'Doctor not found';
            RETURN;
        END

        DELETE FROM Doctor
        WHERE Id = @Id;

        COMMIT TRAN;

        SET @Message = 'Doctor deleted successfully';

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        SET @Message = ERROR_MESSAGE();
    END CATCH
END
GO
 