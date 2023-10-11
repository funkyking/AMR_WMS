using Microsoft.EntityFrameworkCore;

namespace WMS_Web_API
{
    public class ApiContext : DbContext
    {

        public ApiContext(DbContextOptions<ApiContext> options)
            :base(options)
        {
        }

        public DbSet<Tasks> Tasks { get; set; } = null!;
    }
}
