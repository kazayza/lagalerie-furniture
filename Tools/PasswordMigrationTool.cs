using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace LagalerieFurniture.Tools;

/// <summary>
/// أداة سطر أوامر لتشفير كلمات المرور النصية الموجودة في قاعدة البيانات.
///
/// طريقة التشغيل (من مجلد المشروع):
///   dotnet run --project LagalerieFurniture.csproj -- migrate-passwords
///
/// تقوم الأداة بالآتي:
///   1. تجيب كل المستخدمين
///   2. اللي باسورده مش BCrypt (نص صريح قديم) → تشفّره وتحفظه
///   3. تطبع تقرير بكم واحد اتشفّر
///
/// ملاحظة: تعمل على نفس connection string في appsettings.json.
/// يُنصح بأخذ نسخة احتياطية من الـ Users قبل التشغيل.
/// </summary>
public static class PasswordMigrationTool
{
    public static async Task<int> RunAsync(string[] args)
    {
        if (args.Length == 0 || args[0] != "migrate-passwords")
        {
            Console.WriteLine("الاستخدام: dotnet run -- migrate-passwords");
            return 0; // لا تنفّذ شيئاً عند التشغيل العادي للسيرفر
        }

        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((_, services) =>
            {
                // إعادة بناء نفس تسجيلات DbContext + PasswordHasher
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", optional: false)
                    .Build();

                services.AddDbContext<Data.ApplicationDbContext>(options =>
                    options.UseSqlServer(config.GetConnectionString("DefaultConnection")));
                services.AddSingleton<Services.IPasswordHasher, Services.PasswordHasher>();
            })
            .Build();

        Console.WriteLine("=== أداة تشفير كلمات المرور ===");
        Console.WriteLine("جارٍ الاتصال بقاعدة البيانات...\n");

        try
        {
            using var scope = host.Services.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<Data.ApplicationDbContext>();
            var hasher = scope.ServiceProvider.GetRequiredService<Services.IPasswordHasher>();

            var users = await db.Users.ToListAsync();
            Console.WriteLine($"إجمالي المستخدمين: {users.Count}");

            int alreadyHashed = 0;
            int migrated = 0;

            foreach (var user in users)
            {
                if (hasher.IsBCryptHash(user.PasswordHash))
                {
                    alreadyHashed++;
                    continue;
                }

                // باسورد نصي قديم → نشفّره
                user.PasswordHash = hasher.Hash(user.PasswordHash);
                migrated++;
            }

            if (migrated > 0)
            {
                await db.SaveChangesAsync();
            }

            Console.WriteLine($"\nالنتيجة:");
            Console.WriteLine($"  ✅ تم تشفيرها الآن:  {migrated}");
            Console.WriteLine($"  ✓ كانت مشفّرة بالفعل: {alreadyHashed}");

            Console.WriteLine(migrated > 0
                ? "\n✅ تمت العملية بنجاح. جميع كلمات المرور الآن مشفّرة بـ BCrypt."
                : "\nℹ️ لا توجد كلمات مرور بحاجة للترقية — كلها مشفّرة بالفعل.");

            return 0;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n❌ خطأ: {ex.Message}");
            Console.WriteLine(ex.StackTrace);
            return 1;
        }
    }
}
