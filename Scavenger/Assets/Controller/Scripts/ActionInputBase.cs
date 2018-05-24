using UnityEngine;

public enum Platform { OSX, Windows, Linux, PS4, XboxOne };

public abstract class ActionInputBase
{
    public string name;

    public Platform[] platforms;
    public InputMode mode;

    public bool Enabled(RuntimePlatform platform, InputMode mode)
    {
        bool input = mode == this.mode;
        if (!input)
            return false;

        foreach (Platform plat in platforms)
        {
            if (((plat == Platform.Linux && platform == RuntimePlatform.LinuxEditor || plat == Platform.Linux && platform == RuntimePlatform.LinuxPlayer) ||
                (plat == Platform.OSX && platform == RuntimePlatform.OSXEditor || plat == Platform.OSX && platform == RuntimePlatform.OSXPlayer) ||
                (plat == Platform.Windows && platform == RuntimePlatform.WindowsEditor || plat == Platform.Windows && platform == RuntimePlatform.WindowsPlayer) ||
                plat == Platform.PS4 && platform == RuntimePlatform.PS4 ||
                plat == Platform.XboxOne && platform == RuntimePlatform.XboxOne)
                && input)
                return true;
        }

        return false;
    }
}
