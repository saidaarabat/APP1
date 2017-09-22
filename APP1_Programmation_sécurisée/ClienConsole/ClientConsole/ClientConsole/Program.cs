using System;
using System.Net;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace ClientConsole
{
    class Program
	{
        // Fonction principal
        public static void Main(string[] args)
        {
            // Choix du sondage par l utilisateur
            Console.WriteLine("Bienvenue dans l'application client dédiée au sondage, quel sondage voulez vous sélectionner : 1-Lecture à la maison ou 2-Consommation de café et d'alcool ? Tapez 1 ou 2 ");

            // Lecture de son choix et conversion en int
            int poolId = Int32.Parse(Console.ReadLine());

            // parcours des questions 
            for (int i = 0; i < 4; i++)
            {
                if (i == 0)
                {
                    // Get premeiere question
                    GetQuestion(poolId, 0);
                } else
                {
                    // Get autres questions
                    GetQuestion(poolId, poolId * 10 + i);
                }
                // Lecture reponse
                string reponseQuestion = Console.ReadLine();
                // Post de la reponse vers l api
                PostQuestion(poolId,poolId * 10 + i +1 , reponseQuestion);
            }

            // Fin du Main
            Console.WriteLine("Merci d'avoir participé au sondage");
            int milliseconds = 2000;
            Thread.Sleep(milliseconds);

        }

        /*
         * Fonction Get pour demander à l'API REST la question 
         * poolId : ID du sondage 
         * questionId : Id de la question
        */
        public static void GetQuestion(int poolId, int questionId)
        {
            try
            {
                // Adresse auquel correspond le GET de l API
                string webAddr = "https://localhost:8081/api/values/" + questionId.ToString();

                // Définition du Header
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json; charset=utf-8";
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
                    var responseText = streamReader.ReadToEnd();
                    Console.WriteLine(responseText);

                    //Now you have your response.
                    //or false depending on information in the response     
                }
            }

            // Exception
            catch (WebException ex)
            {
                Console.WriteLine(value: "Une erreur est survenue, veuillez réessayer plutard");
            }

        }

        /*
         * Fonction POST pour repondre à la question 
         * poolId : ID du sondage 
         * questionId : Id de la question
         * text : reponse a la question
        */
        public static void PostQuestion(int pollId, int questionId, string text)
        {
            try
			{
                // Adresse auquel correspond le GET de l API
                string webAddr = "https://localhost:8081/api/values/";

                // Définition du Header
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
				httpWebRequest.ContentType = "application/json; charset=utf-8";
				httpWebRequest.Method = "POST";

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
				    Console.WriteLine(value: "Une erreur est survenue, veuillez réessayer plutard");

			    }
            }
        }
}