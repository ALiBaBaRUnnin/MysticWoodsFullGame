using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Key : MonoBehaviour
{
    public GameObject Gate;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //WinPAnel
            UIManager.Instance.gameWinPanel_Ref.SetActive(true);
            this.gameObject.SetActive(false);
        }
    }
}
