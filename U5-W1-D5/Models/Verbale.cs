using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace U5_W1_D5.Models
{
    public class Verbale
    {
        public int Id { get; set; }
        public DateTime Data { get; set; }
        public string Indirizzo { get; set; }
        public string Agente { get; set; }
        public DateTime DataVerbale { get; set; }
        public decimal Importo { get; set; }
        public int PuntiDecurtati { get; set; }
        public int idanagrafica { get; set; }
        public int idviolazione { get; set; }

        public int TotaleVerbaliPerAnagrafe { get; set; }

    }
}