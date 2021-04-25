using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Diplom111
{
    // пул всех ключей
    class Pool
    {
        private static LinkedList<BitArray> general_pool; // общий пул ключей

        static Pool()
        {
            general_pool = new LinkedList<BitArray>(); // создание пустого пула
        }

        public static void AddKeyInPool(BitArray key) // добавление ключа в пул
        {
            general_pool.AddLast(key); // добавление ключа в пул
        }

        public static void ClearPool() // очищение пула
        {
            general_pool = new LinkedList<BitArray>(); // создание пустого пула
        }
    }
}
