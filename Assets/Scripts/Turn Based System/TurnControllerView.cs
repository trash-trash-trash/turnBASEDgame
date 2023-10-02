using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TurnControllerView : MonoBehaviour
{
    public GameObject playerOneTextObj;
    public TMP_Text playerOneText;
    private string p1prevText;

    public GameObject playerTwoTextObj;
    public TMP_Text playerTwoText;
    private string p2prevText;

    public TurnController controller;

    public Vector3 moveAmount;

    public float textFadeSpeed;

    public bool fading = false;

    public void OnEnable()
    {
        controller.AnnounceControllerString += ChangeText;
    }

    public void ChangeText(string input, TurnTakerID ID)
    {
        NewText(ID, input);
    }

    public void NewText(TurnTakerID ID, string newString)
    {
        if (ID == TurnTakerID.PlayerOne)
        {

            playerOneText.text = newString;
            p1prevText = newString;
            
            if (!fading)
            {
                GameObject newPlayerOneTextObj =
                    Instantiate(playerOneTextObj, playerOneTextObj.transform, true) as GameObject;

                TMP_Text newText = newPlayerOneTextObj.GetComponent<TMP_Text>();

                newText.text = newString;

                StartCoroutine(FadeText(newPlayerOneTextObj, newText, -moveAmount));
            }

        }
        else if (ID == TurnTakerID.PlayerTwo)
        {

            playerTwoText.text = newString;
            p2prevText = newString;

            if (!fading)
            {

                GameObject newPlayerTwoTextObj =
                    Instantiate(playerTwoTextObj, playerTwoTextObj.transform, true) as GameObject;

                TMP_Text newText = newPlayerTwoTextObj.GetComponent<TMP_Text>();

                newText.text = newString;


                StartCoroutine(FadeText(newPlayerTwoTextObj, newText, moveAmount));
            }

        }

    }

    private IEnumerator FadeText(GameObject input, TMP_Text inputText, Vector3 amount)
    {
        fading = true;
        while (inputText.alpha > 0)
        {
            inputText.alpha -= textFadeSpeed * Time.deltaTime;

            input.transform.Translate(amount * Time.deltaTime);

            yield return null;
        }

        inputText.alpha = 0;

        Destroy(input);
        fading = false;
    }

    public void OnDisable()
    {
        controller.AnnounceControllerString -= ChangeText;
    }
}