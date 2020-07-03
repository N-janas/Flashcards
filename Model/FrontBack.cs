using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Model
{
    using DAL.Encje;
    class FrontBack : TrainData
    {
        public Word Front { get; private set; }
        public Word Back { get; private set; }
        public sbyte Knowledge { get; private set; }

        public FrontBack(Word front, Word back, sbyte knowledge)
        {
            Front = front;
            Back = back;
            Knowledge = knowledge;
        }
    }
}
