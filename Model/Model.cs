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
        #region Własności
        public sbyte? CurrentUser { get; set; } = null;
        #endregion

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
        

        // Po wciśnięciu trenuj
        public List<Word> PassWordCollection(sbyte word_id, sbyte translation_id, string difficulty)
        {
            // Dodawnie słów powiązanych z językiem 1 i 2
            List<Word> randomWords = new List<Word>();
            if (difficulty == null)
            {
                foreach (var word in Words)
                {
                    if (word.Id_lang == word_id || word.Id_lang == translation_id)
                        randomWords.Add(word);
                }
            }
            else
            {
                foreach (var word in Words)
                {
                    if ((word.Id_lang == word_id || word.Id_lang == translation_id) && word.Difficulty.Equals(difficulty))
                        randomWords.Add(word);
                }
            }
            return randomWords;
        }

        // Koncept podajemy do VM performance usera i uczymy go ile user chce (po sesji aktualizacja w bazie [zmiana częstotliwości słów])
        public ObservableCollection<WordKnowledge> PassUserPerformance(sbyte user_id)
        {
            ObservableCollection<WordKnowledge> currentUserPerformance = new ObservableCollection<WordKnowledge>();
            // Załaduj zgodnie z knowledge 
            // if new user(none wpisów) then send empty

            return currentUserPerformance;
        }


        public bool WordKnowledgeExists(WordKnowledge wk) => WordKnowledges.Contains(wk);
        // UPDperfSet może byc pusty , same nowe , nowe i updated - do delete koment
        // NOT YET DONEEE
        public bool UpdateWordKnowledge(ObservableCollection<WordKnowledge> updatedPerfSet)
        {
            foreach (var knowledge in updatedPerfSet)
            {
                // Equals ovveride bez sprawdzania level, więc szuka tylko krotki wg.: id_front, id_back, id_user
                // Check czy user już sie tego uczył
                if (WordKnowledgeExists(knowledge))
                {
                    var oldLevel = WordKnowledges[WordKnowledges.IndexOf(knowledge)];
                    // Czy zmienił poziom w danej krotce
                    if (oldLevel.Knowledge != knowledge.Knowledge)
                    {
                        WordKnowledges[WordKnowledges.IndexOf(knowledge)] = knowledge;
                    }
                }
                else
                {
                    WordKnowledges.Add(knowledge);
                }

            }
            return true; // Jaki return ? bo w sumie to guzikiem nie bedzie to chyba typ void ?
        }
        #endregion
    }
}
