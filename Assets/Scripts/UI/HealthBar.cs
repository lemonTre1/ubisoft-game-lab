﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthBar : MonoBehaviour {

    public Slider healthBar;
    private Player player;
    public Image fill;
    public Image background;
    private IEnumerator flashingBar;
    private bool isBarFlashing = false;
    [SerializeField]
    private GenericSoundManager soundManager;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        player.LightEnergy.LightChanged += OnLightChanged;
        if (soundManager == null)
        {
            GameObject soundGameObject = GameObject.FindWithTag("SoundManager");
            if (soundGameObject !=null)
            {
                soundManager = soundGameObject.GetComponent<GenericSoundManager>();
            }
        }
    }
    
    void Update()
    {
        if (healthBar.value <= 1500)
        {
            fill.color = Color.red;
            background.color = Color.blue;
            if (!isBarFlashing)
            {
                if (flashingBar != null) {StopCoroutine(flashingBar);}
                flashingBar = FlashHealthBar();
                isBarFlashing = true;  
                StartCoroutine(flashingBar); 
            }
        }
        else
        {
            if (flashingBar != null) {StopCoroutine(flashingBar);}
            fill.color = Color.white;
            background.color = Color.white;
            isBarFlashing = false;
        }
    }
    
    private IEnumerator FlashHealthBar()
    {
        while (isBarFlashing)
        {
            yield return new WaitForSeconds(0.35f);        
            fill.enabled = false;
            
            yield return new WaitForSeconds(0.35f);
            fill.enabled = true;
            
        }
    } 
    
    private void CriticalHealthSound()
    {
        if (soundManager)
        {
            soundManager.CriticalHealthSound(this.gameObject);
        }
    }
    
    void onDisable()
    {
        player.LightEnergy.LightChanged -= OnLightChanged;
    }
    
    void OnLightChanged(float currentEnergy)
    {
        // Debug.Log("currentEnergy: " + currentEnergy + "\n healthBar: " + healthBar.value);
        healthBar.value = currentEnergy * 100;
    }
}
