namespace Banana_Chess
{
    internal static class ChessLogic
    {
        internal static int enPassantX = -1;

        internal static bool myTurn;
        internal static bool imPlaying = false;
        private static bool enPassantHappened;
        internal static ColorsOfFigures myColor;
        internal static TypesOfFigures promotedFig;

        internal static void setPossibleCells(int i, int j)
        {
            ChessBoard.resetAllPoss();
            ChessBoard.Cells[i, j].MyFigure.getPossibleCells(ChessBoard.PossMoves);
        }

        /*internal bool checkForEndOfGame()
        {

        }*/

        internal static void promoteTo(int fromX, int fromY, int toX, int toY, TypesOfFigures destType)
        {
            promoteSwitch(toX, toY, destType);

            ChessBoard.Cells[toX, toY].MyFigure.PositionX = toX;
            ChessBoard.Cells[toX, toY].MyFigure.PositionY = toY;

            SkypeHandler.promoteFigure(UsersHandler.oponent, fromX.ToString() +
                                                          fromY.ToString() +
                                                          toX.ToString() +
                                                          toY.ToString() +
                                                          (int)destType);
        }

        internal static void promoteSwitch(int x, int y, TypesOfFigures destType)
        {
            switch (destType)
            {
                case TypesOfFigures.Queen:
                    ChessBoard.Cells[x, y].MyFigure = new Queen(ChessBoard.Cells[x, y].MyFigure.Color);
                    promotedFig = TypesOfFigures.Queen;
                    break;
                case TypesOfFigures.Rook:
                    ChessBoard.Cells[x, y].MyFigure = new Rook(ChessBoard.Cells[x, y].MyFigure.Color);
                    promotedFig = TypesOfFigures.Rook;
                    break;
                case TypesOfFigures.Knight:
                    ChessBoard.Cells[x, y].MyFigure = new Knight(ChessBoard.Cells[x, y].MyFigure.Color);
                    promotedFig = TypesOfFigures.Knight;
                    break;
                case TypesOfFigures.Bishop:
                    ChessBoard.Cells[x, y].MyFigure = new Bishop(ChessBoard.Cells[x, y].MyFigure.Color);
                    promotedFig = TypesOfFigures.Bishop;
                    break;
            }
        }

        internal static void MoveFigureFromTo(int fromX, int fromY, int toX, int toY)
        {
            enPassantHappened = false;
            if (ChessBoard.Cells[fromX, fromY].MyFigure.Type == TypesOfFigures.Pawn)
            {
                // test for en passant condition
                if (enPassantX == toX && toY == 2)
                {
                    // user asks for an en-Passant movement, so delete the pawn that is further
                    ChessBoard.Cells[toX, 3].Empty = true;
                    enPassantHappened = true;
                }
            }

            //TODO: detect if moving a rokado figure and set its Moved to true
            //and yet if rokado figure, move two figures on the board

            ChessBoard.Cells[fromX, fromY].Empty = true;
            ChessBoard.Cells[toX, toY].MyFigure = ChessBoard.Cells[fromX, fromY].MyFigure;
            ChessBoard.Cells[toX, toY].MyFigure.PositionX = toX;
            ChessBoard.Cells[toX, toY].MyFigure.PositionY = toY;

            if (!enPassantHappened)
            {
                SkypeHandler.moveFigure(UsersHandler.oponent, fromX.ToString() +
                                                              fromY +
                                                              toX +
                                                              toY);
            }
            else
            {
                SkypeHandler.moveEnPassant(UsersHandler.oponent, fromX.ToString() +
                                                              fromY +
                                                              toX +
                                                              toY);
            }
            enPassantX = -1;
        }
    }
}
