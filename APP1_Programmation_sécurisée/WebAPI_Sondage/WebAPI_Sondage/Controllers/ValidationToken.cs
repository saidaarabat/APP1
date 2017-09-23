using System;
using System.Collections.Generic;
using WebAPI_Sondage.Models;
using System.IO;
using Newtonsoft.Json;

namespace WebAPI_Sondage.Controllers
{
    // Class validation token
    public class ValidationToken
    {
        // Lecture du fichier JSON avec les tokens
        private List<User> LoadJson()
        {
            using (StreamReader r = new StreamReader("UserToken.json"))
            {
                string json = r.ReadToEnd();
                List<User> items = JsonConvert.DeserializeObject<List<User>>(json);
                return items;
            }

        }

        // Validation du token true : token ok
        public Boolean checkToken(string token)
        {

            // Variable
            Boolean validationToken = false;
            
            // Lecture fichier JSON
            List<User> users = LoadJson();

            // Parcours liste Users
            foreach (User user in users)
            {
                // Si le token est dans le fichier alors token valide
                if (user.token.Equals(token))
                {
                    validationToken = true;
                }
            }

            // return 
            return validationToken;
        }
    }
}
