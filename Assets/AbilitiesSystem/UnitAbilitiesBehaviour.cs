using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;
using Utilities.Inspector;
using Unity.Collections;

public class UnitAbilitiesBehaviour : MonoBehaviour
    {
        public UnitAbilityBehaviour[] abilities;
        
        [SerializeField]
        [ReadOnly] private List<int> abilityQueue;

        [SerializeField]
        [ReadOnly]private bool abilityCastAllow;

        [SerializeField]
        [ReadOnly]public bool abilityCurrentlyActive;
        
        
        void Awake()
        {
            abilityQueue = new List<int>();
            SetupAbilities();
        }

        void SetupAbilities()
        {
            for(int i = 0; i < abilities.Length; i++)
            {
                abilities[i].SetupID(i);
                abilities[i].SetupAbilityCooldownTimer();
            }
        }

        public void StartAbilityCooldowns()
        {
            abilityCastAllow = true;
            abilityCurrentlyActive = false;

            for(int i = 0; i < abilities.Length; i++)
            {
                abilities[i].StartAbilityCooldown();
            }
        }

        public void AddAbilityToQueue(int abilityID)
        {
        GameLog.LogMessage("added ability to queu" + abilityID);
            abilityQueue.Add(abilityID);
            CheckForNextAbility();
        }

        void CheckForNextAbility()
        {
            if(abilityQueue.Count > 0)
            {
                if(!abilityCurrentlyActive)
                {
                    if(abilityCastAllow)
                    {
                        BeginAbility();
                        abilityCurrentlyActive = true;
                    }   
                }
            }

        }

        void BeginAbility()
        {
            int currentAbility = abilityQueue[0];
            abilities[currentAbility].BeginAbilitySequence();
            abilityQueue.RemoveAt(0);
            
        }

        public void AbilitySequenceFinished()
        {
        GameLog.LogMessage("Ability sequence Finished");
            abilityCurrentlyActive = false;
            CheckForNextAbility();
        }

        public void StopAllAbilities()
        {
            abilityQueue.Clear();
            
            abilityCastAllow = false;

            for(int i = 0; i < abilities.Length; i++)
            {
                abilities[i].StopAbility();
            }

        }

    }