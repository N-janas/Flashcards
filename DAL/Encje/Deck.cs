using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FlashCards.DAL.Encje
{
    class Deck
    {
        #region Własności
        public sbyte? Id { get; set; }
        public string DeckName { get; set; }
        #endregion

        #region Konstruktory
        public Deck(MySqlDataReader reader)
        {
            Id = sbyte.Parse(reader["ID"].ToString());
            DeckName = reader["DeckName"].ToString();
        }

        public Deck(string deckname)
        {
            Id = null;
            DeckName = deckname;
        }
        #endregion

        #region Metody
        public override string ToString()
        {
            return $"{DeckName}";
        }
        public string ToInsert()
        {
            return $"('{DeckName}')";
        }
        #endregion
    }
}
