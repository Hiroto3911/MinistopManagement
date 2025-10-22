INSERT INTO Store (StoreID, StoreName, Address, Phone, IsDeleted, CreatedBy, Created)
VALUES
('S001', N'Ministop Nguyễn Trãi', N'123 Nguyễn Trãi, Quận 5, TP.HCM', '0909000111', 0, N'System', GETDATE()),
('S002', N'Ministop Lê Lợi', N'45 Lê Lợi, Quận 1, TP.HCM', '0909333444', 0, N'System', GETDATE()),
('S004', N'Ministop Lê Lợi 123', N'45 Lê Lợi, Quận 14, TP.HCM', '0909333442', 0, N'System', GETDATE()),
('S005s', N'Ministop quan 1', N'23 Lê Lợi, Quận 1, TP.HCM', '090933341', 0, N'System', GETDATE()),
('S003', N'Ministop Phan Đăng Lưu', N'98 Phan Đăng Lưu, Phú Nhuận, TP.HCM', '0911555666', 0, N'System', GETDATE());
GO

-- Dữ liệu mẫu cho bảng Employee (đã chỉnh đúng quy tắc)
INSERT INTO Employee (
    EmployeeID, StoreID, FullName, Gender, BirthDate, Phone, Position, 
    EmploymentType, PasswordHash, IsDeleted, CreatedBy, Created, LastModifiedBy, LastModified
)
VALUES
-- ADMIN (Fulltime)
('E001', 'S001', N'Nguyễn Văn Minh', 1, '1990-03-12', '0905123456', N'Admin', N'Fulltime', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 0, N'System', GETDATE(), NULL, NULL),

-- QUẢN LÝ CỬA HÀNG (Fulltime)
('E002', 'S001', N'Trần Thị Lan', 0, '1995-06-25', '0908789654', N'Quản lý cửa hàng', N'Fulltime', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 0, N'System', GETDATE(), NULL, NULL),
('E003', 'S002', N'Phạm Thị Hồng', 0, '1992-01-18', '0912456789', N'Quản lý cửa hàng', N'Fulltime', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 0, N'System', GETDATE(), NULL, NULL),
('E004', 'S003', N'Nguyễn Hoàng Tuấn', 1, '1994-02-15', '0945566778', N'Quản lý cửa hàng', N'Fulltime', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 0, N'System', GETDATE(), NULL, NULL),

-- NHÂN VIÊN (Fulltime và Parttime)
('E005', 'S001', N'Lê Quốc Huy', 1, '1998-11-10', '0903654123', N'Nhân viên', N'Fulltime', 'jZae727K08KaOmKSgOaGzww/XVqGr/PKEgIMkjrcbJI=', 0, N'System', GETDATE(), NULL, NULL),
('E006', 'S002', N'Đỗ Thành Nam', 1, '2000-04-22', '0913344556', N'Nhân viên', N'Parttime', 'hashdef', 0, N'System', GETDATE(), NULL, NULL),
('E007', 'S002', N'Võ Thị Mai', 0, '1997-07-09', '0932223344', N'Nhân viên', N'Fulltime', 'hashghi', 0, N'System', GETDATE(), NULL, NULL),
('E008', 'S003', N'Bùi Thị Ngọc', 0, '2001-09-03', '0978899001', N'Nhân viên', N'Parttime', 'hashmno', 0, N'System', GETDATE(), NULL, NULL),
('E009', 'S003', N'Trịnh Văn Hải', 1, '1999-12-28', '0932233445', N'Nhân viên', N'Fulltime', 'hashpqr', 0, N'System', GETDATE(), NULL, NULL),
('E010', 'S001', N'Phan Thị Yến', 0, '1996-08-05', '0909777666', N'Nhân viên', N'Parttime', 'hashstu', 0, N'System', GETDATE(), NULL, NULL);
GO
INSERT INTO Allowances 
(AllowanceID, AllowanceName, DefaultAmount, IsDeleted, CreatedBy, Created, LastModifiedBy, LastModified)
VALUES
(N'PK001', N'Tiền cơm trưa', 35000, 0, N'admin', GETDATE(), NULL, NULL),
(N'PK002', N'Tiền đi lại', 50000, 0, N'admin', GETDATE(), NULL, NULL),
(N'PK003', N'Tiền gửi xe', 15000, 0, N'admin', GETDATE(), NULL, NULL),
(N'PK004', N'Tiền điện thoại', 100000, 0, N'admin', GETDATE(), NULL, NULL),
(N'PK005', N'Tiền chuyên cần', 200000, 0, N'admin', GETDATE(), NULL, NULL),
(N'PK006', N'Tiền nhà ở', 300000, 0, N'admin', GETDATE(), NULL, NULL),
(N'PK007', N'Tiền đồng phục', 100000, 0, N'admin', GETDATE(), NULL, NULL),
(N'PK008', N'Tiền thưởng lễ tết', 500000, 0, N'admin', GETDATE(), NULL, NULL),
(N'PK009', N'Tiền phụ cấp xăng xe', 120000, 0, N'admin', GETDATE(), NULL, NULL),
(N'PK010', N'Tiền hỗ trợ ca đêm', 80000, 0, N'admin', GETDATE(), NULL, NULL);
