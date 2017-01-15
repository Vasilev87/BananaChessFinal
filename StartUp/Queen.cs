namespace Banana_Chess
{
    internal class Queen : MovingFigure
    {
        internal Queen(ColorsOfFigures color) : base(TypesOfFigures.Queen, color, new int[] 
                                                        { 1, 0, -1, 0,  0, -1,  0, 1, //Moves horizontally/vertically
                                                         1, 1, -1, -1, 1, -1, -1, 1  }, 8) //Moves diagonally
        {
        }
    }
}
