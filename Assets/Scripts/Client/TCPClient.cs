using Newtonsoft.Json;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TCPClient : MonoBehaviour, ISubscriber
{
    [SerializeField] TextMeshProUGUI clientLog;
    [SerializeField] TMP_InputField ipInput;

    public string IP = "192.168.1.217";
    public int Port = 5000;

    private TcpClient socketConnection;
    private Thread clientReceiveThread;

    private string _address => $"{IP}:{Port}";  // ===> 192.168.1.217:5000
    private bool _connected = false;
    private string _messageOnScreen;
    private bool _toUpdate = false;

    CharacterModel _modelReceived;
    private bool _sendCharacterWithPublisher = false;

    private void Awake()
    {
        Publisher.Subscribe(this, typeof(CharacterBornMessage));
        ipInput.onValueChanged.AddListener((val) => { IP = val.ToString(); ConnectToTcpServer(); });
    }

    void Start()
    {
        ConnectToTcpServer();
    }

    private void Update()
    {
        if (_toUpdate)
        {
            _toUpdate = false;
            clientLog.text = _messageOnScreen;
        }
        
        if (_sendCharacterWithPublisher && _modelReceived != null)
        {
            Publisher.Publish(new CharacterReceivedMessage(_modelReceived));
            _sendCharacterWithPublisher = false;
            _modelReceived = null;
        }
    }

    private void ConnectToTcpServer()
    {
        try
        {
            clientReceiveThread = new Thread(new ThreadStart(ListenForData));
            clientReceiveThread.IsBackground = true;
            clientReceiveThread.Start();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            InterruptClient();
        }
    }

    // funzione che invia il modello al server
    private void SendMessageToServer(CharacterModel model)
    {
        string jsonString = JsonConvert.SerializeObject(model);

        if (string.IsNullOrEmpty(jsonString))
        {
            Debug.LogError("Errore nella conversione del JSON");
            return;
        }

        if (socketConnection == null || !socketConnection.Connected)
        {
            // TODO: gestire la riconnessione => _retryConnection
            return;
        }

        try
        {
            // AGGIUNTO DELIMITATORE PER LEGGERLO DA SERVER COME FINE DEL MESSAGGIO
            jsonString += "\n";
            byte[] jsonBytes = Encoding.UTF8.GetBytes(jsonString);

            NetworkStream stream = socketConnection.GetStream();
            if (stream.CanWrite)
            {
                stream.Write(jsonBytes, 0, jsonBytes.Length);
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    private void ListenForData()
    {
        try
        {
            ClientLog($"Tentativo di connesisone a {_address} in corso...");

            if (socketConnection == null || !socketConnection.Connected)
            {
                socketConnection = new TcpClient(IP, Port);

                byte[] bytes = new byte[4096];

                while (socketConnection.Connected)
                {
                    // ottiene lo stream di oggetti da leggere
                    using (NetworkStream stream = socketConnection.GetStream())
                    {
                        // leggi lo stream in arrivo da convertire in byte array
                        int length;
                        while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            byte[] incomingData = new byte[length];
                            Array.Copy(bytes, 0, incomingData, 0, length);

                            string jsonCharaterModel = Encoding.UTF8.GetString(incomingData);

                            CharacterModel characterModel = JsonConvert.DeserializeObject<CharacterModel>(jsonCharaterModel);
                            
                            _modelReceived = characterModel;
                            _sendCharacterWithPublisher = true;
                        }
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            InterruptClient();
            InterruptSocket();
        }
    }

    private void InterruptClient()
    {
        if (clientReceiveThread != null)
        {
            clientReceiveThread.Interrupt();
            clientReceiveThread = null;
        }
    }

    private void InterruptSocket()
    {
        if (socketConnection != null)
        {
            socketConnection.Dispose();
            socketConnection = null;
        }
    }

    private void ClientLog(string log)
    {
        if (_messageOnScreen != log)
        {
            _messageOnScreen = log;
            _toUpdate = true;
        }
    }

    public void OnPublish(IPublisherMessage message)
    {
        if (message is CharacterBornMessage bornMessage)
        {
            SendMessageToServer(bornMessage.Model);
        }
    }

    public void OnDisableSubscriber()
    {
        Publisher.Unsubscribe(this, typeof(CharacterBornMessage));
    }

    private void OnDisable()
    {
        OnDisableSubscriber();
    }
}