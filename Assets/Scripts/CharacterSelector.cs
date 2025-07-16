using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelector : MonoBehaviour
{
    public void SelectFox()
    {
        PlayerPrefs.SetString("SelectedCharacter", "Fox");
        SceneManager.LoadScene("SampleScene");
    }

    public void SelectBird()
    {
        PlayerPrefs.SetString("SelectedCharacter", "Bird");
        SceneManager.LoadScene("SampleScene");
    }
}
