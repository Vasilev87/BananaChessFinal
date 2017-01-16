using System;

namespace Banana_Chess
{
    class FailedToAttachToSkypeException : Exception
    {
        public override string ToString()
        {
            return "Failed to attach to Skype. If you click \"cancel\" I will die silently\nDetails:\n" +
                base.ToString();
        }
    }
}
