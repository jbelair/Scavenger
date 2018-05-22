using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public static class StringHelper
{
    public static string CommandParse(string command)
    {
        string value = "";
        int readIndex = 0;
        if (command[readIndex] == '$')
        {
            readIndex++;
            while (readIndex < command.Length)
            {
                string interpret = "";
                if (command[readIndex] == '[')
                {
                    readIndex++;
                    while (command[readIndex] != ']')
                    {
                        interpret += command[readIndex];
                        readIndex++;
                    }

                    interpret = RemoveSpaces(interpret);

                    value += (char)UnityEngine.Random.Range(interpret[0], interpret[2]);
                }
                //else if (command[readIndex] == )
                else
                {
                    value += command[readIndex];
                }
                
                readIndex++;
            }
        }

        return value;
    }

    public static string RemoveTrailingSpaces(string str)
    {
        string value = str;
        while(value[0] == ' ')
        {
            value.Remove(0);
        }
        while(value[value.Length-1] == ' ')
        {
            value.Remove(value.Length - 1);
        }
        return value;
    }

    public static string RemoveSpaces(string str)
    {
        string value = str.Replace(" ", "");
        return value;
    }
}
