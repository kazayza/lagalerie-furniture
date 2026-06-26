using System;
using System.Collections.Generic;

namespace LagalerieFurniture.Models;

public partial class Employee
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string EmployeeCode { get; set; } = null!;

    public string? FingerprintId { get; set; }

    public string? CardId { get; set; }

    public string? NationalId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public DateOnly? BirthDate { get; set; }

    public string? Gender { get; set; }

    public string? MaritalStatus { get; set; }

    public string Phone { get; set; } = null!;

    public string? EmergencyContact { get; set; }

    public string? EmergencyPhone { get; set; }

    public string? Address { get; set; }

    public DateOnly HireDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int DepartmentId { get; set; }

    public string JobTitle { get; set; } = null!;

    public string EmploymentType { get; set; } = null!;

    public decimal BaseSalary { get; set; }

    public string? BankName { get; set; }

    public string? BankAccountNumber { get; set; }

    public string Status { get; set; } = null!;

    public string? Notes { get; set; }

    public int BranchId { get; set; }

    public bool IsDeleted { get; set; }

    public DateTime? DeletedAt { get; set; }

    public int? DeletedById { get; set; }

    public int? CreatedById { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Attendance> Attendances { get; set; } = new List<Attendance>();

    public virtual Branch Branch { get; set; } = null!;

    public virtual User? CreatedBy { get; set; }

    public virtual Department Department { get; set; } = null!;

    public virtual ICollection<EmployeeAdvance> EmployeeAdvances { get; set; } = new List<EmployeeAdvance>();

    public virtual ICollection<EmployeeShift> EmployeeShifts { get; set; } = new List<EmployeeShift>();

    public virtual ICollection<LeaveBalance> LeaveBalances { get; set; } = new List<LeaveBalance>();

    public virtual ICollection<LeaveRequest> LeaveRequests { get; set; } = new List<LeaveRequest>();

    public virtual ICollection<Payroll> Payrolls { get; set; } = new List<Payroll>();

    public virtual ICollection<RawAttendanceLog> RawAttendanceLogs { get; set; } = new List<RawAttendanceLog>();

    public virtual User? User { get; set; }

    public virtual User? UserNavigation { get; set; }
}
