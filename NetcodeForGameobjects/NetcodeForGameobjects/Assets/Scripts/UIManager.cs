using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //our buttons and containers 
    [SerializeField] private Button serverButton;
    [SerializeField] private Button hostButton;
    [SerializeField] private Button clientButton;
    [SerializeField] GameObject uiContainer;
    [SerializeField] GameObject gameContainer;

    private void Awake()
    {
        gameContainer.SetActive(false);
        uiContainer.SetActive(true);
        //start client button functionallity
        serverButton.onClick.AddListener(()=>{
            NetworkManager.Singleton.StartServer();
            uiContainer.SetActive(false);
            gameContainer.SetActive(true);
            SoundManager.instance.PlayWelcomeAudioClip();
        });
        //start client button functionallity
        hostButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartHost();
           uiContainer.SetActive(false);
            gameContainer.SetActive(true);
            SoundManager.instance.PlayWelcomeAudioClip();
       });
        //start client button functionallity
        clientButton.onClick.AddListener(() => {
            NetworkManager.Singleton.StartClient();
            uiContainer.SetActive(false);
            gameContainer.SetActive(true);
            SoundManager.instance.PlayWelcomeAudioClip();
        });
    }
}
