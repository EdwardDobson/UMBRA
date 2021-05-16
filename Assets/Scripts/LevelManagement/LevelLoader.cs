using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;
public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    int m_levelIndex;
    IEnumerator LoadLevelAsync(int _index)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_index);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
    public void LoadLevel(int _index)
    {
        StartCoroutine(LoadLevelAsync(_index));
        RoomManager.Instance.respawnPos = Vector3.zero;
    }
    //void OnTriggerEnter(Collider _collider)
    //{
    //  if(_collider.gameObject.tag.Contains("Player"))
    //    LoadLevel(m_levelIndex);
    //}
    public void RestartLevel()
    {
        //StartCoroutine(LoadLevelAsync(SceneManager.GetActiveScene().buildIndex));
        GameObject.Find("StoryManager").GetComponent<StoryManager>().FinishedStories.Clear();
        Debug.Log(GameObject.Find("StoryManager").GetComponent<StoryManager>().FinishedStories.Count);
        GameoverManager.Instance.go.SetActive(false);
        RoomManager.Instance.respawnPos = Vector3.zero;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        AudioManager.Instance.RestartMusic();
    }

    public void RestartRoom()
    {
        GameoverManager.Instance.go.SetActive(false);
        RoomManager.Instance.respawnPos = RoomManager.Instance.currentRoom.spawnPos;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        Time.timeScale = 1;
        AudioManager.Instance.RestartMusic();
    }
}
