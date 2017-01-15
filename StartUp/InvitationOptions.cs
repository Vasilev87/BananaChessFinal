namespace Banana_Chess
{
    public class InvitationOptions : IInvitationOptions
    {
        private InvOptionsStruct options;

        public InvitationOptions()
        {
            options.white = false;
            options.black = false;
            options.anyColor = true;

            options.time15min = false;
            options.time30min = false;
            options.time45min = false;
            options.time60min = false;
            options.noTime = true;
        }

        public ColorsInvOptions ColorPreffered
        {  // make sure throught tests the dependency between color options is mantained
            get
            {
                if (options.anyColor) return ColorsInvOptions.anyColor;
                else if (options.white) return ColorsInvOptions.white;
                else return ColorsInvOptions.black;
            }
            //the logic for the switching of the colors was moved from Form to here
            set
            {
                if (value == ColorsInvOptions.anyColor)
                {
                    if (!options.anyColor)
                    {
                        options.white = false;
                        options.black = false;
                        options.anyColor = true;
                    }
                    //else
                    //{
                    //unsetting anyColor is not allowed
                    //}
                }
                else
                {
                    if (value == ColorsInvOptions.white)
                    {
                        options.white = !options.white;
                        if (options.white) options.black = false;
                    }
                    else if (value == ColorsInvOptions.black)
                    {
                        options.black = !options.black;
                        if (options.black) options.white = false;
                    }
                    else
                    {
                        // execution flow should never reach this point, because InvitationOptions has no more options
                        throw new System.ArgumentException("The value of ColorInvOptions passed to InvitationOptions class is not expected.\n" +
                                                           "Maybe you changed the possible values to the enum ColorInvOptions.\n" +
                                                           "If so, please add the code to attend such case.");
                    }
                    if (!options.black && !options.white)
                        options.anyColor = true;
                    else
                        options.anyColor = false;
                }
            }
        }

        public string TimePrefferedOut
        {
            get
            {
                return
                  (options.time15min ? "1" : "0") +
                  (options.time30min ? "1" : "0") +
                  (options.time45min ? "1" : "0") +
                  (options.time60min ? "1" : "0") +
                  (options.noTime ? "1" : "0");
            }
        }

        public TimeInvOptions TimePrefferedIn
        {   // manatain the correct dependency between time options with proper tests

            // NOTE:   get   should return collection

            set
            {
                if (value == TimeInvOptions.noTime)
                {
                    options.noTime = true;
                    options.time15min = false;
                    options.time30min = false;
                    options.time45min = false;
                    options.time60min = false;
                }
                else
                {
                    switch (value)
                    {
                        case TimeInvOptions.time15min:
                            options.time15min = !options.time15min;
                            break;
                        case TimeInvOptions.time30min:
                            options.time30min = !options.time30min;
                            break;
                        case TimeInvOptions.time45min:
                            options.time45min = !options.time45min;
                            break;
                        case TimeInvOptions.time60min:
                            options.time60min = !options.time60min;
                            break;
                        default:
                            // execution flow should never reach this point, because TimeInvOptions has no more options
                            throw new System.ArgumentException("The value of TimeInvOptions passed to InvitationOptions class is not expected.\n" +
                                                           "Maybe you changed the possible values to the enum TimeInvOptions.\n" +
                                                           "If so, please add the code to attend such case.");
                    }
                    if (!options.time15min && !options.time30min && !options.time45min && !options.time60min)
                        options.noTime = true;
                    else
                        options.noTime = false;
                }
            }
        }

        public void copyInvOptions(InvitationOptions toCopy)
        {
            options.white = toCopy.options.white;
            options.black = toCopy.options.black;
            options.anyColor = toCopy.options.anyColor;
            options.time15min = toCopy.options.time15min;
            options.time30min = toCopy.options.time30min;
            options.time45min = toCopy.options.time45min;
            options.time60min = toCopy.options.time60min;
            options.noTime = toCopy.options.noTime;
        }

        public bool this[int i] => getAtIndex(i);

        private bool getAtIndex(int i)
        {
            switch (i)
            {
                case 0:
                    return options.white;
                case 1:
                    return options.black;
                case 2:
                    return options.anyColor;
                case 3:
                    return options.time15min;
                case 4:
                    return options.time30min;
                case 5:
                    return options.time45min;
                case 6:
                    return options.time60min;
                case 7:
                    return options.noTime;
                default:
                    return false; // sillently ignore bad index passed as argument
            }
        }

        public static bool operator ==(InvitationOptions comp1, InvitationOptions comp2)
        {
            return compareTwo(comp1.options, comp2.options);
        }

        public static bool operator !=(InvitationOptions comp1, InvitationOptions comp2)
        {
            return !compareTwo(comp1.options, comp2.options);
        }

        private static bool compareTwo(InvOptionsStruct comp1struct, InvOptionsStruct comp2struct)
        {
            bool colorCompatibility = comp1struct.anyColor || comp2struct.anyColor ||
                                      comp1struct.white == comp2struct.white; //dont test for black, if white equals, black equals too

            bool timeCompatibility = comp1struct.time15min && comp2struct.time15min ||
                                     comp1struct.time30min && comp2struct.time30min ||
                                     comp1struct.time45min && comp2struct.time45min ||
                                     comp1struct.time60min && comp2struct.time60min ||
                                     comp1struct.noTime && comp2struct.noTime;

            return colorCompatibility && timeCompatibility;
    }

        public void fromStr(string optToCopy)
        { //note above is swithing and here is copying values
            ColorPreffered = (ColorsInvOptions)int.Parse(optToCopy.Substring(0, 1));
            options.time15min = optToCopy[1] == '1' ? true : false;
            options.time30min = optToCopy[2] == '1' ? true : false;
            options.time45min = optToCopy[3] == '1' ? true : false;
            options.time60min = optToCopy[4] == '1' ? true : false;
            options.noTime = optToCopy[5] == '1' ? true : false;
        }

        private struct InvOptionsStruct
        {
            // colors of figures chosen depends one from the other. see setters getters to see the dependency
            public bool white;
            public bool black;
            public bool anyColor;  //default choice

            // clock options depends one from the other. see setters/getters to see dependency
            public bool time15min;
            public bool time30min;
            public bool time45min;
            public bool time60min;
            public bool noTime;         //default choise
        }
    }
}
