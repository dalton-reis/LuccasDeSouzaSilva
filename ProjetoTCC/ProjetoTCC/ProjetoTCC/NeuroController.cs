using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace ProjetoTCC
{
    public class NeuroController
    {
        private string Port;
        private NativeThinkgear thinkgear = new NativeThinkgear();
        private int ConnectionID;

        private bool IsPreReadData;
        private bool IsTestConnection;

        public NeuroController()
        {
            Port = "";
            IsPreReadData = false;
            IsTestConnection = false;
        }

        public bool testConnection()
        {
            string[] ports = SerialPort.GetPortNames();
            string connectionPort = "";
            for (int i = 0; i < ports.Length; i++)
            {
                if (testPortConnection(ports[i]))
                {
                    connectionPort = ports[i];
                    Console.WriteLine("Conexão encontrada na porta " + connectionPort + "!");
                    break;
                }
            }

            if (connectionPort == "")
            {
                return false;
            }
            else
            {
                Port = connectionPort;
                IsTestConnection = true;
                return true;
            }
        }

        private bool testPortConnection(string port)
        {
            int connectionID = NativeThinkgear.TG_GetNewConnectionId();

            if (connectionID < 0)
            {
                Debug.WriteLine("ERROR: TG_GetNewConnectionId() returned: " + connectionID + " for port: " + port);
                return false;
            }
            
            int errCode = 0;
            /* Set/open stream (raw bytes) log file for connection */
            errCode = NativeThinkgear.TG_SetStreamLog(connectionID, "streamLog.txt");
            if (errCode < 0)
            {
                Debug.WriteLine("ERROR: TG_SetStreamLog() returned: " + errCode + " for port: " + port);
                closeCurrentConnection();
                return false;
            }

            /* Set/open data (ThinkGear values) log file for connection */
            errCode = NativeThinkgear.TG_SetDataLog(connectionID, "dataLog.txt");
            if (errCode < 0)
            {
                Debug.WriteLine("ERROR: TG_SetDataLog() returned: " + errCode + " for port: " + port);
                closeCurrentConnection();
                return false;
            }

            string comPortName = "\\\\.\\" + port;
            NativeThinkgear.Baudrate baudRate = NativeThinkgear.Baudrate.TG_BAUD_57600;
            NativeThinkgear.SerialDataFormat dataFormat = NativeThinkgear.SerialDataFormat.TG_STREAM_PACKETS;

            errCode = NativeThinkgear.TG_Connect(connectionID,
                          comPortName,
                          baudRate,
                          dataFormat);
            if (errCode < 0)
            {
                StringBuilder str = new StringBuilder("ERROR TG_Connect(): ");
                switch (errCode)
                {
                    case -1:
                        str.Append("-1 -> connectionID(").Append(connectionID).Append(") does not refer to a valid ThinkGear Connection ID handle");
                        break;
                    case -2:
                        str.Append("-2 -> comPortName(").Append(comPortName).Append(") could not be opened as a serial communication port for any reason");
                        break;
                    case -3:
                        str.Append("-3 -> Baudrate(").Append(baudRate).Append(") is not a valid TG_BAUD_ value");
                        break;
                    case -4:
                        str.Append("-4 -> SerialDataFormat(").Append(dataFormat).Append(") is not a valid TG_STREAM_ value");
                        break;
                    default:
                        str.Append(errCode).Append(" -> not an expected errCode, unknown error");
                        break;
                }
                str.Append(" -> in port: ").Append(port);
                Debug.WriteLine(str.ToString());
                closeCurrentConnection();
                return false;
            }

            int packets = 0;

            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (sw.ElapsedMilliseconds < 5000)
            {
                errCode = NativeThinkgear.TG_ReadPackets(connectionID, 1);
                if (errCode == 1)
                {
                    int[] values = new int[11] {0,0,0,0,0,0,0,0,0,0,0};

                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_POOR_SIGNAL) != 0)
                    {
                        values[0] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_POOR_SIGNAL);
                    }

                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_ATTENTION) != 0)
                    {
                        values[1] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_ATTENTION);
                    }

                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_MEDITATION) != 0)
                    {
                        values[2] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_MEDITATION);
                    }

                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_ALPHA1) != 0)
                    {
                        values[3] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_ALPHA1);
                    }

                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_ALPHA2) != 0)
                    {
                        values[4] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_ALPHA2);
                    }

                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_BETA1) != 0)
                    {
                        values[5] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_BETA1);
                    }

                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_BETA2) != 0)
                    {
                        values[6] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_BETA2);
                    }

                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_DELTA) != 0)
                    {
                        values[7] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_DELTA);
                    }

                    //read Gamma1
                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_GAMMA1) != 0)
                    {
                        values[8] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_GAMMA1);
                    }

                    //read Gamma2
                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_GAMMA2) != 0)
                    {
                        values[9] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_GAMMA2);
                    }

                    //read Theta
                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_THETA) != 0)
                    {
                        values[10] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_THETA);
                    }
                    
                    packets += 1;                    
                }
                else
                {
                    Thread.Sleep(1);
                }
            }
            sw.Stop();

            closeCurrentConnection();

            if (packets > 0)
            {
                return true;
            } else
            {
                Debug.WriteLine("ERROR: Connection established but no packet returned for port: " + comPortName);
                return false;
            }
        }

        public bool preReadData()
        {
            ConnectionID = NativeThinkgear.TG_GetNewConnectionId();

            if (ConnectionID < 0)
            {
                Debug.WriteLine("ERROR: TG_GetNewConnectionId() returned: " + ConnectionID);
                //escritor.WriteLine("ERROR: TG_GetNewConnectionId() returned: " + ConnectionID);
                return false;
            }

            string comPortName = "\\\\.\\" + Port;
            int errCode = NativeThinkgear.TG_Connect(ConnectionID,
                          comPortName,
                          NativeThinkgear.Baudrate.TG_BAUD_57600,
                          NativeThinkgear.SerialDataFormat.TG_STREAM_PACKETS);
            if (errCode < 0)
            {
                Debug.WriteLine("ERROR: TG_Connect() returned: " + errCode);
                //escritor.WriteLine("ERROR: TG_Connect() returned: " + errCode);
                return false;
            }

            IsPreReadData = true;
            return true;
        }

        public NeuroData readData(bool[] toReadData)
        {
            if (!IsTestConnection)
            {
                testConnection();
            }

            if (!IsPreReadData)
            {
                preReadData();
            }

            int[] values = new int[11] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            /* Attempt to read a Packet of data from the connection, 1 Packet at a time */
            int errCode = NativeThinkgear.TG_ReadPackets(ConnectionID, 1);
            /* If TG_ReadPackets() was able to read a complete Packet of data... */
            if (errCode == 1)
            {
                //read PoorSignal
                if (toReadData[0] && NativeThinkgear.TG_GetValueStatus(ConnectionID, NativeThinkgear.DataType.TG_DATA_POOR_SIGNAL) != 0)
                {
                    values[0] = (int)NativeThinkgear.TG_GetValue(ConnectionID, NativeThinkgear.DataType.TG_DATA_POOR_SIGNAL);
                }

                //read Attention
                if (toReadData[1] && NativeThinkgear.TG_GetValueStatus(ConnectionID, NativeThinkgear.DataType.TG_DATA_ATTENTION) != 0)
                {
                    values[1] = (int)NativeThinkgear.TG_GetValue(ConnectionID, NativeThinkgear.DataType.TG_DATA_ATTENTION);
                }

                //read Meditation
                if (toReadData[2] && NativeThinkgear.TG_GetValueStatus(ConnectionID, NativeThinkgear.DataType.TG_DATA_MEDITATION) != 0)
                {
                    values[2] = (int)NativeThinkgear.TG_GetValue(ConnectionID, NativeThinkgear.DataType.TG_DATA_MEDITATION);
                }

                //read Alpha1
                if (toReadData[3] && NativeThinkgear.TG_GetValueStatus(ConnectionID, NativeThinkgear.DataType.TG_DATA_ALPHA1) != 0)
                {
                    values[3] = (int)NativeThinkgear.TG_GetValue(ConnectionID, NativeThinkgear.DataType.TG_DATA_ALPHA1);
                }

                //read Alpha2
                if (toReadData[4] && NativeThinkgear.TG_GetValueStatus(ConnectionID, NativeThinkgear.DataType.TG_DATA_ALPHA2) != 0)
                {
                    values[4] = (int)NativeThinkgear.TG_GetValue(ConnectionID, NativeThinkgear.DataType.TG_DATA_ALPHA2);
                }

                //read Beta1
                if (toReadData[5] && NativeThinkgear.TG_GetValueStatus(ConnectionID, NativeThinkgear.DataType.TG_DATA_BETA1) != 0)
                {
                    values[5] = (int)NativeThinkgear.TG_GetValue(ConnectionID, NativeThinkgear.DataType.TG_DATA_BETA1);
                }

                //read Beta2
                if (toReadData[6] && NativeThinkgear.TG_GetValueStatus(ConnectionID, NativeThinkgear.DataType.TG_DATA_BETA2) != 0)
                {
                    values[6] = (int)NativeThinkgear.TG_GetValue(ConnectionID, NativeThinkgear.DataType.TG_DATA_BETA2);
                }

                //read Delta
                if (toReadData[7] && NativeThinkgear.TG_GetValueStatus(ConnectionID, NativeThinkgear.DataType.TG_DATA_DELTA) != 0)
                {
                    values[7] = (int)NativeThinkgear.TG_GetValue(ConnectionID, NativeThinkgear.DataType.TG_DATA_DELTA);
                }

                //read Gamma1
                if (toReadData[8] && NativeThinkgear.TG_GetValueStatus(ConnectionID, NativeThinkgear.DataType.TG_DATA_GAMMA1) != 0)
                {
                    values[8] = (int)NativeThinkgear.TG_GetValue(ConnectionID, NativeThinkgear.DataType.TG_DATA_GAMMA1);
                }

                //read Gamma2
                if (toReadData[9] && NativeThinkgear.TG_GetValueStatus(ConnectionID, NativeThinkgear.DataType.TG_DATA_GAMMA2) != 0)
                {
                    values[9] = (int)NativeThinkgear.TG_GetValue(ConnectionID, NativeThinkgear.DataType.TG_DATA_GAMMA2);
                }

                //read Theta
                if (toReadData[10] && NativeThinkgear.TG_GetValueStatus(ConnectionID, NativeThinkgear.DataType.TG_DATA_THETA) != 0)
                {
                    values[10] = (int)NativeThinkgear.TG_GetValue(ConnectionID, NativeThinkgear.DataType.TG_DATA_THETA);
                }                             
            }

            bool hasData = false;   

            foreach (int i in values)
            {
                if (i > 0)
                {
                    hasData = true;
                    break;
                }
            }

            NeuroData nr = null;

            if (hasData)
            {
                nr = new NeuroData(values[0], values[1], values[2],
                        values[3], values[4], values[5],
                        values[6], values[7], values[8],
                        values[9], values[10]);
            }

            if (errCode == -1)
            {
                Debug.WriteLine(DateTime.UtcNow + " TG_ReadPackets returned: " + errCode + ". Invalid Connection ID");
            }
            else if (errCode == -2)
            {
                Debug.WriteLine(DateTime.UtcNow + " TG_ReadPackets returned: " + errCode + ". No data available to read");
                nr = new NeuroData(values[0], values[1], values[2],
                        values[3], values[4], values[5],
                        values[6], values[7], values[8],
                        values[9], values[10]);
            }
            else if (errCode == -3)
            {
                Debug.WriteLine(DateTime.UtcNow + " TG_ReadPackets returned: " + errCode + ". IO Error");
            }            

            return nr;
        }

        public void closeReader()
        {
            IsPreReadData = false;
            IsTestConnection = false;

            closeCurrentConnection();
        }

        public void closeCurrentConnection()
        {
            NativeThinkgear.TG_Disconnect(ConnectionID);
            NativeThinkgear.TG_FreeConnection(ConnectionID);            
        }
    }
}
