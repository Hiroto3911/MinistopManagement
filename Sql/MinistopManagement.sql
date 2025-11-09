
IF NOT EXISTS(SELECT * FROM sys.databases WHERE name = 'MinistopManagement')
BEGIN
    CREATE DATABASE MinistopManagement;
END
GO

USE MinistopManagement;
GO


CREATE TABLE Store (
    StoreID NVARCHAR(200) PRIMARY KEY NOT NULL,
    StoreName NVARCHAR(100) NOT NULL,
    Address NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0 ,
    CreatedBy NVARCHAR(max) NULL,
    Created DATETIME NOT NULL,
    LastModifiedBy NVARCHAR(max) NULL,
    LastModified DATETIME NULL
); 
GO


CREATE TABLE StoreFixedExpenses (
    ExpenseID NVARCHAR(200) PRIMARY KEY NOT NULL,
    StoreID NVARCHAR(200) NOT NULL,
    MonthYear CHAR(7) NOT NULL,
    RentCost DECIMAL(18,2) NULL,
    ElectricityCost DECIMAL(18,2) NOT NULL,
    WaterCost DECIMAL(18,2) NOT NULL,
    Note NVARCHAR(255) NULL,
    Status TINYINT NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0  ,
    CreatedBy NVARCHAR(max) NULL,
    Created DATETIME NOT NULL,
    LastModifiedBy NVARCHAR(max) NULL,
    LastModified DATETIME NULL,
    FOREIGN KEY (StoreID) REFERENCES Store(StoreID)
);
GO


CREATE TABLE ProductCategory (
    CategoryID NVARCHAR(200) PRIMARY KEY,
    CategoryName NVARCHAR(100) NOT NULL,
    Description NVARCHAR(200) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0   ,
    CreatedBy NVARCHAR(max) NULL,
    Created DATETIME NOT NULL,
    LastModifiedBy NVARCHAR(max) NULL,
    LastModified DATETIME NULL
); 
GO


CREATE TABLE Products (
    ProductID NVARCHAR(200) PRIMARY KEY,
    CategoryID NVARCHAR(200) NOT NULL,
    ProductName NVARCHAR(200) NOT NULL,
    Unit NVARCHAR(20) NOT NULL,
    StandardPrice DECIMAL(18,2) NOT NULL,
    Status TINYINT NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0 ,
    CreatedBy NVARCHAR(max) NULL,
    Created DATETIME NOT NULL,
    LastModifiedBy NVARCHAR(max) NULL,
    LastModified DATETIME NULL,
    FOREIGN KEY (CategoryID) REFERENCES ProductCategory(CategoryID)
); 
GO

CREATE TABLE Employee (
    EmployeeID NVARCHAR(200) PRIMARY KEY,
    StoreID NVARCHAR(200) NULL,
    FullName NVARCHAR(100) NOT NULL,
    Gender BIT NOT NULL ,
    BirthDate DATE NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Position NVARCHAR(50) NOT NULL,
    EmploymentType NVARCHAR(20) NOT NULL,
    PasswordHash NVARCHAR(200) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0 ,
    CreatedBy NVARCHAR(max) NULL,
    Created DATETIME NOT NULL,
    LastModifiedBy NVARCHAR(max) NULL,
    LastModified DATETIME NULL,
    FOREIGN KEY (StoreID) REFERENCES Store(StoreID)
);
GO

CREATE TABLE SalaryContract (
    ContractID NVARCHAR(200) PRIMARY KEY,
    EmployeeID NVARCHAR(200) NOT NULL,
    BasicSalary DECIMAL(18,2) NULL,  -- Có giá trị nếu fulltime
    HourlyRate DECIMAL(18,2) NULL,   -- Có giá trị nếu parttime
    StartDate DATE NOT NULL,
    EndDate DATE NULL,
    IsDeleted BIT NOT NULL DEFAULT 0 ,
    CreatedBy NVARCHAR(max) NULL,
    Created DATETIME NOT NULL,
    LastModifiedBy NVARCHAR(max) NULL,
    LastModified DATETIME NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);
GO

