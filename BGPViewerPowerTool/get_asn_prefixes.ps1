Import-Module .\api_module.psm1
PrintInitialMessage
Write-Host ""
# Read ASN from input
$asn = $args[0]
# Call the API
$model = RetrieveJsonFrom("https://api.bgpview.io/asn/$($asn)/prefixes")
# Extract from JSON
Write-Host "=== ASN$($asn) Known Prefixes ==="
Write-Host ""
foreach ($address in $model.data.ipv4_prefixes) {
    Write-Host $address.prefix
}
Write-Host ""