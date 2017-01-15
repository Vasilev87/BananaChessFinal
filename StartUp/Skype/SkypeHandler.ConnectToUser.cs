using System;
using SKYPE4COMLib;

namespace Banana_Chess
{
    internal static partial class SkypeHandler
    {
        private static string appName = "Banana_Chess";

        internal delegate void gameStartedFromOponent(User user, ColorsOfFigures color);
        internal delegate void oponentPromote();

        internal static event gameStartedFromOponent gameStartReceived;

        internal static event oponentPromote oponentMoved;

        internal static void initializeEventsForUsersCommunication()
        {
            try
            {
                skype.ApplicationReceiving += new SKYPE4COMLib._ISkypeEvents_ApplicationReceivingEventHandler(receivingEvent);
                skype.ApplicationStreams += new SKYPE4COMLib._ISkypeEvents_ApplicationStreamsEventHandler(streamEvent);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Failed to subscribe to Skype Connection events: " + e.ToString(), "Banana chess message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                exitWholeApp();
            }

            app = skype.get_Application(appName);

            if (app != null)
            {
                try
                {
                    skype.Application[appName].Create();
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("Error creating an app object: " + e.ToString(), "Banana chess message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
                }
            }
            else
            {
                System.Windows.Forms.MessageBox.Show("Error creating an app object", "Banana chess message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            // each time the users list is updated, try to connect with all users or ping them
            UsersHandler.notifyUsersUpdated += updateConnections;
        }

        private static void updateConnections()
        {
            for (int i = 0; i < UsersHandler.UsersCount; i++)
            {
                if (UsersHandler.users[i].Streams.Count == 0)
                {
                    UsersHandler.users[i].hasChessRunning = false;
                    if (!UsersHandler.users[i].tryingToConnect)
                    {
                        UsersHandler.users[i].tryingToConnect = true;
                        try
                        {
                            //System.Windows.Forms.MessageBox.Show("connecting to: " + UsersHandler.users[i].name);
                            skype.Application[appName].Connect(UsersHandler.users[i].name, false);
                        }
                        catch (Exception e)
                        {
                            // actually this is not a connection problem yet
                            System.Windows.Forms.MessageBox.Show("Failed to connect to: " + UsersHandler.users[i].name + " error: " + e.ToString(), "Banana chess message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Information);
                        }
                    }
                }
                else
                {
                    UsersHandler.users[i].hasChessRunning = true;
                    for (int j = 0; j < UsersHandler.users[i].Streams.Count; j++)
                    {
                        //System.Windows.Forms.MessageBox.Show("saved stream to: " + UsersHandler.users[i].Streams[i].stream.Handle);
                        if (UsersHandler.users[i].Streams[j].Alive)
                        {
                            UsersHandler.users[i].Streams[j].Ping();
                            UsersHandler.users[i].Streams[j].decTimeOut();
                        }
                        else
                        {
                            UsersHandler.users[i].Streams.RemoveAt(j);
                            if (UsersHandler.users[i].Streams.Count == 0)
                            {
                                UsersHandler.users[i].tryingToConnect = false;
                            }
                        }
                    }
                }
            }
        }

        //
        // EVENTs Handlers next:
        //

        private static void receivingEvent(Application pApp, ApplicationStreamCollection pStreams)
        {
            if (pApp.Name.Equals(appName)) {
                foreach (SKYPE4COMLib.ApplicationStream stream in pStreams)
                {
                    for (int i = 0; i < UsersHandler.users.Count; i++)
                    {
                        if (stream.Handle.Contains(UsersHandler.users[i].name))
                        {
                            parseStreamMessOfUser(stream, UsersHandler.users[i]);
                        }
                    }
                }
            }
        }

        private static void streamEvent(Application pApp, ApplicationStreamCollection pStreams)
        {
            if (pApp.Name.Equals(appName))
            {
                foreach (SKYPE4COMLib.ApplicationStream stream in pStreams)
                {
                    for (int i = 0; i < UsersHandler.users.Count; i++)
                    {
                        if (stream.Handle.Contains(UsersHandler.users[i].name))
                        {
                            bool repeated = false;
                            for (int j = 0; j < UsersHandler.users[i].Streams.Count; j++)
                            {
                                if (UsersHandler.users[i].Streams[j].stream.Equals(stream))
                                {
                                    repeated = true;
                                    break;
                                }
                            }
                            if (!repeated)
                            {
                                UsersHandler.users[i].Streams.Add(new Stream(stream));
                            }
                        }
                    }
                }
            }
        }

        internal static void endGameWithUser(User user, ColorsOfFigures color)
        {
            // send end game tokken to the oponent
        }

        private static void parseStreamMessOfUser(ApplicationStream stream, User user)
        {
            string mess = stream.Read();

            if (mess.Length >= 4 && mess.Substring(0, 4).Equals("PING"))
            {
                stream.Write(mess.Replace("PING", "ACK"));
                if (ChessLogic.imPlaying)
                {
                    stream.Write("PLAY");
                }
            }
            else if (mess.Length >= 3 && mess.Substring(0, 3).Equals("ACK"))
            {
                for (int i = 0; i < user.Streams.Count; i++)
                {
                    if (user.Streams[i].stream.Handle.Equals(stream.Handle))
                    {
                        user.Streams[i].Ack(mess.Substring(3, mess.Length - 3));
                    }
                }
            }
            else if (mess.Length >= 4 && mess.Substring(0, 4).Equals("GAME"))
            {
                ColorsOfFigures color = (ColorsOfFigures)int.Parse(mess.Substring(4, 1));
                ChessLogic.myTurn = setStartTurn(color);
                UsersHandler.oponent = user;
                ChessLogic.imPlaying = true;
                gameStartReceived?.Invoke(user, color);
            }
            else if (mess.Length >= 4 && mess.Substring(0, 4).Equals("PLAY"))
            {
                user.playingChess = true;
            }
            else if (mess.Length >= 3 && mess.Substring(0, 3).Equals("INV"))
            {
                if (mess.Equals("INVX"))
                {
                    user.InvitationUserToMe = false;
                }
                else
                {
                    user.updateUserParamFromINV(mess.Substring(3, mess.Length - 3));
                }
            }
            else if (mess.Length >= 4 && mess.Substring(0, 4).Equals("MOVE"))
            {
                ChessLogic.myTurn = !ChessLogic.myTurn;

                mess = mess.Substring(4, mess.Length - 4);

                int fromX = int.Parse(mess[0].ToString());
                int fromY = 7 - int.Parse(mess[1].ToString());
                int toX = int.Parse(mess[2].ToString());
                int toY = 7 - int.Parse(mess[3].ToString());

                if (ChessBoard.Cells[fromX, fromY].MyFigure.Type == TypesOfFigures.Pawn)
                {
                    if (fromY == 1 && toY == 3)
                    {
                        ChessLogic.enPassantX = fromX;
                    } 
                }

                ChessBoard.Cells[toX, toY].MyFigure = ChessBoard.Cells[fromX, fromY].MyFigure;
                ChessBoard.Cells[fromX, fromY].Empty = true;

                oponentMoved?.Invoke();
            }
            else if (mess.Length >= 3 && mess.Substring(0, 3).Equals("PRO"))
            {
                mess = mess.Substring(3, mess.Length - 3);

                int fromX = int.Parse(mess[0].ToString());
                int fromY = 7 - int.Parse(mess[1].ToString());
                int toX = int.Parse(mess[2].ToString());
                int toY = 7 - int.Parse(mess[3].ToString());

                ChessLogic.promoteSwitch(toX, toY, (TypesOfFigures)int.Parse(mess[4].ToString()));
                ChessBoard.Cells[fromX, fromY].Empty = true;

                oponentMoved?.Invoke();
            }
            else if (mess.Length >= 3 && mess.Substring(0, 3).Equals("ENP"))
            {
                ChessLogic.myTurn = !ChessLogic.myTurn;

                mess = mess.Substring(3, mess.Length - 3);

                int fromX = int.Parse(mess[0].ToString());
                int fromY = 7 - int.Parse(mess[1].ToString());
                int toX = int.Parse(mess[2].ToString());
                int toY = 7 - int.Parse(mess[3].ToString());

                ChessBoard.Cells[toX, toY-1].Empty = true;
                ChessBoard.Cells[toX, toY].MyFigure = ChessBoard.Cells[fromX, fromY].MyFigure;
                ChessBoard.Cells[fromX, fromY].Empty = true;

                oponentMoved?.Invoke();
            }
        }

        internal static void sendInvitation(User user)
        {
            for (int i = 0; i < user.Streams.Count; i++)
            {
                user.Streams[i].stream.Write("INV" + user.getOptionsAsString());
            }
        }

        internal static void cancelInvitation(User user)
        {
            for (int i = 0; i < user.Streams.Count; i++)
            {
                user.Streams[i].stream.Write("INVX");
            }
        }

        internal static void moveEnPassant(User user, string movement)
        {
            ChessLogic.myTurn = !ChessLogic.myTurn;

            sendMovement(user, "ENP" + movement);
        }

        internal static void moveFigure(User user, string movement)
        {
            ChessLogic.myTurn = !ChessLogic.myTurn;

            sendMovement(user, "MOVE" + movement);
        }

        internal static void sendMovement(User user, string command)
        {
            for (int i = 0; i < user.Streams.Count; i++)
            {
                user.Streams[i].stream.Write(command);
            }
        }

        internal static void promoteFigure(User user, string movement)
        {
            sendMovement(user, "PRO" + movement);
        }

        internal static void startGameWithUser(User user, int color)
        {
            ChessLogic.myTurn = setStartTurn((ColorsOfFigures)(1 - color));

            for (int i = 0; i < user.Streams.Count; i++)
            {
                user.Streams[i].stream.Write("GAME" + color);
            }
            ChessLogic.imPlaying = true;
            notifyPlaying();
        }

        internal static void notifyPlaying()
        {
            for (int i = 0; i < UsersHandler.UsersCount; i++)
            {
                for (int j = 0; j < UsersHandler.getUserAtIndex(i).Streams.Count; j++)
                {
                    if (UsersHandler.getUserAtIndex(i).hasChessRunning) {
                        UsersHandler.getUserAtIndex(i).Streams[j].stream.Write("PLAY");
                    }
                }
            }
        }

        internal static void exitWholeApp()
        {

        }

        private static bool setStartTurn(ColorsOfFigures color)
        {
            return color == ColorsOfFigures.white;
        }
    }
}
