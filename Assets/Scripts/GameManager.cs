using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject startScreen;
    public GameObject header;
    public GameObject scoreScreen;
    private static readonly List<int> ParList = new List<int> {2, 3, 2 ,3};
    public GameObject completeScreen;
    public static int StrokeCount;
    private static int _score;
    public TextMeshProUGUI strokeText;
    public TextMeshProUGUI parText;
    public TextMeshProUGUI holeText;
    public TextMeshProUGUI scoreText;
    public List<TextMeshProUGUI> scoreTexts;
    public Transform cameraPoint;
    public PlayerAction playerAction;
    private Rigidbody _rigidbody;


    private static int _currentHole = 1;

    private static readonly List<Vector3> HolePositions = new List<Vector3>
    {
        new(4.5f, 1, 23),
        new(0, 1, 0),
        new(-5, 1, 9),
        new(14, 1, 18)
    };

    private void Start()
    {
        transform.position = HolePositions[_currentHole - 1];
        _rigidbody = GetComponent<Rigidbody>();
        holeText.text = _currentHole.ToString();
        parText.text = ParList[_currentHole - 1].ToString();
        strokeText.text = "0";
    }

    // Update is called once per frame
    private void Update()
    {
        strokeText.text = StrokeCount.ToString();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hole"))
        {
            MoveNextHole();
        }
        else if (other.CompareTag("LevelBounds"))
        {
            transform.position = HolePositions[_currentHole - 1];
            _rigidbody.velocity = new Vector3(0, 0, 0);
        }
    }

    private void MoveNextHole()
    {
        if (_currentHole == 4)
        {
            playerAction.enabled = false;
            cameraPoint.position = new Vector3(0, 50, 0);
            completeScreen.SetActive(true);
            header.SetActive(false);
        }
        else
        {
            scoreTexts[_currentHole - 1].text = (StrokeCount - ParList[_currentHole - 1]).ToString();
            _score += (StrokeCount - ParList[_currentHole - 1]);
            scoreText.text = "SCORE: " + _score;
            StrokeCount = 0;
            _currentHole++;
            var nextHolePos = HolePositions[_currentHole - 1];
            holeText.text = _currentHole.ToString();
            parText.text = ParList[_currentHole - 1].ToString();
            transform.position = nextHolePos;
            _rigidbody.velocity = new Vector3(0, 0, 0);
        }
    }

    public void StartGame()
    {
        startScreen.SetActive(false);
        header.SetActive(true);
        scoreScreen.SetActive(true);
        playerAction.enabled = true;
    }

    public void GoMenu()
    {
        completeScreen.SetActive(false);
        scoreScreen.SetActive(false);
        startScreen.SetActive(true);
    }
}
