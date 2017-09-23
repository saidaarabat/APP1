using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using WebAPI_Sondage.Models;
using System.Net.Http;
using System.IO;
using System.Net;
using Newtonsoft.Json;


namespace WebAPI_Sondage.Controllers
{
    [Route("api/[controller]")]
	[RESTAuthorize]
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
        public PollQuestion Get(int idPoll, int idQuestion)
        {

            Boolean validationToken = false;
            Console.WriteLine("Get");
			ISondageDAO test = new SimpleSondageDAO();
			List<User> users = LoadJson();
            PollQuestion defaultResponse = test.GetNextQuestion(0, 0);

            var requete = Request;
            var token = requete.Headers["Token"].ToString();


            foreach (User user in users){
                if (user.token.Equals(token)) {
                    validationToken = true;
                }
            }
            if (validationToken.Equals(true))
            {
                PollQuestion question = test.GetNextQuestion(idPoll, idQuestion);
                string json = JsonConvert.SerializeObject(question);

                return question;
            }
            else
                return defaultResponse;
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
