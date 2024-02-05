using Unity.Collections;
using UnityEngine;
using Unity.Netcode;
using TMPro;

public class PlayerNetwork : NetworkBehaviour
{
    /// <summary>
    /// Test Script for Networking!!!!
    /// </summary>

    [SerializeField] private float speed;
    private TextMeshProUGUI messageText;
    private TMP_InputField inputField;
    private bool sentMessage;
    [SerializeField] private float position=5f;
    private NetworkVariable<MyCustomData> CustomData = new NetworkVariable<MyCustomData>(new MyCustomData
    {

    }, NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    private void Start()
    {
        //we find the input field in the scene
        inputField = FindObjectOfType<TMP_InputField>();
        //we get the text conmponent of the player
        messageText = GetComponentInChildren<TextMeshProUGUI>();
        messageText.text = "";

        speed = 5;

        //we set up the event listener from the code and we check if we are the owner 
        if (inputField != null && IsOwner)
        {
            inputField.onEndEdit.AddListener(OnInputFieldSubmit);
        }
    }
    public struct MyCustomData : INetworkSerializable
    {
        //we define a string of 128 characters so the message can be long 
        public FixedString128Bytes _message;

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            //we serialize the value of our message to send it to the network
            serializer.SerializeValue(ref _message);
        }
    }
    //when the object is spawned inside the network
    public override void OnNetworkSpawn()
    {

        transform.position = new Vector3(Random.Range(position, -position), 0, Random.Range(position, -position));
        //this is triggered when the value data is changed
        CustomData.OnValueChanged += (MyCustomData previousValue, MyCustomData newValue) =>
        {
            Debug.Log(OwnerClientId + ";" + newValue._message);

            //we update the message text from our custom data
            messageText.text = newValue._message.ToString();
        };
    }
    void Update()
    {
    

        if (!IsOwner) return;


        //get input axis 
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        //movement vector
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement.Normalize();

        //move the object
        transform.Translate(movement * speed * Time.deltaTime, Space.World);
        //if the message has been sent we call the make message dissapear rpc and clear the inputfield text
        if (sentMessage)
        {
            Invoke("MakeMessageDissapearRpc", 2f);
            Invoke("ClearInputFieldText", 0.5f);
            sentMessage = false;

        }
        
      
        //if we press the enter key then the message can be sent
        if (Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            string typedMessage = inputField.text;
            messageText.text = typedMessage;

            //we update the customdata with a new message
            CustomData.Value = new MyCustomData
            {

                _message = typedMessage
            };
        }
    }
    public void OnInputFieldSubmit(string typedMessage)
    {
        //we get the text from the input field
        typedMessage = inputField.text;
        //we set the message text to the message we typed in the input field
        messageText.text = typedMessage;
        CustomData.Value = new MyCustomData
        {

            _message = typedMessage
        };

        sentMessage = true;
    }
    //rpc call to make the message text dissapear 
    [Rpc(SendTo.Everyone, RequireOwnership = false)]
    private void MakeMessageDissapearRpc()
    {
        messageText.text = "";

    }
    //method that clears the input field text
    private void ClearInputFieldText()
    {
        inputField.text = "";
    }
}
