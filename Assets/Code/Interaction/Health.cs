using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GA.Platformer
{
    public class Health : MonoBehaviour, IHealth
    {
        [SerializeField]
        private int minHealth;

        [SerializeField]
        private int maxHealth;

        [SerializeField]
        private int startingHealth;

        //public Image Fill;

        private int currentHealth;

        public int CurrentHealth
        {
            get {return currentHealth;}
            private set
            {
                // Makes sure the currentHealth stays within the defined range
                currentHealth = Mathf.Clamp(value, MinHealth, MaxHealth);
                //currentHealth = (int)Mathf.Clamp((float)value, (float)MinHealth, (float)MaxHealth);
                
                // sync
                //CurrentHealth = currentHealth;

                // automatically update the health display
                //Fill.fillAmount = CurrentHealth / MaxHealth;
            }
        }

        public int MaxHealth
        {
            get { return maxHealth; }
        }

        public int MinHealth
        {
            get { return minHealth; }
        }

        private void Start()
        {
            Reset();
        }

        /// <summary>
        /// Decreases the health.
        /// </summary>
        /// <param name="amount">The amount to decrease.</param>
        /// <returns>True, if the object is still alive. False otherwise.</returns>
        public bool DecreaseHealth(int amount)
        {
            if (amount < 0) return CurrentHealth > MinHealth;

            CurrentHealth -= amount;

            return CurrentHealth > MinHealth;
        }

        /// <summary>
        /// Increases the health.
        /// </summary>
        /// <param name="amount">Amount to increase</param>
        public void IncreaseHealth(int amount)
        {
            if (amount < 0) return;

            CurrentHealth += amount;
        }

        public void Reset()
        {
            CurrentHealth = startingHealth;
        }
    }
}
