using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Elevator : MonoBehaviour
{
    [SerializeField] private Light elevatorLight;
    [SerializeField] private Transform doorL;
    [SerializeField] private Transform doorR;
    [SerializeField] private GameObject part;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FlickerLight());
    }
    
    IEnumerator FlickerLight()
    {
        while (true)
        {
            elevatorLight.enabled = !elevatorLight.enabled;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.5f));
        }
    }

    public void PartBroughtBack()
    {
        if (!part.activeSelf)
        {
            SceneManager.LoadScene("EndScene");
        }
    }


    public void OpenElevator()
    {
        doorL.transform.DOBlendableLocalMoveBy(new Vector3(-3f, 0, 0f), 4f);
        doorR.transform.DOBlendableLocalMoveBy(new Vector3(3f, 0, 0f), 4f);
    }
}
