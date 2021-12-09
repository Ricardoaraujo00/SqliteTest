using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TesteSqlite.Models
{
    public class Matricula
    {
        
        public int Id { get; set; }
        public string matricula1 { get; set; }
        public DateTime DataMatricula { get; set; }
        public int CamiaoID { get; set; }
        public virtual Camiao Camioes { get; set; }
    }
}
