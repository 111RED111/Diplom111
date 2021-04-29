using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Windows.Forms;
using Diplom111.Game;

namespace Diplom111
{
    // пул всех ключей
    class Pool
    {
        private static Label kol; // записано сколько сделано ключей

        private static LinkedList<BitArray> general_pool; // общий пул ключей

        static Pool()
        {
            general_pool = new LinkedList<BitArray>(); // создание пустого пула
        }

        public static void AddKeyInPool(BitArray key) // добавление ключа в пул
        {
            if (Pool.GetKolKey() < Math.Min(ClassGame.NujKey, 2000))
            {
                general_pool.AddLast(key); // добавление ключа в пул
            }  
            
            if (kol != null) // вывод кол-ва ключей
            {
                kol.Invoke(new Action(() => kol.Text = "Ключей сгенерировано: " + general_pool.Count));
                //kol.Text = "Ключей сгенерировано: "+general_pool.Count;
            }
        }

        public static int GetKolKey() // возвращаем кол-во ключей для остановки игры при достижении максимума
        {
            return general_pool.Count;
        }

        public static void ClearPool() // очищение пула
        {
            general_pool = new LinkedList<BitArray>(); // создание пустого пула
        }

        public static void SetLabel(Label kolvo) // даём ссылку на место, где писать кол-во ключей
        {
            Pool.kol = kolvo;
        }
    }
}
