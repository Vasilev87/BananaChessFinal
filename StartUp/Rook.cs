namespace Banana_Chess
{
    internal class Rook : FigureRokade
    {
        internal Rook(ColorsOfFigures color) : base(TypesOfFigures.Rook, color, new int[] { 1,  0,
                                                                                            -1,  0,
                                                                                            0, -1,
                                                                                            0,  1 }, 8)
        {
        }
    }
}