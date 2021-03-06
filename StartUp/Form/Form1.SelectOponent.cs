﻿using System;
using System.Drawing;
using System.Windows.Forms;
using System.Timers;

namespace Banana_Chess
{
    internal partial class Form1
    {
        /////////////////////////////////////////////////////////////
        //
        //  This part of the Form1 class is drawing the Select Oponent Menu and handling its usability
        //
        ////////////////////////////////////////////////////////////

        // blue-ish main color of the design
        private static Color selOpBckGroundColor = Color.Teal;

        // select users btn properties
        private const int selectUsrBtnBottomMargin = 40;
        private const int selectUsrBtnButtonWidth = 220;
        private const int selectUsrBtnButtonHeigth = 60;
        private bool usersLoadedFirsTime = false;

        // properties for the scroll box of the users
        private const int userBoxWidth = 440;
        private const int userBoxHeight = 400;
        private const int userBoxTop = 60;
        private const int userBoxLeft = (boardWidth - userBoxWidth) / 2;
        private Pen userBoxBorderPen = new Pen(Brushes.DeepSkyBlue, 8);
        private SolidBrush userBoxBckGroundColor = new SolidBrush(Color.AntiqueWhite);
        private SolidBrush userBoxFakeMaskColor = new SolidBrush(selOpBckGroundColor);
        private readonly int maxUserClickableArea;
        private readonly int minUserClickableArea;

        // properties for the scrolls
        private const int userBoxScrollBarWidth = 20;
        private const int userBoxScrollBarLeft = userBoxLeft + (userBoxWidth - userBoxScrollBarWidth);
        private double scrollDelta = 0;
        private bool scrollNeeded;
        private double scrollBtnHeight;
        private const int scrollBtnMargin = 4;
        private readonly int scrollBtnWidth;
        private const int scrollBtnLeft = userBoxScrollBarLeft + scrollBtnMargin;
        private double scrollBtnTop;
        private SolidBrush userScrollBckgroundColor = new SolidBrush(Color.Azure);
        private SolidBrush userScrollColor = new SolidBrush(Color.LightSkyBlue);
        private SolidBrush userScrollClickedColor = new SolidBrush(Color.FromArgb(255, 0, 150, 150));
        private int wholeUserAreaHeight;
        private double calculableScrollArea;
        private readonly double maxScrollTop;
        private double minScrollTop;
        private double maxDelta;

        private Image loadImg = Image.FromFile(imagesPathPrefix + "load.png");
        private System.Timers.Timer loadingTimer = new System.Timers.Timer((int)(1000/60));
        private float loaderRotation = 0;
        private float rotationStep = 360 / 50;
        private Font drawFont = new System.Drawing.Font("Arial", 24);
        private SolidBrush connectingStringBrush = new SolidBrush(Color.FloralWhite);
        StringFormat drawFormat = new System.Drawing.StringFormat();

        // properties for item displayed inside the user box
        private const int userItemVertMargin = 20;
        private const int userItemHorizMargin = 20;
        private const int userItemHeight = 120;
        private int userItemWidth;
        private const int optInsideUsrItemMargin = 7;
        private Pen photoBorder = new Pen(Brushes.DeepSkyBlue, 2);
        private Pen InvLineColor = new Pen(Color.FromArgb(255, 20, 80, 80));
        private Pen InvLineColorCompat = new Pen(Color.FromArgb(255, 56, 184, 0));
        private Pen userMarkedPen = new Pen(Color.Yellow, 4);                   // linked together
        private static Pen userNotMarkedPen = new Pen(selOpBckGroundColor, 4);  // ////// ////////   //make it static to satisfy Form1.PromotePawn
        private SolidBrush outernOrnamentColor = new SolidBrush(Color.DeepSkyBlue);
        private const int photoMargin = 10;
        private const int photoWidth = userItemHeight - photoMargin * 2;
        private const int userItemLeft = userItemHorizMargin + userBoxLeft;
        private const int photoLeft = userItemLeft + photoMargin;
        private const int ornament2Width = 2;
        private const int iconMargin = 4;
        private int iconLeft;
        private int iconTop;
        private const int iconWidth = 30;
        private SolidBrush userItemOrnamentColor = new SolidBrush(Color.FromArgb(255, 0, 60, 60));
        private SolidBrush userItemBckgroundColor = new SolidBrush(Color.FromArgb(255, 53, 227, 217));
        private const int invHeight = 50;
        private const int invLeft = photoLeft + photoWidth + ornament2Width + invMarginLeft;
        private const int invMarginLeft = 20;
        private const int arrowsHeight = 40;
        private const int leftArrowWidth = 35;
        private const int cancelAcceptWidth = 40;
        private const int rightArrowWidth = 36;
        private const int invMargin = (userItemHeight - (arrowsHeight * 2)) / 3;
        private int cancelAcceptLeft;
        private const int optionsIconWidth = 24;
        private const int optionsIconsLeft = (boardWidth - optionsIconWidth * 8) / 2;
        private readonly int optionsIconsTop;
        private double centerOfCancelX;
        private const double centerOfCancelY = (cancelAcceptWidth / 2) + invMargin;
        private const int cancelAcceptRadius = 18;
        private const int invLeftStartX = invLeft + 18;
        private const int invRightStartX = invLeft + rightArrowWidth - 2;
        private const int invLineWidth = 6;
        private int topDrawingOffset;
        private int clickableAreasIndex;
        private Image leftArrowImg = Image.FromFile(imagesPathPrefix + "leftArrow.png");
        private Image leftArrowImgGreen = Image.FromFile(imagesPathPrefix + "leftArrowGreen.png");

