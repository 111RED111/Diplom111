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
        private PlayerObject Player;
        private Point MousePosition;
        //Graphics g;
        private Panel p;
        private LinkedList<GameObjects> NPCList; //лист с NPCList объектами
        //LinkedList<NPCListObjects> NPCList;
        private int np = 5; //кол-во нпс
        private int food; //кол-во еды

        private Thread GameThread; // игровой поток
        private bool StopGame; // игра остановлена? 

        private static int DlinaKey; 
        public static int NujKey;


        //public ClassGame(Graphics g)
        //{
        //    this.g = g;
        //    Player = new PlayerObject(g);
        //    NPCList = new LinkedList<GameObjects>();
        //    for (int i=0; i<10; i++)
        //    {
        //        NPCList.AddLast(new NPCListObjects(g));
        //    }
        //}

        public ClassGame(Panel p, int DlinaKey, int NujKey) // констркутор игры 
        {
            food = np * 2; //кол-во еды
            this.p = p;//объект, который создаём запоминает p
            ClassGame.DlinaKey = DlinaKey; // сохраняем длину ключа в ClassGame
            ClassGame.NujKey = NujKey; // сохраняем кол-во ключей в ClassGame
            //  Player = new PlayerObject(p.Size);//создание игрока
            NPCList = new LinkedList<GameObjects>();//список под нпс
            //NPCList = new LinkedList<NPCListObjects>();
            for (int i = 0; i < np; i++)
            {
                NPCList.AddLast(new NPCListObjects(p.Size));//создание нпс
            }

            for (int i = 0; i < food; i++)
            {
                NPCList.AddLast(new Food(p.Size));//создание нпс
            }
        }

        //Начало игры (отрисовка объекта игрока, запуск потока)
        public void StartGame()
        {
            StopGame = false;
            GameThread = new Thread(new ThreadStart(ThreadMove));
            // Start the thread.
            GameThread.Start();
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
            while (StopGame == false)
            {
                Graphics g = p.CreateGraphics();//переменная, через которую рисуем
                g.Clear(Color.White);//обновление панели
                //Player.MoveObject(MousePosition, p.Size);//движение игрока
                //Player.DrawObject(g);//отрисовка игрока
                for (int i = 0; i < NPCList.Count; i++)
                {
                    if (NPCList.ElementAt(i) != null) // исключ, если нпс = null
                    {
                        //  NPCList.ElementAt(i).Vidno_ne(NPCList);//проверка видно объекту другой объект или нет
                        NPCList.ElementAt(i).MoveObject(MousePosition, p.Size, NPCList);//движение нпс
                        NPCList.ElementAt(i).DrawObject(g);//отрисовка нпс
                    }                  
                }             
                Thread.Sleep(60);//скорость игры
                AddObj(); //вызов добавления 

                if (Pool.GetKolKey() >= Math.Min(NujKey, 2000)) // выход по достиижению нужного кол-ва ключей (либо 2000, либо что указано в текстбоксе)
                {
                    StopGame = true;
                }
            }
        }

        private bool PlayerInList() //проверка, есть ли игрок в списке
        {
            for (int i = 0; i < np; i++)
            {
                if (NPCList.ElementAt(i) == null) //проверка если нпс в списке нул
                {
                    continue;
                }
                if (NPCList.ElementAt(i) is PlayerObject) //проаерка типа(PlayerObject) элемента в списке
                {
                    return true;
                }
            }
            return false;
        }

        //Выбор объекта, которым будем управлять 
        public void CreatePlayer(Point MousePosition)
        {
            if (PlayerInList()==false) //проверка не выбран ли уже объект
            {
                //System.Diagnostics.Debug.WriteLine("mouse");
                //System.Diagnostics.Debug.WriteLine(Convert.ToString(MousePosition.X));
                //System.Diagnostics.Debug.WriteLine(Convert.ToString(MousePosition.Y));

                double mindist = 10000; //мин расстояние
                GameObjects podhodNPCList = null;
                for (int i = 0; i < np; i++)
                {
                    if (NPCList.ElementAt(i) == null) //проверка если нпс в списке нул
                    {
                        continue;
                    }
                    Point center = NPCList.ElementAt(i).GetCenter(); //получение координта центра нпс
                    double dist = Math.Sqrt(Math.Pow(MousePosition.X - center.X, 2) + Math.Pow(MousePosition.Y - center.Y, 2)); //рассчтё расстояния от курсора до объектов
                    if (mindist > dist) //нахождение ближайшего нпс
                    {
                        mindist = dist;
                        podhodNPCList = NPCList.ElementAt(i);
                    }
                    
                    //System.Diagnostics.Debug.WriteLine("center");
                    //System.Diagnostics.Debug.WriteLine(Convert.ToString(center.X));
                    //System.Diagnostics.Debug.WriteLine(Convert.ToString(center.Y));
                    //System.Diagnostics.Debug.WriteLine("dist");
                    //System.Diagnostics.Debug.WriteLine(Convert.ToString(dist));

                }

                Player = new PlayerObject(p.Size, podhodNPCList);//создание игрока
                NPCList.Remove(podhodNPCList); //удаление выбранного нпс
                NPCList.AddFirst(Player); //добавление игрока (подмена нпс на игрока)

            }
            
        }

        public PlayerObject GetPlayer() //получить игрока, для исп в другом месте
        {
            return Player;
        }

        private void AddObj() //добавление нового объекта, когда кого-то съедают
        {
            for (int i = 0; i < np; i++)
            {
                if (NPCList.ElementAt(i) == null) //проверка если нпс в списке нул(кого-то съели)
                {
                    NPCList.Find(null).Value = new NPCListObjects(p.Size); //создание нпс, вместо съеденных
                }
            }

            for (int i = np; i < np+food; i++)
            {
                if (NPCList.ElementAt(i) == null) //проверка если еда в списке нул(кого-то съели)
                {
                    NPCList.Find(null).Value = new Food(p.Size); //создание еды, вместо съеденных
                }
            }
        }

        public void Stop() // завершение игры
        {
            StopGame = true;
        }

        public static int GetDlinaKey() // возвращаем длину ключа
        {
            return DlinaKey;
        }
    }
}
