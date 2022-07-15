using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static bool levelStarted, levelFailed, levelPassed;

    [SerializeField]
    Transform levelObjectsFolder;

    [SerializeField]
    Camera myCam;

    [SerializeField] Transform spawnFolder;

    [SerializeField] SpriteRenderer maintTitle, mainTitleBG, instructionsTitle, instructionsBG, whiteSquare, levelPassedSR, levelPassedBG;

    [SerializeField] Transform[] retryTexts, winTexts;

    bool restartLogic, startLogic, passedLogic;

    [SerializeField] TextMeshPro currentLevel;

    [SerializeField] GameObject charTrail;
    [SerializeField] Animator charAnim;

    // level spawner literature
     Vector3 spawnPosition;
    int objToSpawnInt;

    private void Awake()
    {
        Application.targetFrameRate = 60;

        levelStarted = false;
        levelFailed = false;
        levelPassed = false;

        restartLogic = false;
        startLogic = false;
        passedLogic = false;

        currentLevel.text = PlayerPrefs.GetInt("levelCount", 0) + "";
    }

    private void Start()
    {
        StartCoroutine(StartLogic());
        PlayerPrefs.SetInt("GamesSinceLastAdPop", PlayerPrefs.GetInt("GamesSinceLastAdPop", 0)+1);
    }

    private void Update()
    {
        if (!restartLogic && levelFailed && !passedLogic)
        {
            Transform tempObj = retryTexts[Random.Range(0, retryTexts.Length)].transform;
            SpriteRenderer retryTitle, retryBg;
            retryTitle = tempObj.GetComponent<SpriteRenderer>();
            retryBg = tempObj.GetComponentsInChildren<SpriteRenderer>()[1];

            charAnim.SetTrigger("cry");
            Camera_Tracker.stopCameraTracking = true;

            StartCoroutine(RetryLiterature(retryTitle, retryBg));
            StartCoroutine(RestartWait());            

            restartLogic = true;
        }

        if (!startLogic && levelStarted)
        {
            StartCoroutine(FadeImageOut(maintTitle));
            StartCoroutine(FadeImageOut(mainTitleBG));
            StartCoroutine(FadeImageOut(instructionsTitle));
            StartCoroutine(FadeImageOut(instructionsBG));
            startLogic = true;
        }

        if (!passedLogic && levelPassed)
        {
            PlayerPrefs.SetInt("SpawnNewLevel", 1);
            PlayerPrefs.SetInt("TilesCount", 0);

            PlayerPrefs.SetInt("Tile_" + 0, 1);
            for (int i = 1; i < 10; i++)
                PlayerPrefs.SetInt("Tile_"+i, 4);
            PlayerPrefs.SetInt("Tile_" + 10, 1);

            // winTexts
            Transform tempObj = winTexts[4].transform;

            SpriteRenderer winTitle, winBG;
            winTitle = tempObj.GetComponent<SpriteRenderer>();
            winBG = tempObj.GetComponentsInChildren<SpriteRenderer>()[1];

            StartCoroutine(RetryLiterature(winTitle, winBG));

            PlayerPrefs.SetInt("levelCount", PlayerPrefs.GetInt("levelCount", 1) + 1); // increment
            StartCoroutine(RestartWait());

            charTrail.SetActive(false);

            passedLogic = true;
        }
    }


    void SpawnNewLevel()
    {

    }

    IEnumerator StartLogic()
    {
        whiteSquare.enabled = true;
        whiteSquare.color = Color.white;
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(FadeImageOut(whiteSquare));
    }

    IEnumerator RetryLiterature(SpriteRenderer mainText, SpriteRenderer bgText)
    {
        float timer = 0, totalTime = 40;
        Color startingColor1 = mainText.color;
        Color startingColor2 = bgText.color;
        Transform textTransform = mainText.gameObject.transform.parent.transform;

        Vector3 startingScale = textTransform.localScale;

        while (timer <= totalTime)
        {
            if (timer <= 18)
                textTransform.localScale = Vector3.Lerp(startingScale*0.1f, startingScale * 1.65f, timer / (totalTime-18));

            if (timer < totalTime * 0.75f)
            {
                mainText.color = Color.Lerp(startingColor1, new Color(startingColor1.r, startingColor1.g, startingColor1.b, 1), timer / (totalTime*0.7f));
                bgText.color = Color.Lerp(startingColor2, new Color(startingColor2.r, startingColor2.g, startingColor2.b, 1), timer / (totalTime*0.7f));
            }

            yield return new WaitForFixedUpdate();
            timer++;
        }

        timer = 0;
        totalTime = 80;
        startingScale = textTransform.localScale;
        while (timer <= totalTime)
        {
            textTransform.localScale = Vector3.Lerp(startingScale, new Vector3(startingScale.x*1.15f, startingScale.y*1.5f, startingScale.z*1.5f), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator RestartWait()
    {
        yield return new WaitForSecondsRealtime(1.9f);
        StartCoroutine(RestartLevel(whiteSquare));
    }

    IEnumerator RestartLevel(SpriteRenderer myImage)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    IEnumerator FadeImageOut(SpriteRenderer myImage)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 1), new Color(startingColor.r, startingColor.g, startingColor.b, 0), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
        myImage.enabled = false;
    }

    IEnumerator FadeImageIn(SpriteRenderer myImage, float totalTime)
    {
        float timer = 0;
        Color startingColor = myImage.color;
        myImage.enabled = true;
        while (timer <= totalTime)
        {
            myImage.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator FadeTextOut(TextMeshPro myTtext)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myTtext.color;
        while (timer <= totalTime)
        {
            myTtext.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 1), new Color(startingColor.r, startingColor.g, startingColor.b, 0), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }

    IEnumerator FadeTextIn(TextMeshPro myTtext)
    {
        float timer = 0, totalTime = 24;
        Color startingColor = myTtext.color;
        while (timer <= totalTime)
        {
            myTtext.color = Color.Lerp(new Color(startingColor.r, startingColor.g, startingColor.b, 0), new Color(startingColor.r, startingColor.g, startingColor.b, 1), timer / totalTime);
            yield return new WaitForFixedUpdate();
            timer++;
        }
    }
}
