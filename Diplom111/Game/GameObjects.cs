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
        static protected Random rnd = new Random(); //рандом, для характеристик ЕСЛИ ЧТО МОЖНО ЗАМЕНИТЬ РАНДОМ НА ЗВУК!!!!!!!!!!!!
        protected int step; //шаг шара

        protected Key key; // послеодовательность объекта
        protected BitArray StartPosled; // стартовая последовательность, из которой берём параметры

        //Параметры объектов
        protected GameObjects(Size panel_size) //panel_size - размер панели
        {

            center = new Point(rnd.Next(0, panel_size.Width), rnd.Next(0, panel_size.Height));//рандомное место респауна

            StartPosled = Posled.GetPosled(32); // зарезервировали место под все параметры (цвет, размер)
            byte[] byteparam = new byte[4]; // массив байт, для переделывания из массива битов в массив байтов, для всех параметров
            StartPosled.CopyTo(byteparam, 0); // заполнение массива

            radius = byteparam[0]/4; //размер 10-64 пикселей

            if (radius < 10) //если размер из массива пришёл меньше 10, делать объект равным 10
            {
                radius = 10;
            }

            step = 80 - radius; //скорость движения
                        
            color = Color.FromArgb(byteparam[1], byteparam[2], byteparam[3]); // цвет объектов
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
                                List1.Find(target).Value = null; // удалить из списка, кого съели
                                key.AddBitArray(target.GetKey().GetKeyArray()); // тот, кто съедает кого-то получает его последовательность
                                IncRad(target); // вызов увеличения
                                break;
                            }
                        }
                    }
                }
            }
        }

        public Key GetKey() // получить ключ
        {
            return key;
        }

        public virtual void IncRad(GameObjects target) //увеличение размеров объекта и скорости
        {
            radius = radius + (target.radius) / 5; //увеличение радиуса на (радиус цели / 5 )
            if (radius > 64) //радиус не может быть больше 64
            {
                radius = 64;
            }
            step = 80 - radius;
            
        }
    }
}
