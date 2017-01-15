namespace Banana_Chess
{
    interface ICell
    {
        IFigure MyFigure { get; set; }

        bool Empty { get; set; }
    }
}