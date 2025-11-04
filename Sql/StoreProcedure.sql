USE MinistopManagement 
GO 

CREATE PROC SP_DanhSachCuaHangTheoThanhPho(@City NVARCHAR(200))
AS
BEGIN
SELECT *
FROM Store 
WHERE Address LIKE '%'+ @city +'%' And IsDeleted = 0 
END
GO

--EXEC SP_DanhSachCuaHangTheoThanhPho N'TP.HCM'
--GO

CREATE PROCEDURE SP_GetEmployeesByStore
    @StoreID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        E.EmployeeID,
        E.FullName,
        E.Gender,
        E.BirthDate,
        E.Phone,
        E.Position,
        E.EmploymentType,
        E.StoreID   
    FROM Employee E
    WHERE E.StoreID = @StoreID
      AND E.IsDeleted = 0
    ORDER BY E.FullName;
END
GO
--DROP PROCEDURE SP_GetEmployeesByStore;

CREATE PROCEDURE SP_StockImportReport
    @ImportID NVARCHAR(200)
AS
BEGIN
    SELECT 
        SIP.ImportID,
        SIP.ImportDate,
        SIP.StoreID,
		SIP.SupplierID,
        STO.StoreName,
		STO.Address,
        SPL.SupplierName,
        SIP.EmployeeID,
        EMP.FullName,
        PRD.ProductName,
        SDI.Quantity,
        SDI.UnitPrice,
        SDI.Quantity * SDI.UnitPrice AS Total,
	    Sum(SDI.Quantity * SDI.UnitPrice) OVER (PARTITION BY SDI.ImportID)  AS totalAmount
    FROM  StockImport SIP
    INNER JOIN StockImportDetails SDI ON SIP.ImportID = SDI.ImportID
	INNER JOIN Products PRD ON SDI.ProductID = PRD.ProductID
    INNER JOIN Supplier SPL ON SIP.SupplierID = SPL.SupplierID
    INNER JOIN Employee EMP ON SIP.EmployeeID = EMP.EmployeeID
    INNER JOIN Store STO ON SIP.StoreID = STO.StoreID
	 WHERE SIP.ImportID = @ImportID
   
END
GO

--EXEC SP_StockImportReport 'IMP20251101002'
--GO

CREATE PROC SP_StockExport  @ExportID NVARCHAR (200)
AS
BEGIN
     SELECT 
	    SEP.ExportID,
        SEP.ExportDate,
        SEP.StoreID,
        STO.StoreName,
		STO.Address,
        SEP.EmployeeID,
        EMP.FullName,
        PRD.ProductName,
        SDE.Quantity,
        SDE.UnitPrice,
        SDE.Quantity * SDE.UnitPrice AS Total,
	    Sum(SDE.Quantity * SDE.UnitPrice) OVER (PARTITION BY SDE.ExportID)  AS totalAmount
	 FROM StockExport SEP 
	 INNER JOIN StockExportDetails SDE ON SEP.ExportID = SDE.ExportID
	 INNER JOIN Products PRD ON SDE.ProductID = PRD.ProductID
     INNER JOIN Employee EMP ON SEP.EmployeeID = EMP.EmployeeID
     INNER JOIN Store STO ON SEP.StoreID = STO.StoreID
	 WHERE SEP.ExportID = @ExportID
END
GO

--EXEC SP_StockExport 'EXP20251102001'
--GO