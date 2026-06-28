using LagalerieFurniture.Models;

namespace LagalerieFurniture.Services;

/// <summary>
/// خدمة القائمة الجانبية الديناميكية المبنية على صلاحيات المستخدم من قاعدة البيانات
/// </summary>
public interface IMenuService
{
    /// <summary>
    /// جلب مجموعات القائمة (الموديولات) المتاحة للمستخدم بناءً على صلاحياته
    /// </summary>
    Task<List<NavMenuGroup>> GetUserMenuAsync(int userId);

    /// <summary>
    /// حفظ حالة القائمة (مصغرة/موسعة) في localStorage
    /// </summary>
    Task<bool> GetSidebarMiniModeAsync();
    
    /// <summary>
    /// تعيين حالة القائمة
    /// </summary>
    Task SetSidebarMiniModeAsync(bool isMini);

    /// <summary>
    /// جلب عدد الإشعارات غير المقروءة للمستخدم
    /// </summary>
    Task<int> GetUnreadNotificationCountAsync(int userId);
}
