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

CREATE PROC SP_InvoiceReport
    @InvoiceID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        I.InvoiceID,
        I.InvoiceDate,
        ST.StoreID,
        ST.StoreName,
        ST.Address AS StoreAddress,
        ST.Phone AS StorePhone,
        I.EmployeeID,
        E.FullName AS EmployeeName,
        P.ProductName,
        P.Unit,
        ID.Quantity,
        ID.UnitPrice,
        (ID.Quantity * ID.UnitPrice) AS Total,
        SUM(ID.Quantity * ID.UnitPrice) OVER (PARTITION BY I.InvoiceID) AS TotalAmount
    FROM Invoice I
    INNER JOIN InvoiceDetails ID ON I.InvoiceID = ID.InvoiceID
    INNER JOIN Products P ON ID.ProductID = P.ProductID
    INNER JOIN Store ST ON I.StoreID = ST.StoreID
    INNER JOIN Employee E ON I.EmployeeID = E.EmployeeID
    WHERE I.InvoiceID = @InvoiceID;
END
GO
--EXEC SP_InvoiceReport 'EXP20251102001'
--GO
CREATE PROC SP_ReturnReport
    @ReturnID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        RP.ReturnID,
        RP.ReturnDate,
        RP.InvoiceID,
        I.InvoiceDate,
        RP.EmployeeID,
        E.FullName AS EmployeeName,
        S.StoreID,
        S.StoreName,
        S.Address AS StoreAddress,
        P.ProductName,
        P.Unit,
        RD.Quantity,
        RD.RefundAmount,
        (RD.Quantity * RD.RefundAmount) AS TotalRefund,
        SUM(RD.Quantity * RD.RefundAmount) OVER (PARTITION BY RP.ReturnID) AS TotalAmount
    FROM ReturnProduct RP
    INNER JOIN ReturnDetails RD ON RP.ReturnID = RD.ReturnID
    INNER JOIN Products P ON RD.ProductID = P.ProductID
    INNER JOIN Invoice I ON RP.InvoiceID = I.InvoiceID
    INNER JOIN Store S ON I.StoreID = S.StoreID
    INNER JOIN Employee E ON RP.EmployeeID = E.EmployeeID
    WHERE RP.ReturnID = @ReturnID;
END
GO

CREATE PROC SP_StoreRevenueByMonth
    @StoreID NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        YEAR(I.InvoiceDate) AS Year,
        MONTH(I.InvoiceDate) AS Month,
        S.StoreID,
        S.StoreName,
        SUM(ID.Quantity * ID.UnitPrice) AS Revenue
    FROM Invoice I
    INNER JOIN InvoiceDetails ID ON I.InvoiceID = ID.InvoiceID
    INNER JOIN Store S ON I.StoreID = S.StoreID
    WHERE (@StoreID IS NULL OR S.StoreID = @StoreID)
    GROUP BY
        YEAR(I.InvoiceDate),
        MONTH(I.InvoiceDate),
        S.StoreID,
        S.StoreName
    ORDER BY
        YEAR(I.InvoiceDate),
        MONTH(I.InvoiceDate);
END
GO
CREATE PROCEDURE GetStockByPeriod
    @StoreId NVARCHAR(50),
    @ProductId NVARCHAR(50),
    @Month INT,
    @Year INT
AS
BEGIN
    DECLARE @StartDate DATE = DATEFROMPARTS(@Year, @Month, 1);
    DECLARE @EndDate DATE = EOMONTH(@StartDate);

    -- T?n ??u k?
    DECLARE @OpeningStock INT =
    (
        SELECT 
            ISNULL(SUM(PID.Quantity), 0) 
            - ISNULL((SELECT SUM(ID.Quantity)
                      FROM InvoiceDetail ID
                      JOIN Invoice I ON ID.InvoiceID = I.InvoiceID
                      WHERE I.StoreID = @StoreId
                        AND ID.ProductID = @ProductId
                        AND I.InvoiceDate < @StartDate), 0)
        FROM PurchaseInvoiceDetail PID
        JOIN PurchaseInvoice PI ON PID.InvoiceID = PI.InvoiceID
        WHERE PI.StoreID = @StoreId
          AND PID.ProductID = @ProductId
          AND PI.ReceiptDate < @StartDate
    );

    -- Nh?p trong k?
    DECLARE @TotalImport INT =
    (
        SELECT ISNULL(SUM(PID.Quantity), 0)
        FROM PurchaseInvoiceDetail PID
        JOIN PurchaseInvoice PI ON PID.InvoiceID = PI.InvoiceID
        WHERE PI.StoreID = @StoreId
          AND PID.ProductID = @ProductId
          AND PI.ReceiptDate BETWEEN @StartDate AND @EndDate
    );

    -- Xu?t trong k?
    DECLARE @TotalExport INT =
    (
        SELECT ISNULL(SUM(ID.Quantity), 0)
        FROM InvoiceDetail ID
        JOIN Invoice I ON ID.InvoiceID = I.InvoiceID
        WHERE I.StoreID = @StoreId
          AND ID.ProductID = @ProductId
          AND I.InvoiceDate BETWEEN @StartDate AND @EndDate
    );  

    SELECT
        @OpeningStock AS OpeningStock,
        @TotalImport AS ImportInPeriod,
        @TotalExport AS ExportInPeriod,
        @OpeningStock + @TotalImport - @TotalExport AS ClosingStock
END;
