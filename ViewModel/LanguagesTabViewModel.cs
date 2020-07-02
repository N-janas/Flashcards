using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.ViewModel
{
    using System.Diagnostics;
    using DAL.Encje;
    using FlashCards.Model;
    using System.Collections.ObjectModel;
    using DAL.Encje;
    class LanguagesTabViewModel : BaseViewModel
    {
        #region Prywatne składowe
        private Model model = null;
        private sbyte? loggedUser = null;
        private ObservableCollection<Language> langCollection = new ObservableCollection<Language>();
        private List<string> difficulties = new List<string>();
        private Language selectedLangZ;
        private Language selectedLangNa;
        private string selectedDifficulty;
        #endregion

        #region Własności
        public string SelectedDifficulty
        {
            get { return selectedDifficulty; }
            set { selectedDifficulty = value; onPropertyChanged(nameof(SelectedDifficulty)); }
        }

        public Language SelectedLangNa
        {
            get { return selectedLangNa; }
            set { selectedLangNa = value; onPropertyChanged(nameof(SelectedLangNa)); }
        }

        public Language SelectedLangZ
        {
            get { return selectedLangZ; }
            set { selectedLangZ = value; onPropertyChanged(nameof(SelectedLangZ)); }
        }

        public ObservableCollection<Language> LangCollection
        {
            get { return langCollection; }
            set { langCollection = value; onPropertyChanged(nameof(LangCollection)); }
        }

        public List<string> Difficulties
        {
            get { return difficulties; }
            set { difficulties = value; onPropertyChanged(nameof(Difficulties)); }
        }

        public sbyte? LoggedUser
        {
            get { return loggedUser; }
            set { loggedUser = value; onPropertyChanged(nameof(LoggedUser)); }
        }

        #endregion
        #region Konstruktory
        // Trzymany w MainVM
        public LanguagesTabViewModel(Model model)
        {
            this.model = model;
            Difficulties = this.model.PassDifficulties();
        }

        public LanguagesTabViewModel()
        {
            // Initialize only
        }
        #endregion

        #region Metody

        private bool Match(Word wordA, Word wordB, WordKnowledge wordKnowledge)
        {
            if (wordA.GUID == wordB.GUID && ((wordKnowledge.Id_word_back == wordB.Id && wordKnowledge.Id_word_front == wordA.Id) || (wordKnowledge.Id_word_back == wordA.Id && wordKnowledge.Id_word_front == wordB.Id))) 
                return true;
            return false;
        }
        public List<FrontBack> CreateFrontBack(List<Word> langA, List<Word> langB, List<WordKnowledge> wordKnowledges)
        {
            List<FrontBack> frontBackList = new List<FrontBack>();

            foreach (Word wordA in langA)
            {
                foreach (Word wordB in langB)
                {
                    if (wordA.GUID == wordB.GUID)
                    {
                        sbyte knowledge = 0;
                        foreach (WordKnowledge wordKnowledge in wordKnowledges)
                        {
                            if (Match(wordA, wordB, wordKnowledge)) knowledge = wordKnowledge.Knowledge;
                        }
                        frontBackList.Add(new FrontBack(wordA, wordB, knowledge));
                        continue;
                    }
                }
            }

            return frontBackList;
        }

        private void FindMinAndMaxKnowledge(List<FrontBack> frontBackList, out sbyte min, out sbyte max)
        {
            max = 0;
            min = 127;

            foreach (FrontBack frontBack in frontBackList)
            {
                if (frontBack.Knowledge > max) max = frontBack.Knowledge;
                if (frontBack.Knowledge < min) min = frontBack.Knowledge;
            }
        }

        private static Random random = new Random();

        public List<Word> Shuffle(List<Word> list)
        {
            for (int n = list.Count; n > 1; n--)
            {
                int rng = random.Next(n + 1);
                Word value = list[rng];
                list[rng] = list[n];
                list[n] = value;
            }

            return list;
        }

        public void SplitWords(List<Word> allWords, sbyte idFront, sbyte idBack, out List<Word> langA, out List<Word> langB)
        {
            langA = new List<Word>();
            langB = new List<Word>();

            foreach(Word word in allWords)
            {
                if (word.Id == idFront)
                    langA.Add(word);
                else if (word.Id == idBack)
                    langB.Add(word);
                else
                    Debug.WriteLine("Data passed to 'CreateQueue' method appeared to be incorrect");
            }
        }

        public List<Word> CreateQueue(List<Word> allWords, List<WordKnowledge> wordKnowledges, sbyte idFront, sbyte idBack)
        {
            SplitWords(allWords, idFront, idBack, out List<Word> langA, out List<Word> langB);
            List<FrontBack> frontBackList = CreateFrontBack(langA, langB, wordKnowledges);

            FindMinAndMaxKnowledge(frontBackList, out sbyte maxKnowledge, out sbyte minKnowledge);

            sbyte difference = maxKnowledge;
            difference -= minKnowledge;
            //sbyte difference = maxKnowledge - minKnowledge; //why is this not working?

            List<Word> queue = new List<Word>();

            foreach (FrontBack frontBack in frontBackList)
            {
                sbyte repetitions = maxKnowledge;
                repetitions -= frontBack.Knowledge;
                repetitions += 1;

                for (int i = 0; i < repetitions; i++)
                {
                    queue.Add(frontBack.Front);
                }
            }
            queue = Shuffle(queue);
            return queue;
        }
        #endregion

        #region Komendy

        #endregion
    }
}
