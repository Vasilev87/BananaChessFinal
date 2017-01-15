using System;

namespace Banana_Chess
{
    internal class King : FigureRokade, IKing
    {
        internal King(ColorsOfFigures color) : base(TypesOfFigures.King, color, new int[] { 1, 0, -1, 0,  0, -1,  0, 1,
                                                                                            1, 1, -1, -1, 1, -1, -1, 1}, 1)
        {
        }

        public bool AmIunderCheck()
        {
            throw new NotImplementedException();
        }

        public bool CanIMove()
        {
            throw new NotImplementedException();
        }
    }
}
