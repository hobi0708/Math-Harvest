using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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

    private int[] correctAnswers;
    private int currentQuestionIndex;
    private int score;
    private int lives;
    private int maxQuestions = 10;

    void Start()
    {
        lives = hearts.Length;
        currentQuestionIndex = 0;
        score = 0;
        GenerateQuestions();
        ShowQuestion();
        gameOverPanel.SetActive(false);
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
                PlayerPrefs.SetString($"Question{i}", $"{num1} ¡¿ {num2} = ?");
            }
            else
            {
                int dividend = num1 * num2;
                correctAnswers[i] = num1;
                PlayerPrefs.SetString($"Question{i}", $"{dividend} ¡À {num2} = ?");
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

    public void SubmitAnswer()
    {
        int playerAnswer;
        bool isNumeric = int.TryParse(answerInput.text, out playerAnswer);

        if (isNumeric && playerAnswer == correctAnswers[currentQuestionIndex])
        {
            score++;
        }
        else
        {
            ReduceLife();
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

        resultText.text = $"{score}/{maxQuestions} Correct!";

        if (lives == 0)
        {
            messageText.text = "Game Over! Retry?";
            retryButton.SetActive(true);
            nextLevelButton.SetActive(false);
            tryAgainButton.SetActive(false);
        }
        else
        {
            messageText.text = "Correct! Try Again or Next Level?";
            nextLevelButton.SetActive(true);
            tryAgainButton.SetActive(true);
            retryButton.SetActive(false);
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
}
