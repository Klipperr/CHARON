using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class updateRounds : MonoBehaviour
{
    public static int roundValue = 0;
    public TextMeshProUGUI textRounds;


    public void IncrementRounds()
    {
        if (textRounds != null)
        {
            ++roundValue;
            textRounds.text = roundValue.ToString();
        }
    }
}
