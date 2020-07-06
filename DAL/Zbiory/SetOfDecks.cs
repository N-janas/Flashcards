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
                MySqlCommand cmd = new MySqlCommand(query.add_deck, connection);
                // Parametryczny string do zabezpieczenia przed SQL Injection
                cmd.Parameters.AddWithValue("@deckName", d.ToString());

                try
                {
                    connection.Open();
                    var id = cmd.ExecuteNonQuery();
                    state = true;
                    d.Id = (sbyte)cmd.LastInsertedId;
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return state;
        }

        public static bool EditDeck(Deck deck, sbyte idDeck)
        {
            bool state = false;
            using (var connection = DBConnection.Instance.Connection)
            {
                string EDYTUJ_NAZWE_TALII = $"UPDATE deck SET DeckName=@decName WHERE ID=@idD";
                MySqlCommand cmd = new MySqlCommand(EDYTUJ_NAZWE_TALII, connection);
                // Parametryczny string do zabezpieczenia przed SQL Injection
                cmd.Parameters.AddWithValue("@decName", deck.DeckName);
                cmd.Parameters.AddWithValue("@idD", idDeck);

                try
                {
                    connection.Open();
                    var n = cmd.ExecuteNonQuery();
                    if (n == 1) state = true;

                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return state;
        }

        public static bool DeleteDeck(Deck deck)
        {
            bool state = false;
            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand(query.delete_deck + $"{deck.Id}", connection);
                connection.Open();
                var n = cmd.ExecuteNonQuery();
                if (n == 1) state = true;

                connection.Close();
            }
            return state;
        }
        #endregion
    }
}
