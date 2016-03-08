﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : LightSource
{
    [Tooltip("If set to true, this checkpoint will teleport player to the next scene")]
    public bool changeScene = false;
    
    public override void Awake()
    {       
        base.Awake(); // call parent LightSource Awake() first
        this.infiniteEnergy = true; // override default LightSource value
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            int currentLevel = other.gameObject.GetComponent<Player>().CurrentLevel;
            PlayerData data = new PlayerData();
            
            if (changeScene)
            {
                // if checkpoint changes scene, save values for the new scene
                data.playerPosition = DataManager.Vector3ToString(new Vector3(0, 0, 0));
                data.levelID = currentLevel + 1;    
            } 
            else
            {
                data.playerPosition = DataManager.Vector3ToString(other.gameObject.transform.position);
                data.levelID = currentLevel;    
            }            
            
            data.playerRotation = DataManager.Vector3ToString(other.gameObject.transform.localEulerAngles);
            data.playerScale = DataManager.Vector3ToString(other.gameObject.transform.localScale);
            data.playerEnergy = other.gameObject.GetComponent<Player>().LightEnergy.CurrentEnergy;            
            DataManager.SaveFile(data);    
            
            if (changeScene)
            {
                ChangeLevel(currentLevel + 1);
            }        
        }
    }
    
    private void ChangeLevel(int levelID)
    {
        if (SceneManager.sceneCountInBuildSettings > levelID)
        {
            SceneManager.LoadScene(levelID, LoadSceneMode.Single);
        }
    }

}

