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
    public class HomeController : Controller
    {
        List<Anagrafica> anagrafiche = new List<Anagrafica>();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetTrasgressore()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))

                try
                {
                    conn.Open();
                    string query = "SELECT * FROM ANAGRAFICA";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        Anagrafica anagrafica = new Anagrafica();
                        anagrafica.Id = Convert.ToInt32(reader["idAnagrafica"]);
                        anagrafica.Nome = reader["Nome"].ToString();
                        anagrafica.Cognome = reader["Cognome"].ToString();
                        anagrafica.Indirizzo = reader["Indirizzo"].ToString();
                        anagrafica.Citta = reader["Città"].ToString();
                        anagrafica.Cap = reader["Cap"].ToString();
                        anagrafica.CodiceFiscale = reader["Cod_Fisc"].ToString();
                        anagrafiche.Add(anagrafica);
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



            return View(anagrafiche);
        }

        public ActionResult CreateTrasgressore(Anagrafica trasgressore)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))


                try
                {
                    conn.Open();
                    SqlCommand command = new SqlCommand();
                    command.Parameters.AddWithValue("@Cognome", trasgressore.Cognome);
                    command.Parameters.AddWithValue("@Nome", trasgressore.Nome);
                    command.Parameters.AddWithValue("@Indirizzo", trasgressore.Indirizzo);
                    command.Parameters.AddWithValue("@Città", trasgressore.Citta);
                    command.Parameters.AddWithValue("@Cap", trasgressore.Cap);
                    command.Parameters.AddWithValue("@Cod_Fisc", trasgressore.CodiceFiscale);

                    command.CommandText = "Insert into ANAGRAFICA values (@Cognome, @Nome, @Indirizzo, @Città, @Cap, @Cod_Fisc)";
                    command.Connection = conn;
                    command.ExecuteNonQuery();

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
            return View(anagrafiche);
        }


        /*public ActionResult EditTrasgressore(int idTrasgressore, Anagrafica trasgressore)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE ANAGRAFICA SET Nome = @Nome, Cognome = @Cognome, Indirizzo = @Indirizzo, Città = @Città, Cap = @Cap, Cod_Fisc = @Cod_Fisc " +
                                   "WHERE idanagrafica = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Nome", trasgressore.Nome);
                    cmd.Parameters.AddWithValue("@Cognome", trasgressore.Cognome);
                    cmd.Parameters.AddWithValue("@Indirizzo", trasgressore.Indirizzo);
                    cmd.Parameters.AddWithValue("@Città", trasgressore.Citta);
                    cmd.Parameters.AddWithValue("@Cap", trasgressore.Cap);
                    cmd.Parameters.AddWithValue("@Cod_Fisc", trasgressore.CodiceFiscale);
                    cmd.ExecuteNonQuery();
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
            }
            return View(anagrafiche);
        }*/

        public ActionResult EditTrasgressore(int idTrasgressore)
        {
           
            try
            {
                // Recupera la stringa di connessione dal file di configurazione
                string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString;

                // Apri una connessione al database
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    conn.Open();

                    // Costruisci la query SQL per selezionare il trasgressore con l'ID specificato
                    string query = "SELECT * FROM ANAGRAFICA WHERE idAnagrafica = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", idTrasgressore);

                    // Esegui la query per ottenere il trasgressore da modificare
                    SqlDataReader reader = cmd.ExecuteReader();

                    // Verifica se è stato trovato un trasgressore con l'ID specificato
                    if (reader.Read())
                    {
                        // Costruisci un oggetto Anagrafica con i dati del trasgressore trovato
                        Anagrafica trasgressoreModificato = new Anagrafica
                        {
                            Id = Convert.ToInt32(reader["idAnagrafica"]),
                            Nome = reader["Nome"].ToString(),
                            Cognome = reader["Cognome"].ToString(),
                            Indirizzo = reader["Indirizzo"].ToString(),
                            Citta = reader["Città"].ToString(),
                            Cap = reader["Cap"].ToString(),
                            CodiceFiscale = reader["Cod_Fisc"].ToString()
                            // Assicurati di aggiungere altre proprietà se necessario
                        };

                        // Chiudi il reader
                        reader.Close();

                        // Passa il trasgressore trovato alla vista per la modifica
                        return View(trasgressoreModificato);
                    }
                    else
                    {
                        // Se non è stato trovato alcun trasgressore con l'ID specificato, restituisci un errore o reindirizza a una pagina di gestione degli errori
                        return HttpNotFound(); // Ad esempio, restituisci un errore 404 (Risorsa non trovata)
                    }
                }
            }
            catch (Exception ex)
            {
                // Gestisci l'eccezione in qualche modo appropriato
                return Content("Si è verificato un errore durante la modifica del trasgressore: " + ex.Message);
            }
        }




        /*public ActionResult DeleteTrasgressore(int trasgressore)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM ANAGRAFICA WHERE idAnagrafica = @Id";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@Id", trasgressore);
                    cmd.ExecuteNonQuery();
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
            }

            
            return RedirectToAction("GetTrasgressore");
        }*/



        List<Verbale> verbali = new List<Verbale>();

        public ActionResult GetVerbale()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))

                try
                {
                    conn.Open();
                    string query = "SELECT * FROM VERBALE";
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



        public ActionResult CreateVerbale(Verbale verbale)
        {
            if (ModelState.IsValid)
            {
                
                string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    try
                    {
                        conn.Open();
                        string query = "INSERT INTO VERBALE (DataViolazione, IndirizzoViolazione, Nominativo_Agente, DataTrascrizioneVerbale, Importo, DecurtamentoPunti, idanagrafica, idviolazione) VALUES (@DataViolazione, @IndirizzoViolazione, @Nominativo_Agente, @DataTrascrizioneVerbale, @Importo, @DecurtamentoPunti, @idanagrafica, @idviolazione)";
                        SqlCommand cmd = new SqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@DataViolazione", verbale.Data);
                        cmd.Parameters.AddWithValue("@IndirizzoViolazione", verbale.Indirizzo);
                        cmd.Parameters.AddWithValue("@Nominativo_Agente", verbale.Agente);
                        cmd.Parameters.AddWithValue("@DataTrascrizioneVerbale", verbale.DataVerbale);
                        cmd.Parameters.AddWithValue("@Importo", verbale.Importo);
                        cmd.Parameters.AddWithValue("@DecurtamentoPunti", verbale.PuntiDecurtati);
                        cmd.Parameters.AddWithValue("@idanagrafica", verbale.idanagrafica);
                        cmd.Parameters.AddWithValue("@idviolazione", verbale.idviolazione);
                        cmd.ExecuteNonQuery();
                        return RedirectToAction("Index"); 
                    }
                    catch (Exception ex)
                    {
                        ViewBag.ErrorMessage = "Errore durante il salvataggio dei dati: " + ex.Message;
                    }
                }
            }
            return View(verbale);
        }

        List<TipoViolazione> violazioni = new List<TipoViolazione>();

        public ActionResult GetViolazioni()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyDb"].ConnectionString.ToString();
            using (SqlConnection conn = new SqlConnection(connectionString))

                try
                {
                    conn.Open();
                    string query = "SELECT * FROM TIPO_VIOLAZIONE";
                    SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataReader reader = cmd.ExecuteReader();

                    while (reader.Read())
                    {
                        TipoViolazione violazione = new TipoViolazione();
                        violazione.idviolazione = Convert.ToInt32(reader["idviolazione"]);
                        violazione.Descrizione = reader["descrizione"].ToString();
                        violazioni.Add(violazione);
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
            return View(violazioni);
        }


        public ActionResult AltreOperazioni()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}