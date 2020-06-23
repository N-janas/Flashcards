using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FlashCards.DAL.Encje
{
    class Language
    {
        #region Własności
        public sbyte Id { get; set; }
        public string LangName { get; set; }
        #endregion

        #region Konstruktory
        public Language(MySqlDataReader reader)
        {
            Id = sbyte.Parse(reader["ID"].ToString());
            LangName = reader["Name"].ToString();
        }
        #endregion

        #region Metody

        #endregion
    }
}
