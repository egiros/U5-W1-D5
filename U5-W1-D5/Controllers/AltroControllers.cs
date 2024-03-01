using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using U5_W1_D5.Models;

namespace U5_W1_D5.Controllers
{
    public class AltroControllers : Controller
    {
        public ActionResult TotalVerbaliPerTrasgressore()
        {
            List<Verbale> Verbali = new List<Verbale>();
            // Connessione al database
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
                try
                {
                    conn.Open();

                    string query = @"SELECT idanagrafica, COUNT(*) AS TotaleVerbali
                         FROM VERBALE
                         GROUP BY idanagrafica
                         ORDER BY COUNT(*) DESC";

                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Verbale verbale = new Verbale();
                        verbale.idanagrafica = Convert.ToInt32(reader["idanagrafica"]);
                        Trasgressore trasgressore = Trasgressore.MostraTrasgressori().FirstOrDefault(t => t.idanagrafica == verbale.idanagrafica);
                        verbale.Trasgressore = trasgressore;
                        verbale.Id = Convert.ToInt32(reader["TotaleVerbaliXAnagrafe"]);
                        Verbali.Add(verbale);
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Errore SQL: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }

            return View(anagrafiche);
        }
    }


        //public ActionResult TotalPuntiDecurtatiPerTrasgressore()
        //{
        //    // codice per ottenere il totale dei punti decurtati raggruppati per trasgressore
        //    return View();
        //}

        //public ActionResult ViolazioniConPuntiDecurtatiSopraSoglia()
        //{
        //    // codice per ottenere le violazioni con più di 10 punti decurtati
        //    return View();
        //}

        //public ActionResult ViolazioniConImportoSopraSoglia()
        //{
        //    // codice per ottenere le violazioni con importo maggiore di 400 euro
        //    return View();
        //}

    
}