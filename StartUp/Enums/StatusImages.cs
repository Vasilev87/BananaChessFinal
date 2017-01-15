using System.Drawing;

namespace Banana_Chess
{
    internal static class StatusImages
    {
        private static string basePath = System.IO.Directory.GetCurrentDirectory() + "\\chess figures\\";
        private static Image[] statusImages = new Image[]
        {
        Image.FromFile(basePath + "offline.png"), // olsUnknown = -1,
        Image.FromFile(basePath + "offline.png"), // olsOffline = 0,
        Image.FromFile(basePath + "online.png"), // olsOnline = 1,
        Image.FromFile(basePath + "away.png"), // olsAway = 2,
        Image.FromFile(basePath + "offline.png"), // olsNotAvailable = 3,
        Image.FromFile(basePath + "dontDisturb.png"), // olsDoNotDisturb = 4,
        Image.FromFile(basePath + "offline.png"), // olsSkypeOut = 5,
        Image.FromFile(basePath + "online.png"), // olsSkypeMe = 6
        };

        internal static Image GetStatusImage(SKYPE4COMLib.TOnlineStatus status)
        {
            return statusImages[(int)status + 1];   // shift the indexes
        }
    }
}
