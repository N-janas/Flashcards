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
            DeckName = deckname.Trim();
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
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var deck = obj as Deck;
            if (deck is null) return false;
            if (DeckName.ToLower() != deck.DeckName.ToLower()) return false;
            return true;
        }
        #endregion
    }
}
