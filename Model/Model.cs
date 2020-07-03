using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.Model
{
    using DAL.Encje;
    using DAL.Zbiory;
    using System.Collections.ObjectModel;

    class Model
    {
        #region List Danych
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<Language> Langs { get; set; } = new ObservableCollection<Language>();
        public ObservableCollection<Word> Words { get; set; } = new ObservableCollection<Word>();
        public ObservableCollection<WordKnowledge> WordKnowledges { get; set; } = new ObservableCollection<WordKnowledge>();



        #endregion

        public Model()
        {
            // Strona językowa
            var users = SetOfUsers.GetAllUsers();
            foreach (var user in users) Users.Add(user);

            var langs = SetOfLanguages.GetAllLanguages();
            foreach (var lang in langs) Langs.Add(lang);

            var words = SetOfWords.GetAllWords();
            foreach (var word in words) Words.Add(word);

            var wordKnowledges = SetOfWordKnwoledges.GetAllWordKnowledges();
            foreach (var wks in wordKnowledges) WordKnowledges.Add(wks);

            // Strona Fiszki

        }

        #region Metody
        public List<string> PassDifficulties()
        {
            HashSet<string> distinctDiffs = new HashSet<string>();
            // Bezpiecznie można ściągać z Words, ponieważ jak słowo w danym języku ma klase A1
            // to jego tłumaczenia mają tą samą klasę, więc nie istnieje możliwość braku jakiejś klasy w comboBox'ie
            // dla dowolnych dwóch języków
            foreach (var word in Words)
            {
                distinctDiffs.Add(word.Difficulty);
            }

            return distinctDiffs.ToList<string>();
        }

        public bool PersonExists(User user) => Users.Contains(user);

        // Jesli można zalogować to zwróć id user i podłącz go
        // else zwróć null (brak usera programu)
        public sbyte? PassUserIdIfExists(string name, string surname)
        {
            User u = new User(name, surname);
            if (PersonExists(u))
            {
                // Przekazani indeksu usera
                return Users[Users.IndexOf(u)].Id;
            }
            return null;
        }

        // Dodawanie użytkownika
        public bool AddUserToUsers(string name, string surname)
        {
            User u = new User(name, surname);

            if (!PersonExists(u))
            {
                if (SetOfUsers.AddNewUser(u))
                {
                    Users.Add(u);
                    return true;
                }
            }
            return false;
        }
        
        public List<Word> PassWordCollection(sbyte word_id, sbyte translation_id, string difficulty)
        {
            List<Word> randomWords = new List<Word>();
            foreach (var word in Words)
            {
                // Dodawnie słów powiązanych z dwoma wybranymi językami
                if ((word.Id_lang == word_id || word.Id_lang == translation_id) && word.Difficulty.Equals(difficulty))
                    randomWords.Add(word);
            }
            return randomWords;
        }

        private Word FindWordById(int id)
        {
            foreach (var w in Words)
            {
                if (w.Id == id)
                    return w;
            }
            return null;
        }

        public List<WordKnowledge> PassUserPerformance(sbyte? user_id, sbyte langA, sbyte langB)
        {
            List<WordKnowledge> currentUserPerformance = new List<WordKnowledge>();
            // Wybierz języki w kolejności mniejszy na wiekszy
            // Czyli PL -> ANG (1, 5)
            // to ANG -> PL nadal (1, 5)

            sbyte minLang = Math.Min(langA, langB);
            sbyte maxLang = Math.Max(langA, langB);

            // Zakładamy że tłumaczenie języków z mniejszego id na większe
            foreach (var wk in WordKnowledges)
            {
                // if user_id = user_id and smallLang = word.(small)Lang_id and bigLang = word.(big)Lang_id 
                if (wk.Id_user == user_id && minLang == FindWordById(wk.Id_word_front).Id_lang && maxLang == FindWordById(wk.Id_word_back).Id_lang)
                {
                    currentUserPerformance.Add(wk);
                }
            }
            // Przekazanie krotek z obecnym uczeniem użytkownika
            return currentUserPerformance;
        }

        public bool WordKnowledgeExists(WordKnowledge wk) => WordKnowledges.Contains(wk); 

        public bool UpdateWordKnowledge(List<WordKnowledge> updatedPerfSet)
        {
            foreach (var knowledge in updatedPerfSet)
            {
                // Equals ovveride bez sprawdzania level, więc szuka tylko krotki wg.: id_front, id_back, id_user
                // Check czy user już sie tego uczył
                if (WordKnowledgeExists(knowledge))
                {
                    // Edycja (użycie Update)
                    var oldLevel = WordKnowledges[WordKnowledges.IndexOf(knowledge)];
                    // Czy zmienił poziom w danej krotce
                    if (oldLevel.Knowledge != knowledge.Knowledge)
                    {
                        // Jeśli tak to edytuj w bazie 
                        if (SetOfWordKnwoledges.EditWordKnowledge(knowledge, oldLevel.Id))
                        {
                            // i edytuj w kolekcji
                            knowledge.Id = oldLevel.Id;
                            WordKnowledges[WordKnowledges.IndexOf(oldLevel)] = knowledge;
                        }
                        
                    }
                }
                else // Jeśli nowa krotka to
                {
                    // Dodaj nowe ( użycie SetOf...Add (index się doda))
                    if (SetOfWordKnwoledges.AddWordKnowledge(knowledge))
                    {
                        // Dodaj do listy (z indexem)
                        WordKnowledges.Add(knowledge);
                    }
                    
                }

            }
            return true; // Jaki return ? bo w sumie to guzikiem nie bedzie to chyba typ void ?
        }
        #endregion
    }
}
