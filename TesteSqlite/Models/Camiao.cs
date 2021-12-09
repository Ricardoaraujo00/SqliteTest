using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;

namespace TesteSqlite.Models
{
    public class Camiao
    {
        public int Id { get; set; }
        public string Marca_nome { get; set; }
        public string Modelo { get; set; }
        public virtual ICollection<Matricula> Matriculas { get; set; }

    }
}
