namespace Banana_Chess
{
    internal class Bishop : MovingFigure
    {
        internal Bishop(ColorsOfFigures color) : base(TypesOfFigures.Bishop, color, new int[] { 1, 1, -1, -1, 1, -1, -1, 1 }, 8)
        {
        }
    }
} 