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

    static class SetOfLanguages
    {
        #region Metody
        public static List<Language> GetAllLanguages()
        {
            List<Language> langs = new List<Language>();

            using (var connection = DBConnection.Instance.Connection)
            {
                // Wykonanie zapytania o pobranie wszystkich języków
                MySqlCommand cmd = new MySqlCommand(query.all_langs, connection);

                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    langs.Add(new Language(reader));
                connection.Close();
            }
            return langs;
        }
        #endregion
    }
}
