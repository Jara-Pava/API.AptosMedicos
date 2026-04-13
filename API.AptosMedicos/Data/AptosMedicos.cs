using Microsoft.EntityFrameworkCore;

namespace API.AptosMedicos.Data;

public class AptosMedicosDbContext : DbContext
{
    public AptosMedicosDbContext(DbContextOptions<AptosMedicosDbContext> options)
        : base(options)
    {
    }
}