using System.Linq;
using SKYPE4COMLib;
using System.IO;
using System.Windows.Forms;

namespace Banana_Chess
{
    internal static partial class SkypeHandler
    {
        private const string appDirectory = @"C:\Banana Chess\";
        private const string imagesDir = appDirectory + @"Users images\";
        internal delegate void notifySubscribers();
        internal static string log = "";

        static System.Drawing.Image img = null;
        static System.IO.FileStream fs = null;
        
        private static SKYPE4COMLib.Skype skype;
        private static SKYPE4COMLib.Application app;
        private static SKYPE4COMLib.Command command;  // at many places the unnecessary prefixes are intentionally not simplified
                                                      // in order to be easier to figure out what is part of what.
                                                      // in this case, we know instantly that ".Command" is part of Skype API

        static SkypeHandler()
        {
            try
            {
                skype = new SKYPE4COMLib.Skype();
                command = new SKYPE4COMLib.Command();
            }
            catch (SkypeDLLInitializationException e)
            {
                MessageBox.Show(e.ToString(), "Banana chess message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            if (!Directory.Exists(imagesDir))
            {
                try
                {
                    System.IO.Directory.CreateDirectory(imagesDir);
                }
                catch (FileIOExeption e)
                {
                    MessageBox.Show(e.ToString(), "Banana chess message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
        }
        
        internal static System.Drawing.Image saveAvatarToDisk(SKYPE4COMLib.User user)
        {
            if (!user.Handle.Equals("echo123"))
            { //filter out the skype test call bot
                string avatarSavePath = imagesDir + user.Handle.Replace(":", "") + ".jpg";  //keep that replace there!!
                
                command = new Command();
                command.Command = "GET USER " + user.Handle + " Avatar 1 " + avatarSavePath; //tell skype to save avatar to disk(no overwriting, no overloading for the OS' file system)
                skype.SendCommand(command);

                if (File.Exists(avatarSavePath))
                {
                    while (true)
                    {
                        try
                        {
                            fs = new FileStream(avatarSavePath, FileMode.Open, FileAccess.Read);
                            img = System.Drawing.Image.FromStream(fs);
                        }
                        catch
                        {
                            continue;
                        }
                        finally
                        {
                            fs.Close();
                        }
                        break;
                    }
                    if (img.Width == 0 || img.Height == 0)
                    {
                        return null;
                    }
                    else
                    {
                        return img;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }

        internal static System.Collections.IEnumerable getUsersList()
        {
            return skype.Friends.OfType<SKYPE4COMLib.User>();
        }
    }
}