        private Image rightArrowImg = Image.FromFile(imagesPathPrefix + "rigthArrow.png");
        private Image rightArrowImgGreen = Image.FromFile(imagesPathPrefix + "rigthArrowGreen.png");

        private Image invCancelImg = Image.FromFile(imagesPathPrefix + "invCancel.png");
        private Image invCancelImgGreen = Image.FromFile(imagesPathPrefix + "invCancelGreen.png");
        
        private Image invAcceptImg = Image.FromFile(imagesPathPrefix + "invAccept.png");
        private Image invAcceptImgGreen = Image.FromFile(imagesPathPrefix + "invAcceptGreen.png");

        private Image avatarChessBoardImg = Image.FromFile(imagesPathPrefix + "chessboard.png");
        private Image playingChessIcon = Image.FromFile(imagesPathPrefix + "chessboardIcon.png");
        private bool cancelAcceptClicked;

        private int[,] clickableAreas = new int[4, 2];   //in no moment more than 4 users will be simultaneously visible in the scroll user area
        private User[] clicableUsers = new User[4];

        private Point userItemOrnamentPoint1 = new Point(userItemLeft, 0);
        private Point userItemOrnamentPoint2 = new Point(photoLeft + photoWidth + photoMargin, 0);
        private Point userItemOrnamentPoint3 = new Point(photoLeft + photoWidth, 0);
        private Point userItemOrnamentPoint4 = new Point(photoLeft, 0);
        private Point userItemOrnamentPoint5 = new Point(photoLeft, 0);
        private Point userItemOrnamentPoint6 = new Point(userItemLeft, 0);

        private Point userItemOrnament2Point1 = new Point(photoLeft + photoWidth - ornament2Width, 0);
        private Point userItemOrnament2Point2 = new Point(photoLeft + photoWidth + ornament2Width, 0);
        private Point userItemOrnament2Point3 = new Point(photoLeft + photoWidth + ornament2Width, 0);
        private Point userItemOrnament2Point4 = new Point(photoLeft - ornament2Width, 0);
        private Point userItemOrnament2Point5 = new Point(photoLeft + ornament2Width, 0);
        private Point userItemOrnament2Point6 = new Point(photoLeft + photoWidth - ornament2Width, 0);

        private Point userItemOrnament3Point1;
        private Point userItemOrnament3Point2;
        private Point userItemOrnament3Point3;
        private Point userItemOrnament3Point4;
        private Point userItemOrnament3Point5;
        private Point userItemOrnament3Point6;

        private Point userItemOrnament4Point1;
        private Point userItemOrnament4Point2;
        private Point userItemOrnament4Point3;
        private Point userItemOrnament4Point4;
        private Point userItemOrnament4Point5;
        private Point userItemOrnament4Point6;

        private visualButton sendInvitationBtn = new visualButton("selctOpntBtn", imagesPathPrefix, (boardWidth - selectUsrBtnButtonWidth) / 2 + 1, boardWidth - (selectUsrBtnButtonHeigth + selectUsrBtnBottomMargin), selectUsrBtnButtonWidth, selectUsrBtnButtonHeigth);

        private Image[,] invOptionsImgs = new Image[8, 4] {
            { Image.FromFile(imagesPathPrefix + "invWhiteKing.png"), Image.FromFile(imagesPathPrefix + "invWhiteKingLight.png"), null, Image.FromFile(imagesPathPrefix + "invWhiteKingGreen.png")},
            { Image.FromFile(imagesPathPrefix + "invBlackKing.png"), Image.FromFile(imagesPathPrefix + "invBlackKingLight.png"), null, Image.FromFile(imagesPathPrefix + "invBlackKingGreen.png")},
            { Image.FromFile(imagesPathPrefix + "invAnyKing.png"), Image.FromFile(imagesPathPrefix + "invAnyKingLight.png"), null, Image.FromFile(imagesPathPrefix + "invAnyKingGreen.png")},
            { Image.FromFile(imagesPathPrefix + "15min.png"), Image.FromFile(imagesPathPrefix + "15minLight.png"), Image.FromFile(imagesPathPrefix + "15minOrange.png"), Image.FromFile(imagesPathPrefix + "15minGreen.png")},
            { Image.FromFile(imagesPathPrefix + "30min.png"), Image.FromFile(imagesPathPrefix + "30minLight.png"), Image.FromFile(imagesPathPrefix + "15minOrange.png"), Image.FromFile(imagesPathPrefix + "15minGreen.png")},
            { Image.FromFile(imagesPathPrefix + "45min.png"), Image.FromFile(imagesPathPrefix + "45minLight.png"), Image.FromFile(imagesPathPrefix + "15minOrange.png"), Image.FromFile(imagesPathPrefix + "15minGreen.png")},
            { Image.FromFile(imagesPathPrefix + "60min.png"), Image.FromFile(imagesPathPrefix + "60minLight.png"), Image.FromFile(imagesPathPrefix + "15minOrange.png"), Image.FromFile(imagesPathPrefix + "15minGreen.png")},
            { Image.FromFile(imagesPathPrefix + "noClock.png"), Image.FromFile(imagesPathPrefix + "noClockLight.png"), Image.FromFile(imagesPathPrefix + "15minOrange.png"), Image.FromFile(imagesPathPrefix + "15minGreen.png")}
        };

        InvitationOptions tempInvOpt;

