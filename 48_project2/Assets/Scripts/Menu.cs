using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
	public GameObject mainMenuholder;
	public GameObject optionsMenuHolder;
    
    public GameObject helpMenuHolder;
	public Slider[] volumeSliders;

	public Toggle[] resolutionToggles;
	public int[] screenWidths;
	int activeScreenResIndex;

	void Start()
	{
		activeScreenResIndex = PlayerPrefs.GetInt("Screen Resolution index");
		bool isFullScreen = (PlayerPrefs.GetInt("Fullscreen") == 1) ? true : false;

		volumeSliders[0].value = AudioManager.instance.masterVolumePercent;
		volumeSliders[1].value = AudioManager.instance.musicVolumePercent;
		volumeSliders[2].value = AudioManager.instance.sfxVolumePercent;

        for (int i = 0; i < resolutionToggles.Length; i++)
        {
            resolutionToggles [i].isOn = i == activeScreenResIndex;
        }
   
    }
    public void Play(){
        SceneManager.LoadScene("GenerateMapScene");
    }
    public void Quit(){
        Application.Quit();
    }
    public void OptionsMenu(){
        mainMenuholder.SetActive(false);
        optionsMenuHolder.SetActive(true);
    }
    public void MainMenu(){
        mainMenuholder.SetActive(true);
        optionsMenuHolder.SetActive(false);
        helpMenuHolder.SetActive(false);
    }
    public void HelpMenu(){
        helpMenuHolder.SetActive(true);
        mainMenuholder.SetActive(false);
    }
    public void SetScreenResolution(int i){
        if(resolutionToggles[i].isOn){
            activeScreenResIndex = i;
            float aspectRatio = 16/9f;
            Screen.SetResolution(screenWidths[i],(int) (screenWidths[i]/aspectRatio) , false);
            PlayerPrefs.SetInt("Screen Resolution index",activeScreenResIndex);
            PlayerPrefs.Save();
        }
    }

    public void SetMasterVolume(float value){
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Master);
    }
    public void SetMusicVolume(float value){
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Music);
    }
    public void SetSFXVolume(float value){
        AudioManager.instance.SetVolume(value, AudioManager.AudioChannel.Sfx);
    }
}
