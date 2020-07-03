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

    static class SetOfDecks
    {
        #region Metody
        public static List<Deck> GetAllDecks()
        {
            List<Deck> decks = new List<Deck>();

            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand(query.all_decks, connection);

                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    decks.Add(new Deck(reader));
                connection.Close();
            }
            return decks;
        }

        public static bool AddNewDeck(Deck d)
        {
            bool state = false;
            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand($"{query.add_deck} {d.ToInsert()}", connection);
                connection.Open();
                var id = cmd.ExecuteNonQuery();
                state = true;
                d.Id = (sbyte)cmd.LastInsertedId;
                connection.Close();
            }
            return state;
        }

        public static bool DeleteDeck(Deck d)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
