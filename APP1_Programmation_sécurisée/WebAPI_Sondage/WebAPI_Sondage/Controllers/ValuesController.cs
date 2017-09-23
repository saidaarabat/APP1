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

        // GET api/values/idPoll/idQuestion
        [HttpGet("{idPoll}/{idQuestion}")]
        public IActionResult Get(int idPoll, int idQuestion)
        {

            // Print console 
            Console.WriteLine("Get");

            // Definition des variables
			ISondageDAO iSondageDAO = new SimpleSondageDAO();
            ValidationToken validationToken = new ValidationToken();

            // Recuperation du token dans le header
            var token = Request.Headers["Token"].ToString();

            // Validation du token
            Boolean tokenValidation = validationToken.checkToken(token);

            // Si le token est valide : Code : 200 OK et data 
            if (tokenValidation.Equals(true))
            {
                PollQuestion question = iSondageDAO.GetNextQuestion(idPoll, idQuestion);
                string json = JsonConvert.SerializeObject(question);
                return Ok(question);
            }
            else
            {
                // Sinon non autorisé
                return Unauthorized();
            }
                
        }

        [HttpPost]
        public IActionResult Post([FromBody]PollQuestion reponseQuestion)
        {
            Console.WriteLine("Post");

            // Definition des variables
            ISondageDAO iSondageDAO = new SimpleSondageDAO();
            ValidationToken validationToken = new ValidationToken();

            // Recuperation du token dans le header
            var token = Request.Headers["Token"].ToString();

            // Validation du token
            Boolean tokenValidation = validationToken.checkToken(token);

            // Si le token est valide : Code : 200 OK et on enregistre la reponse recu 
            if (tokenValidation.Equals(true))
            {
                iSondageDAO.SaveAnswer(12, reponseQuestion);
                return Ok();
            }
            else
            {
                // Sinon non autorisé
                return Unauthorized();
            }
        }


    }
}
