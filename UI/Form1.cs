using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Text;
using System.Messaging; // dll-ul folosit pentru mesajele care pleaca din interfata
using System.Windows.Forms;

namespace UI
{
    public partial class Form1 : Form
    {
        public string strQueueName = @".\Private$\PrintecMessageQueue";
        public MessageQueue mqQueue;

        public Form1()
        {
            InitializeComponent();
            CreateQueue(strQueueName);
        }

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
                mqMessage.Label = "Printec message: ";
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
        /// metoda trimite catre serviciul windows PrintecMessagingService un mesaj de forma:
        /// sens;nume;prenume;data_nasterii; sensul va fi: [CLIENT->SERVICIU] sau [SERVICIU->CLIENT]
        /// Serviciul windows va lua mesajul si va trimite request-ul la baza de date
        /// </summary>
        /// <param name="strNume"></param>
        /// <param name="strPrenume"></param>
        /// <param name="strDataNasterii"></param>
        /// <returns></returns>
        private string GetFormattedMessage(string strNume, string strPrenume, DateTime dtDataAngajarii)
        {
            // formez un mesaj pentru coada de forma:
            // [CLIENT->SERVICIU];NUME;PRENUME;AZI;MESAJ_EROARE (va fi necompletat initial, pe urma serviciul windows il completeaza);

            return
                "[CLIENT->SERVICIU]" // sensul mesajului
                + ";" + strNume
                + ";" + strPrenume
                + ";" + dtDataAngajarii + ";;"; // aici mai las un camp necompletat pentru rezultatul verificarii ;
        }
        #endregion format messages

        #region form messages
        private void btnSubmitForm_Click(object sender, EventArgs e)
        {
            try
            {
                string numeTrimis = txtNume.Text;
                string prenumeTrimis = txtPrenume.Text;
                DateTime dataAngajariiTrimisa = dtDataAngajarii.Value;

                // start validare date
                string strListaMesajeEroare = "";

                if (string.IsNullOrEmpty(numeTrimis))
                    strListaMesajeEroare += "\nCompletati numele";

                if (string.IsNullOrEmpty(prenumeTrimis))
                    strListaMesajeEroare += "\nCompletati prenumele";

                if (dtDataAngajarii.Value == DateTime.MinValue || dtDataAngajarii.Value == DateTime.MaxValue)
                    strListaMesajeEroare += "\nCompletati data nasterii";

                // daca am cel putin un mesaj de eroare, opresc formularul de la trimitere
                if (!String.IsNullOrEmpty(strListaMesajeEroare))
                    MessageBox.Show("Au aparut urmatoarele erori: \n" + strListaMesajeEroare);

                else
                {
                    string strMesajTrimis = GetFormattedMessage(txtNume.Text, txtPrenume.Text, dataAngajariiTrimisa);

                    PutMessage(strMesajTrimis); // pun mesajul in coada pentru serviciul PrintecMessagingService
                    MessageBox.Show("Mesaj trimis la serviciul de procesare mesaje: \n" + strMesajTrimis);
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("eroare: " + ex.Message);
            }
        }
        #endregion form messages

        /// <summary>
        /// metoda apelata la trecerea unui interval de o secunda, pentru a vedea daca exista sau nu mesaje primite de la serviciul
        /// PrintecMessagingService
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            // vad daca exista un mesaj pe coada
            string receivedMessage = GetMessage();

            // daca e un mesaj pe coada si e de la serviciul windows care a procesat mesajul
            if (!String.IsNullOrEmpty(receivedMessage) && receivedMessage.Contains("[SERVICIU->CLIENT]"))
            {
                if (receivedMessage.Contains("[EROARE]"))
                    MessageBox.Show("Datele NU au fost introduse in baza de date");
                else
                    MessageBox.Show("Datele au fost introduse cu succes in baza de date");
            }
        }
    }
}