        //Note the orden of this array should correspond to the indexes in the enums Types and Colors
        private visualFigure[,] possibleFigures = new visualFigure[,] {
                              { new visualFigure((TypesOfFigures) 0, (ColorsOfFigures) 0, imagesPathPrefix),
                                new visualFigure((TypesOfFigures) 1, (ColorsOfFigures) 0, imagesPathPrefix),
                                new visualFigure((TypesOfFigures) 2, (ColorsOfFigures) 0, imagesPathPrefix),
                                new visualFigure((TypesOfFigures) 3, (ColorsOfFigures) 0, imagesPathPrefix),
                                new visualFigure((TypesOfFigures) 4, (ColorsOfFigures) 0, imagesPathPrefix),
                                new visualFigure((TypesOfFigures) 5, (ColorsOfFigures) 0, imagesPathPrefix) },

                              { new visualFigure((TypesOfFigures) 0, (ColorsOfFigures) 1, imagesPathPrefix),
                                new visualFigure((TypesOfFigures) 1, (ColorsOfFigures) 1, imagesPathPrefix),
                                new visualFigure((TypesOfFigures) 2, (ColorsOfFigures) 1, imagesPathPrefix),
                                new visualFigure((TypesOfFigures) 3, (ColorsOfFigures) 1, imagesPathPrefix),
                                new visualFigure((TypesOfFigures) 4, (ColorsOfFigures) 1, imagesPathPrefix),
                                new visualFigure((TypesOfFigures) 5, (ColorsOfFigures) 1, imagesPathPrefix) }
        };

        public Form1()
        {
            // initialize read only variables
            scrollBtnWidth = userBoxScrollBarWidth - scrollBtnMargin * 2 - (int)userBoxBorderPen.Width / 2;
            optionsIconsTop = sendInvitationBtn.top - optionsIconWidth;

            scrollBtnTop = userBoxTop + scrollBtnMargin + userBoxBorderPen.Width / 2;
            maxScrollTop = scrollBtnTop;

            InvLineColor.Width = invLineWidth;
            InvLineColorCompat.Width = invLineWidth;

            maxUserClickableArea = userBoxTop + userBoxHeight - (int)userBoxBorderPen.Width / 2;
            minUserClickableArea = userBoxTop + (int)userBoxBorderPen.Width / 2;

            userItemOrnament3Point1 = new Point(userItemLeft - (int)userMarkedPen.Width, 0);
            userItemOrnament3Point2 = new Point(photoLeft + photoWidth + photoMargin + (int)userMarkedPen.Width, 0);
            userItemOrnament3Point3 = new Point(photoLeft + photoWidth + photoMargin, 0);
            userItemOrnament3Point4 = new Point(userItemLeft, 0);
            userItemOrnament3Point5 = new Point(userItemLeft, 0);
            userItemOrnament3Point6 = new Point(userItemLeft - (int)userMarkedPen.Width, 0);
            
            // designing code
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            canvas = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(canvas)).BeginInit();
            SuspendLayout();
            canvas.Location = new System.Drawing.Point(chassBoardMargin, chassBoardMargin);
            canvas.Name = "canvas";
            canvas.Size = new System.Drawing.Size(boardWidth, boardWidth);
            canvas.TabIndex = 0;
            canvas.TabStop = false;
            canvas.Paint += new PaintEventHandler(canvas_Paint_SendInv);
            AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(boardWidth + chassBoardMargin * 2, boardWidth + chassBoardMargin * 2);
            Controls.Add(this.canvas);
            Name = "Form1";
            Text = "Banana Chess";
            Load += new System.EventHandler(Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(canvas)).EndInit();
            ResumeLayout(false);
        }

        private void paintMe() {
            if (!usersLoadedFirsTime)
            {  
                // stop drawing the loading screen
                loadingTimer.Elapsed -= rotateDegrees;
                loadingTimer.Stop();
                usersLoadedFirsTime = true;
                canvas.Paint -= canvas_Paint_Loading;

                //calculate the hight of the scroll button
                wholeUserAreaHeight = UsersHandler.UsersCount * (userItemHeight + userItemVertMargin) + userItemVertMargin;
                maxDelta = wholeUserAreaHeight - (userBoxHeight - userBoxBorderPen.Width);

                if (wholeUserAreaHeight > userBoxHeight)
                { //not much friends online
                    calculableScrollArea = userBoxHeight - scrollBtnMargin * 2 - (int)userBoxBorderPen.Width;
                    scrollBtnHeight = calculableScrollArea / ((double)wholeUserAreaHeight / ((double)userBoxHeight));
                    userItemWidth = (userBoxWidth - (userItemHorizMargin * 2)) - userBoxScrollBarWidth;
                    minScrollTop = userBoxHeight + userBoxTop - scrollBtnMargin - userBoxBorderPen.Width / 2 - scrollBtnHeight;
                    scrollNeeded = true;
                }
                else
                {
                    userItemWidth = (userBoxWidth - (userItemHorizMargin * 2));
                    scrollNeeded = false;
                }

                iconLeft = photoLeft + photoWidth - iconMargin - iconWidth;
                iconTop = photoMargin + photoWidth - iconMargin - iconWidth;
                cancelAcceptLeft = userItemLeft + userItemWidth - invMargin - cancelAcceptWidth;
                centerOfCancelX = cancelAcceptLeft + cancelAcceptWidth / 2;

                userItemOrnament4Point1 = new Point(userItemLeft + userItemWidth, 0);
                userItemOrnament4Point2 = new Point(userItemLeft + userItemWidth + (int)userMarkedPen.Width, 0);
                userItemOrnament4Point3 = new Point(userItemLeft + userItemWidth + (int)userMarkedPen.Width, 0);
                userItemOrnament4Point4 = new Point(userItemLeft + userItemWidth - photoWidth - iconMargin * 2, 0);
                userItemOrnament4Point5 = new Point(userItemLeft + userItemWidth - photoWidth - iconMargin * 2 + (int)userMarkedPen.Width, 0);
                userItemOrnament4Point6 = new Point(userItemLeft + userItemWidth, 0);

                // load Select User UI events
                canvas.Paint += new PaintEventHandler(canvas_Paint_SendInv);
                canvas.MouseDown += new MouseEventHandler(canvas_MouseDown_SendInv);
                canvas.MouseMove += new MouseEventHandler(canvas_MouseMove_SendInv);
                canvas.MouseUp += new MouseEventHandler(canvas_MouseUp_SendInv);
                canvas.MouseWheel += new MouseEventHandler(canvas_MouseWheel_SendInv);

                SkypeHandler.gameStartReceived += startGame;
            }
            canvas.Invalidate();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // display loading screen
            canvas.Paint += canvas_Paint_Loading;
            loadingTimer.Elapsed += new System.Timers.ElapsedEventHandler(rotateDegrees);
            loadingTimer.Start();

            // connect to Skype
            UsersHandler.notifyUsersUpdated += paintMe;
            SkypeHandler.connectingToSkypeDone += UsersHandler.startUpdatingUsersThread;
            SkypeHandler.ConnectToSkype();
        }

