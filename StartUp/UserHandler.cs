using System;
using System.Collections.Generic;
using System.Timers;

namespace Banana_Chess
{
    internal static class UsersHandler
    {
        internal static List<User> users;   //for the connection code which uses safe    foreach()
        private static List<User> newUsers;
        internal static User oponent;
        internal static InvitationOptions defaultInvOptions = new InvitationOptions();

        static System.Drawing.Image img = null; //do not simplify prefix to know what belongs to what
        private static bool repeated;
        private static System.Timers.Timer syncTimer;

        static UsersHandler()
        {
            users = new List<User>();
            newUsers = new List<User>();
        }

        internal static void startUpdatingUsersThread()
        {
            //start main timer responsible to update statuses and invitations of the users in the users' list
            syncAllUsers(null, null);
            syncTimer = new Timer(1000); //each 5 secconds update the status and invitations of users
            syncTimer.Elapsed += new ElapsedEventHandler(syncAllUsers);
            syncTimer.Start();
        }

        internal delegate void notifyFormDelegate();

        internal static event notifyFormDelegate notifyUsersUpdated;

        private static void syncAllUsers(object sender, ElapsedEventArgs e)
        //private static void syncAllUsers()
        {
            newUsers.Clear();

            System.Collections.IEnumerable skypeUsers = SkypeHandler.getUsersList();

            foreach (SKYPE4COMLib.User user in skypeUsers)
            {
                // find out which users are already loaded and for them update only the status
                repeated = false;

                foreach (User currentUser in users)
                {
                    if (currentUser.name.Equals(user.Handle))
                    {
                        newUsers.Add(currentUser);
                        newUsers[newUsers.Count - 1].skypeStatus = user.OnlineStatus;
                        // demand for update of the chessPlaying state
                        repeated = true;
                        break;
                    }
                }

                if (!repeated)  //if not repeated, take it form skype
                {
                    img = SkypeHandler.saveAvatarToDisk(user);
                    if (img != null)
                    {
                        newUsers.Add(new User(user.Handle, img, user.OnlineStatus));
                    }
                }
            }
            users = new List<User>(newUsers);

            // tell the form to repaint the users list
            // tell Skype to keep online all current users
            notifyUsersUpdated?.Invoke();
        }

        internal static int UsersCount
        {
            get { return users.Count; }
        }

        internal static User getUserAtIndex(int i)
        {
            if (users.Count > 0)
            {
                return users[i];
            }
            else
            {
                throw new ArgumentException("An index to method getUserAtIndex of class UsersHandler was passed,\n" +
                                            "but there are no users right now. Please be sure to check if there are users online using UsersCount method\n" +
                                            "before calling getUserAtIndex method");
            }
        }
    }
}