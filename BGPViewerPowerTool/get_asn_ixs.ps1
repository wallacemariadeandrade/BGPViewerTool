Import-Module .\api_module.psm1
PrintInitialMessage
Write-Host ""
# Read ASN from input
$asn = $args[0]
# Call the API
$model = RetrieveJsonFrom("https://api.bgpview.io/asn/$($asn)/ixs")
Write-Host "=== ASN$($asn) IPv4 Known IXs ==="
Write-Host ""
foreach ($ix in $model.data) {
    Write-Host "Name: $($ix.name)"
    Write-Host "Full Name: $($ix.name_full)"
    Write-Host "Country Code: $($ix.contry_code)"
    Write-Host "IPv4 Address: $($ix.ipv4_address)"
    Write-Host "IPv6 Address: $($ix.ipv6_address)"
    Write-Host "Speed: $($ix.speed)"
    Write-Host ""
}