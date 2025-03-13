using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Text;
using System.Timers; // folosesc un timer ca o data la o secunda sa trimit spre SQL mesajul pe care-l primesc din Form1()
using System.Messaging;
using System.Data.SqlClient;
using System.Collections;
using System.Configuration;

namespace MessagingService
{
    public partial class PrinteMessagingService : ServiceBase
    {
        Timer timer1 = new Timer();

        public string strQueueName = @".\Private$\MessageQueueMessageQueue";
        public MessageQueue mqQueue;

        #region connection string
        public string connectionString = ConfigurationManager.AppSettings["TestMessageQueueConnectionString"].ToString();
        #endregion connection string

        #region service methods
        public PrinteMessagingService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer1.Elapsed += new ElapsedEventHandler(timer1_Elapsed); // pornesc cronometrul si astept  sa vad daca am un mesaj pe coada
            // de la Form1
            timer1.Interval = 1000; // setez serviciul sa asculte coada de mesaje la o secunda
            timer1.Enabled = true;
            timer1.Start();

            // daca am un mesaj, trimit o interogare la SQL
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
        }

        /// <summary>
        /// apelata la trecerea tick-ului
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Elapsed(object sender, EventArgs e)
        {
            string receivedMessage = GetMessage();
            string[] splittedMessage = receivedMessage.Split(';');

            // vad daca am un mesaj pe coada
            if (!String.IsNullOrEmpty(receivedMessage) && receivedMessage.Contains("[CLIENT->SERVICIU]")) // daca mesajul de pe coada este de la client
            {
                // mesajul vine in forma nume;prenume;data_angajarii;

                string strNumeTrimis = splittedMessage[0]; // numele
                string strPrenumeTrimis = splittedMessage[1]; // prenumele
                string dtDataAngajariiTrimisa = splittedMessage[2];

                // verific rezultatul pe care mi l-a dat query-ul sql
                if (SendMessageToSql(strNumeTrimis, strPrenumeTrimis, dtDataAngajariiTrimisa) == 0)
                {
                    PutMessage(
                        "[SERVICIU->CLIENT]" // sensul mesajului
                        + ";" + strNumeTrimis
                        + ";" + strPrenumeTrimis
                        + ";" + dtDataAngajariiTrimisa + ";;" // daca nu am obtinut nici o eroare in sql, nu pun mesajul de eroare
                    );
                }

                else
                {
                    PutMessage(
                        "[SERVICIU->CLIENT]" // sensul mesajului
                        + ";" + strNumeTrimis
                        + ";" + strPrenumeTrimis
                        + ";" + dtDataAngajariiTrimisa + ";[EROARE];"
                    );
                }
            }
        }
        #endregion service methods

        #region sql methods
        /// <summary>
        /// trimite mesajul la Sql Server
        /// </summary>
        protected int SendMessageToSql(string strNume, string strPrenume, string strDataAngajarii)
        {
            // setez valorile parametrilor
            ArrayList alListaParametri = new ArrayList();
            alListaParametri.Add("@nume");
            alListaParametri.Add("@prenume");
            alListaParametri.Add("@data_angajarii");

            ArrayList alValoriParametri = new ArrayList();
            alValoriParametri.Add(strNume);
            alValoriParametri.Add(strPrenume);
            alValoriParametri.Add(strDataAngajarii);

            string strProcedureName = "sp_Angajati_Insert";

            // execut metoda de adaugare si intorc rezultatul ei 0 sau difderit de 0
            return GenericOperation(strProcedureName, alListaParametri, alValoriParametri);            
        }
        #endregion sql methods

        #region data methods

        /// <summary>
        /// intoarce o comanda generica de tip SqlCommand
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="strNumeProcedura"></param>
        /// <returns></returns>
        protected SqlCommand GetSqlCommand(string connectionString, string strNumeProcedura)
        {
            /////////////////////////////////
            int? ERC = 0;
            string MESSAGE = "";

            // Create a new data adapter based on the specified query.
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            SqlCommand sqlCommand = new SqlCommand(strNumeProcedura, sqlConnection);

            // transmit valorile parametrilor
            sqlCommand.CommandType = CommandType.StoredProcedure;
            sqlCommand.CommandText = strNumeProcedura;

            sqlCommand.Parameters.AddWithValue("@ERC", ERC).Direction = ParameterDirection.InputOutput;
            sqlCommand.Parameters.AddWithValue("@MESSAGE", MESSAGE).Direction = ParameterDirection.InputOutput;
            sqlCommand.Parameters["@MESSAGE"].Size = 4000;
            /////////////////////////////

            return sqlCommand;
        }        

