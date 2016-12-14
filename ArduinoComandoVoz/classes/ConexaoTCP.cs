using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Networking;
using System.Diagnostics;
using System.Threading;
using System.Net.Sockets;
using System.Net;
using Windows.UI.Xaml.Controls;

namespace ArduinoComandoVoz
{
    class ConexaoTCP
    {
        //MainPage _listener = null;
        Controle _listener = null;        
        Socket _socket = null;

        static ManualResetEvent _clientDone = new ManualResetEvent(false);

        // Define a timeout in milliseconds for each asynchronous call. If a response is not received within this 
        // timeout period, the call is aborted.
        const int TIMEOUT_MILLISECONDS = 5000;

        // The maximum size of the data buffer to use with the asynchronous socket methods
        const int MAX_BUFFER_SIZE = 2048;


        private readonly string _ip;
        private readonly int _port;


        public delegate void Error(string message);
        public event Error OnError;

        public delegate void DataRecived(string data);
        public event DataRecived OnDataRecived;

        public string Ip { get { return _ip; } }
        public int Port { get { return _port; } }

        //public ConexaoTCP(MainPage listener, string ip, int port)
        public ConexaoTCP(Controle listener, string ip, int port)
        {
            _listener = listener;
            _ip = ip;
            _port = port;            
        }

        public ConexaoTCP(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public async void Connect()
        {
            try
            {
                string result = string.Empty;

                // Create DnsEndPoint. The hostName and port are passed in to this method.
                DnsEndPoint hostEntry = new DnsEndPoint(_ip, _port);

                // Create a stream-based, TCP socket using the InterNetwork Address Family. 
                _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Create a SocketAsyncEventArgs object to be used in the connection request
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = hostEntry;

                // Inline event handler for the Completed event.
                // Note: This event handler was implemented inline in order to make this method self-contained.
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate (object s, SocketAsyncEventArgs e)
                {
                    // Retrieve the result of this request
                    result = e.SocketError.ToString();

                    // Signal that the request is complete, unblocking the UI thread
                    _clientDone.Set();
                });

                // Sets the state of the event to nonsignaled, causing threads to block
                _clientDone.Reset();

                // Make an asynchronous Connect request over the socket
                _socket.ConnectAsync(socketEventArg);

                // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
                // If no response comes back within this time then proceed
                _clientDone.WaitOne(TIMEOUT_MILLISECONDS);


            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(ex.Message);
            }
        }

        public String Send(string data)
        {
            string response = "Operation Timeout";

            // We are re-using the _socket object initialized in the Connect method
            if (_socket != null)
            {
                // Create SocketAsyncEventArgs context object
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();

                // Set properties on context object
                socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;
                socketEventArg.UserToken = null;

                // Inline event handler for the Completed event.
                // Note: This event handler was implemented inline in order 
                // to make this method self-contained.
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate (object s, SocketAsyncEventArgs e)
                {
                    response = e.SocketError.ToString();

                    // Unblock the UI thread
                    _clientDone.Set();
                });

                // Add the data to be sent into the buffer
                byte[] payload = Encoding.UTF8.GetBytes(data);
                socketEventArg.SetBuffer(payload, 0, payload.Length);

                // Sets the state of the event to nonsignaled, causing threads to block
                _clientDone.Reset();

                // Make an asynchronous Send request over the socket
                _socket.SendAsync(socketEventArg);

                // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
                // If no response comes back within this time then proceed
                _clientDone.WaitOne(TIMEOUT_MILLISECONDS);
            }
            else
            {
                response = "Socket is not initialized";
            }

            return response;
        }

        public async void Read()
        {
            try
            {
                while (true)
                {
                    string response = Receive();
                }

            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(ex.Message);
            }

        }

        public string Receive()
        {
            string response = "Operation Timeout";

            // We are receiving over an established socket connection
            if (_socket != null)
            {
                // Create SocketAsyncEventArgs context object
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
                socketEventArg.RemoteEndPoint = _socket.RemoteEndPoint;

                // Setup the buffer to receive the data
                socketEventArg.SetBuffer(new Byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);

                // Inline event handler for the Completed event.
                // Note: This even handler was implemented inline in order to make 
                // this method self-contained.
                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate (object s, SocketAsyncEventArgs e)
                {
                    if (e.SocketError == System.Net.Sockets.SocketError.Success)
                    {
                        // Retrieve the data from the buffer
                        response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                        response = response.Trim('\0');

                        Debug.WriteLine("Receive " + response);
                        if (_listener != null)
                        {
                            _listener.receiveCallback(response);
                        }
                    }
                    else
                    {
                        response = e.SocketError.ToString();
                    }

                    _clientDone.Set();
                });

                // Sets the state of the event to nonsignaled, causing threads to block
                _clientDone.Reset();

                // Make an asynchronous Receive request over the socket
                _socket.ReceiveAsync(socketEventArg);

                // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
                // If no response comes back within this time then proceed
                _clientDone.WaitOne(TIMEOUT_MILLISECONDS);
            }
            else
            {
                response = "Socket is not initialized";
            }

            return response;
        }

        public void Close()
        {
            if (_socket != null)
            {
                _socket.Dispose();
            }
        }

    }
}


/*
using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.Sockets;
using Windows.Storage.Streams;
using Windows.Networking;
using System.Diagnostics;
using System.Threading;

namespace ArduinoComandoVoz
{
    class ConexaoTCP
    {
        private readonly string _ip;
        private readonly int _port;
        private StreamSocket _socket;
        private DataWriter _writer;
        private DataReader _reader;

        public delegate void Error(string message);
        public event Error OnError;

        public delegate void DataRecived(string data);
        public event DataRecived OnDataRecived;

        public string Ip { get { return _ip; } }
        public int Port { get { return _port; } }

        public ConexaoTCP(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }

        public async void Connect()
        {
            try
            {
                var hostName = new HostName(Ip);
                _socket = new StreamSocket();
                await _socket.ConnectAsync(hostName, Port.ToString());
                _writer = new DataWriter(_socket.OutputStream);

                _reader = new DataReader(_socket.InputStream);
                Read();
            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(ex.Message);
            }
        }

        public async void Send(string message)
        {
            //Envia o tamanho da string
            _writer.WriteUInt32(_writer.MeasureString(message));
            //Envia a string em si
            _writer.WriteString(message);

            try
            {
                //Faz o Envio da mensagem
                await _writer.StoreAsync();
                //Limpa para o proximo envio de mensagem
                await _writer.FlushAsync();
            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(ex.Message);
            }
        }

        public async void Read()
        {
            //_reader = new DataReader(_socket.InputStream);
            try
            {
                while (true)
                {
                    uint sizeFieldCount = await _reader.LoadAsync(sizeof(uint));
                    //if desconneted
                    if (sizeFieldCount != sizeof(uint))
                        return;

                    uint stringLenght = _reader.ReadUInt32();
                    uint actualStringLength = await _reader.LoadAsync(stringLenght);
                    //if desconneted
                    if (stringLenght != actualStringLength)
                        return;
                    if (OnDataRecived != null)
                        OnDataRecived(_reader.ReadString(actualStringLength));
                }

            }
            catch (Exception ex)
            {
                if (OnError != null)
                    OnError(ex.Message);
            }
        }

        public void Close()
        {
            try
            {
                if (_writer != null)
                {
                    _writer.DetachStream();
                    _writer.Dispose();
                }

                if (_reader != null)
                {
                    _reader.DetachBuffer();
                    _reader.DetachStream(); //Causando exceção
                    _reader.Dispose();
                }

                if (_socket != null)
                    _socket.Dispose();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }
    }
}

*/
