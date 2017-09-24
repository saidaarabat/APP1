using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ClientConsole
{
    /// <summary>
    /// Définit l'interface de communication entre le logiciel et le médium de stockage qui sera utilisé (BD, fichier XML, mémoire, etc).
    /// </summary>
    public interface ISondageDAO
    {
        /// <summary>
        /// Liste les sondages disponibles.
        /// </summary>
        /// <returns>Les sondages disponibles.</returns>
        /// <exception cref="PersistenceException">Cette exception sera lancée si le DAO est incapable de communiquer avec le médium de stockage pour lire les sondages disponibles</exception>
        IList<Poll> GetAvailablePolls();

        /// <summary>
        /// En fonction de la question courante, retourne la question suivante, ou null si le sondage est terminé.
        /// </summary>
        /// <param name="pollId">L'identifiant unique du sondage.</param>
        /// <param name="currentQuestionId">L'identifiant unique de la question courante, ou -1 si on veut la première question du sondage.</param>
        /// <returns>La prochaine question, ou null si le sondage est terminé.</returns>
        /// <exception cref="InvalidIdException">Cette exception sera lancée si l'un des identifiants fournis au DAO est invalide</exception>
        /// <exception cref="PersistenceException">Cette exception sera lancée si le DAO est incapable de communiquer avec le médium de stockage pour lire les questions</exception>
        PollQuestion GetNextQuestion(int pollId, int currentQuestionId);

        IList<PollQuestion> getAllQuestion(int pollId);
    }
}
