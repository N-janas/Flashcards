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
            //jakieś instrukcje
        }
        #endregion

        #region Metody
        private void ClearForm()
        {
            DeckTitle = "";
            SelectedDeck = null;
        }
        // Reague na selected
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
        // Reague na selected
        private void Train(object obj)
        {
            throw new NotImplementedException();
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
