namespace Banana_Chess
{
    internal abstract class Figure : IFigure
    {
        protected TypesOfFigures figureType;
        protected ColorsOfFigures figureColor;
        protected bool onTable;  //Shows if figure is alive
        protected bool imBlocked;   //Set when the figure cannot move in any position/cell
        protected int positionX = 0;
        protected int positionY = 0;

        public abstract void getPossibleCells(bool[,] outArr);

        protected Figure(TypesOfFigures type, ColorsOfFigures color)
        {
            figureType = type;
            figureColor = color;
            onTable = true;
            imBlocked = false; //Initialize this to avoid errors due to weird case of somebody reading this property uninitialized
        }

        protected static bool testIfNextCellIsEmptyAndSetRegard(int i, int j, bool[,] arr, ColorsOfFigures color) //Not used for pawn
        {
            if (i >= 0 && i < 8 && j >= 0 && j < 8)
            {
                if (ChessBoard.Cells[i, j].Empty)
                {
                    arr[i, j] = true; //It is possible
                    return true; //Don't interrupt the iterator inside the caller
                }
                else
                {
                    if (ChessBoard.Cells[i, j].MyFigure.Color != color)
                    {
                        arr[i, j] = true; //It is possible
                    }
                    //Cell was occupied by other color figure and caller needs to stop iteration in this path/direction 
                    //regardless of color, because previous cell is always possible so it shouldn't give a problem
                    return false;
                }
            }
            //Position is not inside the board
            return false;
        }

        public TypesOfFigures Type
        {
            get { return figureType; }
        }

        public ColorsOfFigures Color
        {
            get { return figureColor; }
        }

        public bool OnTable
        {
            get { return onTable; }
            set { onTable = value; }
        }

        public bool Blocked
        {
            get { return imBlocked; }
        }

        public int PositionX
        {
            get { return positionX; }
            set { positionX = value; }
        }

        public int PositionY
        {
            get { return positionY; }
            set { positionY = value; }
        }
    }
}
