using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class Score : MonoBehaviour
{
    [SerializeField] private string URL;
    [SerializeField] Text[] texts;
    [SerializeField] GameObject peekaboo;
    private string Token;

    public static int score = 0;
    public static int highScore = 0;

    void Start()
    {
        score = 0;
        highScore = PlayerPrefs.GetInt("High Score", highScore);
        Token = PlayerPrefs.GetString("token");
    }
    private void Update()
    {
        GetComponent<Text>().text = score.ToString();
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("High Score", highScore);
        }
    }
    public void ClickGetScores()
    {
        StartCoroutine(GetScores());
    }
    public void ClosePeekaboo()
    {
        peekaboo.SetActive(false);
    }
    public void UpdatePoints()
    {
        Token = PlayerPrefs.GetString("token");
        UserData userData = new UserData();
        userData.username = PlayerPrefs.GetString("username");
        userData.score = PlayerPrefs.GetInt("High Score");
        string postData = JsonUtility.ToJson(userData);
        StartCoroutine(PatchScore(postData));
    }
    IEnumerator GetScores()
    {
        string url = URL + "/api/usuarios" + "?limit=5&sort=true";
        UnityWebRequest www = UnityWebRequest.Get(url);

        www.method = "GET";
        www.SetRequestHeader("content-type", "application/json");
        www.SetRequestHeader("x-token", Token);

        peekaboo.SetActive(true);
        yield return www.SendWebRequest();

        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if (www.responseCode == 200)
        {
            Debug.Log(www.downloadHandler.text);
            Scores resData = JsonUtility.FromJson<Scores>(www.downloadHandler.text);
            int i = 0;
            foreach (UserData score in resData.usuarios)
            {
                texts[i].text = resData.usuarios[i].username + " | " + resData.usuarios[i].score;
                Debug.Log(resData.usuarios[i].username + "," + resData.usuarios[i].score);
                i++;
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }
    IEnumerator PatchScore(string postData)
    {
        Debug.Log("PATCH SCORE: ");
        string url = URL + "/api/usuarios";
        UnityWebRequest www = UnityWebRequest.Put(url, postData);

        www.method = "PATCH";
        www.SetRequestHeader("content-type", "application/json");
        www.SetRequestHeader("x-token", Token);

        yield return www.SendWebRequest();
        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if (www.responseCode == 200)
        {
            AuthData resData = JsonUtility.FromJson<AuthData>(www.downloadHandler.text);
        }
        else
        {
            Debug.Log(www.error);
            Debug.Log(www.downloadHandler.text);
        }
    }

    [Serializable]
    public class Scores
    {
        public UserData[] usuarios;
    }
}
