using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprite : MonoBehaviour
{
    public GameObject player;
    private player playerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        playerScript = player.GetComponent<player>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
        //transform.localScale = player.transform.localScale;
        
        
    }
}