        // painting methods for the loader screen
        private void rotateDegrees(object sender, ElapsedEventArgs e)
        {
            if (loaderRotation >= 360)
            {
                loaderRotation = 0;
            }
            else
            {
                loaderRotation += rotationStep;
            }
            canvas.Invalidate();
        }

        private void canvas_Paint_Loading(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(Color.DeepSkyBlue);

            e.Graphics.DrawString("Connecting...", drawFont, connectingStringBrush, 210, 370, drawFormat);

            // Set world transform of graphics object to translate.
            e.Graphics.TranslateTransform((float)(boardWidth / 2), (float)(boardWidth / 2));

            // Then to rotate, prepending rotation matrix.
            e.Graphics.RotateTransform(loaderRotation);

            e.Graphics.TranslateTransform(-(float)boardWidth / 2, -(float)boardWidth / 2);

            e.Graphics.DrawImage(loadImg, boardWidth / 2 - 64, boardWidth / 2 - 64, 128, 128);
        }
        
        //================================================================================================
        // methods/events subscribers for selecting oponent from list menu
        //================================================================================================

        // this method will be executed each time somebody calls "canvas.Invalidate()"
        private void canvas_Paint_SendInv(object sender, PaintEventArgs e)
        {
            //this clears the whole canvas by paining a clear color on it
            e.Graphics.Clear(selOpBckGroundColor);

            // NOTE: scrollNeeded is not the same as mouseDowned
            // scrollNeeded says if there are more than 3 oponents to show, in other words if there is point to have a scroll at all
            // mouseDowned says if user requests scrolling
            if (!scrollNeeded)
            {
                //draw the oponents box background
                e.Graphics.FillRectangle(userBoxBckGroundColor, userBoxLeft, userBoxTop, userBoxWidth, userBoxHeight);
            }
            else
            {
                //draw the oponents box background
                e.Graphics.FillRectangle(userBoxBckGroundColor, userBoxLeft, userBoxTop, userBoxWidth, userBoxHeight);

                //draw the scroll bar (what is under the scroll bar button)
                e.Graphics.FillRectangle(userScrollBckgroundColor, userBoxScrollBarLeft, userBoxTop, userBoxScrollBarWidth, userBoxHeight);

                //draw the scroll bar button 
                e.Graphics.FillRectangle(mouseDowned ? userScrollClickedColor : userScrollColor, scrollBtnLeft, (int)scrollBtnTop, scrollBtnWidth, (int)scrollBtnHeight);
            }

            //
            // draw the oponent items
            //
            topDrawingOffset = userItemVertMargin - (int)scrollDelta + (int)userBoxBorderPen.Width / 2;

            // mark all the cells inside the 4 cells long array of visible users as impossible putting some impossible value inside
            for (int i = 0; i < 4; i++)
            {
                clickableAreas[i, 1] = -100;    //only this will invalidate all click tests
            }

            // just a counter, that is global, but will be used from inside the drawUserItem method
            // i put it globally because drawUserItem already has too much arguments/parameters
            clickableAreasIndex = 0;

            // test which of the users are falling into the visible window
            for (int i = 0; i < UsersHandler.UsersCount; i++)
            {
                // and if inside the visible area, paint them and 
                if ((topDrawingOffset < userBoxHeight && topDrawingOffset + userItemHeight + userBoxTop > userBoxTop))
                {
                    drawUserItem(topDrawingOffset + userBoxTop, UsersHandler.getUserAtIndex(i), e);
                    clickableAreasIndex++;
                }
                topDrawingOffset = topDrawingOffset + userItemHeight + userItemVertMargin;
            }
            // draw the masks that ocult the hiden part of the user box
            e.Graphics.FillRectangle(userBoxFakeMaskColor, userBoxLeft, 0, userBoxWidth, userBoxTop);
            e.Graphics.FillRectangle(userBoxFakeMaskColor, userBoxLeft, userBoxTop + userBoxHeight, userBoxWidth, boardWidth - (userBoxTop + userBoxHeight));

            // draw the border of the user box
            e.Graphics.DrawRectangle(userBoxBorderPen, userBoxLeft, userBoxTop, userBoxWidth, userBoxHeight);

            // draw the button
            e.Graphics.DrawImage(sendInvitationBtn.clicked ? sendInvitationBtn.imgShadow : sendInvitationBtn.image,
                                                         sendInvitationBtn.left, sendInvitationBtn.top,
                                                         sendInvitationBtn.width, sendInvitationBtn.height);
            // draw the default options
            for (int i = 0; i < 8; i++)
            {
                e.Graphics.DrawImage(UsersHandler.defaultInvOptions[i] ? invOptionsImgs[i, 1] : invOptionsImgs[i, 0], optionsIconsLeft + optionsIconWidth * i, optionsIconsTop, optionsIconWidth, optionsIconWidth);
            }
        }

