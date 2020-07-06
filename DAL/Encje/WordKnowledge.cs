using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FlashCards.DAL.Encje
{
    class WordKnowledge
    {
        #region Własności
        public ulong? Id { get; set; }
        public uint Id_word_front { get; set; }
        public uint Id_word_back { get; set; }
        public sbyte Id_user { get; set; }
        public sbyte Knowledge { get; set; }
        #endregion

        public WordKnowledge(MySqlDataReader reader)
        {
            Id = ulong.Parse(reader["ID"].ToString());
            Id_word_front = uint.Parse(reader["ID_Word_Front"].ToString());
            Id_word_back = uint.Parse(reader["ID_Word_Back"].ToString());
            Id_user = sbyte.Parse(reader["ID_User"].ToString());
            Knowledge = sbyte.Parse(reader["Knowledge"].ToString());
        }

        public WordKnowledge(uint id_front, uint id_back, sbyte id_user, sbyte knowledge)
        {
            Id = null;
            Id_word_front = id_front;
            Id_word_back = id_back;
            Id_user = id_user;
            Knowledge = knowledge;
        }

        #region Metody
        public string ToInsert()
        {
            return $"({Id_word_front}, {Id_word_back}, {Id_user}, {Knowledge})";
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            var wk = obj as WordKnowledge;
            if (wk is null) return false;
            if (Id_word_front != wk.Id_word_front) return false;
            if (Id_word_back != wk.Id_word_back) return false;
            if (Id_user != wk.Id_user) return false;
            return true;
        }
        #endregion
    }
}
