using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Diplom111
{
    class Posled
    {
        private static byte[] masbyte; // массив байтов, который везде используем
        static protected Random rnd = new Random(); //рандом, для выбора из массива последовательности

        static Posled() //создание массива байтов
        {
            masbyte = new byte[0];
        }

        public static BitArray GetPosled(int dlina_chasti_posled) // доставание из массива цифр (выбираем байт и достаём рандомный бит)
        {
            BitArray chast_posled = new BitArray(dlina_chasti_posled); // создали массив битов, для формирования двоичной послед

            for (int i=0; i < dlina_chasti_posled; i++)
            {
                int index = rnd.Next(0, masbyte.Length); // выбор байта в массиве
                int bit = rnd.Next(0, 8); // выбор бита в байте
                byte rndbyte = masbyte[index]; // достаём байт
                bool rndbit = (rndbyte & 1 << bit)==0; // достаём бит
                chast_posled.Set(i, rndbit); // запись вывода в массив
            }
            return chast_posled;
        }

        public static void AddPosled(byte[] convertmas) // сохранение конвертированных цифр в виде байтов в массив
                                                        // convertmas-конвертированный из звука массив байт
                                                        // masbyte-массив где хранится всё что наконвертировалл
        {
            int masbyteOriginalLength = masbyte.Length; // текущий размер masbyte
            Array.Resize<byte>(ref masbyte, masbyteOriginalLength + convertmas.Length); // увеличение размера masbyte для добавления convertmas
            Array.Copy(convertmas, 0, masbyte, masbyteOriginalLength, convertmas.Length); // добавление convertmas
        }
    }
}
