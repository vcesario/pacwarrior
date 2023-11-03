Game Screen instancia 3 ghosts na metade superior da fase

fazer uma lista com todos os tiles andaveis?

o ghost precisa de uma position e de um center => isso esta no bounding box

como renderizar o ghost?

criar texturebox a partir da posição do grid (calcular top left a partir da coordenada do grid)

ainda nao precisa de collider

tex é igual a do player

---- ghost manager:

para cada fantasma:
    pegar posição atual no grid, e direção atual
    calcular distancia em relação ao player (distancia simples ou por pathfinding?)
    decidir objetivo baseado na distancia em relação ao player: roam ou chase
    
    roam:
        baseado na direção atual, olhar para tile da frente e tile dos lados
        escolher um aleatório entre os que não são collider
        mover até esse tile baseado na moveSpeed
    
    chase:
        ???
    
    flee:
    

movimentando fantasmas:
    é melhor calcular o path finding pra cada fantasma de forma independente? ou tentar otimizar calculando tudo no mesmo frame, sempre que possível? independente

    mapgrid contem uma função de getshortestpath(source, target)... não

    cada fantasma tem uma lista de PathPoints (PathPoint é uma struct contendo uma coordenada do grid e uma direção)
    sempre que um fantasma termina um movimento de tile, ele atualiza o comportamento (roam, chase ou flee)
    caso o comportamento seja o mesmo que o anterior, ele verifica se sua lista acabou.
    se nao acabou, continua percorrendo ela até que acabe. se acabou, calcula uma lista nova baseada nesse comportamento.
    caso o comportamento tenha mudado, joga a lista fora e calcula uma nova para aquele comportamento
        só precisa jogar a lista fora mesmo se for de roam pra chase, roam pra flee, chase pra flee ou flee pra chase. de flee e chase pra roam, não preciso me preocupar em jogar a lista fora
    
------

refatorando fantasmas:

a classe GhostAI não esta fazendo muito sentido. a ideia era processar todos os fantasmas em um batch pra economizar volume de memória e taxa de acesso de memória.

mas o que acabou acontecendo foi que eu criei metodos individuais dentro da classe geral.
a classe GhostAI está com proposito duplicado: gerenciar o conjunto ativo de ghosts na cena, e tambem cuidar do comportamento dos ghosts. talvez faria mais sentido separar GhostAI em: GhostManager e GhostBrain