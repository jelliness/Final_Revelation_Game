using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Data;
using TMPro;


public class disaPAPER : MonoBehaviour
{
    // Start is called before the first frame update
    public Interactable interactable;
    public float interactionRange = 1.0f; // Set the range as needed
    public KeyCode interactKey = KeyCode.E;
    public string playerUsername = "deokgoo";
    public int currentlvl;
    public int paperCollected = 0;    // Number of papers collected
    public int keyCollected = 0;      // Number of keys collected
    public int remainingHealth = 3; // The player's remaining health
    public TextMeshProUGUI paperText;
    public TextMeshProUGUI keyText;
    public Animator animator;
    public GameObject go;
    protected bool idle = false, run = false, grab_item = false, dead = false;
    public AudioSource audioPlayerPaper, audioPlayerKey;
    private Sprite img1;
    public GameObject MyImage;
    public GameObject ImageHolder;

    void Start()
    {
        paperText = GameObject.FindWithTag("PaperText").GetComponent<TextMeshProUGUI>();
        //keyText = GameObject.FindWithTag("KeyText").GetComponent<TextMeshProUGUI>();
        go = GameObject.FindWithTag("Player");
        animator = GameObject.FindWithTag("Player").GetComponent<Animator>();
        MyImage = GameObject.FindWithTag("ImageFrame");
        ImageHolder = MyImage.transform.Find("ImageHolder").gameObject;
        if (paperText == null)
        {
            Debug.LogError("No GameObject with tag 'PaperText' found.");
        }
        // if (keyText == null)
        // {
        //     Debug.LogError("No GameObject with tag 'KeyText' found.");
        // }

        StartCoroutine(getPlayerLevel("http://localhost/unity2/getPlayerLevel.php", playerUsername));
    }

    void Update()
    {

    }
    void SetBoolValue()
    {
        idle = false;
        run = false;
        grab_item = true;
        dead = false;
        animator.SetBool("Idle", idle);
        animator.SetBool("Run", run);
        animator.SetBool("Grab_Item", grab_item);
        animator.SetBool("Dead", dead);
        Debug.Log("SetBoolValue called: Grab_Item set to true");
    }

    public void Disappear()
    {
        //SetBoolValue();
        string gameElementTag = gameObject.tag; // Retrieve the tag of the paper
        if (gameElementTag == "Key")
        {
            //audioPlayerKey.Play();
            keyText.text = "1/1";
        }
        else if (gameElementTag.Substring(0, 5) == "Paper")
        {
            //audioPlayerPaper.Play();
            int currentPapers = int.Parse(paperText.text.Split('/')[0]);
            paperText.text = (currentPapers + 1).ToString() + "/5";
            if (paperText.text == "5/5")
            {
                Image imageComponent = ImageHolder.GetComponent<Image>();
                img1 = Resources.Load<Sprite>("Clues/Clue_1");
                imageComponent.sprite = img1;
                ImageHolder.SetActive(true);
            }
        }
        paperCollected = int.Parse(paperText.text.Split('/')[0]);
        //keyCollected = int.Parse(keyText.text.Split('/')[0]); //Menu_Script.userInput
        StartCoroutine(storeCollectedItem("http://localhost/unity2/gameElementAdd.php", playerUsername, currentlvl, gameElementTag));
        StartCoroutine(updatePlayer("http://localhost/unity2/progressUpdate.php", playerUsername, currentlvl, go.transform.position.x, go.transform.position.y, paperCollected, keyCollected, remainingHealth, () => gameObject.SetActive(false))); // Pass a callback to run after the coroutine));
        //gameObject.SetActive(false);
    }
    IEnumerator getPlayerLevel(string url, string username)
    {
        WWWForm form = new WWWForm();
        form.AddField("player_username", username);

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

        currentlvl = int.Parse(uwr.downloadHandler.text);

        StartCoroutine(resumeProgress("http://localhost/unity2/gameElementFetch.php", playerUsername, currentlvl));
    }
    IEnumerator storeCollectedItem(string url, string username, int lvl, string gameElementTag)
    {
        WWWForm form = new WWWForm();
        form.AddField("player_username", username);
        form.AddField("player_level", lvl);
        form.AddField("game_element_collected", gameElementTag);
        Debug.Log(gameElementTag);
        using (UnityWebRequest uwr = UnityWebRequest.Post(url, form))
        {
            yield return uwr.SendWebRequest();

            if (uwr.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log("Error While Sending: " + uwr.error);
            }
            else
            {
                Debug.Log("Received: " + uwr.downloadHandler.text);
            }
        }
    }
    IEnumerator updatePlayer(string url, string username, int lvl, double player_position_x, double player_position_y, int paperCollected, int keyCollected, int remainingHealth, Action onCompleted)
    {
        WWWForm form = new WWWForm();
        form.AddField("player_username", username);
        form.AddField("player_level", lvl);
        form.AddField("player_position_x", player_position_x.ToString());
        form.AddField("player_position_y", player_position_y.ToString());
        form.AddField("paper_collected", paperCollected);
        form.AddField("key_collected", keyCollected);
        form.AddField("remaining_health", remainingHealth);

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
            onCompleted?.Invoke(); //para mawait before magdisappear yung papel
        }
    }
    IEnumerator resumeProgress(string url, string username, int lvl)
    {
        WWWForm form = new WWWForm();
        form.AddField("player_username", username);
        form.AddField("player_level", lvl);

        UnityWebRequest uwr = UnityWebRequest.Post(url, form);
        yield return uwr.SendWebRequest();

        if (uwr.result == UnityWebRequest.Result.ConnectionError)
        {
            Debug.Log("Error While Sending: " + uwr.error);
        }
        else
        {
            Debug.Log("Received: " + uwr.downloadHandler.text);
        }

        string[] gameElements = (uwr.downloadHandler.text).Split('/');

        try
        {
            foreach (string element in gameElements)
            {
                GameObject objectToDisappear = GameObject.Find(element);
                objectToDisappear.SetActive(false);
            }
        }
        catch (NullReferenceException)
        {
            ;
        }
        catch (UnassignedReferenceException)
        {
            ;
        }
    }
}
