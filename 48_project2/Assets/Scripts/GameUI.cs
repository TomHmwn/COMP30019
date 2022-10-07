using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
public class GameUI : MonoBehaviour
{

	public Image fadePlane;
	public GameObject gameOverUI;
	public Text gameOverScoreUI;
	public Text gameOverWaveUI;


	public Text scoreUI;
	public HealthHUD healthBar;
	public AmmoDisplay ammoDisplay;
	public Text waveDisplay;
	public Image controlsUI;
	Player player;
	void Start()
	{
		player = FindObjectOfType<Player>();




		player.OnDeath += OnGameOver;
	}

	void OnGameOver()
	{
		Cursor.visible = true;
		StartCoroutine(Fade(Color.clear, new Color(1, 1, 1, .95f), 1));
		gameOverScoreUI.text = scoreUI.text;
		gameOverWaveUI.text = waveDisplay.text;
		scoreUI.gameObject.SetActive(false);
		healthBar.gameObject.SetActive(false);
		ammoDisplay.gameObject.SetActive(false);
		waveDisplay.gameObject.SetActive(false);
		gameOverUI.SetActive(true);
	}

	void Update()
	{
		if(Input.GetKey(KeyCode.Tab)){
			controlsUI.gameObject.SetActive(true);
		}
		else{
			controlsUI.gameObject.SetActive(false);

		}

		scoreUI.text = ScoreManager.score.ToString("D6");
	}
	IEnumerator Fade(Color from, Color to, float time)
	{
		float speed = 1 / time;
		float percent = 0;

		while (percent < 1)
		{
			percent += Time.deltaTime * speed;
			fadePlane.color = Color.Lerp(from, to, percent);
			yield return null;
		}
	}

    // UI
    public void StartNewGame(){
		SceneManager.LoadScene("GenerateMapScene");
    }
	public void returnMainMenu(){
		SceneManager.LoadScene("MenuScene");
	}
}
