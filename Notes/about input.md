

o manager detecta os botões que estão sendo apertados no momento. ele mapeia esses botoes para comandos especificos. assim, mais de um botao pode representar um mesmo comando.

cada comando armazena se ele ja foi chamado naquele frame (down, ispressing, up)


quando eu carregar numa nova screen, ela precisa ignorar todos os inputs que ja estao apertados
consigo isso atraves de um manager
pra filtrar os comandos ja apertados... na verdade acho melhor nao seguir por esse caminho

acho que a forma mais simples e correta é colocar uma transição de 1s ou algo assim