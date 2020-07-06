using FlashCards.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.ViewModel
{
    using FlashCards.ViewModel.BaseClasses;
    using Model;
    using System.Windows.Input;
    using DAL.Encje;

    class FlaszkardsViewModel : BaseViewModel
    {
        #region Pola prywatne
        private string _deckTitle = "";
        private sbyte? _loggedUser = null;
        private Model model = null;
        private Deck _selectedDeck;
        ObservableCollection<Deck> setOfDecks = new ObservableCollection<Deck>();
        #endregion

        #region Własności
        public ObservableCollection<Deck> SetOfDecks
        {
            get
            {
                return setOfDecks;
            }
            set
            {
                setOfDecks = value; onPropertyChanged(nameof(SetOfDecks));
            }
        }

        public string DeckTitle
        {
            get { return _deckTitle; }
            set { _deckTitle = value; onPropertyChanged(nameof(DeckTitle)); }
        }


        public Deck SelectedDeck
        {
            get { return _selectedDeck; }
            set { _selectedDeck = value; onPropertyChanged(nameof(SelectedDeck)); }
        }

        #endregion

        #region Konstruktory
        public FlaszkardsViewModel(Model model, sbyte? user)
        {
            this._loggedUser = user;
            this.model = model;
            this.SetOfDecks = this.model.Decks;
        }
        public FlaszkardsViewModel()
        {
            //Initialize only
        }
        #endregion

        #region Metody
        private static Random random = new Random();

        public List<FlipCard> Shuffle(List<FlipCard> list)
        {
            for (int n = list.Count - 1; n > 1; n--)
            {
                int rng = random.Next(n + 1);
                FlipCard value = list[rng];
                list[rng] = list[n];
                list[n] = value;
            }

            return list;
        }

        private void FindMinAndMaxKnowledge(List<FlipCardWithKnowledge> flipCardFullList, out sbyte min, out sbyte max)
        {
            max = 0;
            min = 127;

            foreach (FlipCardWithKnowledge flipCardFull in flipCardFullList)
            {
                if (flipCardFull.Knowledge > max) max = flipCardFull.Knowledge;
                if (flipCardFull.Knowledge < min) min = flipCardFull.Knowledge;
            }
        }

        public List<FlipCardWithKnowledge> createFlipCardsWithKnowledge(List<FlipCard> flipCardList, List<FlipCardKnowledge> flipCardKnowledgeList)
        {
            List<FlipCardWithKnowledge> fullFlipCards = new List<FlipCardWithKnowledge>();

            foreach (FlipCard flipCard in flipCardList)
            {
                sbyte knowledge = 0;
                foreach (FlipCardKnowledge flipCardKnowledge in flipCardKnowledgeList)
                {
                    if (flipCard.Id == flipCardKnowledge.Id_FlipCard)
                        knowledge = flipCardKnowledge.Knowledge;
                }
                fullFlipCards.Add(new FlipCardWithKnowledge(flipCard, knowledge));
            }

            return fullFlipCards;
        }

        public List<FlipCard> CreateQueue(List<FlipCard> flipCardList, List<FlipCardKnowledge> flipCardKnowledgeList, out List<FlipCardWithKnowledge> fcwk)
        {
            List<FlipCardWithKnowledge> fullFlipCards = createFlipCardsWithKnowledge(flipCardList, flipCardKnowledgeList);

            FindMinAndMaxKnowledge(fullFlipCards, out sbyte maxKnowledge, out sbyte minKnowledge);

            fcwk = fullFlipCards;

            sbyte difference = minKnowledge;
            difference -= maxKnowledge;

            sbyte tempDifference = difference;
            sbyte differenceDecreaser = 1;
            while (tempDifference > 5)
            {
                tempDifference /= 2;
                differenceDecreaser += 1;
            }

            // Tworzenie kolejki z której będą losowane słowa
            List<FlipCard> queue = new List<FlipCard>();

            // Utworzenie odpowiedniej liczby duplikatów, zwiększającej prawdopodobieństwo
            // na wylosowanie słówek mniej znanych (wyznacznik knowledgeLevel)
            foreach (FlipCardWithKnowledge flipCardFull in fullFlipCards)
            {
                sbyte ownDifference = difference;
                ownDifference /= differenceDecreaser;
                sbyte repetitions = maxKnowledge;
                repetitions += ownDifference;
                repetitions -= flipCardFull.Knowledge;
                repetitions += 1;

                if (repetitions > 5)
                    repetitions = 5;

                if (repetitions <= 0)
                    repetitions = 1;

                for (int i = 0; i < repetitions; i++)
                {
                    queue.Add(flipCardFull.FlipCard);
                }

            }

            queue = Shuffle(queue);

            foreach (FlipCard item in queue)
            {
                System.Diagnostics.Debug.WriteLine(item);
            }

            return queue;
        }
        private void ClearForm()
        {
            DeckTitle = "";
            SelectedDeck = null;
        }
        // Reaguje na selected
        private void Edit(object obj)
        {
            // Przejście do edycji talii podając wybraną talię
            if (SelectedDeck != null)
                Mediator.Notify("EditFlashCard", SelectedDeck);
            else
                System.Windows.MessageBox.Show("Wybierz talię");
        }
        private void Add(object obj)
        {
            // Jeśli tytuł ma znaki
            if (!string.IsNullOrEmpty(DeckTitle.Trim()))
            {
                if (model.AddDeckToDecks(DeckTitle))
                {
                    ClearForm();
                }
                else
                    System.Windows.MessageBox.Show("Talia już istnieje");
            }
            else
                System.Windows.MessageBox.Show("Niepoprawny tytuł");
        }
        // Reaguje na selected item nie na title
        private void Delete(object obj)
        {
            if (SelectedDeck != null)
            {
                // Usun deck i jego cały content
                if (model.DeleteDeck(SelectedDeck))
                {
                    SelectedDeck = null;
                }
            }
            else
                System.Windows.MessageBox.Show("Wybierz talię");
        }
        // Reaguje na selected
        private void Train(object obj)
        {
            if (SelectedDeck != null)
            {
                List<List<TrainData>> daneTreningowe = new List<List<TrainData>>();

                List<FlipCard> queue = CreateQueue(
                    model.PassFlipCardCollection((sbyte)SelectedDeck.Id),
                    model.PassUserPerformanceFC(_loggedUser, (sbyte)SelectedDeck.Id),
                    out List<FlipCardWithKnowledge> fcwk
                    );

                if (queue.Any())
                {
                    daneTreningowe.Add(queue.Cast<TrainData>().ToList());
                    daneTreningowe.Add(fcwk.Cast<TrainData>().ToList());
                    Mediator.Notify("TrainFC", daneTreningowe);
                }
                else
                    System.Windows.MessageBox.Show("Talia jest pusta");
            }
            else
                System.Windows.MessageBox.Show("Wybierz talię do treningu");
        }
        private void LogOut(object obj)
        {
            Mediator.Notify("Logout", "");
        }

        #endregion

        #region Komendy

        private ICommand _edytuj;

        public ICommand Edytuj
        {
            get
            {
                if (_edytuj == null)
                {
                    _edytuj = new RelayCommand(
                       Edit,
                        arg => true
                    );
                }
                return _edytuj;
            }
        }

        private ICommand _dodaj;

        public ICommand Dodaj
        {
            get
            {
                if (_dodaj == null)
                {
                    _dodaj = new RelayCommand(
                       Add,
                        arg => true
                    );
                }
                return _dodaj;
            }
        }

        private ICommand _usun;

        public ICommand Usun
        {
            get
            {
                if (_usun == null)
                {
                    _usun = new RelayCommand(
                       Delete,
                        arg => true
                    );
                }
                return _usun;
            }
        }

        private ICommand _trenuj;

        public ICommand Trenuj
        {
            get
            {
                if (_trenuj == null)
                {
                    _trenuj = new RelayCommand(
                       Train,
                        arg => true
                    );
                }
                return _trenuj;
            }
        }

        private ICommand _wroc;

        public ICommand Wroc
        {
            get
            {
                if (_wroc == null)
                {
                    _wroc = new RelayCommand(
                       LogOut,
                        arg => true
                    );
                }
                return _wroc;
            }
        }
        #endregion
    }
}
