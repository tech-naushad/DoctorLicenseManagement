--b drop table Doctor

CREATE TABLE Doctor (
    Id INT IDENTITY(1,1) PRIMARY KEY,	 
    FullName NVARCHAR(150) NOT NULL,
    Email NVARCHAR(150) UNIQUE NOT NULL,
    Specialization NVARCHAR(100),
    LicenseNumber NVARCHAR(100) UNIQUE NOT NULL,
    LicenseExpiryDate DATE NOT NULL,
    LicenseStatus INT NOT NULL, -- ENUM stored as INT
    CreatedDate DATETIME NOT NULL DEFAULT GETDATE(),
	UpdatedDate DATETIME NULL
);

ALTER TABLE Doctor
ADD CONSTRAINT CK_Doctor_LicenseStatus
CHECK (LicenseStatus IN (1, 2, 3));

 