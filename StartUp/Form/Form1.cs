using System;
using System.Drawing;

namespace Banana_Chess
{
    internal partial class Form1 : System.Windows.Forms.Form
    {
        // variables common for all of the partial Form1 classes
        private System.ComponentModel.IContainer components = null;
        internal System.Windows.Forms.PictureBox canvas;
        private const int boardWidth = 600; //the width of the board
        private const int chassBoardMargin = 16;
        private static string imagesPathPrefix = System.IO.Directory.GetCurrentDirectory() + "\\chess figures\\";
        private bool mouseDowned = false;
        private int mouseX;
        private int mouseY;
        private const int cellWidth = boardWidth / 8;
        Random randomColorGen = new Random();
        
        // click detecting routine for buttons
        private bool testButtonCollision(visualButton button, int mouseX, int mouseY)
        {
            if (mouseX > button.left && mouseX < button.width + button.left && mouseY > button.top && mouseY < button.top + button.height)
            {

                //handle transparency of the image for the figure
                int relativeX = mouseX - button.left;
                int relativeY = mouseY - button.top;

                if (button.bitmap.GetPixel(relativeX, relativeY).A >= 8)
                {
                    return true;
                }
            }
            return false;
        }

        // click detecting routine for chess figures
        private bool testFigureCollision(IFigure figure, int top, int left, int mouseX, int mouseY)
        {
            int relativeX = mouseX - left * cellWidth;
            int relativeY = mouseY - top * cellWidth;

            if (relativeX >= 0 && relativeY >= 0 &&
                relativeX < possibleFigures[(int)figure.Color, (int)figure.Type].width &&
                relativeY < possibleFigures[(int)figure.Color, (int)figure.Type].height)
            {
                //handle transparency of the image for the figure
                if (possibleFigures[(int)figure.Color, (int)figure.Type].bitmap.GetPixel(relativeX, relativeY).A >= 8)
                {
                    return true;
                }
            }
            return false;
        }

        // classes used by all the parts of Form1 class
        private class visualFigure : visualElement
        {
            internal visualFigure(TypesOfFigures type, ColorsOfFigures color, string prefix) :
                            base(color.ToString() + type.ToString(), prefix, ChessBoard.CellWidth, ChessBoard.CellWidth)
            { }
        }

        private class visualButton : visualElement
        {
            internal int top;
            internal int left;
            //internal Image imgOver;
            internal visualButton(string name, string prefix, int left, int top, int width, int height) : base(name, prefix, width, height)
            {
                this.left = left;
                this.top = top;
                //imgShadow = Image.FromFile(prefix + name + "Over.png");
            }
        }

        private class visualElement
        {
            internal Image image;
            internal Bitmap bitmap;
            internal Image imgShadow;
            internal int width;
            internal int height;
            internal bool clicked = false;
            internal visualElement(string name, string prefix, int width, int height)
            {
                image = Image.FromFile(prefix + name + ".png");
                this.width = width;
                this.height = height;
                bitmap = new Bitmap(image, width, height);
                imgShadow = Image.FromFile(prefix + name + "Shaded.png");
            }
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
