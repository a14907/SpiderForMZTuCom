using Microsoft.EntityFrameworkCore;

namespace SpiderForMZTuCom
{
    public class MyDbContext:DbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> opt) :base(opt)
        {
            
        } 
        public DbSet<FileLocNames> FileLocNames { get; set; }
    }
     

    public class FileLocNames
    {
        public int Id { get; set; }
        public string CollectionName { get; set; }
        public string CollectionUrl { get; set; }
    }
}
