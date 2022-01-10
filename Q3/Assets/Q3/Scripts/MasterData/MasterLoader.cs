using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public class MasterLoader : MonoSingleton<MasterLoader>
{
    private const string URL = "https://script.google.com/macros/s/AKfycbzNr5XQwIIpGb8JEQ719wp_9W_4I5KOnncUSvo4hdYcYU0byzi260I27kGlupFLmefo/exec";

    public IEnumerator GetMaster()
    {
        Debug.Log("Request Start");
        UnityWebRequest webRequest = UnityWebRequest.Get(URL);
        yield return webRequest.SendWebRequest();

        Debug.Log(webRequest.result);

        switch (webRequest.result)
        {
            case UnityWebRequest.Result.Success:
                yield return webRequest.downloadHandler.text;
                break;
            default:
                Debug.LogError(webRequest.error);
                break;

        }
    }

    public IEnumerator LoadQuizzes()
    {
        var ie = GetMaster();
        var coroutine = StartCoroutine(ie);
        yield return coroutine;

        var json = (string)ie.Current;
        Debug.Log(json);
        var quizArray = JsonUtility.FromJson<QuizArray>("{\"quizzes\":" + json + "}");
        yield return quizArray.quizzes.ToList();
    }
}