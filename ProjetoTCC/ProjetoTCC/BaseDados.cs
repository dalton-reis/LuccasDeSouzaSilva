using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using Accord.Video.DirectShow;

namespace ProjetoTCC
{
    abstract class BaseDados
    {
        static List<Paciente> listaPacientes;

        static List<Sessao> listaSessoes;

        static List<Especialista> listaEspecialistas;

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
                        string[] s = new string[2];
                        s[0] = str.Substring(0, str.IndexOf(':'));
                        s[1] = str.Substring(str.IndexOf(':')+1);
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

        public static void updateEspecialistas()
        {
            if (listaEspecialistas != null)
            {
                listaEspecialistas.Clear();
            }
            else
            {
                listaEspecialistas = new List<Especialista>();
            }

            string startFolder = @"MindsEye";

            if(Directory.Exists(caminhoArquivos))
            {
                startFolder = caminhoArquivos;
            }

            if (startFolder.LastIndexOf("MindsEye") == -1)
            {
                startFolder += "\\MindsEye";
            }

            if (!Directory.Exists(startFolder + "\\Especialistas"))
            {
                Directory.CreateDirectory(startFolder + "\\Especialistas");
            }
            startFolder += "\\Especialistas";
            iniConfigEspecialistas(startFolder);
        }

