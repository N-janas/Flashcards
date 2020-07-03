﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace FlashCards.DAL.Encje
{
    class Word : TrainData
    {
        #region Własności
        public uint Id { get; set; }
        public string WordName { get; set; }
        public sbyte Id_lang { get; set; }
        public string Difficulty { get; set; }
        public long GUID { get; set; }
        #endregion

        #region Konstruktory
        public Word(MySqlDataReader reader)
        {
            Id = uint.Parse(reader["ID"].ToString());
            WordName = reader["Word"].ToString();
            Id_lang = sbyte.Parse(reader["ID_Language"].ToString());
            Difficulty = reader["Difficulty"].ToString();
            GUID = long.Parse(reader["GUID"].ToString());
        }
        #endregion

        #region Metody
        public override string ToString()
        {
            return $"{WordName}";
        }
        #endregion
    }
}
