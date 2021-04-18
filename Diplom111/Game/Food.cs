using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace Diplom111.Game
{
    //еда
    class Food : GameObjects
    {
        public Food(Size panel_size) : base(panel_size)
        {
            color = Color.Black; //характеристики еды
            radius = 5;
        }
        public override void MoveObject(Point MousePosition, Size sizepanel, LinkedList<GameObjects> List1)
        {

        }

    }
}