        // function in response to the button of the mouse down button event
        void canvas_MouseDown_SendInv(object sender, MouseEventArgs e)
        {
            // test if button for sending invitation is clicked
            if (testButtonCollision(sendInvitationBtn, e.X, e.Y))
            {
                //mark button as clicked to change its image with the shadowed one
                sendInvitationBtn.clicked = true;
                
                for (int i = 0; i < UsersHandler.UsersCount; i++)
                {
                    // for each of the users if they are marked in the form, send invitations to them and unmark 'em
                    if (UsersHandler.getUserAtIndex(i).markedInForm)
                    {
                        if (UsersHandler.getUserAtIndex(i).hasChessRunning)
                        {
                            UsersHandler.getUserAtIndex(i).InvitationMeToUser = true;

                            // copy the invitation options selected from the menu over the invitation button to the internal invitation-sent
                            // register inside the oponent object
                            UsersHandler.getUserAtIndex(i).invitation_I_SentToOponentOptions.copyInvOptions(UsersHandler.defaultInvOptions);

                            SkypeHandler.sendInvitation(UsersHandler.getUserAtIndex(i));
                        }

                        UsersHandler.getUserAtIndex(i).markedInForm = false;
                    }
                }
            }
            // test if mouese cursor is on top of the scroll button
            else if (e.X > scrollBtnLeft && e.X < scrollBtnLeft + scrollBtnWidth && e.Y > scrollBtnTop && e.Y < scrollBtnTop + scrollBtnHeight)
            {
                // tell the mouseMove listener that it should dragg the scroll button
                mouseDowned = true;

                // calculate offset needed for calculations of the scroll
                mouseY = e.Y - (int)scrollBtnTop;
            }
            // test if mouse is over the oponent preview box(window) in the center of the Form
            else if (e.Y > minUserClickableArea && e.Y < maxUserClickableArea && e.X > userItemLeft && e.X < userItemLeft + userItemWidth)
            {
                // this here is looping up to 4 because at each moment only 4 oponents could be visible through the oponent preview window
                for (int i = 0; i < 4; i++)
                {
                    // the top and bottom of each visible user squares are set inside the clicableAreas array
                    if (e.Y > clickableAreas[i, 0] && e.Y < clickableAreas[i, 1])
                    {
                        // if not playing chess already
                        if (!clicableUsers[i].playingChess)
                        {
                            // prepare a flag that will help to avoid selecting the oponent's box at the same time as clicking the cancel/accept button
                            cancelAcceptClicked = false;

                            //
                            //test if mouse is over the cancel/accept circular buttons
                            //
                            double baseTop = (centerOfCancelY + clickableAreas[i, 0]);

                            // if I sent invitation to oponent, it will alway appear on top of the eventual received invitation(a matter of design decision)
                            if (clicableUsers[i].InvitationMeToUser)
                            {
                                // calculate the distance from the center of the cancel circular button
                                if (Math.Sqrt((e.X - centerOfCancelX) * (e.X - centerOfCancelX) +
                                              (e.Y - baseTop) * (e.Y - baseTop)) < cancelAcceptRadius)
                                {
                                    //if inside the circle unmark the oponent's invitation received flag
                                    clicableUsers[i].InvitationMeToUser = false;
                                    SkypeHandler.cancelInvitation(clicableUsers[i]);
                                    cancelAcceptClicked = true;
                                }

                                //shift the Y of the center of circle to be tested one margin to down to become on top of the Accept circular button(in case there is one)
                                baseTop += cancelAcceptWidth + invMargin;
                            }

                            // test if there should be an invitation received shown in the oponent's square
                            if (clicableUsers[i].InvitationUserToMe)
                            {
                                // test for clicking on the accept button
                                if (Math.Sqrt((e.X - centerOfCancelX) * (e.X - centerOfCancelX) +
                                              (e.Y - baseTop) * (e.Y - baseTop)) < cancelAcceptRadius)
                                {
                                    //
                                    // here determine which color each player will play with
                                    //     (by clicking the accept button I accept all the prefferences of the oponent)

                                    // if oponent haven't selected a color to play with
                                    if (clicableUsers[i].invitationOponentSentTo_Me_Options.ColorPreffered == ColorsInvOptions.anyColor)
                                    {
                                        // if me haven't pick up a color yet either..
                                        if (clicableUsers[i].invitation_I_SentToOponentOptions.ColorPreffered == ColorsInvOptions.anyColor)
                                        {
                                            // ..then let the colors be randomly chosen
                                            ChessLogic.myColor = (ColorsOfFigures)randomColorGen.Next(0, 2);
                                        }
                                        else
                                        { // I have selected the color
                                            if (clicableUsers[i].invitation_I_SentToOponentOptions.ColorPreffered == ColorsInvOptions.white)
                                            {
                                                ChessLogic.myColor = ColorsOfFigures.white;
                                            }
                                            else
                                            {
                                                ChessLogic.myColor = ColorsOfFigures.black;
                                            }
                                        }
                                    }
                                    else
                                    { // oponent chosen a color and I have to pick up the contrary
                                        if (clicableUsers[i].invitationOponentSentTo_Me_Options.ColorPreffered == ColorsInvOptions.white)
                                        {
                                            ChessLogic.myColor = ColorsOfFigures.black;
                                        }
                                        else
                                        {
                                            ChessLogic.myColor = ColorsOfFigures.white;
                                        }
                                    }

                                    //======================================
                                    //  pass control to the code handling the chess table
                                    //======================================

                                    //unsuscribe from any events the code for drawitn and using the SelectOponentUI listened to
                                    unsubscribeEvents();

                                    UsersHandler.oponent = clicableUsers[i];

                                    //ask to connect ot the oponent
                                    SkypeHandler.startGameWithUser(clicableUsers[i], 1 - (int)ChessLogic.myColor);

                                    passControlToChessBoard(UsersHandler.getUserAtIndex(i));
                                }
                            }
                            // switch the marked flag
                            if (!cancelAcceptClicked)
                                if (clicableUsers[i].hasChessRunning)
                                {
                                    clicableUsers[i].markedInForm = !clicableUsers[i].markedInForm;
                                } else
                                {
                                    clicableUsers[i].InvitationMeToUser = false;
                                    clicableUsers[i].InvitationUserToMe = false;
                                }
                            break;
                        } else
                        {
                            clicableUsers[i].InvitationMeToUser = false;
                            clicableUsers[i].InvitationUserToMe = false;
                        }
                    }
                }
            }
            // test if mouse is inside the vertical range of the invitation options buttons
            else if (e.Y > optionsIconsTop && e.Y < optionsIconsTop + optionsIconWidth)
            {
                // click detect on kings
                //    ((directly set the booleans. the code inside the Invitation options class will handle the logic))
                if (e.X > optionsIconsLeft && e.X < optionsIconsLeft + optionsIconWidth)
                {
                    UsersHandler.defaultInvOptions.ColorPreffered = ColorsInvOptions.white;
                }
                else if (e.X > optionsIconsLeft + optionsIconWidth && e.X < optionsIconsLeft + optionsIconWidth * 2)
                {
                    UsersHandler.defaultInvOptions.ColorPreffered = ColorsInvOptions.black;
                }
                else if (e.X > optionsIconsLeft + optionsIconWidth * 2 && e.X < optionsIconsLeft + optionsIconWidth * 3)
                {
                    UsersHandler.defaultInvOptions.ColorPreffered = ColorsInvOptions.anyColor;
                }

                //test if over the 4 specific time options (from 15min to 60min)
                else if (e.X > optionsIconsLeft + optionsIconWidth * 3 && e.X < optionsIconsLeft + optionsIconWidth * 7)
                {
                    //find out which option from the 4 time options is clicked exactly
                    //
                    if (e.X < optionsIconsLeft + optionsIconWidth * 4)
                    {
                        UsersHandler.defaultInvOptions.TimePrefferedIn = TimeInvOptions.time15min;
                    }
                    else if (e.X < optionsIconsLeft + optionsIconWidth * 5)
                    {
                        UsersHandler.defaultInvOptions.TimePrefferedIn = TimeInvOptions.time30min;
                    }
                    else if (e.X < optionsIconsLeft + optionsIconWidth * 6)
                    {
                        UsersHandler.defaultInvOptions.TimePrefferedIn = TimeInvOptions.time45min;
                    }
                    else
                    {
                        UsersHandler.defaultInvOptions.TimePrefferedIn = TimeInvOptions.time60min;
                    }
                }
                // test if clicked the noTime option
                else if (e.X > optionsIconsLeft + optionsIconWidth * 7 && e.X < optionsIconsLeft + optionsIconWidth * 8)
                {
                    UsersHandler.defaultInvOptions.TimePrefferedIn = TimeInvOptions.noTime;
                }
            }
            canvas.Invalidate(); // request redraw
        }

