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

        private async void Read()
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


        
//        StreamSocket socket;

//        /// <summary>
//        /// CONNECT TO SERVER
//        /// </summary>
//        /// <param name="host">Host name/IP address</param>
//        /// <param name="port">Port number</param>
//        /// <param name="message">Message to server</param>
//        /// <returns>Response from server</returns>
//        public async Task connect(string host, string port, string message)
//        {
//            HostName hostName;

//            using (socket = new StreamSocket())
//            {
//                hostName = new HostName(host);

//                // Set NoDelay to false so that the Nagle algorithm is not disabled
//                socket.Control.NoDelay = false;

//                try
//                {
//                    // Connect to the server
//                    await socket.ConnectAsync(hostName, port);
//                    // Send the message
//                    await this.send(message);
//                    // Read response
//                    await this.read();
//                }
//                catch (Exception exception)
//                {
//                    switch (Windows.Networking.Sockets.SocketError.GetStatus(exception.HResult))
//                    {
//                        case SocketErrorStatus.HostNotFound:
//                            // Handle HostNotFound Error
//                            throw;
//                        default:
//                            // If this is an unknown status it means that the error is fatal and retry will likely fail.
//                            throw;
//                    }
//                }
//            }
//        }

//        /// <summary>
//        /// SEND DATA
//        /// </summary>
//        /// <param name="message">Message to server</param>
//        /// <returns>void</returns>
//        public async Task send(string message)
//        {
//            DataWriter writer;

//            // Create the data writer object backed by the in-memory stream. 
//            using (writer = new DataWriter(socket.OutputStream))
//            {
//                // Set the Unicode character encoding for the output stream
//                writer.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
//                // Specify the byte order of a stream.
//                writer.ByteOrder = Windows.Storage.Streams.ByteOrder.LittleEndian;

//                // Gets the size of UTF-8 string.
//                writer.MeasureString(message);
//                // Write a string value to the output stream.
//                writer.WriteString(message);

//                // Send the contents of the writer to the backing stream.
//                try
//                {
//                    await writer.StoreAsync();
//                }
//                catch (Exception exception)
//                {
//                    switch (Windows.Networking.Sockets.SocketError.GetStatus(exception.HResult))
//                    {
//                        case SocketErrorStatus.HostNotFound:
//                            // Handle HostNotFound Error
//                            throw;
//                        default:
//                            // If this is an unknown status it means that the error is fatal and retry will likely fail.
//                            throw;
//                    }
//                }

//                await writer.FlushAsync();
//                // In order to prolong the lifetime of the stream, detach it from the DataWriter
//                writer.DetachStream();
//            }
//        }

//        /// <summary>
//        /// READ RESPONSE
//        /// </summary>
//        /// <returns>Response from server</returns>
//        public async Task<String> read()
//        {
//            DataReader reader;
//            StringBuilder strBuilder;
//            CancellationTokenSource cts = new CancellationTokenSource();
//            //set timeout here.
//            cts.CancelAfter(50);

//            using (reader = new DataReader(socket.InputStream))
//            {
//                strBuilder = new StringBuilder();

//                // Set the DataReader to only wait for available data (so that we don't have to know the data size)
//                reader.InputStreamOptions = Windows.Storage.Streams.InputStreamOptions.Partial;
//                // The encoding and byte order need to match the settings of the writer we previously used.
//                reader.UnicodeEncoding = Windows.Storage.Streams.UnicodeEncoding.Utf8;
//                reader.ByteOrder = Windows.Storage.Streams.ByteOrder.LittleEndian;

//                // Send the contents of the writer to the backing stream. 
//                // Get the size of the buffer that has not been read.
//                //await reader.LoadAsync(256);
                
//                await reader.LoadAsync(1000);

//                // Keep reading until we consume the complete stream.
//                while (reader.UnconsumedBufferLength > 0)
//                {
//                    Debug.WriteLine("Antes");
//                    Debug.WriteLine(reader.UnconsumedBufferLength);
//                    strBuilder.Append(reader.ReadString(reader.UnconsumedBufferLength));
//                    Debug.WriteLine(strBuilder);
//                    Debug.WriteLine(reader.UnconsumedBufferLength);
//                    //await reader.LoadAsync(256);
                    
//                    reader.LoadAsync(1000).;
//                    Debug.WriteLine("B");
//                }
                

//                /*
//                // Send the contents of the writer to the backing stream. 
//                // Get the size of the buffer that has not been read.
//                //await reader.LoadAsync(256);
//                uint numBytes = 0;
//                do
//                {
//                    try
//                    {
//                        numBytes = await reader.LoadAsync(1000);//. AsTask(cts.Token).Wait();
//                        Debug.WriteLine("Antes");
//                        Debug.WriteLine(reader.UnconsumedBufferLength);
//                        strBuilder.Append(reader.ReadString(reader.UnconsumedBufferLength));
//                        Debug.WriteLine(strBuilder);
//                        Debug.WriteLine(reader.UnconsumedBufferLength);
//                        Debug.WriteLine("B");
//                    }
//                    catch (System.Threading.Tasks.TaskCanceledException)
//                    {
//                        Debug.WriteLine("E");
//                        numBytes = 0;
//                        cts.Dispose();
//                    }
//                    // Keep reading until we consume the complete stream.
//                } while (numBytes > 0);
//                */



//        Debug.WriteLine("AQ2!!!");
//                reader.DetachStream();
//                Debug.WriteLine("A3Q!!!");
//                return strBuilder.ToString();
//            }
//        }
//        /*
//        private readonly string _ip;
//        private readonly int _port;
//        private StreamSocket _socket;
//        private DataWriter _writer;
//        private DataReader _reader;

