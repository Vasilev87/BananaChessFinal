namespace Banana_Chess
{
    internal static class ChessBoard
    {
        private static readonly int widthTable;
        private static int cellWidth;
        private static ICell[,] cells;
        private static bool[,] possMoves;

        static ChessBoard()
        {
            widthTable = 600;  //NOTE: USe 600, because this way the cell size is 75 and this is the size of the Images used to draw the figures
            cellWidth = widthTable / 8;

            cells = new ICell[8, 8];
            possMoves = new bool[8, 8];

            //Set all cells as free

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    cells[x, y] = new Cell(true); //Empty cell
                }
            }
        }

        internal static void resetAllPoss()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 8; y++)
                {
                    possMoves[x, y] = false;
                }
            }
        }

        internal static void repartFigures(ColorsOfFigures firstPlayerColor)
        {
            if (firstPlayerColor == ColorsOfFigures.black)
            {
                for (int x = 0; x < 8; x++)
                {
                    cells[x, 0].MyFigure = FiguresBox.whiteFiguresArray[x];
                    cells[x, 1].MyFigure = FiguresBox.whiteFiguresArray[x + 8];
                    cells[x, 6].MyFigure = FiguresBox.blackFiguresArray[x + 8];
                    cells[x, 7].MyFigure = FiguresBox.blackFiguresArray[x];
                }
            }
            else
            {
                for (int x = 0; x < 8; x++)
                {
                    cells[x, 0].MyFigure = FiguresBox.blackFiguresArray[x];
                    cells[x, 1].MyFigure = FiguresBox.blackFiguresArray[x + 8];
                    cells[x, 6].MyFigure = FiguresBox.whiteFiguresArray[x + 8];
                    cells[x, 7].MyFigure = FiguresBox.whiteFiguresArray[x];
                }
            }
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    cells[x, y].MyFigure.PositionX = x;
                    cells[x, y].MyFigure.PositionY = y;
                }
                for (int y = 6; y < 8; y++)
                {
                    cells[x, y].MyFigure.PositionX = x;
                    cells[x, y].MyFigure.PositionY = y;
                }
            }
        }

        internal static ICell[,] Cells
        {
            get { return cells; }
        }

        internal static bool[,] PossMoves
        {
            get { return possMoves; }
        }

        internal static int BoardWidth
        {
            get { return widthTable; }
        }

        internal static int CellWidth
        {
            get { return cellWidth; }
        }
    }
}