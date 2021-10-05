using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetButton : MonoBehaviour
{
    [SerializeField]
    string sceneToLoad;
    public void ResetScene()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Joystick1Button7))
        {
            ResetScene();
        }
    }
}
