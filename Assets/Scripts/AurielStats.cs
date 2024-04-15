using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DigitalMedia.Core;
using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

namespace DigitalMedia
{
    public class AurielStats : StatsComponent
    {
        private int currentLibraryAsset = 0;
        private SpriteLibraryAsset[] _spriteLibraryAssets;
        private SpriteLibrary _library;
        
        private void Start()
        {
            if (!overrideHealthMax)
            {
                health = data.BasicData.maxHealth;
            }
            
            vitality = data.BasicData.maxVitality;
            stamina = data.BasicData.stamina;
         
            healthbar = GetComponentInChildren<Slider>();

            rb = this.gameObject.GetComponent<Rigidbody2D>();

            _library = GetComponent<SpriteLibrary>();
        }
        
        public override void HandleLives()
        {
            //1. Correct the values to match the new lives remaining and change health to max.
            currentLives -= 1;
            if (currentLives <= 0)
            {
                InitiateStateChange(State.Dying);
                //if (this.gameObject.name == "Player") return;
                //spawn other things like guts and blood!
                Destroy(this.gameObject);
            }

            currentLibraryAsset++;
            _library.spriteLibraryAsset = _spriteLibraryAssets[currentLibraryAsset];
            
            health = data.BasicData.maxHealth;
            vitality = data.BasicData.maxVitality;
            vitalityBar.value = 1;
            healthbar.value = 1;
            vitalityBar.GetComponent<Slider>().fillRect.GetComponent<Image>().color = new Color(0, 1, 0.5437737f, 1);

            //2. Update UI to match lives remaining.
            livesUI.ElementAt(currentLives).SetActive(false);
            
            //3. Re-enabling enemy states if they were attacking -- or just turn their BTs back on. 
           
        }
    }
}
