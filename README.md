# Objetivo do serviço Service_Edita_XML

Esse serviço foi desenvolvido para monitorar uma pasta escolhida pelo usuário onde tenha arquivo com extensão ".XML" e em seguida esses arquivos serão lidos e o valor dentro da TAG "codigo_lis" será corrigido, removendo o espaço em branco antes do valor, após isso o arquivo será atualizado e o próximo arquivo será lido, terminando apenas quando todos estiverem atualizados.

O caminho a ser percorrido dentro do XML para encontrar a TAG "codigo_lis" é //entidade/pacientes/paciente

O método de monitorar a pasta foi definido para todos os arquivo que forem criados dentro da pasta, incluindo arquivos arrastados, arquivos atualizados serão ignorados.

# Para instalar o serviço
*1° Crie uma pasta no disco C:// com o nome "Service_Edita_XML" (OBRIGATÓRIO)

*2° Execute o arquivo "Instalar Servico.bat" como administrador (OBRIGATÓRIO) 

*3° Edite o arquivo "XML_Config.txt" que foi criado e insira o endereço da pasta que contém os arquivos XML (OBRIGATÓRIO) 

*4° Execute o arquivo "Iniciar Servico.bat" como administrador

OBS: Após o serviço estar instalado ele receberá o nome de "Editor_XML"

# Para alterar o endereço da pasta que contém os arquivos XML
*1° Execute o arquivo "Parar Servico.bat" como administrador 

*2° Edite o arquivo "XML_Config.txt" e insira o novo endereço da pasta que contém os arquivos XML 

*3° Execute o arquivo "Iniciar Servico.bat" como administrador

# Para desinstalar o serviço
*1° Para desinstalar o serviço execute o arquivo "Desinstalar Servico.bat" como administrador

OBS.: Caso o serviço não altere os arquivos XML, reinicie o serviço e verifique o arquivo "XML_Log.txt", caso tenha problemas durante a instalação verifique se todos os passos foram seguidos.

# O serviço foi testado nas seguintes versões Windows
WINDOWS 7

WINDOWS 8

WINDOWS 10

Caso tenha dúvidas sobre como o SC create ou SC delete funciona utilize esse link:
https://centraldeatendimento.totvs.com/hc/pt-br/articles/360040025993-Windows-Configura%C3%A7%C3%A3o-de-um-servi%C3%A7o-do-Windows-atrav%C3%A9s-do-sc-exe

