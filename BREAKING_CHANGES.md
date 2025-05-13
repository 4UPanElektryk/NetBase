# ⚠️ Breaking Changes: HttpListener Migration

NetBase has recently transitioned from `SimpleTCP` to `HttpListener` to improve compatibility beyond localhost.

To run NetBase outside your local machine, you'll need to configure your system to allow external access. Follow these steps:

---

## 🔧 Step 1: Open the Port in Windows Firewall

Run PowerShell as **Administrator**, and replace `8080` with your actual port number:

```pwsh
$port = 8080

New-NetFirewallRule -DisplayName "Allow Port $port" `
                    -Direction Inbound `
                    -Protocol TCP `
                    -LocalPort $port `
                    -Action Allow
````

---

## 🔑 Step 2: Register the URL

Run Command Prompt as **Administrator**, and replace `{port_here}` with your chosen port:

```cmd
netsh http add urlacl url=http://+:{port_here}/ user=Everyone
```

These steps are required to ensure that `HttpListener` can bind to the selected port and accept incoming connections from external sources.
