namespace Banana_Chess
{
    class Cell : ICell
    {
        private bool empty;
        private IFigure figureOnMe; //Tells if there is a figure on the cell

        public Cell(bool empty)
        {
            this.empty = empty;
        }

        public IFigure MyFigure
        {
            get { return figureOnMe; } //Make sure you dont access figure of an empty cell
            set
            {
                empty = false;
                figureOnMe = value;
            }
        }

        public bool Empty
        {
            get { return empty; }
            set { empty = value; }
        }
    }
}
