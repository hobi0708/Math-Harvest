using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class GameManagerL3 : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TMP_InputField answerInput;
    public GameObject[] hearts;
    public GameObject gameOverPanel;
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI messageText;
    public GameObject titleButton;
    public GameObject retryButton;
    public GameObject tryAgainButton;
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
        lives = hearts.Length;
        currentQuestionIndex = 0;
        score = PlayerPrefs.GetInt("Score", 0);
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
            int op = Random.Range(0, 4);
            int num1 = Random.Range(10, 50);
            int num2 = Random.Range(1, 10);

            switch (op)
            {
                case 0:
                    correctAnswers[i] = num1 + num2;
                    PlayerPrefs.SetString($"Question{i}", $"{num1} + {num2} = ?");
                    break;
                case 1:
                    correctAnswers[i] = num1 - num2;
                    PlayerPrefs.SetString($"Question{i}", $"{num1} - {num2} = ?");
                    break;
                case 2:
                    correctAnswers[i] = num1 * num2;
                    PlayerPrefs.SetString($"Question{i}", $"{num1} ¡¿ {num2} = ?");
                    break;
                case 3:
                    int result = num1;
                    int divisor = num2;
                    int dividend = result * divisor;
                    correctAnswers[i] = result;
                    PlayerPrefs.SetString($"Question{i}", $"{dividend} ¡À {divisor} = ?");
                    break;
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
            tryAgainButton.SetActive(false);
            titleButton.SetActive(false);
        }
        else
        {
            messageText.text = "Well Done! Play Again or Return to Title?";
            tryAgainButton.SetActive(true);
            retryButton.SetActive(false);
            titleButton.SetActive(true);
        }
    }

    public void GoToTitle()
    {
        PlayerPrefs.SetInt("Score", 0);
        SceneManager.LoadScene("Title");
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("Level3");
    }

    void UpdateScoreText()
    {
        scoreText.text = $": {score}";
    }
}
