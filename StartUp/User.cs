using SKYPE4COMLib;
using System;
using System.Collections.Generic;

namespace Banana_Chess
{
    internal class User
    {
        internal System.Drawing.Image photo;    // the avatar of the user
        private bool invitationMeToUser = false;        // linked by logic in setters/getters
        private bool invitationUserToMe = false;        //
        private int invMeToUserExpirationCount = 0;     //  // this will not let an invitation stands forever in the other users 
        private int invUserToMeExpirationCount = 0;     //  //
        internal InvitationOptions invitation_I_SentToOponentOptions = new InvitationOptions();
        internal InvitationOptions invitationOponentSentTo_Me_Options = new InvitationOptions();
        internal bool markedInForm = false;
        internal readonly string name;
        internal bool playingChess = false;
        internal SKYPE4COMLib.TOnlineStatus skypeStatus;  //no need to test this with set/getters
        internal List<Stream> Streams; // the streams between this user and me
        internal bool hasChessRunning = false;
        private const int defaultInvExpirationCount = 600;
        internal bool tryingToConnect = false;

        internal User(string name, System.Drawing.Image photo, SKYPE4COMLib.TOnlineStatus status)
        {
            this.photo = photo;
            this.skypeStatus = status;
            this.name = name;
            Streams = new List<Stream>();
            UsersHandler.notifyUsersUpdated += decrementCounters;
        }

        internal bool InvitationMeToUser
        {
            get { return invitationMeToUser; }
            set
            {
                if (value)
                    invMeToUserExpirationCount = defaultInvExpirationCount;
                else
                    invMeToUserExpirationCount = 0;
                
                invitationMeToUser = value;
            }
        }

        internal bool InvitationUserToMe
        {
            get { return invitationUserToMe; }
            set
            {
                if (value)
                    invUserToMeExpirationCount = defaultInvExpirationCount;
                else
                    invUserToMeExpirationCount = 0;
                
                invitationUserToMe = value;
            }
        }

        internal void decrementCounters()
        {
            if (invMeToUserExpirationCount > 0) {
                invMeToUserExpirationCount--;
                if (invMeToUserExpirationCount == 0)
                {
                    InvitationMeToUser = false;
                }
            }
            if (invUserToMeExpirationCount > 0)
            {
                invUserToMeExpirationCount--;
                if (invUserToMeExpirationCount == 0)
                {
                    InvitationUserToMe = false;
                }
            }
        }



        internal string getOptionsAsString()
        {
            return ("" + (int)(invitation_I_SentToOponentOptions.ColorPreffered) +
                               invitation_I_SentToOponentOptions.TimePrefferedOut);
        }

        internal void updateUserParamFromINV(string opt)  //receiving options from the stream
        {
            InvitationUserToMe = true;
            invitationOponentSentTo_Me_Options.fromStr(opt);
        }

        internal bool CompatibleInvOptions
        {
            get {
                return invitationMeToUser && InvitationUserToMe &&
                  invitationOponentSentTo_Me_Options == invitation_I_SentToOponentOptions; }
        }

        ~User()
        {
            if (Streams.Count > 0)
            {
                for (int i = 0; i < Streams.Count; i++) {
                    try
                    {
                        Streams[i].stream.Disconnect();
                        Streams[i].stream.Disconnect();
                    }
                    catch { }
                }
            }
            photo.Dispose();
        }
    }

    internal class Stream
    {
        internal ApplicationStream stream;
        private bool streamAlife = true;
        internal string sentPing = "";
        internal int timeOut = 0;
        internal bool pingSent = false;
        
        internal Stream(ApplicationStream stream)
        {
            this.stream = stream;
        }

        internal void Ping()
        {
            if (!pingSent)
            {
                sentPing = DateTime.Now.ToString();
                try
                {
                    stream.Write("PING" + sentPing);
                }
                catch { }
                timeOut = 10;
                pingSent = true;
            }
        }
        internal void Ack(string mess)
        {
            if (mess.Equals(sentPing))
            {
                streamAlife = true;
                pingSent = false;
                timeOut = 100000;
            }
        }
        internal void decTimeOut()
        {
            if (--timeOut < 0)
            {
                streamAlife = false;
            }
        }

        internal bool Alive
        {
            get { return streamAlife; }
        }
    }
}