        // function fired by the event of lifting the mouse button up
        void canvas_MouseUp_SendInv(object sender, MouseEventArgs e)
        {
            // tell the painting routine to paint the send invitation button without shadow
            sendInvitationBtn.clicked = false;

            //tell the mouseMove function it should not drag the scroll button
            mouseDowned = false;
            canvas.Invalidate(); //request redraw
        }

        // method provoqued by the mouse moving event
        void canvas_MouseMove_SendInv(object sender, MouseEventArgs e)
        {
            //do something only if there is need to drag the scroll button
            if (mouseDowned && scrollNeeded)
            {
                // if scroll is at its maimal high position, it cant move more to up, so just draw it on the top
                if (e.Y - mouseY <= maxScrollTop)
                {
                    scrollBtnTop = maxScrollTop;
                    scrollDelta = 0;
                }
                // if scroll button cant go more to down just draw it at its most lower possible position
                else if (e.Y - mouseY >= minScrollTop)
                {
                    scrollBtnTop = minScrollTop;
                    scrollDelta = wholeUserAreaHeight - (userBoxHeight - userBoxBorderPen.Width);
                }
                // if scroll has room to move, recalculate its new vertical position and the position of the oponents array
                else
                {
                    scrollBtnTop = e.Y - mouseY;

                    //lame solution mathematically but it works
                    scrollDelta = (double)wholeUserAreaHeight / (calculableScrollArea / (scrollBtnTop - maxScrollTop));
                }
                canvas.Invalidate(); //ask for repainting
            }
        }

