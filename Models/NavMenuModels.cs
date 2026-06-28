using MudBlazor;

namespace LagalerieFurniture.Models;

/// <summary>
/// مجموعة قائمة رئيسية (موديول) مع أبنائه
/// </summary>
public class NavMenuGroup
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public string DisplayName { get; set; } = null!;
    public string Icon { get; set; } = Icons.Material.Filled.Circle;
    public int SortOrder { get; set; }
    public bool IsExpanded { get; set; } = true;
    public List<NavMenuItem> Items { get; set; } = new();
}

/// <summary>
/// عنصر قائمة فردي (لينك)
/// </summary>
public class NavMenuItem
{
    public string Label { get; set; } = null!;
    public string Href { get; set; } = "/";
    public string Icon { get; set; } = Icons.Material.Filled.Circle;
    public string? PermissionCode { get; set; }
    public string? Badge { get; set; }
    public string? BadgeColor { get; set; }
    public bool IsActive { get; set; }
}

/// <summary>
/// حالة القائمة الجانبية
/// </summary>
public class SidebarState
{
    public bool IsOpen { get; set; } = true;
    public bool IsMiniMode { get; set; } = false;
    public string ActiveGroup { get; set; } = string.Empty;
    public string ActiveLink { get; set; } = string.Empty;
}
