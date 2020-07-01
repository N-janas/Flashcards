using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.ViewModel
{
    using DAL.Encje;
    using Model;
    class Queue
    {
        private bool match(Word wordA, Word wordB, WordKnowledge wordKnowledge)
        {
            if (wordA.GUID == wordB.GUID && wordKnowledge.Id_word_back == wordB.Id && wordKnowledge.Id_word_front == wordA.Id) return true;
            return false;
        }
        public List<FrontBack> createFrontBack(List<Word> langA, List<Word> langB, List<WordKnowledge> wordKnowledges)
        {
            List<FrontBack> frontBackList = new List<FrontBack>();

            foreach(Word wordA in langA)
            {
                foreach(Word wordB in langB)
                {
                    if (wordA.GUID == wordB.GUID)
                    {
                        sbyte knowledge = 0;
                        foreach (WordKnowledge wordKnowledge in wordKnowledges)
                        {
                            if (match(wordA, wordB, wordKnowledge)) knowledge = wordKnowledge.Knowledge;
                        }
                        frontBackList.Add(new FrontBack(wordA, wordB, knowledge));
                        continue;
                    }
                }
            }

            return frontBackList;
        }

        private void findMinAndMaxKnowledge(List<FrontBack> frontBackList, out sbyte min, out sbyte max)
        {
            max = 0;
            min = 127;

            foreach(FrontBack frontBack in frontBackList)
            {
                if (frontBack.Knowledge > max) max = frontBack.Knowledge;
                if (frontBack.Knowledge < min) min = frontBack.Knowledge;
            }
        }

        private static Random random = new Random();

        public List<Word> Shuffle(List<Word> list)
        {
            for(int n=list.Count; n>1; n--)
            {
                int rng = random.Next(n + 1);
                Word value = list[rng];
                list[rng] = list[n];
                list[n] = value;
            }

            return list;
        }

        public List<Word> CreateQueue(List<Word> langA, List<Word> langB, List<WordKnowledge> wordKnowledges)
        {
            //zakladam, ze dane w langA i langB sa poprawne
            List<FrontBack> frontBackList = createFrontBack(langA, langB, wordKnowledges);

            findMinAndMaxKnowledge(frontBackList, out sbyte maxKnowledge, out sbyte minKnowledge);

            sbyte difference = maxKnowledge;
            difference -= minKnowledge;
            //sbyte difference = maxKnowledge - minKnowledge; //why is this not working?

            List<Word> queue = new List<Word>();

            foreach(FrontBack frontBack in frontBackList)
            {
                sbyte repetitions = maxKnowledge;
                repetitions -= frontBack.Knowledge;
                repetitions += 1;

                for(int i=0; i<repetitions; i++)
                {
                    queue.Add(frontBack.Front);
                }
            }
            queue = Shuffle(queue);
            return queue;
        }
    }
}
