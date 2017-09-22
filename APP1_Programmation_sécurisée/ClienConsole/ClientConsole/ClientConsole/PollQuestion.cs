﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Runtime.Serialization;


namespace ClientConsole
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
	}
}
