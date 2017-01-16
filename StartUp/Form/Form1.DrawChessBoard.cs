using System;
using System.Drawing;
using System.Windows.Forms;

namespace Banana_Chess
{
    internal partial class Form1
    {
        /////////////////////////////////////////////////////////////
        //
        //  This part of the Form1 class is drawing the Playing Chess part of the UI
        //
        ////////////////////////////////////////////////////////////

        private Pen nearestSquareBorder = new Pen(Brushes.Black, 4);

        private SolidBrush rectangleFillDark = new SolidBrush(Color.BlueViolet);
        private SolidBrush rectangleFillLight = new SolidBrush(Color.LightCyan);
        private SolidBrush rectangleFillPossLight = new SolidBrush(Color.FromArgb(255, 119, 239, 108));
        private SolidBrush rectangleFillPossDark = new SolidBrush(Color.FromArgb(255, 42, 196, 64));

        private int figureOnFocusIndexX = -1;
        private int figureOnFocusIndexY = -1;

        private int squareOnFocusIndexX;
        private int squareOnFocusIndexY;

        private bool lightSquare = false;
        private double tempDist;
        private double nearDistance;
        private int localTopLeftX;
        private int localTopLeftY;
        private int globalTopLeftX;
        private int globalTopLeftY;

        private Pen nearesrSquareBorder = new Pen(Brushes.Black, 4);

        private void passControlToChessBoard(User oponent)
        {
            ChessBoard.repartFigures(ChessLogic.myColor);

            // connect to oponent
            // reset states 

            loadEventHandlersForChessBoard();
        }

        private void passControlToChessBoardAfterPromote()
        {
            figureOnFocusIndexX = -1;
            figureOnFocusIndexY = -1;

            loadEventHandlersForChessBoard();
        }

        private void loadEventHandlersForChessBoard()
        {
            canvas.Paint += canvas_Paint_ChessTbl_BckGrndFigs;
            canvas.Paint += canvas_Paint_ChessTable;
            canvas.MouseDown += canvas_MouseDown_ChessTable;
            canvas.MouseUp += canvas_MouseUp_ChessTable;
            canvas.MouseMove += canvas_MouseMove_ChessTable;
            SkypeHandler.oponentMoved += repaintOponentMoved;
        }

