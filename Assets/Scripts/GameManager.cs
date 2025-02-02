using System.Collections;
using System.Collections.Generic;
using System.Security;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string playerID;
    public string playerEmail;
    public string playerName;
    public string playerDateJoined;


    private void Awake()
    {
        if (Instance == null) 
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject); 
        }
        else if (Instance != null && Instance != this) 
        {
            Destroy(gameObject); 
        }

    }
    public void StorePlayerDetails(string uID, string name, string email, string dateJoined)
    {
        playerID = uID;
        playerName = name;
        playerEmail = email;
        playerDateJoined = dateJoined;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
