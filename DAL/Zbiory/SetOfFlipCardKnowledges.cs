using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FlashCards.DAL.Zbiory
{
    using Encje;
    // Skracanie nazwy dla zapytań
    using query = Properties.Resources;

    static class SetOfFlipCardKnowledges
    {
        #region Metody
        public static List<FlipCardKnowledge> GetAllFlipCardKnowledges()
        {
            List<FlipCardKnowledge> flipCardKnowledges = new List<FlipCardKnowledge>();

            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand(query.all_flip_card_knowledge, connection);

                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    flipCardKnowledges.Add(new FlipCardKnowledge(reader));
                connection.Close();
            }
            return flipCardKnowledges;
        }
        public static bool AddNewFlipCardKnowledge(FlipCardKnowledge fck)
        {
            bool state = false;
            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand($"{query.add_flip_card_knowledge} {fck.ToInsert()}", connection);
                connection.Open();
                var id = cmd.ExecuteNonQuery();
                state = true;
                fck.Id = (byte)cmd.LastInsertedId;
                connection.Close();
            }
            return state;
        }

        public static bool DeleteFlipCardKnowledge(FlipCardKnowledge fck)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