        private void canvas_Paint_ChessTbl_BckGrndFigs(object sender, PaintEventArgs e)
        {
            if (ChessLogic.myColor == ColorsOfFigures.white)
            {
                lightSquare = true;
            }
            else
            {
                lightSquare = false;
            }

            //draw the backgrond
            e.Graphics.Clear(Color.Teal); //clears the background 
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (lightSquare)
                    {
                        if (ChessBoard.PossMoves[i, j] && mouseDowned)
                        {
                            e.Graphics.FillRectangle(rectangleFillPossLight, i * cellWidth, j * cellWidth, cellWidth, cellWidth);
                        }
                        else
                        {
                            e.Graphics.FillRectangle(rectangleFillLight, i * cellWidth, j * cellWidth, cellWidth, cellWidth);
                        }
                    }
                    else
                    {
                        if (ChessBoard.PossMoves[i, j] && mouseDowned)
                        {
                            e.Graphics.FillRectangle(rectangleFillPossDark, i * cellWidth, j * cellWidth, cellWidth, cellWidth);
                        }
                    }
                    lightSquare = !lightSquare;
                }
                lightSquare = !lightSquare;
            }
            //draw the figures

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (!ChessBoard.Cells[i, j].Empty)
                    {
                        if (i != figureOnFocusIndexX || j != figureOnFocusIndexY)
                        {
                            e.Graphics.DrawImage(possibleFigures[(int)ChessBoard.Cells[i, j].MyFigure.Color,
                                                                 (int)ChessBoard.Cells[i, j].MyFigure.Type].image,
                                                                 i * cellWidth,
                                                                 j * cellWidth,
                                                                 cellWidth,
                                                                 cellWidth);
                        }
                    }
                }
            }
        }

        private void canvas_Paint_ChessTable(object sender, PaintEventArgs e)
        {
            if (mouseDowned)
            {
                nearDistance = 1000;

                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        tempDist = Math.Sqrt((i * cellWidth - globalTopLeftX) * (i * cellWidth - globalTopLeftX) +
                                             (j * cellWidth - globalTopLeftY) * (j * cellWidth - globalTopLeftY));
                        if (tempDist < nearDistance)
                        {
                            nearDistance = tempDist;
                            squareOnFocusIndexX = i;
                            squareOnFocusIndexY = j;
                        }
                    }
                }

                e.Graphics.DrawRectangle(nearesrSquareBorder, squareOnFocusIndexX * cellWidth,
                                                              squareOnFocusIndexY * cellWidth,
                                                              cellWidth, cellWidth);

                e.Graphics.DrawImage(possibleFigures[(int)ChessBoard.Cells[figureOnFocusIndexX, figureOnFocusIndexY].MyFigure.Color,
                                                     (int)ChessBoard.Cells[figureOnFocusIndexX, figureOnFocusIndexY].MyFigure.Type].imgShadow,
                                                     globalTopLeftX,
                                                     globalTopLeftY,
                                                     cellWidth,
                                                     cellWidth);
            }
        }

        private void canvas_MouseDown_ChessTable(object sender, MouseEventArgs e)
        {
            figureOnFocusIndexX = -1;
            figureOnFocusIndexY = -1;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (!ChessBoard.Cells[i, j].Empty)
                    {
                        if (ChessBoard.Cells[i, j].MyFigure.Color == ChessLogic.myColor)
                        {
                            if (testFigureCollision(ChessBoard.Cells[i, j].MyFigure, j, i, e.X, e.Y))
                            {
                                figureOnFocusIndexX = i;
                                figureOnFocusIndexY = j;
                                mouseDowned = true;

                                localTopLeftX = e.X % cellWidth;
                                localTopLeftY = e.Y % cellWidth;

                                globalTopLeftX = e.X - localTopLeftX;
                                globalTopLeftY = e.Y - localTopLeftY;

                                mouseX = e.X;
                                mouseY = e.Y;
                                if (ChessLogic.myTurn)
                                {
                                    ChessLogic.setPossibleCells(i, j);  // mark the possible Cells
                                }
                                else
                                {
                                    ChessBoard.resetAllPoss();
                                }
                                break;
                            }
                        }
                    }
                }
            }
            canvas.Invalidate();
        }

        private void canvas_MouseUp_ChessTable(object sender, MouseEventArgs e)
        {
            mouseDowned = false;
            if (figureOnFocusIndexX >= 0 && ChessBoard.PossMoves[squareOnFocusIndexX, squareOnFocusIndexY])
            {
                // if (ChessLogic.TestIfEndOfGame()) {
                //do what is needed to comunnicate the end of game to the players
                //}

                ChessLogic.MoveFigureFromTo(figureOnFocusIndexX, figureOnFocusIndexY, squareOnFocusIndexX, squareOnFocusIndexY);
                if (ChessBoard.Cells[figureOnFocusIndexX, figureOnFocusIndexY].MyFigure.GetType() == typeof(Pawn) &&
                    squareOnFocusIndexY == 0)
                {
                    //show dialog to change which figure to promote the pawn to
                    canvas.Paint -= canvas_Paint_ChessTable;
                    canvas.MouseDown -= canvas_MouseDown_ChessTable;
                    canvas.MouseUp -= canvas_MouseUp_ChessTable;
                    canvas.MouseMove -= canvas_MouseMove_SendInv;
                    SkypeHandler.oponentMoved -= repaintOponentMoved;

                    loadMouseEventsForPromotePawn();
                }
                else
                {
                    canvas.Invalidate();
                    figureOnFocusIndexX = -1;
                    figureOnFocusIndexY = -1;
                }
            }
            else
            {
                figureOnFocusIndexX = -1;
                figureOnFocusIndexY = -1;
            }
        }

        private void canvas_MouseMove_ChessTable(object sender, MouseEventArgs e)
        {
            if (mouseDowned)
            {
                globalTopLeftX = e.X - localTopLeftX;
                globalTopLeftY = e.Y - localTopLeftY;

                canvas.Invalidate(); //ask for draw
            }
            canvas.Invalidate();
        }

        private void repaintOponentMoved()
        {
            canvas.Paint -= canvas_Paint_ChessTable;
            canvas.Invalidate();
            canvas.Paint += canvas_Paint_ChessTable;
        }
    }
}
