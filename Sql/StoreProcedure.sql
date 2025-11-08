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


CREATE PROC SP_StoreRevenueByTimeResult
    @StoreID NVARCHAR(200) = NULL,
    @FromDate DATE = NULL,
    @ToDate DATE = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        YEAR(I.InvoiceDate) AS [Year],
        MONTH(I.InvoiceDate) AS [Month],
        S.StoreID,
        S.StoreName,
        SUM(ID.Quantity * ID.UnitPrice) AS Revenue
    FROM Invoice I
    INNER JOIN InvoiceDetails ID ON I.InvoiceID = ID.InvoiceID
    INNER JOIN Store S ON I.StoreID = S.StoreID
    WHERE 
        (@StoreID IS NULL OR S.StoreID = @StoreID)
        AND (@FromDate IS NULL OR I.InvoiceDate >= @FromDate)
        AND (@ToDate IS NULL OR I.InvoiceDate <= @ToDate)
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

GO
CREATE PROCEDURE sp_GetSalaryContractReport
    @EmployeeID NVARCHAR(200)
AS
BEGIN
    SET NOCOUNT ON;

    -- Lấy thông tin hợp đồng và nhân viên
    SELECT 
        sc.ContractID,
        e.EmployeeID,
        e.FullName,
        e.Position,
        e.EmploymentType,
        s.StoreName,
        sc.BasicSalary,
        sc.HourlyRate,
        sc.StartDate,
        sc.EndDate,
        ISNULL(SUM(ISNULL(sca.CustomAmount, a.DefaultAmount)), 0) AS TotalAllowance,
        ISNULL(sc.BasicSalary, 0) + ISNULL(SUM(ISNULL(sca.CustomAmount, a.DefaultAmount)), 0) AS EstimatedTotalIncome
    FROM SalaryContract sc
        INNER JOIN Employee e ON sc.EmployeeID = e.EmployeeID
        LEFT JOIN Store s ON e.StoreID = s.StoreID
        LEFT JOIN SalaryContract_Allowances sca ON sc.ContractID = sca.ContractID
        LEFT JOIN Allowances a ON sca.AllowanceID = a.AllowanceID
    WHERE sc.EmployeeID = @EmployeeID AND sc.IsDeleted = 0
    GROUP BY 
        sc.ContractID, e.EmployeeID, e.FullName, e.Position, e.EmploymentType,
        s.StoreName, sc.BasicSalary, sc.HourlyRate, sc.StartDate, sc.EndDate;
	SELECT 
        a.AllowanceName,
        ISNULL(sca.CustomAmount, a.DefaultAmount) AS Amount
    FROM SalaryContract sc
    INNER JOIN SalaryContract_Allowances sca ON sc.ContractID = sca.ContractID
    INNER JOIN Allowances a ON sca.AllowanceID = a.AllowanceID
    WHERE sc.EmployeeID = @EmployeeID AND sc.IsDeleted = 0
END
GO
-- DROP PROCEDURE sp_GetSalaryContractReport @EmployeeID = 'EMP20251026200001c2c'

--CREATE PROC SP_StoreFinancialReportByMonth
--    @StoreID NVARCHAR(200) = NULL
--AS
--BEGIN
--    SET NOCOUNT ON;

--    -- Doanh Thu theo tháng
--    ;WITH RevenueCTE AS (
--        SELECT
--            YEAR(I.InvoiceDate) AS Year,
--            MONTH(I.InvoiceDate) AS Month,
--            I.StoreID,
--            SUM(ID.Quantity * ID.FinalUnitPrice) AS Revenue
--        FROM Invoice I
--        INNER JOIN InvoiceDetails ID ON I.InvoiceID = ID.InvoiceID
--        WHERE I.Status = 1 -- chỉ tính hóa đơn đã hoàn tất
--          AND (@StoreID IS NULL OR I.StoreID = @StoreID)
--        GROUP BY YEAR(I.InvoiceDate), MONTH(I.InvoiceDate), I.StoreID
--    ),

--    -- Chi phí cố định theo tháng
--    FixedExpenseCTE AS (
--        SELECT
--            CAST(LEFT(FE.MonthYear, 4) AS INT) AS Year,
--            CAST(RIGHT(FE.MonthYear, 2) AS INT) AS Month,
--            FE.StoreID,
--            SUM(
--                COALESCE(FE.RentCost,0) 
--                + COALESCE(FE.ElectricityCost,0) 
--                + COALESCE(FE.WaterCost,0)
--            ) AS FixedExpense
--        FROM StoreFixedExpenses FE
--        WHERE FE.IsDeleted = 0
--          AND FE.Status = 1 -- chỉ tính chi phí đã duyệt
--          AND (@StoreID IS NULL OR FE.StoreID = @StoreID)
--        GROUP BY FE.StoreID, FE.MonthYear
--    ),

