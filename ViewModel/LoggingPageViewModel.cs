using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FlashCards.ViewModel
{
    using BaseClasses;
    using FlashCards.Model;
    using FlashCards.DAL.Encje;
    using System.Collections.ObjectModel;
    using System.Windows.Input;

    class LoggingPageViewModel : BaseViewModel
    {
        #region Składowe prywatne
        private Model model = null;
        private User _selectedUserFromList;
        private string _imie;
        private string _nazwisko;
        private ObservableCollection<User> _listOfUsers = null;
        #endregion

        #region Konstruktory

        public LoggingPageViewModel(Model model)
        {
            this.model = model;
            ListOfUsers = this.model.Users;
            // Not null
            this._imie = "";
            this._nazwisko = "";
        }
        public LoggingPageViewModel()
        {
            // Initialize only
        }

        #endregion

        #region Właściwości

        public User SelectedUserFromList
        {
            get { return _selectedUserFromList; }
            set { _selectedUserFromList = value; onPropertyChanged(nameof(SelectedUserFromList)); }
        }

        public string Imie
        {
            get { return _imie; }
            set { _imie = value; onPropertyChanged(nameof(Imie)); }
        }

        public string Nazwisko
        {
            get { return _nazwisko; }
            set { _nazwisko = value; onPropertyChanged(nameof(Nazwisko)); }
        }

        public ObservableCollection<User> ListOfUsers
        {
            get { return _listOfUsers; }
            set { _listOfUsers = value; onPropertyChanged(nameof(ListOfUsers)); }
        }
        #endregion

        #region Metody
        private void ClearForm()
        {
            Imie = "";
            Nazwisko = "";
            SelectedUserFromList = null;
        }

        private void GetUserData(User u)
        {
            Imie = u.Name;
            Nazwisko = u.Surname;
        }

        // Ładuje dane użytkownika przy zmianach kontrolki
        private void list_SelectionChanged(object sender)
        {
            if (SelectedUserFromList != null)
            {
                GetUserData(SelectedUserFromList);
            }
        }
        #endregion

        #region Komendy
        private ICommand _loadUser = null;

        public ICommand LoadUser
        {
            get
            {
                if (_loadUser == null)
                {
                    _loadUser = new RelayCommand(
                        list_SelectionChanged,
                        arg => true
                        );
                }

                return _loadUser;
            }
        }

        private ICommand _addUser = null;

        public ICommand AddUser
        {
            get
            {
                if (_addUser == null)
                {
                    _addUser = new RelayCommand(
                        arg =>
                        {
                            bool IsImieValid = !(string.IsNullOrEmpty(Imie.Trim()));
                            bool IsNazwiskoValid = !(string.IsNullOrEmpty(Nazwisko.Trim()));

                            if (IsImieValid && IsNazwiskoValid)
                            {
                                // Tworzony potem user więc trim jest wewnątrz
                                if (model.AddUserToUsers(Imie, Nazwisko))
                                {
                                    ClearForm();
                                    System.Windows.MessageBox.Show("Użytkownik dodany");
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Użytkownik już istnieje");
                                }
                            }
                            else
                                System.Windows.MessageBox.Show("Niepoprawne dane");

                        },
                        arg => true
                        );
                }
                return _addUser;
            }
        }


        private ICommand _zalogujIDodaj = null;

        public ICommand ZalogujIDodaj
        {
            get
            {
                if (_zalogujIDodaj == null)
                {
                    _zalogujIDodaj = new RelayCommand(
                        arg =>
                        {
                            bool IsImieValid = !(string.IsNullOrEmpty(Imie.Trim()));
                            bool IsNazwiskoValid = !(string.IsNullOrEmpty(Nazwisko.Trim()));

                            if(IsNazwiskoValid && IsImieValid)
                            {
                                // Tworzony potem user więc trim jest wewnątrz
                                if (model.AddUserToUsers(Imie, Nazwisko))
                                {
                                    // Sprawdzamy czy user jest w bazie jeśli tak to mamy jego id jeśli nie to null
                                    sbyte? userTrial = model.PassUserIdIfExists(Imie, Nazwisko);
                                    if (userTrial != null)
                                    {
                                        ClearForm();
                                        Mediator.Notify("GoToTabsPage", userTrial);
                                    }
                                }
                                else
                                {
                                    System.Windows.MessageBox.Show("Użytkownik już istnieje");
                                }
                            }
                            else
                                System.Windows.MessageBox.Show("Niepoprawne dane");

                        },
                        arg => true
                        );
                }
                return _zalogujIDodaj;
            }
        }


        private ICommand _zaloguj = null;

        public ICommand Zaloguj
        {
            get
            {
                if (_zaloguj == null)
                {
                    _zaloguj = new RelayCommand(
                       arg =>
                       {
                           bool IsImieValid = !(string.IsNullOrEmpty(Imie.Trim()));
                           bool IsNazwiskoValid = !(string.IsNullOrEmpty(Nazwisko.Trim()));

                           if (IsNazwiskoValid && IsImieValid)
                           {
                               // Tworzony potem user więc trim jest wewnątrz
                               // Sprawdzamy czy user jest w bazie jeśli tak to mamy jego id jeśli nie to null
                               sbyte? userTrial = model.PassUserIdIfExists(Imie, Nazwisko);
                               if (userTrial != null)
                               {
                                   ClearForm();
                                   Mediator.Notify("GoToTabsPage", userTrial);
                               }
                               else
                               {
                                   System.Windows.MessageBox.Show("Użytkownik nie istnieje");
                               }
                           }
                           else
                               System.Windows.MessageBox.Show("Niepoprawne dane");
                       },
                       arg => true
                    );
                }
                return _zaloguj;
            }
        }
        #endregion
    }
}
