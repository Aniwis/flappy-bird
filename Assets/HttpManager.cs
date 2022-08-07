using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class HttpManager : MonoBehaviour
{

    [SerializeField]
    private string URL;
    [SerializeField]
    private Text[] texts;
    [SerializeField]
    private GameObject peekaboo;
    public void ClickGetScores()
    {
        StartCoroutine(GetScores());
    }

    IEnumerator GetScores()
    {
        string url = URL + "/leaders";
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();
        peekaboo.SetActive(true);
        if (www.isNetworkError)
        {
            Debug.Log("NETWORK ERROR " + www.error);
        }
        else if(www.responseCode == 200){
            //Debug.Log(www.downloadHandler.text);
            Scores resData = JsonUtility.FromJson<Scores>(www.downloadHandler.text);
            int i = 0;
            foreach (ScoreData score in resData.scores)
            {
                texts[i].text = score.name + " | " + score.value;
                Debug.Log(score.name +" | "+score.value);
                i++;
            }
        }
        else
        {
            Debug.Log(www.error);
        }
    }

   public void OnClickOcultar()
    {
        peekaboo?.SetActive(false);
    }
}


[System.Serializable]
public class ScoreData
{
    public string name;
    public int userId;
    public int value;

}


[System.Serializable]
public class Scores
{
    public ScoreData[] scores;
}

