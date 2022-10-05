using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bullet : MonoBehaviour {
    private void OnCollisionEnter(Collision other) {
        Destroy(gameObject);

        if(other.gameObject.CompareTag("Player")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        else if(other.gameObject.CompareTag("Enemy")) {
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject.CompareTag("Finish")) {
            GameController.instance.subject.Notify(other.gameObject.GetHashCode());
        }
    }
}
