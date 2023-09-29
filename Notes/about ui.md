"give it a chance to handle input" => handle input retorna um bool


game screen
  '-> hud screen

pergunta pra hud screen se ela vai tratar input, ela retorna false
entao pergunta pra game screen, que retorna true

na verdade, o bool representa "esta screen vai deixar o input passar pra tela de baixo?"
no caso da hud screen, sim.
no caso da game screen, nao.

no caso de um menu de pause, nao.
no caso de uma popup, nao.


existem varias formas de sair de uma mesma screen?
    sim. uma popup pode ir pra uma proxima tela, ou voltar pra tela anterior

ScreenManager.AddScreen(new Screen())

-----------

que informacoes eu quero na hud?
- player life: 0
- score: 000
- draw fps: 000
- match time: 00:00