# 🧠 Hero Shooter — Brain & Knowledge Base

Bem-vindo à base de conhecimento e planejamento ágil do **Hero Shooter** inspirado em *Dust* (Counter-Strike), desenvolvido em Unity com ProBuilder, FSM modular e sistema de percepção acústica.

---

## 🗺️ Mapa Mental & Wikilinks da Arquitetura

- **Level Design & Arquitetura Espacial:**
  - [[LevelDesign-Blockmesh|Blockmesh & Prototipagem ProBuilder]]
  - [[LevelDesign-CriticalPath|Critical Path, Lanes & Chokepoints]]
  - [[LevelDesign-Circulation|Circulation, Flanks & Loop Design]]
  - [[LevelDesign-Verticality|Verticality, Ramps & Windows]]
  - [[Navigation-NavMesh|NavMeshSurface & OffMeshLinks]]

- **Sistemas do Jogador:**
  - [[Player-Movement|Player Controller & Mecânicas de Movimento]]
  - [[Player-Camera|Câmera Híbrida 1ª/3ª Pessoa (Tecla P) & ADS]]
  - [[Weapons-System|Sistema de Armas (Left Click, Scroll & Recoil)]]
  - [[Hero-Skills|Habilidades e Magias (1, 2, 3, 4) & Efeitos de Partículas]]

- **Percepção & Áudio:**
  - [[Audio-Footsteps|Emissão e Propagação de Passos 3D]]
  - [[Perception-Hearing|Sensor de Audição de Passos (Player & AI)]]

- **Inteligência Artificial (FSM):**
  - [[AI-FSM-Core|Máquina de Estados Finitos (Core FSM)]]
  - [[AI-State-Wander|Estado: Wander (Patrulha)]]
  - [[AI-State-Pursuit|Estado: Pursuit (Perseguição / Investigação)]]
  - [[AI-State-Attack|Estado: Attack (Combate & Magias)]]
  - [[AI-State-Flee|Estado: Flee (Fuga por Rotas de Circulação)]]
  - [[AI-State-Evade|Estado: Evade (Esquiva Lateral / Strafe)]]
  - [[AI-State-JumpEvade|Estado: JumpToEvade (Salto de Desvio)]]
  - [[AI-State-CallFriends|Estado: CallFriends (Chamado de Aliados)]]

- **Interface & HUD:**
  - [[UI-HUD|HUD Dinâmico, Mira, Cooldowns e Barras de Status]]

---

## 🏃 Organização de Sprints

### 🟢 Sprint 1: Prototipagem do Mapa (Blockmesh & Level Design)
**Objetivo:** Construir a geometria do mapa inspirado em Dust utilizando ProBuilder e aplicar os 4 pilares de Level Design.

- [ ] **Task 1.1:** Setup da cena `HeroShooterDust.unity` e paleta de materiais ProBuilder (Arenito, Concreto, Madeira, Metal).
- [ ] **Task 1.2 ([[LevelDesign-Blockmesh|Blockmesh]]):** Criar piso base, pátio central (*Mid Courtyard*), corredores laterais (*Long A* e *Short B*).
- [ ] **Task 1.3 ([[LevelDesign-Verticality|Verticality]]):** Construir rampas de acesso, escadas e casas elevadas com janelas abertas para sniping/vantage point.
- [ ] **Task 1.4 ([[LevelDesign-Circulation|Circulation]]):** Garantir rotas circulares conectando as lanes e posicionamento de caixotes de cobertura tática.
- [ ] **Task 1.5 ([[Navigation-NavMesh|NavMesh]]):** Configurar `NavMeshSurface` com suporte a rampas e links de salto verticais (*NavMeshLinks*).

---

### 🟢 Sprint 2: Player Controller, Câmera Híbrida & Áudio
**Objetivo:** Implementar movimentação fluida do jogador, alternância de câmera 1ª/3ª pessoa e emissão de passos.

- [ ] **Task 2.1 ([[Player-Movement|PlayerMovement]]):** Criar `PlayerController.cs` com movimentação WASD, corrida (Shift), pulo (Espaço) e amortecimento.
- [ ] **Task 2.2 ([[Player-Camera|CameraManager]]):** Criar `PlayerCameraManager.cs` com alternância suave entre 1ª e 3ª pessoa ao pressionar a tecla **`P`**.
- [ ] **Task 2.3 ([[Player-Camera|ADS]]):** Implementar mira com botão direito (**Right Click**) ajustando FOV e alinhamento de mira em ambos os modos de câmera.
- [ ] **Task 2.4 ([[Audio-Footsteps|FootstepEmission]]):** Criar `NoiseEmitter.cs` que gera ruído sonoro e acústico sincronizado com o ritmo de passos do jogador.

