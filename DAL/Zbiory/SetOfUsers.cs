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

    static class SetOfUsers
    {
        #region Metody
        public static List<User> GetAllUsers()
        {
            List<User> users = new List<User>();
            using (var connection = DBConnection.Instance.Connection)
            {
                MySqlCommand cmd = new MySqlCommand(query.all_users, connection);

                connection.Open();
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                    users.Add(new User(reader));
                connection.Close();
            }
            return users;
        }

        // Dodaj usera
        #endregion

    }
}