        // function fired by the wheel of the mouse event
        void canvas_MouseWheel_SendInv(object sender, MouseEventArgs e)
        {
            // do something only if mouse is currently on top of the oponent preview boz(window)
            if (e.X > userBoxLeft && e.X < userBoxLeft + userBoxWidth && e.Y > userBoxTop && e.Y < userBoxTop + userBoxHeight &&
                scrollNeeded)  // if scroll is needed at all
            {
                scrollDelta -= e.Delta / 2;

                // if can't move more to up more, just go to the upper most position
                if (scrollDelta < 0)
                {
                    scrollDelta = 0;
                }
                // if can't move more to the down, just go to the lower most position
                else if (scrollDelta > maxDelta)
                {   //i think delta cant be zero here?
                    scrollDelta = maxDelta;
                }

                //now apply the same logic to calculate the vertical position of the scroll button
                scrollBtnTop = (calculableScrollArea / (wholeUserAreaHeight / scrollDelta)) + maxScrollTop;
                if (scrollBtnTop < maxScrollTop)
                {
                    scrollBtnTop = maxScrollTop;
                }
                else if (scrollBtnTop > minScrollTop)
                {
                    scrollBtnTop = minScrollTop;
                }

                // ask for painting
                canvas.Invalidate();
            }
        }

        //==================================================================

        private void startGame(User user, ColorsOfFigures color)
        {
            ChessLogic.myColor = color;

            unsubscribeEvents();

            passControlToChessBoard(user);
        }

        private void unsubscribeEvents()
        {
            canvas.Paint -= canvas_Paint_SendInv;
            canvas.MouseDown -= canvas_MouseDown_SendInv;
            canvas.MouseUp -= canvas_MouseUp_SendInv;
            canvas.MouseMove -= canvas_MouseMove_SendInv;
            canvas.MouseWheel -= canvas_MouseWheel_SendInv;
        }

        // method created to reuse code
        private void drawUserItem(int top, User user, PaintEventArgs e)
        {
            // load the vertical dimensions of the oponent's square to be painted
            clickableAreas[clickableAreasIndex, 0] = top;
            clickableAreas[clickableAreasIndex, 1] = top + userItemHeight;

            // load the visible oponents inside an array, but this time with their objects
            clicableUsers[clickableAreasIndex] = user;

            // draw the background square for the oponent box. it is of the size of the whole oponent box item
            e.Graphics.FillRectangle(userItemBckgroundColor, userItemLeft, top, userItemWidth, userItemHeight);

            // draw a bold line arround the oponent's box
            e.Graphics.DrawRectangle(user.markedInForm ? userMarkedPen : userNotMarkedPen, //use yellow if selected/marked
                                                        userItemLeft - userMarkedPen.Width / 2, top - userMarkedPen.Width / 2,
                                                        userItemWidth + userMarkedPen.Width, userItemHeight + userMarkedPen.Width);

            // draw the thick angled ornament on the top left of the oponent's image
            userItemOrnamentPoint1.Y = top;
            userItemOrnamentPoint2.Y = top;
            userItemOrnamentPoint3.Y = top + photoMargin;
            userItemOrnamentPoint4.Y = top + photoMargin;
            userItemOrnamentPoint5.Y = top + photoWidth + photoMargin;
            userItemOrnamentPoint6.Y = top + userItemHeight;

            e.Graphics.FillPolygon(userItemOrnamentColor, new Point[]{ userItemOrnamentPoint1,
                                                                       userItemOrnamentPoint2,
                                                                       userItemOrnamentPoint3,
                                                                       userItemOrnamentPoint4,
                                                                       userItemOrnamentPoint5,
                                                                       userItemOrnamentPoint6});

            // draw the photo of the user
            e.Graphics.DrawImage(user.photo, photoLeft, top + photoMargin, photoWidth, photoWidth);
            if (user.hasChessRunning)
                e.Graphics.DrawImage(avatarChessBoardImg, photoLeft, top + photoMargin, photoWidth, photoWidth);

            // the border of the oponent's photo
            e.Graphics.DrawRectangle(photoBorder, photoLeft, top + photoMargin, photoWidth, photoWidth);

            // draw the thin ornament on the bottom right side of the oponent's image
            userItemOrnament2Point1.Y = top + photoMargin + ornament2Width;
            userItemOrnament2Point2.Y = top + photoMargin - ornament2Width;
            userItemOrnament2Point3.Y = top + photoMargin + photoWidth + ornament2Width;
            userItemOrnament2Point4.Y = top + photoMargin + photoWidth + ornament2Width;
            userItemOrnament2Point5.Y = top + photoMargin + photoWidth - ornament2Width;
            userItemOrnament2Point6.Y = top + photoMargin + photoWidth - ornament2Width;

            e.Graphics.FillPolygon(userItemOrnamentColor, new Point[]{ userItemOrnament2Point1,
                                                                       userItemOrnament2Point2,
                                                                       userItemOrnament2Point3,
                                                                       userItemOrnament2Point4,
                                                                       userItemOrnament2Point5,
                                                                       userItemOrnament2Point6});

            // draw a thin ornament on the very(outern) top left side of the oponents image
            userItemOrnament3Point1.Y = top - (int)userMarkedPen.Width;
            userItemOrnament3Point2.Y = top - (int)userMarkedPen.Width;
            userItemOrnament3Point3.Y = top;
            userItemOrnament3Point4.Y = top;
            userItemOrnament3Point5.Y = top + userItemHeight;
            userItemOrnament3Point6.Y = top + userItemHeight + (int)userMarkedPen.Width;

            e.Graphics.FillPolygon(outernOrnamentColor, new Point[]{ userItemOrnament3Point1,
                                                                       userItemOrnament3Point2,
                                                                       userItemOrnament3Point3,
                                                                       userItemOrnament3Point4,
                                                                       userItemOrnament3Point5,
                                                                       userItemOrnament3Point6});

            // draw the right bottom ornament on top of the border of the oponent's square
            userItemOrnament4Point1.Y = top;
            userItemOrnament4Point2.Y = top - (int)userMarkedPen.Width;
            userItemOrnament4Point3.Y = top + userItemHeight + (int)userMarkedPen.Width;
            userItemOrnament4Point4.Y = top + userItemHeight + (int)userMarkedPen.Width;
            userItemOrnament4Point5.Y = top + userItemHeight;
            userItemOrnament4Point6.Y = top + userItemHeight;

            e.Graphics.FillPolygon(userBoxFakeMaskColor, new Point[]{ userItemOrnament4Point1,
                                                                       userItemOrnament4Point2,
                                                                       userItemOrnament4Point3,
                                                                       userItemOrnament4Point4,
                                                                       userItemOrnament4Point5,
                                                                       userItemOrnament4Point6});

            if (user.playingChess)
            {
                e.Graphics.DrawImage(playingChessIcon, iconLeft, top + iconTop, iconWidth, iconWidth);
            }
            else
            {
                // draw the skype status images
                e.Graphics.DrawImage(StatusImages.GetStatusImage(user.skypeStatus), iconLeft, top + iconTop, iconWidth, iconWidth);
            }

            // draw first the sent for oponent invitation if one
            // it is decision of designt, the invitation sent to oponent(if any) goes first
            int topInv = top + invMargin;
            if (user.InvitationMeToUser)
            {
                drawItemOptions(topInv, 0, e, user);

                // one invitation was drawn, so the next(if any) should be displaced
                topInv += arrowsHeight + invMargin;
            }
            // now draw the second inivtation field(if any) in the position that depends from was there a sent invitation or not
            if (user.InvitationUserToMe)
            {
                drawItemOptions(topInv, 1, e, user);
            }
        }

