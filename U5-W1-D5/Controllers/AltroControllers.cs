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
        // Metodo per visualizzare i verbali totale ordinati per trasgressore
        public ActionResult VerbaliPerTrasgressore()
        {
            // Connessione al database
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Punti> VerbaliPerTrasgressore = new List<Punti>();

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
                    Punti punti = new Punti();
                    punti.Nome = reader["Nome"].ToString();
                    punti.Cognome = reader["Cognome"].ToString();
                    punti.TotaleVerbali = Convert.ToInt32(reader["TotaleVerbali"]);
                    VerbaliPerTrasgressore.Add(punti);
                }

            }
            catch (Exception ex)
            {
                Response.Write("Error: ");
                Response.Write(ex.Message); ;
            }
            finally
            {
                conn.Close();
            }

            return View(VerbaliPerTrasgressore);
        }


        // Metodo per visualizzare i punti decurtati ordinati per trasgressore
        public ActionResult Puntidecurtati()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Polizia"].ConnectionString.ToString();
            SqlConnection conn = new SqlConnection(connectionString);
            List<Punti> puntiDecurtati = new List<Punti>();
            try
            {
                conn.Open();
                string query = "Select a.Nome, a.Cognome, Sum(b.DecurtamentoPunti) as PuntiDecurtati from DatiAnagrafici a, Verbali b where a.IDAnagrafica = b.IDAnagrafica group by a.Nome, a.Cognome";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    Punti punti = new Punti();
                    punti.Nome = reader["Nome"].ToString();
                    punti.Cognome = reader["Cognome"].ToString();
                    punti.PuntiDecurtati = Convert.ToInt32(reader["PuntiDecurtati"]);
                    puntiDecurtati.Add(punti);
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine("Errore SQL: " + ex.Message);
            }
            finally
            {
                conn.Close();
            }
            return View(puntiDecurtati);
        }


        // Metodo per visualizzare i verbali con decurtamento punti maggiore di 10
        public ActionResult GetVerbaliDecurtamentoPuntiMaggioreDi10()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["Polizia"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            List<Verbale> verbali = new List<Verbale>();

            try
            {
                connection.Open();
                string query = "SELECT * from Verbali where DecurtamentoPunti > 2";
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    Verbale verbale = new Verbale();
                    verbale.Id = Convert.ToInt32(reader["idverbale"]);
                    verbale.Data = Convert.ToDateTime(reader["DataViolazione"]);
                    verbale.Indirizzo = reader["IndirizzoViolazione"].ToString();
                    verbale.Agente = reader["Nominativo_Agente"].ToString();
                    verbale.DataVerbale = Convert.ToDateTime(reader["DataTrascrizioneVerbale"]);
                    verbale.Importo = Convert.ToDecimal(reader["Importo"]);
                    verbale.PuntiDecurtati = Convert.ToInt32(reader["DecurtamentoPunti"]);
                    verbale.idanagrafica = Convert.ToInt32(reader["idanagrafica"]);
                    verbale.idviolazione = Convert.ToInt32(reader["idviolazione"]);
                    verbali.Add(verbale);
                }
            }
            catch (SqlException ex)
            {
                Response.Write("Error: ");
                Response.Write(ex.Message);
            }
            finally
            {
                connection.Close();
            }
            return View(verbali);
        }

        // Metodo per visualizzare i verbali con importo maggiore di 400
        public ActionResult GetVerbaleSopra400()
        {
            List<Verbale> verbali = new List<Verbale>();
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))

                try
                {
                    conn.Open();
                    string query = "SELECT * FROM VERBALE WHERE Importo > 400";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Verbale verbale = new Verbale();
                        verbale.Id = Convert.ToInt32(reader["idverbale"]);
                        verbale.Data = Convert.ToDateTime(reader["DataViolazione"]);
                        verbale.Indirizzo = reader["IndirizzoViolazione"].ToString();
                        verbale.Agente = reader["Nominativo_Agente"].ToString();
                        verbale.DataVerbale = Convert.ToDateTime(reader["DataTrascrizioneVerbale"]);
                        verbale.Importo = Convert.ToDecimal(reader["Importo"]);
                        verbale.PuntiDecurtati = Convert.ToInt32(reader["DecurtamentoPunti"]);
                        verbale.idanagrafica = Convert.ToInt32(reader["idanagrafica"]);
                        verbale.idviolazione = Convert.ToInt32(reader["idviolazione"]);
                        verbali.Add(verbale);
                    }
                    reader.Close();
                }
                catch (Exception ex)
                {
                    Response.Write("Error: ");
                    Response.Write(ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            return View(verbali);
        }
    }
}