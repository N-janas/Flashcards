using FlashCards.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.ViewModel
{
    using FlashCards.ViewModel.BaseClasses;
    using Model;
    using System.Collections.ObjectModel;
    using System.Windows.Input;
    using DAL.Encje;

    class EditFlaszkardViewModel : BaseViewModel
    {
        #region Pola
        private Model model = null;
        private Deck editedDeck = null;
        private string _deckTitle = "";
        private ObservableCollection<FlipCard> setOfFlipCards = new ObservableCollection<FlipCard>();
        private FlipCard _selectedFlipCard = null;
        private string _front = "";
        private sbyte? _loggedUser = null;
        private string _back = "";
        #endregion Pola

        #region Własności

        public string Back
        {
            get { return _back; }
            set { _back = value; onPropertyChanged(nameof(Back)); }
        }
        public string Front
        {
            get { return _front; }
            set { _front = value; onPropertyChanged(nameof(Front)); }
        }
        public FlipCard SelectedFlipCard
        {
            get { return _selectedFlipCard; }
            set { _selectedFlipCard = value; onPropertyChanged(nameof(SelectedFlipCard)); }
        }
        public string DeckTitle
        {
            get { return _deckTitle; }
            set { _deckTitle = value; onPropertyChanged(nameof(SelectedFlipCard)); }
        }
        public ObservableCollection<FlipCard> SetOfFlipCards
        {
            get
            {
                return setOfFlipCards;
            }
            set
            {
                setOfFlipCards = value; onPropertyChanged(nameof(SetOfFlipCards));
            }
        }

        #endregion

        #region Konstruktory
        public EditFlaszkardViewModel(Model model, Deck deck, sbyte? user)
        {
            this.model = model;
            this._loggedUser = user;
            this.editedDeck = deck;
            DeckTitle = editedDeck.DeckName;
            SetOfFlipCards = model.PassDeckContent(editedDeck);
        }
        public EditFlaszkardViewModel()
        {
            // Initialize Only
        }

        #endregion

        #region Metody
        private void RefreshFlipCards() => SetOfFlipCards = model.PassDeckContent(editedDeck);
        private void LoadFC(FlipCard fc)
        {
            Front = fc.FrontContent;
            Back = fc.BackContent;
        }
        private void list_SelectionChanged(object sender)
        {
            if (SelectedFlipCard != null)
            {
                LoadFC(SelectedFlipCard);
            }
        }
        private void ClearForm()
        {
            Front = "";
            Back = "";
            SelectedFlipCard = null;
        }
        private void Edit(object obj)
        {
            if (SelectedFlipCard != null)
            {
                if (!string.IsNullOrEmpty(Front.Trim()) && !string.IsNullOrEmpty(Back.Trim()))
                {
                    if (model.EditFlipCardContent(SelectedFlipCard, new FlipCard(Front, Back, (sbyte)editedDeck.Id)))
                    {
                        RefreshFlipCards();
                        ClearForm();
                    }
                    else
                        System.Windows.MessageBox.Show("Taka fiszka już istnieje");
                }
                else
                    System.Windows.MessageBox.Show("Wypełnij pola");

            }
            else
                System.Windows.MessageBox.Show("Zaznacz fiszke do edycji");
        }
        private void Add(object obj)
        {
            // Jeśli pola są wypełnione
            if (!string.IsNullOrEmpty(Front.Trim()) && !string.IsNullOrEmpty(Back.Trim()))
            {
                // Dodaj przez model (w modelu jest sprawdzane czy fiszka jest już w tej talii)
                if (model.AddFlipCardToFCs(Front, Back, (sbyte)editedDeck.Id))
                {
                    RefreshFlipCards();
                    ClearForm();
                }
                else
                    System.Windows.MessageBox.Show("Fiszka istnieje już w tej talii");
            }
            else
                System.Windows.MessageBox.Show("Wypełnij pola");
        }
        private void Delete(object obj)
        {
            if (SelectedFlipCard != null)
            {
                if (model.DeleteFlipcard(SelectedFlipCard))
                {
                    SelectedFlipCard = null;
                    RefreshFlipCards();
                }
            }
            else
                System.Windows.MessageBox.Show("Zaznacz fiszke do usunięcia");
        }

        private void LeaveEdition(object obj)
        {
            Deck test = new Deck(DeckTitle);
            bool canChangeTitle = true;

            foreach (var deck in model.Decks)
            {
                if ((deck.Equals(test) && deck.Id != editedDeck.Id) || string.IsNullOrEmpty(DeckTitle.Trim()))
                {
                    // Znaleziono inny deck który ma tą samą nazwę którą chcemy nadać obecnemu lub podany tytuł był pustym stringiem
                    canChangeTitle = false;
                    break;
                }
            }

            if (canChangeTitle)
            {
                model.EditDeckTitle(editedDeck, test);
            }
            else
                System.Windows.MessageBox.Show("Nie można było zmienić nazwy talii");

            // Wrzucamy true aby wrócić do zakładki fiszek
            Mediator.Notify("BackFromEditionFC", true);
        }

        #endregion

        #region Komendy
        private ICommand _loadFC = null;

        public ICommand LoadFlipCard
        {
            get
            {
                if (_loadFC == null)
                {
                    _loadFC = new RelayCommand(
                        list_SelectionChanged,
                        arg => true
                        );
                }

                return _loadFC;
            }
        }


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

        private ICommand _exit;

        public ICommand Exit
        {
            get
            {
                if (_exit == null)
                {
                    _exit = new RelayCommand(
                       LeaveEdition,
                        arg => true
                    );
                }
                return _exit;
            }
        }
        #endregion
    }
}
