using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace Diplom111
{
    // тут хранится двоичная последовательность для ключа
    abstract class Key
    {
        protected int index; // индекс куда записывать след значение в битаррей(длина того, что записано)
        protected BitArray key; // ключ из массива битов
        public Key(int keylength)
        {
            key = new BitArray(keylength); // ключ
        }

        public abstract void AddBitArray(BitArray addkey); // метод добавления битов в ключ

        public BitArray GetKeyArray() // возвращаем собранную последовательность
        {
            BitArray FillArray = new BitArray(index); // заполняем массив "FillArray" массивом "key", что бы не было незаполненных значений
            
            for (int i = 0; i < index; i++)
            {            
                bool bit = key.Get(i); // берём бит из добавляемой последовательности
                FillArray.Set(i, bit); // копирование в массив FillArray
            }
            return FillArray;
        }
    }
}
