using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Diplom111
{
    // тут хранится двоичная последовательность для ключа нпс
    class KeyNPC:Key
    {
        public KeyNPC(int keynpclength) : base(keynpclength)
        {

        }

        public override void AddBitArray(BitArray addkey) // Добавляем биты в последовательность
        {
            for (int i = 0; i < addkey.Length; i++)
            {
                if (index >= key.Length) // если в нпс больше не помещается последовательность
                {
                    break;
                }
                bool bit = addkey.Get(i); // берём бит из добавляемой последовательности
                key.Set(index, bit); //добавляем бит в конец послеовательности
                index++;
            }
        }
    }
}
