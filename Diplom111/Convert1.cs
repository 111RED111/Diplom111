using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using NAudio.Wave;
using NAudio.FileFormats;
using NAudio.CoreAudioApi;
using NAudio;

namespace Diplom111
{
    //Конвертирование аудио в цифры
    class Convert1
    {
        public static void ProcConvert()
        {
            using (WaveFileReader reader = new WaveFileReader("F:/VSProg/file.wav"))
            {
               // Assert.AreEqual(16, reader.WaveFormat.BitsPerSample, "Only works with 16 bit audio");
                byte[] buffer = new byte[reader.Length];
                int read = reader.Read(buffer, 44, buffer.Length-44); //в wav данные с 44 байта
                //short[] sampleBuffer = new short[read / 2];
                //Buffer.BlockCopy(buffer, 0, sampleBuffer, 0, read);

                Posled.AddPosled(buffer); // передача массива цифр 

                //Console.WriteLine(sampleBuffer);
                //for (int i = 44; i < buffer.Length; i++) //в wav данные с 44 байта
                //{
                //    Console.WriteLine(buffer[i]);
                //    Console.WriteLine(Convert.ToString(buffer[i], 2));
                //    System.Diagnostics.Debug.WriteLine(Convert.ToString(buffer[i], 2));
                //}
            }
        }


    }
}
