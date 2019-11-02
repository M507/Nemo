# Nemo

## What does it do:
- Manipulates local and domain policies
  - Disables Windows Firewall.
  - Disables Windows task manager.
- Deletes Windows Defender AV signature database.
- Deletes Windows Defender.
- Creates a Powershell profile.
  - Drops firewall rules
  - Starts WinRM, RDP, and <> services
  - Creates users.
  - Remaps powershell aliases
- Hides processes, files, and directories that start with $vmware-<whatever>
-- 
- Installs Powershell v2
- TODO.... <continue>
- ...
- ...
- Removes everything from event manager




# List of processes:
## Example:
#### What is it
- Real binary name: KeyChain.exe
- Real binary path: <path>
- Hidden binary name: $vmware-005.exe
- Hidden binary path: <path>
- Description:
  - It does something.

#### Keylogger 
- Real binary name: KeyChain.exe
- Real binary path: <path>
- Hidden binary name: $vmware-005.exe
- Hidden binary path: <path>
- Description:
  - It sends everything back to <IP>:80/bose.php.

#### Callbacks
- Real binary name: PFRE.exe
- Real binary path: C:\Windows\
- Hidden binary name: $vmware-001.exe
- Hidden binary path: C:\ProgramData\Microsoft\Windows\Caches\
- Description:
  - This binary sends back a shell to S-Nemo.py

#### Malware 
- Real binary name: WinHypro.exe
- Real binary path: C:\Windows\
- Hidden binary name: $vmware-002.exe
- Hidden binary path: C:\ProgramData\Microsoft\Windows\Caches\
- Description:
  - Infects every new file in the system according to your needs.
  - Removes for all Sysinternals binaries.

#### Nemo.exe
- Real binary name: Wherever you want
- Real binary path: Wherever you want
- Hidden binary name: Has no hidden process 
- Hidden binary path: Has no hidden process
- Description:
  - This is the implant that downloads, configures, and installs all the needed files.
  - It needs to run once. It's the process that starts everything.

#### Installing Rootkit
- Real binary name: NT.exe
- Real binary path: C:\Program Files\Windows NT\
- Hidden binary name: Has no hidden process since it runs once
- Hidden binary path: Has no hidden process since it runs once
- Description:
  - NT.exe is what loads <name>.dll into <Key>

#### Checker
- Real binary name: WMSys.exe
- Real binary path: C:\Windows\
- Hidden binary name:$vmware-000.exe
- Hidden binary path: C:\ProgramData\Microsoft\Windows\Caches\
- Description:
  - This binary makes sure that Nemo implant is running aka makes sure that Red-Team has a shell.
  
#### Clean.exe
- Real binary name: Wherever you want
- Real binary path: Wherever you want
- Hidden binary name: Has no hidden process 
- Hidden binary path: Has no hidden process
- Description:
  - This binary cleans almost every evidence after deploying Nemo


## Integrated projects:
- https://github.com/M507/M-Botnet
- https://github.com/bytecode77/r77-rootkit
- https://github.com/RITRedteam/WindowsPlague
