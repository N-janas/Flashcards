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
        // Języki
        public ObservableCollection<User> Users { get; set; } = new ObservableCollection<User>();
        public ObservableCollection<Language> Langs { get; set; } = new ObservableCollection<Language>();
        public ObservableCollection<Word> Words { get; set; } = new ObservableCollection<Word>();
        public ObservableCollection<WordKnowledge> WordKnowledges { get; set; } = new ObservableCollection<WordKnowledge>();

        // Fiszki
        public ObservableCollection<FlipCard> FlipCards { get; set; } = new ObservableCollection<FlipCard>();
        public ObservableCollection<FlipCardKnowledge> FlipCardKnowledges { get; set; } = new ObservableCollection<FlipCardKnowledge>();
        public ObservableCollection<Deck> Decks { get; set; } = new ObservableCollection<Deck>();

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
            var flipcards = SetOfFlipCards.GetAllFlipCards();
            foreach (var flipcard in flipcards) FlipCards.Add(flipcard);

            var flipCardKnowledges = SetOfFlipCardKnowledges.GetAllFlipCardKnowledges();
            foreach (var flipCardKwl in flipCardKnowledges) FlipCardKnowledges.Add(flipCardKwl);

            var decks = SetOfDecks.GetAllDecks();
            foreach (var deck in decks) Decks.Add(deck);
        }

        #region Metody fiszek
        public ObservableCollection<FlipCard> PassDeckContent(Deck deck)
        {
            ObservableCollection<FlipCard> tmp = new ObservableCollection<FlipCard>();
            foreach (var fcard in FlipCards)
            {
                if (fcard.Id_Deck == deck.Id)
                {
                    tmp.Add(fcard);
                }
            }

            return tmp;
        }
        // Sprawdza też czy w danej talii
        public bool FlipCardExist(FlipCard fc) => FlipCards.Contains(fc);
        public bool AddFlipCardToFCs(string front, string back, sbyte id_deck)
        {
            FlipCard fc = new FlipCard(front, back, id_deck);

            if (!FlipCardExist(fc))
            {
                if (SetOfFlipCards.AddNewFlipCard(fc))
                {
                    FlipCards.Add(fc);
                    return true;
                }
            }
            return false;
        }

        public bool DeckExist(Deck d) => Decks.Contains(d);
        public bool AddDeckToDecks(string deckName)
        {
            Deck d = new Deck(deckName);

            if (!DeckExist(d))
            {
                if (SetOfDecks.AddNewDeck(d))
                {
                    Decks.Add(d);
                    return true;
                }
            }
            return false;
        }
        public bool DeleteFlipcard(FlipCard flipCard)
        {
            // Przeszukaj knowledege leveli dla flipcarda
            foreach (var knowledge in FlipCardKnowledges.ToList())
            {
                // Jeśli znaleziono usuń
                if (flipCard.Id == knowledge.Id_FlipCard)
                {
                    if (SetOfFlipCardKnowledges.DeleteFlipCardKnowledge(knowledge))
                    {
                        FlipCardKnowledges.Remove(knowledge);
                    }
                }
            }

            if (SetOfFlipCards.DeleteFlipCard(flipCard))
            {
                FlipCards.Remove(flipCard);
                return true;
            }

            return false;
        }

        public bool DeleteDeck(Deck deck)
        {
            // Jeśli deck istnieje
            if (DeckExist(deck))
            {
                // To szukaj czy miał kontent
                foreach (var flipcard in FlipCards.ToList())
                {
                    // Jeśli flipcard należał do talii
                    if (flipcard.Id_Deck == deck.Id)
                    {
                        // Przeszukaj odpowiadających FlipCardKnowledges
                        foreach (var flipcardKnowledge in FlipCardKnowledges.ToList())
                        {
                            // Jeśli znaleziono knowledgeLevel dla tego flipcarda też usuń
                            if (flipcardKnowledge.Id_FlipCard == flipcard.Id)
                            {
                                if (SetOfFlipCardKnowledges.DeleteFlipCardKnowledge(flipcardKnowledge))
                                {
                                    FlipCardKnowledges.Remove(flipcardKnowledge);
                                }
                            }
                        }
                        // Usuń samego flipcarda
                        if (SetOfFlipCards.DeleteFlipCard(flipcard))
                        {
                            FlipCards.Remove(flipcard);
                        }
                    }
                }

                // Usuń samą talie
                if (SetOfDecks.DeleteDeck(deck))
                {
                    Decks.Remove(deck);
                    return true;
                }              
            }
            return false;
        }

        public bool EditFlipCardContent(FlipCard oldFlipCard, FlipCard newFlipCard)
        {
            // Jeśli istnieje to aktualizuj w bazie
            if (!FlipCardExist(newFlipCard))
            {
                if (SetOfFlipCards.EditFlipCard(newFlipCard, (uint)oldFlipCard.Id, newFlipCard.Id_Deck))
                {
                    newFlipCard.Id = oldFlipCard.Id;
                    FlipCards[FlipCards.IndexOf(oldFlipCard)] = newFlipCard;
                    return true;
                }
            }
            return false;
        }

        public void EditDeckTitle(Deck oldDeck, Deck newDeck)
        {
            if(SetOfDecks.EditDeck(newDeck, (sbyte)oldDeck.Id))
            {
                newDeck.Id = oldDeck.Id;
                Decks[Decks.IndexOf(oldDeck)] = newDeck;
            }
        }

        public bool FlipCardKnowledgeExist(FlipCardKnowledge f) => FlipCardKnowledges.Contains(f);
        public void UpdateFlipCardKnowledge(FlipCardKnowledge flipKnowledge)
        {
            if (FlipCardKnowledgeExist(flipKnowledge))
            {
                // Edycja (użycie Update)
                // Znajdź poprzednią wersję
                var oldLevel = FlipCardKnowledges[FlipCardKnowledges.IndexOf(flipKnowledge)];
                // Czy zmienił poziom w danej krotce
                if (oldLevel.Knowledge != flipKnowledge.Knowledge)
                {
                    // Jeśli tak to edytuj w bazie 
                    if (SetOfFlipCardKnowledges.EditFlipCardKnowledge(flipKnowledge, oldLevel.Id))
                    {
                        // i edytuj w kolekcji
                        flipKnowledge.Id = oldLevel.Id;
                        FlipCardKnowledges[FlipCardKnowledges.IndexOf(oldLevel)] = flipKnowledge;
                    }

                }
            }
            else // Jeśli nowa krotka to
            {
                // Dodaj nowe ( użycie SetOf...Add (index się doda))
                if (SetOfFlipCardKnowledges.AddNewFlipCardKnowledge(flipKnowledge))
                {
                    // Dodaj do listy (z indexem)
                    FlipCardKnowledges.Add(flipKnowledge);
                }
            }
        }

        public List<FlipCard> PassFlipCardCollection(sbyte deck_id)
        {
            List<FlipCard> flipCards = new List<FlipCard>();
            foreach (var fc in FlipCards)
            {
                // Uzupełnianie talii
                if (fc.Id_Deck == deck_id)
                    flipCards.Add(fc);
            }
            return flipCards;
        }

        private FlipCard FindFlipCardById(uint id)
        {
            foreach (var f in FlipCards)
            {
                if (f.Id == id)
                    return f;
            }
            return null;
        }

        public List<FlipCardKnowledge> PassUserPerformanceFC(sbyte? user_id, sbyte deck)
        {
            List<FlipCardKnowledge> currentUserPerformance = new List<FlipCardKnowledge>();

            foreach (var fcKnowledge in FlipCardKnowledges)
            {
                // Jeśli krotka jest usera i z danej talii
                if (fcKnowledge.Id_User == user_id && deck == FindFlipCardById(fcKnowledge.Id_FlipCard).Id_Deck)
                {
                    currentUserPerformance.Add(fcKnowledge);
                }
            }
            // Przekazanie krotek z obecnym uczeniem użytkownika
            return currentUserPerformance;
        }
        #endregion

        #region Metody języków
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
        
        public List<Word> PassOtherTranslations(Word origin, Language translation)
        {
            List<Word> others = new List<Word>();
            foreach (var w in Words)
            {
                // Znajdujemy takie samo słowo ale z innym guidem (inne znaczenie)
                if (w.Id_lang == origin.Id_lang && w.WordName.ToLower() == origin.WordName.ToLower() && w.GUID != origin.GUID)
                {
                    foreach (var t in Words)
                    {
                        // Jeśli znaczenie znalezionego słowa ma to samo znaczenie w języku tłumaczenia to dodaj 
                        if( w.GUID == t.GUID && t.Id_lang == translation.Id)
                        {
                            others.Add(t);
                        }
                    }
                }
            }
            return others;
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

        private Word FindWordById(uint id)
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

        public void UpdateWordKnowledge(WordKnowledge knowledge)
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
        #endregion
    }
}
