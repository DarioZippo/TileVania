using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenePersist : MonoBehaviour
{
    //Singleton pattern per il reload del livello
    void Awake() {
        int numScenePersist = FindObjectsOfType<ScenePersist>().Length;
        if(numScenePersist > 1) {
            Destroy(gameObject);
        }
        else {
            DontDestroyOnLoad(gameObject);
        }
    }
    //La scena viene resettata quando perdo tutte le vite o quando
    //cambio livello
    public void ResetScenePersist() {
        Destroy(gameObject);
    }
}