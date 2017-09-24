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
	public class ValuesController : Controller
    {
        [HttpGet("token:string")]
        public IActionResult tokenCheck()
        {
            Console.WriteLine("Vérification de token");

            // Definition des variables
            ValidationToken validationToken = new ValidationToken();

            // Recuperation du token dans le header
            var token = Request.Headers["Token"].ToString();

            // Validation du token
            Boolean tokenValidation = validationToken.checkToken(token);

            // Si le token est valide : Code : 200 OK et data 
            if (tokenValidation.Equals(true))
            {
                return Ok();
            }
            else
            {
                // Sinon non autorisé
                return Unauthorized();
            }
        }
        // GET api/values/idPoll/idQuestion
        [HttpGet("{idPoll:int}/{idQuestion:int}")]
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

            // Boolean condition de validation de l'URL
            Boolean bIdQuestion = false;

            Boolean bIdPoll = idPoll.Equals(1) || idPoll.Equals(2); // true si pollId = 1 ou 2
            if (idPoll.Equals(1))
            {
                bIdQuestion = idQuestion.Equals(1) || idQuestion.Equals(11) || idQuestion.Equals(12) || idQuestion.Equals(13);
            }
            else if (idPoll.Equals(2))
            {
                bIdQuestion = idQuestion.Equals(1) || idQuestion.Equals(21)
                || idQuestion.Equals(22) || idQuestion.Equals(23);
            }

            Console.WriteLine(bIdPoll.ToString() + bIdQuestion.ToString());

            // Si le token est valide : Code : 200 OK et data 
            if (tokenValidation.Equals(true) && bIdPoll && bIdQuestion)
            {
                PollQuestion question = iSondageDAO.GetNextQuestion(idPoll, idQuestion);
                string json = JsonConvert.SerializeObject(question);
                return Ok(question);
            }
            else
            {
                if (bIdPoll || bIdQuestion)
                {
                    return BadRequest();
                }
                // Sinon non autorisé
                return Unauthorized();
            }
                
        }


        // Post le reponse a la question du sondage
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
            if (tokenValidation.Equals(true)) {

                // variables
                List<User> users;
                int userId = 0;

                // Parcoursdu fichier json et extraction des données
                using (StreamReader r = new StreamReader("UserToken.json"))
                {
                    string json = r.ReadToEnd();
                    users = JsonConvert.DeserializeObject<List<User>>(json);
                }

                // Parcours liste Users
                foreach (User user in users)
                {
                    // Si le token est dans le fichier alors token valide
                    if (user.token.Equals(token))
                    {
                        userId = user.id;
                    }
                }

                iSondageDAO.SaveAnswer(userId, reponseQuestion);
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
