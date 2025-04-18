using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManagerL2 : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TMP_InputField answerInput;
    public GameObject[] hearts;
    public GameObject gameOverPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI messageText;
    public GameObject nextLevelButton;
    public GameObject retryButton;
    public GameObject tryAgainButton;
    public GameObject toTitleButton1;
    public GameObject toTitleButton2;
    public AudioSource sfxSource;
    public AudioClip correctClip;
    public AudioClip wrongClip;
    public Image farmerImage;
    public Sprite neutralSprite;
    public Sprite correctSprite;
    public Sprite wrongSprite;
    public TextMeshProUGUI scoreText;


    private int[] correctAnswers;
    private int currentQuestionIndex;
    private int score;
    private int lives;
    private int maxQuestions = 10;
    private int correctCount = 0;

    void Start()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        lives = hearts.Length;
        currentQuestionIndex = 0;
        GenerateQuestions();
        ShowQuestion();
        gameOverPanel.SetActive(false);
        UpdateScoreText();
    }


    void GenerateQuestions()
    {
        correctAnswers = new int[maxQuestions];

        for (int i = 0; i < maxQuestions; i++)
        {
            int num1 = Random.Range(10, 50);
            int num2 = Random.Range(1, 10);
            bool isMultiplication = Random.value > 0.5f;

            if (isMultiplication)
            {
                correctAnswers[i] = num1 * num2;
                PlayerPrefs.SetString($"Question{i}", $"{num1} �� {num2} = ?");
            }
            else
            {
                int dividend = num1 * num2;
                correctAnswers[i] = num1;
                PlayerPrefs.SetString($"Question{i}", $"{dividend} �� {num2} = ?");
            }
        }
    }

    void ShowQuestion()
    {
        if (currentQuestionIndex < maxQuestions)
        {
            questionText.text = PlayerPrefs.GetString($"Question{currentQuestionIndex}");
        }
        else
        {
            GameOver();
        }
    }

    void PlaySound(bool isCorrect)
    {
        if (isCorrect)
            sfxSource.PlayOneShot(correctClip);
        else
            sfxSource.PlayOneShot(wrongClip);
    }

    IEnumerator ShowFarmerReaction(Sprite reactionSprite)
    {
        farmerImage.sprite = reactionSprite;
        yield return new WaitForSeconds(1f);
        farmerImage.sprite = neutralSprite;
    }

    void UpdateScoreText()
    {
        scoreText.text = $": {score}";
    }

    public void SubmitAnswer()
    {
        int playerAnswer;
        bool isNumeric = int.TryParse(answerInput.text, out playerAnswer);

        if (isNumeric && playerAnswer == correctAnswers[currentQuestionIndex])
        {
            score += 10;
            correctCount++;
            PlaySound(true);
            StartCoroutine(ShowFarmerReaction(correctSprite));
            UpdateScoreText();
        }
        else
        {
            ReduceLife();
            PlaySound(false);
            StartCoroutine(ShowFarmerReaction(wrongSprite));
        }

        currentQuestionIndex++;
        answerInput.text = "";
        ShowQuestion();
    }

    void ReduceLife()
    {
        if (lives > 0)
        {
            lives--;
            hearts[lives].SetActive(false);
        }

        if (lives == 0)
        {
            GameOver();
        }
    }

    void GameOver()
    {
        gameOverPanel.SetActive(true);

        resultText.text = $"{correctCount}/{maxQuestions} Correct!";

        PlayerPrefs.SetInt("Score", score);

        if (lives == 0)
        {
            messageText.text = "Game Over! Retry?";
            retryButton.SetActive(true);
            nextLevelButton.SetActive(false);
            tryAgainButton.SetActive(false);
            toTitleButton1.SetActive(true);
            toTitleButton2.SetActive(false);
        }
        else
        {
            messageText.text = "Correct! Try Again or Next Level?";
            nextLevelButton.SetActive(true);
            tryAgainButton.SetActive(true);
            retryButton.SetActive(false);
            toTitleButton1.SetActive(false);
            toTitleButton2.SetActive(true);
        }
    }


    public void RetryGame()
    {
        SceneManager.LoadScene("Level2");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene("Level3");
    }

    public void GoToTitle()
    {
        PlayerPrefs.SetInt("Score", 0);
        SceneManager.LoadScene("Title");
    }
}
