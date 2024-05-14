using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PopUpSystem : MonoBehaviour
{
    [SerializeField] private Image popWindow;
    [SerializeField] private GameObject popMessage;
    public string warnText;
    public GameObject textMeshPro_warnText;
    TextMeshProUGUI textmeshpro_objective_text;
    

    void Start() {
        // Text Mesh Pro
        textmeshpro_objective_text = textMeshPro_warnText.GetComponent<TextMeshProUGUI>();
        popWindow.enabled = false;
        popMessage.SetActive(false);
    }

    void Update() {
        textmeshpro_objective_text.text = warnText;
    }

    void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            popWindow.enabled = true;
            popMessage.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other) {
        if (other.CompareTag("Player")) {
            popWindow.enabled = false;
            popMessage.SetActive(false);
        }
    }

}
