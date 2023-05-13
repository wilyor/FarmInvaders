using System;

[Serializable]
public class PlayerInfo
{
    public string playerName;
    public int maxScore;

    public PlayerInfo(string name, int score)
    {
        playerName = name;
        maxScore = score;
    }
}
