using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

[Serializable]
public class AbecadlowoScore : IComparable<AbecadlowoScore>
{
    [SerializeField]
    private DateTime m_Date;
    //  private string m_FormattedValue;
    [SerializeField]
    private string m_UserID;
    [SerializeField]
    private int m_Rank;
    [SerializeField]
    private int m_Coins_Amount;
    [SerializeField]
    private int m_Words_Amount;
    [SerializeField]
    private string m_Time_Amount;
    [SerializeField]
    private long m_value;
    public DateTime date => m_Date;
    //  public string formattedValue => m_FormattedValue;
   
    public string userID => m_UserID;
  
    public int rank => m_Rank;
    [SerializeField]
    public int Coins_Amount { get => m_Coins_Amount; set => m_Coins_Amount = value; }
    [SerializeField]
    public int Words_Amount { get => m_Words_Amount; set => m_Words_Amount = value; }
    [SerializeField]
    public string Time_Amount { get => m_Time_Amount; set => m_Time_Amount = value; }
    [SerializeField]
    public long value { get => m_value; set => m_value = value; }

    public AbecadlowoScore()
        : this(-1L)
    {
    }
    public AbecadlowoScore(long value)
        : this(value, "0", DateTime.Now, -1, -1, -1, "")
    {
    }
    public AbecadlowoScore(long value, string userID, DateTime date, int rank,int _Coins_Amount,int _Words_Amount,string _Time_Amount)
    {
        this.value = value;
        m_UserID = userID;
        m_Date = date;
        // m_FormattedValue = formattedValue;
        m_Coins_Amount = _Coins_Amount;
        m_Words_Amount= _Words_Amount;
        m_Time_Amount= _Time_Amount;
        m_Rank = rank;
    }

    public override string ToString()
    {
        return "Rank: '" + m_Rank + "' Value: '" + value + "' PlayerID: '" + m_UserID + "' Date: '" + m_Date.ToString();
    }

 

    public int CompareTo(AbecadlowoScore other)
    {
        if (this.value == other.value)
            return 0;
        if (this.value < other.value)
            return -1;
        return 1;
    }
}
