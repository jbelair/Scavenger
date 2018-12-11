using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRef : MonoBehaviour
{
    public enum Format { Internal, External, Player };
    public Format format;
    public Entity entity;
    public Entity Entity
    {
        get
        {
            switch(format)
            {
                case Format.Internal:
                    return entity;
                case Format.External:
                    return entity;
                case Format.Player:
                    if (entity == null)
                        entity = Players.players[0];
                    return entity;
            }
            return entity;
        }
        set
        {

        }
    }
}
