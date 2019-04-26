using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterParts : MonoBehaviour {

    public static CharacterParts Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("2 CharacterParts objects present");
            Destroy(this);
        }
    }

    private void OnDestroy()
    {
        if(Instance == this)
        {
            Instance = null;
        }
    }
}
