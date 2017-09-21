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

namespace WebAPI_Sondage.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            ISondageDAO test = new SimpleSondageDAO();
            PollQuestion question = test.GetNextQuestion(1, id);
            return question.Text;
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]Poll reponseQuestion)
        {
            HttpResponseMessage reponse = new HttpResponseMessage();
            if (reponseQuestion != null)
                reponse.StatusCode = HttpStatusCode.OK;
			else
				reponse.StatusCode = HttpStatusCode.NotAcceptable;
            return reponse;
	    }
    }
}
