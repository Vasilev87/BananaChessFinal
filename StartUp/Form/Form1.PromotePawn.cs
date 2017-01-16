using System.Drawing;
using System.Windows.Forms;

namespace Banana_Chess
{
    internal partial class Form1
    {
        /////////////////////////////////////////////////////////////
        //
        //  This part of the Form1 class is drawing the menu for promoting the pawns
        //
        ////////////////////////////////////////////////////////////

        private Pen figureChoiceBorder = new Pen(Color.FromArgb(255, 20, 80, 80), 4);
        private SolidBrush figureHoverColor = new SolidBrush(Color.FromArgb(255, 55, 159, 177));

        private const int figureChoiseMargin = 20;
        private const int figureChoiseBtnHeight = 40;
        private const int figureChoiseBtnWidth = 127;
        private const int figureChoiseMenuTop = 50;
        private const int figureChoiseMenuLeft = (boardWidth - figureChoiseMenuWidth) / 2;
        private const int figureChoiseMenuWidth = cellWidth * 4 + figureChoiseMargin * 5;
        private const int figureChoiceMenuHeight = cellWidth + figureChoiseMargin * 3 + selectUsrBtnButtonHeigth - 10;
        private const int figureChoiseBtnTop = figureChoiseMenuTop + figureChoiseMargin * 2 + cellWidth;
        private const int figureChoiseBtnLeft = (boardWidth - figureChoiseBtnWidth) / 2;

        private visualButton figureChoiceBtn = new visualButton("promoteBtn", imagesPathPrefix, figureChoiseBtnLeft, figureChoiseBtnTop, figureChoiseBtnWidth, figureChoiseBtnHeight);
        
        private Point figureChoise1Point1 = new Point(0, figureChoiseMenuTop + figureChoiseMargin - (int)userNotMarkedPen.Width / 2);
        private Point figureChoise1Point2 = new Point(0, figureChoiseMenuTop + figureChoiseMargin - (int)userNotMarkedPen.Width / 2);
        private Point figureChoise1Point3 = new Point(0, figureChoiseMenuTop + figureChoiseMargin + (int)userNotMarkedPen.Width / 2);
        private Point figureChoise1Point4 = new Point(0, figureChoiseMenuTop + figureChoiseMargin + (int)userNotMarkedPen.Width / 2);
        private Point figureChoise1Point5 = new Point(0, figureChoiseMenuTop + figureChoiseMargin + cellWidth - (int)userNotMarkedPen.Width / 2);
        private Point figureChoise1Point6 = new Point(0, figureChoiseMenuTop + figureChoiseMargin + cellWidth + (int)userNotMarkedPen.Width / 2);

        private int selectedFigureInd = 1;
        private int tempLeft;

        private void loadMouseEventsForPromotePawn()
        {
            canvas.Paint += canvas_Paint_FigureChoice;
            canvas.MouseDown += canvas_MouseDown_FigureChoice;
        }

        private void canvas_Paint_FigureChoice(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(userScrollColor, figureChoiseMenuLeft, figureChoiseMenuTop, figureChoiseMenuWidth, figureChoiceMenuHeight);

            e.Graphics.DrawRectangle(figureChoiceBorder, figureChoiseMenuLeft, figureChoiseMenuTop, figureChoiseMenuWidth, figureChoiceMenuHeight);

            if (selectedFigureInd >= 0)
            {
                e.Graphics.FillRectangle(figureHoverColor, figureChoiseMenuLeft + figureChoiseMargin + selectedFigureInd * (cellWidth + figureChoiseMargin), figureChoiseMenuTop + figureChoiseMargin,
                                     cellWidth, cellWidth);
            }

            for (int i = 0; i < 4; i++)
            {

                tempLeft = figureChoiseMenuLeft + figureChoiseMargin + i * (cellWidth + figureChoiseMargin);

                e.Graphics.DrawImage(possibleFigures[(int)ChessLogic.myColor, i + 1].image, tempLeft, figureChoiseMenuTop + figureChoiseMargin,
                                 cellWidth, cellWidth);

                e.Graphics.DrawRectangle(userNotMarkedPen,
                           figureChoiseMenuLeft + figureChoiseMargin + i * (cellWidth + figureChoiseMargin), figureChoiseMenuTop + figureChoiseMargin,
                                 cellWidth, cellWidth);

                figureChoise1Point1.X = tempLeft - (int)userNotMarkedPen.Width / 2;
                figureChoise1Point2.X = tempLeft + cellWidth + (int)userNotMarkedPen.Width / 2;
                figureChoise1Point3.X = tempLeft + cellWidth - (int)userNotMarkedPen.Width / 2;
                figureChoise1Point4.X = tempLeft + (int)userNotMarkedPen.Width / 2;
                figureChoise1Point5.X = tempLeft + (int)userNotMarkedPen.Width / 2;
                figureChoise1Point6.X = tempLeft - (int)userNotMarkedPen.Width / 2;

                e.Graphics.FillPolygon(userItemOrnamentColor, new Point[]{ figureChoise1Point1,
                                                                           figureChoise1Point2,
                                                                           figureChoise1Point3,
                                                                           figureChoise1Point4,
                                                                           figureChoise1Point5,
                                                                           figureChoise1Point6,});
            }

            e.Graphics.DrawImage(figureChoiceBtn.image, figureChoiceBtn.left, figureChoiceBtn.top, figureChoiceBtn.width, figureChoiceBtn.height);
        }

        void canvas_MouseDown_FigureChoice(object sender, MouseEventArgs e)
        {
            if (e.Y > figureChoiseMenuTop + figureChoiseMargin && e.Y < figureChoiseMenuTop + figureChoiseMargin + cellWidth)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (e.X > figureChoiseMenuLeft + figureChoiseMargin + i * (cellWidth + figureChoiseMargin) &&
                        e.X < figureChoiseMenuLeft + figureChoiseMargin + i * (cellWidth + figureChoiseMargin) + cellWidth)
                    {
                        selectedFigureInd = i;
                        break;
                    }
                }
            }

            if (testButtonCollision(figureChoiceBtn, e.X, e.Y))
            {
                ChessLogic.promoteTo(figureOnFocusIndexX, figureOnFocusIndexY, squareOnFocusIndexX, squareOnFocusIndexY, (TypesOfFigures)(selectedFigureInd + 1));

                //remove event listeners for promoting pawns
                canvas.Paint -= canvas_Paint_FigureChoice;
                canvas.MouseDown -= canvas_MouseDown_FigureChoice;

                passControlToChessBoardAfterPromote();
            }

            canvas.Invalidate();
        }
    }
}