CREATE TABLE Allowances (
    AllowanceID NVARCHAR(200) PRIMARY KEY,
    AllowanceName NVARCHAR(100) NOT NULL,      -- VD: "Cơm", "Đi lại", "Gửi xe"
    DefaultAmount  DECIMAL(18,2) NOT NULL, -- mức trợ cấp mặc định 
    IsDeleted BIT NOT NULL DEFAULT 0 ,
    CreatedBy NVARCHAR(max) NULL,
    Created DATETIME NOT NULL,
    LastModifiedBy NVARCHAR(max) NULL,
    LastModified DATETIME NULL
 
);
GO
CREATE TABLE SalaryContract_Allowances (
    Id NVARCHAR(200) NOT NULL PRIMARY KEY,
    ContractID NVARCHAR(200) NOT NULL,
    AllowanceID NVARCHAR(200) NOT NULL,
    CustomAmount DECIMAL(18,2) NULL,  -- Nếu NULL thì dùng DefaultAmount
    FOREIGN KEY (ContractID) REFERENCES SalaryContract(ContractID),
    FOREIGN KEY (AllowanceID) REFERENCES Allowances(AllowanceID),
    UNIQUE (ContractID, AllowanceID)
);

 
CREATE TABLE Shifts (
    ShiftID NVARCHAR(200) PRIMARY KEY,
    ShiftName NVARCHAR(50) NOT NULL,
    StartTime TIME NOT NULL,
    EndTime TIME NOT NULL
);
GO



CREATE TABLE ShiftAssignment (
    Id NVARCHAR(200) NOT NULL PRIMARY KEY,
    EmployeeID NVARCHAR(200) NOT NULL,
    ShiftID NVARCHAR(200) NOT NULL,
    WorkDate DATE NOT NULL,
    Note NVARCHAR(MAX) NULL,
    IsDeleted BIT NOT NULL DEFAULT 0,
    CreatedBy NVARCHAR(MAX) NULL,
    Created DATETIME NOT NULL,
    LastModifiedBy NVARCHAR(MAX) NULL,
    LastModified DATETIME NULL,
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID),
    FOREIGN KEY (ShiftID) REFERENCES Shifts(ShiftID),
    UNIQUE (EmployeeID, ShiftID, WorkDate)
);

CREATE TABLE Absence (
    AbsenceID NVARCHAR(200) PRIMARY KEY,
    EmployeeID NVARCHAR(200) NOT NULL,
    ShiftID NVARCHAR(200) NOT NULL,
    WorkDate DATE NOT NULL,
    IsLeaveOfAbsence BIT NOT NULL DEFAULT 0  ,
    Reason NVARCHAR (MAX),
    IsPaid BIT	NOT NULL,
    CONSTRAINT UQ_Absence_shiftAssignment UNIQUE (EmployeeID, ShiftID, WorkDate),
    FOREIGN KEY (EmployeeID, ShiftID, WorkDate) 
        REFERENCES ShiftAssignment(EmployeeID, ShiftID, WorkDate)
);
GO


CREATE TABLE Salary (
    SalaryID NVARCHAR(200) PRIMARY KEY,
    ContractID NVARCHAR(200) NOT NULL,
    MonthYear CHAR(7) NOT NULL,
    Bonus  DECIMAL(18,2)  NULL,
    Deduction DECIMAL(10,2)  NULL,
    Status NVARCHAR(20)  NULL,
    FOREIGN KEY (ContractID) REFERENCES SalaryContract(ContractID)
); 
GO


CREATE TABLE Supplier (
    SupplierID NVARCHAR(200) PRIMARY KEY,
    SupplierName NVARCHAR(200) NOT NULL,
    Phone NVARCHAR(20) NOT NULL,
    Address NVARCHAR(200) NOT NULL,
    IsDeleted BIT NOT NULL DEFAULT 0 ,
    CreatedBy NVARCHAR(max) NULL,
    Created DATETIME NOT NULL,
    LastModifiedBy NVARCHAR(max) NULL,
    LastModified DATETIME NULL
); 
GO


