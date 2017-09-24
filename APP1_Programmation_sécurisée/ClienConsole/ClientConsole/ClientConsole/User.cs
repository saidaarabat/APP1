using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace ClientConsole
{
    [DataContract]
    public class User
	{
		/// <summary>
		/// L'identifiant du sondage.
		/// </summary>
		[DataMember]
		public int id { get; set; }

		/// <summary>
		/// La description du sondage.
		/// </summary>
		[DataMember]
		public string token { get; set; }
	}
}
