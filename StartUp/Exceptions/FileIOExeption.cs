using System;

namespace Banana_Chess
{
    internal class FileIOExeption : Exception
    {
        public override string ToString()
        {
            return "Failed to create program folder in C:\\ Try to run this program again as super user\nDetails:\n" +
                base.ToString();
        }
    }
}
