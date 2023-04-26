using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberPad : MonoBehaviour
{
    [SerializeField] private int[] goodCode;

    [SerializeField] private TextMeshProUGUI codeDisplay;


    [SerializeField] private AudioClip goodCodeAudioClip;
    [SerializeField] private AudioClip wrongCodeAudioClip;

    [SerializeField] private GameObject keyCard;


    private int[] codeEntered;

    private int index = 0;

    private bool goodCodeEntered = false;

    private AudioSource audioSource;

    void Start()
    {
        codeEntered = new int[goodCode.Length];
        audioSource = GetComponent<AudioSource>();
        ChangeCodeDisplayColor(Color.white);
    }


    public void ButtonPressed(int buttonValue)
    {
        if (!goodCodeEntered)
        {
            codeEntered[index] = buttonValue;
            index++;
            ChangeCodeDisplayColor(Color.white);

            DisplayCode();

            if (index == goodCode.Length)
            {
                if (CheckCode())
                {
                    GoodCode();
                }
                else
                {
                    WrongCode();
                }
            }
        }
    }

    bool CheckCode()
    {
        for (int i = 0; i < goodCode.Length; i++)
        {
            if (goodCode[i] != codeEntered[i])
            {
                return false;
            }
        }
        return true;
    }


    void DisplayCode()
    {
        codeDisplay.text = "";

        for (int i = 0; i < index; i++)
        {
            codeDisplay.text += codeEntered[i];
        }

    }


    void GoodCode()
    {
        audioSource.PlayOneShot(goodCodeAudioClip);
        ChangeCodeDisplayColor(Color.green);
        keyCard.SetActive(true);
        keyCard.GetComponent<Rigidbody>().isKinematic = true;
        goodCodeEntered = true;
    }

    void WrongCode()
    {
        index = 0;
        audioSource.PlayOneShot(wrongCodeAudioClip);
        ChangeCodeDisplayColor(Color.red);
    }

    void ChangeCodeDisplayColor(Color color)
    {
        codeDisplay.material.color = color;
    }

    private void OnDisable()
    {
        ChangeCodeDisplayColor(Color.white);
    }
}
