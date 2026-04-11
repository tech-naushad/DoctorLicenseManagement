ALTER PROCEDURE sp_GetDoctors
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
        LicenseStatus,
        CreatedDate
    FROM Doctor;
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

    DELETE FROM Doctor
    WHERE Id = @Id;
END
go