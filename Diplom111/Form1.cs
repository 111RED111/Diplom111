using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Diplom111.Game;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Diplom111
{
    public partial class Form1 : Form
    {

        ClassGame Game;
        Recording Record;

        public Form1()
        {
            InitializeComponent();
            Record = new Recording();
            //comboBox1.SelectedItem = "64";
            comboBox1.SelectedIndex = 0;

            Pool.SetLabel(KolKey); // передача метки, что бы можно было записывать

        }

        //Начинаем запись
        private void StartRecord_Click(object sender, EventArgs e)
        {
            Record.StartRecord();
            StopRecord.Enabled = true; // включение второй кнопки при нажатии первой
            StartRecord.Enabled = false; // выключ первой кнопки 
        }

        //Прерываем запись и конвертирование в цифры
        private void StopRecord_Click(object sender, EventArgs e)
        {
            Record.StopRecord();
            Convert1.ProcConvert();
            StopRecord.Enabled = false;
            StartRecord.Enabled = true;
            Convert.Enabled = true;
        }

        //Конвертирование аудио в цифры
        private void Convert_Click(object sender, EventArgs e)
        {
            Convert1.ProcConvert();
            StartGame.Enabled = true;
            Convert.Enabled = false;
        }

        //Рисуем объекты
        private void StartGame_Click(object sender, EventArgs e)
        {            
            Pool.ClearPool(); // очищение пула
            int DlinaKey = int.Parse(comboBox1.Text); // достаём длину ключа из комбобокса
            int NujKey = int.Parse(NujnoKey.Text); // достаём сколько ключей нужно из текстбокса
            Game = new ClassGame(panel1, DlinaKey, NujKey);            
            Game.StartGame();
            //g.FillEllipse(new SolidBrush(Color.Black), 100, 100, 10, 10);
            Convert.Enabled = false;
            StartGame.Enabled = false;
            StopGame.Enabled = true;
            
        }

        //Движение курсора по панели
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (Game != null)
            {
                Game.SetMousePosition(e.Location);
            }            
        }

        //выбор игрока
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            if (Game != null)
            {
                Game.CreatePlayer(e.Location);
            }                
        }

        // закрытие формы
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Game != null)
            {
                Game.Stop();
            }
        }

        // остановка игры по кнопке
        private void StopGame_Click(object sender, EventArgs e)
        {
            if (Game != null)
            {
                Game.Stop();
            }
            Graphics g = panel1.CreateGraphics();//переменная, через которую рисуем
            g.Clear(Color.White);//обновление панели
            Convert.Enabled = true;
            StartGame.Enabled = true;
            StopGame.Enabled = false;
        }


    }
}
