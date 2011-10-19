 -- Infra-estrutura de chave p�blica para testes --
 --------------------------------------------------
 
 1. Autoridades certificadoras
 
	1.1 CA1
		- Autoridade de certifica��o raiz: "CA1 - Autoridade Certificadora Raiz - Projecto GNS"
		- Autoridade de certifica��o interm�dia: "CA1 - Autoridade Certificadora Intermedia - Projecto GNS"
		- "End entities": Alice, Carol e Eleanor
		
	1.2 CA2
		- Autoridade de certifica��o raiz: "CA2 - Autoridade Certificadora Raiz - Projecto GNS"
		- Autoridade de certifica��o interm�dia: "CA2 - Autoridade Certificadora Intermedia - Projecto GNS"
		- "End entities": Bob, David
		
2. Pastas e ficheiros

	- Pasta "certs.end.entities"
		- Ficheiros .cer para todas as "end entities" (Alice, Bob, Carol, David, Eleanor).
		- Cada entidade tem tr�s ficheiros com "key usage diferentes"
		
	- Pasta pfx
		- Ficheiros .pfx (formato PKCS #12) com certificados e chaves privadas das "end entities". 
		- A password � "changeit"
		
	- Pasta "certs.CA.intermediate"
		- Ficheiros .cer com os certificados das autoridades de certifica��o interm�dias
		
	- Pasta "trustanchors"
		- Ficheiros .jks com as "trust anchors" (certificados auto-assinados das autoridades de certifica��o raiz)
		- A password � changeit
	