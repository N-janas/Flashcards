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
    //using FlashCards.DAL.Zbiory;
    using System.Windows.Input;

    class LoggingPageViewModel : BaseViewModel
    {
        #region Składowe prywatne
        //private BaseViewModel _mainViewModel = null;
        private Model model = null;
        private User _selectedUserFromList;
        private string _imie;
        private string _nazwisko;
        private sbyte? _currentUser;
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
            //this._imie = "Pablo";
            //this._nazwisko = "Wpisz nazwisko";
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

        private void SignInAndAdd(object obj)
        {
            //System.Windows.Application.Current.MainWindow.DataContext = new MainViewModel(model);
        }
        private void SignIn(object obj)
        {
            this._currentUser = this.model.PassUserIdIfExists(this._imie, this._nazwisko);
            System.Windows.Application.Current.MainWindow.DataContext = new LanguagesTabViewModel(model);
            //System.Windows.Application.Current.MainWindow.Content = new LanguagesTabViewModel(model);
            if (this._currentUser != null)
            {
                //Console.WriteLine("");
                //System.Windows.Application.Current.MainWindow.DataContext = new LanguagesTabViewModel(model,this._currentUser);
                //new FlashCardsTabViewModel
            }

        }

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
                                    sbyte? userTrial = model.PassUserIdIfExists(Imie, Nazwisko);
                                    if (userTrial != null)
                                    {
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
                               sbyte? userTrial = model.PassUserIdIfExists(Imie, Nazwisko);
                               if (userTrial != null)
                               {
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
