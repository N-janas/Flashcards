using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FlashCards.DAL.Encje
{
    class FlipCard
    {
        #region Własności
        public uint? Id { get; set; }
        public string FrontContent { get; set; }
        public string BackContent { get; set; }
        public sbyte Id_Deck { get; set; }

        #endregion

        #region Konstruktory
        public FlipCard(MySqlDataReader reader)
        {
            Id = uint.Parse(reader["ID"].ToString());
            FrontContent = reader["FrontContent"].ToString();
            BackContent = reader["BackContent"].ToString();
            Id_Deck = sbyte.Parse(reader["ID_Deck"].ToString());
        }

        public FlipCard(string frontcontent, string backcontent, sbyte id_deck)
        {
            Id = null;
            FrontContent = frontcontent;
            BackContent = backcontent;
            Id_Deck = id_deck;
        }
        #endregion

        #region Metody
        public override string ToString()
        {
            return $"{FrontContent} -> {BackContent}"; //jezeli chcielibysmy wrocic do pomyslu z notatnikiem to tak to bedzie wygladac
        }
        public string ToInsert()
        {
            return $"('{FrontContent}', '{BackContent}', {Id_Deck})";
        }
        #endregion
    }
}
