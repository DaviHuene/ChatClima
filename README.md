# 🌦️ ChatClima

O **ChatClima** é uma aplicação desenvolvida em WPF (Windows Presentation Foundation) que combina um sistema de **chat TCP** com dados climáticos em tempo real. Inspirado visualmente no WhatsApp, o sistema permite que usuários conversem entre si enquanto acessam informações meteorológicas de diferentes capitais do mundo.

---

## 📸 Telas do Sistema

### 🔐 Tela de Login
![Login Screen 19_05_2025 21_09_26](https://github.com/user-attachments/assets/32559225-5151-4f35-a627-0381fa5ecb17)

Permite que o usuário acesse o sistema com seu apelido.

### 📝 Tela de Cadastro
![Cadastro Screen 19_05_2025 21_09_12](https://github.com/user-attachments/assets/1750e28b-cd10-4e57-9c89-95c215189b78)

Permite registrar um novo apelido para utilizar no chat.
![Digite seu apelido_ 19_05_2025 21_10_34](https://github.com/user-attachments/assets/b51c972e-0776-474a-a0d3-4d0be8047dec)

### 💬 Tela do Cliente
![ClientScreen 19_05_2025 21_09_43](https://github.com/user-attachments/assets/75b713fb-1395-41e3-afcb-c3f920624e34)

Interface principal do usuário com design inspirado no WhatsApp:

* Lista de mensagens
* Lista de usuários conectados
* Campo de digitação
* Botão "+" para escolher países

### 🌐 Tela de Clima
![Clima 19_05_2025 21_09_54](https://github.com/user-attachments/assets/8a908e10-901e-49e6-9637-415976cb6c66)

Mostra o clima atual de capitais selecionadas:

* Nome do país e capital
* Temperatura atual
* Umidade e clima
* Ícone correspondente

### 🖥️ Tela do Servidor
![Server Screen 19_05_2025 21_09_37](https://github.com/user-attachments/assets/e4f7e73d-c354-466c-99c9-366eda9d5e96)

Permite ao administrador iniciar o servidor e acompanhar os usuários conectados.

---

## ⚙️ Tecnologias Utilizadas

* C# com WPF (.NET)
* TCP/IP (Sockets)
* MongoDB (armazenamento dos países e dados climáticos)
* Estilo visual inspirado no WhatsApp
* Integração com API de clima (ex: OpenWeather ou similar)
* Git/GitHub para controle de versão

---

## 📁 Estrutura do Projeto

```
ChatClima/
├── Client/            # Cliente WPF
├── Server/            # Servidor TCP
├── Models/            # Modelos como Clima, Usuario
├── Assets/            # Ícones e imagens
├── App.config
├── README.md
└── .gitignore
```

---

## 🚀 Como Executar

1. Clone o repositório:

   ```bash
   git clone https://github.com/DaviHuene/ChatClima.git
   cd ChatClima
   ```

2. Abra a solução no Visual Studio.

3. Execute o projeto `Server` primeiro (inicia o servidor).

4. Execute o projeto `Client` e entre com seu apelido ou cadastre um novo.

5. Clique no botão `+` para abrir os países e acessar os dados climáticos.

---

## ✅ Funcionalidades

* [x] Cadastro e login de usuários
* [x] Comunicação via TCP
* [x] Exibição de clima por capital
* [x] Seleção de países com dados climáticos
* [x] Interface moderna e responsiva

---

## 👨‍💻 Desenvolvedor

**Davi Huene**
Estudante de Ciência da Computação - FIAP
Contato: \[Seu email ou LinkedIn, se quiser colocar]

---

## 📜 Licença

Este projeto está sob a licença MIT.