        private void drawItemOptions(int top, int type, PaintEventArgs e, User user)
        {
            if (user.CompatibleInvOptions)
            {
                //draw right/left arrow image regard the invitation booleans inside the User object
                e.Graphics.DrawImage((type == 0 ? leftArrowImgGreen : rightArrowImgGreen), invLeft, top, (type == 0 ? leftArrowWidth : rightArrowWidth), arrowsHeight);

                // draw the upper line connecting the arrow image and the accept/cancel image
                e.Graphics.DrawLine(InvLineColorCompat, (type == 0 ? invRightStartX : invLeftStartX), top + invLineWidth / 2, cancelAcceptLeft + 10, top + invLineWidth / 2);

                // sraw cancel, accept image regard of the User's invitation sent/received options
                e.Graphics.DrawImage((type == 0 ? invCancelImgGreen : invAcceptImgGreen), cancelAcceptLeft, top, cancelAcceptWidth, cancelAcceptWidth);

                // draw the ower line connecting the arrow image and the cancel/accept image
                e.Graphics.DrawLine(InvLineColorCompat, (type == 0 ? invRightStartX : invLeftStartX), top + cancelAcceptWidth - invLineWidth / 2, cancelAcceptLeft + 10, top + cancelAcceptWidth - invLineWidth / 2);
            }
            else
            {
                //the same drawing code(same shapes) but using other colors
                e.Graphics.DrawImage((type == 0 ? leftArrowImg : rightArrowImg), invLeft, top, (type == 0 ? leftArrowWidth : rightArrowWidth), arrowsHeight);

                e.Graphics.DrawLine(InvLineColor, (type == 0 ? invRightStartX : invLeftStartX), top + invLineWidth / 2, cancelAcceptLeft + 10, top + invLineWidth / 2);

                e.Graphics.DrawImage((type == 0 ? invCancelImg : invAcceptImg), cancelAcceptLeft, top, cancelAcceptWidth, cancelAcceptWidth);

                e.Graphics.DrawLine(InvLineColor, (type == 0 ? invRightStartX : invLeftStartX), top + cancelAcceptWidth - invLineWidth / 2, cancelAcceptLeft + 10, top + cancelAcceptWidth - invLineWidth / 2);
            }

            // for the pourpose of comoddity, put the options to draw inside a temporal InvitaionOptions object
            // other wise i would have to pass it down as parameter, and there are too much parameters
            if (type == 0)
            {
                tempInvOpt = user.invitation_I_SentToOponentOptions;
            }
            else
            {
                tempInvOpt = user.invitationOponentSentTo_Me_Options;
            }

            //first draw one of the first 3 options. they are excluding each other so no need to draw them all inside the little
            //space between the arrow and the cancel.accept button 
            for (int i = 0; i < 3; i++)
            {
                if (tempInvOpt[i])
                {
                    // take the image to draw from the 1st dimension of the array of options images. 
                    // the third(forth) one in the second dimension is the one that is meaned to be displayed
                    // between the arrow and cancel.accept buttons
                    e.Graphics.DrawImage(invOptionsImgs[i, 3], invLeft + leftArrowWidth + optInsideUsrItemMargin, top + 8, 24, 24);

                    // if any option from the first three color options is true, just break, because only one of them could be trua at any moment
                    break;
                }
            }

            // just plain copy the rest of options from the inv options and draw them
            for (int i = 3; i < 8; i++)
            {
                if (tempInvOpt[i])
                {   // if active use image darker image to draw it
                    e.Graphics.DrawImage(invOptionsImgs[i, 3], invLeft + leftArrowWidth + optInsideUsrItemMargin + optionsIconWidth * (i - 2), top + 8, 24, 24);
                }
                else
                {   // if inactive, use brighter image to paint it
                    e.Graphics.DrawImage(invOptionsImgs[i, 2], invLeft + leftArrowWidth + optInsideUsrItemMargin + optionsIconWidth * (i - 2), top + 8, 24, 24);
                }
            }
        }
    }
}