//        public delegate void Error(string message);
//        public event Error OnError;

//        public delegate void DataRecived(string data);
//        public event DataRecived OnDataRecived;
        
//        public ConexaoTCP(string ip, int port)
//        {
//            _ip = ip;
//            _port = port;
//        }

//public async void Connect()
//        {
//            try
//            {
//                var hostName = new HostName(_ip);
//                _socket = new StreamSocket();
//                await _socket.ConnectAsync(hostName, _port.ToString());
//                _writer = new DataWriter(_socket.OutputStream);
//                _reader = new DataReader(_socket.InputStream);
//                Read();
//            }
//            catch (Exception ex)
//            {
//                if (OnError != null)
//                    OnError(ex.Message);
//            }
//        }
        
//public async void Send(string message)
//        {
//            //Envia o tamanho da string
//            _writer.WriteUInt32(_writer.MeasureString(message));
//            //Envia a string
//            _writer.WriteString(message);


//            try
//            {
//                //Faz o Envio da mensagem
//                await _writer.StoreAsync();
//                //Limpa para o proximo envio de mensagem
//                await _writer.FlushAsync();
//            }
//            catch (Exception ex)
//            {
//                if (OnError != null)
//                    OnError(ex.Message);
//            }
//        }

//        public async void Read()
//        {
//            try
//            {
//                while (true)
//                {
//                    // Read first 4 bytes (length of the subsequent string).
//                    //uint sizeFieldCount = await _reader.LoadAsync(sizeof(uint));
//                    //if (sizeFieldCount != sizeof(uint))
//                    //{
//                    //    // The underlying socket was closed before we were able to read the whole data.
//                    //    return;
//                    //}

//                    // Read the string.
//                    uint stringLength = _reader.ReadUInt32();
//                    uint actualStringLength = await _reader.LoadAsync(stringLength);
//                    //if (stringLength != actualStringLength)
//                    //{
//                        // The underlying socket was closed before we were able to read the whole data.
//                    //    return;
//                    //}
//                    if (stringLength > 0)
//                    // Display the string on the screen. The event is invoked on a non-UI thread, so we need to marshal
//                    // the text back to the UI thread.
//                        NotifyUserFromAsyncThread(
//                            String.Format("Received data: \"{0}\"", _reader.ReadString(actualStringLength)),
//                            NotifyType.StatusMessage);
//                }
//            }
//            catch (Exception exception)
//            {
//                // If this is an unknown status it means that the error is fatal and retry will likely fail.
//                if (Windows.Networking.Sockets.SocketError.GetStatus(exception.HResult) == SocketErrorStatus.Unknown)
//                {
//                    throw;
//                }

//                NotifyUserFromAsyncThread(
//                    "Read stream failed with error: " + exception.Message,
//                    NotifyType.ErrorMessage);
//            }
//        }
//        public enum NotifyType
//        {
//            StatusMessage,
//            ErrorMessage
//        };
//        private void NotifyUserFromAsyncThread(string strMessage, NotifyType type)
//        {
//            MessageDialog msgbox = new MessageDialog(strMessage);
//        }

//        public void Close()
//        {
//            _writer.DetachStream();
//            _writer.Dispose();

//            _reader.DetachStream();
//            _reader.Dispose();
//            _socket.Dispose();
//        }
//        */


//        /*
//        private TcpClient tcp;
//        private NetworkStream stream;
//        private bool connected;
//        private Thread receiveThread;
//        private List<string> listaHex;

//        public ConexaoTCP()
//        {
//            connected = false;
//            tcp = null;
//            stream = null;
//            receiveThread = null;
//            listaHex = null;
//        }

//        public static byte[] ToByteArray(String hexString)
//        {
//            byte[] retval = new byte[hexString.Length / 2];
//            for (int i = 0; i < hexString.Length; i += 2)
//                retval[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
//            return retval;
//        }

//        public bool Disconnect()
//        {
//            if (connected)
//            {
//                if (tcp != null)
//                {
//                    tcp.Close();
//                }
//                connected = false;
//            }
//            return connected;
//        }

//        public bool Connect(string aIp, string aPorta)
//        {
//            if (connected == false)
//            {
//                int porta = Convert.ToInt32(aPorta);

//                string ip = aIp.Replace(" ", "");

//                IPAddress address = IPAddress.Parse(ip);
                
//                tcp = new TcpClient(ip, porta);
//                stream = tcp.GetStream();

//                receiveThread = new Thread(DoWork);
//                receiveThread.Start();

//                string texto = "HERE\r";

//                byte[] dataToSend = Encoding.ASCII.GetBytes(texto);

//                // Send the message to the connected TcpServer. 
//                stream.Write(dataToSend, 0, dataToSend.Length);
                
//                connected = true;
//            }
//            return connected;
//        }

//        // This is run as a thread
//        public void DoWork()
//        {
//            byte[] bytes = new byte[1024];
//            while ((tcp.Connected) && (stream != null))
//            {
//                if (stream.DataAvailable)
//                {
//                    int bytesRead = stream.Read(bytes, 0, bytes.Length);
//                    SetText(Encoding.ASCII.GetString(bytes, 0, bytesRead));
//                }
//                Thread.Sleep(5);
//                //MessageBox.Show(Encoding.ASCII.GetString(bytes, 0, bytesRead));
//            }
//        }
//        */
    }
}
