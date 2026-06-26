using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Payroll
{
    public long Id { get; set; }

    public int EmployeeId { get; set; }

    public int PeriodYear { get; set; }

    public int PeriodMonth { get; set; }

    public decimal BaseSalary { get; set; }

    public decimal OvertimePay { get; set; }

    public decimal Bonus { get; set; }

    public decimal Allowances { get; set; }

    public decimal Deductions { get; set; }

    public decimal AdvanceDeductions { get; set; }

    public decimal TaxDeduction { get; set; }

    public decimal InsuranceDeduction { get; set; }

    public decimal NetSalary { get; set; }

    public DateTime? PaymentDate { get; set; }

    public string? PaymentMethod { get; set; }

    public string Status { get; set; } = null!;

    public int? ApprovedById { get; set; }

    public string? Notes { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual User? ApprovedBy { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
