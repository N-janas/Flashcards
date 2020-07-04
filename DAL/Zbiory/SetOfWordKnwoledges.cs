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
    class SetOfWordKnwoledges
    {
        #region Metody
        public static List<WordKnowledge> GetAllWordKnowledges()
        {
            List<WordKnowledge> wknowledges = new List<WordKnowledge>();
            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand(query.all_word_knowledge, connection);

                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    wknowledges.Add(new WordKnowledge(reader));
                connection.Close();
            }
            return wknowledges;
        }

        public static bool AddWordKnowledge(WordKnowledge wk)
        {
            bool state = false;
            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand($"{query.add_word_knowledge} {wk.ToInsert()}", connection);
                connection.Open();
                var id = cmd.ExecuteNonQuery();
                state = true;
                wk.Id = (ulong)cmd.LastInsertedId;
                connection.Close();
            }
            return state;
        }

        public static bool EditWordKnowledge(WordKnowledge wk, ulong? idWk)
        {
            bool state = false;
            using(var connection = DBConnection.Instance.Connection)
            {
                // Aktualizacja krotki (jeśli wymagana) w miejscu poprzednika (zmiana tylko knowledge)
                string AKTUALIZUJ_POZIOM = $"UPDATE wordknowledge SET knowledge={wk.Knowledge} WHERE ID={idWk}";
                MySqlCommand cmd = new MySqlCommand(AKTUALIZUJ_POZIOM, connection);
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
