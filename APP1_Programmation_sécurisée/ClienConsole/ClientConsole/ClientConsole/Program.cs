using System;
using System.Net;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using System.Runtime.Serialization.Json;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace ClientConsole
{
    class Program
	{
        // Fonction principal
        public static void Main(string[] args)
        {
			// Choix du Token
			Console.WriteLine("Authentification : 1 = Token valide -- 2 = Token non valide");

            // Initialisation du token
            string token="TokenInvalid";

            // Lecture de son choix et conversion en Int
			string valideToken = Console.ReadLine();

            // Validation de l entree utilisateur
            while (!valideToken.Equals("1") && !valideToken.Equals("2"))
            {
                Console.WriteLine("Veuillez faire une saisie valide");
                valideToken = Console.ReadLine();
            }

            // Definition du token en fonction entrée utilisateur
            if (valideToken.Equals("1"))
            {
                token = "eyJpZCI6IjEiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6IjEifQ.pQYKhDF1yyKOjPvZQEMSc5LAizD3R2VMzr1rbH6ih0I";
            }

            // Validation du token envers l API
            CheckToken(token);

            // Choix du sondage par l utilisateur
            Console.WriteLine("Bienvenue dans l'application client dédiée au sondage, quel sondage voulez vous sélectionner : 1-Lecture à la maison ou 2-Consommation de café et d'alcool ? Tapez 1 ou 2 ");

            // Lecture de son choix et conversion en int
            string poolIdString = Console.ReadLine();

            // Validation de l entrée de l utilisateru
            while (!poolIdString.Equals("1") && !poolIdString.Equals("2"))
            {
                Console.WriteLine("Veuillez faire une saisie valide");
                valideToken = Console.ReadLine();
            }

            // Parse l entree en int
            int poolId = Int32.Parse(poolIdString);

            // parcours des questions 
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    // Get premeiere question
                    GetQuestion(token, poolId, 1);
                    i = 0;
                } else
                {
                    // Get autres questions
                    GetQuestion(token, poolId, poolId * 10 + i);
                }

                // Lecture reponse
                string reponseQuestion = Console.ReadLine();

                // check the answer
                Boolean check = checkAnswer(poolId, poolId * 10 + i + 1, reponseQuestion);

                // VAlidation de la reponse de l ulitisateur pour l aquestion idQuestion du sondage idSondage
                while (check == false)
                {
                    Console.WriteLine("Veuillez faire une réponse valide");
                    // Lecture reponse
                    reponseQuestion = Console.ReadLine();
                    // check the answer
                    check = checkAnswer(poolId, poolId * 10 + i + 1, reponseQuestion);
                }

                // Post de la reponse vers l api
                PostQuestion(token, poolId,poolId * 10 + i +1 , reponseQuestion);
            }

            // Fin du Main
            Console.WriteLine("Merci d'avoir participé au sondage");
            Thread.Sleep(2000);

        }

        /*
         * Fonction Get pour demander à l'API REST la question 
         * poolId : ID du sondage 
         * questionId : Id de la question
        */
        public static void GetQuestion(string token, int poolId, int questionId)
        {
            try
            {
                // Adresse auquel correspond le GET de l API
                string webAddr = "https://localhost:8081/api/values/" + poolId.ToString() + "/" + questionId.ToString();

                // Définition du Header
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Headers["Token"] = token;
                httpWebRequest.Method = "GET";

                // Token authentification 

                // Certificat pour le SSL/TLS
                //X509Certificate2 cert = new X509Certificate2("mycerts.cer","password");
                //httpWebRequest.ClientCertificates.Add(cert);
                // Ignore the certificate check when ssl
                httpWebRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                // Envoi de la request et lecture de la reponse
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    
                    var response = streamReader.ReadToEnd();

                    // Afficher la question depuis la RequestReponse
                    var pollQuestion = JsonConvert.DeserializeObject<PollQuestion>(response);
                    Console.WriteLine(pollQuestion.Text);    
                }
            }

            // Exception
            catch (WebException ex)
            {
                // Affichage du message de l exeption retourné par  API
                Console.WriteLine(value: ex.Message);
                Thread.Sleep(2000);
                System.Environment.Exit(1);
            }

        }

        /*
         * Fonction POST pour repondre à la question 
         * poolId : ID du sondage 
         * questionId : Id de la question
         * text : reponse a la question
        */
        public static void PostQuestion(string token, int pollId, int questionId, string text)
        {
            try
			{
                // Adresse auquel correspond le GET de l API
                string webAddr = "https://localhost:8081/api/values/";

                // Définition du Header
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
				httpWebRequest.ContentType = "application/json; charset=utf-8";
				httpWebRequest.Method = "POST";
                httpWebRequest.Headers["Token"] = token;

                // Certificat pour le SSL/TLS
                //X509Certificate2 cert = new X509Certificate2("mycerts.cer","password");
                //httpWebRequest.ClientCertificates.Add(cert);
                // Ignore the certificate check when ssl
                httpWebRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                // Formation des données au format JSON
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
				{
                    // Definition d un structure PollQuestion 
                    PollQuestion poolQuestion = new PollQuestion();
                    poolQuestion.PollId = pollId;
                    poolQuestion.QuestionId = questionId;
                    poolQuestion.Text = text;

                    // Convertion en JSON
                    string json = JsonConvert.SerializeObject(poolQuestion);

                    // Ajout des données a la request
                    streamWriter.Write(json);
					streamWriter.Flush();
				}

                // Envoi de la requete et lecture reponse
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			    }
                
                // Exception
			    catch (WebException ex)
			    {
                    // Affichage du message de l exeption retourné par  API
                    Console.WriteLine(value: ex.Message);
                    Thread.Sleep(2000);
                    System.Environment.Exit(1);
            }
            }

        /*
         * Fonction Get pour demander à l'API REST la question 
        */
        public static void CheckToken(string token)
        {
            try
            {
                // Adresse auquel correspond le GET de l API
                string webAddr = "https://localhost:8081/api/values/token";

                // Définition du Header
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Headers["Token"] = token;
                httpWebRequest.Method = "GET";

                // Token authentification 

                // Certificat pour le SSL/TLS
                //X509Certificate2 cert = new X509Certificate2("mycerts.cer","password");
                //httpWebRequest.ClientCertificates.Add(cert);
                // Ignore the certificate check when ssl
                httpWebRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                // Envoi de la request et lecture de la reponse
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }

            // Exception
            catch (WebException ex)
            {
                // Affichage du message de l exeption retourné par  API
                Console.WriteLine(value: ex.Message);
                Thread.Sleep(2000);
                System.Environment.Exit(1);
            }

        }


        /*
         * Fonction validation de la reponse envoyer par l utilisateur a l application
         * idPoll id sondage
         * idQuestion id de la question
         * reponse reponse donnée par l utilisateur
         * retrun Boolean : true si reponse valide
         */
        public static Boolean checkAnswer(int idPoll, int idQuestion, string reponse)
        {
            // initialisation du boolean
            Boolean exist = false;

            // Recuperation de la question idQuestion du sondage IdPool 
            ISondageDAO sondageDAO = new SimpleSondageDAO();
            IList<PollQuestion> list = sondageDAO.getAllQuestion(idPoll);
            PollQuestion question = list[idQuestion - idPoll*10 - 1];

            // Split la liste des possibilitées de repose
            string[] answersPossibilities = question.listeReponses.Split(",");

            // Parcours de la liste
            foreach(string answerPossibility in answersPossibilities)
            {
                // Check si elle est contenue dans la liste
                if (reponse == answerPossibility)
                {
                    exist = true;
                }
            }

            return exist;
        }
    }
}