--    -- Lương (chi phí linh hoạt)
--    SalaryExpenseCTE AS (
--        SELECT
--            CAST(LEFT(S.MonthYear, 4) AS INT) AS Year,
--            CAST(RIGHT(S.MonthYear, 2) AS INT) AS Month,
--            E.StoreID,
--            SUM(
--                COALESCE(SC.BasicSalary, 0) 
--                + COALESCE(S.Bonus,0)
--                - COALESCE(S.Deduction,0)
--            ) AS SalaryExpense
--        FROM Salary S
--        INNER JOIN SalaryContract SC ON S.ContractID = SC.ContractID
--        INNER JOIN Employee E ON SC.EmployeeID = E.EmployeeID
--        WHERE S.Status = N'Đã duyệt'
--          AND E.IsDeleted = 0
--          AND (@StoreID IS NULL OR E.StoreID = @StoreID)
--        GROUP BY E.StoreID, S.MonthYear
--    )

--    SELECT
--        R.Year,
--        R.Month,
--        S.StoreID,
--        ST.StoreName,
--        R.Revenue,
--        COALESCE(F.FixedExpense,0) AS FixedExpense,
--        COALESCE(SL.SalaryExpense,0) AS SalaryExpense,
--        (R.Revenue 
--            - COALESCE(F.FixedExpense,0) 
--            - COALESCE(SL.SalaryExpense,0)) AS Financial
--    FROM RevenueCTE R
--    INNER JOIN Store ST ON R.StoreID = ST.StoreID
--    LEFT JOIN FixedExpenseCTE F ON R.StoreID = F.StoreID 
--        AND R.Year = F.Year AND R.Month = F.Month
--    LEFT JOIN SalaryExpenseCTE SL ON R.StoreID = SL.StoreID 
--        AND R.Year = SL.Year AND R.Month = SL.Month
--    ORDER BY R.Year, R.Month;
--END
--GO
CREATE PROC SP_StoreFinancialReportByMonth
    @StoreID NVARCHAR(200) = NULL
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH RevenueCTE AS (
        SELECT
            YEAR(HD.InvoiceDate) AS ReportYear,
            MONTH(HD.InvoiceDate) AS ReportMonth,
            HD.StoreID AS StoreID,
            SUM(ISNULL(CTHD.FinalUnitPrice,0)) AS Revenue
        FROM Invoice HD
        INNER JOIN InvoiceDetails CTHD ON HD.InvoiceID = CTHD.InvoiceID
        WHERE HD.Status = 1
          AND (@StoreID IS NULL OR HD.StoreID = @StoreID)
        GROUP BY YEAR(HD.InvoiceDate), MONTH(HD.InvoiceID), HD.StoreID
    ),

    FixedExpenseCTE AS (
        SELECT
            CAST(RIGHT(FE.MonthYear, 4) AS INT) AS ReportYear,
            CAST(LEFT(FE.MonthYear, CHARINDEX('/', FE.MonthYear) - 1) AS INT) AS ReportMonth,
            FE.StoreID AS StoreID,
            SUM(
                COALESCE(FE.TienThueMatBang,0)
                + COALESCE(FE.TienDien,0)
                + COALESCE(FE.TienNuoc,0)
            ) AS FixedExpense
        FROM StoreFixedExpenses FE
        WHERE FE.IsDeleted = 0
          AND FE.Status = 1
          AND (@StoreID IS NULL OR FE.StoreID = @StoreID)
        GROUP BY FE.StoreID, FE.MonthYear
    )

    SELECT
        R.ReportYear AS [Year],
        R.ReportMonth AS [Month],
        R.StoreID,
        CH.StoreName AS StoreName,
        R.Revenue,
        COALESCE(F.FixedExpense,0) AS FixedExpense,
        (R.Revenue - COALESCE(F.FixedExpense,0)) AS Financial
    FROM RevenueCTE R
    INNER JOIN Store CH ON R.StoreID = CH.StoreID
    LEFT JOIN FixedExpenseCTE F 
        ON R.StoreID = F.StoreID 
        AND R.ReportYear = F.ReportYear 
        AND R.ReportMonth = F.ReportMonth
    ORDER BY R.ReportYear, R.ReportMonth;
END
GO

exec SP_StoreFinancialReportByMonth 'STR20251026202700739'
Go