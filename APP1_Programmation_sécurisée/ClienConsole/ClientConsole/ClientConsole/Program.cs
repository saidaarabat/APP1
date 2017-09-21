using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace WebAPIClient
{
	class Program
	{
		public static void Main(string[] args)
		{
            GetQuestions().Wait();
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