namespace Banana_Chess
{
    public interface IFigure
    {
        TypesOfFigures Type { get; } //for end of game calculating purposes
        
        ColorsOfFigures Color { get; }   //we can not change the color of a figure once it is created. there is not such option in the game as changing color of figures

        bool OnTable { get; set; }  //this can be set and get because it can become false when the figure is kicked out of the board

        // other parts in the program will address figures using this Figure class, so it needs to implement those moving methods, not the children classes
        void getPossibleCells(bool [,] arr);  //a method that all figures has to implement in order the chess game to work

        bool Blocked { get; } //needed for the method that checks for Stalemate condition

        int PositionX { get; set; }

        int PositionY { get; set; }
    }
}
