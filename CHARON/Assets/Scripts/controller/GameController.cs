using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using System;


public class GameController : MonoBehaviour
{
    public AudioSource ballaudioSource;
    public AudioSource winaudioSource;
    public AudioSource matchaudioSource;

    public Button startButton;
    public Button credsupButton;
    public Button credsdownButton;
    public Button newcardButton;

    public TextMeshProUGUI startText;

    public TextMeshProUGUI credsText;
    public TextMeshProUGUI roundsText;
    public TextMeshProUGUI winText;

    public GameObject ballsBoard;
    public GameObject winBoard;
    public GameObject coverBoard;

    public int maxValue;

    public TextMeshProUGUI[] resultValue;
    public TextMeshProUGUI[] numbersCardText;

    public GameObject[] gridValues;
    public GameObject[] ballsObject;

    public Button[] allButtons;
    public TextMeshProUGUI[] allbuttonsText;

    public int[] reward;

    private List<int> yourNumbers;
    private List<int> results;
    private List<int> possibleResults;


    private void Start()
    {
        startButton.interactable = !startButton.interactable;
        winText.text = "";

        for (int i = 0; i < 30; i++)
        {
            resultValue[i].text = "";
        }
        for (int i = 0; i < 15; i++)
        {
            numbersCardText[i].text = "";
        }
        //GetNewCard();
    }

    public void Update()
    {
        for (int i = 0; i < 4 ; i++)
        {
            if (allButtons[i].interactable == false)
            {
                allbuttonsText[i].color = Color.grey;
            }
            else
            {
                allbuttonsText[i].color = Color.white;
            }
        }
    }

    //Start to play the game and make buttons non interactable
    public void StartGame()
    {
        startButton.interactable = false;
        credsupButton.interactable = false;
        credsdownButton.interactable = false;
        newcardButton.interactable = false;

        if (updateCreds.credsValue != 0)
        {
            updateCreds.credsValue--;
            credsText.text = updateCreds.credsValue.ToString();

            updateRounds.roundValue++;
            roundsText.text = updateRounds.roundValue.ToString();

            winText.text = "";

            for (int i = 0; i < 30; i++)
            {
                resultValue[i].text = "";
            }
            StartCoroutine(PickBall());
        }
    }

    //print the numbers inside the card
    public void GetNewCard()
    {
        winText.text = "";
        startButton.interactable = true;
        coverBoard.SetActive(false);

        foreach (GameObject obj in ballsObject)
        {
            obj.SetActive(false);
        }

        for (int i = 0; i < 30; i++)
        {
            resultValue[i].text = "";
        }

        yourNumbers = GetNumbers();

        for (int i = 0; i < 15; i++)
        {
            numbersCardText[i].text = yourNumbers[i].ToString("00");
        }
        //make them not show until pressed new card
        foreach(GameObject _gameObject in gridValues)
        {
            _gameObject.SetActive(false);
        }
    }

    //count the numbers that matched with the card
    private int GetMatchingTotal()
    {
        int total = 0;

        foreach (int i in yourNumbers)
        {
            if (results.Contains(i))
            {
                total++;
            }
        }
        return total;
    }

    //At the end of taking 30 balls and depending on how many matched, show a message
    private void DealWithReward(int total)
    {
        winBoard.SetActive(true);
        winaudioSource.Play();

        updateCreds.credsValue = updateCreds.credsValue + +reward[total - 1];
        credsText.text = updateCreds.credsValue.ToString();

        credsupButton.interactable = !startButton.interactable;
        credsdownButton.interactable = !startButton.interactable;
        newcardButton.interactable = !startButton.interactable;

        if (total == 15)
        {
            winText.text = "BINGO";
            ballaudioSource.Play();
            return;
        }
        winText.text = "You lost, but you matched " + total + " and won " + reward[total - 1] + " credits."+ "\n" + "Press New card and Start to play again";
    }

    //List of possible results for the ball numbers
    private void ResetPossibleResults()
    {
        possibleResults = new List<int>();

        for (int i = 1; i <= 30; i++)
        {
            possibleResults.Add(i);
        }
    }

    //Generate list of new numbers for the card 
    private List<int> GetNumbers()
    {
        ResetPossibleResults();

        var numbers = Enumerable.Range(1, 60).ToList();
        List<int> list = new List<int>();
        //generate random list of 15 values
        while (list.Count != 15)
        {
            int position = UnityEngine.Random.Range(0, numbers.Count);

            list.Add(numbers[position]);
            numbers.RemoveAt(position);
        }
        list.Sort();
        return list;
    }

    //generate new list of numbers for the Balls
    private List<int> GetBallsNumbers()
    {
        ResetPossibleResults();

        var numbers = Enumerable.Range(1, 60).ToList();
        List<int> list = new List<int>();
        //create random list of 30 values
        while (list.Count != 30)
        {
            int position = UnityEngine.Random.Range(0, numbers.Count);

            list.Add(numbers[position]);
            numbers.RemoveAt(position);
        }
        return list;
    }

    //Take a number from the randomized list
    private IEnumerator PickBall()
    {
        ballsBoard.SetActive(true);

        yield return new WaitForSeconds(2);

        results = GetBallsNumbers();
        //print the number on the ball every 2 seconds
        for (int i = 0; i < 30; i++)
        {
            resultValue[i].text = results[i].ToString("00");
            ballsObject[i].SetActive(true);
            ballaudioSource.Play();
            //If the number matches, spawn the cross
            if (yourNumbers.Contains(results[i]))
            {
                int c = yourNumbers.IndexOf(results[i]);
                gridValues[c].SetActive(true);
                matchaudioSource.Play();
                Debug.Log(c);
            }
            yield return new WaitForSeconds(1);
        }
        DealWithReward(GetMatchingTotal());
    }
}
