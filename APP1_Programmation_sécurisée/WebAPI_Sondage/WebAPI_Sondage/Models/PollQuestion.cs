using System;
using System.Runtime.Serialization;


namespace WebAPI_Sondage.Models
{
    public class PollQuestion
	{
		/// <summary>
		/// Identifiant du sondage auquel appartient cette question.
		/// </summary>
		[DataMember]
		public int PollId { get; set; }

		/// <summary>
		/// Identifiant unique de la question.
		/// </summary>
		[DataMember]
		public int QuestionId { get; set; }

		/// <summary>
		/// Si l'usager a répondu à la question, contient la réponse. Sinon, contient le
		/// texte de la question (le libellé).
		/// </summary>
		[DataMember]
		public String Text { get; set; }

        /// <summary>
        /// Liste des reponses possible pour la question
        /// </summary>
        [DataMember]
        public String listeReponses { get; set; }
    }
}
