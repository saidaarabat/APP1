using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Security.Cryptography.X509Certificates;
using System.IO;

namespace WebAPIClient
{
	class Program
	{
		public static void Main(string[] args)
		{

            try
			{
                string webAddr = "https://localhost:8081/api/values/";

				var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
				httpWebRequest.ContentType = "application/json; charset=utf-8";
				httpWebRequest.Method = "POST";

                X509Certificate cert = X509Certificate.CreateFromCertFile("mycerts.cer");
                httpWebRequest.ClientCertificates.Add(cert);

				using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
				{
					string json = "{ \"Id\" : 33 , \"Description\" : \"test\" }";

					streamWriter.Write(json);
					streamWriter.Flush();
				}
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
				using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
				{
					var responseText = streamReader.ReadToEnd();
					Console.WriteLine(responseText);

					//Now you have your response.
					//or false depending on information in the response     
				}

			}
			catch (WebException ex)
			{
				Console.WriteLine(ex.Message);
			}

		}

		static async Task GetQuestions()
		{
			var client = new HttpClient();			
            var stringTask = client.GetStringAsync("https://localhost:8081/api/values");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("localhost.pfx");
			var msg = await stringTask;
			Console.Write(msg);
		}


	}
}