using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Diplom111.Game
{
    //Все объекты(NPCList)
    class NPCListObjects:GameObjects
    {
        private double angle_rad; //направление движения
        private int Hstep; //число шагов в одном направлении
        private int radobz; //радиус обзора объекта 

        private LinkedList<GameObjects> WhoEat;//список, тех кто может съесть
        private GameObjects target;//самый приоритетный объект 

        //конструктор, описывает, как создаётся нпс
        public NPCListObjects(Size panel_size) : base(panel_size)//size_p - длина панели
        {
            radobz = 5 * radius; //радиус обзора
            key = new KeyNPC(ClassGame.GetDlinaKey()*10); // создали новый пустой ключ для нпс (нпс может хранить 10 ключей)
            key.AddBitArray(StartPosled); // в ключ записали начальные параметры 
        }

        //Сдвигаем объект
        public override void MoveObject(Point MousePosition, Size sizepanel, LinkedList<GameObjects>List1)
        {

            Vidno_ne(List1); //
            int kod = KudaMove(sizepanel); // kod, что делает объект 1,2,3,4
            KillObj(List1, target); //вызов съедания

            //if (0 > center.Y && 0 > center.X)
            //{
             //   angle_rad = Math.PI * 3 / 2;
             //   Hstep = 1;
            //}

            if (kod == 3) // если при погоне, расстояние меньше шага, идёт проверка
            {
                double targdist = Math.Sqrt(Math.Pow(target.GetCenter().X - center.X, 2) + Math.Pow(target.GetCenter().Y - center.Y, 2)); //targdist дистанция до цели
                if (targdist < step)
                {
                    center.X = target.GetCenter().X;
                    center.Y = target.GetCenter().Y;
                }
                else
                {
                    Hstep--; //шаги
                    double stepx = Math.Cos(angle_rad) * step;  //свдиг по X и Y
                    double stepy = Math.Sin(angle_rad) * step;
                    center.X = (int)(center.X + stepx); //прибавление сдвига к координатам
                    center.Y = (int)(center.Y - stepy);
                }
            }
            else
            {
                Hstep--; //шаги
                double stepx = Math.Cos(angle_rad) * step;  //свдиг по X и Y
                double stepy = Math.Sin(angle_rad) * step;
                center.X = (int)(center.X + stepx); //прибавление сдвига к координатам
                center.Y = (int)(center.Y - stepy);
            }            
        }

        public override void DrawObject(Graphics g)
        {
            base.DrawObject(g); //вызывается DrawObject из GameObjects

            g.DrawEllipse(new Pen(Color.Black), center.X - radobz, center.Y - radobz, 2 * radobz, 2 * radobz);//рисуем радиус обзора объекта

            //SolidBrush redBrush = new SolidBrush(Color.Blue);
            // Create rectangle for ellipse.
            //Rectangle rect = new Rectangle(center.X - radobz, center.Y - radobz, 2 * radobz, 2 * radobz);
            
            //float startAngle = (float)(360-((angle_rad * 180)/Math.PI))+60;
            //float sweepAngle = -120;
            //g.FillPie(redBrush, rect, startAngle, sweepAngle); //рисуем сектор обзора
        }

        public bool YouCanSeeMe(GameObjects NPCList)//проверка видит конкретный нпс,  кого-то или нет
        {
            double d = Math.Sqrt(Math.Pow(NPCList.GetCenter().X - center.X, 2) + Math.Pow(NPCList.GetCenter().Y - center.Y, 2)); //расчёт расстояние между окружностями(объект и обзор) 
            if (d <= (NPCList.GetRadius() + radobz))//пересечение объекта и обзора
            {
                //double angle = Math.Atan2(NPCList.center.Y - center.Y, NPCList.center.X - center.X);//попадает ли объект в сектор
                //angle = (angle * 180) / Math.PI;//перевод в градусы
                //Console.WriteLine(angle);
                ///System.Diagnostics.Debug.WriteLine("нпс видит");
                return true;
            }
            return false;
        }

        public void Vidno_ne(LinkedList<GameObjects>List1)//проверка видно кого или нет
        {
            WhoEat = new LinkedList<GameObjects>(); //новый список, от кого убегать
            target = null;
            for (int i=0; i<List1.Count; i++)
            {
                GameObjects NPCList_obj = List1.ElementAt(i);//перебор всех нпс
                if (NPCList_obj == null)
                {
                    continue;
                }
                if (NPCList_obj != this)//проверка какой это нпс(что-бы не проверять самого себя)
                {
                    bool vidno = YouCanSeeMe(NPCList_obj);//нпс проверяет видит ли он другой объект   
                    if (vidno == true)
                    {
                        bool nadoubegat = Ubegat(NPCList_obj);
                        if (nadoubegat == true) //надо ли убегать?
                        {
                            WhoEat.AddLast(NPCList_obj); //добавляем в лист 
                        }
                        bool mojnoestb = Estb(NPCList_obj); 
                        if (mojnoestb == true) //можно есть?
                        {
                            if (target == null)
                            {
                                target = NPCList_obj;
                            }
                            else
                            {
                                if (NPCList_obj.GetRadius() > target.GetRadius())//поиск наилучшей цели  МОЖНО ОДИН ИФ
                                {
                                    target = NPCList_obj;//выбирается новая цель
                                }
                            }
                        }
                    }
                }
               
            }
        }

        private bool Ubegat(GameObjects NPCList_obj)//проверка убегать объекту или нет
        {
            double bolshe = (double) NPCList_obj.GetRadius() / this.radius;//радиус объекта, который видно делится с текущим
            //if (bolshe > 1.15)//есть можно, если на 15% меньше
            ///System.Diagnostics.Debug.WriteLine("ubegat");
            ///System.Diagnostics.Debug.WriteLine(Convert.ToString(bolshe));
            if (bolshe > 1.01)//есть можно, если на 1% меньше
            {
                ///System.Diagnostics.Debug.WriteLine("убегать");
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool Estb(GameObjects NPCList_obj)//проверка можно ли съесть или нет
        {
            double bolshe = (double) this.radius / NPCList_obj.GetRadius();//радиус текущего объекта, делится с который видно
            //if (bolshe > 1.15)//есть можно, если на 15% меньше
            ///System.Diagnostics.Debug.WriteLine("Estb");
            ///System.Diagnostics.Debug.WriteLine(Convert.ToString(bolshe));
            if (bolshe > 1.01)//есть можно, если на 1% меньше
            {
               /// System.Diagnostics.Debug.WriteLine("можно есть");
                return true;
            }
            else
            {
                return false;
            }
        }

        private int KudaMove(Size sizepanel) //куда двигается (направление движения) 
            // 1)контроль границ, 2)убегание, 3)погоня, 4)рандомное движение
        {
            bool granb = MoveGranici(sizepanel); // контроль границ
            if (granb == false)
            {
                bool run = MoveUbeganie(); // убегание
                if (run == false)
                {
                    bool hunt = MovePogonia(); // погоня
                    if (hunt == false)
                    {
                        MoveRand(); // рандомное движение
                        return 4; // рандомное движение
                    }
                    return 3; // погоня
                }
                return 2; // убегание
            }
            return 1; // контроль границ
        }

        private bool MoveGranici(Size sizepanel) // контроль границ
        {
            if (sizepanel.Width < center.X) // уехал вправо
            {
                angle_rad = Math.PI;
                Hstep = 1;
                ///System.Diagnostics.Debug.WriteLine("выбрал другое");
                return true;
            }

            if (sizepanel.Height < center.Y) //уехал вниз
            {
                angle_rad = Math.PI / 2;
                Hstep = 1;
                ///System.Diagnostics.Debug.WriteLine("выбрал другое");
                return true;
            }

            //if (sizepanel.Height < center.Y && sizepanel.Width < center.X)
            //{
            //    angle_rad = 2*Math.PI*135/360;
            //    Hstep = 1;
            //}

            if (0 > center.X) //уехал влево
            {
                angle_rad = 0;
                Hstep = 1;
                ///System.Diagnostics.Debug.WriteLine("выбрал другое");
                return true;
            }

            if (0 > center.Y) //уехал вверх
            {
                angle_rad = Math.PI * 3 / 2;
                Hstep = 1;
                ///System.Diagnostics.Debug.WriteLine("выбрал другое");
                return true;
            }
            return false;
        }

        private bool MoveUbeganie() // убегание СДЕЛАТЬ ПРАВИЛЬНО!!!!!!!!!!!!!!! сделан только 0
        {
            if (WhoEat.Count > 0)
            {
                angle_rad = Math.Atan2(center.Y - WhoEat.ElementAt(0).GetCenter().Y, WhoEat.ElementAt(0).GetCenter().X - center.X)-Math.PI; // движение в сторону цели
                Hstep = 5;
                ///System.Diagnostics.Debug.WriteLine("Убэгаiм");
                ///System.Diagnostics.Debug.WriteLine(angle_rad);
                return true;
            }
            return false;
        }

        private bool MovePogonia() // погоня
        {
            if (target != null)
            {
                angle_rad = Math.Atan2(center.Y - target.GetCenter().Y, target.GetCenter().X - center.X); // движение в сторону цели
                Hstep = 5;
                ///System.Diagnostics.Debug.WriteLine("Поiхали");
                ///System.Diagnostics.Debug.WriteLine(angle_rad);
                return true;
            }
            
            return false;
        }

        private void MoveRand() // рандомное движение
        {
            if (Hstep == 0)//выбор направления движения
            {
                int angle_deg = rnd.Next(0, 360); //определение направление движения NPCList
                angle_rad = (angle_deg * Math.PI) / 180;
                Hstep = rnd.Next(5, 20); //определение кол-ва шагов
            }
        }

        public override void IncRad(GameObjects target) // увеличение радиуса обзора 
        {
            base.IncRad(target);
            radobz = 5 * radius;
        }

    }
}
