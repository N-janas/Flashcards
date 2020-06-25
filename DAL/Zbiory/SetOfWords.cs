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
    class SetOfWords
    {
        public static List<Word> GetAllWords()
        {
            List<Word> words = new List<Word>();
            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand(query.all_words, connection);

                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    words.Add(new Word(reader));
                connection.Close();
            }
            return words;
        }
    }
}
