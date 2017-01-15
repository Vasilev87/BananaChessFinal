using SKYPE4COMLib;
using System;
using System.Collections.Generic;
using System.Timers;
using System.Windows.Forms;

namespace Banana_Chess
{
    internal static partial class SkypeHandler
    {
        private static System.Timers.Timer syncTimer = new System.Timers.Timer();
        private const int secsToWaitToConnect = 2;
        private static bool responseFromAttach = false;
        private static int startSkypeAttempts = 15;
        private static List<TAttachmentStatus> attachStatusses = new List<TAttachmentStatus>();
        internal static event notifySubscribers connectingToSkypeDone;
        
        internal static void ConnectToSkype()
        {
            //make sure skype is running first
            if (!skype.Client.IsRunning)
            {
                //ask user to start skype for him/her/it
                System.Windows.Forms.MessageBox.Show("Skype is not Running. I can start it for you!",
                                                     "Banana chess message",
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Information);
                //send commands to connect in a row
                while (!skype.Client.IsRunning && startSkypeAttempts-- > 0)
                {
                    skype.Client.Start();
                    System.Threading.Thread.Sleep(1000);
                }
                //if not connected yet after 15 sec
                if (!skype.Client.IsRunning)
                {
                    DialogResult promtResult = System.Windows.Forms.MessageBox.Show("Skype is very slow to start",
                                                     "Banana chess message",
                                                     System.Windows.Forms.MessageBoxButtons.AbortRetryIgnore,
                                                     System.Windows.Forms.MessageBoxIcon.Question);

                    if (promtResult == DialogResult.Abort)
                    {
                        // quit chess here
                    }
                }
            }

            try
            {
                // Try setting our custom event handler
                ((_ISkypeEvents_Event)skype).AttachmentStatus += parseAttachmentStatus;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Failed to subscribe to Attach to Skype Event: " + e.Source +
                                                     ",\n Details: " + e.Message,
                                                     "Fataru Erroru",
                                                     System.Windows.Forms.MessageBoxButtons.OK,
                                                     System.Windows.Forms.MessageBoxIcon.Error);
            }

            syncTimer.Elapsed += new ElapsedEventHandler(testIfAttachResponse);
            syncTimer.Interval = secsToWaitToConnect * 1000;
            syncTimer.Start();

            try
            {
                skype.Attach(100, true); //this is a stopper
            }
            catch (FailedToAttachToSkypeException e)
            {
                MessageBox.Show(e.ToString(), "Banana chess message", System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Error);
            }

            initializeEventsForUsersCommunication();

            connectingToSkypeDone?.Invoke();
        }

        private static void testIfAttachResponse(object sender, ElapsedEventArgs e)
        {
            syncTimer.Stop();
            syncTimer.Elapsed -= testIfAttachResponse;
            if (!responseFromAttach)
            {
                if (attachStatusses.Count > 0)
                {
                    if (attachStatusses[attachStatusses.Count - 1] != TAttachmentStatus.apiAttachSuccess)
                    {
                        System.Windows.Forms.MessageBox.Show("waited " + secsToWaitToConnect + " seconds to connect to Skype without response.\n",
                                                             "Fataru Erroru",
                                                             System.Windows.Forms.MessageBoxButtons.OK,
                                                             System.Windows.Forms.MessageBoxIcon.Error);
                        //quit program
                    }
                    else if (attachStatusses[attachStatusses.Count - 1] == TAttachmentStatus.apiAttachPendingAuthorization)
                    {
                        DialogResult promtResult = System.Windows.Forms.MessageBox.Show("Please go to Skype and allow access for this program!",
                                                             "Fataru Erroru",
                                                             System.Windows.Forms.MessageBoxButtons.OKCancel,
                                                             System.Windows.Forms.MessageBoxIcon.Information);
                        if (promtResult == DialogResult.Cancel)
                        {
                            // quit chess here
                        }

                        // wait inside an infinite loop here for user to allow access to the app inside skype
                    }
                    else
                    {
                        // quit chess here
                    }
                } else
                {
                    // quit chess here
                }
            }
        }

        private static void parseAttachmentStatus(TAttachmentStatus Status)
        {
            responseFromAttach = true;
            attachStatusses.Add(Status);
            if (Status == TAttachmentStatus.apiAttachSuccess)
            {
                ((_ISkypeEvents_Event)skype).AttachmentStatus -= parseAttachmentStatus;
            }
        }
    }
}
