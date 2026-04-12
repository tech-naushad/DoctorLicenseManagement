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
------------------------------
ALTER PROCEDURE sp_CreateDoctor
    @FullName NVARCHAR(150),
    @Email NVARCHAR(150),
    @Specialization NVARCHAR(100),
    @LicenseNumber NVARCHAR(100),
    @LicenseExpiryDate DATE,
    @LicenseStatus INT
AS
BEGIN
    SET NOCOUNT ON;

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
    @LicenseStatus INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Doctor
    SET 
        FullName = @FullName,
        Email = @Email,
        Specialization = @Specialization,
        LicenseNumber = @LicenseNumber,
        LicenseExpiryDate = @LicenseExpiryDate,
        LicenseStatus = @LicenseStatus
    WHERE Id = @Id;
END
go

----------
ALTER PROCEDURE sp_DeleteDoctor
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        BEGIN TRAN;

        IF NOT EXISTS (SELECT 1 FROM Doctor WHERE Id = @Id)
        BEGIN
            ROLLBACK TRAN;

            SELECT 
                CAST(0 AS BIT) AS Success,
                'Doctor not found' AS Message;

            RETURN;
        END

        DELETE FROM Doctor
        WHERE Id = @Id;

        COMMIT TRAN;

        SELECT 
            CAST(1 AS BIT) AS Success,
            'Doctor deleted successfully' AS Message;

    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRAN;

        SELECT 
            CAST(0 AS BIT) AS Success,
            ERROR_MESSAGE() AS Message;
    END CATCH
END
GO
 