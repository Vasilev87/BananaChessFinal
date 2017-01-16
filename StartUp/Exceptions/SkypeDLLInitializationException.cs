using System;

namespace Banana_Chess
{
    class SkypeDLLInitializationException : Exception
    {
        public override string ToString()
        {
            return "Failed to create an instance of Skype. Yawn to the programmer!\nDetails:\n" +
                base.ToString();
        }
    }
}
