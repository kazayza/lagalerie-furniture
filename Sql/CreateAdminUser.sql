-- ============================================================
-- سكريبت إنشاء مستخدم Admin افتراضي بكلمة مرور مشفّرة BCrypt
-- ============================================================
-- كلمة المرور الافتراضية: Admin@123
-- ⚠️  مهم جداً: غيّر كلمة المرور فوراً بعد أول تسجيل دخول
--
-- الـ hash التالي مولّد بـ BCrypt (work factor = 12) للباسورد: Admin@123
-- لو احتجت hash مختلف، استخدم أداة: dotnet run -- migrate-passwords
-- ============================================================

USE [db57253];  -- استبدل باسم قاعدة البيانات الفعلي لو اختلف
GO

-- التأكد من وجود دور Admin، وإن لم يوجد ننشئه
IF NOT EXISTS (SELECT 1 FROM [dbo].[Roles] WHERE Name = 'Admin')
BEGIN
    INSERT INTO [dbo].[Roles] (Name, Description, IsActive, CreatedAt)
    VALUES ('Admin', N'مدير النظام - صلاحيات كاملة', 1, GETUTCDATE());
END
GO

-- إنشاء/تحديث مستخدم admin بكلمة مرور BCrypt مشفّرة
IF NOT EXISTS (SELECT 1 FROM [dbo].[Users] WHERE Username = 'admin')
BEGIN
    INSERT INTO [dbo].[Users]
        (Username, Email, PasswordHash, FirstName, LastName, DisplayName,
         RoleId, IsActive, IsDeleted, MustChangePassword, FailedLoginAttempts,
         CreatedAt)
    SELECT
        'admin',
        'admin@lagalerie.local',
        '$2a$12$71S2txPGHPGGSSJ8BuAUwuYO1p9LpQ7d5uUHhA3ey2Vi3RkC4Bdva',  -- Admin@123 (BCrypt)
        N'مدير', N'النظام', N'مدير النظام',
        (SELECT Id FROM [dbo].[Roles] WHERE Name = 'Admin'),
        1, 0, 1, 0,  -- IsActive=1, IsDeleted=0, MustChangePassword=1 (إجبار تغيير الباسورد)
        GETUTCDATE();

    PRINT N'✅ تم إنشاء مستخدم admin بنجاح. كلمة المرور: Admin@123';
END
ELSE
BEGIN
    -- تحديث باسورد admin الموجود لو كان نصاً صريحاً
    UPDATE [dbo].[Users]
    SET PasswordHash = '$2a$12$71S2txPGHPGGSSJ8BuAUwuYO1p9LpQ7d5uUHhA3ey2Vi3RkC4Bdva',
        MustChangePassword = 1
    WHERE Username = 'admin'
      AND PasswordHash NOT LIKE '$2a$%';

    PRINT N'ℹ️ تم تحديث باسورد admin الموجود.';
END
GO

-- ============================================================
-- (اختياري) ترقية كل الباسوردات النصية المتبقية لـ BCrypt
-- ملاحظة: هذا السكريبت يعيّن نفس الـ hash (Admin@123) لكل من له باسورد نصي
--         مجرد وسيلة طوارئ. الأفضل استخدام أداة .NET:
--           dotnet run -- migrate-passwords
--         فهي تحافظ على كل باسورد كما هو (بمجرد تشفيره).
-- ============================================================
PRINT N'---';
PRINT N'لترقية باسوردات المستخدمين الحالية دون تغييرها، استخدم:';
PRINT N'  dotnet run -- migrate-passwords';
GO
