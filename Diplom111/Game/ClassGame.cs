using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace Diplom111.Game
{
    //Создание игры
    class ClassGame
    {
        PlayerObject Player;
        Point MousePosition;
        //Graphics g;
        Panel p;
        LinkedList<GameObjects> NPC; //лист с npc объектами
        //LinkedList<NPCObjects> NPC;
        int np = 2; //кол-во нпс


        //public ClassGame(Graphics g)
        //{
        //    this.g = g;
        //    Player = new PlayerObject(g);
        //    NPC = new LinkedList<GameObjects>();
        //    for (int i=0; i<10; i++)
        //    {
        //        NPC.AddLast(new NPCObjects(g));
        //    }
        //}

        public ClassGame(Panel p)
        {
            this.p = p;//объект, который создаём запоминает p
        //    Player = new PlayerObject(p.Size);//создание игрока
            NPC = new LinkedList<GameObjects>();//список под нпс
            //NPC = new LinkedList<NPCObjects>();
            for (int i = 0; i < np; i++)
            {
                NPC.AddLast(new NPCObjects(p.Size));//создание нпс
            }
        }

        //Начало игры (отрисовка объекта игрока, запуск потока)
        public void StartGame()
        {
            Thread InstanceCaller = new Thread(new ThreadStart(ThreadMove));
            // Start the thread.
            InstanceCaller.Start();
        }

        //Определение позиции курсора
        public void SetMousePosition(Point mp)
        {
            MousePosition = mp;
            //Console.WriteLine(mp);
        }

        //Сдвиг объектов(поток)
        private void ThreadMove()
        {
            while (true)
            {
                Graphics g = p.CreateGraphics();//переменная, через которую рисуем
                g.Clear(Color.White);//обновление панели
                //Player.MoveObject(MousePosition, p.Size);//движение игрока
                //Player.DrawObject(g);//отрисовка игрока
                for (int i = 0; i < np; i++)
                {
                    try // исключ, если нпс = null
                    {
                        //  NPC.ElementAt(i).Vidno_ne(NPC);//проверка видно объекту другой объект или нет
                        NPC.ElementAt(i).MoveObject(MousePosition, p.Size, NPC);//движение нпс
                        NPC.ElementAt(i).DrawObject(g);//отрисовка нпс
                    }
                    catch(NullReferenceException excep)
                    {
                        continue;
                    }
                    
                }             
                Thread.Sleep(100);//скорость игры
            }
        }

        //Выбор объекта, которым будем управлять 
        public void CreatePlayer(Point MousePosition)
        {
            if (Player == null) //проверка не выбран ли уже объект
            {
                //System.Diagnostics.Debug.WriteLine("mouse");
                //System.Diagnostics.Debug.WriteLine(Convert.ToString(MousePosition.X));
                //System.Diagnostics.Debug.WriteLine(Convert.ToString(MousePosition.Y));

                double mindist = 10000; //мин расстояние
                GameObjects podhodnpc = null;
                for (int i = 0; i < np; i++)
                {
                    Point center = NPC.ElementAt(i).GetCenter(); //получение координта центра нпс
                    double dist = Math.Sqrt(Math.Pow(MousePosition.X - center.X, 2) + Math.Pow(MousePosition.Y - center.Y, 2)); //рассчтё расстояния от курсора до объектов
                    if (mindist > dist) //нахождение ближайшего нпс
                    {
                        mindist = dist;
                        podhodnpc = NPC.ElementAt(i);
                    }
                    
                    //System.Diagnostics.Debug.WriteLine("center");
                    //System.Diagnostics.Debug.WriteLine(Convert.ToString(center.X));
                    //System.Diagnostics.Debug.WriteLine(Convert.ToString(center.Y));
                    //System.Diagnostics.Debug.WriteLine("dist");
                    //System.Diagnostics.Debug.WriteLine(Convert.ToString(dist));

                }

                Player = new PlayerObject(p.Size, podhodnpc);//создание игрока
                NPC.Remove(podhodnpc); //удаление выбранного нпс
                NPC.AddFirst(Player); //добавление игрока (подмена нпс на игрока)
            }
            
        }
    }
}
