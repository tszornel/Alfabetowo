using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;


[CreateAssetMenu(fileName = "_Health", menuName = "Webaby/Unit/Health", order = 1)]
public class Health : ScriptableObject
{
    public Life[] lives = { new Life(), new Life(), new Life() };
    public bool isAlive = true;
    public Action<Health> UpdateHealthAction;
    private int livesAmount = 3;

    public int LivesAmount { 
        get => livesAmount; set => livesAmount = value; }

 
    public int GetFullLives() 
    {
        int i = 0;
        foreach (var life in lives)
        {
           if( life.lifePercentage == life.lifeTotalAmount)
                i++;
        }
        return i;
    
    }

    public int GetCurrentHealth() {

        for (int i = 0; i < lives.Length; i++)
        {
            if (lives[lives.Length - 1 - i].lifePercentage > 0)
            {
                return lives[lives.Length - 1 - i].lifePercentage;

                
            }
        }
        return 0;

    }
    public int GetMaxHealth()
    {
        return lives[0].lifeTotalAmount;


    }


        public int GetLivesAmount()
    {
        int i = 0;
        foreach (var life in lives)
        {
            if (life.lifePercentage > 0 )
                i++;
        }
        return i;

    }

    public int GetCurrentLiveIndex()
    {
        int i = 0;
        foreach (var life in lives)
        {
            if (life.lifePercentage > 0)
                i++;
        }

        if (i == 0)
            return i;
        else
            return i - 1;

    }

    private bool CheckIsAlive()
    {
        foreach (var life in lives)
        {
            if (life.lifePercentage > 0)
               return true;
        }

        return false;

    }

 
    public Health LostLife()
    {
        GameLog.LogMessage("Lost Life entered");
       // LivesAmount = LivesAmount - 1;
        for (int i = 0; i < lives.Length; i++)
        {
            if (lives[lives.Length - 1 - i].lifePercentage > 0) 
            { 
                lives[lives.Length - 1 - i].lifePercentage = 0;
                
                break;
            }
        }
        isAlive = CheckIsAlive();
        UpdateHealthAction(this);
        return this;
        
    }

    public void AddLife() {

        for (int i = 0; i < lives.Length; i++)
        {
            if (lives[i].lifePercentage <= 0 )
            {
                lives[i].lifePercentage = lives[i].lifeTotalAmount;
                break;
            }
        }
        UpdateHealthAction(this);

    }


   

    public void RefillLife()
    {
        for (int i = 0; i < lives.Length; i++)
        {
            if (lives[i].lifePercentage >= 0 && lives[i].lifePercentage < lives[i].lifeTotalAmount)
            {
                lives[i].lifePercentage = lives[i].lifeTotalAmount;
                break;
            }
        }
        UpdateHealthAction(this);


    }

    public void resetHealth() 
    {
        foreach (var life in lives)
        {
            life.lifePercentage = life.lifeTotalAmount;
        }
        isAlive = true;
        LivesAmount = lives.Length;
    }

    public Health DecreseLife(int amount)
    {
        GameLog.LogMessage("Decrease Life entered amount=" + amount);
        int decreasedLifePercent = 0;
        for (int i = 0; i < lives.Length; i++)
        {
            if (lives[lives.Length - 1 - i].lifePercentage > 0) 
            {
                decreasedLifePercent =  lives[lives.Length - 1 - i].DecreseLifePercent(amount);

                if (lives[lives.Length - 1 - i].lifePercentage <= 0)
                    LivesAmount = LivesAmount - 1;

                break;
            }
        }
        isAlive = CheckIsAlive();
        UpdateHealthAction(this);
        return this;
    }

    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        sb.AppendLine("isAlive:" + isAlive);
        sb.AppendLine("fullLivesAmount=" + LivesAmount);
        int INDEX = 0;
        foreach (Life _life in lives)
        {
            INDEX++;
            sb.AppendLine("live "+ INDEX+" amount:" + _life.lifePercentage); 

        }
        return sb.ToString();
    }
}


[System.Serializable]
public class Life 
{
    [SerializeField]
    public int lifePercentage;
    public int lifeTotalAmount;

   // public int LifePercentage { get => lifePercentage; set => lifePercentage = value; }

    public Life() {
        lifeTotalAmount = 100;
        lifePercentage = lifeTotalAmount;
    }

    
    public int DecreseLifePercent(int damageAmount) {

        if (damageAmount != 0)
            lifePercentage = lifePercentage - damageAmount;
        else
            lifePercentage = 0;
        return lifePercentage;  
        
    }

}
