using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using TesteSqlite.Models;
using static TesteSqlite.Program;

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

                    Marca_nome = "Volvo",
                    Modelo = "FH12"
                };
                var cm2 = new Camiao()
                {
                    Marca_nome = "Man",
                    Modelo = "EFline"
                };
                PropertyInfo[] properties = typeof(Camiao).GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    Console.WriteLine("property.Name:" + property.Name);
                    Console.WriteLine("property.GetValue(cm1):" + property.GetValue(cm1));
                    if (property.Name == "Marca_nome")
                    {
                        property.SetValue(cm1, "DAF");
                    }
                }
                db.Camioes.Add(cm1);
                await db.SaveChangesAsync();
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
                    Console.WriteLine($"Id:{cm.Id}\nVarName:{cm.Marca_nome}\nValue:{cm.Modelo}");
                }

                List<Matricula> matriculas = await db.Matriculas.ToListAsync();
                Console.WriteLine("Lista de Matriculas");
                foreach(var gn in matriculas)
                {
                    Console.WriteLine($"ID:{gn.Id}\nMatricula:{gn.matricula1}\nNova:{gn.DataMatricula}\nCamiaoID:{gn.CamiaoID}");
                }


                string mynamespace2 = "TesteSqlite.Models";
                var q = from t in Assembly.GetExecutingAssembly().GetTypes()
                        where t.IsClass && t.Namespace == mynamespace2
                        select t;
                string classname2 = "Camiao";
                Type tipo = q.FirstOrDefault(x => x.Name == classname2);
                if (tipo == null)
                {
                    throw new Exception("Type not found.");
                }
                var instance = Activator.CreateInstance(tipo);
                //or
                var newClass = System.Reflection.Assembly.GetAssembly(tipo).CreateInstance(tipo.FullName);

                PropertyInfo[] newClassProperties = tipo.GetProperties();
                foreach (PropertyInfo property in newClassProperties)
                {
                    property.SetValue(newClass, property.GetValue(cm1));
                    var x = property.PropertyType;
                    if (x.Name== "ICollection`1")
                    {
                        property.SetValue(newClass, null);
                    }
                    
                    if (property.Name == "Id")
                    {
                        property.SetValue(newClass, null);
                    }
                    if (property.Name == "Marca_nome")
                    {
                        property.SetValue(newClass, "Marca do caralhão");
                    }
                   
                    Console.WriteLine("property.GetValue(cm1):" + property.GetValue(newClass));

                }

                Castt(newClass);


                //#####################

                string classname23 = "Matricula";
                Type tipo3 = q.FirstOrDefault(x => x.Name == classname23);
                if (tipo3 == null)
                {
                    throw new Exception("Type not found.");
                }
                var instance3 = Activator.CreateInstance(tipo3);
                //or
                var newClass3 = System.Reflection.Assembly.GetAssembly(tipo3).CreateInstance(tipo3.FullName);

                PropertyInfo[] newClassProperties3 = tipo3.GetProperties();
                foreach (PropertyInfo property in newClassProperties3)
                {
                    property.SetValue(newClass3, property.GetValue(gn1));
                    var x = property.PropertyType;
                    if (x.BaseType== typeof(object))
                    {
                        property.SetValue(newClass3, null );
                    }
                    
                    if (property.Name == "Id")
                    {
                        property.SetValue(newClass3, null);
                    }
                    if (property.Name == "matricula1")
                    {
                        property.SetValue(newClass3, "Matricula do caralhão");
                    }
                    if (property.Name == "Camioes")
                    {
                        property.SetValue(newClass3, null);
                    }

                    Console.WriteLine("property.GetValue(cm1):" + property.GetValue(newClass3));

                }

                Castt(newClass3);

                //PropertyInfo[] properties2 = typeof(Camiao).GetProperties();
                //PropertyInfo[] properties2 = tipo.GetProperties();
                //foreach (PropertyInfo property in properties2)
                //{                    


                //    Console.WriteLine("property.Name:" + property.Name);
                //    Console.WriteLine("property.GetValue(cm1):" + property.GetValue(cm1));
                //    if (property.Name == "Marca")
                //    {
                //        property.SetValue(cm1, "DAF");
                //    }
                //}


                Console.WriteLine("Press any key to exit");
                Console.ReadKey();
            }
        }

        

        public static void Castt (object data)
        {
            PropertyInfo[] ptinf = data.GetType().GetProperties();
            Type tp = data.GetType();
            //var novaclasstosave = System.Reflection.Assembly.GetAssembly(tp).CreateInstance(tp.FullName);
            ContextSave(data);

        }

        public static void ContextSave<T>(T data)
        {
            using (var db = new DatabaseDbContext())
            {
                db.Add(data);
                db.SaveChanges();
            }
            Console.WriteLine(data.ToString());
        }

        //private IEnumerable<TEntity> GetList<TEntity>(DbContext ctx, Func<object, T> caster)
        //{

        //        var setMethod = ctx.GetType().GetMethod("Set").MakeGenericMethod(typeof(T));

        //        var querable = ((DbSet<object>)setMethod
        //        .Invoke(this, null))
        //        .AsNoTracking()
        //        .AsQueryable();

        //        return querable
        //            .Select(x => caster(x))
        //            .ToList();

        //}


        public class Action
        {
            public string key { get; set; }
            public string Value { get; set; }
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
                //modelBuilder.Entity<Camiao>(entity =>
                //{
                //    entity.HasKey(e => e.Id);
                //});

                modelBuilder.Entity<Matricula>().ToTable("Matricula");
                modelBuilder.Entity<Matricula>(entity =>
                {
                    //entity.HasKey(m => m.Id);
                    entity.HasOne(m => m.Camioes)
                    .WithMany(c => c.Matriculas)
                    .HasForeignKey(m => m.CamiaoID)
                    .HasConstraintName("FK_Generic_ID");
                });
                base.OnModelCreating(modelBuilder);
            }
        }
    }

   

    
}
