using libStreamSDK;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace thinkgear_testapp_csharp
{
    class Program_Test
    {

        public static bool testPortConnection(string port, StreamWriter escritor)
        {

            Console.WriteLine("Inicio Teste Porta: " + port);
            escritor.WriteLine("Inicio Teste Porta: " + port);

            NativeThinkgear thinkgear = new NativeThinkgear();
            int connectionID = NativeThinkgear.TG_GetNewConnectionId();

            if (connectionID < 0)
            {
                Console.WriteLine("ERROR: TG_GetNewConnectionId() returned: " + connectionID);
                escritor.WriteLine("ERROR: TG_GetNewConnectionId() returned: " + connectionID);
                return false;
            }

            int errCode = 0;
            /* Set/open stream (raw bytes) log file for connection */
            errCode = NativeThinkgear.TG_SetStreamLog(connectionID, "streamLog.txt");
            escritor.WriteLine("errCode for TG_SetStreamLog : " + errCode);
            if (errCode < 0)
            {
                Console.WriteLine("ERROR: TG_SetStreamLog() returned: " + errCode);
                escritor.WriteLine("ERROR: TG_SetStreamLog() returned: " + errCode);
                return false;
            }

            /* Set/open data (ThinkGear values) log file for connection */
            errCode = NativeThinkgear.TG_SetDataLog(connectionID, "dataLog.txt");
            escritor.WriteLine("errCode for TG_SetDataLog : " + errCode);
            if (errCode < 0)
            {
                Console.WriteLine("ERROR: TG_SetDataLog() returned: " + errCode);
                escritor.WriteLine("ERROR: TG_SetDataLog() returned: " + errCode);
                return false;
            }

            /* Attempt to connect the connection ID handle to serial port "COM5" */
            string comPortName = "\\\\.\\COM8";
            //Step 1
            errCode = NativeThinkgear.TG_Connect(connectionID,
                          comPortName,
                          NativeThinkgear.Baudrate.TG_BAUD_57600,
                          NativeThinkgear.SerialDataFormat.TG_STREAM_PACKETS);
            if (errCode < 0)
            {
                Console.WriteLine("ERROR: TG_Connect() returned: " + errCode);
                escritor.WriteLine("ERROR: TG_Connect() returned: " + errCode);
                return false;
            }

            int packets = 0;
            while (packets < 20)
            {
                /* Attempt to read a Packet of data from the connection, 1 Packet at a time */
                errCode = NativeThinkgear.TG_ReadPackets(connectionID, 1);
                /* If TG_ReadPackets() was able to read a complete Packet of data... */
                if (errCode == 1)
                {
                    //escritor.WriteLine("TG_ReadPackets returned: " + errCode + ". Success!");
                    bool tst = false;
                    int[] values = new int[3];

                    /* If ATTENTION value has been updated by TG_ReadPackets()... */
                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_ATTENTION) != 0)
                    {
                        /* Get and print out the updated DATA_RAW value */
                        values[0] = (int) NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_ATTENTION);
                        tst = true;
                        //escritor.WriteLine("Type 1 - New RAW value: : " + (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_RAW));
                    } /* end "If attention value has been updated..." */

                    /* If MEDITATION value has been updated by TG_ReadPackets()... */
                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_MEDITATION) != 0)
                    {
                        /* Get and print out the updated DATA_RAW value */
                        values[1] = (short)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_MEDITATION);
                        tst = true;
                        //escritor.WriteLine("Type 1 - New RAW value: : " + (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_RAW));
                    } /* end "If attention value has been updated..." */

                    /* If POOR SIGNAL value has been updated by TG_ReadPackets()... */
                    if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_POOR_SIGNAL) != 0)
                    {
                        /* Get and print out the updated DATA_RAW value */
                        values[2] = (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_POOR_SIGNAL);
                        tst = true;
                        //escritor.WriteLine("Type 1 - New RAW value: : " + (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_RAW));
                    } /* end "If attention value has been updated..." */
                    if (tst)
                    {
                        packets += 1;
                        escritor.WriteLine(DateTime.Now + " - TG_ReadPackets Attention: " + values[0]
                            + "  Meditation: " + values[1]
                            + "  Poor Signal: " + values[2]);
                    }

                } else
                {
//                    escritor.WriteLine(DateTime.Now + " - TG_ReadPackets errCode " + errCode);
                    Thread.Sleep(1);
                }
            }

            ///* Read 10 ThinkGear Packets from the connection, 1 Packet at a time */
            //int packetsRead = 0;
            //while (packetsRead < 20)
            //{

            //    /* Attempt to read a Packet of data from the connection */
            //    errCode = NativeThinkgear.TG_ReadPackets(connectionID, 1);
            //    /* If TG_ReadPackets() was able to read a complete Packet of data... */
            //    if (errCode == 1)
            //    {
            //        escritor.WriteLine("Type 1 - TG_ReadPackets returned: " + errCode + ". Success!");
            //        packetsRead++;

            //        /* If attention value has been updated by TG_ReadPackets()... */
            //        if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_RAW) != 0)
            //        {

            //            /* Get and print out the updated attention value */
            //            escritor.WriteLine("Type 1 - New RAW value: : " + (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_RAW));

            //        } /* end "If attention value has been updated..." */

            //    } /* end "If a Packet of data was read..." */
            //    if (errCode == -1)
            //    {
            //        escritor.WriteLine("Type 1 - TG_ReadPackets returned: " + errCode + ". Invalid Connection ID");
            //    }
            //    else if (errCode == -2)
            //    {
            //        escritor.WriteLine("Type 1 - TG_ReadPackets returned: " + errCode + ". No data available to read");
            //        int sleep = 200;
            //        Thread.Sleep(sleep);
            //        escritor.WriteLine("Sleep " + sleep.ToString());
            //    }
            //    else if (errCode == -3)
            //    {
            //        escritor.WriteLine("Type 1 - TG_ReadPackets returned: " + errCode + ". IO Error");
            //    }
            //} /* end "Read 10 Packets of data from connection..." */

            //escritor.WriteLine("auto read test begin:");

            //errCode = NativeThinkgear.TG_EnableAutoRead(connectionID, 1);
            //if (errCode == 0)
            //{
            //    packetsRead = 0;
            //    NativeThinkgear.MWM15_setFilterType(connectionID, NativeThinkgear.FilterType.MWM15_FILTER_TYPE_50HZ);
            //    while (packetsRead < 2000)
            //    {
            //        /* If raw value has been updated ... */
            //        if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.TG_DATA_RAW) != 0)
            //        {

            //            /* Get and print out the updated raw value */
            //            if (packetsRead % 10 == 0)
            //            {
            //                escritor.WriteLine("Type 2 - New RAW value: : " + (int)NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_RAW));
            //            }
            //            else
            //            {
            //                NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.TG_DATA_RAW);
            //            }
            //            packetsRead++;

            //            if (packetsRead == 800 || packetsRead == 1600)
            //            {
            //                NativeThinkgear.MWM15_getFilterType(connectionID);
            //                escritor.WriteLine(" MWM15_getFilterType called");
            //            }

            //        }

            //        if (NativeThinkgear.TG_GetValueStatus(connectionID, NativeThinkgear.DataType.MWM15_DATA_FILTER_TYPE) != 0)
            //        {
            //            escritor.WriteLine(" Find Filter Type:  " + NativeThinkgear.TG_GetValue(connectionID, NativeThinkgear.DataType.MWM15_DATA_FILTER_TYPE));
            //            break;
            //        }
            //    }

            //    errCode = NativeThinkgear.TG_EnableAutoRead(connectionID, 0); //stop
            //    escritor.WriteLine("auto read test stoped: " + errCode);
            //}
            //else
            //{
            //    escritor.WriteLine("auto read test failed: " + errCode);
            //    return false;
            //}

            NativeThinkgear.TG_Disconnect(connectionID); // disconnect test
            /* Clean up */
            NativeThinkgear.TG_FreeConnection(connectionID);

            Console.WriteLine("Fim Teste Porta: " + port);
            escritor.WriteLine("Fim Teste Porta: " + port);

            return true;
        }

        static void Main(string[] args)
        {
            string[] ports = SerialPort.GetPortNames();
            string connectionPort = "";
            for (int i = 0; i < ports.Length; i++)
            {
                Stream saida = File.Open("poc_log_port_" + ports[i] + ".txt", FileMode.Create);
                StreamWriter escritor = new StreamWriter(saida);
                
                if (testPortConnection(ports[i], escritor))
                {
                    connectionPort = ports[i];
                    Console.WriteLine("Conexão encontrada na porta " + connectionPort + "!");
                }

                escritor.Close();
                saida.Close();
                Console.WriteLine("File log created for port " + ports[i]);
            }

            if (connectionPort == "")
            {
                Console.WriteLine("Não foi possível conectar com o equipamento neste momento.");
            } else
            {
                Console.WriteLine("Conexão encontrada!");
            }
            Console.ReadLine();

            /* Print driver version number */
//            Console.WriteLine("Version: " + NativeThinkgear.TG_GetVersion());

            /* End program */
            Console.ReadLine();

        }
    }
}
