# 📋 خطة تطوير نظام ERP - Lagalerie Furniture

> **آخر تحديث:** 2026-06-27
> **الحالة الحالية:** مرحلة البداية (الأساس جاهز — تسجيل دخول + داشبورد)
> **التقنيات:** Blazor Server (.NET 8) + MudBlazor 9.5 + EF Core 8 + SQL Server

---

## 1️⃣ المراجعة الحالية للمشروع (Status Assessment)

### ✅ ما تم إنجازه (شغّال)

| المجال | الحالة | ملاحظات |
|--------|--------|---------|
| هيكل المشروع | ✅ جيد | مشروع Blazor Server واحد، بنية منظّمة (`Components`, `Models`, `Data`, `Services`) |
| الـ Build | ✅ ناجح | `dotnet build` = 0 errors, 0 warnings |
| قاعدة البيانات | ✅ جاهزة | SQL schema كامل في `ERP_Furniture_Database.sql` (~85 جدول)، موجود على سيرفر `db57253.public.databaseasp.net` |
| النماذج (Models) | ✅ ~85 موديل | تغطي: مبيعات، مخزون، إنتاج، مالية، HR، CRM، أصول ثابتة |
| DbContext | ✅ مكتمل | `ApplicationDbContext.cs` (~2381 سطر) كل العلاقات والـ Indexes مكتوبة |
| المصادقة (Auth) | ⚠️ أساسي | `Login` + `Logout` + `CustomAuthStateProvider` — شغّال لكن **بدون تشفير كلمات المرور** |
| الـ Layout | ✅ ممتاز | `MainLayout` (Dark Glassmorphism + Gold) + `EmptyLayout` للـ login |
| الـ Dashboard | ⚠️ وهمي | `Home.razor` فيه **بيانات ثابتة (hardcoded)**، الأرقام مش من الـ DB |

### 🔴 المشاكل الحرجّة (Critical Issues)

#### 1. **كلمات المرور بدون تشفير** 🔥
- **الموقع:** `Services/AuthService.cs:58`
- **المشكلة:** `if (user.PasswordHash != password)` — مقارنة نص صريح! عمود `PasswordHash` بيخزّن الباسورد كما هو.
- **الخطر:** لو اتسرقت الـ DB، كل الباسوردات مكشوفة. **مرفوض تماماً في إنتاج.**
- **الحل:** استخدام `BCrypt.Net-Next` لـ hashing + salt.

#### 2. **بلا EF Migrations**
- **المشكلة:** الـ schema اتضبط يدوياً بـ SQL script، مفيش مجلد `Migrations/`.
- **الأثر:** أي تعديل على الـ Models مش هيطبّق على الـ DB تلقائياً.
- **الحل:** إمّا ننشئ migration أولي (snapshot) أو نعتمد SQL scripts منفصلة.

#### 3. **بيانات الـ Dashboard ثابتة (Hardcoded)**
- **الموقع:** `Components/Pages/Home.razor`
- **المشكلة:** الأرقام (24,580 ج.م — 1,248 منتج...) مكتوبة يدوياً.
- **الحل:** إنشاء `DashboardService` يجيب البيانات الحقيقية من الـ DB.

### 🟡 مشاكل متوسطة (Medium Issues)

#### 4. **الـ Roles/Permissions غير فعّالة فعلياً**
- الـ MainLayout بيعرض Menu كامل للكل، مفيش `[Authorize(Roles=...)]` على الـ routes، ومفيش فلترة Menu حسب الصلاحيات.

#### 5. **مفيش مرجع للـ Roles/Permissions في الـ UI**
- النظام فيه جداول `Roles`, `Permissions`, `UserPermissions` لكن مفيش صفحة إدارة ليها.

#### 6. **نقص Dependency Injection للـ Services**
- فيه `AuthService` و `ThemeService` بس. لازم Service لكل module (مبيعات، مخزون...).
- أفضل: استخدام **Repository Pattern** أو **Generic Service** عشان نتجنب تكرار الكود.

#### 7. **عدم وجود Migrations**
- مفيش تتبّع لتغييرات الـ DB.

