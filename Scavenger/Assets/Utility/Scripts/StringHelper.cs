using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class StringHelper
{
    public static string TagParse(string tags)
    {
        string[] split = tags.Split(' ');

        string value = "";

        for (int i = 0; i < split.Length; i++)
        {
            if (split[i][0] == '$')
            {
                if (i != split.Length - 1)
                    value += CommandParse(split[i]) + " ";
                else
                    value += CommandParse(split[i]);
            }
            else if (i != split.Length - 1)
                value += split[i] + " ";
            else
                value += split[i];
        }

        return value;
    }

    public static string[] TagParseAll(string tags)
    {
        List<string> split = new List<string>();
        split.AddRange(tags.Split(' '));

        for (int i = 0; i < split.Count; i++)
        {
            if (split[i][0] == '$')
            {
                string skim = "";
                int readIndex = 0;
                while (split[i][readIndex] != '[' && readIndex < split[i].Length)
                {
                    readIndex++;
                }
                readIndex++;
                while (split[i][readIndex] != ']' && readIndex < split[i].Length)
                {
                    skim += split[i][readIndex];
                    readIndex++;
                }
                split.AddRange(skim.Split(','));
                split.RemoveAt(i);
            }
        }

        return split.ToArray();
    }

    public static string ClearExtraSpaces(string str)
    {
        string[] split = str.Split(' ');
        string ret = "";
        foreach (string s in split)
        {
            if (s != "")
            {
                ret += s + " ";
            }
        }

        return ret;
    }

    public static string CommandParse(string command)
    {
        string value = "";
        int readIndex = 0;
        bool commandInterpret = false;
        while (command.Length > readIndex)
        {
            if (command[readIndex] == '$')
            {
                commandInterpret = true;
            }
            else
            {
                value += command[readIndex];
                readIndex++;
            }

            while (commandInterpret)
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

                    if (interpret.Length > 3 || interpret[1] != '-')
                    {
                        value += "!$[*-*]";
                    }

                    value += (char)UnityEngine.Random.Range(interpret[0], interpret[2]);
                    commandInterpret = false;
                }
                else if (command[readIndex].IsNumber())
                {
                    //Debug.Log("Command Parse: " + command);
                    // $#A:#B[str,str,str]
                    // $#A:
                    while (command[readIndex] != ':')
                    {
                        interpret += command[readIndex];
                        readIndex++;
                    }
                    // #A
                    int a = int.Parse(interpret);
                    interpret = "";
                    readIndex++;
                    //:#B[
                    while (command[readIndex] != '[')
                    {
                        interpret += command[readIndex];
                        readIndex++;
                    }
                    // #B
                    int b = int.Parse(interpret);
                    interpret = "";
                    readIndex++;
                    // [str,str,str]
                    while (command[readIndex] != ']')
                    {
                        interpret += command[readIndex];
                        readIndex++;
                    }
                    string[] split = interpret.Split(',');
                    for (int i = 0; i < split.Length; i++)
                    {
                        if (1 == UnityEngine.Random.Range(0, b / a))
                        {
                            value += split[i] + " ";
                        }
                    }
                    commandInterpret = false;
                }

                readIndex++;
            }
        }

        return value;
    }

    public static string RemoveTrailingSpaces(string str)
    {
        string value = str;
        while (value[0] == ' ')
        {
            value.Remove(0);
        }
        while (value[value.Length - 1] == ' ')
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

    private static readonly string indexIntToChar = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
    public static string IndexIntToChar(int i)
    {
        string ret = "";

        int AA = Mathf.FloorToInt(i / 26f);
        int AAA = Mathf.FloorToInt(AA / 26f);
        int A = i - (AA * 26);

        if (AA > 0)
        {
            if (AA < 26)
                ret = indexIntToChar[AA - 1].ToString() + indexIntToChar[A].ToString();
            else
                ret = indexIntToChar[AAA - 1].ToString() + indexIntToChar[AA - 1].ToString() + indexIntToChar[A].ToString();
        }
        else
            ret = indexIntToChar[A].ToString();

        return ret;
    }

    public static string RarityIntToString(int i)
    {
        int log = Mathf.FloorToInt(Mathf.Clamp(Mathf.Log10(i), 0, 6f));
        switch (log)
        {
            case 0:
                return "Abundant";
            case 1:
                return "Common";
            case 2:
                return "Uncommon";
            case 3:
                return "Rare";
            case 4:
                return "Legendary";
            case 5:
                return "Epic";
            case 6:
                return "Unique";
        }

        return "Unknown";
    }

    public static string RiskIntToString(int risk)
    {
        switch (risk)
        {
            case 0:
                return "None";
            case 1:
                return "Low";
            case 2:
                return "Medium";
            case 3:
                return "High";
            case 4:
                return "Extreme";
            case 5:
                return "Terminal";
            default:
                return "Unknown";
        }
    }

    public static string CoordinateName(Vector3 position)
    {
        string name;

        if (position.x >= 0)
            name = "+" + position.x;
        else
            name = position.x.ToString();

        if (position.y >= 0)
            name = name + "+" + position.y;
        else
            name = name + position.y;

        return name;
    }

    public static string SchemeParse(string text)
    {
        string ret = "";

        string[] split = text.Split(' ');

        for(int i = 0; i < split.Length; i++)
        {
            if (split[i] == "Distance")
            {
                ret += split[i] + " " + RiskIntToString(Mathf.FloorToInt((float.Parse(split[i + 1]) / Environment.jumpRadius) * 5));
                i++;
            }
            else if (i != split.Length - 1)
            {
                ret += split[i] + " ";
            }
            else
                ret += split[i];
        }

        return ret;
    }
}
