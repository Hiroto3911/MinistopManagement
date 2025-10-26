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