using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class User
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string? Phone { get; set; }

    public string PasswordHash { get; set; } = null!;

    public string? PasswordSalt { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? AvatarUrl { get; set; }

    public int? DepartmentId { get; set; }

    public int RoleId { get; set; }

    public int? DefaultBranchId { get; set; }

    public int? EmployeeId { get; set; }

    public string? JobTitle { get; set; }

    public bool IsActive { get; set; }

    public bool MustChangePassword { get; set; }

    public DateTime? LastLoginAt { get; set; }

    public string? LastLoginIp { get; set; }

    public int FailedLoginAttempts { get; set; }

    public DateTime? LockoutEnd { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<ActivityLog> ActivityLogs { get; set; } = new List<ActivityLog>();

    public virtual ICollection<AssetTransaction> AssetTransactions { get; set; } = new List<AssetTransaction>();

    public virtual ICollection<BankDeposit> BankDeposits { get; set; } = new List<BankDeposit>();

    public virtual ICollection<BillOfMaterial> BillOfMaterials { get; set; } = new List<BillOfMaterial>();

    public virtual ICollection<Branch> Branches { get; set; } = new List<Branch>();

    public virtual ICollection<Budget> Budgets { get; set; } = new List<Budget>();

    public virtual ICollection<CashRegisterDailySettlement> CashRegisterDailySettlements { get; set; } = new List<CashRegisterDailySettlement>();

    public virtual ICollection<CashRegisterTransaction> CashRegisterTransactions { get; set; } = new List<CashRegisterTransaction>();

    public virtual ICollection<CashRegister> CashRegisters { get; set; } = new List<CashRegister>();

    public virtual ICollection<CashTransfer> CashTransferApprovedBies { get; set; } = new List<CashTransfer>();

    public virtual ICollection<CashTransfer> CashTransferCreatedBies { get; set; } = new List<CashTransfer>();

    public virtual ICollection<ComplaintActivity> ComplaintActivities { get; set; } = new List<ComplaintActivity>();

    public virtual ICollection<Complaint> ComplaintAssignedTos { get; set; } = new List<Complaint>();

    public virtual ICollection<Complaint> ComplaintCreatedBies { get; set; } = new List<Complaint>();

    public virtual ICollection<Customer> CustomerAssignedTos { get; set; } = new List<Customer>();

    public virtual ICollection<Customer> CustomerCreatedBies { get; set; } = new List<Customer>();

    public virtual ICollection<DataImportsExport> DataImportsExports { get; set; } = new List<DataImportsExport>();

    public virtual Branch? DefaultBranch { get; set; }

    public virtual Department? Department { get; set; }

    public virtual ICollection<Department> Departments { get; set; } = new List<Department>();

    public virtual Employee? Employee { get; set; }

    public virtual ICollection<EmployeeAdvance> EmployeeAdvanceApprovedBies { get; set; } = new List<EmployeeAdvance>();

    public virtual ICollection<EmployeeAdvance> EmployeeAdvanceCreatedBies { get; set; } = new List<EmployeeAdvance>();

    public virtual ICollection<EmployeeAdvance> EmployeeAdvancePaidBies { get; set; } = new List<EmployeeAdvance>();

    public virtual ICollection<Employee> EmployeeCreatedBies { get; set; } = new List<Employee>();

    public virtual Employee? EmployeeUser { get; set; }

    public virtual ICollection<FiscalPeriod> FiscalPeriods { get; set; } = new List<FiscalPeriod>();

    public virtual ICollection<FixedAsset> FixedAssets { get; set; } = new List<FixedAsset>();

    public virtual ICollection<GoodsReceipt> GoodsReceiptCreatedBies { get; set; } = new List<GoodsReceipt>();

    public virtual ICollection<GoodsReceipt> GoodsReceiptInspectedBies { get; set; } = new List<GoodsReceipt>();

    public virtual ICollection<InventoryTransaction> InventoryTransactions { get; set; } = new List<InventoryTransaction>();

    public virtual ICollection<InventoryTransfer> InventoryTransferApprovedBies { get; set; } = new List<InventoryTransfer>();

    public virtual ICollection<InventoryTransfer> InventoryTransferCreatedBies { get; set; } = new List<InventoryTransfer>();

    public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

    public virtual ICollection<JournalEntry> JournalEntryApprovedBies { get; set; } = new List<JournalEntry>();

    public virtual ICollection<JournalEntry> JournalEntryCreatedBies { get; set; } = new List<JournalEntry>();

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<MaterialConsumption> MaterialConsumptions { get; set; } = new List<MaterialConsumption>();

    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();

    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    public virtual ICollection<ProductionOrder> ProductionOrderCompletedBies { get; set; } = new List<ProductionOrder>();

    public virtual ICollection<ProductionOrder> ProductionOrderCreatedBies { get; set; } = new List<ProductionOrder>();

    public virtual ICollection<ProductionOrder> ProductionOrderStartedBies { get; set; } = new List<ProductionOrder>();

    public virtual ICollection<ProductionOrder> ProductionOrderSupervisors { get; set; } = new List<ProductionOrder>();

    public virtual ICollection<ProductionStage> ProductionStages { get; set; } = new List<ProductionStage>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();

    public virtual ICollection<PurchaseOrder> PurchaseOrderApprovedBies { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<PurchaseOrder> PurchaseOrderCreatedBies { get; set; } = new List<PurchaseOrder>();

    public virtual ICollection<Quotation> Quotations { get; set; } = new List<Quotation>();

    public virtual Role Role { get; set; } = null!;

    public virtual ICollection<StockCountAdjustment> StockCountAdjustments { get; set; } = new List<StockCountAdjustment>();

    public virtual ICollection<StockCount> StockCountApprovedBies { get; set; } = new List<StockCount>();

    public virtual ICollection<StockCountItem> StockCountItems { get; set; } = new List<StockCountItem>();

    public virtual ICollection<StockCount> StockCountStartedBies { get; set; } = new List<StockCount>();

    public virtual ICollection<Supplier> Suppliers { get; set; } = new List<Supplier>();

    public virtual ICollection<SystemSetting> SystemSettings { get; set; } = new List<SystemSetting>();

    public virtual ICollection<Warehouse> Warehouses { get; set; } = new List<Warehouse>();
}
