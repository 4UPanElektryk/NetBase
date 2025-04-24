![NetBase](https://github.com/4UPanElektryk/NetBase/assets/80508394/c341fbed-818b-407f-940e-b246c5c6a6da)
# NetBase
## NetBase is a simple framework for developing websites

## Found a bug?
Help us fix it!

[Open a new issue](https://github.com/4UPanElektryk/NetBase/issues/new) with a description of your problem.

Remember to include screenshots!

### Caution!
Due to a recent refactor from SimpleTCP to HttpListener to use them outside of your local machine youll need to run the following comands depending on your port

in powershell as admin
```pwsh
$port = 8080

New-NetFirewallRule -DisplayName "Allow Port $port" `
                    -Direction Inbound `
                    -Protocol TCP `
                    -LocalPort $port `
                    -Action Allow
```

and to allow to connect you'll need also to run cmd as admin

```cmd
netsh http add urlacl url=http://+:{port_here}/ user=Everyone
```
