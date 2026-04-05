CREATE PROCEDURE sp_GetDoctors
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        Id,
        FullName,
        Email,
        Specialization,
        LicenseNumber,
        LicenseExpiryDate,
        Status,
        CreatedDate
    FROM Doctor;
END
go
------------------------------
CREATE PROCEDURE sp_CreateDoctor
    @FullName NVARCHAR(150),
    @Email NVARCHAR(150),
    @Specialization NVARCHAR(100),
    @LicenseNumber NVARCHAR(100),
    @LicenseExpiryDate DATE,
    @Status INT
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
        Status,
        CreatedDate
    )
    VALUES
    (
        @FullName,
        @Email,
        @Specialization,
        @LicenseNumber,
        @LicenseExpiryDate,
        @Status,
        GETDATE()
    );

    SELECT CAST(SCOPE_IDENTITY() AS INT);
END
go

---------------------
CREATE PROCEDURE sp_UpdateDoctor
    @Id INT,
    @FullName NVARCHAR(150),
    @Email NVARCHAR(150),
    @Specialization NVARCHAR(100),
    @LicenseNumber NVARCHAR(100),
    @LicenseExpiryDate DATE,
    @Status INT
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
        Status = @Status
    WHERE Id = @Id;
END
go

----------
CREATE PROCEDURE sp_DeleteDoctor
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Doctor
    WHERE Id = @Id;
END
go