CREATE TABLE StockImport (
    ImportID NVARCHAR(200) PRIMARY KEY,
    StoreID NVARCHAR(200) NOT NULL,
    SupplierID NVARCHAR(200) NOT NULL,
    EmployeeID NVARCHAR(200) NOT NULL,
    ImportDate DATETIME NOT NULL,
    Status TINYINT NOT NULL,
    FOREIGN KEY (StoreID) REFERENCES Store(StoreID),
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);
GO

CREATE TABLE StockImportDetails (
    Id NVARCHAR(200) NOT NULL PRIMARY KEY,
    ImportID NVARCHAR(200) NOT NULL,
    ProductID NVARCHAR(200) NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (ImportID) REFERENCES StockImport(ImportID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    UNIQUE (ImportID, ProductID)
);

CREATE TABLE StockExport (
    ExportID NVARCHAR(200) PRIMARY KEY,
    StoreID NVARCHAR(200) NOT NULL,
    EmployeeID NVARCHAR(200) NOT NULL,
    TypeExport NVARCHAR(200) NOT NULL,
    ExportDate DATETIME NOT NULL,
    Reason NVARCHAR(100) NULL,
    Status TINYINT NOT NULL,
    FOREIGN KEY (StoreID) REFERENCES Store(StoreID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);
GO

CREATE TABLE StockExportDetails (
    Id NVARCHAR(200) NOT NULL PRIMARY KEY,
    ExportID NVARCHAR(200) NOT NULL,
    ProductID NVARCHAR(200) NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (ExportID) REFERENCES StockExport(ExportID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    UNIQUE (ExportID, ProductID)
);


CREATE TABLE StockCheck (
    CheckID NVARCHAR(200) PRIMARY KEY,
    StoreID NVARCHAR(200) NOT NULL,
    EmployeeID NVARCHAR(200) NOT NULL,
    CheckDate DATE NOT NULL,
    Status TINYINT NOT NULL,
    FOREIGN KEY (StoreID) REFERENCES Store(StoreID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);
GO

CREATE TABLE StockCheckDetails (
    Id NVARCHAR(200) NOT NULL PRIMARY KEY,
    CheckID NVARCHAR(200) NOT NULL,
    ProductID NVARCHAR(200) NOT NULL,
    QuantitySystem INT NOT NULL,
    QuantityActual INT NOT NULL,
    Note NVARCHAR(200) NULL,
    FOREIGN KEY (CheckID) REFERENCES StockCheck(CheckID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    UNIQUE (CheckID, ProductID)
);

CREATE TABLE StockDetail (
    StockDetailID NVARCHAR(200) PRIMARY KEY,
    StoreID NVARCHAR(200) NOT NULL,
    ProductID NVARCHAR(200) NOT NULL,
    Quantity INT NOT NULL,
    Price DECIMAL(18,2) NOT NULL,
    LastUpdate DATETIME NOT NULL,
    FOREIGN KEY (StoreID) REFERENCES Store(StoreID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID)
);
GO


CREATE TABLE StockHistory (
    StockHistoryID NVARCHAR(200) PRIMARY KEY,
    StockDetailID NVARCHAR(200) NOT NULL,
    ChangeDate DATETIME NOT NULL,
    ChangeType NVARCHAR(20) NOT NULL,
    QuantityChange INT NOT NULL,
    RefID NVARCHAR(200) NULL,
    FOREIGN KEY (StockDetailID) REFERENCES StockDetail(StockDetailID)
); 
GO


CREATE TABLE Invoice (
    InvoiceID NVARCHAR(200) PRIMARY KEY,
    StoreID NVARCHAR(200) NOT NULL,
    EmployeeID NVARCHAR(200) NOT NULL,
    InvoiceDate DATETIME NOT NULL,
    FinalAmount DECIMAL(18,2),
    DiscountTotal DECIMAL(18,2) NULL, -- tổng số tiền giảm
    Status TINYINT NOT NULL DEFAULT 0, -- 0: đang tạo | 1: đã hoàn tất | 2: đã trả hàng
    FOREIGN KEY (StoreID) REFERENCES Store(StoreID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID)
);
GO

CREATE TABLE InvoiceDetails (
    Id NVARCHAR(200) NOT NULL PRIMARY KEY,
    InvoiceID NVARCHAR(200) NOT NULL,
    ProductID NVARCHAR(200) NOT NULL,
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(18,2) NOT NULL,
    FinalUnitPrice DECIMAL(18,2) NOT NULL, -- giá sau giảm
    DiscountAmount DECIMAL(18,2) NOT NULL DEFAULT 0,
    FOREIGN KEY (InvoiceID) REFERENCES Invoice(InvoiceID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    UNIQUE(InvoiceID,ProductID)
);
GO


CREATE TABLE ReturnProduct (
    ReturnID NVARCHAR(200) PRIMARY KEY,
    InvoiceID NVARCHAR(200) NOT NULL,
    EmployeeID NVARCHAR(200) NOT NULL,
    StoreID NVARCHAR(200) NOT NULL,
    ReturnDate DATETIME NOT NULL,
     Status TINYINT NOT NULL DEFAULT 0,
    FOREIGN KEY (InvoiceID) REFERENCES Invoice(InvoiceID),
    FOREIGN KEY (EmployeeID) REFERENCES Employee(EmployeeID),
    FOREIGN KEY (StoreID) REFERENCES Store(StoreID)
); 
GO

CREATE TABLE ReturnDetails (
    Id NVARCHAR(200) NOT NULL PRIMARY KEY,
    ReturnID NVARCHAR(200) NOT NULL,
    ProductID NVARCHAR(200) NOT NULL,
    Quantity INT NOT NULL,
    RefundAmount DECIMAL(18,2) NOT NULL,
    FOREIGN KEY (ReturnID) REFERENCES ReturnProduct(ReturnID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    UNIQUE (ReturnID, ProductID)
); 
GO


CREATE TABLE Promotion (
    PromotionID NVARCHAR(200) PRIMARY KEY,
    PromotionName NVARCHAR(100) NOT NULL,
    StartDate DATE NOT NULL,
    EndDate DATE NOT NULL,
    Priority INT NOT NULL,
    Status BIT NOT NULL DEFAULT 0 ,
    IsDeleted BIT NOT NULL DEFAULT 0 ,
    CreatedBy NVARCHAR(max) NULL,
    Created DATETIME NOT NULL,
    LastModifiedBy NVARCHAR(max) NULL,
    LastModified DATETIME NULL
);
GO

CREATE TABLE Promotion_Products (
    Id NVARCHAR(200) NOT NULL PRIMARY KEY,
    PromotionID NVARCHAR(200) NOT NULL,
    ProductID NVARCHAR(200) NOT NULL,
    DiscountAmount DECIMAL(18,2) NOT NULL,
    MinQuantity INT NOT NULL,
    Note NVARCHAR(255) NULL,
    FOREIGN KEY (PromotionID) REFERENCES Promotion(PromotionID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    UNIQUE (PromotionID, ProductID)
); 
GO


CREATE TABLE PriceProposal (
    ProposalID NVARCHAR(200) PRIMARY KEY,
    ProductID NVARCHAR(200) NOT NULL,
    StoreID NVARCHAR(200) NOT NULL,
    ManagerID NVARCHAR(200) NOT NULL, 
    OldPrice DECIMAL(18,2) NOT NULL,
    NewPrice DECIMAL(18,2) NOT NULL,
    Reason NVARCHAR(200) NOT NULL,
    ProposalDate DATETIME NOT NULL,
    Status TINYINT NOT NULL,
    ApprovedBy NVARCHAR(200) NULL, 
    FOREIGN KEY (ManagerID) REFERENCES Employee(EmployeeID),
    FOREIGN KEY (ApprovedBy) REFERENCES Employee(EmployeeID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    FOREIGN KEY (StoreID) REFERENCES Store(StoreID)
);
GO


CREATE TABLE SupplierProduct (
    Id NVARCHAR(200) NOT NULL PRIMARY KEY,
    SupplierID NVARCHAR(200) NOT NULL,
    ProductID NVARCHAR(200) NOT NULL,
    SupplyPrice DECIMAL(18,2) NOT NULL,
    Status TINYINT NOT NULL,  
    FOREIGN KEY (SupplierID) REFERENCES Supplier(SupplierID),
    FOREIGN KEY (ProductID) REFERENCES Products(ProductID),
    UNIQUE (SupplierID, ProductID)
); 
GO
