using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Threading.Tasks;

namespace TesteSqlite
{
    class Program
    {
        static string database = "dbsqlite.db";
        static async Task Main(string[] args)
        {
            using(var db = new DatabaseDbContext())
            {
                await db.Database.EnsureCreatedAsync();
                var cm1 = new Camiao()
                {

                    Marca = "Volvo",
                    Modelo = "FH12"
                };
                var cm2 = new Camiao()
                {
                    Marca = "Man",
                    Modelo = "EFline"
                };
                db.Camioes.Add(cm1);
                db.Camioes.Add(cm2);
                await db.SaveChangesAsync();
                var gn1 = new Matricula()
                {
                     CamiaoID=1,
                      matricula1="49-64-xe",
                       DataMatricula= DateTime.Now
                };
                //string horas = "12:05";
                //DateTime datex = DateTime.ParseExact(horas, "dd/MM/YYYY HH:mm:ss", CultureInfo.InvariantCulture);
                DateTime novadata = DateTime.Parse("01/02/11");
                TimeSpan _time = TimeSpan.Parse("07:35");
                var gn2 = new Matricula()
                {
                    CamiaoID = 2,
                    matricula1 = "64-49-EX",
                    DataMatricula = novadata + _time
                };
                db.Matriculas.Add(gn1);
                await db.SaveChangesAsync();
                db.Matriculas.Add(gn2);
                await db.SaveChangesAsync();

                
                Console.WriteLine("Vamos mostrar agora os resultados");
                List<Camiao> camioes = await db.Camioes.ToListAsync();
                Console.WriteLine("Lista de camioes");
                foreach (var cm in camioes)
                {
                    Console.WriteLine($"Id:{cm.id}\nVarName:{cm.Marca}\nValue:{cm.Modelo}");
                }

                List<Matricula> matriculas = await db.Matriculas.ToListAsync();
                Console.WriteLine("Lista de Matriculas");
                foreach(var gn in matriculas)
                {
                    Console.WriteLine($"ID:{gn.ID}\nMatricula:{gn.matricula1}\nNova:{gn.DataMatricula}\nCamiaoID:{gn.CamiaoID}");
                }
                
                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }




        

        public class Camiao
        {
            public int id { get; set; }
            public string Marca { get; set; }
            public string Modelo { get; set; }
            public virtual ICollection<Matricula> Matriculas { get; set; }
            //public ICollection<GenericClass> genericClasses1 { get; set; }
            //public GenericClass genericClass { get; set; }
        }

        public class Matricula
        {
            public int ID { get; set; }
            public string matricula1 { get; set; }
            public DateTime DataMatricula { get; set; }
            public int CamiaoID { get; set; }
            public virtual Camiao Camioes { get; set; }
            //public ICollection<Camiao> Camioes { get; set; }

        }

        public class DatabaseDbContext : DbContext
        {
            public DbSet<Matricula> Matriculas { get; set; }
            public DbSet<Camiao> Camioes { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                optionsBuilder.UseSqlite(connectionString: "Filename=" + database,
                    sqliteOptionsAction: op =>
                    {
                        op.MigrationsAssembly(
                            Assembly.GetExecutingAssembly().FullName);
                    });
                    

                base.OnConfiguring(optionsBuilder);
            }

            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.Entity<Camiao>().ToTable("Camioes");
                modelBuilder.Entity<Camiao>(entity =>
                {
                    entity.HasKey(e => e.id);
                });

                modelBuilder.Entity<Matricula>().ToTable("Matricula");
                modelBuilder.Entity<Matricula>(entity =>
                {
                    entity.HasKey(e => e.ID);
                    entity.HasOne(d => d.Camioes)
                    .WithMany(p => p.Matriculas)
                    .HasForeignKey(d => d.CamiaoID)
                    .HasConstraintName("FK_Generic_ID");
                });
                base.OnModelCreating(modelBuilder);
            }
        }
    }
}
