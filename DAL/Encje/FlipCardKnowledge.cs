﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FlashCards.DAL.Encje
{
    class FlipCardKnowledge
    {
        #region Własności
        public byte? Id { get; set; }
        public sbyte Id_User { get; set; }
        public uint Id_FlipCard { get; set; }
        public sbyte Knowledge { get; set; }

        #endregion

        #region Konstruktory
        public FlipCardKnowledge(MySqlDataReader reader)
        {
            Id = byte.Parse(reader["ID"].ToString());
            Id_User = sbyte.Parse(reader["ID_User"].ToString());
            Id_FlipCard = uint.Parse(reader["ID_FlipCard"].ToString());
            Knowledge = sbyte.Parse(reader["Knowledge"].ToString());
        }

        public FlipCardKnowledge(sbyte id_user, uint id_flipcard, sbyte knowledge)
        {
            Id = null;
            Id_User = id_user;
            Id_FlipCard = id_flipcard;
            Knowledge = knowledge;
        }

        #endregion

        #region Metody
        public string ToInsert()
        {
            return $"({Id_User}, {Id_FlipCard}, {Knowledge})";
        }
        #endregion
    }
}
