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
                Game = new ClassGame(panel1);
            }

        //Начинаем запись
        private void button1_Click(object sender, EventArgs e)
        {
            Record.StartRecord();
        }

        //Прерываем запись и конвертирование в цифры
        private void button2_Click(object sender, EventArgs e)
        {
            Record.StopRecord();
            Convert1.ProcConvert();
        }

        //Конвертирование аудио в цифры
        private void button3_Click(object sender, EventArgs e)
        {
            Convert1.ProcConvert();
        }

        //Рисуем объект игрока
        private void button4_Click(object sender, EventArgs e)
        {
            Game.StartGame();
            //g.FillEllipse(new SolidBrush(Color.Black), 100, 100, 10, 10);
        }

        //Движение курсора по панели
        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            Game.SetMousePosition(e.Location);
        }

        //выбор игрока
        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            Game.CreatePlayer(MousePosition);
        }
    }
}
