# Chat Application with Private Messaging

## Table of Contents
1. [Project Overview](#project-overview)
2. [Key Features](#key-features)
3. [Requirements](#requirements)
4. [Getting Started](#getting-started)
5. [Code Structure](#code-structure)
6. [Communication Protocol](#communication-protocol)
7. [Future Enhancements](#future-enhancements)
8. [Contributing](#contributing)
9. [License](#license)

## Project Overview
This project is a network chat application consisting of two main components:
- **Server**: Manages connections and distributes messages between clients
- **Client**: User interface for communicating with the server and other users

## Key Features
- **Multi-user simultaneous connections**
- **Public messaging** to all users
- **Private messaging** between specific users
- **Online users list** display
- **User-friendly interface**
- **Message logging** on the server

## Requirements
- Windows OS
- .NET Framework 4.7.2 or higher
- Visual Studio 2019 or newer (for development)

## Getting Started

### Server Setup
1. Run `SocketPrograming_ServerWpf.exe`
2. Enter desired IP address and port
3. Click "Start Server" button

### Client Setup
1. Run `SocketPrograming_ClientWpf.exe`
2. Enter server IP address and port
3. Enter your username
4. Click "Connect" button

## Code Structure

### Server
- **MainWindow.xaml.cs**: Main server class
  - `AcceptClients()`: Handles new connections
  - `HandleClient()`: Manages client communication
  - `BroadcastMessage()`: Sends messages to all users
  - `SendPrivateMessage()`: Handles private messaging

- **ClientInfo.cs**: Client information container class

### Client
- **MainWindow.xaml.cs**: Main client class
  - `ReceiveData()`: Receives messages from server
  - `BtnSend_Click()`: Sends public messages
  - `BtnPrivate_Click()`: Initiates private messages
  - `Disconnect()`: Terminates connection

## Communication Protocol
Messages are sent in UTF-8 format with the following structures:

1. **Public Message**:
   ```
   message text
   ```

2. **Private Message**:
   ```
   /private recipient_name message_text
   ```

3. **System Messages**:
   - New user: `New user joined: username`
   - User left: `User left the chat: username`
   - Server shutdown: `Server is shutting down...`

## Future Enhancements
- File transfer capability
- Message encryption
- Chat history storage
- Multiple chat rooms support
- Emoji and text formatting support
