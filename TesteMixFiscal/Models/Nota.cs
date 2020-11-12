using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TesteMixFiscal.Models
{
    [Table("tbl_Nota")]
    public class Nota
    {
        
        [Key]
        public int NItemId { get; set; }

        public string Descricao { get; set; }

        public string CodItem { get; set; }

        public string DescCompl { get; set; }

        public long? Ean { get; set; }

        public int Qtd { get; set; }

        public decimal VlItem { get; set; }

        [NotMapped]
        public int Vinculo { get; set; }

        #region ForignKey

        [ForeignKey("Tipo")]
        public int TipoId { get; set; }

        public virtual Tipo Tipo { get; set; }

        #endregion
    }
}