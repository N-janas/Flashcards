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
        
        // 2 opcje
        // 1: trzymanie filtrow w modelu i szukanie od nich ( z, na, diff)
        // 2: funkcje w modelu przyjmujące te 3 argsy
        

        // Po wciśnięciu trenuj (czy nie observable hmm ????????)
        // Shuffle (pierwsze ułóż te z Knowledge level = 0)
        public List<Word> PassWordCollection(sbyte word_id, sbyte translation_id, string difficulty)
        {
            // Dodawnie słów powiązanych z językiem 1 i 2
            List<Word> randomWords = new List<Word>();
            foreach (var word in Words)
            {
                if ((word.Id_lang == word_id || word.Id_lang == translation_id) && word.Difficulty.Equals(difficulty))
                    randomWords.Add(word);
            }
            //if (difficulty == null)
            //{
            //    foreach (var word in Words)
            //    {
            //        if (word.Id_lang == word_id || word.Id_lang == translation_id)
            //            randomWords.Add(word);
            //    }
            //}
            //else
            //{

            //}
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

        // Koncept podajemy do VM performance usera i uczymy go ile user chce (po sesji aktualizacja w bazie [zmiana częstotliwości słów])
        public ObservableCollection<WordKnowledge> PassUserPerformance(sbyte user_id, sbyte word_id, sbyte translation_id)
        {
            ObservableCollection<WordKnowledge> currentUserPerformance = new ObservableCollection<WordKnowledge>();
            // Załaduj zgodnie z knowledge 
            // if new user(none wpisów) then send empty
            // Ładuj performance if user_id i języki wybrane i inverse(języki wybrane)
            sbyte minLang = Math.Min(word_id, translation_id);
            sbyte maxLang = Math.Max(word_id, translation_id);

            foreach (var wk in WordKnowledges)
            {
                // Podczas pobierania istniejacych krotek chcemy aby niższe poziomy były zduplikowane odpowiednio więcej razy
                // ponieważ w vm będzie tasowanie i w zbiorze będzie większa szansa na trafienie tych których jeszcze nie potrafimy
                // bo będzie ich odpowiednio więcej
                if (wk.Id_user == user_id && minLang == FindWordById(wk.Id_word_front).Id_lang && maxLang == FindWordById(wk.Id_word_back).Id_lang)
                {
                    currentUserPerformance.Add(wk);
                }
            }
            return currentUserPerformance;
        }


        public bool WordKnowledgeExists(WordKnowledge wk) => WordKnowledges.Contains(wk); 
        // UPDperfSet może byc pusty , same nowe , nowe i updated - do delete koment
        // Chyba DONE
        // Dodawanie WK przez SetOf...AddWordKnowledge dodaje last inserted index czy jak nie bylo zadnego indexu (empty list) to doda

        public bool UpdateWordKnowledge(ObservableCollection<WordKnowledge> updatedPerfSet)
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
