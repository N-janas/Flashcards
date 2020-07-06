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

    static class SetOfFlipCards
    {
        #region Metody
        public static List<FlipCard> GetAllFlipCards()
        {
            List<FlipCard> flipCards = new List<FlipCard>();

            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand(query.all_flip_cards, connection);

                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    flipCards.Add(new FlipCard(reader));
                connection.Close();
            }
            return flipCards;
        }
        public static bool AddNewFlipCard(FlipCard fc)
        {
            bool state = false;
            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand(query.add_flip_card, connection);
                // Parametryczny string do zabezpieczenia przed SQL Injection
                cmd.Parameters.AddWithValue("@frontC", fc.FrontContent);
                cmd.Parameters.AddWithValue("@backC", fc.BackContent);
                cmd.Parameters.AddWithValue("@idD", fc.Id_Deck);

                try
                {
                    connection.Open();
                    var id = cmd.ExecuteNonQuery();
                    state = true;
                    fc.Id = (uint)cmd.LastInsertedId;
                    connection.Close();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            return state;
        }

        public static bool EditFlipCard(FlipCard fpc, uint idFpc, sbyte idDeck)
        {
            bool state = false;
            using (var connection = DBConnection.Instance.Connection)
            {
                string EDYTUJ_FLIPCARD = $"UPDATE flipcard SET FrontContent=@frontC, BackContent=@backC WHERE ID=@idF AND ID_Deck=@idD";
                MySqlCommand cmd = new MySqlCommand(EDYTUJ_FLIPCARD, connection);
                cmd.Parameters.AddWithValue("@frontC", fpc.FrontContent);
                cmd.Parameters.AddWithValue("@backC", fpc.BackContent);
                cmd.Parameters.AddWithValue("@idF", idFpc);
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

        public static bool DeleteFlipCard(FlipCard fc)
        {
            bool state = false;
            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand(query.delete_flipCard + $"{fc.Id}", connection);
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
