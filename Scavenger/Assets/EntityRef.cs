using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityRef : MonoBehaviour
{
    public enum Format { Internal, External, Player, Players, Entities };
    public Format format;
    public List<Entity> entities = new List<Entity>();
    public Entity Entity
    {
        get
        {
            switch(format)
            {
                case Format.Internal:
                    if (entities.Count > 0)
                        return entities[0];
                    else
                        return null;
                case Format.External:
                    if (entities.Count > 0)
                        return entities[0];
                    else
                        return null;
                case Format.Player:
                    if (entities.Count < 1)
                        entities.Add(Players.players[0]);
                    return entities[0];
                case Format.Players:
                    return Players.players[Random.Range(0, Players.players.Count)];
                case Format.Entities:
                    entities.Clear();
                    entities.AddRange(FindObjectsOfType<Entity>());
                    return entities[Random.Range(0, entities.Count)];
            }
            return entities[Random.Range(0, entities.Count)];
        }
        set
        {

        }
    }

    public List<Entity> Entities
    {
        get
        {
            switch(format)
            {
                case Format.Player:
                    if (entities.Count < 1)
                        entities.Add(Players.players[0]);
                    return entities;
                case Format.Players:
                    if (entities.Count < 1)
                    {
                        foreach (Player player in Players.players)
                        {
                            entities.Add(player);
                        }
                    }
                    return entities;
                case Format.Entities:
                    entities.Clear();
                    entities.AddRange(FindObjectsOfType<Entity>());
                    return entities;
            }
            return entities;
        }
    }
}
