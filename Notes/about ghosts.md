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
    

movimentando fantasmas:
    