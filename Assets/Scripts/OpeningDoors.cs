using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningDoors : MonoBehaviour
{
    public Transform teleportPosition;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (this.gameObject.tag == "Door3")
            {
                if (SceneManager.GetActiveScene().name == "Level1")
                {
                    SceneManager.LoadScene("Level2");
                    PlayerController.instance.GetComponent<Transform>().position = new Vector3(22f, -10f, 0f);
                } 
                else if (SceneManager.GetActiveScene().name == "Level2") {
                    SceneManager.LoadScene("Level3");
                    PlayerController.instance.GetComponent<Transform>().position = new Vector3(22f, -10f, 0f);
                }
                else if (SceneManager.GetActiveScene().name == "Level3")
                {
                    SceneManager.LoadScene("Level4");
                    PlayerController.instance.GetComponent<Transform>().position = new Vector3(22f, -10f, 0f);
                }
                else if (SceneManager.GetActiveScene().name == "Level4")
                {
                    SceneManager.LoadScene("Level5");
                    PlayerController.instance.GetComponent<Transform>().position = new Vector3(22f, -10f, 0f);
                }
                else if (SceneManager.GetActiveScene().name == "Level5")
                {
                    SceneManager.LoadScene("DungeonScene");
                    PlayerController.instance.GetComponent<Transform>().position = new Vector3(22f, -10f, 0f);
                }
            }
            else { 
                if (teleportPosition != null) { 
                    other.gameObject.transform.position = teleportPosition.position;
                }
            }
        }
    }
}
