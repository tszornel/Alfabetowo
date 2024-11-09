using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;


    public enum TargetType
    {
        BasicEnemy,
        BossEnemy
    }

    public enum ValueModifier
    {
        Positive,
        Negative
    }

    [CreateAssetMenu(fileName = "UnitData_NAME_Ability_", menuName = "Webaby/Unit/Ability Data", order = 1)]
    public class UnitAbilityData : ScriptableObject
    {
        [Header("Display Info")]
        public string abilityName;

        [Header("Value Settings")]
        public ValueModifier valueModifer;
        public Vector2Int valueRange;

        [Header("Cooldown")]
        public float cooldownTime;

        [Header("Target Type")]
        public TargetType targetType;

        public int GetRandomValueInRange()
        {       
            return Random.Range(valueRange.x, valueRange.y) * ValueModifierResult();
        }

        private int ValueModifierResult()
        {
            int modifier = 0;

            switch(valueModifer)
            {
                case ValueModifier.Positive:
                    modifier = 1;
                    break;

                case ValueModifier.Negative:
                    modifier = -1;
                    break;
            }

            return modifier;
        }

    }

