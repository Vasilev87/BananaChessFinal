namespace Banana_Chess
{
    internal class Pawn : Figure
    {
        internal Pawn(ColorsOfFigures color) : base(TypesOfFigures.Pawn, color) { }

        public override void getPossibleCells(bool[,] outArr)
        {
            // pawn is a different animal, it needs dedicated moving logic
            if (positionY > 0) //test if not at the finish top line
            {
                //test if possible to move one position to up
                if (ChessBoard.Cells[positionX, positionY - 1].Empty)
                    ChessBoard.PossMoves[positionX, positionY - 1] = true;

                //test if can eat a figures on the side:
                testOnSides(1);
                testOnSides(-1);

                // test if on the first line
                if (positionY == 6)
                    if (ChessBoard.PossMoves[positionX, 5]) //before testing if second step is possible, test if first step was possible
                        if (ChessBoard.Cells[positionX, 4].Empty) //test if second step is possible
                            ChessBoard.PossMoves[positionX, 4] = true;
            }
        }

        private void testOnSides(int dir)
        {
            //avoid out of array error
            if (positionX + dir >= 0 && positionX + dir < 8)
            {
                //test if the figure at the side is empy
                if (!ChessBoard.Cells[positionX + dir, positionY - 1].Empty)
                {
                    //if not empty, test if is the contrary color
                    if (ChessBoard.Cells[positionX + dir, positionY - 1].MyFigure.Color != figureColor)
                    {
                        //mark the cell to the side as possible to move to
                        ChessBoard.PossMoves[positionX + dir, positionY - 1] = true;
                    }
                }
                else  // Yempty
                {
                    //test if en pasant condition could happen
                    if (ChessLogic.enPassantX == positionX + dir && positionY == 3)
                        //mark the cell to the side as possible to move to
                        ChessBoard.PossMoves[ChessLogic.enPassantX, 2] = true;
                }
            }
        }
    }
}
