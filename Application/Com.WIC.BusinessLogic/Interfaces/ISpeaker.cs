using Com.WIC.BusinessLogic.Classes;

namespace Com.WIC.BusinessLogic.Interfaces
{
    public interface ISpeaker
    {
        Result<string> Speak(string text);
    }
}
