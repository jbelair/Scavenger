using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public static class StringHelper
{
    public static string PredictType(string value)
    {
        return "unknown";
    }

    public static string Units(string units)
    {
        switch(units)
        {
            case "persecond":
                return "/s";
            case "seconds":
                return "s";
            case "metres":
                return "m";
            case "metrespersecond":
                return "m/s";
            case "metrespersecond2":
                return "m/s<sup>2</sup>";
            case "degreespersecond":
                return "°/s";
            case "kilometres":
                return "km";
            case "multiplier":
                return "x";
            default:
                return units;
        }
    }

    public static string ClosestMatch(string[] comparisons, string str)
    {
        if (comparisons.Length < 1)
            return str;

        float[] scores = new float[comparisons.Length];
        float lowestScore = float.MaxValue;
        int index = 0;
        for (int i = comparisons.Length - 1; i >= 0; i--)
        {
            if (comparisons[i].Contains(str))
            {
                scores[i] += Mathf.Max(comparisons[i].Length, str.Length) / Mathf.Min(comparisons[i].Length, str.Length);
                if (scores[i] <= lowestScore)
                {
                    lowestScore = scores[i];
                    index = i;
                }
                continue;
            }

            for(int j = 0; j < str.Length; j++)
            {
                if (j < comparisons[i].Length)
                {
                    scores[i] += comparisons[i][j] - str[j];
                }
                if (scores[i] <= lowestScore)
                {
                    lowestScore = scores[i];
                    index = i;
                }
            }
        }

        return comparisons[index];
    }

    public static string[] ClosestMatches(string[] comparisons, string str)
    {
        if (comparisons.Length < 1)
            return new string[] { str };

        List<string> sorted = new List<string>();
        List<float> scores = new List<float>();
        float highestScore = 0;
        for (int i = comparisons.Length - 1; i >= 0; i--)
        {
            float score = comparisons[i].Length;

            for (int j = 0; j < str.Length; j++)
            {
                if (j < comparisons[i].Length)
                {
                    // Character Delta Heuristic
                    // This calculates a score for each character j in str as its difference from each character j in comparisons[i]
                    // Since this checks j in str against j in comparisons[i] this checks for direct word matches, and displays recommendations in alphabetical order from most matching to least
                    score += Mathf.Abs(comparisons[i][j] - str[j]);
                    
                    // Character Match Heuristic
                    // This checks every character of comparisons[i] against the character j in str
                    // Since this checks all characters in comparisons[i] the character j in str this checks for common letters, and dislays recommendations ordered by number of repetitions of similar characters
                    //for (int k = j; k < comparisons[i].Length; k++)
                    //{
                    //    score += Mathf.Max(0, Mathf.Abs(comparisons[i][k] - str[j]) - k);
                    //}
                }
                else
                    break;
            }

            highestScore = Mathf.Max(score, highestScore);

            if (score < highestScore)
            {
                for (int j = 0; j < scores.Count; j++)
                {
                    if (score < scores[j])
                    {
                        scores.Insert(j, score);
                        sorted.Insert(j, comparisons[i]);
                        break;
                    }
                }
            }
            else
            {
                scores.Add(score);
                sorted.Add(comparisons[i]);
            }
        }

        return sorted.ToArray();
    }

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
        while (value.Length > 0 && value[0] == ' ')
        {
            value = value.Remove(0, 1);
        }
        while (value.Length > 0 && value[value.Length - 1] == ' ')
        {
            value = value.Remove(value.Length - 1, 1);
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
        if (i == 0)
            log = -1;
        switch (log)
        {
            case 0:
                return "rarity_abundant";
            case 1:
                return "rarity_common";
            case 2:
                return "rarity_uncommon";
            case 3:
                return "rarity_rare";
            case 4:
                return "rarity_legendary";
            case 5:
                return "rarity_epic";
            case 6:
                return "rarity_immortal";
            default:
                return "rarity_unique";
        }
    }

    public static string RiskIntToString(int risk)
    {
        switch (risk)
        {
            case 0:
                return "risk_none";
            case 1:
                return "risk_low";
            case 2:
                return "risk_medium";
            case 3:
                return "risk_high";
            case 4:
                return "risk_extreme";
            case 5:
                return "risk_terminal";
            default:
                return "risk_unknown";
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
                ret += split[i] + " " + RiskIntToString(Mathf.FloorToInt((float.Parse(split[i + 1]) / Environment.JumpRadius) * 5)).Replace("risk_", "");
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
