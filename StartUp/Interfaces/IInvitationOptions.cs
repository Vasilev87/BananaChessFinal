
namespace Banana_Chess
{
    interface IInvitationOptions
    {
        ColorsInvOptions ColorPreffered { get; set; }

        string TimePrefferedOut { get; }

        TimeInvOptions TimePrefferedIn { set; }

        void copyInvOptions(InvitationOptions toCopy);

        void fromStr(string optToCopy);
    }
}
