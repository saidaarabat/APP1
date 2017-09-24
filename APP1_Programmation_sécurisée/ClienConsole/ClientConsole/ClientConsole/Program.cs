using System;
using System.Net;
using System.IO;
using System.Threading;
using Newtonsoft.Json;
using System.Text;
using System.Runtime.Serialization.Json;

namespace ClientConsole
{
    class Program
	{
        // Fonction principal
        public static void Main(string[] args)
        {
			// Choix du Token
			Console.WriteLine("Authentification : 1 = Token valide -- 2 = Token non valide");

            string token=null;
            // Lecture de son choix et conversion en Int
			string valideToken = Console.ReadLine();

            while (!valideToken.Equals("1") && !valideToken.Equals("2"))
            {
                Console.WriteLine("Veuillez faire une saisie valide");
                valideToken = Console.ReadLine();
            }

            if (valideToken.Equals("1"))
            {
                token = "eyJpZCI6IjEiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6IjEifQ.pQYKhDF1yyKOjPvZQEMSc5LAizD3R2VMzr1rbH6ih0I";
            }
            else if (valideToken.Equals("1"))
            {
                token = "InvalidToken";
            }

            // Validation du token
            CheckToken(token);

            // Choix du sondage par l utilisateur
            Console.WriteLine("Bienvenue dans l'application client dédiée au sondage, quel sondage voulez vous sélectionner : 1-Lecture à la maison ou 2-Consommation de café et d'alcool ? Tapez 1 ou 2 ");

            // Lecture de son choix et conversion en int
            string poolIdString = Console.ReadLine();

            while (!poolIdString.Equals("1") && !poolIdString.Equals("2"))
            {
                Console.WriteLine("Veuillez faire une saisie valide");
                valideToken = Console.ReadLine();
            }

            int poolId = Int32.Parse(poolIdString);

            // parcours des questions 
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    // Get premeiere question
                    GetQuestion(token, poolId, 0);
                } else
                {
                    // Get autres questions
                    GetQuestion(token, poolId, poolId * 10 + i);
                }
                // Lecture reponse
                string reponseQuestion = Console.ReadLine();
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
                //X509Certificate2 cert = new X509Certificate2("mycerts.cer");
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
                //X509Certificate2 cert = new X509Certificate2("mycerts.cer");
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
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					var responseText = streamReader.ReadToEnd();
				}

			    }
                
                // Exception
			    catch (WebException ex)
			    {
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
                //X509Certificate2 cert = new X509Certificate2("mycerts.cer");
                //httpWebRequest.ClientCertificates.Add(cert);
                // Ignore the certificate check when ssl
                httpWebRequest.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;

                // Envoi de la request et lecture de la reponse
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            }

            // Exception
            catch (WebException ex)
            {
                Console.WriteLine(value: ex.Message);
                Thread.Sleep(2000);
                System.Environment.Exit(1);
            }

        }
    }
}