using System;

namespace Banana_Chess
{
    internal abstract class FigureRokade : MovingFigure, IRokade
    {
        internal FigureRokade(TypesOfFigures type, ColorsOfFigures color, int[] dirArr, int range) : 
            base(type, color, dirArr, range)
        {
        }

        protected bool moved = false;

        public bool Moved
        {
            get
            {
                return moved;
            }
            set
            {
                moved = true;   //NOTE it is not getting the value, it is only setting it to true always
            }
        }

        public override void getPossibleCells(bool[,] arr)
        {
            base.getPossibleCells(arr);
            if (moved)
            {
                //Code for rokade
                if (figureType == TypesOfFigures.King)
                {

                }
                else
                {

                }
            }
            moved = true;
        }
    }
}
