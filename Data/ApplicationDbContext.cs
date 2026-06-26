using System;
using System.Collections.Generic;
using LagalerieFurniture.Models;
using Microsoft.EntityFrameworkCore;
using Attribute = LagalerieFurniture.Models.ProductAttribute;

namespace LagalerieFurniture.Data;

public partial class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ActivityLog> ActivityLogs { get; set; }

    public virtual DbSet<AdvanceInstallment> AdvanceInstallments { get; set; }

    public virtual DbSet<AssetDepreciation> AssetDepreciations { get; set; }

    public virtual DbSet<AssetTransaction> AssetTransactions { get; set; }

    public virtual DbSet<Attendance> Attendances { get; set; }

    public virtual DbSet<Attribute> Attributes { get; set; }

    public virtual DbSet<AttributeValue> AttributeValues { get; set; }

    public virtual DbSet<BankDeposit> BankDeposits { get; set; }

    public virtual DbSet<BillOfMaterial> BillOfMaterials { get; set; }

    public virtual DbSet<BiometricDevice> BiometricDevices { get; set; }

    public virtual DbSet<Bomitem> Bomitems { get; set; }

    public virtual DbSet<Branch> Branches { get; set; }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<CashRegister> CashRegisters { get; set; }

    public virtual DbSet<CashRegisterDailySettlement> CashRegisterDailySettlements { get; set; }

    public virtual DbSet<CashRegisterTransaction> CashRegisterTransactions { get; set; }

    public virtual DbSet<CashTransfer> CashTransfers { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<ChartOfAccount> ChartOfAccounts { get; set; }

    public virtual DbSet<Complaint> Complaints { get; set; }

    public virtual DbSet<ComplaintActivity> ComplaintActivities { get; set; }

    public virtual DbSet<CostCenter> CostCenters { get; set; }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<DataImportsExport> DataImportsExports { get; set; }

    public virtual DbSet<Department> Departments { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<EmployeeAdvance> EmployeeAdvances { get; set; }

    public virtual DbSet<EmployeeShift> EmployeeShifts { get; set; }

    public virtual DbSet<FiscalPeriod> FiscalPeriods { get; set; }

    public virtual DbSet<FixedAsset> FixedAssets { get; set; }

    public virtual DbSet<GoodsReceipt> GoodsReceipts { get; set; }

    public virtual DbSet<GoodsReceiptItem> GoodsReceiptItems { get; set; }

    public virtual DbSet<ImportError> ImportErrors { get; set; }

    public virtual DbSet<Installment> Installments { get; set; }

    public virtual DbSet<InstallmentDetail> InstallmentDetails { get; set; }

    public virtual DbSet<Inventory> Inventories { get; set; }

    public virtual DbSet<InventoryTransaction> InventoryTransactions { get; set; }

    public virtual DbSet<InventoryTransfer> InventoryTransfers { get; set; }

    public virtual DbSet<InventoryTransferItem> InventoryTransferItems { get; set; }

    public virtual DbSet<Invoice> Invoices { get; set; }

    public virtual DbSet<InvoiceItem> InvoiceItems { get; set; }

    public virtual DbSet<JournalEntry> JournalEntries { get; set; }

    public virtual DbSet<JournalEntryLine> JournalEntryLines { get; set; }

    public virtual DbSet<LeadSource> LeadSources { get; set; }

    public virtual DbSet<LeaveBalance> LeaveBalances { get; set; }

    public virtual DbSet<LeaveRequest> LeaveRequests { get; set; }

    public virtual DbSet<MaterialConsumption> MaterialConsumptions { get; set; }

    public virtual DbSet<Module> Modules { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Payroll> Payrolls { get; set; }

    public virtual DbSet<Permission> Permissions { get; set; }

    public virtual DbSet<PermissionAuditLog> PermissionAuditLogs { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductBundleComponent> ProductBundleComponents { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<ProductVariant> ProductVariants { get; set; }

    public virtual DbSet<ProductVariantAttribute> ProductVariantAttributes { get; set; }

    public virtual DbSet<ProductionOrder> ProductionOrders { get; set; }

    public virtual DbSet<ProductionStage> ProductionStages { get; set; }

    public virtual DbSet<PurchaseOrder> PurchaseOrders { get; set; }

    public virtual DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }

    public virtual DbSet<Quotation> Quotations { get; set; }

    public virtual DbSet<QuotationItem> QuotationItems { get; set; }

    public virtual DbSet<RawAttendanceLog> RawAttendanceLogs { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<RolePermission> RolePermissions { get; set; }

    public virtual DbSet<ShiftSchedule> ShiftSchedules { get; set; }

    public virtual DbSet<StockCount> StockCounts { get; set; }

    public virtual DbSet<StockCountAdjustment> StockCountAdjustments { get; set; }

    public virtual DbSet<StockCountItem> StockCountItems { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }

    public virtual DbSet<SystemSetting> SystemSettings { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserEntityPermission> UserEntityPermissions { get; set; }

    public virtual DbSet<UserPermission> UserPermissions { get; set; }

    public virtual DbSet<VwBalanceSheet> VwBalanceSheets { get; set; }

    public virtual DbSet<VwCashRegisterStatement> VwCashRegisterStatements { get; set; }

    public virtual DbSet<VwDailyAttendance> VwDailyAttendances { get; set; }

    public virtual DbSet<VwDueInstallment> VwDueInstallments { get; set; }

    public virtual DbSet<VwEmployeeAdvance> VwEmployeeAdvances { get; set; }

    public virtual DbSet<VwIncomeStatement> VwIncomeStatements { get; set; }

    public virtual DbSet<VwInventoryStatus> VwInventoryStatuses { get; set; }

    public virtual DbSet<VwMonthlySale> VwMonthlySales { get; set; }

    public virtual DbSet<Warehouse> Warehouses { get; set; }

    public virtual DbSet<WarehouseLocation> WarehouseLocations { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Arabic_CI_AI");

        modelBuilder.Entity<ActivityLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Activity__3214EC0728028A7B");

            entity.HasIndex(e => new { e.EntityType, e.EntityId }, "IX_ActivityLogs_Entity");

            entity.HasIndex(e => new { e.UserId, e.CreatedAt }, "IX_ActivityLogs_UserDate").IsDescending(false, true);

            entity.Property(e => e.ActivityType).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EntityType).HasMaxLength(100);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
            entity.Property(e => e.UserAgent).HasMaxLength(500);

            entity.HasOne(d => d.User).WithMany(p => p.ActivityLogs)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("FK__ActivityL__UserI__47C69FAC");
        });

        modelBuilder.Entity<AdvanceInstallment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AdvanceI__3214EC07CD4E2C94");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DeductedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("pending");

            entity.HasOne(d => d.Advance).WithMany(p => p.AdvanceInstallments)
                .HasForeignKey(d => d.AdvanceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AdvanceIn__Advan__282DF8C2");
        });

        modelBuilder.Entity<AssetDepreciation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AssetDep__3214EC071002C94E");

            entity.Property(e => e.AccumulatedValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DepreciationAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetDepreciations)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetDepr__Asset__30E33A54");

            entity.HasOne(d => d.JournalEntry).WithMany(p => p.AssetDepreciations)
                .HasForeignKey(d => d.JournalEntryId)
                .HasConstraintName("FK__AssetDepr__Journ__31D75E8D");
        });

        modelBuilder.Entity<AssetTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__AssetTra__3214EC076382DDE7");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.TransactionDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.TransactionType).HasMaxLength(50);

            entity.HasOne(d => d.Asset).WithMany(p => p.AssetTransactions)
                .HasForeignKey(d => d.AssetId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Asset__35A7EF71");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.AssetTransactions)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AssetTran__Creat__38845C1C");

            entity.HasOne(d => d.JournalEntry).WithMany(p => p.AssetTransactions)
                .HasForeignKey(d => d.JournalEntryId)
                .HasConstraintName("FK__AssetTran__Journ__379037E3");
        });

        modelBuilder.Entity<Attendance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attendan__3214EC07671E7D4D");

            entity.ToTable("Attendance");

            entity.HasIndex(e => new { e.EmployeeId, e.Date }, "IX_Attendance_EmployeeDate");

            entity.HasIndex(e => e.Status, "IX_Attendance_Status");

            entity.HasIndex(e => new { e.EmployeeId, e.Date }, "UQ_Attendance").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.OvertimeHours).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(20)
                .HasDefaultValue("present");
            entity.Property(e => e.WorkingHours).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Employee).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attendanc__Emplo__3C89F72A");

            entity.HasOne(d => d.ShiftSchedule).WithMany(p => p.Attendances)
                .HasForeignKey(d => d.ShiftScheduleId)
                .HasConstraintName("FK__Attendanc__Shift__4336F4B9");
        });

        modelBuilder.Entity<LagalerieFurniture.Models.ProductAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attribut__3214EC072C3EF8CC");

            entity.HasIndex(e => e.Name, "UQ__Attribut__737584F6B2C2B433").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasDefaultValue("text");
        });

        modelBuilder.Entity<AttributeValue>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Attribut__3214EC0741FEE6AA");

            entity.HasIndex(e => new { e.AttributeId, e.Value }, "UQ_AttrValue").IsUnique();

            entity.Property(e => e.ColorCode).HasMaxLength(20);
            entity.Property(e => e.Value).HasMaxLength(200);

            entity.HasOne(d => d.Attribute).WithMany(p => p.AttributeValues)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Attribute__Attri__3E1D39E1");
        });

        modelBuilder.Entity<BankDeposit>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BankDepo__3214EC07E1C2AD34");

            entity.HasIndex(e => e.DepositNumber, "UQ__BankDepo__7BBA5C747A21B609").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DepositDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DepositNumber).HasMaxLength(50);
            entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("pending");

            entity.HasOne(d => d.CashRegister).WithMany(p => p.BankDeposits)
                .HasForeignKey(d => d.CashRegisterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BankDepos__CashR__2CBDA3B5");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.BankDeposits)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BankDepos__Creat__2F9A1060");
        });

        modelBuilder.Entity<BillOfMaterial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BillOfMa__3214EC07577ED1FF");

            entity.HasIndex(e => e.ProductId, "UQ_BOM_Product").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EstimatedCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EstimatedHours).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(300);

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.BillOfMaterials)
                .HasForeignKey(d => d.CreatedById)
                .HasConstraintName("FK__BillOfMat__Creat__78D3EB5B");

            entity.HasOne(d => d.Product).WithOne(p => p.BillOfMaterial)
                .HasForeignKey<BillOfMaterial>(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BillOfMat__Produ__75035A77");
        });

        modelBuilder.Entity<BiometricDevice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Biometri__3214EC0763FFB710");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DeviceIp).HasMaxLength(45);
            entity.Property(e => e.DeviceName).HasMaxLength(100);
            entity.Property(e => e.DevicePort).HasDefaultValue(4370);
            entity.Property(e => e.IsActive).HasDefaultValue(true);

            entity.HasOne(d => d.Branch).WithMany(p => p.BiometricDevices)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Biometric__Branc__269AB60B");
        });

        modelBuilder.Entity<Bomitem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__BOMItems__3214EC0758B21138");

            entity.ToTable("BOMItems");

            entity.Property(e => e.Bomid).HasColumnName("BOMId");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitMeasure).HasMaxLength(20);
            entity.Property(e => e.WastagePercent).HasColumnType("decimal(5, 2)");

            entity.HasOne(d => d.Bom).WithMany(p => p.Bomitems)
                .HasForeignKey(d => d.Bomid)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BOMItems__BOMId__7CA47C3F");

            entity.HasOne(d => d.MaterialProduct).WithMany(p => p.Bomitems)
                .HasForeignKey(d => d.MaterialProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__BOMItems__Materi__7D98A078");
        });

        modelBuilder.Entity<Branch>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Branches__3214EC07AD307A66");

            entity.HasIndex(e => e.Code, "UQ__Branches__A25C5AA711B91DEF").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.NameEn).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Phone2).HasMaxLength(20);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.Manager).WithMany(p => p.Branches)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Branches_Manager");
        });

        modelBuilder.Entity<Budget>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Budgets__3214EC07015CCE8F");

            entity.HasIndex(e => new { e.BudgetYear, e.AccountId, e.BranchId }, "UQ_Budget").IsUnique();

            entity.Property(e => e.ActualAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.PlannedAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("draft");
            entity.Property(e => e.Variance).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Account).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Budgets__Account__1DD065E0");

            entity.HasOne(d => d.Branch).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK__Budgets__BranchI__1EC48A19");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.Budgets)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Budgets__Created__22951AFD");
        });

        modelBuilder.Entity<CashRegister>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CashRegi__3214EC0798087B15");

            entity.HasIndex(e => e.Code, "UQ__CashRegi__A25C5AA7D5FC6120").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("EGP");
            entity.Property(e => e.CurrentBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.MaximumBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.MinimumBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.OpeningBalance).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Branch).WithMany(p => p.CashRegisters)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CashRegis__Branc__11158940");

            entity.HasOne(d => d.Manager).WithMany(p => p.CashRegisters)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__CashRegis__Manag__18B6AB08");
        });

        modelBuilder.Entity<CashRegisterDailySettlement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CashRegi__3214EC07F2E2DB6C");

            entity.ToTable("CashRegisterDailySettlement");

            entity.HasIndex(e => new { e.CashRegisterId, e.SettlementDate }, "UQ_RegisterSettlement").IsUnique();

            entity.Property(e => e.ActualBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ClosingBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Difference).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ExpectedBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OpeningBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("draft");
            entity.Property(e => e.TotalInflow).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalOutflow).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CashRegister).WithMany(p => p.CashRegisterDailySettlements)
                .HasForeignKey(d => d.CashRegisterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CashRegis__CashR__345EC57D");

            entity.HasOne(d => d.SettledBy).WithMany(p => p.CashRegisterDailySettlements)
                .HasForeignKey(d => d.SettledById)
                .HasConstraintName("FK__CashRegis__Settl__382F5661");
        });

        modelBuilder.Entity<CashRegisterTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CashRegi__3214EC07E8CE4E26");

            entity.HasIndex(e => e.TransactionNumber, "UQ__CashRegi__E733A2BF42AB8801").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Direction).HasMaxLength(10);
            entity.Property(e => e.NewBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PreviousBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReferenceType).HasMaxLength(50);
            entity.Property(e => e.TransactionNumber).HasMaxLength(50);
            entity.Property(e => e.TransactionType).HasMaxLength(20);

            entity.HasOne(d => d.CashRegister).WithMany(p => p.CashRegisterTransactions)
                .HasForeignKey(d => d.CashRegisterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CashRegis__CashR__1D7B6025");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.CashRegisterTransactions)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CashRegis__Creat__1E6F845E");
        });

        modelBuilder.Entity<CashTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CashTran__3214EC076AA7C502");

            entity.HasIndex(e => e.TransferNumber, "UQ__CashTran__02A4FA0D939449A1").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("pending");
            entity.Property(e => e.TransferDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.TransferNumber).HasMaxLength(50);

            entity.HasOne(d => d.ApprovedBy).WithMany(p => p.CashTransferApprovedBies)
                .HasForeignKey(d => d.ApprovedById)
                .HasConstraintName("FK__CashTrans__Appro__27F8EE98");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.CashTransferCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CashTrans__Creat__2704CA5F");

            entity.HasOne(d => d.FromCashRegister).WithMany(p => p.CashTransferFromCashRegisters)
                .HasForeignKey(d => d.FromCashRegisterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CashTrans__FromC__2334397B");

            entity.HasOne(d => d.ToCashRegister).WithMany(p => p.CashTransferToCashRegisters)
                .HasForeignKey(d => d.ToCashRegisterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__CashTrans__ToCas__24285DB4");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Categori__3214EC0789793145");

            entity.HasIndex(e => e.Slug, "UQ__Categori__BC7B5FB68C3B1F39").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Icon).HasMaxLength(100);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Slug).HasMaxLength(200);

            entity.HasOne(d => d.Branch).WithMany(p => p.Categories)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK__Categorie__Branc__32AB8735");

            entity.HasOne(d => d.ParentCategory).WithMany(p => p.InverseParentCategory)
                .HasForeignKey(d => d.ParentCategoryId)
                .HasConstraintName("FK__Categorie__Paren__2EDAF651");
        });

        modelBuilder.Entity<ChartOfAccount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ChartOfA__3214EC07B0E63531");

            entity.HasIndex(e => e.AccountCode, "UQ__ChartOfA__38D0C56A0C90C8C0").IsUnique();

            entity.Property(e => e.AccountCategory).HasMaxLength(100);
            entity.Property(e => e.AccountCode).HasMaxLength(30);
            entity.Property(e => e.AccountName).HasMaxLength(200);
            entity.Property(e => e.AccountNameEn).HasMaxLength(200);
            entity.Property(e => e.AccountType).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.NormalBalance)
                .HasMaxLength(10)
                .HasDefaultValue("debit");
            entity.Property(e => e.Notes).HasMaxLength(500);

            entity.HasOne(d => d.ParentAccount).WithMany(p => p.InverseParentAccount)
                .HasForeignKey(d => d.ParentAccountId)
                .HasConstraintName("FK__ChartOfAc__Paren__75C27486");
        });

        modelBuilder.Entity<Complaint>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Complain__3214EC0765CCF605");

            entity.HasIndex(e => e.ComplaintNumber, "UQ__Complain__357BC936836DC580").IsUnique();

            entity.Property(e => e.ComplaintNumber).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.OpenedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .HasDefaultValue("medium");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("open");
            entity.Property(e => e.Subject).HasMaxLength(300);
            entity.Property(e => e.Type).HasMaxLength(50);

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.ComplaintAssignedTos)
                .HasForeignKey(d => d.AssignedToId)
                .HasConstraintName("FK__Complaint__Assig__695C9DA1");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.ComplaintCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Complaint__Creat__6B44E613");

            entity.HasOne(d => d.Customer).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Complaint__Custo__6497E884");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK__Complaint__Invoi__658C0CBD");

            entity.HasOne(d => d.Product).WithMany(p => p.Complaints)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Complaint__Produ__668030F6");
        });

        modelBuilder.Entity<ComplaintActivity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Complain__3214EC07BC0F6323");

            entity.Property(e => e.ActivityType).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Complaint).WithMany(p => p.ComplaintActivities)
                .HasForeignKey(d => d.ComplaintId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Complaint__Compl__6F1576F7");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.ComplaintActivities)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Complaint__Creat__70FDBF69");
        });

        modelBuilder.Entity<CostCenter>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__CostCent__3214EC073FF827D0");

            entity.HasIndex(e => e.Code, "UQ__CostCent__A25C5AA70BEA498F").IsUnique();

            entity.Property(e => e.Code).HasMaxLength(30);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);

            entity.HasOne(d => d.Department).WithMany(p => p.CostCenters)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__CostCente__Depar__041093DD");

            entity.HasOne(d => d.ParentCostCenter).WithMany(p => p.InverseParentCostCenter)
                .HasForeignKey(d => d.ParentCostCenterId)
                .HasConstraintName("FK__CostCente__Paren__031C6FA4");
        });

        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Customer__3214EC0789AC1DF0");

            entity.HasIndex(e => e.CustomerCode, "UQ__Customer__06678521E9B25723").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.Area).HasMaxLength(100);
            entity.Property(e => e.BlacklistReason).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CurrentBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerCode).HasMaxLength(50);
            entity.Property(e => e.CustomerType)
                .HasMaxLength(50)
                .HasDefaultValue("retail");
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Phone2).HasMaxLength(20);
            entity.Property(e => e.TotalPurchases).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.CustomerAssignedTos)
                .HasForeignKey(d => d.AssignedToId)
                .HasConstraintName("FK__Customers__Assig__7E02B4CC");

            entity.HasOne(d => d.Branch).WithMany(p => p.Customers)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK__Customers__Branc__01D345B0");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.CustomerCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .HasConstraintName("FK__Customers__Creat__02C769E9");

            entity.HasOne(d => d.LeadSource).WithMany(p => p.Customers)
                .HasForeignKey(d => d.LeadSourceId)
                .HasConstraintName("FK__Customers__LeadS__7755B73D");
        });

        modelBuilder.Entity<DataImportsExport>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DataImpo__3214EC07906C072C");

            entity.Property(e => e.EntityType).HasMaxLength(100);
            entity.Property(e => e.FileFormat).HasMaxLength(20);
            entity.Property(e => e.FileName).HasMaxLength(500);
            entity.Property(e => e.FilePath).HasMaxLength(500);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.OperationType).HasMaxLength(20);
            entity.Property(e => e.StartedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("pending");

            entity.HasOne(d => d.StartedBy).WithMany(p => p.DataImportsExports)
                .HasForeignKey(d => d.StartedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__DataImpor__Start__40257DE4");
        });

        modelBuilder.Entity<Department>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Departme__3214EC07545EE931");

            entity.HasIndex(e => e.Name, "UQ__Departme__737584F652F34346").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Manager).WithMany(p => p.Departments)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK_Departments_Manager");

            entity.HasOne(d => d.ParentDepartment).WithMany(p => p.InverseParentDepartment)
                .HasForeignKey(d => d.ParentDepartmentId)
                .HasConstraintName("FK__Departmen__Paren__52593CB8");
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC0778E43AF1");

            entity.HasIndex(e => e.UserId, "UQ__Employee__1788CC4D1CD1F967").IsUnique();

            entity.HasIndex(e => e.EmployeeCode, "UQ__Employee__1F64254814564679").IsUnique();

            entity.HasIndex(e => e.CardId, "UQ__Employee__55FECDAF8DB2AA4F").IsUnique();

            entity.HasIndex(e => e.FingerprintId, "UQ__Employee__773F7676E76DE64F").IsUnique();

            entity.HasIndex(e => e.NationalId, "UQ__Employee__E9AA32FA3177EB5F").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.BankAccountNumber).HasMaxLength(50);
            entity.Property(e => e.BankName).HasMaxLength(200);
            entity.Property(e => e.BaseSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CardId).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.EmergencyContact).HasMaxLength(200);
            entity.Property(e => e.EmergencyPhone).HasMaxLength(20);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.EmploymentType)
                .HasMaxLength(50)
                .HasDefaultValue("full_time");
            entity.Property(e => e.FingerprintId).HasMaxLength(50);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.JobTitle).HasMaxLength(100);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.MaritalStatus).HasMaxLength(20);
            entity.Property(e => e.NationalId).HasMaxLength(20);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("active");

            entity.HasOne(d => d.Branch).WithMany(p => p.Employees)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Branc__151B244E");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.EmployeeCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .HasConstraintName("FK__Employees__Creat__17036CC0");

            entity.HasOne(d => d.Department).WithMany(p => p.Employees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Employees__Depar__114A936A");

            entity.HasOne(d => d.User).WithOne(p => p.EmployeeUser)
                .HasForeignKey<Employee>(d => d.UserId)
                .HasConstraintName("FK__Employees__UserI__10566F31");
        });

        modelBuilder.Entity<EmployeeAdvance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07B851566B");

            entity.HasIndex(e => e.AdvanceNumber, "UQ__Employee__D7D8906BC9499237").IsUnique();

            entity.Property(e => e.AdvanceNumber).HasMaxLength(50);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.InstallmentAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InstallmentCount).HasDefaultValue(1);
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.RequestDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("pending");

            entity.HasOne(d => d.ApprovedBy).WithMany(p => p.EmployeeAdvanceApprovedBies)
                .HasForeignKey(d => d.ApprovedById)
                .HasConstraintName("FK__EmployeeA__Appro__22751F6C");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.EmployeeAdvanceCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeA__Creat__245D67DE");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeAdvances)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeA__Emplo__1EA48E88");

            entity.HasOne(d => d.PaidBy).WithMany(p => p.EmployeeAdvancePaidBies)
                .HasForeignKey(d => d.PaidById)
                .HasConstraintName("FK__EmployeeA__PaidB__236943A5");
        });

        modelBuilder.Entity<EmployeeShift>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Employee__3214EC07EE9ED21E");

            entity.HasIndex(e => new { e.EmployeeId, e.EffectiveDate }, "UQ_EmployeeShift").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

            entity.HasOne(d => d.Employee).WithMany(p => p.EmployeeShifts)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeS__Emplo__35DCF99B");

            entity.HasOne(d => d.Shift).WithMany(p => p.EmployeeShifts)
                .HasForeignKey(d => d.ShiftId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeS__Shift__36D11DD4");
        });

        modelBuilder.Entity<FiscalPeriod>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FiscalPe__3214EC079AFBC948");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("open");

            entity.HasOne(d => d.ClosedBy).WithMany(p => p.FiscalPeriods)
                .HasForeignKey(d => d.ClosedById)
                .HasConstraintName("FK__FiscalPer__Close__7E57BA87");
        });

        modelBuilder.Entity<FixedAsset>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__FixedAss__3214EC0796884150");

            entity.HasIndex(e => e.AssetCode, "UQ__FixedAss__2DDE5240B3E4C46D").IsUnique();

            entity.Property(e => e.AccumulatedDepreciation).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.AssetCode).HasMaxLength(50);
            entity.Property(e => e.AssetName).HasMaxLength(300);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CurrentValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.DepreciationMethod)
                .HasMaxLength(50)
                .HasDefaultValue("straight_line");
            entity.Property(e => e.DepreciationRate).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.PurchaseCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SalvageValue).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.SerialNumber).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("active");

            entity.HasOne(d => d.Branch).WithMany(p => p.FixedAssets)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FixedAsse__Branc__2A363CC5");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.FixedAssets)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__FixedAsse__Creat__2D12A970");

            entity.HasOne(d => d.Location).WithMany(p => p.FixedAssets)
                .HasForeignKey(d => d.LocationId)
                .HasConstraintName("FK__FixedAsse__Locat__2942188C");
        });

        modelBuilder.Entity<GoodsReceipt>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GoodsRec__3214EC0749A84A5F");

            entity.HasIndex(e => e.ReceiptNumber, "UQ__GoodsRec__C08AFDABE9B6726D").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DocumentNumber).HasMaxLength(100);
            entity.Property(e => e.DriverName).HasMaxLength(200);
            entity.Property(e => e.ReceiptDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ReceiptNumber).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("pending");
            entity.Property(e => e.VehicleNumber).HasMaxLength(50);

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.GoodsReceiptCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GoodsRece__Creat__689D8392");

            entity.HasOne(d => d.InspectedBy).WithMany(p => p.GoodsReceiptInspectedBies)
                .HasForeignKey(d => d.InspectedById)
                .HasConstraintName("FK__GoodsRece__Inspe__67A95F59");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.GoodsReceipts)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GoodsRece__Purch__62E4AA3C");

            entity.HasOne(d => d.Supplier).WithMany(p => p.GoodsReceipts)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GoodsRece__Suppl__63D8CE75");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.GoodsReceipts)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__GoodsRece__Wareh__64CCF2AE");
        });

        modelBuilder.Entity<GoodsReceiptItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__GoodsRec__3214EC0766AE1AB5");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.QuantityDamaged).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuantityOrdered).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuantityReceived).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.GoodsReceipt).WithMany(p => p.GoodsReceiptItems)
                .HasForeignKey(d => d.GoodsReceiptId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GoodsRece__Goods__6C6E1476");

            entity.HasOne(d => d.Product).WithMany(p => p.GoodsReceiptItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__GoodsRece__Produ__6E565CE8");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.GoodsReceiptItems)
                .HasForeignKey(d => d.ProductVariantId)
                .HasConstraintName("FK__GoodsRece__Produ__6F4A8121");

            entity.HasOne(d => d.PurchaseOrderItem).WithMany(p => p.GoodsReceiptItems)
                .HasForeignKey(d => d.PurchaseOrderItemId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__GoodsRece__Purch__6D6238AF");
        });

        modelBuilder.Entity<ImportError>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ImportEr__3214EC0726C29311");

            entity.Property(e => e.ColumnName).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ErrorMessage).HasMaxLength(500);

            entity.HasOne(d => d.Import).WithMany(p => p.ImportErrors)
                .HasForeignKey(d => d.ImportId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ImportErr__Impor__43F60EC8");
        });

        modelBuilder.Entity<Installment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Installm__3214EC07ECFF39A5");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DownPayment).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InstallmentAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("active");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Customer).WithMany(p => p.Installments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Installme__Custo__3EA749C6");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Installments)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Installme__Invoi__3DB3258D");
        });

        modelBuilder.Entity<InstallmentDetail>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Installm__3214EC0745718B7C");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LateFee).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("pending");

            entity.HasOne(d => d.Installment).WithMany(p => p.InstallmentDetails)
                .HasForeignKey(d => d.InstallmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Installme__Insta__4460231C");
        });

        modelBuilder.Entity<Inventory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC0711D94D31");

            entity.ToTable("Inventory", tb => tb.HasTrigger("trg_LowStockAlert"));

            entity.HasIndex(e => e.QuantityAvailable, "IX_Inventory_Avaliable");

            entity.HasIndex(e => e.ProductVariantId, "IX_Inventory_Product");

            entity.HasIndex(e => e.WarehouseId, "IX_Inventory_Warehouse");

            entity.HasIndex(e => new { e.ProductVariantId, e.WarehouseId }, "UQ_Inventory").IsUnique();

            entity.Property(e => e.QuantityAvailable).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuantityOnHand).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuantityReserved).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Produ__4B422AD5");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Wareh__4C364F0E");

            entity.HasOne(d => d.WarehouseLocation).WithMany(p => p.Inventories)
                .HasForeignKey(d => d.WarehouseLocationId)
                .HasConstraintName("FK__Inventory__Wareh__4D2A7347");
        });

        modelBuilder.Entity<InventoryTransaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC073C64A647");

            entity.HasIndex(e => e.TransactionNumber, "UQ__Inventor__E733A2BFDE787757").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.NewQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PreviousQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.ReferenceType).HasMaxLength(50);
            entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TransactionNumber).HasMaxLength(50);
            entity.Property(e => e.TransactionType).HasMaxLength(50);
            entity.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Creat__589C25F3");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Produ__56B3DD81");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.InventoryTransactions)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Wareh__57A801BA");
        });

        modelBuilder.Entity<InventoryTransfer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC0705E3B399");

            entity.HasIndex(e => e.TransferNumber, "UQ__Inventor__02A4FA0D855B6925").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("pending");
            entity.Property(e => e.TransferDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.TransferNumber).HasMaxLength(50);

            entity.HasOne(d => d.ApprovedBy).WithMany(p => p.InventoryTransferApprovedBies)
                .HasForeignKey(d => d.ApprovedById)
                .HasConstraintName("FK__Inventory__Appro__640DD89F");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.InventoryTransferCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Creat__6319B466");

            entity.HasOne(d => d.FromBranch).WithMany(p => p.InventoryTransferFromBranches)
                .HasForeignKey(d => d.FromBranchId)
                .HasConstraintName("FK__Inventory__FromB__5D60DB10");

            entity.HasOne(d => d.FromWarehouse).WithMany(p => p.InventoryTransferFromWarehouses)
                .HasForeignKey(d => d.FromWarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__FromW__5F492382");

            entity.HasOne(d => d.ToBranch).WithMany(p => p.InventoryTransferToBranches)
                .HasForeignKey(d => d.ToBranchId)
                .HasConstraintName("FK__Inventory__ToBra__5E54FF49");

            entity.HasOne(d => d.ToWarehouse).WithMany(p => p.InventoryTransferToWarehouses)
                .HasForeignKey(d => d.ToWarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__ToWar__603D47BB");
        });

        modelBuilder.Entity<InventoryTransferItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Inventor__3214EC073C32BA56");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReceivedQuantity).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.InventoryTransferItems)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Produ__68D28DBC");

            entity.HasOne(d => d.Transfer).WithMany(p => p.InventoryTransferItems)
                .HasForeignKey(d => d.TransferId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Inventory__Trans__67DE6983");
        });

        modelBuilder.Entity<Invoice>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Invoices__3214EC07324EDBF1");

            entity.ToTable(tb => tb.HasTrigger("trg_UpdateCustomerBalance"));

            entity.HasIndex(e => e.CustomerId, "IX_Invoices_CustomerId");

            entity.HasIndex(e => e.InvoiceDate, "IX_Invoices_Date");

            entity.HasIndex(e => e.Status, "IX_Invoices_Status");

            entity.HasIndex(e => e.InvoiceNumber, "UQ__Invoices__D776E98176912122").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("EGP");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InvoiceDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.InvoiceNumber).HasMaxLength(50);
            entity.Property(e => e.InvoiceType)
                .HasMaxLength(20)
                .HasDefaultValue("sales");
            entity.Property(e => e.IsTaxInvoice).HasDefaultValue(true);
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RemainingAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("draft");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__Create__25DB9BFC");

            entity.HasOne(d => d.Customer).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Invoices__Custom__1975C517");

            entity.HasOne(d => d.Quotation).WithMany(p => p.Invoices)
                .HasForeignKey(d => d.QuotationId)
                .HasConstraintName("FK__Invoices__Quotat__1881A0DE");
        });

        modelBuilder.Entity<InvoiceItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__InvoiceI__3214EC07F095BB88");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.LineTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Bundle).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.BundleId)
                .HasConstraintName("FK__InvoiceIt__Bundl__30592A6F");

            entity.HasOne(d => d.Invoice).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.InvoiceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__InvoiceIt__Invoi__2AA05119");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.ProductVariantId)
                .HasConstraintName("FK__InvoiceIt__Produ__2C88998B");

            entity.HasOne(d => d.QuotationItem).WithMany(p => p.InvoiceItems)
                .HasForeignKey(d => d.QuotationItemId)
                .HasConstraintName("FK__InvoiceIt__Quota__2B947552");
        });

        modelBuilder.Entity<JournalEntry>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JournalE__3214EC077663D122");

            entity.HasIndex(e => e.EntryDate, "IX_JournalEntries_Date");

            entity.HasIndex(e => e.Status, "IX_JournalEntries_Status");

            entity.HasIndex(e => e.EntryNumber, "UQ__JournalE__488B566F98DEC4EB").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.EntryDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EntryNumber).HasMaxLength(50);
            entity.Property(e => e.ReferenceType).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("draft");
            entity.Property(e => e.TotalCredit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalDebit).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ApprovedBy).WithMany(p => p.JournalEntryApprovedBies)
                .HasForeignKey(d => d.ApprovedById)
                .HasConstraintName("FK__JournalEn__Appro__116A8EFB");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.JournalEntryCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JournalEn__Creat__10766AC2");

            entity.HasOne(d => d.FiscalPeriod).WithMany(p => p.JournalEntries)
                .HasForeignKey(d => d.FiscalPeriodId)
                .HasConstraintName("FK__JournalEn__Fisca__0F824689");

            entity.HasOne(d => d.ReversedEntry).WithMany(p => p.InverseReversedEntry)
                .HasForeignKey(d => d.ReversedEntryId)
                .HasConstraintName("FK__JournalEn__Rever__0E8E2250");
        });

        modelBuilder.Entity<JournalEntryLine>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__JournalE__3214EC07E70A6362");

            entity.HasIndex(e => e.AccountId, "IX_JournalEntryLines_Account");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Credit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Debit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Description).HasMaxLength(500);

            entity.HasOne(d => d.Account).WithMany(p => p.JournalEntryLines)
                .HasForeignKey(d => d.AccountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JournalEn__Accou__162F4418");

            entity.HasOne(d => d.CostCenter).WithMany(p => p.JournalEntryLines)
                .HasForeignKey(d => d.CostCenterId)
                .HasConstraintName("FK__JournalEn__CostC__190BB0C3");

            entity.HasOne(d => d.JournalEntry).WithMany(p => p.JournalEntryLines)
                .HasForeignKey(d => d.JournalEntryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__JournalEn__Journ__153B1FDF");
        });

        modelBuilder.Entity<LeadSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeadSour__3214EC07A3844F20");

            entity.HasIndex(e => e.Name, "UQ__LeadSour__737584F622984DD0").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<LeaveBalance>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveBal__3214EC072E3F367F");

            entity.HasIndex(e => new { e.EmployeeId, e.Year, e.LeaveType }, "UQ_LeaveBalance").IsUnique();

            entity.Property(e => e.LeaveType).HasMaxLength(50);

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveBalances)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LeaveBala__Emplo__5CF6C6BC");
        });

        modelBuilder.Entity<LeaveRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__LeaveReq__3214EC07B54A22FD");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LeaveType).HasMaxLength(50);
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.RejectionReason).HasMaxLength(500);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("pending");

            entity.HasOne(d => d.ApprovedBy).WithMany(p => p.LeaveRequests)
                .HasForeignKey(d => d.ApprovedById)
                .HasConstraintName("FK__LeaveRequ__Appro__5832119F");

            entity.HasOne(d => d.Employee).WithMany(p => p.LeaveRequests)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__LeaveRequ__Emplo__5649C92D");
        });

        modelBuilder.Entity<MaterialConsumption>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Material__3214EC076052B839");

            entity.ToTable("MaterialConsumption");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.QuantityPlanned).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuantityUsed).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuantityWasted).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.UnitCost).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.MaterialConsumptions)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MaterialC__Creat__21D600EE");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.MaterialConsumptions)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MaterialC__Produ__1D114BD1");

            entity.HasOne(d => d.ProductionOrder).WithMany(p => p.MaterialConsumptions)
                .HasForeignKey(d => d.ProductionOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MaterialC__Produ__1B29035F");

            entity.HasOne(d => d.ProductionStage).WithMany(p => p.MaterialConsumptions)
                .HasForeignKey(d => d.ProductionStageId)
                .HasConstraintName("FK__MaterialC__Produ__1C1D2798");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.MaterialConsumptions)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__MaterialC__Wareh__1E05700A");
        });

        modelBuilder.Entity<Module>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Modules__3214EC07FC800EA9");

            entity.HasIndex(e => e.Name, "UQ__Modules__737584F6FDBC0656").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.Icon).HasMaxLength(100);
            entity.Property(e => e.IsEnabled).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Version).HasMaxLength(20);

            entity.HasOne(d => d.RequiresModule).WithMany(p => p.InverseRequiresModule)
                .HasForeignKey(d => d.RequiresModuleId)
                .HasConstraintName("FK__Modules__Require__5BE2A6F2");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Notifica__3214EC0773CC5DF4");

            entity.Property(e => e.ActionUrl).HasMaxLength(500);
            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ReferenceType).HasMaxLength(100);
            entity.Property(e => e.Title).HasMaxLength(300);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasDefaultValue("info");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Notificat__UserI__4B973090");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payments__3214EC07B605DA96");

            entity.ToTable(tb => tb.HasTrigger("trg_UpdateCashRegisterBalance"));

            entity.HasIndex(e => e.CustomerId, "IX_Payments_CustomerId");

            entity.HasIndex(e => e.PaymentDate, "IX_Payments_Date");

            entity.HasIndex(e => e.InvoiceId, "IX_Payments_InvoiceId");

            entity.HasIndex(e => e.PaymentNumber, "UQ__Payments__E2C1723B68A753B3").IsUnique();

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BankName).HasMaxLength(200);
            entity.Property(e => e.CheckNumber).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.PaymentDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentNumber).HasMaxLength(50);
            entity.Property(e => e.ReferenceNumber).HasMaxLength(100);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("completed");

            entity.HasOne(d => d.CashRegister).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CashRegisterId)
                .HasConstraintName("FK__Payments__CashRe__370627FE");

            entity.HasOne(d => d.Customer).WithMany(p => p.Payments)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Custom__361203C5");

            entity.HasOne(d => d.Invoice).WithMany(p => p.Payments)
                .HasForeignKey(d => d.InvoiceId)
                .HasConstraintName("FK__Payments__Invoic__351DDF8C");

            entity.HasOne(d => d.ReceivedBy).WithMany(p => p.Payments)
                .HasForeignKey(d => d.ReceivedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payments__Receiv__39E294A9");
        });

        modelBuilder.Entity<Payroll>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Payrolls__3214EC0717307160");

            entity.HasIndex(e => new { e.EmployeeId, e.PeriodYear, e.PeriodMonth }, "UQ_Payroll").IsUnique();

            entity.Property(e => e.AdvanceDeductions).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Allowances).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.BaseSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Bonus).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Deductions).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.InsuranceDeduction).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.NetSalary).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OvertimePay).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("draft");
            entity.Property(e => e.TaxDeduction).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ApprovedBy).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.ApprovedById)
                .HasConstraintName("FK__Payrolls__Approv__52793849");

            entity.HasOne(d => d.Employee).WithMany(p => p.Payrolls)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Payrolls__Employ__47FBA9D6");
        });

        modelBuilder.Entity<Permission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC07E69977B1");

            entity.HasIndex(e => e.Code, "UQ__Permissi__A25C5AA72C257743").IsUnique();

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.Code).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.Name).HasMaxLength(100);

            entity.HasOne(d => d.Module).WithMany(p => p.Permissions)
                .HasForeignKey(d => d.ModuleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Permissio__Modul__6754599E");
        });

        modelBuilder.Entity<PermissionAuditLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Permissi__3214EC07A01B2920");

            entity.Property(e => e.Action).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EntityType).HasMaxLength(100);
            entity.Property(e => e.IpAddress).HasMaxLength(45);
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Products__3214EC074A226836");

            entity.HasIndex(e => e.CategoryId, "IX_Products_CategoryId");

            entity.HasIndex(e => e.IsDeleted, "IX_Products_IsDeleted");

            entity.HasIndex(e => e.Sku, "IX_Products_Sku");

            entity.HasIndex(e => e.Sku, "UQ__Products__CA1FD3C5281A23C0").IsUnique();

            entity.Property(e => e.BundleDiscount).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DefaultWarrantyMonths).HasDefaultValue(12);
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.IsManufactured).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.NameEn).HasMaxLength(300);
            entity.Property(e => e.ProductType)
                .HasMaxLength(20)
                .HasDefaultValue("single");
            entity.Property(e => e.ShortDescription).HasMaxLength(500);
            entity.Property(e => e.Sku).HasMaxLength(100);
            entity.Property(e => e.Weight).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.Branch).WithMany(p => p.Products)
                .HasForeignKey(d => d.BranchId)
                .HasConstraintName("FK__Products__Branch__4A8310C6");

            entity.HasOne(d => d.Category).WithMany(p => p.Products)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Products__Catego__42E1EEFE");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.Products)
                .HasForeignKey(d => d.CreatedById)
                .HasConstraintName("FK__Products__Create__4B7734FF");
        });

        modelBuilder.Entity<ProductBundleComponent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductB__3214EC07D6C17991");

            entity.HasIndex(e => new { e.BundleProductId, e.ComponentProductId, e.ComponentVariantId }, "UQ_BundleComponent").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Quantity).HasDefaultValue(1);

            entity.HasOne(d => d.BundleProduct).WithMany(p => p.ProductBundleComponentBundleProducts)
                .HasForeignKey(d => d.BundleProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductBu__Bundl__5CA1C101");

            entity.HasOne(d => d.ComponentProduct).WithMany(p => p.ProductBundleComponentComponentProducts)
                .HasForeignKey(d => d.ComponentProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductBu__Compo__5D95E53A");

            entity.HasOne(d => d.ComponentVariant).WithMany(p => p.ProductBundleComponents)
                .HasForeignKey(d => d.ComponentVariantId)
                .HasConstraintName("FK__ProductBu__Compo__5E8A0973");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductI__3214EC0718451E1E");

            entity.Property(e => e.AltText).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.ThumbnailUrl).HasMaxLength(500);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductIm__Produ__6AEFE058");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductVariantId)
                .HasConstraintName("FK__ProductIm__Produ__6BE40491");
        });

        modelBuilder.Entity<ProductVariant>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductV__3214EC0754FB16EB");

            entity.HasIndex(e => e.Barcode, "UQ__ProductV__177800D3705F559C").IsUnique();

            entity.HasIndex(e => e.Sku, "UQ__ProductV__CA1FD3C5CF64C031").IsUnique();

            entity.Property(e => e.ActualCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Barcode).HasMaxLength(100);
            entity.Property(e => e.CostPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DiscountPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ImageUrl).HasMaxLength(500);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.RetailPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ShowroomPrice).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Sku).HasMaxLength(100);
            entity.Property(e => e.WholesalePrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductVariants)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductVa__Produ__51300E55");
        });

        modelBuilder.Entity<ProductVariantAttribute>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProductV__3214EC0766D2BADA");

            entity.HasIndex(e => new { e.ProductVariantId, e.AttributeId }, "UQ_VariantAttr").IsUnique();

            entity.HasOne(d => d.Attribute).WithMany(p => p.ProductVariantAttributes)
                .HasForeignKey(d => d.AttributeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductVa__Attri__671F4F74");

            entity.HasOne(d => d.AttributeValue).WithMany(p => p.ProductVariantAttributes)
                .HasForeignKey(d => d.AttributeValueId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductVa__Attri__681373AD");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.ProductVariantAttributes)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProductVa__Produ__662B2B3B");
        });

        modelBuilder.Entity<ProductionOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC07024DB40D");

            entity.HasIndex(e => e.ProductionNumber, "UQ__Producti__6DAF7582915E5627").IsUnique();

            entity.Property(e => e.ActualCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ActualHours).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Bomid).HasColumnName("BOMId");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EstimatedCost).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EstimatedHours).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.Priority)
                .HasMaxLength(20)
                .HasDefaultValue("normal");
            entity.Property(e => e.ProductionNumber).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("planned");

            entity.HasOne(d => d.Bom).WithMany(p => p.ProductionOrders)
                .HasForeignKey(d => d.Bomid)
                .HasConstraintName("FK__Productio__BOMId__08162EEB");

            entity.HasOne(d => d.Branch).WithMany(p => p.ProductionOrders)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Productio__Branc__090A5324");

            entity.HasOne(d => d.CompletedBy).WithMany(p => p.ProductionOrderCompletedBies)
                .HasForeignKey(d => d.CompletedById)
                .HasConstraintName("FK__Productio__Compl__119F9925");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.ProductionOrderCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Productio__Creat__0EC32C7A");

            entity.HasOne(d => d.InvoiceItem).WithMany(p => p.ProductionOrders)
                .HasForeignKey(d => d.InvoiceItemId)
                .HasConstraintName("FK__Productio__Invoi__062DE679");

            entity.HasOne(d => d.Product).WithMany(p => p.ProductionOrders)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Productio__Produ__07220AB2");

            entity.HasOne(d => d.StartedBy).WithMany(p => p.ProductionOrderStartedBies)
                .HasForeignKey(d => d.StartedById)
                .HasConstraintName("FK__Productio__Start__10AB74EC");

            entity.HasOne(d => d.Supervisor).WithMany(p => p.ProductionOrderSupervisors)
                .HasForeignKey(d => d.SupervisorId)
                .HasConstraintName("FK__Productio__Super__0FB750B3");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.ProductionOrders)
                .HasForeignKey(d => d.WarehouseId)
                .HasConstraintName("FK__Productio__Wareh__0DCF0841");
        });

        modelBuilder.Entity<ProductionStage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Producti__3214EC07410FAD7C");

            entity.Property(e => e.ActualHours).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.EstimatedHours).HasColumnType("decimal(10, 2)");
            entity.Property(e => e.StageName).HasMaxLength(200);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("pending");

            entity.HasOne(d => d.AssignedTo).WithMany(p => p.ProductionStages)
                .HasForeignKey(d => d.AssignedToId)
                .HasConstraintName("FK__Productio__Assig__1758727B");

            entity.HasOne(d => d.ProductionOrder).WithMany(p => p.ProductionStages)
                .HasForeignKey(d => d.ProductionOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Productio__Produ__15702A09");
        });

        modelBuilder.Entity<PurchaseOrder>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Purchase__3214EC071517CB90");

            entity.HasIndex(e => e.PoNumber, "UQ__Purchase__E5352153DB0AE59C").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OrderDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PaymentMethod).HasMaxLength(50);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .HasDefaultValue("unpaid");
            entity.Property(e => e.PoNumber).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("draft");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ApprovedBy).WithMany(p => p.PurchaseOrderApprovedBies)
                .HasForeignKey(d => d.ApprovedById)
                .HasConstraintName("FK__PurchaseO__Appro__558AAF1E");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.PurchaseOrderCreatedBies)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Creat__54968AE5");

            entity.HasOne(d => d.Supplier).WithMany(p => p.PurchaseOrders)
                .HasForeignKey(d => d.SupplierId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Suppl__4C0144E4");
        });

        modelBuilder.Entity<PurchaseOrderItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Purchase__3214EC0786962614");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ItemDescription).HasMaxLength(500);
            entity.Property(e => e.LineTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReceivedQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UnitMeasure).HasMaxLength(20);
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__PurchaseO__Produ__5A4F643B");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.ProductVariantId)
                .HasConstraintName("FK__PurchaseO__Produ__5B438874");

            entity.HasOne(d => d.PurchaseOrder).WithMany(p => p.PurchaseOrderItems)
                .HasForeignKey(d => d.PurchaseOrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PurchaseO__Purch__595B4002");
        });

        modelBuilder.Entity<Quotation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC079A04F518");

            entity.HasIndex(e => e.QuotationNumber, "UQ__Quotatio__F3A63C4ACD2D6F26").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuotationNumber).HasMaxLength(50);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("draft");
            entity.Property(e => e.SubTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.ConvertedInvoice).WithMany(p => p.Quotations)
                .HasForeignKey(d => d.ConvertedInvoiceId)
                .HasConstraintName("FK_Quotations_Invoice");

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.Quotations)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Quotation__Creat__0B27A5C0");

            entity.HasOne(d => d.Customer).WithMany(p => p.Quotations)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Quotation__Custo__056ECC6A");
        });

        modelBuilder.Entity<QuotationItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Quotatio__3214EC0706CEEAC3");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DiscountPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.ItemDescription).HasMaxLength(500);
            entity.Property(e => e.LineTotal).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Quantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.TaxPercent).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Product).WithMany(p => p.QuotationItems)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK__Quotation__Produ__0FEC5ADD");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.QuotationItems)
                .HasForeignKey(d => d.ProductVariantId)
                .HasConstraintName("FK__Quotation__Produ__10E07F16");

            entity.HasOne(d => d.Quotation).WithMany(p => p.QuotationItems)
                .HasForeignKey(d => d.QuotationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Quotation__Quota__0EF836A4");
        });

        modelBuilder.Entity<RawAttendanceLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RawAtten__3214EC072E4DF687");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.LogType).HasMaxLength(10);
            entity.Property(e => e.VerifiedBy).HasMaxLength(20);

            entity.HasOne(d => d.Device).WithMany(p => p.RawAttendanceLogs)
                .HasForeignKey(d => d.DeviceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RawAttend__Devic__2B5F6B28");

            entity.HasOne(d => d.Employee).WithMany(p => p.RawAttendanceLogs)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RawAttend__Emplo__2C538F61");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Roles__3214EC077E2C03A7");

            entity.HasIndex(e => e.Name, "UQ__Roles__737584F6FA029044").IsUnique();

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.DisplayName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<RolePermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__RolePerm__3214EC075566744D");

            entity.HasIndex(e => new { e.RoleId, e.PermissionId }, "UQ_RolePermission").IsUnique();

            entity.Property(e => e.GrantedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsGranted).HasDefaultValue(true);

            entity.HasOne(d => d.Permission).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolePermi__Permi__6E01572D");

            entity.HasOne(d => d.Role).WithMany(p => p.RolePermissions)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__RolePermi__RoleI__6D0D32F4");
        });

        modelBuilder.Entity<ShiftSchedule>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ShiftSch__3214EC070247B08E");

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.WorkingHoursPerDay).HasColumnType("decimal(4, 2)");
        });

        modelBuilder.Entity<StockCount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StockCou__3214EC07C7F596F0");

            entity.HasIndex(e => e.CountNumber, "UQ__StockCou__BC624C3657AB5079").IsUnique();

            entity.Property(e => e.CountDate).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CountNumber).HasMaxLength(50);
            entity.Property(e => e.CountType)
                .HasMaxLength(20)
                .HasDefaultValue("full");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .HasDefaultValue("draft");

            entity.HasOne(d => d.ApprovedBy).WithMany(p => p.StockCountApprovedBies)
                .HasForeignKey(d => d.ApprovedById)
                .HasConstraintName("FK__StockCoun__Appro__73501C2F");

            entity.HasOne(d => d.StartedBy).WithMany(p => p.StockCountStartedBies)
                .HasForeignKey(d => d.StartedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockCoun__Start__725BF7F6");

            entity.HasOne(d => d.Warehouse).WithMany(p => p.StockCounts)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockCoun__Wareh__6E8B6712");
        });

        modelBuilder.Entity<StockCountAdjustment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StockCou__3214EC0722805B16");

            entity.Property(e => e.AdjustmentAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.NewQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.OldQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Reason).HasMaxLength(500);

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.StockCountAdjustments)
                .HasForeignKey(d => d.CreatedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockCoun__Creat__00AA174D");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.StockCountAdjustments)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockCoun__Produ__7FB5F314");

            entity.HasOne(d => d.StockCount).WithMany(p => p.StockCountAdjustments)
                .HasForeignKey(d => d.StockCountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockCoun__Stock__7EC1CEDB");
        });

        modelBuilder.Entity<StockCountItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StockCou__3214EC070CA847F7");

            entity.Property(e => e.CountedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Difference).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PhysicalQuantity).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Reason).HasMaxLength(500);
            entity.Property(e => e.SystemQuantity).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Bundle).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.BundleId)
                .HasConstraintName("FK__StockCoun__Bundl__79FD19BE");

            entity.HasOne(d => d.CountedBy).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.CountedById)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockCoun__Count__7AF13DF7");

            entity.HasOne(d => d.ProductVariant).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.ProductVariantId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockCoun__Produ__7814D14C");

            entity.HasOne(d => d.StockCount).WithMany(p => p.StockCountItems)
                .HasForeignKey(d => d.StockCountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockCoun__Stock__7720AD13");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Supplier__3214EC07C85C5609");

            entity.HasIndex(e => e.SupplierCode, "UQ__Supplier__44BE981B2B6CA1F0").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.CommercialRegister).HasMaxLength(50);
            entity.Property(e => e.ContactPerson).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.CreditLimit).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Currency)
                .HasMaxLength(10)
                .HasDefaultValue("EGP");
            entity.Property(e => e.CurrentBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(300);
            entity.Property(e => e.NameEn).HasMaxLength(300);
            entity.Property(e => e.PaymentTerms).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Phone2).HasMaxLength(20);
            entity.Property(e => e.Rating).HasColumnType("decimal(3, 1)");
            entity.Property(e => e.SupplierCode).HasMaxLength(50);
            entity.Property(e => e.TaxNumber).HasMaxLength(50);

            entity.HasOne(d => d.CreatedBy).WithMany(p => p.Suppliers)
                .HasForeignKey(d => d.CreatedById)
                .HasConstraintName("FK__Suppliers__Creat__0C50D423");
        });

        modelBuilder.Entity<SystemSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SystemSe__3214EC07B31CD8CC");

            entity.HasIndex(e => e.SettingKey, "UQ__SystemSe__01E719AD7C1E8DD1").IsUnique();

            entity.Property(e => e.Category).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.SettingKey).HasMaxLength(200);
            entity.Property(e => e.SettingType)
                .HasMaxLength(50)
                .HasDefaultValue("string");

            entity.HasOne(d => d.UpdatedBy).WithMany(p => p.SystemSettings)
                .HasForeignKey(d => d.UpdatedById)
                .HasConstraintName("FK__SystemSet__Updat__542C7691");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Users__3214EC07300EE5A2");

            entity.HasIndex(e => e.Email, "IX_Users_Email");

            entity.HasIndex(e => e.IsActive, "IX_Users_IsActive");

            entity.HasIndex(e => e.RoleId, "IX_Users_RoleId");

            entity.HasIndex(e => e.Username, "UQ__Users__536C85E44BB1406C").IsUnique();

            entity.HasIndex(e => e.EmployeeId, "UQ__Users__7AD04F105E726B81").IsUnique();

            entity.HasIndex(e => e.Email, "UQ__Users__A9D105346BF936AA").IsUnique();

            entity.Property(e => e.AvatarUrl).HasMaxLength(500);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.DisplayName).HasMaxLength(200);
            entity.Property(e => e.Email).HasMaxLength(255);
            entity.Property(e => e.FirstName).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.JobTitle).HasMaxLength(100);
            entity.Property(e => e.LastLoginIp).HasMaxLength(45);
            entity.Property(e => e.LastName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasMaxLength(500);
            entity.Property(e => e.PasswordSalt).HasMaxLength(200);
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Username).HasMaxLength(100);

            entity.HasOne(d => d.DefaultBranch).WithMany(p => p.Users)
                .HasForeignKey(d => d.DefaultBranchId)
                .HasConstraintName("FK__Users__DefaultBr__03F0984C");

            entity.HasOne(d => d.Department).WithMany(p => p.Users)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FK__Users__Departmen__02084FDA");

            entity.HasOne(d => d.Employee).WithOne(p => p.UserNavigation)
                .HasForeignKey<User>(d => d.EmployeeId)
                .HasConstraintName("FK_Users_Employee");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Users__RoleId__02FC7413");
        });

        modelBuilder.Entity<UserEntityPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserEnti__3214EC07695B9E5F");

            entity.HasIndex(e => new { e.UserId, e.EntityType, e.EntityId, e.PermissionType }, "UQ_UserEntityPermission").IsUnique();

            entity.Property(e => e.EntityType).HasMaxLength(100);
            entity.Property(e => e.GrantedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.PermissionType).HasMaxLength(50);
        });

        modelBuilder.Entity<UserPermission>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__UserPerm__3214EC071AFF1B4E");

            entity.HasIndex(e => new { e.UserId, e.PermissionId }, "UQ_UserPermission").IsUnique();

            entity.Property(e => e.GrantedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsGranted).HasDefaultValue(true);
            entity.Property(e => e.Reason).HasMaxLength(500);

            entity.HasOne(d => d.Permission).WithMany(p => p.UserPermissions)
                .HasForeignKey(d => d.PermissionId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__UserPermi__Permi__73BA3083");
        });

        modelBuilder.Entity<VwBalanceSheet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_BalanceSheet");

            entity.Property(e => e.AccountCode).HasMaxLength(30);
            entity.Property(e => e.AccountName).HasMaxLength(200);
            entity.Property(e => e.Balance).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Category)
                .HasMaxLength(12)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwCashRegisterStatement>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_CashRegisterStatement");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CashRegisterName).HasMaxLength(200);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedBy).HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(500);
            entity.Property(e => e.Direction).HasMaxLength(10);
            entity.Property(e => e.NewBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.PreviousBalance).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.ReferenceType).HasMaxLength(50);
            entity.Property(e => e.TransactionNumber).HasMaxLength(50);
            entity.Property(e => e.TransactionType).HasMaxLength(20);
        });

        modelBuilder.Entity<VwDailyAttendance>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DailyAttendance");

            entity.Property(e => e.Department).HasMaxLength(100);
            entity.Property(e => e.EmployeeCode).HasMaxLength(50);
            entity.Property(e => e.EmployeeName).HasMaxLength(200);
            entity.Property(e => e.OvertimeHours).HasColumnType("decimal(5, 2)");
            entity.Property(e => e.Status).HasMaxLength(20);
            entity.Property(e => e.WorkingHours).HasColumnType("decimal(5, 2)");
        });

        modelBuilder.Entity<VwDueInstallment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_DueInstallments");

            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.CustomerName).HasMaxLength(200);
            entity.Property(e => e.PaidAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.Phone).HasMaxLength(20);
            entity.Property(e => e.Status).HasMaxLength(50);
        });

        modelBuilder.Entity<VwEmployeeAdvance>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_EmployeeAdvances");

            entity.Property(e => e.AdvanceNumber).HasMaxLength(50);
            entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.EmployeeName).HasMaxLength(200);
            entity.Property(e => e.InstallmentAmount).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.RemainingBalance).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Status).HasMaxLength(50);
            entity.Property(e => e.TotalDeducted).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<VwIncomeStatement>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_IncomeStatement");

            entity.Property(e => e.AccountCode).HasMaxLength(30);
            entity.Property(e => e.AccountName).HasMaxLength(200);
            entity.Property(e => e.Amount).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.Category)
                .HasMaxLength(9)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwInventoryStatus>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_InventoryStatus");

            entity.Property(e => e.BranchName).HasMaxLength(200);
            entity.Property(e => e.ProductName).HasMaxLength(300);
            entity.Property(e => e.QuantityAvailable).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuantityOnHand).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.QuantityReserved).HasColumnType("decimal(18, 2)");
            entity.Property(e => e.StockStatus)
                .HasMaxLength(5)
                .IsUnicode(false);
            entity.Property(e => e.VariantName).HasMaxLength(300);
            entity.Property(e => e.VariantSku).HasMaxLength(100);
            entity.Property(e => e.WarehouseName).HasMaxLength(200);
        });

        modelBuilder.Entity<VwMonthlySale>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_MonthlySales");

            entity.Property(e => e.AvgInvoiceValue).HasColumnType("decimal(38, 6)");
            entity.Property(e => e.TotalPaid).HasColumnType("decimal(38, 2)");
            entity.Property(e => e.TotalRevenue).HasColumnType("decimal(38, 2)");
        });

        modelBuilder.Entity<Warehouse>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warehous__3214EC07FAFDC639");

            entity.HasIndex(e => e.Code, "UQ__Warehous__A25C5AA740A3B536").IsUnique();

            entity.Property(e => e.Address).HasMaxLength(500);
            entity.Property(e => e.City).HasMaxLength(100);
            entity.Property(e => e.Code).HasMaxLength(50);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(200);
            entity.Property(e => e.Notes).HasMaxLength(500);
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasDefaultValue("general");

            entity.HasOne(d => d.Branch).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.BranchId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__Branc__3CF40B7E");

            entity.HasOne(d => d.Manager).WithMany(p => p.Warehouses)
                .HasForeignKey(d => d.ManagerId)
                .HasConstraintName("FK__Warehouse__Manag__3EDC53F0");
        });

        modelBuilder.Entity<WarehouseLocation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Warehous__3214EC07A0419B6E");

            entity.Property(e => e.Aisle).HasMaxLength(20);
            entity.Property(e => e.Bin).HasMaxLength(20);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");
            entity.Property(e => e.FullLocation).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Rack).HasMaxLength(20);
            entity.Property(e => e.Shelf).HasMaxLength(20);

            entity.HasOne(d => d.Warehouse).WithMany(p => p.WarehouseLocations)
                .HasForeignKey(d => d.WarehouseId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Warehouse__Wareh__4589517F");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
