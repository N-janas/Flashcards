using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlashCards.ViewModel
{
    public static class Mediator
    {
        private static IDictionary<string, List<Action<object>>> instructionsDict =
            new Dictionary<string, List<Action<object>>>(); /*słownik który trzyma w sobie odnośniki
            do obiektów, w naszym wypadku do konstruktorów UserControl*/

        public static void Subscribe(string token, Action<object> callback)
        {
            /*Metoda słuząca do wpisywania obiektów do słownika*/
            if (!instructionsDict.ContainsKey(token))
            {
                var lista = new List<Action<object>>();
                lista.Add(callback);
                instructionsDict.Add(token, lista);
            }
            else
            {
                bool found = false;
                foreach (var item in instructionsDict[token])
                {
                    if (item.Method.ToString() == callback.Method.ToString())
                        found = true;
                }

                if (!found)
                {
                    instructionsDict[token].Add(callback);
                }
            }
        }

        public static void Unsubscrive(string token, Action<object> callback)
        {
            /*Metoda która usuwa obiekty ze słownika, nigdy nie wykorzystana w naszym projekcie, zostawiona dla spójności*/
            if (instructionsDict.ContainsKey(token))
                instructionsDict[token].Remove(callback);
        }

        public static void Notify(string token, object args = null)
        {
            /* Najczęściej wykorzystywana metoda, słuząca do zmiany widoku interfejsu uzytkownika,
            ma mozliwość wywołania kontstruktora z odpowiednimi parametrami
            w dalszym rozwoju planujemy podmienić konstruktory na referencje do istniejących obiektów*/
            if (instructionsDict.ContainsKey(token))
            {
                foreach (var callback in instructionsDict[token])
                    callback(args);
            }
        }
    }
}
