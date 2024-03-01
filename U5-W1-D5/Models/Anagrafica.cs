using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace U5_W1_D5.Models
{
    public class Anagrafica
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Indirizzo { get; set; }
        public string Citta { get; set; }
        public string Cap { get; set; }
        public string CodiceFiscale { get; set; }


        public int TotaleVerbali { get; set; }
    }
}