using Microsoft.EntityFrameworkCore;

namespace Impactabank.Models
{
    public class BancoContext : DbContext
    {

        public DbSet<Cliente> Clientes { get; set; }

        public DbSet<Conta> Contas { get; set; }

        public DbSet<Municipio> Municipios { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                    .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                    .AddJsonFile("appsettings.json")
                    .Build();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("impactabank"));
        }
    }
}
