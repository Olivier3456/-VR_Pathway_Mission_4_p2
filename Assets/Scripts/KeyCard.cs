using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KeyCard : MonoBehaviour
{
    private void OnEnable()
    {
        StartCoroutine(EjectOverSeconds(1.5f));
    }

    public IEnumerator EjectOverSeconds(float seconds)
    {
        float elapsedTime = 0;
        while (elapsedTime < seconds)
        {
            transform.position += -transform.forward * Time.deltaTime * 0.1f;
            elapsedTime += Time.deltaTime;

            yield return null;
        }        
    }
}
