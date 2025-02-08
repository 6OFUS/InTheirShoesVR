using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canal : MonoBehaviour
{
    public SceneTransitionManager transitionManager;

    private void OnTriggerEnter(Collider other)
    {
        transitionManager.ChangeSceneAsyc(SceneManager.GetActiveScene().buildIndex);
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