---

### 🟢 Sprint 3: Sistema de Armas & Magias Hero Shooter (1, 2, 3, 4)
**Objetivo:** Criar o arsenal de armas com troca no scroll e as 4 magias heróicas com sistemas de partículas.

- [ ] **Task 3.1 ([[Weapons-System|Weapons]]):** Criar `WeaponManager.cs` e `Weapon.cs` com tiro no **Left Click**, recuo, muzzle flash e impacto.
- [ ] **Task 3.2 ([[Weapons-System|WeaponSwitch]]):** Implementar troca de armas pelo **Scroll do Mouse** (Rifle, Shotgun, Pistola).
- [ ] **Task 3.3 ([[Hero-Skills|Skill 1 - Fireball]]):** Criar magia de Projétil de Energia / Bola de Fogo com explosão de partículas em área.
- [ ] **Task 3.4 ([[Hero-Skills|Skill 2 - Blink/Dash]]):** Criar teleporte rápido na direção do movimento com rastro de partículas e distorção.
- [ ] **Task 3.5 ([[Hero-Skills|Skill 3 - Shield/Heal]]):** Criar cúpula de escudo cinético / pulso de cura com efeito visual esférico.
- [ ] **Task 3.6 ([[Hero-Skills|Skill 4 - Ultimate Meteor]]):** Criar habilidade suprema em área com grande efeito de partículas e impacto destrutivo.

---

### 🟢 Sprint 4: Percepção Acústica & FSM dos Inimigos
**Objetivo:** Criar o cérebro da IA com percepção de passos e todos os 7 estados comportamentais.

- [ ] **Task 4.1 ([[Perception-Hearing|HearingSensor]]):** Criar `HearingSensor.cs` no jogador e inimigos para detectar ruídos emitidos no cenário.
- [ ] **Task 4.2 ([[AI-FSM-Core|FSM Core]]):** Criar arquitetura modular de estados (`IState`, `StateMachine`, `EnemyBrain.cs`).
- [ ] **Task 4.3 ([[AI-State-Wander|Wander & Patrol]]):** Movimento autônomo por waypoints e pontos de interesse do mapa.
- [ ] **Task 4.4 ([[AI-State-Pursuit|Pursuit & Investigate]]):** Perseguição ao avistar o jogador ou ao ouvir passos/tiros.
- [ ] **Task 4.5 ([[AI-State-Attack|Attack]]):** Disparo de projéteis e lançamento de magias com linha de visada.
- [ ] **Task 4.6 ([[AI-State-Evade|Evade & Strafe]]):** Esquiva lateral quando o jogador mira ou atira no inimigo.
- [ ] **Task 4.7 ([[AI-State-JumpEvade|JumpToEvade]]):** Salto evasivo para desviar de magias ou transpor obstáculos.
- [ ] **Task 4.8 ([[AI-State-Flee|Flee]]):** Fuga tática por rotas de circulação quando com pouca vida (< 25%).
- [ ] **Task 4.9 ([[AI-State-CallFriends|CallFriends]]):** Emissão de alerta sonoro que atrai inimigos próximos para engajar no combate.

---

### 🟢 Sprint 5: HUD, Polimento & Integração Geral
**Objetivo:** Integrar interface, áudio espacial e testes de jogabilidade completos na cena.

- [ ] **Task 5.1 ([[UI-HUD|HUD]]):** Montar UI com Mira, Barra de Vida, Indicador de Arma/Munição, Ícones de Cooldown das Magias (1-4) e Modo de Câmera.
- [ ] **Task 5.2:** Spawners de inimigos distribuídos pelo mapa para validar combates táticos e chamados de aliados.
- [ ] **Task 5.3:** Testes de jogabilidade e validação final da FSM e balanço das habilidades.

---

## 📚 Glossário de Level Design do Projeto

- **[[LevelDesign-Blockmesh|Blockmesh]]:** Etapa de prototipagem cinzenta/geométrica onde volumes brutos definem a arquitetura antes da arte final.
- **[[LevelDesign-CriticalPath|Critical Path]]:** As artérias centrais do mapa onde ocorrem os encontros mais frequentes e decisivos entre o jogador e as forças inimigas.
- **[[LevelDesign-Circulation|Circulation]]:** A fluidez com que os atores transitam pelo espaço, garantindo opções de flanco e fuga dinâmica.
- **[[LevelDesign-Verticality|Verticality]]:** O uso intencional do eixo Y (alturas, sacadas, janelas e rampas) para criar vantagens estratégicas de posicionamento e visibilidade.
