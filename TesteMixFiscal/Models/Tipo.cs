using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TesteMixFiscal.Models
{
    [Table("tbl_Tipo")]
    public class Tipo
    {
        [Key]
        public int TipoId { get; set; }

        public string Nome { get; set; }

        
    }

}