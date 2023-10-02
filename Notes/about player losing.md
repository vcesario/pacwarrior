ao colidir com um fantasma, o jogador perde input e o guerreiro toca uma animação rapida de morte.
se houver vidas sobrando, ele perde 1 e respawna no ponto inicial, piscando pois esta invulneravel por um tempo (pra evitar de dar ruim caso spawne em cima de um fantasma)
se nao houver vidas sobrando, apos animaçao sobe a tela de game over

começa o jogo com 3 vidas. onde que essa variavel vai ficar? no proprio obj do player
maquina de estado pro guerreiro? =>
    - Walkable: recebe input para andar e pausar jogo, coleta coisas, é detectavel e perseguivel pelos fantasmas, relogio avança normal
    - Powered: recebe input para andar e pausar jogo, anda batendo, coleta coisas, é detectavel pelos fantasmas mas eles fogem, relogio avança normal
    - Dying: recebe input para pausar jogo, fantasmas nao detectam, nao coleta nada, relogio nao avança
    - Spawning: recebe input para andar e pausar, fantasmas nao detectam, coleta coisas, relogio avança
    - Intro: nao recebe input de andar nem de pausar, fantasmas nao detectam (tanto faz), nao coleta nada (tanto faz), relogio nao avança
    - GameOver: recebe input para voltar a tela de titulo, fantasmas nao detectam, nao coleta nada, relogio nao avança

acho um pouco estranho o relogio avançar dependendo do estado do player...
talvez faria mais sentido se o jogo tivesse seus proprios estados?
    - Intro: introduzindo o jogo, relogio nao avança (player esta no estado intro), sai apos animação de intro
    - Running: jogo rolando (player nos estados walkable powered, dying, spawning), sai apos jogador causar fim de jogo
    - GameOver: tela de fim de jogo, relogio nao avança (player no estado gameover), sai apos jogador pressionar botao