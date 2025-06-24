using UnityEngine;

public class RecordManager : MonoBehaviour
{
    private int winCount = 0;
    private int loseCount = 0;

    public void Initialize(bool reset = false)
    {
        if (reset)
        {
            ResetRecord();
        }
        else
        {
            winCount = ES3.Load("win", 0);
            loseCount = ES3.Load("lose", 0);
        }
    }

    public void AddWin()
    {
        winCount++;
        ES3.Save("win", winCount);
    }

    public void AddLose()
    {
        loseCount++;
        ES3.Save("lose", loseCount);
    }

    public int GetWinCount() => winCount;
    public int GetLoseCount() => loseCount;

    public void ResetRecord()
    {
        winCount = 0;
        loseCount = 0;
        ES3.Save("win", winCount);
        ES3.Save("lose", loseCount);
    }
}