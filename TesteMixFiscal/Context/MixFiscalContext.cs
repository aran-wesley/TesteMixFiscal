using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using TesteMixFiscal.Models;

namespace TesteMixFiscal.Context
{
    public class MixFiscalContext : DbContext
    {
        public MixFiscalContext() : base("Name=MixFiscal")
        {
            Database.SetInitializer<MixFiscalContext>(new CreateDatabaseIfNotExists<MixFiscalContext>());

            Database.Initialize(false);
        }

        public DbSet<Nota> Nota { get; set; }

        public DbSet<Tipo> Tipo { get; set; }
    }
}