        private static void iniConfigEspecialistas(string folder)
        {
            string file = folder + "\\config.txt";
            if (File.Exists(file))
            {
                try
                {
                    StringBuilder txt = new StringBuilder();

                    Stream entrada = File.Open(file, FileMode.Open);
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

                    string[] arr1 = linha.Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);

                    listaEspecialistas.Clear();

                    foreach (string str1 in arr1)
                    {
                        string[] arr2 = str1.Split(';');

                        long ID = 0;
                        string nome = "";
                        DateTime dataNasc = DateTime.Now;
                        string cpf = "";
                        string rg = "";
                        string sexo = "O";
                        string descricao = "";

                        foreach (string str2 in arr2)
                        {
                            string[] s = new string[2];
                            s[0] = Regex.Replace(str2.Substring(0, str2.IndexOf(':')).Replace("{", "").Trim(), @"\r\n?|\n", "");
                            s[1] = Regex.Replace(str2.Substring(str2.IndexOf(':') + 1).Trim(), @"\r\n?|\n", "");

                            string s0 = s[0].Substring(s[0].IndexOf("[") + 1, s[0].LastIndexOf("]") - 1).Trim();
                            switch (s0)
                            {
                                case "ID":
                                    ID = long.Parse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim());
                                    break;
                                case "nome":
                                    nome = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "dataNasc":
                                    dataNasc = DateTime.Parse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim());
                                    break;
                                case "cpf":
                                    cpf = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "rg":
                                    rg = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "sexo":
                                    sexo = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "descricao":
                                    descricao = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                            }
                        }
                        listaEspecialistas.Add(Especialista.create(ID, nome, dataNasc, cpf, rg, descricao, sexo));
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
            }
            else
            {
                try
                {
                    updateConfigEspecialistas();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Não foi possível criar o arquivo de configuração para os especialistas." +
                        "\nTente fechar o programa e abrir novamente." +
                        "\nSituação técnica:" + e.Message +
                        "\nOrigem:" + e.StackTrace.Substring(0, 255), "Aviso");
                }
            }
        }

        private static void updateConfigEspecialistas()
        {
            string startFolder = @"MindsEye";

            if (Directory.Exists(caminhoArquivos))
            {
                startFolder = caminhoArquivos;
            }

            if (startFolder.LastIndexOf("MindsEye") == -1)
            {
                startFolder += "\\MindsEye";
            }

            startFolder += "\\Especialistas";

            Directory.CreateDirectory(startFolder);

            Stream saida = File.Open(startFolder + "\\config.txt", FileMode.Create);
            StreamWriter escritor = new StreamWriter(saida);

            List<Especialista> l1 = listaEspecialistas.OrderBy(p => p.ID).ToList();


            foreach (Especialista esp in listaEspecialistas)
            {
                escritor.WriteLine("{");
                escritor.WriteLine("[ID]:[" + esp.ID + "];");
                escritor.WriteLine("[nome]:[" + esp.nome.Trim() + "];");
                escritor.WriteLine("[dataNasc]:[" + esp.dataNasc.ToString("dd/MM/yyyy") + "];");
                escritor.WriteLine("[cpf]:[" + esp.cpf.Trim() + "];");
                escritor.WriteLine("[rg]:[" + esp.rg.Trim() + "];");
                escritor.WriteLine("[sexo]:[" + esp.sexo.Trim() + "];");
                escritor.WriteLine("[descricao]:[" + esp.descricao.Trim() + "]");
                escritor.WriteLine("};;");
            }

            escritor.Close();
            saida.Close();

            foreach (Especialista esp in l1)
            {
                string folder = startFolder + "\\E_" + esp.ID;
                Directory.CreateDirectory(folder);
            }
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

            string startFolder = @"MindsEye";

            if (Directory.Exists(caminhoArquivos))
            {
                startFolder = caminhoArquivos;
            }

            if (startFolder.LastIndexOf("MindsEye") == -1)
            {
                startFolder += "\\MindsEye";
            }

            if (!Directory.Exists(startFolder + "\\Pacientes"))
            {
                Directory.CreateDirectory(startFolder + "\\Pacientes");
            }
            startFolder += "\\Pacientes";
            iniConfigPacientes(startFolder);
        }

        public static string getPacientesFolder()
        {
            string startFolder = @"MindsEye";

            if (Directory.Exists(caminhoArquivos))
            {
                startFolder = caminhoArquivos;
            }

            if (startFolder.LastIndexOf("MindsEye") == -1)
            {
                startFolder += "\\MindsEye";
            }

            if (!Directory.Exists(startFolder + "\\Pacientes"))
            {
                Directory.CreateDirectory(startFolder + "\\Pacientes");
            }
            return startFolder + "\\Pacientes";
        }

        public static string getEspecialistasFolder()
        {
            string startFolder = @"MindsEye";

            if (Directory.Exists(caminhoArquivos))
            {
                startFolder = caminhoArquivos;
            }

            if (startFolder.LastIndexOf("MindsEye") == -1)
            {
                startFolder += "\\MindsEye";
            }

            startFolder += "\\Especialistas";

            Directory.CreateDirectory(startFolder);

            return startFolder;
        }

        public static void updatePacienteSelecionado(long ID, string nome, DateTime dataNasc, string cpf, string rg, string descricao,
            string sexo, IdadeAprendizagem idadeAprLig, IdadeAprendizagem idadeAprLog, IdadeAprendizagem idadeAprMat, DateTime dataCadastro)
        {
            listaPacientes.Where(p => p.ID == ID).First().updateValues(nome, dataNasc, cpf, rg, descricao, 
                sexo, idadeAprLig, idadeAprLog, idadeAprMat, dataCadastro);
            updateConfigPacientes();
        }

        public static void excluiPacienteSelecionado(long ID)
        {
            listaPacientes.Remove(listaPacientes.Where(p => p.ID == ID).First());
            excluiArquivosPaciente(ID);
            updateConfigPacientes();
        }

        public static void updateEspecialistaSelecionado(long ID, string nome, DateTime dataNasc, string cpf, string rg, string descricao, string sexo)
        {
            listaEspecialistas.Where(e => e.ID == ID).First().updateValues(nome, dataNasc, cpf, rg, descricao, sexo);
            updateConfigEspecialistas();
        }

        public static void excluiEspecialistaSelecionado(long ID)
        {
            listaEspecialistas.Remove(listaEspecialistas.Where(e => e.ID == ID).First());
//            excluiArquivosEspecialista(ID);
            updateConfigEspecialistas();
        }

        public static void updateSessaoSelecionada(long ID, Especialista especialista, DateTime dataSessao, string caminhoVideo, string material, 
            string descricao, IdadeAprendizagem idadeAprLing, IdadeAprendizagem idadeAprLog, IdadeAprendizagem idadeAprMat, bool ieVerificado)
        {
            listaSessoes.Where(s => s.ID == ID).First().updateValues(especialista, dataSessao, caminhoVideo, material, descricao,
                idadeAprLing, idadeAprLog, idadeAprMat, ieVerificado);
            long IDPaciente = listaSessoes.Where(s => s.ID == ID).First().paciente.ID;
            updateConfigSessoesPaciente(IDPaciente);
        }

        public static void excluiSessaoSelecionada(long ID)
        {
            excluiArquivosSessao(ID);
            long IDPaciente = listaSessoes.Where(s => s.ID == ID).First().paciente.ID;
            listaSessoes.Remove(listaSessoes.Where(s => s.ID == ID).First());
            updateConfigSessoesPaciente(IDPaciente);
        }

        private static void excluiArquivosSessao(long IDSessao)
        {
            long IDPaciente = listaSessoes.Where(s => s.ID == IDSessao).First().paciente.ID;

            string startFolder = @"MindsEye";

            string caminhoArquivos = BaseDados.caminhoArquivos;

            if (Directory.Exists(caminhoArquivos))
            {
                startFolder = caminhoArquivos;
            }

            if (startFolder.LastIndexOf("MindsEye") == -1)
            {
                startFolder += "\\MindsEye";
            }

            if (!Directory.Exists(startFolder + "\\Pacientes"))
            {
                Directory.CreateDirectory(startFolder + "\\Pacientes");
            }
            startFolder += "\\Pacientes\\P_" + IDPaciente;

            if (!Directory.Exists(startFolder))
            {
                Directory.CreateDirectory(startFolder);
            }

            startFolder += "\\S_" + IDSessao;

            if (!Directory.Exists(startFolder))
            {
                Directory.CreateDirectory(startFolder);
            }
            
            Directory.Delete(startFolder, true);
        }

        private static void excluiArquivosPaciente(long IDPaciente)
        {
            string startFolder = @"MindsEye";

            string caminhoArquivos = BaseDados.caminhoArquivos;

            if (Directory.Exists(caminhoArquivos))
            {
                startFolder = caminhoArquivos;
            }

            if (startFolder.LastIndexOf("MindsEye") == -1)
            {
                startFolder += "\\MindsEye";
            }

            if (!Directory.Exists(startFolder + "\\Pacientes"))
            {
                Directory.CreateDirectory(startFolder + "\\Pacientes");
            }
            startFolder += "\\Pacientes\\P_" + IDPaciente;

            Directory.CreateDirectory(startFolder);

            Directory.Delete(startFolder, true);
        }

        private static void iniConfigPacientes(string folder)
        {
            string file = folder + "\\config.txt";
            if (File.Exists(file))
            {
                try
                {
                    StringBuilder txt = new StringBuilder();

                    Stream entrada = File.Open(file, FileMode.Open);
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

                    string[] arr1 = linha.Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);

                    listaPacientes.Clear();

                    foreach (string str1 in arr1)
                    {
                        string[] arr2 = str1.Split(';');
//                        string[] arr2 = Regex.Split(str1, ".;-[/;]", RegexOptions.Singleline);

                        long ID = 0;
                        string nome = "";
                        DateTime dataNasc = DateTime.Now;
                        string cpf = "";
                        string rg = "";
                        string sexo = "O";
                        string descricao = "";
                        IdadeAprendizagem idadeAprLing = IdadeAprendizagem.Ano9;
                        IdadeAprendizagem idadeAprLog = IdadeAprendizagem.Ano9;
                        IdadeAprendizagem idadeAprMat = IdadeAprendizagem.Ano9;
                        DateTime dataCadastro = DateTime.Now;

                        foreach (string str2 in arr2)
                        {
                            string[] s = new string[2];
                            s[0] = Regex.Replace(str2.Substring(0, str2.IndexOf(':')).Replace("{","").Trim(), @"\r\n?|\n", "");
                            s[1] = Regex.Replace(str2.Substring(str2.IndexOf(':') + 1).Trim(), @"\r\n?|\n", "");

                            string s0 = s[0].Substring(s[0].IndexOf("[") + 1, s[0].LastIndexOf("]") - 1).Trim();
                            switch (s0)
                            {
                                case "ID":
                                    string st = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    ID = long.Parse(s[1].Substring(s[1].IndexOf("[")+1, s[1].LastIndexOf("]")-1).Trim());
                                    break;
                                case "nome":
                                    nome = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "dataNasc":
                                    dataNasc = DateTime.Parse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim());
                                    break;
                                case "dataCadastro":
                                    dataCadastro = DateTime.Parse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim());
                                    break;
                                case "cpf":
                                    cpf = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "rg":
                                    rg = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "sexo":
                                    sexo = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "descricao":
                                    descricao = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "idadeAprLing":
                                    int a = -1;
                                    if (int.TryParse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim(), out a))
                                    {
                                        idadeAprLing = (IdadeAprendizagem)a;
                                    }
                                    break;
                                case "idadeAprLog":
                                    int b = -1;
                                    if (int.TryParse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim(), out b))
                                    {
                                        idadeAprLog = (IdadeAprendizagem)b;
                                    }
                                    break;
                                case "idadeAprMat":
                                    int c = -1;
                                    if (int.TryParse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim(), out c))
                                    {
                                        idadeAprMat = (IdadeAprendizagem)c;
                                    }
                                    break;
                            }
                        }
                        listaPacientes.Add(Paciente.create(ID, nome, dataNasc, cpf, rg, descricao, sexo, idadeAprLing, idadeAprLog, idadeAprMat, dataCadastro));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Não foi possível carregar o arquivo de configuração." +
                        "\nTente carrega-lo novamente através da janela de configurações." +
                        "\nSituação técnica:" + e.Message +
                        "\nOrigem:" + e.StackTrace.Substring(0, 255), "Aviso");
                }
            }
            else
            {
                try
                {
                    updateConfigPacientes();
                }
                catch (Exception e)
                {
                    MessageBox.Show("Não foi possível criar o arquivo de configuração para os pacientes." +
                        "\nTente fechar o programa e abrir novamente." +
                        "\nSituação técnica:" + e.Message +
                        "\nOrigem:" + e.StackTrace.Substring(0, 255), "Aviso");
                }
            }
        }

        private static void updateConfigPacientes()
        {
            string startFolder = @"MindsEye";

            if (Directory.Exists(caminhoArquivos))
            {
                startFolder = caminhoArquivos;
            }

            if (startFolder.LastIndexOf("MindsEye") == -1)
            {
                startFolder += "\\MindsEye";
            }

            startFolder += "\\Pacientes";

            Directory.CreateDirectory(startFolder);

            Stream saida = File.Open(startFolder + "\\config.txt", FileMode.Create);
            StreamWriter escritor = new StreamWriter(saida);

            List<Paciente> l1 = listaPacientes.OrderBy(p => p.ID).ToList();

            foreach (Paciente pac in l1)
            {
                escritor.WriteLine("{");
                escritor.WriteLine("[ID]:[" + pac.ID + "];");
                escritor.WriteLine("[nome]:[" + pac.nome.Trim() + "];");

                escritor.WriteLine("[dataNasc]:[" + pac.dataNasc.ToString("dd/MM/yyyy") + "];");
                escritor.WriteLine("[dataCadastro]:[" + pac.dataCadastro.ToString("dd/MM/yyyy") + "];");

                escritor.WriteLine("[cpf]:[" + pac.cpf.Trim() + "];");
                escritor.WriteLine("[rg]:[" + pac.rg.Trim() + "];");
                escritor.WriteLine("[sexo]:[" + pac.sexo.Trim() + "];");
                escritor.WriteLine("[descricao]:[" + pac.descricao.Trim() + "];");

                escritor.WriteLine("[idadeAprLing]:[" + ((int) pac.idadeAprLing) + "];");
                escritor.WriteLine("[idadeAprLog]:[" + ((int) pac.idadeAprLog) + "];");
                escritor.WriteLine("[idadeAprMat]:[" + ((int) pac.idadeAprMat) + "]");

                escritor.WriteLine("};;");
            }

            escritor.Close();
            saida.Close();

            foreach (Paciente pac in l1)
            {
                string folder = startFolder + "\\P_" + pac.ID;
                Directory.CreateDirectory(folder);
            }
        }

        public static void addPaciente(Paciente pac)
        {
            listaPacientes.Add(pac);
            updateConfigPacientes();
        }

        public static void addSessao(Sessao ses)
        {
            listaSessoes.Add(ses);
            updateConfigSessoesPaciente(ses.paciente.ID);
        }

        public static void addEspecialista(Especialista esp)
        {
            listaEspecialistas.Add(esp);
            updateConfigEspecialistas();
        }

        public static void updateSessoesPaciente(long pID)
        {
            Paciente pac = listaPacientes.Where(p => p.ID == pID).ToList().ElementAt(0);

            if (listaSessoes != null)
            {
                listaSessoes.Clear();
            }
            else
            {
                listaSessoes = new List<Sessao>();
            }

            string startFolder = @"MindsEye";

            if (Directory.Exists(caminhoArquivos))
            {
                startFolder = caminhoArquivos;
            }

            if (startFolder.LastIndexOf("MindsEye") == -1)
            {
                startFolder += "\\MindsEye";
            }

            if (!Directory.Exists(startFolder + "\\Pacientes"))
            {
                Directory.CreateDirectory(startFolder + "\\Pacientes");
            }
            startFolder += "\\Pacientes\\P_" + pac.ID;            

            if (!Directory.Exists(startFolder))
            {
                Directory.CreateDirectory(startFolder);
            }

            iniConfigSessoesPaciente(pac, startFolder);
        }

        private static List<Sessao> getSessoesPaciente(Paciente pac, string folder)
        {
            List<Sessao> list = new List<Sessao>();

            string file = folder + "\\config.txt";

            if (File.Exists(file))
            {
                try
                {
                    StringBuilder txt = new StringBuilder();

                    Stream entrada = File.Open(file, FileMode.Open);
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

                    string[] arr1 = linha.Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);

                    list.Clear();

                    foreach (string str1 in arr1)
                    {
                        string[] arr2 = str1.Split(';');

                        long ID = 0;
                        Paciente p = pac;
                        Especialista esp = null;
                        DateTime dataSessao = DateTime.Now;
                        string descricao = "";
                        string material = "";
                        string caminhoVideo = "";

                        IdadeAprendizagem idadeAprLing = IdadeAprendizagem.Ano9;
                        IdadeAprendizagem idadeAprLog = IdadeAprendizagem.Ano9;
                        IdadeAprendizagem idadeAprMat = IdadeAprendizagem.Ano9;
                        bool ieVerificado = false;

                        foreach (string str2 in arr2)
                        {
                            string[] s = new string[2];
                            s[0] = Regex.Replace(str2.Substring(0, str2.IndexOf(':')).Replace("{", "").Trim(), @"\r\n?|\n", "");
                            s[1] = Regex.Replace(str2.Substring(str2.IndexOf(':') + 1).Trim(), @"\r\n?|\n", "");

                            string s0 = s[0].Substring(s[0].IndexOf("[") + 1, s[0].LastIndexOf("]") - 1).Trim();
                            switch (s0)
                            {
                                case "ID":
                                    ID = long.Parse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim());
                                    break;
                                case "ID_ESP":
                                    long ID_ESP = long.Parse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim());
                                    List<Especialista> l1 = listaEspecialistas.Where(e => e.ID == ID_ESP).ToList();
                                    if (l1.Count > 0)
                                    {
                                        esp = l1.ElementAt(0);
                                    }
                                    break;
                                case "dataSessao":
                                    dataSessao = DateTime.Parse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim());
                                    break;
                                case "nomeVideo":
                                    caminhoVideo = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "descricao":
                                    descricao = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "material":
                                    material = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "idadeAprLing":
                                    int a = -1;
                                    if (int.TryParse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim(), out a))
                                    {
                                        idadeAprLing = (IdadeAprendizagem)a;
                                    }
                                    break;
                                case "idadeAprLog":
                                    int b = -1;
                                    if (int.TryParse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim(), out b))
                                    {
                                        idadeAprLog = (IdadeAprendizagem)b;
                                    }
                                    break;
                                case "idadeAprMat":
                                    int c = -1;
                                    if (int.TryParse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim(), out c))
                                    {
                                        idadeAprMat = (IdadeAprendizagem)c;
                                    }
                                    break;
                                case "verificado":
                                    string v = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    if (v.ToUpper().Equals("V"))
                                    {
                                        ieVerificado = true;
                                    }
                                    else
                                    {
                                        ieVerificado = false;
                                    }
                                    break;
                            }
                        }
                        list.Add(Sessao.create(ID, pac, esp, dataSessao, descricao, material, descricao, idadeAprLing, idadeAprLog, idadeAprMat, ieVerificado));
                    }
                }
                catch (Exception e)
                {
                    //MessageBox.Show("Não foi possível carregar o arquivo de configuração." +
                    //    "\nTente carrega-lo novamente através da janela de configurações." +
                    //    "\nSituação técnica:" + e.Message +
                    //    "\nOrigem:" + e.StackTrace.Substring(0, 255), "Aviso");
                }
            }

            return list;
        } 

        private static void iniConfigSessoesPaciente(Paciente pac, string folder)
        {
            string file = folder + "\\config.txt";
            if (File.Exists(file))
            {
                try
                {
                    StringBuilder txt = new StringBuilder();

                    Stream entrada = File.Open(file, FileMode.Open);
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

                    string[] arr1 = linha.Split(new string[] { ";;" }, StringSplitOptions.RemoveEmptyEntries);

                    listaSessoes.Clear();

                    foreach (string str1 in arr1)
                    {
                        string[] arr2 = str1.Split(';');

                        long ID = 0;
                        Paciente p = pac;
                        Especialista esp = null;
                        DateTime dataSessao = DateTime.Now;
                        string descricao = "";
                        string material = "";
                        string caminhoVideo = "";

                        IdadeAprendizagem idadeAprLing = IdadeAprendizagem.Ano9;
                        IdadeAprendizagem idadeAprLog = IdadeAprendizagem.Ano9;
                        IdadeAprendizagem idadeAprMat = IdadeAprendizagem.Ano9;
                        bool ieVerificado = false;

                        foreach (string str2 in arr2)
                        {
                            string[] s = new string[2];
                            s[0] = Regex.Replace(str2.Substring(0, str2.IndexOf(':')).Replace("{", "").Trim(), @"\r\n?|\n", "");
                            s[1] = Regex.Replace(str2.Substring(str2.IndexOf(':') + 1).Trim(), @"\r\n?|\n", "");

                            string s0 = s[0].Substring(s[0].IndexOf("[") + 1, s[0].LastIndexOf("]") - 1).Trim();
                            switch (s0)
                            {
                                case "ID":
                                   ID = long.Parse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim());
                                    break;
                                case "ID_ESP":
                                    long ID_ESP = long.Parse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim());
                                    List<Especialista> l1 = listaEspecialistas.Where(e => e.ID == ID_ESP).ToList();
                                    if(l1.Count > 0)
                                    {
                                        esp = l1.ElementAt(0);
                                    }
                                    break;
                                case "dataSessao":
                                    dataSessao = DateTime.Parse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim());
                                    break;
                                case "nomeVideo":
                                    caminhoVideo = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "descricao":
                                    descricao = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "material":
                                    material = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    break;
                                case "idadeAprLing":
                                    int a = -1;
                                    if (int.TryParse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim(), out a))
                                    {
                                        idadeAprLing = (IdadeAprendizagem)a;
                                    }
                                    break;
                                case "idadeAprLog":
                                    int b = -1;
                                    if (int.TryParse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim(), out b))
                                    {
                                        idadeAprLog = (IdadeAprendizagem)b;
                                    }
                                    break;
                                case "idadeAprMat":
                                    int c = -1;
                                    if (int.TryParse(s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim(), out c))
                                    {
                                        idadeAprMat = (IdadeAprendizagem)c;
                                    }
                                    break;
                                case "verificado":
                                    string v = s[1].Substring(s[1].IndexOf("[") + 1, s[1].LastIndexOf("]") - 1).Trim();
                                    if (v.Trim().ToUpper().Equals("V"))
                                    {
                                        ieVerificado = true;
                                    } else
                                    {
                                        ieVerificado = false;
                                    }
                                    break;
                            }
                        }
                        listaSessoes.Add(Sessao.create(ID, pac, esp, dataSessao, descricao, material, descricao, idadeAprLing, idadeAprLog, idadeAprMat, ieVerificado));
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Não foi possível carregar o arquivo de configuração." +
                        "\nTente carrega-lo novamente através da janela de configurações." +
                        "\nSituação técnica:" + e.Message +
                        "\nOrigem:" + e.StackTrace.Substring(0, 255), "Aviso");
                }
            }
            else
            {
                try
                {
                    updateConfigSessoesPaciente(pac.ID);
                }
                catch (Exception e)
                {
                    MessageBox.Show("Não foi possível criar o arquivo de configuração para as sessões." +
                        "\nTente fechar o programa e abrir novamente." +
                        "\nSituação técnica:" + e.Message +
                        "\nOrigem:" + e.StackTrace.Substring(0, 255), "Aviso");
                }
            }
        }

        private static void updateConfigSessoesPaciente(long IDPaciente)
        {
            string startFolder = @"MindsEye";

            if (Directory.Exists(caminhoArquivos))
            {
                startFolder = caminhoArquivos;
            }

            if (startFolder.LastIndexOf("MindsEye") == -1)
            {
                startFolder += "\\MindsEye";
            }

            startFolder += "\\Pacientes\\P_" + IDPaciente;

            Directory.CreateDirectory(startFolder);

            Stream saida = File.Open(startFolder + "\\config.txt", FileMode.Create);
            StreamWriter escritor = new StreamWriter(saida);

            List<Sessao> l1 = listaSessoes.OrderBy(s => s.ID).ToList();

            foreach (Sessao ses in listaSessoes)
            {
                escritor.WriteLine("{");
                escritor.WriteLine("[ID]:[" + ses.ID + "];");
                escritor.WriteLine("[ID_ESP]:[" + ses.especialista.ID + "];");
                escritor.WriteLine("[dataSessao]:[" + ses.dataSessao.ToString("dd/MM/yyyy HH:mm") + "];");
                escritor.WriteLine("[nomeVideo]:[" + ses.nomeVideo.Trim() + "];");
                escritor.WriteLine("[material]:[" + ses.material.Trim() + "];");

                escritor.WriteLine("[idadeAprLing]:[" + ((int)ses.idadeAprLing) + "];");
                escritor.WriteLine("[idadeAprLog]:[" + ((int)ses.idadeAprLog) + "];");
                escritor.WriteLine("[idadeAprMat]:[" + ((int)ses.idadeAprMat) + "];");

                escritor.WriteLine("[verificado]:[" + ses.getStringVerificado() + "];");

                escritor.WriteLine("[descricao]:[" + ses.descricao.Trim() + "]");
                escritor.WriteLine("};;");
            }

            escritor.Close();
            saida.Close();

            foreach (Sessao ses in l1)
            {
                string folder = startFolder + "\\S_" + ses.ID;
                Directory.CreateDirectory(folder);
            }
        }

        public static List<Especialista> getEspecialistas()
        {
            if (listaEspecialistas  != null)
            {
                return listaEspecialistas.OrderBy(e => e.nome).ToList();
            }
            else
            {
                return new List<Especialista>();
            }
        }

        public static Paciente getPaciente(long ID)
        {
            return BaseDados.getPacientes().Where(p => p.ID == ID).First();
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

        public static List<Sessao> getSessoesEspecialista(long IDEspecialista)
        {
            List<Sessao> list = new List<Sessao>();

            if (IDEspecialista > 0)
            {
                foreach (Paciente p in getPacientes())
                {
                    list.AddRange(getPacienteSessoes(p));
                }
                list = list.Where(s => s.especialista.ID == IDEspecialista).ToList();
            }

            return list;
        }

        public static List<Sessao> getPacienteSessoes(Paciente pac)
        {
            List<Sessao> list = new List<Sessao>();
            if (pac.ID > 0)
            {
                string startFolder = @"MindsEye";

                if (Directory.Exists(caminhoArquivos))
                {
                    startFolder = caminhoArquivos;
                }

                if (startFolder.LastIndexOf("MindsEye") == -1)
                {
                    startFolder += "\\MindsEye";
                }

                startFolder += "\\Pacientes\\P_" + pac.ID;

                Directory.CreateDirectory(startFolder);

                list.AddRange(getSessoesPaciente(pac, startFolder));
            }
            return list;
        }

        public static List<Sessao> getPacienteSessoes()
        {
            if (listaSessoes != null)
            {
                return listaSessoes.OrderBy(p => p.dataSessao).ToList();
            }
            else
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

            FilterInfoCollection VideoDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);

            if (VideoDevices.Count > 0 && cameraIndex < VideoDevices.Count)
            {
                nomeCamera = VideoDevices[cameraIndex].Name;
            }
            else
            {
                nomeCamera = "";
            }
            BaseDados.gravaConfig();
        }

        public static string getIdadeAprendizagem(IdadeAprendizagem value)
        {
            switch (value)
            {
                case IdadeAprendizagem.Ano1:
                    return "1o Ano";
                case IdadeAprendizagem.Ano2:
                    return "2o Ano";
                case IdadeAprendizagem.Ano3:
                    return "3o Ano";
                case IdadeAprendizagem.Ano4:
                    return "4o Ano";
                case IdadeAprendizagem.Ano5:
                    return "5o Ano";
                case IdadeAprendizagem.Ano6:
                    return "6o Ano";
                case IdadeAprendizagem.Ano7:
                    return "7o Ano";
                case IdadeAprendizagem.Ano8:
                    return "8o Ano";
                case IdadeAprendizagem.Ano9:
                    return "9o Ano";
                default:
                    return "";
            }
        }

        private static string getSaveCleanText(string text)
        {
            return text.Replace("{","/{").Replace("}", "/}").Replace("[", "/[").Replace("]", "/]").Replace(":", "/:").Replace(";", "/;");
        }

        private static string getSaveUncleanText(string text)
        {
            return text.Replace("/{", "{").Replace("/}", "}").Replace("/[", "[").Replace("/]", "]").Replace("/:", ":").Replace("/;", ";");
        }

        // Referência: https://social.msdn.microsoft.com/Forums/pt-BR/1dbe81e6-c063-4ae5-ae1d-5643fb4b0e62/validar-cpf-em-c?forum=vscsharppt
        public static bool validaCpf(string cpf)
        {
            try
            {
                if (cpf.Trim().Length < 1)
                {
                    return false;
                }
                else
                {
                    int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
                    int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };

                    string tempCpf;

                    string digito;

                    int soma;

                    int resto;

                    cpf = cpf.Trim().Replace(".", "").Replace(",", "").Replace("-", "");

                    if (cpf.Length != 11)
                    {
                        return false;
                    }

                    tempCpf = cpf.Substring(0, 9);

                    soma = 0;

                    for (int i = 0; i < 9; i++)
                    {
                        soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
                    }

                    resto = soma % 11;

                    if (resto < 2)
                    {
                        resto = 0;
                    }
                    else
                    {
                        resto = 11 - resto;
                    }

                    digito = resto.ToString();

                    tempCpf = tempCpf + digito;

                    soma = 0;

                    for (int i = 0; i < 10; i++)
                    {
                        soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
                    }

                    resto = soma % 11;

                    if (resto < 2)
                    {
                        resto = 0;
                    }
                    else
                    {
                        resto = 11 - resto;
                    }
                    digito = digito + resto.ToString();

                    return cpf.EndsWith(digito);
                }
            } catch (Exception ex)
            {
                return false;
            }
        }
    }
}