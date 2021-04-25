using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;



namespace Diplom111.Game
{
    //Объект игрока
    class PlayerObject:GameObjects
    {
        public PlayerObject(Size panel_size) : base(panel_size) //выбор рандомного обзекта(не используется)
        {

        }

        public PlayerObject(Size panel_size, GameObjects NPC) : base(panel_size) //берём у нпс все параметры
        {
            this.center = NPC.GetCenter();
            this.radius = NPC.GetRadius();
            this.color = NPC.GetColor();
            this.step = NPC.GetStep();
            key = new KeyPlayer(ClassGame.GetDlinaKey()); // создали новый пустой ключ для игрока 256 переделать на значения комбобокса!!!!!!!!!!!!!!!!!!
            key.AddBitArray(NPC.GetKey().GetKeyArray()); // ключь нпс переделываем в ключ игрока
        }

        //Сдвигаем объект
        public override void MoveObject(Point MousePosition, Size sizepanel, LinkedList<GameObjects> List1)
        {


            double dist = Math.Sqrt(Math.Pow(MousePosition.X - center.X, 2) + Math.Pow(MousePosition.Y - center.Y, 2)); //расчёт расстояния

            //Console.WriteLine(dist);

            if (dist <= step) //сдвиг объекта
            {
                center = MousePosition;
            }
            else
            {
                //сдвиг на шаг к курсору
                center.X = (int)(center.X + (MousePosition.X - center.X) * (step / dist));
                center.Y = (int)(center.Y + (MousePosition.Y - center.Y) * (step / dist));
            }

            for (int i = 0; i < List1.Count; i++)
            {
                GameObjects NPCList_obj = List1.ElementAt(i);//перебор всех нпс
                if (NPCList_obj == null)
                {
                    continue;
                }
                if (NPCList_obj != this)//проверка какой это нпс(что-бы не проверять самого себя)
                {
                    KillObj(List1, NPCList_obj); //вызов съедания
                }
            }
        }

    }
}