#### 8. **اتصال قاعدة البيانات بسيط**
- `EnableRetryOnFailure` موجود ✅، لكن مفيش logging للاستعلامات البطيئة ولا connection resiliency كاملة.

### 🟢 ملاحظات معمارية (Architectural Notes)

- **`CustomAuthStateProvider` بسيط:** بيخزّن المستخدم في memory فقط — بمجرد عمل Refresh للصفحة (F5) في Blazor Server بيفقد الحالة؟ **لا**، لكن بمجرد إعادة تشغيل السيرفر كل الجلسات بتضيع (مفيش cookie/token persistence). **ينبغي إضافة Cookie Auth.**
- **`Program.cs` كويس** لكن مفيش `app.UseAuthentication()` — الـ Auth كله client-side في الـ circuit، ده غير آمن للـ API endpoints لو اتضافوا.
- **مفيش Globalization/Localization** إعدادات (رغم أن المحتوى عربي). ينصح بضبط `RequestLocalizationOptions` للعربية.

---

## 2️⃣ الخريطة العامة للوحدات (Module Map)

الـ database فيها الوحدات دي. كل وحدة محتاجة صفحات (List + Create/Edit + Details):

| # | الوحدة | الجداول الأساسية | الأولوية |
|---|--------|------------------|----------|
| 0 | 🔐 الأمان والمستخدمين | `Users`, `Roles`, `Permissions`, `UserPermissions`, `UserEntityPermissions` | **عالية جدًا** |
| 1 | 🛒 المبيعات | `Invoices`, `InvoiceItems`, `Quotations`, `Customers`, `Payments`, `Installments` | **عالية** |
| 2 | 📦 المخزون | `Products`, `ProductVariants`, `Categories`, `Inventory`, `Warehouses`, `GoodsReceipts`, `StockCounts` | **عالية** |
| 3 | 🏭 الإنتاج | `ProductionOrders`, `ProductionStages`, `BillOfMaterials`, `MaterialConsumption` | متوسطة |
| 4 | 💰 المالية | `JournalEntries`, `ChartOfAccounts`, `CashRegisters`, `Budgets`, `FixedAssets` | متوسطة |
| 5 | 👥 الموظفين/HR | `Employees`, `Attendance`, `Payrolls`, `LeaveRequests`, `EmployeeAdvances` | متوسطة |
| 6 | 📞 CRM/الموردين | `Suppliers`, `Complaints`, `LeadSources`, `PurchaseOrders` | منخفضة |
| 7 | 📊 التقارير | الـ Views (`VwIncomeStatement`, `VwBalanceSheet`, `VwMonthlySale`...) | منخفضة |
| 8 | ⚙️ الإعدادات | `Branches`, `Departments`, `SystemSettings`, `CostCenters` | منخفضة |

---

## 3️⃣ ترتيب التنفيذ المقترح (Roadmap)

> المبدأ: **أمّن الأساس الأول، ثم ابنِ وحدة كاملة (Sales) كنموذج، ثم كرّر.**

### 🚀 المرحلة 1: تأمين الأساس (الأسبوع 1) — **الأولوية القصوى**
> **الهدف:** نظام أمان احترافي يصلح للإنتاج قبل أي حاجة تانية.

- [ ] **1.1** تثبيت `BCrypt.Net-Next` وتشفير كلمات المرور (hash + salt + verify)
- [ ] **1.2** إضافة **Cookie Authentication** حقيقي عبر `app.UseAuthentication()` + `HttpContext.SignInAsync`
- [ ] **1.3** ربط `CustomAuthStateProvider` بالـ cookie (عشان الجلسة تعيش بعد الـ refresh وإعادة التشغيل)
- [ ] **1.4** إنشاء `AuthorizeView` / `[Authorize]` على الصفحات المحمية
- [ ] **1.5** فلترة الـ Navigation Menu حسب صلاحيات المستخدم (Role + Permissions)
- [ ] **1.6** Hashing لكل الباسوردات الموجودة في الـ DB (سكريبت ترحيل)
- [ ] **1.7** صفحة تغيير كلمة المرور + إجبار `MustChangePassword`