        /// <summary>
        /// o metoda generica pentru adaugare
        /// </summary>
        /// <param name="strNumeProcedura"></param>
        /// <param name="alListaParametri"></param>
        /// <param name="alValoriParametri"></param>
        public int GenericOperation(string strNumeProcedura, ArrayList alListaParametri, ArrayList alValoriParametri)
        {
            SqlCommand sqlCommand = GetSqlCommand(connectionString, strNumeProcedura);

            /////////////
            int ID_MESAJ_EROARE_PROCEDURA = 0;
            string MESAJ_EROARE_PROCEDURA = null;

            // adaug parametrii si valorile
            for (int i = 0; i < alListaParametri.Count; i++)
            {
                sqlCommand.Parameters.AddWithValue(alListaParametri[i].ToString(), (alValoriParametri[i].ToString() == "") ? null : alValoriParametri[i]);
            }

            try
            {
                sqlCommand.ExecuteNonQuery();

                ID_MESAJ_EROARE_PROCEDURA = (int)sqlCommand.Parameters["@ERC"].Value;
                MESAJ_EROARE_PROCEDURA = sqlCommand.Parameters["@MESSAGE"].Value.ToString();
                //////////////////////////

                return ID_MESAJ_EROARE_PROCEDURA;
            }

            catch (Exception ex)
            {
                return ID_MESAJ_EROARE_PROCEDURA;
            }

            finally
            {
                if (sqlCommand.Connection.State == ConnectionState.Open)
                    sqlCommand.Connection.Close();
            }
        }
        #endregion data methods

        #region Queue
        /// <summary>
        /// creaza o coada de mesage. Aici vor ajunge mesajele din GUI
        /// </summary>
        /// <param name="queueName"></param>
        private void CreateQueue(string queueName)
        {
            try
            {
                if (MessageQueue.Exists(queueName)) // verific daca exista coada de mesaje
                    mqQueue = new MessageQueue(queueName); // daca exista, o folosesc
                else
                    mqQueue = MessageQueue.Create(queueName); // daca nu exista, o creez
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// pune un mesaj in coada
        /// </summary>
        /// <param name="strMessage"></param>
        private void PutMessage(string strMessage)
        {
            try
            {
                System.Messaging.Message mqMessage = new System.Messaging.Message();
                mqMessage.Body = strMessage;
                mqMessage.Label = "MessageQueue message: ";
                mqQueue.Send(mqMessage);
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// incearca sa citeasca un mesaj din coada
        /// </summary>
        private string GetMessage()
        {
            System.Messaging.Message mqMessage = new System.Messaging.Message();

            string returnedMessage = "";

            try
            {
                mqMessage = mqQueue.Receive(new TimeSpan(0, 0, 1)); // asteapta o secunda dupa mesaj. Daca nu-l gaseste, arunca eroare
                mqMessage.Formatter = new XmlMessageFormatter(new String[] { "System.String,mscorlib" });
                returnedMessage = mqMessage.Body.ToString();
            }

            catch (Exception ex)
            {
                returnedMessage = "";
            }

            return returnedMessage;

        }
        #endregion Queue

        #region Format messages
        /// <summary>
        /// metoda trimite catre serviciul windows MessageQueueMessagingService un mesaj de forma:
        /// nume;prenume;data_nasterii;
        /// Serviciul windows va lua mesajul si va trimite request-ul la baza de date
        /// </summary>
        /// <param name="strNume"></param>
        /// <param name="strPrenume"></param>
        /// <param name="strDataNasterii"></param>
        /// <returns></returns>
        private string GetFormattedMessage(string strNume, string strPrenume, DateTime dtDataAngajarii)
        {
            return
                strNume
                + ";" + strPrenume
                + ";" + dtDataAngajarii + ";";
        }
        #endregion format messages
    }
}
