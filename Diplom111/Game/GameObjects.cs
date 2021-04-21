using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;


namespace Diplom111.Game
{
    //Общий класс объктов
    abstract class GameObjects
    {
        //protected Graphics g; //отрисовка шаров
        protected Point center; //центр шара 
        protected int radius; //размер шара
        protected Color color; //цвет шара
        static protected Random rnd = new Random(); //рандом, для характеристик
        protected int step; //шаг шара

        

        //Параметры объектов
        protected GameObjects(Size panel_size) //panel_size - размер панели
        {

            center = new Point(rnd.Next(0, panel_size.Width), rnd.Next(0, panel_size.Height));//рандомное место респауна

            BitArray posledrad = Posled.GetPosled(8);
            byte[] byterad = new byte[1]; // массив байт, для переделывания из массива битов в массив байтов 
            posledrad.CopyTo(byterad, 0); // заполнение массива

            radius = byterad[0]/4; //размер

            if (radius < 10) //если размер из массива пришёл меньше 10, делать объект равным 10
            {
                radius = 10;
            }

            step = 80 - radius; //скорость движения

            BitArray posledcolor = Posled.GetPosled(24); // часть последовательности под цвет

            byte[] bytecolor = new byte[3]; // массив байт, для переделывания из массива битов в массив байтов 
            posledcolor.CopyTo(bytecolor, 0); // заполнение массива

            color = Color.FromArgb(bytecolor[0], bytecolor[1], bytecolor[2]); // цвет объектов
        }

        //Рисуем объект
        public virtual void DrawObject(Graphics g) 
        {
            g.FillEllipse(new SolidBrush(color), center.X - radius, center.Y - radius, 2 * radius, 2 * radius);//рисуем объект            
        }

        //Возвращает координаты объекта
        public Point GetCenter()
        {
            return center;
        }

        public int GetRadius()
        {
            return radius;
        }

        public Color GetColor()
        {
            return color;
        }

        public int GetStep()
        {
            return step;
        }


        //Сдвигаем объект
        public abstract void MoveObject(Point MousePosition, Size sizepanel, LinkedList<GameObjects>List1);
        //{
        //    int step = 10;
        //    Point MousePosition1 = new Point(MousePosition.X - radius, MousePosition.Y - radius);

        //    double dist = Math.Sqrt(Math.Pow(MousePosition1.X - center.X, 2) + Math.Pow(MousePosition1.Y - center.Y, 2)); //расчёт расстояния

        //    Console.WriteLine(dist);

        //    if (dist <= step) //сдвиг объекта
        //    {
        //        center = MousePosition1;
        //    }
        //    else
        //    {
        //        //сдвиг на шаг к курсору
        //        center.X = (int)(center.X + (MousePosition1.X - center.X) * (step / dist));
        //        center.Y = (int)(center.Y + (MousePosition1.Y - center.Y) * (step / dist));
        //    }
        //}

        protected void KillObj(LinkedList<GameObjects> List1, GameObjects target) //съедание
        {
            if (target != null) //проверка, что кто-то выбран для съедания
            {
                if (radius > target.radius) //проерка кто больше
                {
                    double dist = Math.Sqrt(Math.Pow(center.X - target.GetCenter().X, 2) + Math.Pow(center.Y - target.GetCenter().Y, 2)); //момент съедания(центр круга еды в круге охотника)
                    if (dist < radius) //проверка ^
                    {
                        for (int i = 0; i < List1.Count; i++) //удалить кого съели
                        {
                            if (List1.ElementAt(i) == target)
                            {
                                List1.Find(target).Value = null;
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
