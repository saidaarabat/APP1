using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Sondage.Models;
using System.Web;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace WebAPI_Sondage.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
		// GET api/values
		[HttpGet]
		public List<Poll> Categories()
		{
			ISondageDAO test = new SimpleSondageDAO();
			List<Poll> questions = test.GetAvailablePolls().ToList();
			return questions;
		}

        // GET api/values/5
        [HttpGet("{idPoll}/{idQuestion}")]
        public HttpResponseMessage Get(int idPoll, int idQuestion)
        {
            HttpResponseMessage reponse = new HttpResponseMessage();
            Boolean validationToken = false;
            Console.WriteLine("Get");
            List<User> users = LoadJson();
            string token = "eyJpZCI6IjEiLCJhbGciOiJIUzI1NiJ9.eyJpZCI6IjEifQ.pQYKhDF1yyKOjPvZQEMSc5LAizD3R2VMzr1rbH6ih0I";

            foreach (User user in users){
                if (user.token.Equals(token)) {
                    validationToken = true;
                }
            }

            if (validationToken.Equals(true)){
				reponse.StatusCode = HttpStatusCode.OK;
				ISondageDAO test = new SimpleSondageDAO();
				PollQuestion question = test.GetNextQuestion(1, idQuestion);
                string json = JsonConvert.SerializeObject(question);
                reponse.Content = new StringContent(json);
            }
			else
                reponse.StatusCode = HttpStatusCode.Unauthorized;
            return reponse;
        }

        [HttpPost]
        public User Post([FromBody]PollQuestion reponseQuestion)
        {
            Console.WriteLine("Post");
            HttpResponseMessage reponse = new HttpResponseMessage();
            List<User> users = LoadJson();



            if (reponseQuestion != null)
                reponse.StatusCode = HttpStatusCode.OK;
			else
				reponse.StatusCode = HttpStatusCode.NotAcceptable;
            return users.First();
	    }

        public List<User> LoadJson()
		{
			using (StreamReader r = new StreamReader("UserToken.json"))
			{
				string json = r.ReadToEnd();
                List<User> items = JsonConvert.DeserializeObject<List<User>>(json);
                return items;
            }

		}
    }
}
