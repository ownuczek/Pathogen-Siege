using UnityEngine;
using UnityEngine.SceneManagement;  

public class SceneController : MonoBehaviour
{
  
    public void RestartGame()
    {
       
        PlayerStats.instance.ResetXP();
        
        
        
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

   
    public void LoadNextLevel()
    {
        
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        SceneManager.LoadScene(nextSceneIndex);
    }
}