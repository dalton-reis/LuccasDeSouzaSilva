using DirectShowLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WorldSimulator
{
    abstract class Biblioteca
    {
        static List<Paciente> listaPacientes;

        static List<Sessao> listaSessoes;

        public static string caminhoArquivos { get; private set; }
        public static string nomeEstab { get; private set; }
        public static string enderecoEstab { get; private set; }
        public static string foneEstab { get; private set; }
        public static string caminhoLogo { get; private set; }
        public static string nomeCamera { get; private set; }

        public static void iniConfig()
        {
            if (File.Exists("config.txt"))
            {
                try
                {
                    StringBuilder txt = new StringBuilder();
                    
                    Stream entrada = File.Open("config.txt", FileMode.Open);
                    StreamReader leitor = new StreamReader(entrada);
                    string linha = leitor.ReadLine();
                    while (linha != null)
                    {
                        txt.Append(linha.Trim());
                        linha = leitor.ReadLine();
                    }
                    leitor.Close();
                    entrada.Close();
                
                    linha = txt.ToString();

                    string[] config = linha.Replace("{", "").Replace("}", "").Trim().Split(';');
                    object[] vars = {caminhoArquivos, nomeEstab, enderecoEstab, foneEstab, nomeCamera, caminhoLogo};

                    foreach (string str in config)
                    {
                        string[] s = str.Split(':');
                        string s0 = s[0].Replace("[","").Replace("]","").Trim();
                        switch (s0)
                        {
                            case "caminhoArquivos":
                                caminhoArquivos = s[1].Replace("[", "").Replace("]", "").Trim();
                                break;
                            case "nomeEstab":
                                nomeEstab = s[1].Replace("[", "").Replace("]", "").Trim();
                                break;
                            case "enderecoEstab":
                                enderecoEstab = s[1].Replace("[", "").Replace("]", "").Trim();
                                break;
                            case "foneEstab":
                                foneEstab = s[1].Replace("[", "").Replace("]", "").Trim();
                                break;
                            case "nomeCamera":
                                nomeCamera = s[1].Replace("[", "").Replace("]", "").Trim();
                                break;
                            case "caminhoLogo":
                                caminhoLogo = s[1].Replace("[", "").Replace("]", "").Trim();
                                break;
                        }
                    }
                }
                catch (Exception e)
                {
                    caminhoArquivos = "";
                    nomeEstab = "";
                    enderecoEstab = "";
                    foneEstab = "";
                    caminhoLogo = "";
                    nomeCamera = "";
                    MessageBox.Show("Não foi possível carregar o arquivo de configuração." +
                        "\nTente carrega-lo novamente através da janela de configurações." +
                        "\nSituação técnica:" + e.Message +
                        "\nOrigem:" + e.StackTrace.Substring(0, 255), "Aviso");
                }
            } else
            {
                try
                {
                    caminhoArquivos = "";
                    nomeEstab = "";
                    enderecoEstab = "";
                    foneEstab = "";
                    caminhoLogo = "";
                    nomeCamera = "";
                    gravaConfig();
                } catch (Exception e)
                {
                    MessageBox.Show("Não foi possível criar o arquivo de configuração." +
                        "\nTente fechar o programa e abrir novamente." +
                        "\nSituação técnica:" + e.Message +
                        "\nOrigem:" + e.StackTrace.Substring(0,255), "Aviso");
                } finally
                {
                    caminhoArquivos = "";
                    nomeEstab = "";
                    enderecoEstab = "";
                    foneEstab = "";
                    caminhoLogo = "";
                    nomeCamera = "";
                }
            }
        }

        private static void gravaConfig()
        {
            Stream saida = File.Open("config.txt", FileMode.Create);
            StreamWriter escritor = new StreamWriter(saida);

            escritor.WriteLine("{");
            escritor.WriteLine("[caminhoArquivos]:[" + caminhoArquivos + "];");
            escritor.WriteLine("[nomeEstab]:[" + nomeEstab + "];");
            escritor.WriteLine("[enderecoEstab]:[" + enderecoEstab + "];");
            escritor.WriteLine("[foneEstab]:[" + foneEstab + "];");
            escritor.WriteLine("[nomeCamera]:[" + nomeCamera + "];");
            escritor.WriteLine("[caminhoLogo]:[" + caminhoLogo + "]");
            escritor.WriteLine("}");

            escritor.Close();
            saida.Close();
        }

        public static void updatePacientes()
        {
            if(listaPacientes != null)
            {
                listaPacientes.Clear();
            } else
            {
                listaPacientes = new List<Paciente>();
            }

            listaPacientes.Add(new Paciente("João de Souza Borges", DateTime.Parse("01/07/2012"), "", "", "", ""));
            listaPacientes.Add(new Paciente("Carolina D'alencar", DateTime.Parse("16/01/2013"), "", "", "", ""));
            listaPacientes.Add(new Paciente("Guilherme Antunes", DateTime.Parse("25/11/2011"), "", "", "", ""));

            updateSessoes();
        }

        private static void updateSessoes()
        {
            if (listaSessoes != null)
            {
                listaSessoes.Clear();
            }
            else
            {
                listaSessoes = new List<Sessao>();
            }

            listaSessoes.Add(new Sessao(listaPacientes.ElementAt(0), DateTime.Parse("01/07/2012"), "", "", ""));
            listaSessoes.Add(new Sessao(listaPacientes.ElementAt(1), DateTime.Parse("17/01/2013"), "", "", ""));
            listaSessoes.Add(new Sessao(listaPacientes.ElementAt(2), DateTime.Parse("25/11/2011"), "", "", ""));
        }

        public static List<Paciente> getPacientes()
        {
            if (listaPacientes != null)
            {
                return listaPacientes.OrderBy(p => p.nome).ToList();
            } else
            {
                return new List<Paciente>();
            }
        }

        public static List<Sessao> getPacienteSessoes(long pacienteID)
        {
            if (listaSessoes != null)
            {
                List<Sessao> l1 = listaSessoes.Where(s => ((Paciente)s.paciente).ID.Equals(pacienteID)).ToList();
                return l1;
            } else
            {
                return new List<Sessao>();
            }
        }

        public static void updateConfig(string fileFolder, int cameraIndex, string nome, string endereco, string fone, string logoFolder)
        {
            caminhoArquivos = fileFolder.Trim();
            nomeEstab = nome.Trim();
            enderecoEstab = endereco.Trim();
            foneEstab = fone.Trim();
            caminhoLogo = logoFolder.Trim();

            DsDevice[] capDevices = DsDevice.GetDevicesOfCat(FilterCategory.VideoInputDevice);
            if (capDevices.Length > 0 && cameraIndex < capDevices.Length)
            {
                nomeCamera = capDevices[cameraIndex].Name;
            } else
            {
                nomeCamera = "";
            }
        }
    }
}
