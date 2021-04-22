using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Diplom111
{
    // тут хранится двоичная последовательность для ключа игрока
    class KeyPlayer:Key
    {
        public KeyPlayer(int keyplayerlength) : base(keyplayerlength)
        {

        }

        public override void AddBitArray(BitArray addkey) // добавляем биты в последовательность
        {
            if (addkey.Length+index <= key.Length) // длина добавляемого ключа + длина уже добавленнного должна быть меньше размера всего ключа
            {
                for (int i = 0; i < addkey.Length; i++)
                {
                    bool bit = addkey.Get(i); // берём бит из добавляемой последовательности
                    key.Set(index, bit); //добавляем бит в конец послеовательности
                    index++;
                }
                if (index==key.Length) // сохранение в пул
                {
                    Pool.AddKeyInPool(key); // добавление ключа в пул
                    index = 0; // типо очистка (длина=0)
                }
            }
            else
            {
                BitArray part1 = new BitArray(key.Length - index); // часть, которая дополнит ключ до целого
                BitArray part2 = new BitArray(addkey.Length - part1.Length); // всё что осталось после разделения
                AddBitArray(part1); // дополняем ключ до целого 
                AddBitArray(part2); // составляем дальнейшие ключи
            }
        }
    }
}
