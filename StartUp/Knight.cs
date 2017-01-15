namespace Banana_Chess
{
    internal class Knight : MovingFigure
    {
        internal Knight(ColorsOfFigures color) : base(TypesOfFigures.Knight, color, new int[] { 2, 1,  2, -1,
                                                                                                -2, 1, -2, -1,
                                                                                                1, 2,  1, -2,
                                                                                                -1, 2, -1, -2 }, 1)
        {
        }
    }
}
