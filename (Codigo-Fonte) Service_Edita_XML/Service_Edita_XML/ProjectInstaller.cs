/*
 * Created by SharpDevelop.
 * User: Omnia
 * Date: 17/08/2020
 * Time: 14:06
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Service_Edita_XML
{
	[RunInstaller(true)]
	public class ProjectInstaller : Installer
	{
		private ServiceProcessInstaller serviceProcessInstaller;
		private ServiceInstaller serviceInstaller;
		
		public ProjectInstaller()
		{
			serviceProcessInstaller = new ServiceProcessInstaller();
			serviceInstaller = new ServiceInstaller();
			// Here you can set properties on serviceProcessInstaller or register event handlers
			serviceProcessInstaller.Account = ServiceAccount.LocalService;//Nivel de permissao
														//LocalService = Admin do sistema
														//NetworkService = Sem privilegios e com credencial
														//LocalSystem = Executa sem privilegios e sem credencial
														//User = Solicita senha para iniciar
			
			serviceInstaller.ServiceName = Service_Edita_XML.MyServiceName;
			this.Installers.AddRange(new Installer[] { serviceProcessInstaller, serviceInstaller });
		}
	}
}
