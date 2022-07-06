using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private GameObject[] statisticObjects;
    private Dictionary<ItemTypes, GameObject> statisticDictionary = new Dictionary<ItemTypes, GameObject>();
    private Dictionary<ItemTypes, int> percentages = new Dictionary<ItemTypes, int>();
    private Camera cam;
    
    [SerializeField] Transform win;
    [SerializeField] Transform lose;
    [SerializeField] private TextMeshProUGUI levelText;
    private TextMeshProUGUI celebrationText, levelCompleteText, consolationText, tryAgainText;
    private Button nextButton;
    private Button restartButton;

    private void Start()
    {
        cam = Camera.main;
        for (int i = 0; i < statisticObjects.Length; i++)
        {
            statisticDictionary.Add((ItemTypes)(i + 3), statisticObjects[i]);
        }

        levelText.text = "LEVEL " + gameManager.levelData.LevelIndex;
        celebrationText = win.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        levelCompleteText = win.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        consolationText = lose.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>();
        tryAgainText = lose.transform.GetChild(1).GetChild(0).GetComponent<TextMeshProUGUI>();
        nextButton = win.GetChild(2).GetComponent<Button>();
        restartButton = lose.GetChild(2).GetComponent<Button>();
    }

    private void OnEnable()
    {
        EventManager.current.onLastDanceTriggered += OnLastDanceTriggered;
        EventManager.current.onWinGame += OnWinGame;
        EventManager.current.onLoseGame += OnLoseGame;
    }

    private void OnDisable()
    {
        EventManager.current.onLastDanceTriggered -= OnLastDanceTriggered;
        EventManager.current.onWinGame -= OnWinGame;
        EventManager.current.onLoseGame -= OnLoseGame;
    }
    
    private void OnLastDanceTriggered()
    {
        ComputeStatistics();
        ShowStatisticsOnCanvas();
        PlaceStatisticsOnCanvas();
        DetermineWinner();
    }

    private void OnWinGame()
    {
        IncreaseLevel();
        win.gameObject.SetActive(true);
        levelCompleteText.alpha = 0;
        celebrationText.transform.localPosition = new Vector3(transform.localPosition.x - 500, transform.position.y, transform.position.z);
        celebrationText.transform.DOMoveX(500, 1f).OnComplete(() =>
        {
            celebrationText.DOFade(0f, 1f).OnComplete(() =>
            {
                nextButton.transform.gameObject.SetActive(true);
                levelCompleteText.DOFade(100f, 1f).OnComplete(() =>
                {
                    EventManager.current.OnFinishGame(); //Go GameManager
                });
            });
        });
    }

    private void OnLoseGame()
    {
        lose.gameObject.SetActive(true);
        
        tryAgainText.alpha = 0;
        consolationText.transform.gameObject.SetActive(true);
        consolationText.transform.localPosition = new Vector3(transform.localPosition.x - 500, transform.position.y, transform.position.z);
        consolationText.transform.DOMoveX(500, 1f).OnComplete(() =>
        {
            consolationText.DOFade(0f, 1f).OnComplete(() =>
            {
                restartButton.transform.gameObject.SetActive(true);

                tryAgainText.DOFade(100f, 1f).OnComplete(() =>
                {
                    EventManager.current.OnFinishGame(); //Go GameManager
                });
            });
        });
    }

    private void ComputeStatistics()
    {
        Dictionary<ItemTypes, int> itemCounts = new Dictionary<ItemTypes, int>(); // { {ItemTypes.CubeRed, 0}, {ItemTypes.CubeGreen, 0}, {ItemTypes.CubeYellow, 0} }
        for (int i = 0; i < statisticDictionary.Count; i++)
        {
            itemCounts.Add(statisticDictionary.ElementAt(i).Key, 0);
        }

        percentages = itemCounts;

        for (var i = 0; i < gameManager.levelGrid.Count; i++)
        {
            for (int j = 0; j < itemCounts.Count; j++)
            {
                if (gameManager.levelGrid[i].type == itemCounts.ElementAt(j).Key)
                {
                    itemCounts[itemCounts.ElementAt(j).Key] += 1;
                }
            }
        }

        int totalCube = 0;
        for (int i = 0; i < itemCounts.Count; i++)
        {
            totalCube += itemCounts.ElementAt(i).Value;
        }
        
        for (int i = 0; i < percentages.Count; i++)
        {
            percentages[percentages.ElementAt(i).Key] = 100 * itemCounts[percentages.ElementAt(i).Key] / totalCube;
        }
    }

    private void ShowStatisticsOnCanvas()
    {
        int biggest = 0;
        for (int i = 0; i < percentages.Count; i++)
        {
            if (percentages.ElementAt(i).Value > biggest)
            {
                biggest = percentages.ElementAt(i).Value;
            }
        }

        //Show Text and Crown
        for (int i = 0; i < statisticDictionary.Count; i++)
        {
            GameObject item = statisticDictionary.ElementAt(i).Value;
            item.SetActive(true);
            
            if (percentages.ElementAt(i).Value == biggest)
            {
                item.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
            }
            else
            {
                item.transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
            }

            if (percentages.ElementAt(i).Value == 0)
            {
                item.SetActive(false);
            }

            item.transform.GetChild(0).GetChild(0).GetComponent<TextMeshProUGUI>().text = percentages.ElementAt(i).Value + "%";
        }
    }

    private void PlaceStatisticsOnCanvas()
    {
        for (int i = 0; i < statisticDictionary.Count; i++)
        {
            GameObject canvasItem = statisticDictionary.ElementAt(i).Value;
            for (int j = 0; j < gameManager.levelGrid.Count; j++)
            {
                if (gameManager.levelGrid[j].type == statisticDictionary.ElementAt(i).Key)
                {
                    canvasItem.transform.position = cam.WorldToScreenPoint(gameManager.levelGrid[j].position);
                    break;
                }
            }
        }
    }

    private void DetermineWinner()
    {
        int biggest = 0;
        for (int i = 0; i < percentages.Count; i++)
        {
            if (percentages.ElementAt(i).Value > biggest)
            {
                biggest = percentages.ElementAt(i).Value;
            }
        }
        
        ItemTypes winner = percentages.FirstOrDefault(x => x.Value == biggest).Key;
        
        if (winner == ItemTypes.CubeGreen)
        {
            EventManager.current.OnWinGame(); //In this Class
        }
        else
        {
            EventManager.current.OnLoseGame(); //In this Class
        }

    }
    
    private void IncreaseLevel()
    {
        if (gameManager.levelData.LevelIndex + 1 <= gameManager.levelData.GetLevelCount())
        {
            gameManager.levelData.LevelIndex++;
        }
    }
    
    public void OnPressNextButton()
    {
        ReloadScene();
    }
    
    public void OnPressRestartButton()
    {
        ReloadScene();
    }
    
    private void ReloadScene()
    {
        DOTween.KillAll();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
