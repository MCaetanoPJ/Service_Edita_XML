/*
 * Created by SharpDevelop.
 * User: Omnia
 * Date: 17/08/2020
 * Time: 14:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;

//Usadas para ler o XML
using System.Linq;
using System.Xml;

//Usada para manipular arquivos
using System.IO;

//Usada pelo serviço windows
using System.ServiceProcess;


namespace Service_Edita_XML
{
	public class Service_Edita_XML : ServiceBase
	{
		//Nome do Servico
		public const string MyServiceName = "Service_Edita_XML";
		
		//Variaveis para controle do servico
		private static FileSystemWatcher monitorar;//Monitorar a pasta XML
		const string filtrar_extensao = "*.xml"; //XML = LER SOMENTE XML DENTRO DA PASTA INFORMADA
		const bool is_monitorar_subdiretorio_pasta = false;//TRUE = LER O SUBDIRETORIO || FALSE = NAO LER O SUBDIRETORIO
		//Enderecos para os arquivos
		private string Diretorio_Pasta_XML;//Endereço da pasta XML dentro do CONFIG
		const string Diretorio_Arquivo_Config = "C:\\Service_Edita_XML\\XML_Config.txt";//Endereço do CONFIG TXT
		const string Diretorio_Arquivo_Log = "C:\\Service_Edita_XML\\XML_Log.txt";//Endereço do LOG TXT
		
		public Service_Edita_XML()
		{
			InitializeComponent();
		}
		
		private void InitializeComponent()
		{
			this.ServiceName = MyServiceName;
		}
		
		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			// TODO: Add cleanup code here (if required)
			base.Dispose(disposing);
		}
		
		//Iniciar o servico
		protected override void OnStart(string[] args)
		{
			Criar_Log_TXT("O serviço foi iniciado","OnStart");
			Criar_Arquivo_Config_TXT();
			Ler_Arquivo_Config_TXT();
			Ler_Todos_Arquivos_XML();//Altera todos os arquivos XML na pasta quando iniciar
			Monitorar_Arquivo_XML();//Altera apenas os novos arquivos criados
		}
		//Encerrar o servico
		protected override void OnStop()
		{
			Criar_Log_TXT("O serviço foi finalizado \n","OnStop");
			// TODO: Add tear-down code here (if required) to stop your service.
		}
		//Monitorar a pasta
		private void Monitorar_Arquivo_XML()
        {
			try{
				bool is_monitorar_pasta;
				if(File.Exists(Diretorio_Arquivo_Config)){//Verifica se o arquivo com o endereço existe
					is_monitorar_pasta = true;//Ativar monitoramento da pasta
		            monitorar = new FileSystemWatcher(Diretorio_Pasta_XML, filtrar_extensao)
		            {
		                IncludeSubdirectories = is_monitorar_subdiretorio_pasta// vai monitorar os subdiretorios da pasta selecionada
		            };
		            monitorar.Created += OnFileChanged;//Monitora novos arquivos criados dentro da pasta
		            monitorar.EnableRaisingEvents = is_monitorar_pasta;//Vai monitorar a pasta selecionada
				}
			}
			catch(Exception ex){
				Criar_Log_TXT("Erro ao Monitorar a pasta com XML: "+ex.Message, "Monitorar_Arquivo_XML");
			}
        }
		private void OnFileChanged(object sender, FileSystemEventArgs e)
        {
			string aux = "O arquivo "+e.Name + " sera Corrigido";
			Criar_Log_TXT(aux,"OnFileChanged");
			Corrigir_Arquivo_XML(e.FullPath);//Envia para o método o endereço do arquivo que foi alterado
        }
		//Criar e ler o arquivo com o endereco da pasta a ser monitorada
		private void Criar_Arquivo_Config_TXT(){
			try{
				if(!File.Exists(Diretorio_Arquivo_Config)){//Verifica se o arquivo com o endereço existe
					StreamWriter c = new StreamWriter(Diretorio_Arquivo_Config);
					c.WriteLine(Diretorio_Pasta_XML);
					c.Close();
					c.Dispose();
					Criar_Log_TXT("Arquivo Config Criado", "Criar_Arquivo_Config_TXT");
				}
			}
			catch(Exception ex){
				Criar_Log_TXT("Erro ao criar arquivo Config "+ex.Message, "Criar_Arquivo_Config_TXT");
			}
		}
		private void Ler_Arquivo_Config_TXT(){
			try{
				if(File.Exists(Diretorio_Arquivo_Config)){//Verifica se o arquivo com o endereço existe
					StreamReader c = new StreamReader(Diretorio_Arquivo_Config);
						Diretorio_Pasta_XML = c.ReadLine();
						c.Close();
						c.Dispose();
						Criar_Log_TXT("Arquivo CONFIG lido", "Ler_Arquivo_Config_TXT");
				}
				else{
					Criar_Log_TXT("Não foi encontrado o arquivo CONFIG", "Ler_Arquivo_Config_TXT");
				}
			}
			catch(Exception ex){
				Criar_Log_TXT("Erro na leitura do arquivo CONFIG: "+ex.Message,"Ler_Arquivo_Config_TXT");
			}
		}
		//Leitura e alteracao do XML
		private void Ler_Todos_Arquivos_XML(){
			try{
				if(Diretorio_Pasta_XML != null){
					string[] arquivos_XML_no_Diretorio = Directory.GetFiles(Diretorio_Pasta_XML, filtrar_extensao, SearchOption.TopDirectoryOnly);//Ler apenas os arquivo XML no diretorio Selecionado
		        	foreach (string arq in arquivos_XML_no_Diretorio)//Percorre o vetor com o endereço dos arquivos XML
		        	{
		        		Corrigir_Arquivo_XML(arq);
		        	}
				}
				else{
					Criar_Log_TXT("O arquivo CONFIG está vazio", "Ler_Todos_Arquivos_XML");
				}
			}
			catch(Exception ex){
				Criar_Log_TXT("O Diretorio informado em CONFIG é inválido: "+ex.Message, "Ler_Todos_Arquivos_XML");
			}
		}
		private void Corrigir_Arquivo_XML(string Diretorio_Arquivo_XML){
			try{
				XmlDocument Arquivo_XML = new XmlDocument();
	            Arquivo_XML.Load(Diretorio_Arquivo_XML);
	            XmlNodeList Elemento_XML = Arquivo_XML.SelectNodes("//entidade/pacientes/paciente");//Endereco do atributo a ser selecionado
	            foreach(XmlNode endereco_Elemento_XML in Elemento_XML)//Percorre dentro do arquivo XML
	            {
	                string codigolis_antigo = endereco_Elemento_XML.Attributes["codigo_lis"].Value;//Lê o valor atual do codigo_lis
	                string codigolis_novo = codigolis_antigo.TrimEnd();//remove o espaço no final do valor do codigo_lis
	                endereco_Elemento_XML.Attributes["codigo_lis"].Value = codigolis_novo;//Insere o novo valor no codigo_lis
	            }
	            Arquivo_XML.Save(Diretorio_Arquivo_XML);//endereco para atualizar o arquivo XML
	            Criar_Log_TXT("Arquivo XML corrigido: "+Diretorio_Arquivo_XML, "Corrigir_Arquivo_XML");
			}
			catch(Exception ex){
				Criar_Log_TXT("Erro ao corrigir o arquivo XML "+ex.Message, "Corrigir_Arquivo_XML");
			}
		}
		//Criar o LOG de todo o sistema
		private void Criar_Log_TXT(string Mensagem, string nome_metodo){
			if (!File.Exists(Diretorio_Arquivo_Log)){
					StreamWriter c = File.CreateText(Diretorio_Arquivo_Log); 
					string Mensagem_Final = DateTime.Now + " - Nome do Metodo: "+nome_metodo+" - Mensagem: "+ Mensagem;
					c.WriteLine(Mensagem_Final);
					c.Close();
					c.Dispose();
			}
			else{
					StreamWriter sw = File.AppendText(Diretorio_Arquivo_Log);
					string Mensagem_Final = DateTime.Now + " - Nome do Metodo: "+nome_metodo+" - Mensagem: "+ Mensagem;
					sw.WriteLine(Mensagem_Final);
					sw.Close();
					sw.Dispose();
				}
		}
	}
}
