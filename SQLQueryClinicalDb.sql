-- Database
CREATE DATABASE clinicalDb2025;
USE clinicalDb2025;

-- 1. Role Table
CREATE TABLE RoleTbl (
    RoleId INT IDENTITY(1,1) PRIMARY KEY,
    RoleName NVARCHAR(50) NOT NULL UNIQUE
);

-- 2. User Table
CREATE TABLE UserTbl (
    UserId INT IDENTITY(1,1) PRIMARY KEY,
    FullName NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    BloodGroup NVARCHAR(50),
    Gender CHAR(1) CHECK (Gender IN ('M','F','O')),
    UserName NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(10) NOT NULL,
    JoiningDate DATE NOT NULL DEFAULT GETDATE(),
    RoleId INT NOT NULL FOREIGN KEY REFERENCES RoleTbl(RoleId),
    MobileNumber VARCHAR(15),
    IsActive BIT DEFAULT 1
);

-- 3. Department Table
CREATE TABLE DepartmentTbl (
    DepartmentId INT IDENTITY(1,1) PRIMARY KEY,
    DepartmentName NVARCHAR(100) NOT NULL UNIQUE
);

-- 4. Medicine Master Table
CREATE TABLE MedicineTbl (
    MedicineId INT IDENTITY(1,1) PRIMARY KEY,
    MedicineName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(200),
    UnitPrice DECIMAL(10,2) NOT NULL,
    Stock INT DEFAULT 0,
    Manufacturer NVARCHAR(100)
);

-- 5. Test Master Table
CREATE TABLE TestTbl (
    TestId INT IDENTITY(1,1) PRIMARY KEY,
    TestName NVARCHAR(100) NOT NULL UNIQUE,
    Description NVARCHAR(200),
    TestFee DECIMAL(10,2) NOT NULL
);

-- 6. Patient Table
CREATE TABLE PatientTbl (
    PatientId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    DateOfBirth DATE NOT NULL,
    Gender CHAR(1) CHECK (Gender IN ('M','F','O')),
    Phone VARCHAR(15),
    Address NVARCHAR(200),
    MMRNumber NVARCHAR(20) UNIQUE NOT NULL
);

-- 7. Membership Master Table
CREATE TABLE MembershipTbl (
    MembershipId INT IDENTITY(1,1) PRIMARY KEY,
    MembershipName NVARCHAR(100) NOT NULL,
    DiscountPercent DECIMAL(5,2) NOT NULL,
    DurationMonths INT NOT NULL,
    Fee DECIMAL(10,2) NOT NULL
);

-- 8. Patient Membership Table
CREATE TABLE PatientMembershipTbl (
    PatientMembershipId INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL FOREIGN KEY REFERENCES PatientTbl(PatientId),
    MembershipId INT NOT NULL FOREIGN KEY REFERENCES MembershipTbl(MembershipId),
    StartDate DATE NOT NULL DEFAULT GETDATE(),
    EndDate DATE,
    IsActive BIT DEFAULT 1
);

-- 9. Doctor Table
CREATE TABLE DoctorTbl (
    DoctorId INT IDENTITY(1,1) PRIMARY KEY,
    Specialization NVARCHAR(100),
    UserId INT NOT NULL FOREIGN KEY REFERENCES UserTbl(UserId),
    DepartmentId INT NULL FOREIGN KEY REFERENCES DepartmentTbl(DepartmentId),
    IsAvailable BIT DEFAULT 1,
    DoctorFee DECIMAL(10,2) DEFAULT 0
);
SET IDENTITY_INSERT DoctorTbl ON;

-- 10. Appointment Table
CREATE TABLE AppointmentTbl (
    AppointmentId INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL FOREIGN KEY REFERENCES PatientTbl(PatientId),
    DoctorId INT NOT NULL FOREIGN KEY REFERENCES DoctorTbl(DoctorId),
    AppointmentDate DATETIME NOT NULL,
    Status NVARCHAR(50)
);
ALTER TABLE AppointmentTbl
ADD TokenNo INT;

