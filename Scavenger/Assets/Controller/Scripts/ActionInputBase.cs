using UnityEngine;

public abstract class ActionInputBase
{
    public string name;

    public RuntimePlatform[] platforms;
    public InputMode mode;

    public bool Enabled(RuntimePlatform platform, InputMode mode)
    {
        bool input = mode == this.mode;
        if (!input)
            return false;

        foreach (RuntimePlatform plat in platforms)
        {
            if (platform == plat && input)
                return true;
        }
        return false;
    }
}