### 🏗️ المرحلة 2: البنية التحتية للكود (الأسبوع 1-2)
> **الهدف:** أساس نظيف عشان باقي الوحدات تتبنى عليه بسرعة.

- [ ] **2.1** إنشاء **Generic Repository** (`IRepository<T>`) أو **Base Service** عشان نتجنب تكرار الكود
- [ ] **2.2** إنشاء `DashboardService` يجيب بيانات حقيقية من الـ DB
- [ ] **2.3** ربط الـ `Home.razor` بالبيانات الحقيقية + إضافة **ApexCharts** (مثبّت بالفعل) للرسوم البيانية
- [ ] **2.4** إنشاء **Reusable Components**:
  - `DataGrid` wrapper مخصّص بالـ RTL والعربي
  - `ConfirmDialog` للحذف
  - `StatusBadge` للحالات (pending/approved/...)
  - `PageHeader` موحّد للصفحات
- [ ] **2.5** إنشاء أول EF Migration snapshot من الـ DB الحالي
- [ ] **2.6** ضبط `RequestLocalization` (عربي + RTL) وضبط `decimal` culture

### 🧩 المرحلة 3: وحدة المبيعات (الأسبوع 2-4) — **أول وحدة كاملة**
> **الهدف:** وحدة كاملة من الـ DB للـ UI تكون نموذج لباقي الوحدات.

- [ ] **3.1** صفحات **العملاء** (`/customers`): List (مع بحث/فلترة) → Create → Edit → Details → Soft-delete
- [ ] **3.2** صفحات **الفواتير** (`/invoices`): List + إنشاء فاتورة (مع سطور InvoiceItems + حساب ضريبة وخصم)
- [ ] **3.3** صفحات **التسعير/عروض الأسعار** (`/quotations`)
- [ ] **3.4** صفحات **المدفوعات والأقساط** (`/payments`)
- [ ] **3.5** طباعة الفاتورة (PDF/Print view)

### 📦 المرحلة 4: وحدة المخزون (الأسبوع 4-6)
- [ ] **4.1** المنتجات + المتغيرات (Variants) + الأصناف (Categories)
- [ ] **4.2** المخازن + الأماكن (Warehouse Locations)
- [ ] **4.3** كارت الصنف (Inventory) + حركات المخزون (Transactions)
- [ ] **4.4** أذون الاستلام (Goods Receipt) + التحويلات (Transfers)
- [ ] **4.5** الجرد (Stock Count) + تنبيهات الحد الأدنى

### 🏭 المرحلة 5: وحدة الإنتاج (الأسبوع 6-8)
- [ ] **5.1** شجرة المواد (BOM)
- [ ] **5.2** أوامر الإنتاج + مراحلها
- [ ] **5.3** استهلاك المواد + حساب التكلفة

### 💰 المرحلة 6: المالية + التقارير (الأسبوع 8-10)
- [ ] **6.1** القيود اليومية (Journal Entries) + التحقق من التوازن (Debit = Credit)
- [ ] **6.2** دليل الحسابات (Chart of Accounts)
- [ ] **6.3** الصناديق (Cash Registers) + التسوية اليومية
- [ ] **6.4** التقارير: قائمة الدخل، الميزانية، المبيعات الشهرية

### 👥 المرحلة 7: HR + CRM + الإعدادات (الأسبوع 10+)
- [ ] **7.1** الموظفين + الحضور + الرواتب
- [ ] **7.2** الموردين + أوامر الشراء
- [ ] **7.3** الشكاوى + مصادر العملاء
- [ ] **7.4** إدارة الفروع والأقسام والإعدادات العامة

---

## 4️⃣ معايير الجودة (Definition of Done لكل صفحة/وحدة)

كل صفحة جديدة لازم تحقّق:

- [ ] **Search + Filter + Pagination** على كل الـ Lists (استخدام `MudTable` أو مخصّص)
- [ ] **Validation** على الـ Forms (Required, Range, Custom)
- [ ] **Soft-delete** (بلا حذف فعلي — فيه `IsDeleted` في معظم الجداول) ✅ موجود
- [ ] **Audit trail** (تسجيل `ActivityLog` لكل عملية Create/Update/Delete)
- [ ] **Arabic RTL** صحيح + تنسيق موحّد مع الـ theme
- [ ] **Authorization** على الصفحة (Role/Permission)
- [ ] **Error handling** + Snackbar messages للمستخدم
- [ ] **Loading states** (skeleton/spinner)

---

## 5️⃣ قرارات معمارية مقترحة (Architectural Decisions)

### أ) نمط الوصول للبيانات (Data Access Pattern)

نقترح **3 خيارات** — يُفضّل الاتفاق على واحد قبل البدء:

| الخيار | الوصف | الميزة | العيب |
|-------|-------|--------|-------|
| **A: Generic Repository** | `IRepository<T>` واحد لكل الكيانات | أقل كود، مرونة | بيسبب "fat" interface للمتطلبات المعقّدة |
| **B: Service-per-Module** | `SalesService`, `InventoryService`... | منطق أعمال منفصل | كود أكتر |
| **C: Hybrid (موصى به)** | Generic Repo للـ CRUD الأساسي + Services للمنطق المعقّد | أفضل توازن | تنظيم دقيق |

### ب) المصادقة (Authentication)

الأن الموجود "in-memory circuit auth" غير آمن للإنتاج. الخيارات:
- **Cookie Auth (موصى به):** Standard ASP.NET Core cookie، يعيش بعد الـ refresh
- JWT: لو فيه API منفصل (مش حالياً)

### ج) Localization

الأن النصوص العربية مكتوبة inline. لو فيه خطة للإنجليزية مستقبلاً، نستخدم `.resx` files دلوقتي.

---

## 6️⃣ المخاطر والتنبيهات (Risks)

| الخطر | الأثر | التخفيف |
|------|------|---------|
| 🔴 كلمات المرور غير مشفّرة | **حرج** — تسرّب كل الباسوردات | المرحلة 1.1 فوراً |
| 🟡 قاعدة بيانات remote عامة | بطء شبكة + تكلفة | اعتبار local DB للـ dev + caching |
| 🟡 مفيش backups للمشروع (مش git repo) | ضياع الشغل | **تهيئة Git فوراً** |
| 🟡 بلا EF migrations | صعوبة تتبّع الـ schema | المرحلة 2.5 |
| 🟢 apexcharts مثبّت لكن مش مستخدم | موارد بلا فائدة | استخدامه في الـ dashboard |

---

## 7️⃣ الخطوة التالية الفورية (Immediate Next Step)

> **نبدأ بالمرحلة 1 (الأمان) لأنها بوابة كل حاجة بعدها.**

### الترتيب الموصى به لأول جلسة عمل:
1. تهيئة **Git** للمشروع (commit snapshot آمن)
2. تثبيت `BCrypt.Net-Next` + كتابة `PasswordHasher` service
3. تحويل `AuthService` لاستخدام `BCrypt.Verify`
4. سكريبت SQL لتحديث باسوردات الـ DB الموجودة

---

## 8️⃣ ملاحظات وقرارات مفتوحة (تُناقش قبل البدء)

1. **هل نكمل بنفس قاعدة البيانات الموجودة** (`db57253.public.databaseasp.net`) أم ننتقل لـ local SQL Server للتطوير؟
2. **هل نعتمد SQL scripts** للـ migrations أم نولّد EF migrations؟
3. **نمط الوصول للبيانات:** Generic Repo vs Service-per-Module vs Hybrid؟
4. **هل نحتاج اللغة الإنجليزية** مستقبلاً؟ (يأثر على القرار الآن)
5. **هل الـ Roles محدّدة مسبقاً** (مثل Admin/Manager/User) أم dynamic من الـ DB؟
6. **هل نريد Activity Log / Audit trail** فعّال من البداية؟

---

> 📌 **هذا الملف حيّ — يتم تحديثه بعد كل مرحلة.** ضع علامة ✅ على المهام المكتملة.