-- 11. Consultation Table
CREATE TABLE ConsultationTbl (
    ConsultationId INT IDENTITY(1,1) PRIMARY KEY,
    AppointmentId INT NOT NULL FOREIGN KEY REFERENCES AppointmentTbl(AppointmentId),
    Diagnosis NVARCHAR(200),
    CreatedOn DATETIME DEFAULT GETDATE()
);

-- 12. Lab Prescription (Master)
CREATE TABLE LabPrescriptionTbl (
    LabPrescriptionId INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL FOREIGN KEY REFERENCES PatientTbl(PatientId),
    ConsultationId INT NOT NULL FOREIGN KEY REFERENCES ConsultationTbl(ConsultationId),
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- 13. Lab Prescription Details
CREATE TABLE LabPrescriptionDetailTbl (
    LabDetailId INT IDENTITY(1,1) PRIMARY KEY,
    LabPrescriptionId INT NOT NULL FOREIGN KEY REFERENCES LabPrescriptionTbl(LabPrescriptionId),
    TestName NVARCHAR(100),
    TestFee DECIMAL(10,2),
    TestId INT NULL FOREIGN KEY REFERENCES TestTbl(TestId)
);

-- 14. Lab Report Table
CREATE TABLE LabReportTbl (
    ReportId INT IDENTITY(1,1) PRIMARY KEY,
    LabDetailId INT NOT NULL FOREIGN KEY REFERENCES LabPrescriptionDetailTbl(LabDetailId),
    Result NVARCHAR(200),
    ReportDate DATE DEFAULT GETDATE()
);

-- 15. Pharmacy Prescription
CREATE TABLE PharmacyPrescriptionTbl (
    PharmacyPresId INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL FOREIGN KEY REFERENCES PatientTbl(PatientId),
    ConsultationId INT NOT NULL FOREIGN KEY REFERENCES ConsultationTbl(ConsultationId),
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- 16. Pharmacy Prescription Details
CREATE TABLE PharmacyPrescriptionDetailTbl (
    PharmacyDetailId INT IDENTITY(1,1) PRIMARY KEY,
    PharmacyPresId INT NOT NULL FOREIGN KEY REFERENCES PharmacyPrescriptionTbl(PharmacyPresId),
    MedicineName NVARCHAR(100),
    Dosage NVARCHAR(50),
    Duration NVARCHAR(50),
    Price DECIMAL(10,2),
    MedicineId INT NULL FOREIGN KEY REFERENCES MedicineTbl(MedicineId)
);

-- 17. Bill Table
CREATE TABLE BillTbl (
    BillId INT IDENTITY(1,1) PRIMARY KEY,
    PatientId INT NOT NULL FOREIGN KEY REFERENCES PatientTbl(PatientId),
    ConsultationId INT NOT NULL FOREIGN KEY REFERENCES ConsultationTbl(ConsultationId),
    BillDate DATETIME DEFAULT GETDATE(),
    ConsultationFee DECIMAL(10,2),
    LabFee DECIMAL(10,2),
    MedicineFee DECIMAL(10,2),
    TotalAmount AS (ISNULL(ConsultationFee,0) + ISNULL(LabFee,0) + ISNULL(MedicineFee,0))
);

-- 18. Payment Details Table
CREATE TABLE PaymentDetailTbl (
    PaymentId INT IDENTITY(1,1) PRIMARY KEY,
    BillId INT NOT NULL FOREIGN KEY REFERENCES BillTbl(BillId),
    PaymentDate DATETIME DEFAULT GETDATE(),
    PaymentMode NVARCHAR(50) CHECK (PaymentMode IN ('Cash','Card','UPI','Insurance')),
    AmountPaid DECIMAL(10,2) NOT NULL,
    TransactionRef NVARCHAR(100),
    IsSuccessful BIT DEFAULT 1
);

-- Insert into role table
INSERT INTO RoleTbl (RoleName) VALUES
('Receptionist'),
('Doctor');

-- Insert into Department table
INSERT INTO DepartmentTbl (DepartmentName) VALUES
('Cardiology'),
('Neurology'),
('Orthopedics'),
('General Medicine');

-- Insert into User table
INSERT INTO UserTbl (FullName, DateOfBirth, BloodGroup, Gender, UserName, Password, JoiningDate, RoleId, MobileNumber, IsActive)
VALUES
('Anu', '1995-03-15', 'A+', 'F', 'Reception', 'Recept@123', '2025-01-10', 1, '9876543211', 1),
('Dr.John', '1985-06-20', 'B+', 'M', 'Drjohn', 'John@123', '2025-01-05', 2, '9876543213', 1),
('Dr.Mary', '1987-08-25', 'AB+', 'F', 'Drmary', 'Mary@123', '2025-01-07', 2, '9876543214', 1),
('Dr.Alice', '1985-09-15','B+', 'F', 'Dralice', 'Alice@123','2025-04-06', 2, '9876501234',1),
('Dr.Arun', '1990-08-15','B+', 'M', 'Drarun', 'Arun@123','2024-04-06', 2, '9876508034',1);

-- Insert into Doctor table
INSERT INTO DoctorTbl (DoctorId,Specialization, UserId, DepartmentId, IsAvailable, DoctorFee)
VALUES
(1,'Orthopedist', 7, 1, 3, 1300.00),
(2,'General Physician', 9, 4, 6, 1000.00),
(3,'Cardiologist', 6, 1, 1, 1000.00),
(4,'Neurologist', 5, 2, 1, 1200.00);

-- Insert some membership plans
INSERT INTO MembershipTbl (MembershipName, DiscountPercent, DurationMonths, Fee)
VALUES 
('Silver Plan', 5.00, 6, 1000.00),
('Gold Plan', 10.00, 12, 1800.00),
('Platinum Plan', 15.00, 24, 3000.00);


INSERT INTO MedicineTbl (MedicineName, Description, UnitPrice, Stock, Manufacturer)
VALUES ('Paracetamol', 'Pain reliever', 5.00, 100, 'Cipla');

INSERT INTO TestTbl (TestName, Description, TestFee)
VALUES ('Blood Test', 'Basic blood test', 150.00);

INSERT INTO MedicineTbl (MedicineName, Description, UnitPrice, Stock, Manufacturer)
VALUES
('ORS Pack', 'Oral rehydration solution', 4.00, 600, 'GlaxoSmithKline'),
('Vitamin D3', 'Vitamin supplement', 18.00, 200, 'Abbott');

INSERT INTO TestTbl (TestName, Description, TestFee)
VALUES
('Complete Blood Count (CBC)', 'General health check, infections', 300.00),
('Thyroid Profile (T3, T4, TSH)', 'Thyroid gland check', 450.00),
('Vitamin D Test', 'Vitamin D deficiency check', 900.00);

select * from DoctorTbl

--1. Stored procedure for user login
CREATE PROCEDURE sp_AuthenticateUser @UserName NVARCHAR(50),@Password NVARCHAR(10),@RoleId INT OUTPUT
AS
BEGIN
SELECT @RoleId = RoleId 
FROM UserTbl
WHERE UserName = @UserName AND Password = @Password AND IsActive = 1;
IF @RoleId IS NULL
SET @RoleId = 0;
END

--2. Search Patient by MMRNumber
CREATE PROCEDURE sp_SearchPatientByMMR @MMRNumber NVARCHAR(20)
AS
BEGIN
SELECT P.PatientId,P.Name,P.DateOfBirth,P.Gender,P.Phone,P.Address,
P.MMRNumber,B.BillId,B.ConsultationFee,B.LabFee,B.MedicineFee,B.TotalAmount,
ISNULL(PD.AmountPaid,0) AS AmountPaid,
CASE 
WHEN ISNULL(PD.AmountPaid,0) >= ISNULL(B.TotalAmount,0) THEN 'Paid'
ELSE 'Pending'
END AS PaymentStatus,
ISNULL(M.MembershipName, 'No Membership') AS MembershipName
FROM PatientTbl P
LEFT JOIN BillTbl B 
ON P.PatientId = B.PatientId
LEFT JOIN PaymentDetailTbl PD 
ON B.BillId = PD.BillId
LEFT JOIN PatientMembershipTbl PM
ON P.PatientId = PM.PatientId
LEFT JOIN MembershipTbl M
ON PM.MembershipId = M.MembershipId
WHERE P.MMRNumber = @MMRNumber;
END;

DROP PROCEDURE sp_SearchPatientByPhone;

--3. Search Patient by Phone Number
CREATE OR ALTER PROCEDURE sp_SearchPatientByPhone @Phone VARCHAR(15)
AS
BEGIN
SELECT P.PatientId,P.Name,P.DateOfBirth,P.Gender,P.Phone,P.Address,P.MMRNumber,
B.BillId,.ConsultationFee,B.LabFee,B.MedicineFee,B.TotalAmount,
ISNULL(PD.AmountPaid,0) AS AmountPaid,
CASE 
WHEN ISNULL(PD.AmountPaid,0) >= ISNULL(B.TotalAmount,0) THEN 'Paid'
ELSE 'Pending'
END AS PaymentStatus,
ISNULL(M.MembershipName, 'No Membership') AS MembershipName   
FROM PatientTbl P
LEFT JOIN BillTbl B ON P.PatientId = B.PatientId
LEFT JOIN PaymentDetailTbl PD ON B.BillId = PD.BillId
LEFT JOIN PatientMembershipTbl PM ON P.PatientId = PM.PatientId
LEFT JOIN MembershipTbl M ON PM.MembershipId = M.MembershipId
WHERE P.Phone = @Phone;
END;



--4. Add New Patient (Generate MMRNumber)
CREATE OR ALTER PROCEDURE sp_AddPatient @Name NVARCHAR(100),@DOB DATE,@Gender CHAR(1),@Phone VARCHAR(15),
@Address NVARCHAR(200)
AS
BEGIN
DECLARE @MMR NVARCHAR(20);
DECLARE @Seq INT;
SELECT @Seq = COUNT(*) + 1 FROM PatientTbl 
WHERE YEAR(GETDATE()) = YEAR(GETDATE()); 
SET @MMR = 'MMR'+ RIGHT(CAST(YEAR(GETDATE()) AS NVARCHAR), 2) + RIGHT('00' + CAST(@Seq AS NVARCHAR), 2);
INSERT INTO PatientTbl (Name, DateOfBirth, Gender, Phone, Address, MMRNumber)
VALUES (@Name, @DOB, @Gender, @Phone, @Address, @MMR);
SELECT SCOPE_IDENTITY() AS PatientId, @MMR AS MMRNumber;
END;


--5. Get Membership List
CREATE PROCEDURE sp_GetMemberships
AS
BEGIN
SELECT MembershipId, MembershipName, DiscountPercent, DurationMonths, Fee
FROM MembershipTbl;
END;

--6. Assign Membership to Patient
CREATE PROCEDURE sp_AssignMembership @PatientId INT,@MembershipId INT
AS
BEGIN
DECLARE @DurationMonths INT;
SELECT @DurationMonths = DurationMonths FROM MembershipTbl WHERE MembershipId = @MembershipId;
INSERT INTO PatientMembershipTbl (PatientId, MembershipId, StartDate, EndDate, IsActive)
VALUES (@PatientId, @MembershipId, GETDATE(), DATEADD(MONTH, @DurationMonths, GETDATE()), 1);
END;

--7. View Doctor Availability & Existing Appointments
CREATE PROCEDURE sp_GetDoctorSchedule @DoctorId INT,@AppointmentDate DATE
AS
BEGIN
SELECT A.AppointmentId,A.PatientId,P.Name AS PatientName,A.AppointmentDate,A.Status
FROM AppointmentTbl A
JOIN PatientTbl P ON A.PatientId = P.PatientId
WHERE A.DoctorId = @DoctorId
AND CAST(A.AppointmentDate AS DATE) = @AppointmentDate
ORDER BY A.AppointmentDate;
END;

DROP PROCEDURE IF EXISTS sp_BookAppointment;

--8. Register Appointment
CREATE OR ALTER PROCEDURE sp_BookAppointment @PatientId INT,@DoctorId INT,@AppointmentDate DATETIME
AS
BEGIN
-- Appointment duration in minutes (1.5 hours = 90 mins)
DECLARE @AppointmentEnd DATETIME
SET @AppointmentEnd = DATEADD(MINUTE, 90, @AppointmentDate)
-- Check if doctor has another appointment that overlaps
IF EXISTS (SELECT 1
FROM AppointmentTbl
WHERE DoctorId = @DoctorId
AND Status = 'Scheduled'
AND @AppointmentDate < DATEADD(MINUTE, 90, AppointmentDate)
AND @AppointmentEnd > AppointmentDate
)
BEGIN
-- Doctor is busy, cannot book this slot
RETURN 0
END
-- Optional: check if the patient already has an appointment at this time
IF EXISTS (SELECT 1
FROM AppointmentTbl
WHERE PatientId = @PatientId
AND Status = 'Scheduled'
AND @AppointmentDate < DATEADD(MINUTE, 90, AppointmentDate)
AND @AppointmentEnd > AppointmentDate
)
BEGIN
-- Patient is busy
RETURN -1
END
-- Generate token number for this doctor on this date
DECLARE @Token INT
SELECT @Token = ISNULL(MAX(TokenNo), 0) + 1
FROM AppointmentTbl
WHERE DoctorId = @DoctorId
AND CAST(AppointmentDate AS DATE) = CAST(@AppointmentDate AS DATE)
-- Insert new appointment with token
INSERT INTO AppointmentTbl (PatientId, DoctorId, AppointmentDate, Status, TokenNo)
VALUES (@PatientId, @DoctorId, @AppointmentDate, 'Scheduled', @Token)
-- Return the AppointmentId and TokenNo
SELECT SCOPE_IDENTITY() AS AppointmentId, @Token AS TokenNo;
END

--9. View Today’s Appointments for Doctor
CREATE PROCEDURE sp_Doctor_ViewAppointments @DoctorId INT
AS
BEGIN
SELECT a.AppointmentId,p.Name AS PatientName,p.MMRNumber,a.AppointmentDate,
ISNULL(a.Status, 'Pending') AS Status,  -- Default to Pending
a.TokenNo
FROM AppointmentTbl a
JOIN PatientTbl p ON a.PatientId = p.PatientId
WHERE a.DoctorId = @DoctorId
AND CAST(a.AppointmentDate AS DATE) = CAST(GETDATE() AS DATE)
ORDER BY a.TokenNo;
END;

--10. Start Consultation (Insert Diagnosis)
CREATE PROCEDURE sp_Doctor_StartConsultation @AppointmentId INT,@Diagnosis NVARCHAR(200),@ConsultationId INT OUTPUT
AS
BEGIN
-- Update appointment status to 'In Progress'
UPDATE AppointmentTbl
SET Status = 'Completed'
WHERE AppointmentId = @AppointmentId;
-- Insert consultation details
INSERT INTO ConsultationTbl (AppointmentId, Diagnosis)
VALUES (@AppointmentId, @Diagnosis);
SET @ConsultationId = SCOPE_IDENTITY();
END;

--11. To mark consultation as Completed
CREATE OR ALTER PROCEDURE sp_Doctor_FinishConsultation @AppointmentId INT
AS
BEGIN
UPDATE AppointmentTbl
SET Status = 'Completed'
WHERE AppointmentId = @AppointmentId;
END;

--12. Prescribe Medicine
CREATE PROCEDURE sp_Doctor_AddMedicine @ConsultationId INT,@PatientId INT,@MedicineId INT,@Dosage NVARCHAR(50),
@Duration NVARCHAR(50),@Quantity INT
AS
BEGIN
DECLARE @PharmacyPresId INT;
-- If no prescription exists, create master entry
IF NOT EXISTS (SELECT 1 FROM PharmacyPrescriptionTbl WHERE ConsultationId = @ConsultationId)
BEGIN
INSERT INTO PharmacyPrescriptionTbl (PatientId, ConsultationId)
VALUES (@PatientId, @ConsultationId);
SET @PharmacyPresId = SCOPE_IDENTITY();
END
ELSE
SELECT @PharmacyPresId = PharmacyPresId FROM PharmacyPrescriptionTbl WHERE ConsultationId = @ConsultationId;
-- Insert medicine detail
INSERT INTO PharmacyPrescriptionDetailTbl (PharmacyPresId, MedicineName, Dosage, Duration, Price, MedicineId)
SELECT @PharmacyPresId, MedicineName, @Dosage, @Duration, UnitPrice * @Quantity, MedicineId
FROM MedicineTbl WHERE MedicineId = @MedicineId;
END;

--13. Prescribe Test
CREATE PROCEDURE sp_Doctor_AddTest @ConsultationId INT,@PatientId INT,@TestId INT
AS
BEGIN
DECLARE @LabPresId INT;
-- If no prescription exists, create master entry
IF NOT EXISTS (SELECT 1 FROM LabPrescriptionTbl WHERE ConsultationId = @ConsultationId)
BEGIN
INSERT INTO LabPrescriptionTbl (PatientId, ConsultationId)
VALUES (@PatientId, @ConsultationId);
SET @LabPresId = SCOPE_IDENTITY();
END
ELSE
SELECT @LabPresId = LabPrescriptionId FROM LabPrescriptionTbl WHERE ConsultationId = @ConsultationId;
-- Insert test detail
INSERT INTO LabPrescriptionDetailTbl (LabPrescriptionId, TestName, TestFee, TestId)
SELECT @LabPresId, TestName, TestFee, TestId FROM TestTbl WHERE TestId = @TestId;
END;

--14. Generate Bill (with Membership Discount)
CREATE PROCEDURE sp_Doctor_GenerateBill @ConsultationId INT, @PatientId INT,@DoctorId INT
AS
BEGIN
-- Get Consultation Fee (Doctor Fee)
DECLARE @ConsultFee DECIMAL(10,2) = (
SELECT DoctorFee 
FROM DoctorTbl 
WHERE DoctorId = @DoctorId
);
-- Total is only consultation fee
DECLARE @TotalAmount DECIMAL(10,2) = ISNULL(@ConsultFee,0);
-- Apply membership discount
DECLARE @DiscountPercent DECIMAL(5,2) = (
SELECT TOP 1 m.DiscountPercent
FROM PatientMembershipTbl pm
JOIN MembershipTbl m ON pm.MembershipId = m.MembershipId
WHERE pm.PatientId = @PatientId
AND pm.IsActive = 1
AND GETDATE() BETWEEN pm.StartDate AND pm.EndDate
);
IF @DiscountPercent IS NULL SET @DiscountPercent = 0;
DECLARE @FinalAmount DECIMAL(10,2) = @TotalAmount - (@TotalAmount * @DiscountPercent / 100);
 -- Insert Bill (only Consultation Fee considered)
INSERT INTO BillTbl (PatientId, ConsultationId, ConsultationFee, LabFee, MedicineFee)
VALUES (@PatientId, @ConsultationId, ISNULL(@ConsultFee,0), 0, 0);
DECLARE @BillId INT = SCOPE_IDENTITY();
-- Return Bill info
SELECT @BillId AS BillId,@TotalAmount AS GrossAmount,@DiscountPercent AS DiscountPercent,@FinalAmount AS NetPayable;
END;
 

