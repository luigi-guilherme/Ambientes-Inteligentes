# 🌱 Sistema de Supervisão Ambiental Inteligente com IoT e FIWARE

> Projeto acadêmico desenvolvido na **Faculdade Engenheiro Salvador Arena**
> **Curso:** Engenharia da Computação – 5º semestre
> **Tipo:** Projeto em grupo – Trabalho interdisciplinar

---

## 📌 Visão Geral

Este projeto consiste em um **sistema de supervisão ambiental inteligente**, baseado em **Internet das Coisas (IoT)**, capaz de monitorar em tempo real variáveis ambientais como **temperatura, umidade e luminosidade**.

A solução integra um **dispositivo de borda (ESP32)** com a plataforma **FIWARE**, permitindo não apenas o monitoramento, mas também a **atuação automática** por meio de alertas visuais e sonoros sempre que limites configurados são ultrapassados.

O sistema foi desenvolvido como parte do trabalho interdisciplinar das disciplinas:

* **Linguagem de Programação I** – Prof. Eduardo Rosalem
* **Sistemas Embarcados** – Prof. Fábio Cabrini

---

## 🧠 Arquitetura da Solução

A arquitetura segue o modelo clássico de **três camadas**:

### 1️⃣ Camada de Borda – Dispositivo IoT

* **Microcontrolador:** ESP32
* **Sensores:**

  * DHT11 (Temperatura e Umidade)
  * LDR (Luminosidade)
* **Atuadores:**

  * LED Verde (Normal)
  * LED Amarelo (Atenção)
  * LED Vermelho (Crítico)
  * Buzzer (Alerta sonoro)
* **Comunicação:** MQTT via Wi-Fi

### 2️⃣ Camada de Plataforma – Middleware FIWARE

* **IoT Agent (MQTT):** Tradução MQTT → NGSI
* **Orion Context Broker:** Gerenciamento do estado atual (tempo real)
* **STH-Comet + MongoDB:** Armazenamento do histórico de dados
* **Containerização:** Docker

### 3️⃣ Camada de Aplicação – Web

* Interface web para **monitoramento, análise e gestão** do sistema
* Exibição de dados em tempo real e históricos
* Configuração de limites e regras de alerta

---

## 🔄 Fluxo de Dados

### 📡 Do Sensor até a Tela do Usuário

```
ESP32 → MQTT → IoT Agent → Orion Context Broker → STH-Comet → Aplicação Web
```

### 🚨 Do Usuário ao Atuador Físico

```
Aplicação Web → Orion → IoT Agent → MQTT → ESP32 → LED/Buzzer
```

---

## ⚙️ Funcionalidades Principais

### 📊 Monitoramento em Tempo Real

* Visualização instantânea dos dados ambientais
* Gráficos do tipo *Gauge*
* Atualização automática via **Ajax**

### 📈 Análise Histórica

* Consulta de dados armazenados no **STH-Comet**
* Filtros por dispositivo e quantidade de registros
* Gráficos temporais e tabelas detalhadas

### 🚨 Sistema de Alertas Automáticos (Triggers)

* Definição de limites mínimos e máximos por ambiente
* Detecção automática de valores fora do padrão
* Acionamento físico de LEDs e buzzer no ESP32

### 🗂️ Gestão de Cadastros (CRUD)

* **Locais:** Ambientes monitorados e seus limites
* **Dispositivos:** ESP32 vinculados ao FIWARE
* **Usuários:** Controle básico de acesso
---

## 🧰 Tecnologias Utilizadas

### Backend

* **C# / .NET 8**
* **ASP.NET Core MVC**
* **SQL Server**
* **Padrão DAO com herança**
* **Injeção de Dependência**

### Frontend

* **HTML5 / CSS3 (Bootstrap)**
* **JavaScript / jQuery**
* **Chart.js**
* **Ajax**

### IoT & Middleware

* **ESP32** (simulado no Wokwi)
* **MQTT**
* **FIWARE** (Orion, IoT Agent, STH-Comet)
* **Docker**

---

## 🧩 Destaques de Implementação

* **DAO Genérico:** Classe base que abstrai operações CRUD via Stored Procedures
* **Integração Transparente:** Dados de sensores não são armazenados no SQL Server, mas consultados diretamente no FIWARE
* **Separação de Responsabilidades:** Controllers, Services, DAOs e Models bem definidos

---

## 👨‍💻 Integrantes do Grupo

* **Guilherme de Oliveira Mattos** – RA: 082230009
* **Luigi Guilherme Pereira Silva** – RA: 082230025
* **Paulo Henrique de Carvalho Santos** – RA: 082230006
* **Pedro Henrique de Holanda Carvalho** – RA: 082230005
* **Tayson Moisés Costa do Carmo** – RA: 082230008

---

## 🎓 Instituição

**Faculdade Engenheiro Salvador Arena**
Curso de **Engenharia da Computação**

---

## 📄 Licença

Projeto desenvolvido exclusivamente para fins **acadêmicos e educacionais**.
