using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

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

    private static string indexIntToChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public static string IndexIntToChar(int i)
    {
        int wrapAround = Mathf.FloorToInt(i / 26f);
        int index = i - wrapAround;
        string ret = indexIntToChar[index].ToString();

        if (wrapAround > 0)
            ret = indexIntToChar[wrapAround].ToString() + indexIntToChar[index].ToString();

        return ret;
    }
}
