using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class updateCreds : MonoBehaviour
{
    public static int credsValue = 0;
    public TextMeshProUGUI textCreds;


    public void IncrementCreds()
    {
        if (credsValue >= 0)
        {
            ++credsValue;
            textCreds.text = credsValue.ToString();
        }
    }

    public void DecrementCreds()
    {
        if (credsValue > 0)
        {
            --credsValue;
            textCreds.text = credsValue.ToString();
        }
    }
}