namespace Banana_Chess
{
    internal static class FiguresBox
    {
        private static IFigure[] figuresArray;
        internal static IFigure[] blackFiguresArray;
        internal static IFigure[] whiteFiguresArray;

        static FiguresBox()
        {
            figuresArray = new IFigure[] {
                                new Rook(ColorsOfFigures.white),
                                new Knight(ColorsOfFigures.white),
                                new Bishop(ColorsOfFigures.white),
                                new Queen(ColorsOfFigures.white),
                                new King(ColorsOfFigures.white),
                                new Bishop(ColorsOfFigures.white),
                                new Knight(ColorsOfFigures.white),
                                new Rook(ColorsOfFigures.white),

                                new Pawn(ColorsOfFigures.white),
                                new Pawn(ColorsOfFigures.white),
                                new Pawn(ColorsOfFigures.white),
                                new Pawn(ColorsOfFigures.white),
                                new Pawn(ColorsOfFigures.white),
                                new Pawn(ColorsOfFigures.white),
                                new Pawn(ColorsOfFigures.white),
                                new Pawn(ColorsOfFigures.white),

                                new Rook(ColorsOfFigures.black),
                                new Knight(ColorsOfFigures.black),
                                new Bishop(ColorsOfFigures.black),
                                new Queen(ColorsOfFigures.black),
                                new King(ColorsOfFigures.black),
                                new Bishop(ColorsOfFigures.black),
                                new Knight(ColorsOfFigures.black),
                                new Rook(ColorsOfFigures.black),

                                new Pawn(ColorsOfFigures.black),
                                new Pawn(ColorsOfFigures.black),
                                new Pawn(ColorsOfFigures.black),
                                new Pawn(ColorsOfFigures.black),
                                new Pawn(ColorsOfFigures.black),
                                new Pawn(ColorsOfFigures.black),
                                new Pawn(ColorsOfFigures.black),
                                new Pawn(ColorsOfFigures.black)
                        };

            whiteFiguresArray = new IFigure[16];
            blackFiguresArray = new IFigure[16];

            for (int i = 0; i < 16; i++)
            {
                whiteFiguresArray[i] = figuresArray[i];
                blackFiguresArray[i] = figuresArray[i + 16];
            }
        }

        internal static IFigure[] Figures
        {
            get { return figuresArray; }
        }
    }
}
